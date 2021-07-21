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

namespace coreERP.gl.reports
{
    public partial class chart_of_accounts : corePage
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
            get { return "~/gl/reports/chart_of_accounts.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        { 
            rpt = new coreReports.gl.chart_of_accounts.chartOfAccountsReport();
            rpt.SetDataSource((new reportEntities()).chart_of_accounts);
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            this.chart_of_accounts_report.ReportSource = rpt;
            this.chart_of_accounts_report.DataBind();
        }
    }
}
