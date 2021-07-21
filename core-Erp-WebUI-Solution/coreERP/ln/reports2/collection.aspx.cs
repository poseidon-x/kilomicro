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

namespace coreERP.ln.reports2
{
    public partial class collection : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports2/collection.aspx"; }
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
                rbGrouped.Checked = true;
                var categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, 1,
                    1, 0, 0, 0));
            }
            if (Session["selectedID"] == rbDetailed.ID)
            {
                rbDetailed.Checked = true;
            }
            else if (Session["selectedID"] == rbGrouped.ID)
            {
                rbGrouped.Checked = true;
            } 
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            Session["resAct"] = null;
            Session["resAct2"] = null;
            Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value);
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
            if (rbGrouped.Checked == true)
            {
                rpt = new coreReports.ln2.rptCollection2();
            }
            else if (rbDetailed.Checked == true)
            {
                rpt = new coreReports.ln2.rptCollection();
            }
            if (Session["resAct"] != null)
            {
                var res = Session["resAct"] as List<getCollectionRatio_Result>;
                rpt.SetDataSource(res);
            }
            else
            {
                var res = (new reportEntities()).getCollectionRatio(startDate, endDate).ToList();

                Session["resAct"] = res;
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                rpt.SetDataSource(res);
            }
            rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList()); 
            rpt.SetParameterValue("title", "Payroll Loan Collection Report b/n " +startDate.ToString("dd-MMM-yyyy") +" and " +endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
        protected void rbDetailed_CheckedChanged(object sender, EventArgs e)
        {
            Session["selectedID"] = (sender as RadioButton).ID;
        }
    }
}
