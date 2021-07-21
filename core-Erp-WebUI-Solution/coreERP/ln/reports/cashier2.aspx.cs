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
using coreERP.code;

namespace coreERP.ln.reports
{
    public partial class cashier2 : Page
    {
        ReportDocument rpt;
        public string URL
        {
            get { return "~/ln/reports/cashier.aspx"; }
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
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 0, 0, 0));
                coreLoansEntities le = new coreLoansEntities();
                coreSecurityEntities sec = new coreSecurityEntities();
                cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in sec.users.OrderBy(p => p.full_name))
                {
                    if (le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == r.user_name.ToLower()) != null)
                    {
                        cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.full_name + " (" + r.user_name + ")", r.user_name));
                    }
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            Session["userName"] = cboUserName.SelectedValue;
            Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, cboUserName.SelectedValue); 
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                DateTime startDate = (DateTime)Session["startDate"];
                string userName = Session["userName"].ToString();
                Bind(startDate, endDate, userName); 
            }
        }

        private void Bind(DateTime startDate, DateTime endDate, string userName)
        {
            rpt = new coreReports.ln.rptCashierBalance();
            var res = new List<getCashierBalance2_Result>();
            if (String.IsNullOrEmpty(userName))
            {
                 res = (new reportEntities()).getCashierBalance(startDate, endDate).ToList();
            }
            else
            {
                res = (new reportEntities()).getCashierBalance(startDate, endDate).Where(p => p.userName.ToLower().Trim() == userName.ToLower().Trim()).ToList();
            }

            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);
            rpt.Subreports[1].SetDataSource(res);
            rpt.Subreports[0].SetDataSource(res);
            rpt.Subreports[2].SetDataSource((new reportEntities()).vwCompProfs.ToList());

            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Summary Cashier Report between "+startDate.ToString("dd-MMM-yyyy")
                +" and " +endDate.ToString("dd-MMM-yyyy"));
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
    }
}
