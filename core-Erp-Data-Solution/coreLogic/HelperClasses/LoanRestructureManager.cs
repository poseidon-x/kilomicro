using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using coreReports;
using Microsoft.VisualBasic.ApplicationServices;

namespace coreLogic
{
    public class LoanRestructureManager
    {

        private IJournalExtensions journalextensions = new JournalExtensions();
        private IScheduleManager schedMgr = new ScheduleManager();

        public List<repaymentSchedule> CheckLoanRestructuredSch(coreLoansEntities le, loan ln,
            double additionalPrincipalAmount, DateTime restructuredDate, int additionalTenure, int interestRate,
            string userName)
        {
            List<coreLogic.repaymentSchedule> sched;
            DisbursementsManager disManager = new DisbursementsManager();


            DateTime maturityDate = ln.disbursementDate.Value.AddMonths((int) ln.loanTenure);

            ln.originalAmountDisbursed = ln.amountDisbursed;
            ln.originalLoantenure = ln.loanTenure;
            double totalBalance = additionalPrincipalAmount + GetTotalBalanceAsAt(restructuredDate, ln.loanID, ln);


            if (restructuredDate >= maturityDate)
            {
                double totalInterest = totalBalance*(interestRate*12/100.0)*(additionalTenure/12.0);
                double additionalPrincipalSch = totalBalance/additionalTenure;
                double additionalInterestSch = totalInterest/additionalTenure;

                double currentPrinBalance = totalBalance;
                double currentIntrBalance = totalInterest;

                sched = le.repaymentSchedules.Where(p => p.loanID == ln.loanID).ToList();
                for (var i = 0; i < additionalTenure; i++)
                {
                    currentPrinBalance -= additionalPrincipalSch;
                    currentIntrBalance -= additionalInterestSch;

                    var sh = new repaymentSchedule
                    {
                        loanID = ln.loanID,
                        repaymentDate = maturityDate.AddMonths(i + 1),
                        principalPayment = additionalPrincipalSch,
                        interestPayment = additionalInterestSch,
                        principalBalance = currentPrinBalance,
                        interestBalance = currentIntrBalance,
                        origInterestPayment = additionalInterestSch,
                        origPrincipalPayment = additionalPrincipalSch,
                        additionalInterest = 0,
                        additionalInterestBalance = 0,
                        creation_date = DateTime.Now,
                        creator = userName
                    };
                    sched.Add(sh);
                    ln.repaymentSchedules.Add(sh);
                }
            }
            else
            {
                double daysToExpiry = (maturityDate - restructuredDate).TotalDays;
                double newInterest;

                //double extraInterest = additionalPrincipalAmount*(interestRate/100.0);//* (daysToExpiry / 365.0);
                double additionalPrincipalSch = additionalPrincipalAmount/additionalTenure;
                sched = le.repaymentSchedules.Where(p => p.loanID == ln.loanID).ToList();
                var unexpiredSched =
                    le.repaymentSchedules.Where(p => p.loanID == ln.loanID && p.repaymentDate > restructuredDate)
                        .ToList();

                if (totalBalance > 10 && additionalTenure > 0)
                {
                    newInterest = additionalPrincipalAmount*(interestRate/100.0)*
                                  (additionalTenure + unexpiredSched.Count);

                    double additionalInterestSchNew = newInterest/(additionalTenure + unexpiredSched.Count);
                    double currentPrinBalance = additionalPrincipalAmount;
                    double currentIntrBalance = newInterest;

                    foreach (var record in unexpiredSched)
                    {
                        record.interestPayment += additionalInterestSchNew;
                        record.interestBalance += additionalInterestSchNew;
                        currentIntrBalance -= additionalInterestSchNew;
                    }

                    for (var i = 0; i < additionalTenure; i++)
                    {
                        currentPrinBalance -= additionalPrincipalSch;
                        currentIntrBalance -= additionalInterestSchNew;

                        var sh = new repaymentSchedule
                        {
                            loanID = ln.loanID,
                            repaymentDate = maturityDate.AddMonths(i + 1),
                            principalPayment = additionalPrincipalSch,
                            interestPayment = additionalInterestSchNew,
                            principalBalance = currentPrinBalance,
                            interestBalance = currentIntrBalance,
                            origInterestPayment = additionalInterestSchNew,
                            origPrincipalPayment = additionalPrincipalSch,
                            additionalInterest = 0,
                            additionalInterestBalance = 0,
                            creation_date = DateTime.Now,
                            creator = userName
                        };
                        sched.Add(sh);
                        ln.repaymentSchedules.Add(sh);
                    }
                }
                else if (totalBalance > 10 && additionalTenure == 0)
                {
                    double extraInterest = (additionalPrincipalAmount*(interestRate/100.0))*unexpiredSched.Count;
                    //newInterest = totalBalance * (interestRate * 12 / 100.0);


                    double additionalInterestSchNew = extraInterest/unexpiredSched.Count;
                    additionalPrincipalSch = additionalPrincipalAmount/unexpiredSched.Count;

                    double currentPrinBalance = totalBalance;
                    double currentIntrBalance = extraInterest;

                    foreach (var record in unexpiredSched)
                    {
                        record.principalPayment += additionalPrincipalSch;
                        record.interestPayment += additionalInterestSchNew;
                        record.principalBalance += additionalPrincipalSch;
                        record.interestBalance += additionalInterestSchNew;

                        currentPrinBalance -= additionalPrincipalSch;
                        currentIntrBalance -= additionalInterestSchNew;

                    }

                }

            }

            ln.amountDisbursed += additionalPrincipalAmount;
            ln.loanTenure += additionalTenure;

            return sched;
        }

