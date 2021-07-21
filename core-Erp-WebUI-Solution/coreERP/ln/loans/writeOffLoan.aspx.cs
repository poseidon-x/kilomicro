using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreReports;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class writeOffLoan : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;

        string categoryID = null;
        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int clientID = -1;
            if (cboClient.SelectedValue != "")
            {
                clientID = int.Parse(cboClient.SelectedValue);
            }
            var data = (from c in le.clients
                from l in le.loans
                where c.clientID == l.clientID
                      && l.loanStatusID == 4
                      && l.clientID == clientID
                select new 
                {
                    loan=l,
                    client=c
                })
                .ToList()
                .Select(p=> new LoanClosureViewModel
                {
                    loanId = p.loan.loanID,
                    clientName = (p.client.clientTypeID == 3 || p.client.clientTypeID == 4 || p.client.clientTypeID == 5)
                        ? p.client.companyName
                        : ((p.client.clientTypeID == 6) ? p.client.accountName : p.client.surName + ", " + p.client.otherNames),
                    accountNumber = p.client.accountNumber,
                    loanNo = p.loan.loanNo,
                    proposedWriteOff = GetBalanceAsAt(DateTime.Now, p.loan.loanID),
                    principalBalance = GetPrincipalBalanceAsAt(DateTime.Now, p.loan.loanID),
                    interestBalance =
                        GetBalanceAsAt(DateTime.Now, p.loan.loanID) - GetPrincipalBalanceAsAt(DateTime.Now, p.loan.loanID)
                        -GetPenaltyBalanceAsAt(DateTime.Now, p.loan.loanID),
                    penaltyBalance = GetPenaltyBalanceAsAt(DateTime.Now, p.loan.loanID)
                })
                .Where(p=> p.proposedWriteOff>0.99)
                .ToList();
            gridSchedule.DataSource = data;
            gridSchedule.DataBind();

            foreach (var item in gridSchedule.Items)
            {
                if (item is GridDataItem)
                {
                    var item2 = item as GridDataItem;
                    var txt = item2.FindControl("txtAmount") as RadNumericTextBox;
                    var lblDate = item2.FindControl("txtDate") as Label;
                    if (lblDate != null)
                    {
                        lblDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    }
                }
            }
        }

        private double GetBalanceAsAt(DateTime date, int loanId)
        {
            double bal = 0.0;
            using (var _rent = new reportEntities())
            {
                _rent.Database.CommandTimeout = 600000;
                using (var _le = new coreLoansEntities())
                {
                    _le.Database.CommandTimeout = 60000;
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

            return bal;
        }

        private double GetPrincipalBalanceAsAt(DateTime date, int loanId)
        {
            double bal = 0.0;
            using (var _rent = new reportEntities())
            {
                _rent.Database.CommandTimeout = 600000;
                using (var _le = new coreLoansEntities())
                {
                    _le.Database.CommandTimeout = 60000;
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
                              - rs.Sum(p => p.principalPaid);
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

            return bal;
        }

        private double GetPenaltyBalanceAsAt(DateTime date, int loanId)
        {
            double bal = 0.0;
            using (var _rent = new reportEntities())
            {
                _rent.Database.CommandTimeout = 600000;
                using (var _le = new coreLoansEntities())
                {
                    _le.Database.CommandTimeout = 60000;
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
                        bal = rs.Sum(p => p.penaltyAmount)
                              - rs.Sum(p => p.penaltyPaid);
                    }
                    else
                    {
                        bal = 0.0;
                    }
                    if (bal < 0)
                    {
                        bal = 0;
                    }
                }
            }

            return bal;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            le=new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            le.Database.CommandTimeout = 600000;
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "1";
            if (!Page.IsPostBack)
            { 
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            bool changed=false;
            coreLogic.jnl_batch batch = null;
            var pro = ent.comp_prof.FirstOrDefault();

            foreach (var item in gridSchedule.Items)
            {
                if (item is GridDataItem)
                {
                    var item2 = item as GridDataItem;
                    var txt = item2.FindControl("txtAmount") as RadNumericTextBox;
                    var lblDate = item2.FindControl("txtDate") as Label ;
                    var date = DateTime.Now;
                    if (txt != null && txt.Value != null && item2.Selected==true)
                    {                         
                        var key = item2.GetDataKeyValue("loanId").ToString();
                        var loanId = int.Parse(key);
                        var loan = le.loans.FirstOrDefault(p => p.loanID == loanId);
                        if (loan != null)
                        {
                            var amount =  txt.Value.Value;
                            var princBal = GetPrincipalBalanceAsAt(date, loanId);
                            var penBal = GetPenaltyBalanceAsAt(date, loanId);
                            var totalBal = GetBalanceAsAt(date, loanId);
                            var intWrittenOff = amount >= totalBal - princBal-penBal ? totalBal - princBal-penBal : amount;
                            var penWrittenOff = amount - intWrittenOff >= penBal ? penBal : amount - intWrittenOff;
                            var princWrittenOff = amount - intWrittenOff - penWrittenOff;

                            var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                            if (user.accessLevel.approvalLimit < amount)
                            {
                                HtmlHelper.MessageBox("The amount to be approved is beyond your access level",
                                                            "coreERP©: Failed", IconType.deny);
                                return;
                            }

                            loan.balance -= princWrittenOff;
                            if (loan.balance < 0)
                            {
                                loan.balance = 0;
                            }

                            changed = true;

                            var intRunning = intWrittenOff;
                            var princRunning = princWrittenOff;
                            foreach (
                                var rp in
                                    loan.repaymentSchedules.Where(p => p.principalBalance > 0 || p.interestBalance > 0)
                                    .OrderBy(p=> p.repaymentDate))
                            {
                                var intw = intRunning > rp.interestBalance ? rp.interestBalance : intRunning;
                                var princw = princRunning > rp.principalBalance ? rp.principalBalance : princRunning;

                                rp.interestBalance -= intw;
                                rp.principalBalance -= princw;

                                if (rp.interestBalance < 0)
                                {
                                    rp.interestBalance = 0;
                                }
                                if (rp.principalBalance < 0)
                                {
                                    rp.principalBalance = 0.0;
                                } 
                            }
                            var lw = new coreLogic.loanRepayment
                            {
                                principalPaid = princWrittenOff,
                                interestPaid = intWrittenOff,
                                amountPaid = intWrittenOff + princWrittenOff,
                                repaymentTypeID = 1,
                                repaymentDate = date,
                                creation_date = DateTime.Now,
                                creator = User.Identity.Name,
                                feePaid = 0,
                                penaltyPaid = 0,
                                commission_paid = 0,
                                modeOfPaymentID = 7,
                                bankID = null,
                                checkNo = "",

                            };
                            loan.loanRepayments.Add(lw);
                            lw = new coreLogic.loanRepayment
                            {
                                principalPaid = 0,
                                interestPaid = 0,
                                amountPaid = penWrittenOff,
                                repaymentTypeID = 7,
                                repaymentDate = date,
                                creation_date = DateTime.Now,
                                creator = User.Identity.Name,
                                feePaid = 0,
                                penaltyPaid = penWrittenOff,
                                commission_paid = 0,
                                modeOfPaymentID = 7,
                                bankID = null,
                                checkNo = "",

                            };
                            loan.loanRepayments.Add(lw);

                            var prof = ent.comp_prof.First();
                            //if (prof != null)
                            //{
                            //    if (batch == null)
                            //    {
                            //        batch = journalextensions.Post("LN", loan.loanType.unearnedInterestAccountID,
                            //            loan.loanType.accountsReceivableAccountID, totalBal - princBal,
                            //            "Loan Interest Write off - " + loan.client.surName + "," +
                            //            loan.client.otherNames,
                            //            pro.currency_id.Value, date, loan.loanNo, ent, User.Identity.Name,
                            //            loan.client.branchID);
                            //    }
                            //    else
                            //    {
                            //        var batch2 = journalextensions.Post("LN", loan.loanType.unearnedInterestAccountID,
                            //            loan.loanType.accountsReceivableAccountID, totalBal - princBal,
                            //            "Loan Interest Write off - " + loan.client.surName + "," +
                            //            loan.client.otherNames,
                            //            pro.currency_id.Value, date, loan.loanNo, ent, User.Identity.Name,
                            //            loan.client.branchID);
                            //        var j = batch2.jnl.ToList();
                            //        if (j.Count > 1)
                            //        {
                            //            batch.jnl.Add(j[0]);
                            //            batch.jnl.Add(j[1]);
                            //        }
                            //    }
                            //}
                            if (batch == null)
                            {
                                batch = journalextensions.Post("LN", loan.loanType.writeOffAccountID,
                                    loan.loanType.accountsReceivableAccountID, amount,
                                    "Loan Write off - " + ((loan.client.clientTypeID == 3 || loan.client.clientTypeID == 4 || loan.client.clientTypeID == 5)
                                            ? loan.client.companyName : ((loan.client.clientTypeID == 6) ? loan.client.accountName : loan.client.surName +
                                            ", " + loan.client.otherNames) + " (" + loan.client.accountNumber + ")"),
                                    pro.currency_id.Value, date, loan.loanNo, ent, User.Identity.Name,
                                    loan.client.branchID);
                            }
                            else
                            {
                                var batch2 = journalextensions.Post("LN", loan.loanType.writeOffAccountID,
                                    loan.loanType.accountsReceivableAccountID, amount,
                                    "Loan Write off - " + ((loan.client.clientTypeID == 3 || loan.client.clientTypeID == 4 || loan.client.clientTypeID == 5)
                                            ? loan.client.companyName : ((loan.client.clientTypeID == 6) ? loan.client.accountName : loan.client.surName +
                                            ", " + loan.client.otherNames) + " (" + loan.client.accountNumber + ")"),
                                    pro.currency_id.Value, date, loan.loanNo, ent, User.Identity.Name,
                                    loan.client.branchID);
                                var j = batch2.jnl.ToList();
                                if (j.Count > 1)
                                {
                                    batch.jnl.Add(j[0]);
                                    batch.jnl.Add(j[1]);
                                }
                            }
                        }
                    }
                }
            }

            if(changed)
            {
                ent.jnl_batch.Add(batch);
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Loan Balance Write-off Data Saved Successfully!", 
                    ResolveUrl("~/ln/loans/writeOffLoan.aspx"), "coreERP©: Successful", IconType.ok);   
            }
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnAll_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public class LoanClosureViewModel
        {
            public int loanId { get; set; }
            public string loanNo { get; set; }
            public string accountNumber { get; set; }
            public string clientName { get; set; }
            public double principalBalance { get; set; }
            public double interestBalance { get; set; }
            public double penaltyBalance { get; set; }
            public double proposedWriteOff { get; set; }
        }
    }
}