
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
    public partial class loanDetails : corePage
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
                string categoryID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "";
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 0, 0, 0));

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
            Session["startDate"] = dtpStartDate.SelectedDate;
            if (cboClient.SelectedValue != "")
            {
                var clientID = int.Parse(cboClient.SelectedValue);
                Session["ClientID"] = clientID;
                Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, clientID);
            }
            else
            {
                Session["ClientID"] = null;
                Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, null);
            } 
            //this.rvw.DataBind();

        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["endDate"] != null)
            {
                DateTime endDate = (DateTime)Session["endDate"];
                DateTime startDate = (DateTime)Session["startDate"];
                if (Session["ClientID"] != null)
                {
                    var clientID = (int)Session["ClientID"];
                    Bind(startDate, endDate, clientID);
                }
                else
                {
                    Bind(startDate, endDate, null);
                }
            }
        }

        private void Bind(DateTime startDate, DateTime endDate, int? clientID)
        {
            var le = new coreLoansEntities();
            var categoryID = Request.Params["catID"];
            int? branchID = null;
            if (cboBranch.SelectedValue != "") branchID = int.Parse(cboBranch.SelectedValue);
            if (categoryID == null) categoryID = "";
            var cl = le.clients.Where(p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))
                && (branchID == null || p.branchID == branchID)).ToList();
            rpt = new coreReports.ln.rptLoanDetails();
            if (clientID == null)
            {
                var res = (new reportEntities()).vwLoanAddresses.Where(p => p.disbursementDate == null ||
                    ( p.disbursementDate>=startDate && p.disbursementDate <= endDate)).ToList();
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
                var res2 = (new reportEntities()).vwCheckLists.ToList();
                var res3 = (new reportEntities()).vwSchedules.Where(p => p.repaymentDate <= endDate).ToList();
                var res4 = (new reportEntities()).vwCollaterals.Where(p => p.applicationDate <= endDate).ToList();
                var res5 = (new reportEntities()).vwLoans3.ToList();
                res2.Add(new vwCheckList
                {
                    accountNumber="",
                    clientID=-1,
                    clientName="",
                    comments="",
                    description="",
                    loanID=-1
                });
                res4.Add(new vwCollateral { 
                    accountNumber="",
                    clientID=-1,
                    loanID=-1
                });
                rpt.Subreports[0].SetDataSource(res2);
                // rpt.Subreports[1].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                rpt.Subreports[1].SetDataSource(res3);
                rpt.Subreports[2].SetDataSource(res5);
                rpt.Subreports[3].SetDataSource(res4);
            }
            else
            {
                var res = (new reportEntities()).vwLoanAddresses.Where(p => (p.disbursementDate == null ||
                    (p.disbursementDate >= startDate && p.disbursementDate <= endDate)) && p.clientID == clientID.Value).ToList();
                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                rpt.SetDataSource(res);
                var res2 = (new reportEntities()).vwCheckLists.Where( p=> p.clientID==clientID.Value).ToList();
                var res3 = (new reportEntities()).vwSchedules.Where(p=> p.repaymentDate <= endDate && p.clientID == clientID.Value).ToList();
                var res4 = (new reportEntities()).vwCollaterals.Where(p => p.applicationDate <= endDate && p.clientID == clientID.Value).ToList();
                var res5 = (new reportEntities()).vwLoans3.Where(p=>p.clientID==clientID.Value).ToList();
                res2.Add(new vwCheckList
                {
                    accountNumber = "",
                    clientID = -1,
                    clientName = "",
                    comments = "",
                    description = "",
                    loanID = -1,
                    loanNo="",
                    passed=false
                });
                res4.Add(new vwCollateral
                {
                    accountNumber = "",
                    clientID = -1,
                    loanID = -1,
                    clientName=""
                });
                if (res3.Count == 0)
                {
                    res3.Add(new vwSchedule { 
                        accountNumber="",
                        clientID=-1,
                        clientName="",
                        amountDisbursed=0,
                        interestBalance=0,
                        principalBalance=0,
                        principalPayment=0,
                        interestPayment=0,
                        loanID=-1,
                        loanNo="",
                        repaymentDate=DateTime.Now,
                        repaymentModeName="",
                        totalPayment=0
                    });
                }
                if (res5.Count == 0)
                {
                    res5.Add(new vwLoans3 { 
                        accountNumber="",
                        amountDisbursed=0,
                        repaymentAmountDelta=0,
                        balance=0,
                        interestBalance=0,
                        principalBalance=0,
                        processingFeeBalance=0,
                        categoryName="",
                        clientID=-1,
                        clientName="",
                        collateralType="",
                        collateralValue=0,
                        daysDue=0,
                        disbursementDate=DateTime.Now,
                        lastPaymentDate=DateTime.Now,
                        loanEndDate=DateTime.Now,
                        loanID=-1,
                        loanNo="",
                        paid=0,
                        penalty=0,
                        processingFee=0,
                        repaymentDateDelta=0
                    });
                }
                rpt.Subreports[0].SetDataSource(res2);
               // rpt.Subreports[1].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                rpt.Subreports[1].SetDataSource(res3);
                rpt.Subreports[2].SetDataSource(res5);
                rpt.Subreports[3].SetDataSource(res4);
            }
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Credit Committee Report as at " + endDate.ToString("dd-MMM-yyyy"));
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

    }
}
