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
using coreERP.code;

namespace coreERP.gl.reports
{
    public partial class bal_sht_closed : corePage
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
            get { return "~/gl/reports/bal_sht_closed.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                PopulatePeriods();

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
            var date = DateTime.Parse(cboPeriod.SelectedValue);
            var period = int.Parse(cboPeriod.Text);
            Session["period"] = period;
            Session["endDate"] = date;
            Session["notx"] = chkNoTx.Checked;
            Bind(date, period, chkNoTx.Checked);
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                var period = (int)Session["period"];
                Bind(endDate, period, (bool)Session["notx"]);
            }
        }

        private void Bind(DateTime endDate, int period, bool noTx)
        {
            int? ccID = null;
            if (cboCC.SelectedValue != "")
            {
                ccID = int.Parse(cboCC.SelectedValue);
            }
            rpt = new coreReports.gl.fin_stmt.rptBS();
            var res = (new reportEntities()).get_bal_sht(DateTime.Now.AddYears(-50), endDate, noTx, ccID).ToList<vw_acc_bals>();
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Balance Sheet as at " + endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }

        protected void PopulatePeriods()
        {
            try
            {
                core_dbEntities ent = new core_dbEntities();
                cboPeriod.DataSource = ent.acct_period.ToList();
                cboPeriod.DataBind();
                cboPeriod.Items.Insert(0, new RadComboBoxItem("Select Closed Period", null));
            }
            catch (Exception ex) { }
        }

    }
}
