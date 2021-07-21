using coreLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreReports;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class approvePenalty : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        List<LoanPenaltyVM> penList = new List<LoanPenaltyVM>();
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            { 
                dtDate.SelectedDate = DateTime.Now.Date;
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.jnl_batch jb = null;
            var pro = ent.comp_prof.FirstOrDefault();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtProposedAmount = item.FindControl("txtProposedAmount") as Telerik.Web.UI.RadNumericTextBox;
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                var dtDate2 = item.FindControl("dtDate2") as RadDatePicker;
                if (lblID != null && txtProposedAmount != null && chkSelected != null && chkSelected.Checked == true && dtDate2 != null
                    && txtProposedAmount.Value>0)
                {
                    int id = int.Parse(lblID.Text);
                    var loan = le.loans.FirstOrDefault(p => p.loanID == id);
                    if (loan != null && loan.closed==false)
                    {
                        if (pro.comp_name.ToLower().Contains("jireh") || pro.comp_name.ToLower().Contains("demo"))
                        {
                            loan.repaymentSchedules.Add(new repaymentSchedule
                            {
                                repaymentDate = dtDate2.SelectedDate.Value,
                                interestBalance = txtProposedAmount.Value.Value,
                                interestPayment = txtProposedAmount.Value.Value,
                                principalBalance=0,
                                principalPayment=0,
                                origInterestPayment=txtProposedAmount.Value.Value,
                                origPrincipalBF=0,
                                origPrincipalCD=0,
                                origPrincipalPayment=0,
                                additionalInterest=0,
                                additionalInterestBalance=0,
                                penaltyAmount=0,
                                creation_date = DateTime.Now,
                                creator = User.Identity.Name
                            });

                            jb = journalextensions.Post("LN", loan.loanType.accountsReceivableAccountID,
                                loan.loanType.interestIncomeAccountID, txtProposedAmount.Value.Value,
                                "Additional Interest for Loan - " + loan.client.surName + "," + loan.client.otherNames,
                                pro.currency_id.Value, dtDate2.SelectedDate.Value, loan.loanNo, ent, User.Identity.Name,
                                loan.client.branchID);
                            if (jb != null)
                            {
                                ent.jnl_batch.Add(jb);
                            }
                        }
                        else
                        {
                            loan.loanPenalties.Add(new loanPenalty
                            {
                                penaltyDate = dtDate2.SelectedDate.Value,
                                penaltyBalance = txtProposedAmount.Value.Value,
                                penaltyFee = txtProposedAmount.Value.Value,
                                penaltyTypeId = null,
                                proposedAmount = 0,
                                creation_date = DateTime.Now,
                                creator = User.Identity.Name
                            });

                            jb = journalextensions.Post("LN", loan.loanType.accountsReceivableAccountID,
                                loan.loanType.unearnedInterestAccountID, txtProposedAmount.Value.Value,
                                "Additional Interest for Loan - " + loan.client.surName + "," + loan.client.otherNames,
                                pro.currency_id.Value, dtDate2.SelectedDate.Value, loan.loanNo, ent, User.Identity.Name,
                                loan.client.branchID);
                            if (jb != null)
                            {
                                ent.jnl_batch.Add(jb);
                            }
                        }
                        loan.lastPenaltyDate = dtDate2.SelectedDate;
                    }
                }
            }
            le.SaveChanges();
            ent.SaveChanges();
            HtmlHelper.MessageBox2("Additional Interest applied successfully!", ResolveUrl("/ln/loans/approvePenalty.aspx"), "coreERP©: Successful", IconType.ok);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var id = -1;
            if (cboClient.SelectedValue != "" && dtDate.SelectedDate!= null)
            { 
                id = int.Parse(cboClient.SelectedValue);
                CalculatePenalty(id, dtDate.SelectedDate.Value);
            }
            
            rpPenalty.DataSource = penList;
            rpPenalty.DataBind();
        }

        public void CalculatePenalty(int clientID, DateTime date)
        {
            //try
            //{ 
                var lns = le.loans.Where(p => p.amountDisbursed > 0 && 
                    p.clientID == clientID && (p.closed==false)).ToList();

                var prof = ((new coreLogic.core_dbEntities()).comp_prof.FirstOrDefault());

                foreach (var ln in lns)
                { 
                    var dummyPens = ln.loanPenalties.Where(p => p.proposedAmount > 0 && (prof.penaltyMode != 1 || p.proposedAmount < 0.5)).ToList();
                    for (var i = dummyPens.Count - 1; i >= 0;i-- )
                    {
                        var p = dummyPens[i];
                        le.loanPenalties.Remove(p);
                    }

                    double principalPaid = ln.loanRepayments.Where(p => p.repaymentTypeID == 2 || p.repaymentTypeID == 1)
                        .Sum(p => p.principalPaid);
                 
                    double interestExpected = ln.repaymentSchedules.Sum(p => p.interestPayment);
                    double intersetPaid = ln.loanRepayments.Where(p => p.repaymentTypeID == 3 || p.repaymentTypeID == 1)
                        .Sum(p => p.interestPaid);

                    double penaltyAdded = ln.loanPenalties.Sum(p => p.penaltyFee);
                    double penaltyPaid = ln.loanRepayments.Where(p => p.repaymentTypeID == 7)
                        .Sum(p => p.penaltyPaid);

                    double totalFess = ln.applicationFee + ln.processingFee + ln.insuranceAmount + ln.commission;
                    List<int> feeTypeIds = new List<int> {4,5,6,8};
                    double feesPaid = ln.loanRepayments.Where(p => feeTypeIds.Contains(p.repaymentTypeID))
                    .Sum(p => p.feePaid);

                    double principalBal = ln.amountDisbursed - principalPaid;
                    double interestBal = interestExpected - intersetPaid;
                    double penaltyBalance = penaltyAdded - penaltyPaid;
                    double feesBalance = totalFess - feesPaid;

                    var oneYearAgo = DateTime.Today.Date.AddYears(-1);
                    var rps = ln.repaymentSchedules
                        .Where(p => (p.loan.loanType.loanTypeName.Contains("invoice") && p.loan.disbursementDate < oneYearAgo) 
                        || (!p.loan.loanType.loanTypeName.Contains("invoice"))
                        || p.repaymentDate <= date && (p.principalBalance > 0|| p.interestBalance>0))
                        .ToList();

                    if (rps.Count == 0) continue;
                    var lastDuePaymentDate = ln.lastPenaltyDate??date;
                    if (prof.comp_name.ToLower().Contains("link") || (rps.Count > 0 || ln.loanPenalties.Any(p=> p.penaltyBalance>1) || principalBal>0 || interestBal> 0
                    || penaltyBalance > 0|| feesBalance>0))
                    {
                        var lastDueDate = date;
                        if (prof.comp_name.ToLower().Contains("jireh") || prof.comp_name.ToLower().Contains("demo"))
                        {
                            lastDueDate = rps.Min(p => p.repaymentDate).AddMonths(1);
                        }
                        else
                        {
                            lastDueDate = rps.Min(p => p.repaymentDate);
                        }
                        if (lastDueDate > lastDuePaymentDate && lastDueDate != date)
                        {
                            lastDuePaymentDate = lastDueDate;
                        }
                    }
                    else if(!prof.comp_name.ToLower().Contains("link"))
                    {
                        continue;
                    }
                    var daysOverdue = (date - lastDuePaymentDate).TotalDays;
                    if (daysOverdue < 0) daysOverdue = 0;
                    var penalty = 0.0;
                    var tenure = ln.loanTenure;
                    if (tenure < 2) tenure = 1;
                    else if (tenure <= 3) tenure = 3;
                    else if (tenure <= 6) tenure = 6;
                    else if (tenure <= 9) tenure = 9;
                    else if (tenure <= 12) tenure = 12;
                    else if (tenure <= 18) tenure = 18;
                    else if (tenure <= 24) tenure = 24;
                    else if (tenure <= 36) tenure = 36;
                    else if (tenure <= 48) tenure = 48;
                    else if (tenure <= 60) tenure = 60;
                    var lt = le.tenors.FirstOrDefault(p => p.tenor1 == tenure);
                    if (lt == null) lt = le.tenors.First();

                    var balance = 0.0;
                    GetBalanceAsAt(date, ln.loanID, ref balance);
                    if (1/*prof.penaltyMode */== 1)
                    {
                        penalty = (daysOverdue / 30.0) * lt.defaultPenaltyRate.Value * balance / 100.0; 
                    }
                    else
                    {
                        var totalPaid = ln.loanRepayments.Sum(p => p.principalPaid + p.interestPaid);
                        var interestPayment = rps.Sum(p => p.interestPayment);
                        var principalPayment = rps.Sum(p => p.principalPayment);
                        var date2 = rps.Min(p => p.repaymentDate);
                        var a = ((Math.Ceiling((date - date2).TotalDays / 30.0) * lt.defaultPenaltyRate.Value * (interestPayment + principalPayment)) / 100.0) - totalPaid;
                        if (a > 0)
                        {
                            penalty += a;
                        }
                    }
                    
                    if (penalty < 0 ) penalty = 0;
                    penList.Add(new LoanPenaltyVM
                    {
                        loanId=ln.loanID,
                        loanNo=ln.loanNo,
                        clientName = (ln.client.clientTypeID == 3 || ln.client.clientTypeID == 4 || ln.client.clientTypeID == 5) ? 
                            ln.client.companyName : ((ln.client.clientTypeID == 6) ? ln.client.accountName : ln.client.surName +
                            ", " + ln.client.otherNames) + " (" + ln.client.accountNumber + ")",
                        penaltyAmount=penalty,
                        penaltyDate=date,
                        accountNumber=ln.client.accountNumber
                    });
                } 
            //}
            //catch (Exception x)
            //{ 
            //}
        }

        protected void dtDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var id = -1;
            if (cboClient.SelectedValue != "" && dtDate.SelectedDate != null)
            {
                id = int.Parse(cboClient.SelectedValue);
                CalculatePenalty(id, dtDate.SelectedDate.Value);
            }  
            rpPenalty.DataSource = penList;
            rpPenalty.DataBind();
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

        //private void getPrincipalBalance(loan ln)
        //{
        //    var principalPaid = le.loanRepayments.Where(p => p.loanID == ln.)
        //}

        private void GetBalanceAsAt(DateTime date, int loanId, ref double bal)
        {
            using (var _rent = new reportEntities())
            {
                _rent.Database.CommandTimeout = 3000;
                using (var _le = new coreLoansEntities())
                {
                    date = date.Date;
                    var rs = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
                                                                    && (
                                                                        (p.amountPaid > 0 || p.date <= date)
                                                                        || (Math.Abs(p.amountPaid) < 1 || p.date < date)
                                                                        )
                        ).ToList();
                    var rs2 = _le.loanRepayments.Where(p => p.loanID == loanId && p.repaymentDate <= date
                                                            &&
                                                            (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 ||
                                                             p.repaymentTypeID == 3)).ToList();
                    if (rs.Count > 0)
                    {
                        bal = rs.Max(p => p.amountDisbursed)
                              + rs.Sum(p => p.interest)
                              + rs.Sum(p => p.penaltyAmount)
                              - rs.Sum(p => p.amountPaid);
                    }
                    else
                    {
                        bal = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId)
                            .Max(p => p.amountDisbursed)
                              -
                              ((rs2.Count > 0)
                                  ? rs2.Sum(p => p.principalPaid)
                                  : 0.0);
                    }
                    if (bal < 0)
                    {
                        bal = 0;
                    }
                }
            }

        


    }

        

        private class LoanPenaltyVM
        {
            public string loanNo { get; set; }
            public string accountNumber { get; set; }
            public string clientName { get; set; }
            public DateTime penaltyDate { get; set; }
            public double penaltyAmount { get; set; }
            public int loanId { get; set; }
        }

    }
}