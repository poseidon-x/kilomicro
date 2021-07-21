using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic
{
    public class InvestmentManager : IInvestmentManager
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLoansEntities le = new coreLoansEntities();

        public savingWithdrawal WithdrawalOthers(ref double pamount, ref double iamount, modeOfPayment mop, int? bankID,
            string withdrawalType, coreLogic.saving dep, double takeHomeAmount, DateTime dateWithdrawn,
            string checkNo, string narration, string userName)
        {
            if (withdrawalType == "I")
            {
                if (Math.Round(dep.availableInterestBalance, 2) >= takeHomeAmount)
                {
                    iamount = takeHomeAmount;
                }
                else
                {
                    throw new ApplicationException("There is not enough interest balance to be withdrawn");
                }
            }
            else if (withdrawalType == "P")
            {
                if (Math.Round(dep.availablePrincipalBalance, 2) >= takeHomeAmount)
                {
                    pamount = takeHomeAmount;
                }
                else
                {
                    throw new ApplicationException("There is not enough interest balance to be withdrawn");
                }
            }
            else if (withdrawalType == "B")
            {
                if (Math.Round(dep.availableInterestBalance, 2) >= takeHomeAmount)
                {
                    iamount = takeHomeAmount;
                }
                else if (Math.Round(dep.availablePrincipalBalance + dep.availableInterestBalance, 2) >= takeHomeAmount)
                {
                    pamount = takeHomeAmount - dep.availableInterestBalance;
                    iamount = dep.availableInterestBalance;
                }
                else
                {
                    throw new ApplicationException("There is not enough interest balance to be withdrawn");
                }
            }
            if (pamount == 0 && iamount == 0) throw new ApplicationException("There is not enough interest balance to be withdrawn");
            var sav = le.savings.FirstOrDefault(p => p.savingID == dep.savingID);
            var dw = new coreLogic.savingWithdrawal
            {
                checkNo = checkNo,
                principalWithdrawal = pamount,
                interestWithdrawal = iamount,
                bankID = bankID,
                interestBalance = sav.interestBalance- iamount,
                interestBalanceBD = sav.interestBalance,
                withdrawalDate = dateWithdrawn.Date,//.Add(DateTime.Now.TimeOfDay),
                creation_date = DateTime.Now,
                creator = userName,
                principalBalance = sav.principalBalance - pamount,
                principalBalanceBD = sav.principalBalance,
                modeOfPaymentID = mop.modeOfPaymentID,
                fxRate = 1,
                localAmount = (pamount + iamount) * 1,
                naration = narration,
                posted = false,
                closed = false
            };
            dep.principalBalance -= pamount;
            dep.interestBalance -= iamount;
            dep.availablePrincipalBalance -= pamount;
            dep.availableInterestBalance = dep.availableInterestBalance - iamount;
            dep.savingWithdrawals.Add(dw);

            var amt = iamount;
            dep.modification_date = DateTime.Now;
            dep.last_modifier = userName;

            return dw;
        }

        public savingWithdrawal WithdrawalEWC(ref double pamount, ref double iamount, modeOfPayment mop, int? bankID,
            string withdrawalType, coreLogic.saving dep, double takeHomeAmount, DateTime dateWithdrawn,
            string checkNo, string narration, string userName, SavingWithdrawalCalcModel calc)
        { 
            if (Math.Round(dep.interestBalance+dep.principalBalance, 2) < calc.grossWithdrawalWamount)
            {
                throw new ApplicationException("There is not enough balance to be withdrawn");
            }
            iamount = calc.interestWithdrawal;
            pamount = calc.principalWithdrawal;

            if (pamount == 0 && iamount == 0) throw new ApplicationException("There is not enough interest balance to be withdrawn");
            
            var dw = new coreLogic.savingWithdrawal
            {
                checkNo = checkNo,
                principalWithdrawal = pamount,
                interestWithdrawal = iamount,
                bankID = bankID,
                interestBalance = dep.interestBalance,
                withdrawalDate = dateWithdrawn,
                creation_date = DateTime.Now,
                creator = userName,
                principalBalance = dep.principalBalance,
                modeOfPaymentID = mop.modeOfPaymentID,
                fxRate = 1,
                localAmount = (pamount + iamount) * 1,
                naration = narration,
                posted = false,
                closed = false
            };
            dep.savingWithdrawals.Add(dw);

            dep.savingCharges.Add(new savingCharge
            {
                amount = calc.interestCharges + calc.principalCharges,
                approvedBy = userName,
                chargeDate = dateWithdrawn,
                chargeTypeID = calc.chargeTypeTier.chargeTypeId,
                creationDate = DateTime.Now,
                memo = calc.chargeTypeTier.chargeType.chargeTypeName,
            });

            dep.principalBalance -= (pamount+ calc.principalCharges);
            dep.interestBalance -= (iamount+calc.interestCharges);
            dep.availablePrincipalBalance -= (pamount + calc.principalCharges);
            dep.availableInterestBalance -= (iamount + calc.interestCharges);
              
            dep.modification_date = DateTime.Now;
            dep.last_modifier = userName;

            return dw;
        }

        public void PostDepositsWithdrawal(depositWithdrawal da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", da.deposit.depositType.accountsPayableAccountID.Value,
                acctID, (da.interestWithdrawal + da.principalWithdrawal),
                "Withdrawal from Investment Account - " + (da.principalWithdrawal + da.interestWithdrawal).ToString("#,###.#0")
                + " - " + da.deposit.client.accountNumber + " - " + da.deposit.client.surName + "," + da.deposit.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.deposit.depositNo, ent, userName,
                 da.deposit.client.branchID);
            var js = jb.jnl.FirstOrDefault(p => p.accts.acct_id == da.deposit.depositType.accountsPayableAccountID);
            js.dbt_amt = da.principalWithdrawal;
            js.crdt_amt = 0;

            var jb2 = journalextensions.Post("LN", da.deposit.depositType.interestPayableAccountID.Value,
                acctID, (da.interestWithdrawal),
                "Withdrawal from Investment Account - "
                + " - " + da.deposit.client.accountNumber + " - " + da.deposit.client.surName + "," + da.deposit.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.deposit.depositNo, ent, userName,
                 da.deposit.client.branchID);
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == acctID);
            ent.Entry(js).State = System.Data.Entity.EntityState.Detached;
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == da.deposit.depositType.interestPayableAccountID);
            jb.jnl.Add(js);

            ent.jnl_batch.Add(jb);
            da.posted = true;
        }















        //TODO Get appropriate accounts and post to.
        public void PostDepositsWithdrawalCharges(depositWithdrawal da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            double charge = da.disInvestmentCharge==null?0: da.disInvestmentCharge.Value;
            var jb = journalextensions.Post("LN", da.deposit.depositType.interestPayableAccountID.Value,
                acctID, charge,
                "Premature disinvestment from Investment Account - " + (charge).ToString("#,###.#0")
                + " - " + da.deposit.client.accountNumber + " - " + da.deposit.client.surName + "," + da.deposit.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.deposit.depositNo, ent, userName,
                 da.deposit.client.branchID);
            var js = jb.jnl.FirstOrDefault(p => p.accts.acct_id == da.deposit.depositType.accountsPayableAccountID);
            js.dbt_amt = da.principalWithdrawal;
            js.crdt_amt = 0;

            var jb2 = journalextensions.Post("LN", da.deposit.depositType.interestPayableAccountID.Value,
                 acctID, (charge),
                "Premature disinvestment from Investment Account - "
                + " - " + da.deposit.client.accountNumber + " - " + da.deposit.client.surName + "," + da.deposit.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.deposit.depositNo, ent, userName,
                 da.deposit.client.branchID);
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == acctID);
            ent.Entry(js).State = System.Data.Entity.EntityState.Detached;
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == da.deposit.depositType.interestPayableAccountID);
            jb.jnl.Add(js);

            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public void PostDepositAdditional(depositAdditional da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", acctID,
                da.deposit.depositType.accountsPayableAccountID.Value, da.depositAmount,
                "Deposit into Investment - " + da.depositAmount
                + " - " + da.deposit.client.accountNumber + " - " + da.deposit.client.surName + "," + da.deposit.client.otherNames,
                pro.currency_id.Value, da.depositDate, da.deposit.depositNo, ent, userName,
                da.deposit.client.branchID);
            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public void PostClientServiceCharge(clientServiceCharge csc, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            //if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            //{
            //    var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
            //    if (ba != null)
            //    {
            //        //ba.acctsReference.Load();
            //        if (ba.accts != null)
            //        {
            //            acctID = ba.accts.acct_id;
            //        }
            //    }
            //}
            var jb = journalextensions.Post("CS", acctID,
                csc.chargeType.accountsReceivableAccountID.Value, csc.chargeAmount,
                 csc.chargeType.chargeTypeName+" Charge - " + csc.chargeAmount
                + " - " + csc.client.accountNumber + " - " + csc.client.surName + "," + csc.client.otherNames,
                pro.currency_id.Value, csc.chargeDate, csc.client.accountNumber, ent, userName,
                csc.client.branchID);
            ent.jnl_batch.Add(jb);
            csc.posted = true;
        }

        public void PostSavingsWithdrawal(savingWithdrawal da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                { 
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            else if (da.modeOfPaymentID == 1)
            {

            }
            var ch = da.saving.savingCharges.FirstOrDefault(p => p.chargeDate == da.withdrawalDate);
            var jb = journalextensions.Post("LN", da.saving.savingType.accountsPayableAccountID.Value,
                acctID, (da.interestWithdrawal + da.principalWithdrawal),
                "Withdrawal from Savings Account - " + (da.principalWithdrawal + da.interestWithdrawal).ToString("#,###.#0")
                + " - " + da.saving.client.accountNumber + " - " + da.saving.client.surName + "," + da.saving.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.saving.savingNo, ent, userName,
                 da.saving.client.branchID);
            var js = jb.jnl.FirstOrDefault(p => p.accts.acct_id == da.saving.savingType.accountsPayableAccountID);
            js.dbt_amt = da.principalWithdrawal;
            js.crdt_amt = 0;

            var jb2 = journalextensions.Post("LN", da.saving.savingType.interestPayableAccountID,
                acctID, (da.interestWithdrawal),
                "Withdrawal from Savings Account - "
                + " - " + da.saving.client.accountNumber + " - " + da.saving.client.surName + "," + da.saving.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.saving.savingNo, ent, userName,
                 da.saving.client.branchID);
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == acctID);
            ent.Entry(js).State = System.Data.Entity.EntityState.Detached;
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == da.saving.savingType.interestPayableAccountID);
            ent.Entry(js).State = EntityState.Detached;
            jb.jnl.Add(js);
            if (ch != null)
            {
                jb2 = journalextensions.Post("LN", da.saving.savingType.interestPayableAccountID,
                da.saving.savingType.chargesIncomeAccountID.Value, (ch.amount),
                "Withdrawal from Savings Account - "
                + " - " + da.saving.client.accountNumber + " - " + da.saving.client.surName + "," + da.saving.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.saving.savingNo, ent, userName,
                 da.saving.client.branchID);
                js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == da.saving.savingType.chargesIncomeAccountID);
                ent.Entry(js).State = EntityState.Detached;
                jb.jnl.Add(js);
                js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == da.saving.savingType.interestPayableAccountID);
                ent.Entry(js).State = EntityState.Detached;
                jb.jnl.Add(js);
            }
            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public void PostSavingAdditional(savingAdditional da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                { 
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", acctID,
                da.saving.savingType.accountsPayableAccountID.Value, da.savingAmount,
                "Deposit into Savings Account - " + da.savingAmount
                + " - " + da.saving.client.accountNumber + " - " + da.saving.client.surName + "," + da.saving.client.otherNames,
                pro.currency_id.Value, da.savingDate, da.saving.savingNo, ent, userName,
                da.saving.client.branchID);
            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public void PostInvestmentWithdrawal(investmentWithdrawal da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN",
                acctID, 
                da.investment.investmentType.accountsPayableAccountID.Value, 
                (da.interestWithdrawal + da.principalWithdrawal),
                "Withdrawal from Investment - " + (da.principalWithdrawal + da.interestWithdrawal).ToString("#,###.#0")
                + " - " + da.investment.client.accountNumber + " - " + da.investment.client.surName + "," + da.investment.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.investment.investmentNo, ent, userName,
                 da.investment.client.branchID);

            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public void PostInvestmentAdditional(investmentAdditional da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", 
                da.investment.investmentType.accountsPayableAccountID.Value, 
                acctID,
                da.investmentAmount,
                "Investment Placed - " + da.investmentAmount
                + " - " + da.investment.client.accountNumber + " - " + da.investment.client.surName + "," + da.investment.client.otherNames,
                pro.currency_id.Value, da.investmentDate, da.investment.investmentNo, ent, userName,
                da.investment.client.branchID);
            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public void CloseSavingsWithdrawal(savingWithdrawal da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct, ref jnl_batch batch)
        {
            var prof = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if ((da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null) || (da.closed == true) || (da.posted == false))
            {
                return;
            }
            if (prof.comp_name.ToLower().Contains("jireh") == false)
            {
                if (batch == null)
                {
                    batch = journalextensions.Post("LN",
                        acctID,
                        da.saving.savingType.vaultAccountID.Value,
                        da.principalWithdrawal + da.interestWithdrawal,
                        "Daily Posting - " + userName + " - " + da.withdrawalDate.ToString("dd-MMM-yyyy"),
                        prof.currency_id.Value, da.withdrawalDate, da.saving.savingNo, ent, userName,
                        da.saving.client.branchID);
                }
                else
                {
                    jnl j = journalextensions.Post("LN", "DR",
                        acctID, da.principalWithdrawal + da.interestWithdrawal,
                        "Daily Posting - " + userName + " - " + da.withdrawalDate.ToString("dd-MMM-yyyy"),
                        prof.currency_id.Value, da.withdrawalDate, userName, ent, userName, da.saving.client.branchID);
                    batch.jnl.Add(j);
                    j = batch.jnl.FirstOrDefault(p => p.accts.acct_id == da.saving.savingType.vaultAccountID);
                    if (j == null)
                    {
                        j = journalextensions.Post("LN", "CR", da.saving.savingType.vaultAccountID.Value,
                            da.principalWithdrawal + da.interestWithdrawal,
                            "Daily Posting - " + userName + " - " + da.withdrawalDate.ToString("dd-MMM-yyyy"),
                            prof.currency_id.Value, da.withdrawalDate, userName, ent, userName,
                            da.saving.client.branchID);
                        batch.jnl.Add(j);
                    }
                    else
                    {
                        j.crdt_amt += da.principalWithdrawal + da.interestWithdrawal;
                    }
                }
            }
            da.closed = true;
        }

        public void CloseSavingAdditional(savingAdditional da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct, ref jnl_batch batch)
        {
            var prof = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if ((da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)|| (da.closed==true)|| (da.posted==false))
            {
                return;
            }
            if (prof.comp_name.ToLower().Contains("jireh") == false)
            {
                if (batch == null)
                {
                    batch = journalextensions.Post("LN",
                        da.saving.savingType.vaultAccountID.Value,
                        acctID, da.savingAmount,
                        "Daily Posting - " + userName + " - " + da.savingDate.ToString("dd-MMM-yyyy"),
                        prof.currency_id.Value, da.savingDate, da.saving.savingNo, ent, userName,
                        da.saving.client.branchID);
                }
                else
                {
                    jnl j = journalextensions.Post("LN", "CR",
                        acctID, da.savingAmount,
                        "Daily Posting - " + userName + " - " + da.savingDate.ToString("dd-MMM-yyyy"),
                        prof.currency_id.Value, da.savingDate, userName, ent, userName, da.saving.client.branchID);
                    batch.jnl.Add(j);
                    j = batch.jnl.FirstOrDefault(p => p.accts.acct_id == da.saving.savingType.vaultAccountID);
                    if (j == null)
                    {
                        j = journalextensions.Post("LN", "DR", da.saving.savingType.vaultAccountID.Value,
                            da.savingAmount,
                            "Daily Posting - " + userName + " - " + da.savingDate.ToString("dd-MMM-yyyy"),
                            prof.currency_id.Value, da.savingDate, userName, ent, userName, da.saving.client.branchID);
                        batch.jnl.Add(j);
                    }
                    else
                    {
                        j.dbt_amt += da.savingAmount;
                    }
                }
            }
            da.closed = true;
        }
    }
}
