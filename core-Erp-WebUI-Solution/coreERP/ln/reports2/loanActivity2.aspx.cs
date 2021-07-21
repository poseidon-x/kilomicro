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
    public partial class loanActivity2 : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports2/loanActivity2.aspx"; }
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
                var categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.Now.Day, 23, 59, 59)); 
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpEndDate.SelectedDate.Value.AddYears(-10);
            Session["resAct"] = null;
            Session["resAct2"] = null;
            Bind(dtpEndDate.SelectedDate.Value.AddYears(-10), dtpEndDate.SelectedDate.Value);
            //this.rvw.DataBind();

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
            var le = new coreLoansEntities(); 
            rpt = new coreReports.ln2.rptLoanActivity();
            if (Session["resAct"] != null && Session["resAct2"] != null)
            {
                var res = Session["resAct"] as List<getLoanActivity_Result>;
                var res2 = Session["resAct2"] as List<getLoanActivity2_Result>;
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource(res2);
            }
            else
            { 
                var res = (new reportEntities()).getLoanActivity(startDate, endDate).ToList();
                 
                var res2 = (new reportEntities()).getLoanActivity2(startDate, endDate).ToList();
                Session["resAct"] = res;
                Session["resAct2"] = res2;
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
                rpt.Subreports[0].SetDataSource(res2); 
            }
            rpt.Subreports[1].SetDataSource((new reportEntities()).vwCompProfs.ToList()); 
            rpt.SetParameterValue("title", "Payroll Loan Activity Report as at " + endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
    }
}
