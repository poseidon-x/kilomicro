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

namespace coreERP.ln.reports3
{
    public partial class cashier : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports3/cashier.aspx"; }
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

                coreLoansEntities le = new coreLoansEntities();
                coreSecurityEntities sec = new coreSecurityEntities();
                string categoryID = "";
                protected void Page_Load(object sender, EventArgs e)
                {
                    categoryID = Request.Params["catID"];
                    if (categoryID == null) categoryID = "";
                    if (!IsPostBack)
                    {
                        Session["endDate"] = null; 

                        cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                        foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                        {
                            cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                        }

                        cboCashier.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", "")); 
                        foreach (var r in sec.users.OrderBy(p => p.full_name))
                        {
                            if (le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == r.user_name.ToLower()) != null)
                            {
                                cboCashier.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.full_name + " (" + r.user_name + ")", r.user_name));
                            }
                        }
                    }
                }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            var endDate = dtpEndDate.SelectedDate.Value;
            Session["endDate"] = endDate;
             
            Bind(endDate, null); 
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                var endDate = (DateTime)Session["endDate"];
                Bind(endDate, null);
            }
        }

        private void Bind(DateTime? endDate, int? clientID)
        {
            var le = new coreLoansEntities();
            var rent = new reportEntities();
             
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            string cashierUser = null;
            if (cboCashier.SelectedValue != "")
            {
                cashierUser = cboCashier.SelectedValue;
            } 
            var cl = le.clients.Where(p =>  (branchID == null || p.branchID == branchID)).ToList();
            rpt = new coreReports.ln3.rptCashierDetail();
            var res = rent.vwCashierDetails.Where(
                    p=> (cashierUser==null || p.userName==cashierUser)
                        && (p.txDate<= dtpEndDate.SelectedDate && p.txDate>= dtpStartDate.SelectedDate)
                ).ToList();
            for (int i = res.Count - 1; i >= 0; i--)
            {
                if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                {
                    res.Remove(res[i]);
                }
            } 
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            rpt.SetDataSource(res);  
            rpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
            var branchName = "";
            if (cboBranch.Text != "")
            {
                branchName = "BRANCH: " + cboBranch.Text;
            }
            try
            {
                rpt.SetParameterValue("reportTitle", "Cashier Transaction Details b/n "+dtpStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                    + " & " + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy")); 
                rpt.SetParameterValue("branch", branchName);
            }
            catch (Exception) { }
            this.rvw.ReportSource = rpt;
        } 
    }
}
