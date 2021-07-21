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

namespace coreERP.ln.reports
{
    public partial class controllerIn : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports/controllerIn.aspx"; }
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpEndDate.SelectedDate = DateTime.Now.Date.AddSeconds(-1);
            }
        }
 
        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["startDate"] != null)
            {
                var startDate = (DateTime)Session["startDate"];
                var endDate = (DateTime)Session["endDate"];
                Bind(startDate, endDate);
            }
        }

        private void Bind(DateTime startDate, DateTime endDate)
        {
            rpt = new coreReports.ln.rptControllerIn();
             
            var res = (new reportEntities()).vwControllerIns.Where(p=>p.disbursementDate<=endDate && p.disbursementDate >= startDate).ToList();
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());  
            this.rvw.ReportSource = rpt;
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["startDate"] = dtpStartDate.SelectedDate.Value;
            Session["endDate"] = dtpEndDate.SelectedDate.Value;
            Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value);
        }
    }
}
