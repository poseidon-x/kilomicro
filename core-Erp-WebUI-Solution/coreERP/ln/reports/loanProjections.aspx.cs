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
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data;
using coreERP.code;

namespace coreERP.ln.reports
{
    public partial class loanProjections : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports/loanProjections.aspx"; }
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
                string categoryID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                if (categoryID == null) categoryID = "";
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 0, 0, 0));
                if (dtpStartDate.SelectedDate.Value < new DateTime(2011, 6, 1))
                {
                    dtpStartDate.SelectedDate = new DateTime(2011, 6, 1);
                } 
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
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            Session["resLoanBalances"] = null;
            int? clientID = null;
            int? staffID = null;
            if (cboClient.SelectedValue != "")
            {
                clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
            }
            else
            {
                Session["ClientID"] = null; 
            }
            if (cboStaff.SelectedValue != "")
            {
                staffID = int.Parse(cboStaff.SelectedValue);
                Session["staffID"] = staffID; 
            }
            else
            {
                Session["staffID"] = null; 
            } 
                Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, clientID, staffID);
             
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int? clientID = null;
                int? staffID = null;
                if (cboClient.SelectedValue != "")
                {
                    clientID = int.Parse(cboClient.SelectedValue);
                    Session["ClientID"] = clientID;
                }
                else
                {
                    Session["ClientID"] = null;
                }
                if (cboStaff.SelectedValue != "")
                {
                    staffID = int.Parse(cboStaff.SelectedValue);
                    Session["staffID"] = staffID;
                }
                else
                {
                    Session["staffID"] = null;
                }
                Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, clientID, staffID);
            }
        }

        private void Bind(DateTime startDate, DateTime endDate, int? clientId, int? staffId)
        {
            var le = new coreLoansEntities(); 
            int? branchId = null;
            if (cboBranch.SelectedValue != "") branchId = int.Parse(cboBranch.SelectedValue);
            string detail = Session["detail"].ToString();
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 60000;

            rpt = new coreReports.ln.rptLoans6S(); 
            bool? all = null;
            if (detail == "1")
            {
                all = true;
            }
            else if (detail=="2")
            {
                all = false;
            }
            int expiry = int.Parse(Session["expiry"].ToString());
            string status = Session["status"].ToString();
            var cl = le.clients
                .Where(p => (branchId == null || p.branchID == branchId))
                .ToList();
            var res = rent
                .GetvwLoans6(startDate, endDate, all, expiry)
                .Where(p => (clientId == null || p.clientID == clientId))
                .Where(p => (staffId == null || p.staffID == staffId))
                .Where(p =>
                    (status == "1")
                    || (status == "2" && p.cumPayable <= p.cumPaid + 10)
                    || (status == "3" && p.cumPaid >= 10)
                    || (status == "4" && p.cumPaid < 10)
                    || (status == "5" && p.cumPayable > p.cumPaid + 10)
                )
                .OrderBy(p => p.categoryName)
                .ThenBy(p => p.clientName)
                .ToList();
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
            Session["resLoanBalances"] = res;
            rpt.SetDataSource(res);
            rpt.Subreports[0].Database.Tables[0].SetDataSource(res);
                 
            rpt.Subreports[1].SetDataSource(rent.vwCompProfs.ToList());
            rpt.Subreports[0].Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList());
            rpt.SetParameterValue("title", dtpStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy") + " TO " 
                + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"));
            rpt.SetParameterValue("companyName", Settings.companyName);
            var branchName = "";
            if (cboBranch.Text != "")
            {
                branchName = "BRANCH: " + cboBranch.Text;
            }
            try
            {
                rpt.SetParameterValue("branch", branchName);
                rpt.SetParameterValue("branch", branchName, rpt.Subreports[0].Name);
            }
            catch (Exception) { }

            bool completed = false;
            if (Session["detail"] != null && Session["detail"] != "")
            {
                completed = Session["detail"] == "1";
            }
            rpt.SetParameterValue("completed", completed);

            if (cboStaff.SelectedValue != "")
            {
                rpt.SetParameterValue("staffTitle", "Assigned to: " + cboStaff.SelectedItem.Text);
                rpt.SetParameterValue("staffTitle", "Assigned to: " + cboStaff.SelectedItem.Text, rpt.Subreports[0].Name);
            }
            else
            {
                rpt.SetParameterValue("staffTitle", "");
                rpt.SetParameterValue("staffTitle", "", rpt.Subreports[0].Name);
            }
            //rpt.SetParameterValue("reportTitle", "Loans Report as at " +endDate.ToString("dd-MMM-yyyy"));
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
                foreach (var cl in le.clients.Where(p => (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
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
