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

namespace coreERP.ln.bogreports
{
    public partial class mf6 : corePage
    {
                coreLoansEntities le = new coreLoansEntities(); 
        public override string URL
        {
            get { return "~/ln/bogreports/mf6.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.Now.Day, 23, 59, 59)).AddDays(-1); 
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = DateTime.Now.AddYears(-10);   
            Bind(DateTime.Now.AddYears(-10), dtpEndDate.SelectedDate.Value); 
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                DateTime startDate = (DateTime)Session["startDate"]; 
                Bind(startDate, endDate); 
            }
        }

        private void Bind(DateTime startDate, DateTime endDate)
        {
            ReportDocument rpt = new coreReports.ln.bog.rptMF6(); 
            var res = (new reportEntities()).getMF6(endDate).ToList();
            rpt.Database.Tables[1].SetDataSource(res);
            rpt.Database.Tables[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("reportTitle", "REPORT ON DEPOSITS EXCEEDING 5% of PAID UP CAPITAL");
            rpt.SetParameterValue("period", "PERIOD OF REPORT:  AS OF " + endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }

    }
}
