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
    public partial class loanBalances : corePage
    {
        ReportDocument rpt;
        string categoryID = "";
        public override string URL
        {
            get { return "~/ln/reports/loanBalances.aspx"; }
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
        protected void Page_Load(object sender, EventArgs e)
        {
                categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            {
                Session["filter"] = 1;
                if (categoryID == null) categoryID = "";
                dtpEndDate.SelectedDate = DateTime.Now.Date;
                dtpStartDate.SelectedDate = new DateTime(2000, 1, 1);
                Session["endDate"] = null;

                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.surName + ", " + cl.otherNames + " - " + cl.staffNo, 
                        cl.staffID.ToString()));
                }

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
                Session["detail"] = "1";
                Session["expiry"] = "1";
                Session["status"] = "1";
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            int? staffID = null;
            Session["endDate"] = dtpEndDate.SelectedDate.Value;
            var type = 2;
            Session["resLoanBalances"] = null;
            if(opt1.Checked==true)
            {
                type = 1;
            }
            if (cboStaff.SelectedValue != "")
            {
                staffID = int.Parse(cboStaff.SelectedValue);
                Session["staffID"] = staffID;
            }

            Session["type"] = type;
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(clientID, dtpEndDate.SelectedDate.Value, type, staffID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(null, dtpEndDate.SelectedDate.Value, type, staffID);
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            int? staffID = null;
            if (Session["staffID"] != null)
            {
                staffID = int.Parse(Session["staffID"].ToString()); 
            }
            if (Session["endDate"] != null)
            {
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(clientID, (DateTime)Session["endDate"], (int)Session["type"], staffID);
                }
                else
                {
                    Bind(null, (DateTime)Session["endDate"], (int)Session["type"], staffID);
                }
            }
        }

        private void Bind(int? clientId, DateTime endDate,int type, int? staffId)
        {
            var le = new coreLoansEntities();
            var categoryId = Request.Params["catID"];
            string detail = Session["detail"].ToString();
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 60000;

            bool? all = null;
            if (detail == "1")
            {
                all = true;
            }
            else if (detail == "2")
            {
                all = false;
            }
            int expiry = int.Parse(Session["expiry"].ToString());
            string status = Session["status"].ToString();
            int? branchId = null;
            if (cboBranch.SelectedValue != "") branchId = int.Parse(cboBranch.SelectedValue);
            if (categoryId == null) categoryId = "";
            var cl = le.clients.Where(p => ((categoryId != "5" && p.categoryID != 5) || (categoryId == "5" && p.categoryID == 5))
                && (branchId == null || p.branchID == branchId)).ToList();
            if (type == 1)
            {
                rpt = new coreReports.ln.rptLoanBalance();
            }
            else
            {
                rpt = new coreReports.ln.rptLoanBalance2();
            }
            List<coreReports.getLoanBalanceReport_Result> res = null;
            if (Session["resLoanBalances"] != null)
            {
                res = Session["resLoanBalances"] as List<coreReports.getLoanBalanceReport_Result>;
            }
            else
            {
                res = (rent.getLoanBalanceReport(endDate)
                    .Where(p => p.date >= dtpStartDate.SelectedDate)
                    .Where(p => clientId == null || p.clientID == clientId)
                    .Where(p => staffId == null || p.staffID == staffId)
                    .Where(
                        p =>
                            expiry == 1
                            || (expiry == 2 && p.expiryDate <= dtpEndDate.SelectedDate)
                            || (expiry == 3 && p.expiryDate > dtpEndDate.SelectedDate))
                    .Where(
                        p =>
                            (status == "1" && 10< p.payable)
                            ||(status == "2" && p.payable - p.paid <= 1)
                            || (status == "3" && p.paid > 1 && p.payable > p.paid + 1)
                            || (status == "4" && p.paid < 1)
                            || (status == "5" && (p.payable-p.paid>10)))
                    .Where(
                        p =>
                            all == true 
                            || (all == false && p.cumPayable >= p.cumPaid + 10) 
                            || (all == null && p.cumPaid + 5 > p.cumPayable))
                    .ToList()
                    );
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox(
                        "Report cannot be displaid because no data was found for the provided criteria",
                        "coreERP©: No Data", IconType.deny);
                    return;
                }
                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                 
                Session["resLoanBalances"] = res;
            }
            var sumRpt = rpt.Subreports[0];
            rpt.SetDataSource(res);
            rpt.Subreports[1].SetDataSource(rent.vwCompProfs.ToList());
            sumRpt.Database.Tables[0].SetDataSource(res);
            sumRpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Summary Loan Statement as at " + endDate.ToString("dd-MMM-yyyy"));
            //sumRpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Summary Loan Statement as at " + endDate.ToString("dd-MMM-yyyy"),sumRpt.Name);
            var branchName = "";
            if (cboBranch.Text != "")
            {
                branchName = "BRANCH: " + cboBranch.Text;
            }
            try
            {
                rpt.SetParameterValue("branch", branchName);
                rpt.SetParameterValue("branch", branchName, sumRpt.Name);
            }
            catch (Exception) { }
            if (staffId != null)
            {
                rpt.SetParameterValue("staff", "Assigned to: " + cboStaff.SelectedItem.Text);
                rpt.SetParameterValue("staff", "Assigned to: " + cboStaff.SelectedItem.Text,sumRpt.Name);
            }
            else
            {
                rpt.SetParameterValue("staff", "");
                rpt.SetParameterValue("staff", "", sumRpt.Name);
            }
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }
        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                    && (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }
 
        protected void rbDetail_CheckedChanged(object sender, EventArgs e)
        {
            Session["detail"] = "1";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbSummary_CheckedChanged(object sender, EventArgs e)
        {
            Session["detail"] = "2";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbCompleted_CheckedChanged(object sender, EventArgs e)
        {
            Session["detail"] = "3";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbExpired_CheckedChanged(object sender, EventArgs e)
        {
            Session["expiry"] = "2";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbRunning_CheckedChanged(object sender, EventArgs e)
        {
            Session["expiry"] = "3";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            Session["expiry"] = "1";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbAll2_CheckedChanged(object sender, EventArgs e)
        {
            Session["status"] = "1";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbFull_CheckedChanged(object sender, EventArgs e)
        {
            Session["status"] = "2";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbPartial_CheckedChanged(object sender, EventArgs e)
        {
            Session["status"] = "3";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            Session["status"] = "4";
            btnRun_Click(btnRun, new EventArgs());
        }

        protected void rbUnpaid_CheckedChanged(object sender, EventArgs e)
        {
            Session["status"] = "5";
            btnRun_Click(btnRun, new EventArgs());
        }

    }
}
