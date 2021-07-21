using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using coreReports;
using coreLogic;
using Telerik.Web.UI;

namespace coreERP.gl.reports.fin_stmt
{
    public partial class bal_sht_ct : corePage
    {
        ReportDocument rpt;

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rpt != null)
            {
                try
                {
                    rpt.Dispose();
                    rpt.Close();
                    rpt = null;
                }
                catch (Exception) { }
            }
        }

        public override string URL
        {
            get { return "~/gl/reports/fin_stmt/bal_sht_ct.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, 1, 1)).AddYears(-4);

                PopulateCostCenters();
            }
        }

        protected void PopulateCostCenters()
        {
            try
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                var cc = (from a in ent.vw_gl_ou
                          select a);
                cboCC.DataSource = cc.ToList();
                cboCC.DataBind();
                cboCC.Items.Insert(0, new RadComboBoxItem("Cost Center", null));
            }
            catch (Exception ex) { }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["notx"] = chkNoTx.Checked;
            Session["sum"] = chkSum.Checked;
            Session["resb_ct"] = null;
            Bind(dtpEndDate.SelectedDate.Value, chkNoTx.Checked, chkSum.Checked);
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                Bind(endDate, (bool)Session["notx"], (bool)Session["sum"]);
            }
        }

        private void Bind(DateTime endDate, bool noTx, bool sum)
        {
            int? ccID = null;
            if (cboCC.SelectedValue != "")
            {
                ccID = int.Parse(cboCC.SelectedValue);
            }
            List<accountBalance> res=null;
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 300000;
            if (sum == false)
            {
                rpt = new coreReports.gl.fin_stmt_2.rptBSCT();
                if (Session["resb_ct"] != null)
                {
                    res = Session["resb_ct"] as List<accountBalance>;
                }
                else
                {
                    res = (new ReportHelper()).getFiveYearBalanceSheet(dtpStartDate.SelectedDate.Value,
                        dtpEndDate.SelectedDate.Value, ccID, noTx);
                    Session["resb_ct"] = res;
                }
            }
            else
            {
                rpt = new coreReports.gl.fin_stmt_2.rptBSCT();
                if (Session["resb_ct"] != null)
                {
                    res = Session["resb_ct"] as List<accountBalance>;
                }
                else
                {
                    res = (new ReportHelper()).getFiveYearBalanceSheetSummary(dtpStartDate.SelectedDate.Value,
                        dtpEndDate.SelectedDate.Value, ccID, noTx);
                    Session["resb_ct"] = res;
                }
            }
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList()); 
            rpt.SetParameterValue("reportTitle", "Balance Sheet");
            this.rvw.ReportSource = rpt;
        }

    }
}
