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
    public partial class loans2 : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports/loans.aspx"; }
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
            if (!IsPostBack)
            {
                dtpEndDate.SelectedDate = DateTime.Now.Date;
                dtpStartDate.SelectedDate = new DateTime(2000, 1, 1);

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
            if (cboClient.SelectedValue != null && cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Bind(clientID);
            }
            else
            {
                Bind(null);
            } 
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        { 
            if (cboClient.SelectedValue != null && cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Bind(clientID);
            }
            else
            {
                Bind(null);
            } 
        }

        private void Bind(int? clientID)
        {
            var le = new coreLoansEntities();
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
            var cl = le.clients.Where(p => (branchId == null || p.branchID == branchId)).ToList();
            rpt = new coreReports.ln.rptLoans3();
            var res = rent.vwLoans2
                .Where(
                    p =>
                        p.disbursementDate >= dtpStartDate.SelectedDate && p.disbursementDate <= dtpEndDate.SelectedDate)
                .Where(p => clientID == null || clientID == p.clientID)
                .Where(
                    p =>
                        expiry == 1 || (expiry == 2 && p.expiryDate <= DateTime.Now) ||
                        (expiry == 3 && p.expiryDate > DateTime.Now))
                .Where(
                    p =>
                        status == "1" 
                        || (status == "2" && p.totalBalance <= 10) ||
                        (status == "3" && p.cumPaid > 0 && p.totalBalance > 10)
                        || (status == "4" && p.cumPaid < 10)
                        || (status == "5" && p.totalBalance >  10))
                .Where(
                    p =>
                        all == true || (all == false && p.cumPayable <= p.cumPaid + 10) ||
                        (all == null && p.cumPaid + 5 > p.cumPayable))
                .OrderByDescending(p => p.disbursementDate)
                .ToList();
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria",
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
            var sumRpt = rpt.Subreports[0];
            rpt.SetDataSource(res);

            rpt.Subreports[1].SetDataSource(rent.vwCompProfs.ToList());
            sumRpt.Database.Tables[1].SetDataSource(rent.vwCompProfs.ToList()); 
            sumRpt.Database.Tables[0].SetDataSource(res);
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("companyName", Settings.companyName, sumRpt.Name);
            if (dtpStartDate.SelectedDate > new DateTime(2000, 12, 1))
            {
                rpt.SetParameterValue("reportTitle",
                    "Loans Given Report b/n " + dtpStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                    + " and " + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"));
                rpt.SetParameterValue("reportTitle",
                    "Loans Given Report b/n " + dtpStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                    + " and " + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"), sumRpt.Name);
            }
            else
            {
                rpt.SetParameterValue("reportTitle",
                    "Loans Given Report as at " + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"));
                rpt.SetParameterValue("reportTitle",
                    "Loans Given Report as at " + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"), sumRpt.Name);
            }
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
            catch (Exception)
            {
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
                foreach (var cl in le.clients.Where(p => (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }
 
    }
}
