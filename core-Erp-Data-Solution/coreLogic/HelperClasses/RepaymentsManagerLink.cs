using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace coreLogic
{
    public class RepaymentsManagerLink : IRepaymentsManager  
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        public string ReceivePayment(coreLoansEntities le, loan ln,
            double amountPaid, DateTime paymentDate, string paymentTypeID,
            string bID, string bankName, string checkNo,
                coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID)
        {
            jnl_batch batch = null;
            return ReceivePayment(le, ln,
             amountPaid, paymentDate, paymentTypeID,
             bID, bankName, checkNo,
                 ent, userName, modeOfPaymentID, 0, null, ref batch);
        }
        public string ReceivePayment(coreLoansEntities le, loan ln,
            double amountPaid, DateTime paymentDate, string paymentTypeID,
            string bID, string bankName, string checkNo,
                coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID, double amt2,
            int? accountID, ref jnl_batch batch)
        {
            var amount = amountPaid;
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();
            int? bankID = null;
            if (bID != "" && bID != null) bankID = int.Parse(bID);
            loanRepayment repayment = null;
            string batchNo = null;

            var pro = ent.comp_prof.FirstOrDefault();
            saving sav = le.savings.FirstOrDefault(p => p.clientID == ln.clientID
                && (p.principalBalance + p.interestBalance >= amountPaid));

            var acctID = ln.loanType.vaultAccountID;
            var acctID2 = ln.loanType.vaultAccountID;
            if (modeOfPaymentID == 4 && sav != null && paymentTypeID == "1")
            {
                acctID = sav.savingType.accountsPayableAccountID.Value;
            }
            else if ((modeOfPaymentID == 2 || modeOfPaymentID == 3) && bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                if (ba != null)
                {
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            else if (accountID != null)
            {
                acctID = accountID.Value;
            }
            else if ((modeOfPaymentID == 2 || modeOfPaymentID == 3) && bankID == null)
            {
                var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1046");
                if (gl != null)
                {
                    acctID = gl.acct_id;
                }
                gl = ent.accts.FirstOrDefault(p => p.acc_num == "1047");
                if (gl != null)
                {
                    acctID2 = gl.acct_id;
                }
            }

            switch (paymentTypeID)
            {
                case "1":
                    ReceiveInterestPlusPrincipal(ln, paymentDate, amountPaid,
                        amount, modeOfPaymentID, sav, le, ent,
                        userName, repayment, paymentTypeID, checkNo,
                        acctID, bankID, bankName, list,
                        pro, ref batch, ref batchNo);
                    break;
                case "2":
                    ReceivePrincipalOnly(ln, paymentDate, amountPaid,
                        amount, modeOfPaymentID, sav, le, ent,
                        userName, repayment, paymentTypeID, checkNo,
                        acctID, bankID, bankName, list,
                        pro, ref batch, ref batchNo, ref amt2);
                    break;
                case "3":
                    ReceiveInterestOnly(ln, paymentDate, amountPaid,
                        amount, modeOfPaymentID, sav, le, ent,
                        userName, repayment, paymentTypeID, checkNo,
                        acctID, bankID, bankName, list,
                        pro, ref batch, ref batchNo, ref amt2);
                    break;
                case "6":
                    ReceiveProcessingFee(ln, paymentDate, amountPaid,
                        amount, modeOfPaymentID, sav, le, ent,
                        userName, repayment, paymentTypeID, checkNo,
                        acctID, bankID, bankName, list,
                        pro, ref batch, ref batchNo, ref amt2);
                    break;
                case "5":
                    ReceiveApplicationFee(ln, paymentDate, amountPaid,
                        amount, modeOfPaymentID, sav, le, ent,
                        userName, repayment, paymentTypeID, checkNo,
                        acctID, bankID, bankName, list,
                        pro, ref batch, ref batchNo, ref amt2);
                    break;
                case "4":
                    ReceiveCommission(ln, paymentDate, amountPaid,
                        amount, modeOfPaymentID, sav, le, ent,
                        userName, repayment, paymentTypeID, checkNo,
                        acctID, bankID, bankName, list,
                        pro, ref batch, ref batchNo, ref amt2);
                    break;
                case "7":
                    ReceivePenalty(ln, paymentDate, amountPaid,
                        amount, modeOfPaymentID, sav, le, ent,
                        userName, repayment, paymentTypeID, checkNo,
                        acctID, bankID, bankName, list,
                        pro, ref batch, ref batchNo, ref amt2);
                    break;
            }
            foreach (var r in list)
            {
                ln.loanRepayments.Add(r);
            }

            return batchNo;
        }

        public string ReceivePayment(coreLoansEntities le, loan ln,
            double amountPaid, DateTime paymentDate, string bID, string bankName, string checkNo,
                coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID,
            int? accountID, repaymentSchedule sch, ref jnl_batch batch)
        {
            var amount = amountPaid;
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();
            int? bankID = null;
            if (bID != "" && bID != null) bankID = int.Parse(bID);
            loanRepayment repayment = null;
            string batchNo = null;

            var pro = ent.comp_prof.FirstOrDefault();

            var acctID = ln.loanType.vaultAccountID;
            if (accountID != null)
            {
                acctID = accountID.Value;
            }
            else if ((modeOfPaymentID == 2 || modeOfPaymentID == 3) && bankID == null)
            {
                var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1046");
                if (gl != null)
                {
                    acctID = gl.acct_id;
                }
            }
            else if ((modeOfPaymentID == 2 || modeOfPaymentID == 3) && bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                if (ba != null)
                {
                    ////ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }

            var iTotal = Math.Ceiling((ln.amountDisbursed) * (ln.interestRate / 100.0)
                                * (paymentDate - ln.disbursementDate.Value).Days / 30.0);
            var iPaid = ln.repaymentSchedules.Sum(p => p.interestPayment - p.interestBalance);
            var pBal = ln.repaymentSchedules.Sum(p => p.principalBalance);
            var iBal = ln.repaymentSchedules.Sum(p => p.interestBalance);

            var sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0 || p.principalBalance > 0).OrderBy(p => p.repaymentDate).ToList();
            var totalInt = ln.repaymentSchedules.Sum(p => p.interestPayment);
            var totalPrinc = ln.repaymentSchedules.Sum(p => p.principalPayment);
            var intRatio = totalInt / (totalInt + totalPrinc);
            var tinterest = 0.0;
            var tprinc = 0.0;
            if (iBal > 0)
            {
                tinterest = Math.Round(intRatio * amountPaid, 2);
                tprinc = amountPaid - tinterest;
                if (tprinc > pBal)
                {
                    tprinc = pBal;
                    tinterest = amount - tprinc;
                }
            }
            else
            {
                tinterest = 0.0;
                tprinc = amountPaid - tinterest;
            }
            var amt = amountPaid;
            amount = tprinc;
            var amount2 = tinterest;
            foreach (var s in sched)
            {
                int i = 0;
                var princ = 0.0;
                var interest = 0.0;
                if (pBal + (iTotal - iPaid) > amountPaid)
                {
                    if (amount + amount2 >= s.principalBalance + s.interestBalance)
                    {
                        princ = s.principalBalance;
                        interest = s.interestBalance;
                    }
                    else if (amount + amount2 >= s.principalBalance)
                    {
                        princ = s.principalBalance;
                        interest = amount + amount2 - s.principalBalance;
                    }
                    else
                    {
                        princ = amount + amount2;
                    }
                    s.principalBalance -= princ;
                    s.interestBalance -= interest;
                }
                else
                {
                    if (amount >= s.principalBalance)
                    {
                        princ = s.principalBalance;
                    }
                    else
                    {
                        princ = amount;
                    }
                    if (amount2 >= s.interestBalance)
                    {
                        interest = s.interestBalance;
                    }
                    else
                    {
                        interest = amount;
                    }
                    s.principalBalance -= princ;
                    s.interestBalance -= interest;
                }
                i++;
                //tprinc += princ;
                //tinterest += interest;
                amount = amount - princ;
                amount2 = amount2 - interest;
                amt = amt - princ - interest;
                if (amt <= 0.1)
                {
                    break;
                }
            }
            repayment = new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = tinterest,
                principalPaid = tprinc,
                repaymentDate = paymentDate,
                modeOfPaymentID = modeOfPaymentID,
                commission_paid = 0,
                repaymentTypeID = 1,
                checkNo = checkNo,
                bankID = bankID,
                bankName = bankName
            };
            ln.balance -= tprinc;
            list.Add(repayment);

            coreLogic.jnl_batch jb = journalextensions.Post("LN", ln.loanType.unearnedInterestAccountID,
                ln.loanType.interestIncomeAccountID, tinterest,
                "Loan repayment for " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
            coreLogic.jnl_batch jb2 = journalextensions.Post("LN", acctID,
                ln.loanType.accountsReceivableAccountID, amountPaid,
                "Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
            var j = jb2.jnl.ToList();
            if (j.Count > 1)
            {
                if (batch != null)
                {
                    var j1 = j.FirstOrDefault(p => p.accts.acct_id == acctID && p.ref_no == ln.loanNo);
                    var j3 = j.FirstOrDefault(p => p.accts.acct_id == ln.loanType.accountsReceivableAccountID && p.ref_no == ln.loanNo);
                    var j4 = jb.jnl.FirstOrDefault(p => p.accts.acct_id == ln.loanType.interestIncomeAccountID && p.ref_no == ln.loanNo);
                    var j5 = jb.jnl.FirstOrDefault(p => p.accts.acct_id == ln.loanType.unearnedInterestAccountID && p.ref_no == ln.loanNo);
                    var j2 = batch.jnl.FirstOrDefault(p => p.accts.acct_id == acctID);
                    j2.crdt_amt += j1.crdt_amt;
                    j2.dbt_amt += j1.dbt_amt;
                    ent.Entry(j1).State = System.Data.Entity.EntityState.Detached;
                    batch.jnl.Add(j3);
                    batch.jnl.Add(j4);
                    batch.jnl.Add(j5);
                }
                else
                {
                    jb.jnl.Add(j[0]);
                    jb.jnl.Add(j[1]);
                }
            }
            if (batch == null)
            {
                ent.jnl_batch.Add(jb);
                batch = jb;
            }

            batchNo = jb.batch_no;

            foreach (var r in list)
            {
                ln.loanRepayments.Add(r);
            }

            return batchNo;
        }

        public void ApplyCheck(coreLoansEntities le, int loanCheckID, string userName,
            int bankID, DateTime cashDate, loan ln)
        {

            var check = le.loanChecks.FirstOrDefault(p => p.loanCheckID == loanCheckID);
            if (check != null)
            {
                check.cashed = true;
                check.cashDate = cashDate;
                core_dbEntities ent = new core_dbEntities();
                ReceivePayment(le, ln, check.checkAmount, cashDate, "1", bankID.ToString(), "",
                    check.checkNumber,
                    ent, userName, 2);
                le.SaveChanges();
                ent.SaveChanges();

            }
        }

        public void WriteOffInterest(coreLoansEntities le, core_dbEntities ent,
            loan ln, string userName)
        {
            jnl_batch batch = null;
            var pro = ent.comp_prof.FirstOrDefault();
            var rps = ln.repaymentSchedules.Where(p => p.interestBalance > 0);
            foreach (var rp in rps)
            {
                rp.proposedInterestWriteOff = rp.interestBalance;
                /*
                    if (batch == null)
                    {
                        batch = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                            rp.loan.loanType.accountsReceivableAccountID, rp.proposedInterestWriteOff,
                            "Loan Interest Write off - " + rp.proposedInterestWriteOff.ToString("#,###.#0")
                            + " - " + rp.loan.client.accountNumber + " - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                            pro.currency_id.Value, DateTime.Now, rp.loan.loanNo, ent, userName);
                    }
                    else
                    {
                        var batch2 = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                            rp.loan.loanType.accountsReceivableAccountID, rp.proposedInterestWriteOff,
                            "Loan Interest Write off - " + rp.proposedInterestWriteOff.ToString("#,###.#0")
                            + " - " + rp.loan.client.accountNumber + " - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                            pro.currency_id.Value, DateTime.Now, rp.loan.loanNo, ent, userName);
                        var j = batch2.jnl.ToList();
                        if (j.Count > 1)
                        {
                            batch.jnl.Add(j[0]);
                            batch.jnl.Add(j[1]);
                        }
                    }*/
            }
            if (batch != null
                )
            {
                ent.jnl_batch.Add(batch);
            }
        }

        public void ClearOffInterest(coreLoansEntities le, core_dbEntities ent,
            loan ln, string userName)
        {
            jnl_batch batch = null;
            var pro = ent.comp_prof.FirstOrDefault();
            var rps = ln.repaymentSchedules.Where(p => p.interestBalance > 0);
            foreach (var rp in rps)
            {
                if (ln.balance < 2)
                {
                    rp.proposedInterestWriteOff = 0;
                }
                /*
                    if (batch == null)
                    {
                        batch = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                            rp.loan.loanType.accountsReceivableAccountID, rp.proposedInterestWriteOff,
                            "Loan Interest Write off - " + rp.proposedInterestWriteOff.ToString("#,###.#0")
                            + " - " + rp.loan.client.accountNumber + " - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                            pro.currency_id.Value, DateTime.Now, rp.loan.loanNo, ent, userName);
                    }
                    else
                    {
                        var batch2 = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                            rp.loan.loanType.accountsReceivableAccountID, rp.proposedInterestWriteOff,
                            "Loan Interest Write off - " + rp.proposedInterestWriteOff.ToString("#,###.#0")
                            + " - " + rp.loan.client.accountNumber + " - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                            pro.currency_id.Value, DateTime.Now, rp.loan.loanNo, ent, userName);
                        var j = batch2.jnl.ToList();
                        if (j.Count > 1)
                        {
                            batch.jnl.Add(j[0]);
                            batch.jnl.Add(j[1]);
                        }
                    }*/
            }
            if (batch != null
                )
            {
                ent.jnl_batch.Add(batch);
            }
        }

        public void CreatePenalty(coreLoansEntities le, core_dbEntities ent,
            loan ln, string userName, double amount, DateTime date)
        {
            ln.loanPenalties.Add(new loanPenalty
            {
                proposedAmount = amount,
                creation_date = DateTime.Now,
                creator = userName,
                loanID = ln.loanID,
                penaltyBalance = amount,
                penaltyDate = date,
                penaltyFee = amount
            });
        }

        public void CashierReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName)
        {
            int? crAccNo = null;
            if (r.paymentModeID == 1)
            {
                var acc = ent.accts.FirstOrDefault(p => p.acct_id == r.cashiersTill.accountID);
                crAccNo = acc.acct_id;
                r.tillAccountID = crAccNo;
            }
            else if (r.paymentModeID == 2)
            {
                return;
            }
            else if (r.paymentModeID == 4)
            {
                var sv = le.savings.FirstOrDefault(p => p.clientID == ln.clientID);
                if (sv != null && sv.savingType.accountsPayableAccountID != null
                    && sv.principalBalance + sv.interestBalance >= r.amount
                    )
                {
                    crAccNo = sv.savingType.accountsPayableAccountID;
                }
            }

            jnl_batch batch = null;
            if (r.principalAmount == 0 && r.interestAmount == 0 & r.feeAmount == 0 && r.addInterestAmount == 0)
            {
                string batchNo = this.ReceivePayment(le, ln, r.amount,
                    r.txDate, r.repaymentTypeID.ToString(), (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                r.batchNo1 = batchNo;
            }
            else
            {
                if (r.principalAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.principalAmount,
                        r.txDate, "2", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                        r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo2 = batchNo;
                }
                if (r.interestAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.interestAmount,
                        r.txDate, "3", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo3 = batchNo;
                }
                if (r.feeAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.feeAmount,
                        r.txDate, "6", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo4 = batchNo;
                    if (ln.loanTypeID == 5)
                    {
                        var pro = ent.comp_prof.FirstOrDefault();
                        var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                           ln.loanType.unpaidCommissionAccountID, ln.processingFee,
                           "Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                           pro.currency_id.Value, ln.disbursementDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                        ent.jnl_batch.Add(jb2);
                    }
                }
                if (r.addInterestAmount > 0)
                {
                    if (ln.loanPenalties.FirstOrDefault(p => p.penaltyBalance == r.addInterestAmount) == null)
                    {
                        ln.loanPenalties.Add(new loanPenalty
                        {
                            proposedAmount = 0,
                            penaltyFee = r.addInterestAmount,
                            loan = ln,
                            creation_date = DateTime.Now,
                            creator = userName,
                            penaltyBalance = r.addInterestAmount,
                            penaltyDate = r.txDate,
                            loanID = ln.loanID
                        });
                        var pro = ent.comp_prof.FirstOrDefault();
                        var jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                 ln.loanType.unearnedInterestAccountID, r.addInterestAmount,
                                 "Additional Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                 pro.currency_id.Value, r.txDate, ln.loanNo, ent, userName, ln.client.branchID);
                        ent.jnl_batch.Add(jb);
                    }
                    string batchNo = this.ReceivePayment(le, ln, r.addInterestAmount,
                        r.txDate, "7", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo5 = batchNo;
                }
            }
        }

        public void CloseCashierReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName, ref jnl_batch batch)
        {
            var credit = 0.0;
            var debit = 0.0;
            string description = "";

            if (r.batchNo1 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo1);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }
            else if (r.batchNo2 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo2);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }
            else if (r.batchNo3 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo3);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }
            else if (r.batchNo4 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo4);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }
            else if (r.batchNo5 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo5);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }

            var prof = ent.comp_prof.First();
            if ((credit > 0 || debit > 0) && prof.comp_name.ToLower().Contains("jireh")==false)
            {
                var amount = debit - credit;
                if (batch == null)
                {
                    if (amount > 0)
                        batch = journalextensions.Post("LN", ln.loanType.vaultAccountID, r.tillAccountID.Value, amount,
                            "Daily Posting - " + r.cashiersTill.userName + " - " + r.txDate.ToString("dd-MMM-yyyy"),
                            prof.currency_id.Value, r.txDate, r.cashiersTill.userName, ent, userName, ln.client.branchID);
                    else
                        batch = journalextensions.Post("LN", r.tillAccountID.Value, ln.loanType.vaultAccountID, -amount,
                            "Daily Posting - " + r.cashiersTill.userName + " - " + r.txDate.ToString("dd-MMM-yyyy"),
                            prof.currency_id.Value, r.txDate, r.cashiersTill.userName, ent, userName, ln.client.branchID);

                }
                else
                {
                    jnl j = journalextensions.Post("LN", "CR", r.tillAccountID.Value, amount,
                        description,
                        prof.currency_id.Value, r.txDate, r.cashiersTill.userName, ent, userName, ln.client.branchID);
                    batch.jnl.Add(j);
                    j = batch.jnl.FirstOrDefault(p => p.accts.acct_id == ln.loanType.vaultAccountID);
                    if (j == null)
                    {
                        j = journalextensions.Post("LN", "DR", ln.loanType.vaultAccountID, amount,
                           "Daily Posting - " + r.cashiersTill.userName + " - " + r.txDate.ToString("dd-MMM-yyyy"),
                           prof.currency_id.Value, r.txDate, r.cashiersTill.userName, ent, userName, ln.client.branchID);
                        batch.jnl.Add(j);
                    }
                    else
                    {
                        j.dbt_amt += amount;
                    }
                }
            }
        }


        //For Multi-Payments
        public void CashierCheckReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName,
            int? crAccNo, ref jnl_batch batch)
        {
            multiPaymentClient mpc = (
                               from m in le.multiPaymentClients
                               from r2 in le.cashierReceipts
                               where r2.txDate == r.txDate
                                   && r2.clientID == r.clientID
                                   && m.cashierReceiptID == r2.cashierReceiptID
                               select m
                           ).FirstOrDefault();

            var acc = ent.def_accts.FirstOrDefault(p => p.code == "RF");
            int? ba = null;
            var b = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == r.bankID);
            if (b != null)
            {
                ba = b.accts.acct_id;
                crAccNo = ba;
            }
            if (r.principalAmount == 0 && r.interestAmount == 0 & r.feeAmount == 0 && r.addInterestAmount == 0)
            {
                string batchNo = this.ReceivePayment(le, ln, r.amount,
                    r.txDate, r.repaymentTypeID.ToString(), (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                r.batchNo1 = batchNo;
            }
            else
            {
                if (r.principalAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.principalAmount,
                        r.txDate, "2", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                        r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo2 = batchNo;
                }
                if (r.interestAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.interestAmount,
                        r.txDate, "3", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo3 = batchNo;
                }
                if (r.feeAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.feeAmount,
                        r.txDate, "6", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID,0, crAccNo, ref batch);
                    r.batchNo4 = batchNo;
                }
                if (r.addInterestAmount > 0)
                {
                    jnl_batch jb = null;
                    if (ln.loanPenalties.FirstOrDefault(p => p.penaltyBalance == r.addInterestAmount)
                        == null)
                    {
                        ln.loanPenalties.Add(new loanPenalty
                        {
                            proposedAmount = 0,
                            penaltyFee = r.addInterestAmount,
                            loan = ln,
                            creation_date = DateTime.Now,
                            creator = userName,
                            penaltyBalance = r.addInterestAmount,
                            penaltyDate = r.txDate,
                            loanID = ln.loanID
                        });
                        var pro = ent.comp_prof.FirstOrDefault();
                        jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                 ln.loanType.unearnedInterestAccountID, r.addInterestAmount,
                                 "Additional Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                 pro.currency_id.Value, r.txDate, ln.loanNo, ent, userName, ln.client.branchID);
                    }
                    string batchNo = this.ReceivePayment(le, ln, r.addInterestAmount,
                        r.txDate, "7", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    if (jb != null && batch != null)
                    {
                        var js = jb.jnl.ToList();
                        batch.jnl.Add(js[0]);
                        batch.jnl.Add(js[1]);
                        ent.Entry(jb).State = System.Data.Entity.EntityState.Detached;
                    }
                    r.batchNo5 = batchNo;
                }
            }
            if (batch != null && ba != null && acc != null)
            {
                var js = batch.jnl.Where(p => p.accts.acct_id == ba
                                && p.description.Contains("Payment made by") == false).ToList();
                //foreach (var j2 in js)
                //{
                //    ent.Entry(j2).State = System.Data.Entity.EntityState.Detached;
                //}
            }
        }

        //For Regular Cashier Payments
        public void CashierCheckReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName)
        {
            int? crAccNo = null;
            if (r.paymentModeID == 1)
            { 
                return;
            } 

            jnl_batch batch = null;
            if (r.repaymentTypeID == 1)
            {
                string batchNo = this.ReceivePayment(le, ln, r.amount,
                    r.txDate, r.repaymentTypeID.ToString(), (r.bankID == null) ? "" : r.bankID.ToString(), "",
                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                r.batchNo1 = batchNo;
            }
            else
            {
                if (r.principalAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.principalAmount,
                        r.txDate, "2", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                        r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo2 = batchNo;
                }
                if (r.interestAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.interestAmount,
                        r.txDate, "3", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                        r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo3 = batchNo;
                }
                if (r.feeAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.feeAmount,
                        r.txDate, "6", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                        r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo4 = batchNo;
                    if (ln.loanTypeID == 5)
                    {
                        var pro = ent.comp_prof.FirstOrDefault();
                        var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                            ln.loanType.unpaidCommissionAccountID, ln.processingFee,
                            "Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, ln.disbursementDate.Value, ln.loanNo, ent, userName,
                            ln.client.branchID);
                        ent.jnl_batch.Add(jb2);
                    }
                }
                if (r.addInterestAmount > 0)
                {
                    if (ln.loanPenalties.FirstOrDefault(p => p.penaltyBalance == r.addInterestAmount) == null)
                    {
                        ln.loanPenalties.Add(new loanPenalty
                        {
                            proposedAmount = 0,
                            penaltyFee = r.addInterestAmount,
                            loan = ln,
                            creation_date = DateTime.Now,
                            creator = userName,
                            penaltyBalance = r.addInterestAmount,
                            penaltyDate = r.txDate,
                            loanID = ln.loanID
                        });
                        var pro = ent.comp_prof.FirstOrDefault();
                        var jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                            ln.loanType.unearnedInterestAccountID, r.addInterestAmount,
                            "Additional Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, r.txDate, ln.loanNo, ent, userName, ln.client.branchID);
                        ent.jnl_batch.Add(jb);
                    }
                    string batchNo = this.ReceivePayment(le, ln, r.addInterestAmount,
                        r.txDate, "7", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                        r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo5 = batchNo;
                }
            }
        }

        public void ApplyNegativeBalanceToLoan(loan ln, coreLoansEntities le, core_dbEntities ent,
            double amountPaid, DateTime paymentDate, string userName, loan ln2)
        {
            var amount = -amountPaid;
            if (amount < 0) return;
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();

            var iTotal = Math.Ceiling((ln.amountDisbursed) * (ln.interestRate / 100.0)
                                * (paymentDate - ln.disbursementDate.Value).Days / 30.0);
            var iPaid = ln.repaymentSchedules.Sum(p => p.interestPayment - p.interestBalance);
            var pBal = ln.repaymentSchedules.Sum(p => p.principalBalance);
            var iBal = ln.repaymentSchedules.Sum(p => p.interestBalance);
            var rem = pBal + (iTotal - iPaid);
            var pBal2 = ln.repaymentSchedules.Where(p => p.repaymentDate <= paymentDate).Sum(p => p.principalBalance);
            var iBal2 = ln.repaymentSchedules.Where(p => p.repaymentDate <= paymentDate).Sum(p => p.interestBalance);

            var sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0 || p.principalBalance > 0).OrderBy(p => p.repaymentDate).ToList();
            var totalInt = ln.repaymentSchedules.Sum(p => p.interestPayment);
            var totalPrinc = ln.repaymentSchedules.Sum(p => p.principalPayment);
            var intRatio = totalInt / (totalInt + totalPrinc);
            var tinterest = 0.0;
            var tprinc = 0.0;
            var tprinc2 = 0.0;
            var tinterest2 = 0.0;
            if (iBal2 > 0)
            {
                tinterest = Math.Round(intRatio * amountPaid, 2);
                tprinc = amountPaid - tinterest;
                if (tinterest > iBal2)
                {
                    tinterest = iBal2;
                    tprinc = amountPaid - tinterest;
                }
                else if (tprinc > pBal)
                {
                    tprinc = pBal;
                    tinterest = amountPaid - tprinc;
                }
            }
            else if (amountPaid >= pBal)
            {
                tprinc = pBal;
                tinterest = amountPaid - tprinc;
            }
            else
            {
                tinterest = 0.0;
                tprinc = amountPaid - tinterest;
            }
            if (sched.Count > 0)
            {
                int i = 0;
                amount = tprinc;
                var amount2 = tinterest;
                while ((amount > 0 || amount2 > 0) && i < sched.Count)
                {
                    var s = sched[i];
                    var princ = 0.0;
                    var interest = 0.0;

                    if (amount >= s.principalBalance)
                    {
                        princ = s.principalBalance;
                    }
                    else
                    {
                        princ = amount;
                    }
                    if (amount2 >= s.interestBalance)
                    {
                        interest = s.interestBalance;
                    }
                    else
                    {
                        interest = amount2;
                    }
                    s.principalBalance -= princ;
                    s.interestBalance -= interest;

                    if (interest > s.additionalInterestBalance) s.additionalInterestBalance = 0;
                    else if (s.additionalInterestBalance != null) s.additionalInterestBalance -= interest;

                    tprinc2 += princ;
                    tinterest2 += interest;

                    i++;
                    amount = amount - princ;
                    amount2 = amount2 - interest;
                }
            }
            ln2.loanRepayments.Add(new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = tinterest2,
                principalPaid = tprinc2,
                repaymentDate = paymentDate,
                modeOfPaymentID = 6,
                commission_paid = 0,
                repaymentTypeID = 1,
                checkNo = "",
                bankID = null,
                bankName = ""
            });
            ln.loanRepayments.Add(new coreLogic.loanRepayment
            {
                amountPaid = -amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = -tinterest2,
                principalPaid = -tprinc2,
                repaymentDate = paymentDate,
                modeOfPaymentID = 6,
                commission_paid = 0,
                repaymentTypeID = 1,
                checkNo = "",
                bankID = null,
                bankName = ""
            });
            ln.balance -= tprinc2;

        }

        private void ReceiveInterestPlusPrincipal(loan ln, DateTime paymentDate, double amountPaid,
            double amount, int modeOfPaymentID, saving sav, coreLoansEntities le, core_dbEntities ent,
            string userName, loanRepayment repayment, string paymentTypeID, string checkNo,
            int acctID, int? bankID, string bankName, List<loanRepayment> list,
            comp_prof pro, ref jnl_batch batch, ref string batchNo)
        {
            if (ln.loanStatusID != 4) return;
            var iTotal = Math.Ceiling((ln.amountDisbursed) * (ln.interestRate / 100.0)
                                        * (paymentDate - ln.disbursementDate.Value).Days / 30.0);
            var iPaid = ln.repaymentSchedules.Sum(p => p.interestPayment - p.interestBalance);
            var pBal = ln.repaymentSchedules.Sum(p => p.principalBalance);
            var iBal = ln.repaymentSchedules.Sum(p => p.interestBalance);
            var rem = pBal + (iTotal - iPaid);
            var pBal2 = ln.repaymentSchedules.Where(p => p.repaymentDate <= paymentDate).Sum(p => p.principalBalance);
            var iBal2 = ln.repaymentSchedules.Where(p => p.repaymentDate <= paymentDate).Sum(p => p.interestBalance);

            var sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0 || p.principalBalance > 0).OrderBy(p => p.repaymentDate).ToList();
            var totalInt = ln.repaymentSchedules.Sum(p => p.interestPayment);
            var totalPrinc = ln.repaymentSchedules.Sum(p => p.principalPayment);
            var intRatio = totalInt / (totalInt + totalPrinc);
            var tinterest = 0.0;
            var tprinc = 0.0;
            var tprinc2 = 0.0;
            var tinterest2 = 0.0;
            //if (iBal2 > 0)
            //{
                tinterest = Math.Round(intRatio * amountPaid, 2);
                tprinc = amountPaid - tinterest;
                if (tinterest > iBal2)
                {
                    tinterest = iBal2;
                    tprinc = amountPaid - tinterest;
                }
                else if (tprinc > pBal)
                {
                    tprinc = pBal;
                    tinterest = amountPaid - tprinc;
                }
            //}
            //else if (amountPaid >= pBal)
            //{
            //    tprinc = pBal;
            //    tinterest = amountPaid - tprinc;
            //}
            //else
            //{
            //    tinterest = 0.0;
            //    tprinc = amountPaid - tinterest;
            //}
            if (sched.Count > 0)
            {
                int i = 0;
                amount = tprinc;
                var amount2 = tinterest;
                while ((amount > 0 || amount2 > 0) && i < sched.Count)
                {
                    var s = sched[i];
                    var princ = 0.0;
                    var interest = 0.0;

                    if (amount >= s.principalBalance)
                    {
                        princ = s.principalBalance;
                    }
                    else
                    {
                        princ = amount;
                    }
                    if (amount2 >= s.interestBalance)
                    {
                        interest = s.interestBalance;
                    }
                    else
                    {
                        interest = amount2;
                    }
                    s.principalBalance -= princ;
                    s.interestBalance -= interest;

                    if (interest > s.additionalInterestBalance) s.additionalInterestBalance = 0;
                    else if (s.additionalInterestBalance != null) s.additionalInterestBalance -= interest;

                    tprinc2 += princ;
                    tinterest2 += interest;
                    if (sav != null && modeOfPaymentID == 4)
                    {
                        sav.principalBalance -= princ;
                        sav.interestBalance -= interest;
                        sav.savingWithdrawals.Add(new savingWithdrawal
                        {
                            localAmount = princ + interest,
                            creation_date = DateTime.Now,
                            creator = userName,
                            fxRate = 1,
                            interestBalance = sav.interestBalance,
                            principalBalance = sav.principalBalance,
                            interestWithdrawal = interest,
                            principalWithdrawal = princ,
                            posted = false,
                            modeOfPaymentID = 4,
                            naration = "Deduction to pay for loan",
                            savingID = sav.savingID,
                            withdrawalDate = paymentDate
                        });
                    }
                    i++;
                    amount = amount - princ;
                    amount2 = amount2 - interest;
                }
                if (pBal + (iTotal - iPaid) <= amountPaid)
                {
                    WriteOffInterest(le, ent, ln, userName);
                }
            }
            repayment = new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = tinterest2,
                principalPaid = tprinc2,
                repaymentDate = paymentDate,
                modeOfPaymentID = modeOfPaymentID,
                commission_paid = 0,
                repaymentTypeID = int.Parse(paymentTypeID),
                checkNo = checkNo,
                bankID = bankID,
                bankName = bankName
            };
            ln.balance -= tprinc2;
            list.Add(repayment);

            coreLogic.jnl_batch jb = journalextensions.Post("LN", acctID,
                ln.loanType.accountsReceivableAccountID, amountPaid,
                "Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
            string startNormalDate = System.Configuration.ConfigurationManager.AppSettings["startCashAccountingLoans"];
            if (startNormalDate == null) startNormalDate = "";
            DateTime startCashAccountingLoans = DateTime.MinValue;

            var postUnEarned = (!le.loanConfigs.Any() ||
                                (DateTime.TryParseExact(startNormalDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture,
                                    DateTimeStyles.AllowInnerWhite,
                                    out startCashAccountingLoans) && startCashAccountingLoans > DateTime.MinValue
                                 && startCashAccountingLoans <= ln.disbursementDate.Value.Date));

            if (( le.configs.Any() && le.configs.FirstOrDefault().postInterestUnIntOnDisb)
                || postUnEarned)
            {
                coreLogic.jnl_batch jb2 = journalextensions.Post("LN", ln.loanType.unearnedInterestAccountID,
                    ln.loanType.interestIncomeAccountID, tinterest2,
                    "Loan repayment for " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                var j = jb2.jnl.ToList();
                if (j.Count > 1)
                {
                    jb.jnl.Add(j[0]);
                    jb.jnl.Add(j[1]);
                }
            }
            else
            {

            }
            var jar = jb.jnl.ToList();
            if (batch != null)
            {
                foreach (var j in jar)
                {
                    batch.jnl.Add(j);
                }
                ent.Entry(jb).State = System.Data.Entity.EntityState.Detached;
            }
            else
            {
                ent.jnl_batch.Add(jb);
                batch = jb;
            }
            batchNo = batch.batch_no;
        }

        private void ReceivePrincipalOnly(loan ln, DateTime paymentDate, double amountPaid,
            double amount, int modeOfPaymentID, saving sav, coreLoansEntities le, core_dbEntities ent,
            string userName, loanRepayment repayment, string paymentTypeID, string checkNo,
            int acctID, int? bankID, string bankName, List<loanRepayment> list,
            comp_prof pro, ref jnl_batch batch, ref string batchNo, ref double amt2)
        {
            if (ln.loanStatusID != 4) return;
            var tprinc = 0.0;
            var sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0 || p.principalBalance > 0).OrderBy(p => p.repaymentDate).ToList();
            if (sched.Count > 0)
            {
                int i = 0;
                while (amount > 0 && i < sched.Count)
                {
                    var s = sched[i];
                    var princ = 0.0;
                    if (amount >= s.principalBalance)
                    {
                        princ = s.principalBalance;
                    }
                    else
                    {
                        princ = amount;
                    }
                    s.principalBalance -= princ;
                    i++;
                    tprinc += princ;
                    amount = amount - princ;
                }

            }

            repayment = new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = 0,
                principalPaid = amountPaid,
                repaymentDate = paymentDate,
                modeOfPaymentID = modeOfPaymentID,
                commission_paid = 0,
                repaymentTypeID = int.Parse(paymentTypeID),
                checkNo = checkNo,
                bankID = bankID,
                bankName = bankName
            };
            ln.balance -= amountPaid;
            list.Add(repayment);
            coreLogic.jnl_batch jb22 = journalextensions.Post("LN", acctID,
                ln.loanType.accountsReceivableAccountID, amt2 > 0 ? amt2 : amountPaid,
                "Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
            var jar2 = jb22.jnl.ToList();
            if (batch != null)
            {
                foreach (var j in jar2)
                {
                    batch.jnl.Add(j);
                }
                ent.Entry(jb22).State = System.Data.Entity.EntityState.Detached;
            }
            else
            {
                ent.jnl_batch.Add(jb22);
                batch = jb22;
            }
            batchNo = batch.batch_no;
        }

        private void ReceiveApplicationFee(loan ln, DateTime paymentDate, double amountPaid,
            double amount, int modeOfPaymentID, saving sav, coreLoansEntities le, core_dbEntities ent,
            string userName, loanRepayment repayment, string paymentTypeID, string checkNo,
            int acctID, int? bankID, string bankName, List<loanRepayment> list,
            comp_prof pro, ref jnl_batch batch, ref string batchNo, ref double amt2)
        {
            if (amount >= ln.applicationFeeBalance)
            {
                ln.applicationFeeBalance = 0;
                amount -= ln.applicationFeeBalance;
            }
            else
            {
                ln.applicationFeeBalance -= amount;
                amount = 0;
            }
            repayment = new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = amountPaid,
                interestPaid = 0,
                principalPaid = 0,
                commission_paid = 0,
                repaymentDate = paymentDate,
                modeOfPaymentID = modeOfPaymentID,
                repaymentTypeID = int.Parse(paymentTypeID),
                checkNo = checkNo,
                bankID = bankID,
                bankName = bankName
            };
            list.Add(repayment);

            var jb5 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                ln.loanType.commissionAndFeesAccountID, amountPaid,
                "Loan Application Fees Payment - " + amountPaid.ToString("#,###.#0")
                + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
            if (amt2 == 0)
            {
                coreLogic.jnl_batch jb6 = journalextensions.Post("LN", acctID,
                        ln.loanType.unpaidCommissionAccountID, amountPaid,
                        "Loan Repayment - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                var j2 = jb6.jnl.ToList();
                if (j2.Count > 1)
                {
                    jb5.jnl.Add(j2[0]);
                    jb5.jnl.Add(j2[1]);
                }
            }
            ent.jnl_batch.Add(jb5);
            batchNo = jb5.batch_no;
            batch = jb5;
        }

        private void ReceiveInterestOnly(loan ln, DateTime paymentDate, double amountPaid,
            double amount, int modeOfPaymentID, saving sav, coreLoansEntities le, core_dbEntities ent,
            string userName, loanRepayment repayment, string paymentTypeID, string checkNo,
            int acctID, int? bankID, string bankName, List<loanRepayment> list,
            comp_prof pro, ref jnl_batch batch, ref string batchNo, ref double amt2)
        {
            if (ln.loanStatusID != 4) return;
            var tinterest = 0.0;
            var tprinc = 0.0;
            List<repaymentSchedule> sched = new List<repaymentSchedule>();
            jnl_batch jb = null;
            jnl_batch jba = null;


            if (ln.repaymentSchedules.Count == 1 && ln.repaymentSchedules.ToList()[0].interestBalance == 0)
            {
                sched = ln.repaymentSchedules.ToList();
                ln.repaymentSchedules.Add(new repaymentSchedule
                {
                    interestPayment = amountPaid,
                    interestBalance = 0,
                    principalBalance = 0,
                    principalPayment = 0,
                    loan = ln,
                    loanID = ln.loanID,
                    creation_date = DateTime.Now,
                    creator = userName,
                    proposedInterestWriteOff = 0,
                    interestWritenOff = 0,
                    repaymentDate = paymentDate,
                    balanceCD = 0,
                    balanceBF = 0
                });
                jba = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                    ln.loanType.unearnedInterestAccountID, amountPaid,
                    "Loan Interest- " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, paymentDate, ln.loanNo, ent, "SYSTEM", ln.client.branchID);

            }
            else 
            {
                sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0).OrderBy(p => p.repaymentDate).ToList();
                if (sched.Count > 0)
                {
                    int i = 0;
                    tprinc = 0.0;
                    while (amount > 0 && i < sched.Count)
                    {
                        var s = sched[i];
                        var princ = 0.0;
                        var interest = 0.0;
                        if (amount >= s.interestBalance)
                        {
                            interest = s.interestBalance;
                        }
                        else
                        {
                            interest = amount;
                        }
                        s.principalBalance -= princ;
                        s.interestBalance -= interest;

                        i++;
                        tprinc += princ;
                        tinterest += interest;
                        amount -= princ + interest;
                    }
                }
            }
            repayment = new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = amountPaid,
                principalPaid = 0,
                commission_paid = 0,
                repaymentDate = paymentDate,
                modeOfPaymentID = modeOfPaymentID,
                repaymentTypeID = int.Parse(paymentTypeID),
                checkNo = checkNo,
                bankID = bankID,
                bankName = bankName
            };
            list.Add(repayment);
            string startNormalDate = System.Configuration.ConfigurationManager.AppSettings["startCashAccountingLoans"];
            if (startNormalDate == null) startNormalDate = "";
            DateTime startCashAccountingLoans = DateTime.MinValue;

            var postUnEarned = (!le.loanConfigs.Any() ||
                                (DateTime.TryParseExact(startNormalDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture,
                                    DateTimeStyles.AllowInnerWhite,
                                    out startCashAccountingLoans) && startCashAccountingLoans > DateTime.MinValue
                                 && startCashAccountingLoans <= ln.disbursementDate.Value.Date));

            if ((le.configs.Any() && le.configs.FirstOrDefault().postInterestUnIntOnDisb)
                || postUnEarned)
            {
                jb = journalextensions.Post("LN", ln.loanType.unearnedInterestAccountID,
                    ln.loanType.interestIncomeAccountID, amountPaid,
                    "Interest Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);

                coreLogic.jnl_batch jb10 = journalextensions.Post("LN", acctID,
                    ln.loanType.accountsReceivableAccountID, amountPaid,
                    "Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                var j5 = jb10.jnl.ToList();
                if (j5.Count > 1)
                {
                    jb.jnl.Add(j5[0]);
                    jb.jnl.Add(j5[1]);
                }
                var jar3 = jb.jnl.ToList();
                if (batch != null)
                {
                    foreach (var j in jar3)
                    {
                        batch.jnl.Add(j);
                    }
                    ent.Entry(jb).State = System.Data.Entity.EntityState.Detached;
                    if (jba != null)
                    {
                        jar3 = jba.jnl.ToList();
                        batch.jnl.Add(jar3[0]);
                        batch.jnl.Add(jar3[1]);
                        ent.Entry(jba).State = System.Data.Entity.EntityState.Detached;
                    }
                }
                else
                {
                    ent.jnl_batch.Add(jb);
                    batch = jb;
                    if (jba != null)
                    {
                        jar3 = jba.jnl.ToList();
                        batch.jnl.Add(jar3[0]);
                        batch.jnl.Add(jar3[1]);
                        ent.Entry(jba).State = System.Data.Entity.EntityState.Detached;
                    }
                }
                batchNo = batch.batch_no;
            }
            else
            {
                if (amt2 == 0)
                {
                    jb = journalextensions.Post("LN", acctID,
                        ln.loanType.accountsReceivableAccountID, amountPaid,
                        "Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var jar3 = jb.jnl.ToList();
                    if (batch != null)
                    {
                        foreach (var j in jar3)
                        {
                            batch.jnl.Add(j);
                        }
                        ent.Entry(jb).State = System.Data.Entity.EntityState.Detached;
                    }
                    else
                    {
                        ent.jnl_batch.Add(jb);
                        batch = jb;
                    }
                    batchNo = batch.batch_no;
                }
            }
        }


        private void ReceiveCommission(loan ln, DateTime paymentDate, double amountPaid,
            double amount, int modeOfPaymentID, saving sav, coreLoansEntities le, core_dbEntities ent,
            string userName, loanRepayment repayment, string paymentTypeID, string checkNo,
            int acctID, int? bankID, string bankName, List<loanRepayment> list,
            comp_prof pro, ref jnl_batch batch, ref string batchNo, ref double amt2)
        {
            if (amount >= ln.commissionBalance)
            {
                ln.commissionBalance = 0;
                amount -= ln.commissionBalance;
            }
            else
            {
                ln.commissionBalance -= amount;
                amount = 0;
            }
            repayment = new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = 0,
                principalPaid = 0,
                repaymentDate = paymentDate,
                modeOfPaymentID = modeOfPaymentID,
                commission_paid = amountPaid,
                repaymentTypeID = int.Parse(paymentTypeID),
                checkNo = checkNo,
                bankID = bankID,
                bankName = bankName
            };
            list.Add(repayment);

            var jb4 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                ln.loanType.commissionAndFeesAccountID, amountPaid,
                "Loan Commission Payment - " + amountPaid.ToString("#,###.#0")
                + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
            if (amt2 == 0)
            {
                coreLogic.jnl_batch jb7 = journalextensions.Post("LN", acctID,
                        ln.loanType.unpaidCommissionAccountID, amountPaid,
                        "Loan Repayment - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                var j3 = jb7.jnl.ToList();
                if (j3.Count > 1)
                {
                    jb4.jnl.Add(j3[0]);
                    jb4.jnl.Add(j3[1]);
                }
            }
            ent.jnl_batch.Add(jb4);
            batchNo = jb4.batch_no;
            batch = jb4;
        }


        private void ReceivePenalty(loan ln, DateTime paymentDate, double amountPaid,
            double amount, int modeOfPaymentID, saving sav, coreLoansEntities le, core_dbEntities ent,
            string userName, loanRepayment repayment, string paymentTypeID, string checkNo,
            int acctID, int? bankID, string bankName, List<loanRepayment> list,
            comp_prof pro, ref jnl_batch batch, ref string batchNo, ref double amt2)
        {
            var pen = ln.loanPenalties.Where(p => p.penaltyBalance > 0).OrderBy(p => p.penaltyDate).ToList();
            if (pen.Count > 0)
            {
                int i = 0;
                var tamount = 0.0;
                while (amount > 0 && i < pen.Count)
                {
                    var s = pen[i];
                    var amt = 0.0;
                    if (amount >= s.penaltyBalance)
                    {
                        amt = s.penaltyBalance;
                    }
                    else
                    {
                        amt = amount;
                    }
                    s.penaltyBalance -= amt;

                    i++;
                    tamount += amt;
                    amount -= amt;
                }
                repayment = new coreLogic.loanRepayment
                {
                    amountPaid = tamount,
                    creation_date = DateTime.Now,
                    creator = userName,
                    feePaid = 0,
                    interestPaid = 0,
                    principalPaid = 0,
                    commission_paid = 0,
                    repaymentDate = paymentDate,
                    modeOfPaymentID = modeOfPaymentID,
                    repaymentTypeID = int.Parse(paymentTypeID),
                    penaltyPaid = tamount,
                    checkNo = checkNo,
                    bankID = bankID,
                    bankName = bankName
                };
                list.Add(repayment);

                coreLogic.jnl_batch jb8 = journalextensions.Post("LN", ln.loanType.unearnedInterestAccountID,
                    ln.loanType.interestIncomeAccountID, tamount,
                    "Additional Interest Payment - " + tamount.ToString("#,###.#0")
                    + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                if (amt2 == 0)
                {
                    coreLogic.jnl_batch jb9 = journalextensions.Post("LN", acctID,
                        ln.loanType.accountsReceivableAccountID, amountPaid,
                        "Additional Interest Payment - " + amountPaid.ToString("#,###.#0")
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var j4 = jb9.jnl.ToList();
                    if (j4.Count > 1)
                    {
                        jb8.jnl.Add(j4[0]);
                        jb8.jnl.Add(j4[1]);
                    }
                }
                var jar5 = jb8.jnl.ToList();
                if (batch != null)
                {
                    foreach (var j in jar5)
                    {
                        batch.jnl.Add(j);
                    }
                    ent.Entry(jb8).State = System.Data.Entity.EntityState.Detached;
                }
                else
                {
                    ent.jnl_batch.Add(jb8);
                    batch = jb8;
                }
                batchNo = batch.batch_no;

            }
        }

        private void ReceiveProcessingFee(loan ln, DateTime paymentDate, double amountPaid,
            double amount, int modeOfPaymentID, saving sav, coreLoansEntities le, core_dbEntities ent,
            string userName, loanRepayment repayment, string paymentTypeID, string checkNo,
            int acctID, int? bankID, string bankName, List<loanRepayment> list,
            comp_prof pro, ref jnl_batch batch, ref string batchNo, ref double amt2)
        {
            if (amount >= ln.processingFeeBalance)
            {
                ln.processingFeeBalance = 0;
                amount -= ln.processingFeeBalance;
            }
            else
            {
                ln.processingFeeBalance -= amount;
                amount = 0;
            }
            repayment = new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = amountPaid,
                interestPaid = 0,
                principalPaid = 0,
                commission_paid = 0,
                repaymentDate = paymentDate,
                modeOfPaymentID = modeOfPaymentID,
                repaymentTypeID = int.Parse(paymentTypeID),
                checkNo = checkNo,
                bankID = bankID,
                bankName = bankName
            };
            list.Add(repayment);

            var jb3 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                ln.loanType.commissionAndFeesAccountID, amountPaid,
                "Processing Fees for " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
            if (amt2 == 0)
            {
                coreLogic.jnl_batch jb11 = journalextensions.Post("LN", acctID,
                        ln.loanType.accountsReceivableAccountID, amountPaid,
                        "Processing fees for " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                var j6 = jb11.jnl.ToList();
                if (j6.Count > 1)
                {
                    jb3.jnl.Add(j6[0]);
                    jb3.jnl.Add(j6[1]);
                }
            }
            var jar4 = jb3.jnl.ToList();
            if (batch != null)
            {
                foreach (var j in jar4)
                {
                    batch.jnl.Add(j);
                }
                ent.Entry(jb3).State = System.Data.Entity.EntityState.Detached;
            }
            else
            {
                ent.jnl_batch.Add(jb3);
                batch = jb3;
            }
            batchNo = batch.batch_no;
        }
    }
}
