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

namespace coreERP.ln.reports4
{
    public partial class susuContributions : corePage
    {          
        public override string URL
        {
            get { return "~/ln/reports4/susuContributions.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = DateTime.Now;
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).AddDays(-1);
            }
        }
 
        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate; 
            Bind(dtpEndDate.SelectedDate.Value, dtpStartDate.SelectedDate.Value); 
        }

        private void Bind(DateTime endDate, DateTime startDate)
        { 
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 300000;
            Telerik.Reporting.Report rpt = null;
            //rpt = new coreReports.telerik.susu.susuContribution();
            //var res = rent.vwSusuContributionReports.Where(p => p.contributionDate <= endDate && p.contributionDate >= startDate).ToList();

            var objectDataSource = new Telerik.Reporting.ObjectDataSource();
            //objectDataSource.DataSource = res; // GetData returns a DataSet with three tables 

            //rpt.DataSource = res;
            //try
            //{
            //    (rpt.Items["table1"] as Telerik.Reporting.Table).DataSource = res;
            //}
            //catch (Exception) { }

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
