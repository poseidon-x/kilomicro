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
    public partial class trial_bal_std : corePage
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
            get { return "~/gl/reports/trial_bal_std.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
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
            Session["res"] = null;
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
            List<vw_acc_bals> res;
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 300000;
            if (sum == false)
            {
                rpt = new coreReports.gl.fin_stmt.rptTB();
                if (Session["res"] != null)
                {
                    res = Session["res"] as List<vw_acc_bals>;
                }
                else
                {
                    res = rent.get_acc_bals(DateTime.Now.AddYears(-50), endDate, noTx, false, ccID).ToList<vw_acc_bals>();
                    Session["res"] = res;
                }
            }
            else
            {
                rpt = new coreReports.gl.fin_stmt.rptTBSum();
                if (Session["res"] != null)
                {
                    res = Session["res"] as List<vw_acc_bals>;
                }
                else
                {
                    res = rent.get_acc_bals_sum(dtpStartDate.SelectedDate, endDate, noTx, false, sum, ccID).ToList<vw_acc_bals>();
                    Session["res"] = res;
                }
            }
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            if(dtpStartDate.SelectedDate.Value.Year==DateTime.Now.Year && dtpStartDate.SelectedDate.Value.Month==1 && dtpStartDate.SelectedDate.Value.Day==1)
                rpt.SetParameterValue("reportTitle", "Trial Balance at at " +endDate.ToString("dd-MMM-yyyy"));
            else
                rpt.SetParameterValue("reportTitle", "Trial Balance b/n " + dtpStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy") +
                    " & " + endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }

        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (dtpEndDate.SelectedDate.Value.CanClose())
            {
                divError.Style["visibility"] = "hidden";
                spanError.InnerHtml = "";
                btnRun.Enabled = true;
            }
            else
            {
                divError.Style["visibility"] = "visible";
                spanError.InnerHtml = "The selected date belongs to a closed period. Please change.";
                btnRun.Enabled = false;
            }
        }

    }
}
