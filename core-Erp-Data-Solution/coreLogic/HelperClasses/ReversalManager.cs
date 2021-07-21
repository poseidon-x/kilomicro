using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic
{
    public class ReversalManager : coreLogic.IReversalManager
    {
        private IJournalExtensions journalextensions = new JournalExtensions();

        public void ReversePayment(coreLoansEntities le, loan ln,
                coreLogic.core_dbEntities ent, loanRepayment lrp, string userName)
        {
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();
            coreSecurityEntities sec = new coreSecurityEntities();

            var pro = ent.comp_prof.FirstOrDefault();

            var acctID = ln.loanType.vaultAccountID;
            var acctID2 = ln.loanType.vaultAccountID;
            var tlrp = le.cashierReceipts.Where(p => p.clientID == lrp.loan.clientID && p.amount == lrp.amountPaid && p.txDate == lrp.repaymentDate).FirstOrDefault();
            if ((lrp.modeOfPaymentID == 2 || lrp.modeOfPaymentID == 3) && lrp.bankID == null)
            {
                var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1046");
                if (tlrp.cashiersTill != null)
                {
                    acctID = tlrp.cashiersTill.accountID;
                }
                else if (gl != null)
                {
                    acctID = gl.acct_id;
                }
                gl = ent.accts.FirstOrDefault(p => p.acc_num == "1047");
                if (gl != null)
                {
                    acctID2 = gl.acct_id;
                }
            }
            else if ((lrp.modeOfPaymentID == 2 || lrp.modeOfPaymentID == 3) && lrp.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == lrp.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var amount = lrp.amountPaid;

            switch (lrp.repaymentTypeID.ToString())
            {
                case "1":
                    var sched = ln.repaymentSchedules.Where(p => p.interestBalance < p.interestPayment || p.principalBalance < p.principalPayment).OrderByDescending(p => p.repaymentDate).ToList();
                    if (sched.Count > 0)
                    {
                        int i = 0;
                        var tprinc = 0.0;
                        var tinterest = 0.0;
                        while (amount > 0 && i < sched.Count)
                        {
                            var s = sched[i];
                            var princ = 0.0;
                            var interest = 0.0;
                            if (amount >= s.principalPayment + s.interestPayment)
                            {
                                princ = s.principalPayment;
                                interest = s.interestPayment;
                            }
                            else if (amount >= s.principalPayment)
                            {
                                princ = s.principalPayment;
                                interest = amount - s.principalBalance;
                            }
                            else
                            {
                                princ = amount;
                            }
                            s.principalBalance += princ;
                            s.interestBalance += interest;

                            i++;
                            tprinc += princ;
                            tinterest += interest;
                            amount = amount - princ - interest;
                        }

                        ln.balance += tprinc;
                        le.loanRepayments.Remove(lrp);

                        coreLogic.jnl_batch jb = journalextensions.Post("LN",
                            ln.loanType.interestIncomeAccountID, ln.loanType.unearnedInterestAccountID, tinterest,
                            "RVSL: Loan repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        coreLogic.jnl_batch jb2 = journalextensions.Post("LN",
                            ln.loanType.accountsReceivableAccountID, acctID, (tprinc + tinterest),
                            "RVSL: Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j = jb2.jnl.ToList();
                        if (j.Count > 1)
                        {
                            jb.jnl.Add(j[0]);
                            jb.jnl.Add(j[1]);
                        }
                        ent.jnl_batch.Add(jb);

                    }
                    break;
                case "2":
                    sched = ln.repaymentSchedules.Where(p => p.interestBalance < p.interestPayment || p.principalBalance < p.principalPayment).OrderByDescending(p => p.repaymentDate).ToList();
                    if (sched.Count > 0)
                    {
                        int i = 0;
                        var tprinc = 0.0;
                        while (amount > 0 && i < sched.Count)
                        {
                            var s = sched[i];
                            var princ = 0.0;
                            if (amount >= s.principalPayment)
                            {
                                princ = s.principalPayment;
                            }
                            else
                            {
                                princ = amount;
                            }
                            s.principalBalance += princ;
                            i++;
                            tprinc += princ;
                            amount = amount - princ;
                        }
                        ln.balance += tprinc;
                        le.loanRepayments.Remove(lrp);

                        coreLogic.jnl_batch jb2 = journalextensions.Post("LN",
                            ln.loanType.accountsReceivableAccountID, acctID, (tprinc),
                            "RVSSL: Principal Only Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        ent.jnl_batch.Add(jb2);

                    }
                    break;
                case "3":
                    sched = ln.repaymentSchedules.Where(p => p.interestBalance < p.interestPayment).OrderByDescending(p => p.repaymentDate).ToList();
                    if (sched.Count > 0)
                    {
                        int i = 0;
                        var tprinc = 0.0;
                        var tinterest = 0.0;
                        while (amount > 0 && i < sched.Count)
                        {
                            var s = sched[i];
                            var princ = 0.0;
                            var interest = 0.0;
                            if (amount >= s.interestPayment)
                            {
                                interest = s.interestPayment;
                            }
                            else
                            {
                                interest = amount;
                            }
                            s.principalBalance += princ;
                            s.interestBalance += interest;

                            i++;
                            tprinc += princ;
                            tinterest += interest;
                            amount -= princ + interest;
                        }
                        le.loanRepayments.Remove(lrp);

                        coreLogic.jnl_batch jb = journalextensions.Post("LN",
                            ln.loanType.interestIncomeAccountID, ln.loanType.unearnedInterestAccountID, lrp.amountPaid,
                            "RVSL: Interest Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        coreLogic.jnl_batch jb10 = journalextensions.Post("LN",
                            ln.loanType.accountsReceivableAccountID, acctID, lrp.amountPaid,
                            "RVSL: Interest Only Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j5 = jb10.jnl.ToList();
                        if (j5.Count > 1)
                        {
                            jb.jnl.Add(j5[0]);
                            jb.jnl.Add(j5[1]);
                        }
                        ent.jnl_batch.Add(jb);

                    }
                    break;
                case "6":
                    if (amount >= ln.processingFee)
                    {
                        ln.processingFeeBalance += ln.processingFee;
                        amount -= ln.processingFee;
                    }
                    else
                    {
                        ln.processingFeeBalance += amount;
                        amount = 0;
                    }
                    le.loanRepayments.Remove(lrp);

                    var jb3 = journalextensions.Post("LN",
                        ln.loanType.commissionAndFeesAccountID, ln.loanType.unpaidCommissionAccountID, lrp.amountPaid,
                        "RVSL: Processing Fees for " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    coreLogic.jnl_batch jb11 = journalextensions.Post("LN",
                            ln.loanType.accountsReceivableAccountID, acctID, lrp.amountPaid,
                            "RVSL: Processing fees for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var j6 = jb11.jnl.ToList();
                    if (j6.Count > 1)
                    {
                        jb3.jnl.Add(j6[0]);
                        jb3.jnl.Add(j6[1]);
                    }
                    ent.jnl_batch.Add(jb3);
                    //ln.loanFees.Load();

                    //var lf = ln.loanFees.FirstOrDefault();
                    //if (lf != null)
                    //{
                    //    le.loanFees.Remove(lf);
                    //}

                    break;
                case "5":
                    if (amount >= ln.applicationFee)
                    {
                        ln.applicationFeeBalance = ln.applicationFee;
                        amount -= ln.applicationFee;
                    }
                    else
                    {
                        ln.applicationFeeBalance += amount;
                        amount = 0;
                    }
                    le.loanRepayments.Remove(lrp);

                    var jb5 = journalextensions.Post("LN",
                        ln.loanType.commissionAndFeesAccountID, ln.loanType.unpaidCommissionAccountID, lrp.amountPaid,
                        "RVSL: Loan Application Fees Payment - "
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    coreLogic.jnl_batch jb6 = journalextensions.Post("LN",
                            ln.loanType.unpaidCommissionAccountID, acctID2, lrp.amountPaid,
                            "RVSL: Loan Repayment - "
                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var j2 = jb6.jnl.ToList();
                    if (j2.Count > 1)
                    {
                        jb5.jnl.Add(j2[0]);
                        jb5.jnl.Add(j2[1]);
                    }
                    ent.jnl_batch.Add(jb5);

                    break;
                case "4":
                    if (amount >= ln.commission)
                    {
                        ln.commissionBalance = ln.commission;
                        amount -= ln.commission;
                    }
                    else
                    {
                        ln.commissionBalance += amount;
                        amount = 0;
                    }
                    le.loanRepayments.Remove(lrp);

                    var jb4 = journalextensions.Post("LN",
                        ln.loanType.commissionAndFeesAccountID, ln.loanType.unpaidCommissionAccountID, lrp.amountPaid,
                        "RVSL: Loan Commission Payment - "
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    coreLogic.jnl_batch jb7 = journalextensions.Post("LN", acctID2,
                            ln.loanType.unpaidCommissionAccountID, lrp.amountPaid,
                            "RVSL: Loan Repayment - "
                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var j3 = jb7.jnl.ToList();
                    if (j3.Count > 1)
                    {
                        jb4.jnl.Add(j3[0]);
                        jb4.jnl.Add(j3[1]);
                    }
                    ent.jnl_batch.Add(jb4);

                    break;
                case "7":
                    var pen = ln.loanPenalties.Where(p => p.penaltyBalance < p.penaltyFee).OrderByDescending(p => p.penaltyDate).ToList();
                    if (pen.Count > 0)
                    {
                        int i = 0;
                        var tamount = 0.0;
                        while (amount > 0 && i < pen.Count)
                        {
                            var s = pen[i];
                            var amt = 0.0;
                            if (amount >= s.penaltyFee)
                            {
                                amt = s.penaltyFee;
                            }
                            else
                            {
                                amt = amount;
                            }
                            s.penaltyBalance += amt;

                            i++;
                            tamount += amt;
                            amount -= amt;
                        }
                        le.loanRepayments.Remove(lrp);

                        coreLogic.jnl_batch jb8 = journalextensions.Post("LN",
                            ln.loanType.interestIncomeAccountID, ln.loanType.unearnedInterestAccountID, tamount,
                            "RBSL: Additional Interest Payment - "
                            + ln.client.surName + ", " + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        coreLogic.jnl_batch jb9 = journalextensions.Post("LN",
                            ln.loanType.unearnedInterestAccountID, acctID, lrp.amountPaid,
                            "RVSL: Additional Interest Payment - "
                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j4 = jb9.jnl.ToList();
                        if (j4.Count > 1)
                        {
                            jb8.jnl.Add(j4[0]);
                            jb8.jnl.Add(j4[1]);
                        }
                        ent.jnl_batch.Add(jb8);

                    }
                    break;
            }
            foreach (var r in list)
            {
                ln.loanRepayments.Add(r);
            }
            if (tlrp != null)
            {
                le.cashierReceipts.Remove(tlrp);
            }

            return;
        }

        public void ReverseInterest(coreLoansEntities le, loan ln,
                coreLogic.core_dbEntities ent, loanPenalty lrp, string userName)
        {
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();

            var pro = ent.comp_prof.FirstOrDefault();

            var acctID = ln.loanType.vaultAccountID;
            var acctID2 = ln.loanType.vaultAccountID;
            var amount = lrp.penaltyFee;

            var jb = journalextensions.Post("LN", lrp.loan.loanType.unearnedInterestAccountID,
                lrp.loan.loanType.accountsReceivableAccountID, lrp.penaltyFee,
                "RVSL: Additional Interest for Loan - " + lrp.loan.client.surName + "," + lrp.loan.client.otherNames,
                pro.currency_id.Value, lrp.penaltyDate, lrp.loan.loanNo, ent, userName, ln.client.branchID);
            ent.jnl_batch.Add(jb);
            le.loanPenalties.Remove(lrp);

            return;
        }

        public void ReverseFee(coreLoansEntities le, loan ln,
                coreLogic.core_dbEntities ent, loanFee lrp, string userName)
        {
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();

            var pro = ent.comp_prof.FirstOrDefault();

            var acctID = ln.loanType.vaultAccountID;
            var acctID2 = ln.loanType.vaultAccountID;
            var amount = lrp.feeAmount;

            ln.processingFee -= lrp.feeAmount;
            ln.processingFeeBalance -= lrp.feeAmount;
            var jb2 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                        ln.loanType.accountsReceivableAccountID, lrp.feeAmount,
                        "RVSL: Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lrp.feeDate, ln.loanNo, ent, userName, ln.client.branchID);
            ent.jnl_batch.Add(jb2);
            le.loanFees.Remove(lrp);

            return;
        }

        public void ReverseDisbursement(coreLoansEntities le, loan ln, loanTranch lt, core_dbEntities ent,
            string userName)
        {
            IScheduleManager schedMgr = new ScheduleManager();
            //ln.loanFees.Load();
            var interest = 0.0;
            loanTranch lt2 = null;
            lt2 = le.loanTranches.FirstOrDefault(p => p.loanID == ln.loanID && p.loanTranchID != lt.loanTranchID);
            var tlrp = le.cashierDisbursements.Where(p => p.clientID == lt.loan.clientID && p.amount == lt.amountDisbursed && p.txDate == lt.disbursementDate).FirstOrDefault();
            var amount = lt.amountDisbursed;
            if (ln.addFeesToPrincipal)
            {
                amount -= ln.applicationFee + ln.processingFee;
                ln.applicationFeeBalance = 0;
                ln.processingFeeBalance = 0;
            }


            ln.amountDisbursed -= lt.amountDisbursed;
            ln.disbursementDate = (lt2 != null) ? (DateTime?)lt2.disbursementDate : null;
            ln.modification_date = DateTime.Now;
            ln.last_modifier = userName;
            ln.balance -= lt.amountDisbursed;
            ln.processingFeeBalance = ln.processingFee;

            if (lt2 == null)
            {
                ln.loanStatusID = 3;
            }

            interest = ln.repaymentSchedules.Sum(p => p.interestPayment);
            foreach (var rs in ln.repaymentSchedules.ToList())
            {
                le.repaymentSchedules.Remove(rs);
            }
            if (lt2 != null)
            {
                List<coreLogic.repaymentSchedule> sched =
                    schedMgr.calculateSchedule(lt2.amountDisbursed, ln.interestRate,
                    lt2.disbursementDate, ln.gracePeriod, ln.loanTenure,
                    ln.interestTypeID.Value, ln.repaymentModeID, ln.client);
                foreach (var rs in sched)
                {
                    rs.creation_date = DateTime.Now;
                    rs.creator = userName;
                    ln.repaymentSchedules.Add(rs);
                }
            }
            var acctID = ln.loanType.vaultAccountID;
            if ((lt.modeOfPaymentID == 2 || lt.modeOfPaymentID == 3) && lt.bankID == null)
            {
                var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1046");
                if (tlrp.cashiersTill != null)
                {
                    acctID = tlrp.cashiersTill.accountID;
                }
                else if (gl != null)
                {
                    acctID = gl.acct_id;
                }
            }
            else if ((lt.modeOfPaymentID == 2 || lt.modeOfPaymentID == 3) && lt.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == lt.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }

            var pro = ent.comp_prof.FirstOrDefault();
            var jb = journalextensions.Post("LN",
                acctID, ln.loanType.accountsReceivableAccountID, lt.amountDisbursed,
                "RVSL: Loan Disbursement Principal- "
                + " - " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, lt.disbursementDate, ln.loanNo, ent, userName, ln.client.branchID);
            interest -= ln.repaymentSchedules.Sum(p => p.interestPayment);
            if (interest > 0)
            {
                var jb2 = journalextensions.Post("LN",
                    ln.loanType.unearnedInterestAccountID, ln.loanType.accountsReceivableAccountID, interest,
                    "RVSL: Loan Disbursement Interest- "
                        + " - " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, lt.disbursementDate, ln.loanNo, ent, userName, ln.client.branchID);
                var list = jb2.jnl.ToList();
                jb.jnl.Add(list[0]);
                jb.jnl.Add(list[1]);
            }
            if (ln.processingFee > 0)
            {
                var jb3 = journalextensions.Post("LN",
                    ln.loanType.unpaidCommissionAccountID, ln.loanType.accountsReceivableAccountID,
                    ln.processingFee,
                    "RVSL: Loan Disbursement Fees- "
                    + " - " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, lt.disbursementDate, ln.loanNo, ent, userName, ln.client.branchID);
                var list2 = jb3.jnl.ToList();
                jb.jnl.Add(list2[0]);
                jb.jnl.Add(list2[1]);
            }
            ent.jnl_batch.Add(jb);
            if (tlrp != null)
            {
                le.cashierDisbursements.Remove(tlrp);
            }
            le.loanTranches.Remove(lt);
            var lf = ln.loanFees.FirstOrDefault();
            if (lf != null)
            {
                le.loanFees.Remove(lf);
            }
        }

        public void ReverseInsurance(coreLoansEntities le, loan ln, core_dbEntities ent, loanInsurance lins, string userName)
        {
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();

            var pro = ent.comp_prof.FirstOrDefault();

            //var acctID = ln.loanType.vaultAccountID;
            //var acctID2 = ln.loanType.vaultAccountID;
            //var amount = lins.amount;

            //ln.insuranceAmount -= amount;
            //ln.insuranceAmount -= amount;
            var jb2 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                        ln.loanType.accountsReceivableAccountID, lins.amount,
                        "RVSL: Loan Insurance Fees- " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lins.insuranceDate, ln.loanNo, ent, userName, ln.client.branchID);
            ent.jnl_batch.Add(jb2);
            le.loanInsurances.Remove(lins);

            return;
        }
    }
}