        public List<repaymentSchedule> RestructureLoanAddAmountAddTenure(coreLoansEntities le, loan ln,
            double additionalPrincipalAmount, DateTime restructuredDate, int bank, int paymentType,  string checkNo, int additionalTenure,
            int interestRate, string userName, int paymentMode )
        {
            core_dbEntities ent = new core_dbEntities();
            List<coreLogic.repaymentSchedule> sched;


            DateTime maturityDate = ln.disbursementDate.Value.AddMonths((int) ln.loanTenure);

            
            double totalBalance = additionalPrincipalAmount + GetTotalBalanceAsAt(restructuredDate, ln.loanID, ln);


            if (restructuredDate >= maturityDate)
            {
                double totalInterest = totalBalance*(interestRate*12/100.0)*(additionalTenure/12.0);
                double additionalPrincipalSch = totalBalance/additionalTenure;
                double additionalInterestSch = totalInterest/additionalTenure;

                double currentPrinBalance = totalBalance;
                double currentIntrBalance = totalInterest;

                sched = le.repaymentSchedules.Where(p => p.loanID == ln.loanID).ToList();
                for (var i = 0; i < additionalTenure; i++)
                {
                    currentPrinBalance -= additionalPrincipalSch;
                    currentIntrBalance -= additionalInterestSch;

                    var sh = new repaymentSchedule
                    {
                        loanID = ln.loanID,
                        repaymentDate = maturityDate.AddMonths(i + 1),
                        principalPayment = additionalPrincipalSch,
                        interestPayment = additionalInterestSch,
                        principalBalance = currentPrinBalance,
                        interestBalance = currentIntrBalance,
                        origPrincipalCD = currentPrinBalance + additionalPrincipalSch,
                        origInterestPayment = additionalInterestSch,
                        origPrincipalPayment = additionalPrincipalSch,
                        additionalInterest = 0,
                        additionalInterestBalance = 0,
                        creation_date = DateTime.Now,
                        creator = userName
                    };
                    sched.Add(sh);
                    ln.repaymentSchedules.Add(sh);
                    le.repaymentSchedules.Add(sh);

                }
            }
            else
            {
                double daysToExpiry = (maturityDate - restructuredDate).TotalDays;
                double newInterest;

                //double extraInterest = additionalPrincipalAmount*(interestRate/100.0);//* (daysToExpiry / 365.0);
                double additionalPrincipalSch = additionalPrincipalAmount/additionalTenure;
                sched = le.repaymentSchedules.Where(p => p.loanID == ln.loanID).ToList();
                var unexpiredSched =
                    le.repaymentSchedules.Where(p => p.loanID == ln.loanID && p.repaymentDate > restructuredDate)
                        .ToList();

                if (totalBalance > 10 && additionalTenure > 0)
                {
                    newInterest = additionalPrincipalAmount*(interestRate/100.0)*
                                  (additionalTenure + unexpiredSched.Count);

                    double additionalInterestSchNew = newInterest/(additionalTenure + unexpiredSched.Count);
                    double currentPrinBalance = additionalPrincipalAmount;
                    double currentIntrBalance = newInterest;

                    foreach (var record in unexpiredSched)
                    {
                        record.interestPayment += additionalInterestSchNew;
                        record.interestBalance += additionalInterestSchNew;
                        currentIntrBalance -= additionalInterestSchNew;
                    }

                    for (var i = 0; i < additionalTenure; i++)
                    {
                        currentPrinBalance -= additionalPrincipalSch;
                        currentIntrBalance -= additionalInterestSchNew;

                        var sh = new repaymentSchedule
                        {
                            loanID = ln.loanID,
                            repaymentDate = maturityDate.AddMonths(i + 1),
                            principalPayment = additionalPrincipalSch,
                            interestPayment = additionalInterestSchNew,
                            principalBalance = currentPrinBalance,
                            interestBalance = currentIntrBalance,
                            origPrincipalCD = currentPrinBalance + additionalPrincipalSch,
                            origInterestPayment = additionalInterestSchNew,
                            origPrincipalPayment = additionalPrincipalSch,
                            additionalInterest = 0,
                            additionalInterestBalance = 0,
                            creation_date = DateTime.Now,
                            creator = userName
                        };
                        sched.Add(sh);
                        ln.repaymentSchedules.Add(sh);
                        le.repaymentSchedules.Add(sh);
                    }
                }
                else if (totalBalance > 10 && additionalTenure == 0)
                {
                    double extraInterest = additionalPrincipalAmount*(interestRate*12/100.0)*(daysToExpiry/365.0);
                    //newInterest = totalBalance * (interestRate * 12 / 100.0);


                    double additionalInterestSchNew = extraInterest/unexpiredSched.Count;
                    additionalPrincipalSch = additionalPrincipalAmount/unexpiredSched.Count;

                    double currentPrinBalance = totalBalance;
                    double currentIntrBalance = extraInterest;

                    foreach (var record in unexpiredSched)
                    {
                        record.principalPayment += additionalPrincipalSch;
                        record.interestPayment += additionalInterestSchNew;
                        record.principalBalance += additionalPrincipalSch;
                        record.interestBalance += additionalInterestSchNew;

                        currentPrinBalance -= additionalPrincipalSch;
                        currentIntrBalance -= additionalInterestSchNew;
                    }
                }




            }
            var loan = le.loans.FirstOrDefault(p => p.loanID == ln.loanID);

            loan.amountDisbursed += additionalPrincipalAmount;
            loan.loanTenure += additionalTenure;
            loan.originalAmountDisbursed = ln.amountDisbursed;
            loan.originalLoantenure = ln.loanTenure;
            loan.originalInterestRate = (int)ln.interestRate;
            loan.interestRate = interestRate;


            if (additionalPrincipalAmount > 0)
            {
                var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == userName.ToLower());
                if (ct == null)
                {
                    throw new ApplicationException("There is no till defined for the currently logged in user");
                }
                var ctd =
                    le.cashiersTillDays.FirstOrDefault(
                        p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == restructuredDate
                             && p.open);
                if (ctd == null)
                {
                    throw new ApplicationException("The till for the selected date has not been opened for this user");
                }
                var soFar = ln.cashierDisbursements.Sum(p => p.amount);
                if (soFar == null) soFar = 0;

                if (ln.amountApproved < additionalPrincipalAmount + soFar)
                {
                    throw new ApplicationException("Amount to disburse is greater than approved amount!");
                }

                int? bankID = null;
                if (bank != null)
                    bankID = bank;
                var cd = new coreLogic.cashierDisbursement
                {
                    amount = additionalPrincipalAmount,
                    bankID = bankID,
                    checkNo = checkNo,
                    clientID = ln.clientID,
                    loanID = ln.loanID,
                    paymentModeID = paymentMode,
                    posted = true,
                    txDate = restructuredDate,
                    cashierTillID = ct.cashiersTillID,
                    postToSavingsAccount = true
                };
                le.cashierDisbursements.Add(cd);
                CashierDisburse(le, ln, ent, cd, userName);
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException("Error saving to server");
            }

