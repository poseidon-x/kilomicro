using coreData.ErrorLog;
using coreLogic.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace coreLogic
{
    public class DisbursementsManager : IDisbursementsManager 
    {         
        IJournalExtensions journalextensions = new JournalExtensions();
        ScheduleManager schedMgr = new ScheduleManager();
        CoreInfoLogger cil = new CoreInfoLogger();


        public void PostLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo, saving sav = null, susuAccount sc = null)
        {

            if (paymentMode == "1")
            {
                PostCashLoan(le, ln, amountPaid, amountApproved,
                    disbDate, bank, paymentType, checkNo, ent,
                    addFees, userName, paymentMode, crAccountNo, sav, sc);
            }
            else if (paymentMode == "2" || paymentMode == "3")
            {
                PostBankLoan(le, ln, amountPaid, amountApproved,
                    disbDate, bank, paymentType, checkNo, ent,
                    addFees, userName, paymentMode, crAccountNo, sav, sc);
            }
            else if (paymentMode == "4")
            {
                PostSavingsLoan(le, ln, amountPaid, amountApproved,
                    disbDate, bank, paymentType, checkNo, ent,
                    addFees, userName, paymentMode, crAccountNo, sav, sc);
            }
            else if (paymentMode == "5")
            {
                PostSusuLoan(le, ln, amountPaid, amountApproved,
                    disbDate, bank, paymentType, checkNo, ent,
                    addFees, userName, paymentMode, crAccountNo, sav, sc);
            }
        }

        public void PostDisbursement(coreLoansEntities le, double? amountPaid,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo)
        {
            if (amountPaid > 0)
            {
                var acctID = ent.accts.FirstOrDefault(p => p.acc_num == "1001").acct_id;
                var acctID2 = ent.accts.FirstOrDefault(p => p.acc_num == "1000").acct_id;
                if (crAccountNo != "" && crAccountNo != null)
                {
                    var gl = ent.accts.FirstOrDefault(p => p.acc_num == crAccountNo);
                    if (gl != null)
                    {
                        acctID2 = gl.acct_id;
                    }
                }

                var pro = ent.comp_prof.FirstOrDefault();
                var jb = journalextensions.Post("LN", acctID,
                    acctID2, amountPaid.Value,
                    "Loan Disbursement",
                    pro.currency_id.Value, disbDate.Value, "", ent, userName, null);

                ent.jnl_batch.Add(jb);
            }
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
                    this.PostLoan(le, ln, d.amount, d.amount,
                            d.txDate, (d.bankID == null) ? "" : d.bankID.ToString(),
                           d.paymentModeID.ToString(), d.checkNo, ent, d.addFees, userName, d.paymentModeID.ToString(),
                            crAccNo, sav, sc);
                }
                else
                {
                    this.PostLoan(le, ln, d.amount, d.amount,
                        d.txDate, (d.bankID == null) ? "" : d.bankID.ToString(),
                        d.paymentModeID.ToString(), d.checkNo, ent, d.addFees,
                        userName, d.paymentModeID.ToString(), null, null);
                }
                ln.disbursedBy = userName;
            

            
        }




        private void PostBankLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo, saving sav = null, susuAccount sc = null)
        {

            if (amountApproved > 0)
            {
                double interest;
                bool first;
                insuranceSetup ls;
                comp_prof pro;
                double insuranceAmount;
                double amountAtBank;
                PreProcessing(le, ln, amountPaid, disbDate, ent, addFees, userName, sc, out interest, out first,
                    out ls, out pro, out insuranceAmount, out amountAtBank);

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
                jb = DoDisbursementPostings(le, ln, amountPaid, disbDate, paymentType, checkNo, ent, userName, ref interest, 
                    first, ls, pro, insuranceAmount, amountAtBank, bankID, disbAcc, acctID);
            }
            ln.loanStatusID = 4;
        }

        private void PostCashLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo, saving sav = null, susuAccount sc = null)
        {
            //try
            //{
                if (amountApproved > 0)
                {
                    double interest;
                    bool first;
                    insuranceSetup ls;
                    comp_prof pro;
                    double insuranceAmount;
                    double amountAtBank;
                    PreProcessing(le, ln, amountPaid, disbDate, ent, addFees, userName, sc, out interest, out first,
                        out ls, out pro, out insuranceAmount, out amountAtBank);

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
                    jb = DoDisbursementPostings(le, ln, amountPaid, disbDate, paymentType, checkNo, ent, userName, ref interest, first,
                        ls, pro, insuranceAmount, amountAtBank, null, disbAcc, acctID);
                }
                ln.loanStatusID = 4;
            //}
            //catch (Exception x)
            //{
            //    cil.logError(x);
            //    throw;
            //}
            
        }

        private void PostSusuLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo, saving sav = null, susuAccount sc = null)
        {

            if (amountApproved > 0)
            {
                double interest;
                bool first;
                insuranceSetup ls;
                comp_prof pro;
                double insuranceAmount;
                double amountAtBank;
                PreProcessing(le, ln, amountPaid, disbDate, ent, addFees, userName, sc, out interest, out first,
                    out ls, out pro, out insuranceAmount, out amountAtBank);

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


                int? acctID = null;
                var acc = ent.accts.FirstOrDefault(p => p.acc_num == crAccountNo);
                if (((paymentMode == "2" || paymentMode == "3") && bankID != null)
                    && (pro.traditionalLoanNo == true))
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
                else if (ln.loanTypeID == 7 || ln.loanTypeID == 8)
                {
                    acctID = le.susuConfigs.FirstOrDefault().contributionsPayableAccountID;
                }
                else if (da != null)
                {
                    if (da.accts != null)
                    {
                        acctID = da.accts.acct_id;
                    }
                }
                else if (acc != null && acc != null && pro.traditionalLoanNo == true)
                {
                    acctID = acc.acct_id;
                }
                else
                {
                    var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1001");
                    if (acc2 != null)
                    {
                        acctID = acc2.acct_id;
                    }
                }
                jnl_batch jb;
                jb = DoDisbursementPostings(le, ln, amountPaid, disbDate, paymentType, checkNo, ent, userName, ref interest, 
                    first, ls, pro, insuranceAmount, amountAtBank, bankID, disbAcc, acctID.Value);
            }
            ln.loanStatusID = 4;
        }

        private void PostSavingsLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo, saving sav = null, susuAccount sc = null)
        {

            if (amountApproved > 0)
            {
                double interest;
                bool first;
                insuranceSetup ls;
                comp_prof pro;
                double insuranceAmount;
                double amountAtBank;
                PreProcessing(le, ln, amountPaid, disbDate, ent, addFees, userName, sc, out interest, out first,
                    out ls, out pro, out insuranceAmount, out amountAtBank);

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
                 
                int? acctID = null;
                var acc = ent.accts.FirstOrDefault(p => p.acc_num == crAccountNo);
                if (((paymentMode == "2" || paymentMode == "3") && bankID != null)
                    && (pro.traditionalLoanNo == true))
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
                else if (ln.loanTypeID == 7 || ln.loanTypeID == 8)
                {
                    acctID = le.susuConfigs.FirstOrDefault().contributionsPayableAccountID;
                }
                else if (da != null)
                {
                    if (da.accts != null)
                    {
                        acctID = da.accts.acct_id;
                    }
                }
                else if (acc != null && acc != null && pro.traditionalLoanNo == true)
                {
                    acctID = acc.acct_id;
                }
                else
                {
                    var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1001");
                    if (acc2 != null)
                    {
                        acctID = acc2.acct_id;
                    }
                }
                jnl_batch jb;
                if (sav != null && pro.disburseLoansToSavingsAccount == true)
                {
                    acctID = sav.savingType.accountsPayableAccountID;
                    sav.savingAdditionals.Add(new savingAdditional
                    {
                        localAmount = amountAtBank,
                        savingAmount = amountAtBank,
                        savingDate = disbDate.Value,
                        naration = "Loan Disbursed to Savings Account",
                        modeOfPaymentID = 1,
                        interestBalance = 0,
                        principalBalance = sav.principalBalance + amountAtBank,
                        posted = true,
                        fxRate = 1,
                        creation_date = DateTime.Now,
                        creator = userName
                    });
                    sav.principalBalance += amountAtBank;
                    sav.amountInvested += amountAtBank;
                }
                jb = DoDisbursementPostings(le, ln, amountPaid, disbDate, paymentType, checkNo, ent, userName, ref interest, first, ls, pro, 
                    insuranceAmount, amountAtBank, bankID, disbAcc, acctID.Value);
            }
            ln.loanStatusID = 4;
        }

        private void PreProcessing(coreLoansEntities le, loan ln, double? amountPaid, DateTime? disbDate,  core_dbEntities ent, 
            bool addFees, string userName, susuAccount sc, out double interest, out bool first, out insuranceSetup ls, 
            out comp_prof pro, out double insuranceAmount, out double amountAtBank)
        {
            var amount = amountPaid.Value;
            if (addFees)
            {
                amount += ln.applicationFeeBalance + ln.processingFeeBalance;
                ln.applicationFeeBalance = 0;
                ln.processingFeeBalance = 0;
            }
            if (ln.disbursementDate == null) ln.disbursementDate = disbDate.Value;
            ln.modification_date = DateTime.Now;
            ln.last_modifier = userName;
            ln.balance += amount;

            interest = 0.0;
            pro = ent.comp_prof.FirstOrDefault();
            first = false;
            if (ln.amountDisbursed == 0)
            {
                first = true;
                if (ln.edited == false)
                {
                    foreach (var rs in ln.repaymentSchedules.ToList())
                    {
                        le.repaymentSchedules.Remove(rs);
                    }
                    if (ln.loanTypeID == 6)
                    {

                        List<coreLogic.repaymentSchedule> sched =
                            schedMgr.calculateScheduleM(amount, ln.interestRate,
                            disbDate.Value, ln.loanTenure);
                        foreach (var rs in sched)
                        {
                            rs.creation_date = DateTime.Now;
                            rs.creator = userName;
                            ln.repaymentSchedules.Add(rs);
                        }
                    }

                    else if (sc != null)
                    {
                        ln.repaymentSchedules.Add(new repaymentSchedule
                        {
                            additionalInterest = 0,
                            additionalInterestBalance = 0,
                            penaltyAmount = 0,
                            balanceBF = ln.amountApproved,
                            balanceCD = 0,
                            creation_date = System.DateTime.Now,
                            creator = userName,
                            edited = false,
                            interestBalance = sc.interestAmount,
                            interestPayment = sc.interestAmount,
                            origInterestPayment = sc.interestAmount,
                            origPrincipalBF = ln.amountApproved,
                            origPrincipalCD = 0,
                            origPrincipalPayment = ln.amountApproved,
                            interestWritenOff = 0,
                            principalBalance = ln.amountApproved,
                            principalPayment = ln.amountApproved,
                            repaymentDate = disbDate.Value,
                            proposedInterestWriteOff = 0
                        });
                    }
                    else
                    {
                        //if company is GFI
                        if (pro.comp_name.ToLower().Contains("gfi"))
                        {
                            List<coreLogic.repaymentSchedule> sched =
                                schedMgr.calculateScheduleDailyGFI(amount, ln.interestRate,
                                    disbDate.Value, ln.gracePeriod, ln.loanTenure,
                                    ln.interestTypeID.Value, ln.repaymentModeID, ln.loanTypeID);
                            foreach (var rs in sched)
                            {
                                rs.creation_date = DateTime.Now;
                                rs.creator = userName;
                                ln.repaymentSchedules.Add(rs);
                            }
                        }
                        else if (pro.comp_name.ToLower().Contains("ttl"))
                        {
                            List<coreLogic.repaymentSchedule> sched = schedMgr.calculateScheduleTTL(amount, ln.interestRate,
                                    disbDate.Value, ln.gracePeriod, ln.loanTenure,
                                    ln.interestTypeID.Value, ln.repaymentModeID, ln);
                            foreach (var rs in sched)
                            {
                                rs.creation_date = DateTime.Now;
                                rs.creator = userName;
                                ln.repaymentSchedules.Add(rs);
                            }
                        }
                        // other companies either than GFI
                        else
                        {
                            List<coreLogic.repaymentSchedule> sched =
                                schedMgr.calculateSchedule(amount, ln.interestRate,
                                    disbDate.Value, ln.gracePeriod, ln.loanTenure,
                                    ln.interestTypeID.Value, ln.repaymentModeID, ln.client);
                            foreach (var rs in sched)
                            {
                                rs.creation_date = DateTime.Now;
                                rs.creator = userName;
                                ln.repaymentSchedules.Add(rs);
                            }
                        }
                    }
                }
            }
            else
            {                
                interest = ln.repaymentSchedules.Sum(p => p.interestPayment);
                var sched = ln.repaymentSchedules.ToList();

                sched =
                    schedMgr.calculateSchedule(amount, ln.interestRate,
                       disbDate.Value, ln.gracePeriod, ln.loanTenure,
                       ln.interestTypeID.Value, ln.repaymentModeID, sched);          

                
                foreach (var rs in sched)
                {
                    rs.modification_date = DateTime.Now;
                    rs.last_modifier = userName;
                }
            }

            ls = le.insuranceSetups.FirstOrDefault(p => p.loanTypeID == ln.loanTypeID);
            insuranceAmount = 0.0;
            amountAtBank = amount;

            if (pro.deductProcFee == true && ln.loanTypeID != 7 && ln.loanStatusID != 4)
            {
                amountAtBank -= ln.processingFee;
            }
            if (pro.deductInsurance == true && ls != null && ln.insuranceAmount > 0 && ln.loanStatusID != 4)
            {
                insuranceAmount = ln.insuranceAmount;
                amountAtBank -= insuranceAmount;
            }
            if (pro.deductInsurance == false && ln.insuranceAmount > 0 && ln.loanStatusID != 4 
                && (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains(AppContants.Lendzee) ||
                ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains(AppContants.Kilo)))
            {
                insuranceAmount = ln.insuranceAmount;
            }

            ln.amountDisbursed += amount;
        }

        private jnl_batch DoDisbursementPostings(coreLoansEntities le, loan ln, double? amountPaid, DateTime? disbDate, string paymentType, string checkNo, core_dbEntities ent, string userName, 
            ref double interest, bool first, insuranceSetup ls, comp_prof pro, double insuranceAmount, 
            double amountAtBank, int? bankID, int? disbAcc, int acctID)
        {
            jnl_batch jb;
            double applicationFee = 0;

            jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                disbAcc.Value, amountAtBank,
                "Loan Disbursement Principal - " + amountAtBank.ToString("#,###.#0")
                + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
            if (!ln.addFeesToPrincipal && first == true && ln.processingFee > 0 && ln.loanStatusID != 4)
            {
                var lf = ln.loanFees.FirstOrDefault();
                if (lf == null)
                {
                    lf = new coreLogic.loanFee
                    {
                        feeAmount = ln.processingFee,
                        feeDate = disbDate.Value,
                        feeTypeID = 1,
                        creation_date = DateTime.Now,
                        creator = userName
                    };
                    ln.loanFees.Add(lf);
                    if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("gfi"))
                    {
                        using (var cl = new coreLoansEntities())
                        {
                            var config = cl.loanConfigs.First();
                            lf = new coreLogic.loanFee
                            {
                                feeAmount = config.applicationFeeAmount,
                                feeDate = disbDate.Value,
                                feeTypeID = 2,
                                creation_date = DateTime.Now,
                                creator = userName
                            };
                            applicationFee = lf.feeAmount;
                            ln.loanFees.Add(lf);
                        }
                    }
                    
                }
                else
                {
                    lf.last_modifier = userName;
                    lf.modification_date = DateTime.Now;
                    lf.feeAmount = ln.processingFee;                    
                }

                if ((ent.comp_prof.First().comp_name.ToLower().Contains(AppContants.Lendzee) ||
                    ent.comp_prof.First().comp_name.ToLower().Contains(AppContants.Kilo)) || pro.deductProcFee == false)
                {
                    var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                        ln.loanType.unpaidCommissionAccountID, ln.processingFee + applicationFee,
                        "Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);
                }
                else 
                {
                    var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                                ln.loanType.commissionAndFeesAccountID, ln.processingFee + applicationFee,
                                                "Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                                                pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);

                    var repayment = new coreLogic.loanRepayment
                    {
                        amountPaid = ln.processingFee,
                        creation_date = DateTime.Now,
                        creator = userName,
                        feePaid = ln.processingFee,
                        interestPaid = 0,
                        principalPaid = 0,
                        commission_paid = 0,
                        repaymentDate = disbDate.Value,
                        modeOfPaymentID = 1,
                        repaymentTypeID = 6,
                        checkNo = checkNo,
                        bankID = null,
                        bankName = null
                    };
                    ln.loanRepayments.Add(repayment);
                    var applicationFeePaym = new coreLogic.loanRepayment
                    {
                        amountPaid = applicationFee,
                        creation_date = DateTime.Now,
                        creator = userName,
                        feePaid = applicationFee,
                        interestPaid = 0,
                        principalPaid = 0,
                        commission_paid = 0,
                        repaymentDate = disbDate.Value,
                        modeOfPaymentID = 1,
                        repaymentTypeID = 5,
                        //checkNo = checkNo,
                        bankID = null,
                        bankName = null
                    };
                    ln.loanRepayments.Add(applicationFeePaym);
                }
            }
            if((ent.comp_prof.First().comp_name.ToLower().Contains(AppContants.Lendzee) || ent.comp_prof.First().comp_name.ToLower().Contains(AppContants.Kilo)) 
                || pro.deductInsurance == false)
            {
                var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                        ln.loanType.unpaidCommissionAccountID, insuranceAmount,
                        "Loan Disbursement Insurance - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);
            }
            else if (pro.deductInsurance == true && ls != null && ln.loanStatusID != 4)
            {
                var lf = ln.loanInsurances.FirstOrDefault();
                if (lf == null)
                {
                    lf = new coreLogic.loanInsurance
                    {
                        amount = insuranceAmount,
                        insuranceDate = disbDate.Value,
                        paid = false
                    };
                    ln.loanInsurances.Add(lf);
                }
                else
                {
                    lf.amount += insuranceAmount;
                }

                var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                        ls.insuranceAccountID.Value, insuranceAmount,
                        "Loan Disbursement Insurance - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                var list = jb2.jnl.ToList();
                jb.jnl.Add(list[0]);
                jb.jnl.Add(list[1]);

                var repayment = new coreLogic.loanRepayment
                {
                    amountPaid = insuranceAmount,
                    creation_date = DateTime.Now,
                    creator = userName,
                    feePaid = insuranceAmount,
                    interestPaid = 0,
                    principalPaid = 0,
                    commission_paid = 0,
                    repaymentDate = disbDate.Value,
                    modeOfPaymentID = 1,
                    repaymentTypeID = 8,
                    checkNo = checkNo,
                    bankID = null,
                    bankName = null
                };
                ln.loanRepayments.Add(repayment);
                lf.paid = true;
                lf.amount -= insuranceAmount;
            }
			if(pro.comp_name.ToLower().Contains(AppContants.Jireh)){
				interest = ln.repaymentSchedules.First().interestPayment;
			}else{
				interest = ln.repaymentSchedules.Sum(p => p.interestPayment) - interest;
			}
            
            string startNormalDate = System.Configuration.ConfigurationManager.AppSettings["startCashAccountingLoans"];
            if (startNormalDate == null) startNormalDate = "";
            DateTime startCashAccountingLoans = DateTime.MinValue;
            if (interest > 0 && ((!le.configs.Any() || le.configs.FirstOrDefault().postInterestUnIntOnDisb)
                || ln.repaymentModeID == -1))
            {
                coreLogic.jnl_batch jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                    ln.loanType.unearnedInterestAccountID, interest,
                    "Loan Disbursement Interest- " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                var j = jb2.jnl.ToList();
                if (j.Count > 1)
                {
                    jb.jnl.Add(j[0]);
                    jb.jnl.Add(j[1]);
                }
            }

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
                amountDisbursed = amountPaid.Value + (ln.addFeesToPrincipal ? ln.processingFee : 0),
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

    }
}
