using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreERP.code;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using coreReports;
using coreLogic;

namespace coreERP.ln.reports
{
    public partial class outstanding : corePage
    {
        ReportDocument rpt;

        public override string URL
        {
            get { return "~/ln/reports/outstanding.aspx"; }
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
            var le = new coreLoansEntities();
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "";
            
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                Session["expired"] = optExpired.Checked;

                //cboOfficer.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                //foreach (var r in le.staffs.OrderBy(p => p.surName))
                //{
                //    cboOfficer.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.otherNames + " ," + r.surName , r.staffID.ToString()));
                //}

                cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.branches.OrderBy(p => p.branchName).ToList())
                {
                    cboBranch.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.branchName, r.branchID.ToString()));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            //int expiredFlag = (int)Session["expired"];
            //Session["loanOfficer"] = cboOfficer.SelectedValue;
            int expiredFlag = Convert.ToInt32(Session["expired"]);

            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(dtpEndDate.SelectedDate.Value, clientID, expiredFlag);
            }
            
            else
            {
                Session["ClientID"] = null;
                Bind(dtpEndDate.SelectedDate.Value, null, expiredFlag);
            }
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e){
            if (Session["endDate"] != null && Session["expired"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                int expiredFlag = Convert.ToInt32(Session["expired"]);
                if (Session["ClientID"] != null )
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(endDate, clientID, expiredFlag);
                }
                else
                {
                    Bind(endDate, null, expiredFlag);
                }
            }
        }

        private void Bind(DateTime endDate, int? clientID, int expiredFlag)
        {
            var le = new coreLoansEntities();
            var currentUserName = User.Identity.Name.Trim().ToLower();
            var staff = le.staffs.FirstOrDefault(r => r.userName.ToLower().Trim() == currentUserName);
            if (staff == null)
            {
                HtmlHelper.MessageBox($"The logged in user {currentUserName} is not a staff.", "coreERP©: No Data", IconType.deny);
                return;
                //throw new ArgumentException($"The logged in user {currentUserName} is not a staff.");
            }
            var categoryID = Request.Params["catID"];
            int selectedLoanOfficer = -1;
            //if (!String.IsNullOrEmpty(cboOfficer.SelectedValue))
            //{
            //    int.TryParse(cboOfficer.SelectedValue, out selectedLoanOfficer);
            //}
            //var loanOfficer = cboOfficer.SelectedValue.ToString() == "" ? null : cboOfficer.SelectedValue;

            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                && (branchID == null || p.branchID == branchID)).ToList();

            var compProf = (new core_dbEntities()).comp_prof.First();
            if (compProf.comp_name.ToLower().Trim().Contains("lendzee"))
            {
                rpt = new coreReports.ln.rptOutstandingByGroup();
            }
            else
            {
                rpt = new coreReports.ln.rptOutstanding();
            }
            if (clientID == null)
            {
                var res = (new reportEntities()).spOutstanding(endDate).Where(p =>
                     (expiredFlag == 0 || (expiredFlag == 1 && p.expiryDate < endDate) || (expiredFlag == 2 && p.expiryDate > endDate))
                     /*&& (selectedLoanOfficer == -1 || p.StaffID == selectedLoanOfficer)*/ && p.StaffID==staff.staffID).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the logged in user.", "coreERP©: No Data", IconType.deny);
                    return;
                }
                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                rpt.SetDataSource(res);
            }
            else
            {
                var res = (new reportEntities()).spOutstanding(endDate).Where(p => p.clientID == clientID
                    && (expiredFlag == 0 || (expiredFlag == 1 && p.expiryDate < endDate) || (expiredFlag == 2 && p.expiryDate > endDate))
                     /*&& (selectedLoanOfficer == -1 || p.StaffID == selectedLoanOfficer)*/ && p.StaffID == staff.staffID).ToList();
                if (res.Count == 0)
                {
                    HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                    return;
                }
                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                rpt.SetDataSource(res);
            }
            rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Loans Outstanding as at " + endDate.ToString("dd-MMM-yyyy"));
            var branchName = "";
            if (cboBranch.Text != "")
            {
                branchName = "BRANCH: " + cboBranch.Text;
            }
            try
            {
                rpt.SetParameterValue("branch", branchName);
            }
            catch (Exception) { }
            this.rvw.ReportSource = rpt;
        }
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (dtpEndDate.SelectedDate.Value.CanClose())
            {
                divError.Style["visibility"] = "hidden";
                spanError.InnerHtml = "";
                btnRun.Enabled = true;
            }
            else
            {
                divError.Style["visibility"] = "visible";
                spanError.InnerHtml = "The selected date belongs to a closed period. Please change.";
                btnRun.Enabled = false;
            }
        }
        protected void cboClient_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p =>
                     (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        //protected void cboClient_OfficerRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        //{
        //    if (e.Text.Trim().Length > 2)
        //    {
        //        cboOfficer.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
        //        foreach (var cl in le.staffs.Where(p =>
        //             (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text))).OrderBy(p => p.surName))
        //        {
        //            cboOfficer.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.surName + ", " + cl.otherNames).ToString()));
        //        }
        //    }
        //}

        protected void optExpired_CheckedChanged(object sender, EventArgs e)
        {
            Session["expired"] = optExpired.Checked? 1:(optNotExpired.Checked?2: 0);
        }

    }
}