            return sched;
        }

        private double GetTotalBalanceAsAt(DateTime date, int loanId, loan ln)
        {
            var _rent = new reportEntities();
            var _le = new coreLoansEntities();
            double bal = 0;
            date = date.Date;
            DateTime startOfMonth = new DateTime(date.Year, date.Month, 1);

            var rs = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
                                                            && (
                                                                (p.amountPaid > 0 || p.date <= date)
                                                                || (Math.Abs(p.amountPaid) < 1 || p.date < date)
                                                                )
                ).ToList();

            var rsPenalty = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
                                                                   && (
                                                                       (p.penaltyAmount > 0 && p.date < startOfMonth)
                                                                       && p.interest == 0
                                                                       )
                ).ToList();
            var rs2 = _le.loanRepayments.Where(p => p.loanID == loanId && p.repaymentDate <= date
                                                    &&
                                                    (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 ||
                                                     p.repaymentTypeID == 3 || p.repaymentTypeID == 7)).ToList();
            if (rs.Count > 0)
            {
                bal = rs.Max(p => p.amountDisbursed)
                      + rs.Sum(p => p.interest)
                      + (rsPenalty.Any() ? rsPenalty.Sum(p => p.penaltyAmount) : 0)
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

            return bal;
        }

        public void PostRestructuredLoan(coreLoansEntities le, loan ln, double? amountPaid, 
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            string userName, int paymentMode, string crAccountNo, saving sav = null,
            susuAccount sc = null)
        {

            if (paymentMode == 1)
            {
                PostCashLoan(le, ln, amountPaid,
                    disbDate, paymentType, checkNo, ent,
                    userName, crAccountNo);
            }
            else if (paymentMode == 2 || paymentMode == 3)
            {
                PostBankLoan(le, ln, amountPaid, 
                    disbDate, bank, paymentType, checkNo, ent,
                    userName, crAccountNo);
            }
        }

        private void PostBankLoan(coreLoansEntities le, loan ln, double? amountPaid,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            string userName, string crAccountNo)
        {

            if (amountPaid > 0)
            {
                comp_prof pro = ent.comp_prof.FirstOrDefault();
                double amountAtBank = amountPaid.Value;

                int? bankID = null;
                if (bank != "") bankID = int.Parse(bank);

                var da = ent.def_accts.FirstOrDefault(p => p.code == "DA");

                int? disbAcc = null;
                if (da != null)
                {
                    if (da.accts != null)
                    {
                        disbAcc = da.accts.acct_id;
                    }
                }
                else
                {
                    var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1001");
                    if (acc2 != null)
                    {
                        disbAcc = acc2.acct_id;
                    }
                }

                int acctID = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID).accts.acct_id;

                jnl_batch jb;
                jb = DoDisbursementPostings(le, ln, amountPaid, disbDate, paymentType, checkNo, ent, userName, pro,
                    amountAtBank, bankID, disbAcc, acctID);
            }
        }


        private void PostCashLoan(coreLoansEntities le, loan ln, double? amountPaid,
            DateTime? disbDate, string paymentType, string checkNo, core_dbEntities ent,
            string userName, string crAccountNo)
        {

            if (amountPaid > 0)
            {

                comp_prof pro = ent.comp_prof.FirstOrDefault();
                ;
                double amountAtBank = amountPaid.Value;

                var da = ent.def_accts.FirstOrDefault(p => p.code == "DA");

                int acctID = ln.loanType.vaultAccountID;
                int? disbAcc = null;
                var acc = ent.accts.FirstOrDefault(p => p.acc_num == crAccountNo);
                if (da != null)
                {
                    if (da.accts != null)
                    {
                        disbAcc = da.accts.acct_id;
                    }
                }
                else
                {
                    var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1001");
                    if (acc2 != null)
                    {
                        disbAcc = acc2.acct_id;
                    }
                }
                if (acc != null && acc != null && pro.traditionalLoanNo == true)
                {
                    acctID = acc.acct_id;
                }
                jnl_batch jb;
                jb = DoDisbursementPostings(le, ln, amountPaid, disbDate, paymentType, checkNo, ent, userName,
                    pro, amountAtBank, null, disbAcc, acctID);
            }
        }



        private jnl_batch DoDisbursementPostings(coreLoansEntities le, loan ln, double? amountPaid,
            DateTime? disbDate, string paymentType, string checkNo, core_dbEntities ent, string userName,
            comp_prof pro,
            double amountAtBank, int? bankID, int? disbAcc, int acctID)
        {
            jnl_batch jb;

            jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                disbAcc.Value, amountAtBank,
                "Loan Disbursement Principal - " + amountAtBank.ToString("#,###.#0")
                + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);

            coreLogic.jnl_batch jb22 = journalextensions.Post("LN", disbAcc.Value,
                acctID, amountAtBank,
                "Loan Disbursement Principal- " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);

            var j2 = jb22.jnl.ToList();
            if (j2.Count > 1)
            {
                jb.jnl.Add(j2[0]);
                jb.jnl.Add(j2[1]);
            }

            ent.jnl_batch.Add(jb);

            var t = new coreLogic.loanTranch
            {
                amountDisbursed = amountPaid.Value,
                creation_date = DateTime.Now,
                creator = userName,
                disbursementDate = disbDate.Value,
                modeOfPaymentID = int.Parse(paymentType),
                bankID = bankID,
                checkNumber = checkNo
            };
            ln.loanTranches.Add(t);
            return jb;
        }

        public void CashierDisburse(coreLoansEntities le, loan ln, core_dbEntities ent, cashierDisbursement d,
             string userName, susuAccount sc = null)
        {

            if (ln.loanTypeID != 5)
            {
                string crAccNo = null;
                var cnfg = le.susuConfigs.FirstOrDefault();
                if (ln.loanTypeID == 7 || ln.loanTypeID == 8)
                {
                    var acc = ent.accts.FirstOrDefault(p => p.acct_id == cnfg.contributionsPayableAccountID);
                    crAccNo = acc.acc_num;
                }
                else if (d.paymentModeID == 1 && ent.comp_prof.FirstOrDefault().traditionalLoanNo == true)
                {
                    var acc = ent.accts.FirstOrDefault(p => p.acct_id == d.cashiersTill.accountID);
                    crAccNo = acc.acc_num;
                }
                var pro = ent.comp_prof.FirstOrDefault();
                saving sav = null;
                if (pro.disburseLoansToSavingsAccount == true && d.postToSavingsAccount == true)
                {
                    sav = le.savings.FirstOrDefault(p => p.clientID == ln.client.clientID);
                    if (sav != null)
                    {
                        crAccNo = ent.accts.FirstOrDefault(p => p.acct_id == sav.savingType.accountsPayableAccountID).acc_num;
                    }
                }
                this.PostRestructuredLoan(le, ln, d.amount, d.txDate,
                         (d.bankID == null) ? "" : d.bankID.ToString(),
                       d.paymentModeID.ToString(), d.checkNo, ent, userName, d.paymentModeID,
                        crAccNo, sav, sc);
            }
            else
            {
                this.PostRestructuredLoan(le, ln, d.amount,
                    d.txDate, (d.bankID == null) ? "" : d.bankID.ToString(),
                    d.paymentModeID.ToString(), d.checkNo, ent, 
                    userName, d.paymentModeID, null, null);
            }
            ln.disbursedBy = userName;
        }



 
    }
}
