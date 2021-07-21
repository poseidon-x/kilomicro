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

namespace coreERP.gl.reports.fin_stmt2
{
    public partial class op_stmt_ct : corePage
    {          
        public override string URL
        {
            get { return "~/gl/reports/fin_stmt2/op_stmt_ct.aspx"; }
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
            Session["reso_ct"] = null;
            coreReports.periodType periodType = (coreReports.periodType)uint.Parse(cboPeriodType.SelectedValue);
            Bind(dtpEndDate.SelectedDate.Value, chkNoTx.Checked, chkSum.Checked,
                periodType); 
        }

        private void Bind(DateTime endDate, bool noTx, bool sum, coreReports.periodType periodType)
        {
            int? ccID = null;
            if (cboCC.SelectedValue != "")
            {
                ccID = int.Parse(cboCC.SelectedValue);
            }
            List<accountAmountTab> res=null;
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 300000;
            Telerik.Reporting.Report rpt = null;
            if (periodType == coreReports.periodType.Yearly)
            {
                rpt = new coreReports.telerik.gl.fin_stmt.opStmtYearly();
            }
            else if (periodType == coreReports.periodType.Monthly)
            {
                rpt = new coreReports.telerik.gl.fin_stmt.opStmtMonthly();
            }
            else if (periodType == coreReports.periodType.Weekly)
            {
                rpt = new coreReports.telerik.gl.fin_stmt.opStmtWeekly();
            }
            else if (periodType == coreReports.periodType.Quarterly)
            {
                rpt = new coreReports.telerik.gl.fin_stmt.opStmtQuarterly();
            }
            else if (periodType == coreReports.periodType.Half_Yearly)
            {
                rpt = new coreReports.telerik.gl.fin_stmt.opStmtHalfYearly();
            }
            else if (periodType == coreReports.periodType.Daily)
            {
                rpt = new coreReports.telerik.gl.fin_stmt.opStmtDaily();
            }
            if (Session["reso_ct"] != null)
            {
                res = Session["reso_ct"] as List<accountAmountTab>;
            }
            else
            {
                res = (new ReportHelper()).getOperatingStatement(dtpStartDate.SelectedDate.Value,
                   dtpEndDate.SelectedDate.Value, ccID, noTx, periodType);
                Session["reso_ct"] = res;
            }
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            (rpt.Items[0].Items["crosstab1"] as Telerik.Reporting.Crosstab).DataSource = res;

            // Use the InstanceReportSource to pass the report to the viewer for displaying
            Telerik.Reporting.InstanceReportSource reportSource = new Telerik.Reporting.InstanceReportSource();
            reportSource.ReportDocument = rpt;

            // Assigning the report to the report viewer.
            rvw.ReportSource = reportSource;

            // Calling the RefreshReport method (only in WinForms applications).
            rvw.RefreshReport();
        }
    }
}
