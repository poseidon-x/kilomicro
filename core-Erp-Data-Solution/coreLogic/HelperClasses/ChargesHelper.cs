using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace coreLogic
{
    public class ChargesHelper : coreLogic.IChargesHelper
    {
        public SavingWithdrawalCalcModel ComputeCharges(int savingId, double takeHomeAmount, DateTime withdrawalDate)
        {
            var withModel = new SavingWithdrawalCalcModel();

            using (var le = new coreLoansEntities())
            {
                var sav = le.savings.First(p => p.savingID == savingId);
                var tier = le.chargeTypeTiers
                        .Include(p => p.chargeType)
                    .FirstOrDefault(p => p.chargeType.chargeTypeCode == "EWC"
                        && p.minimumTransactionAmount <= takeHomeAmount && p.maximumTransactionAmount >= takeHomeAmount);

                if (tier == null)
                {
                    tier = le.chargeTypeTiers
                        .Include(p=> p.chargeType)
                    .Where(p => p.chargeType.chargeTypeCode == "EWC"
                        && p.minimumTransactionAmount <= takeHomeAmount)
                    .OrderByDescending(p => p.maximumTransactionAmount)
                    .FirstOrDefault();
                    if (tier == null)
                    {
                        tier = le.chargeTypeTiers
                        .Include(p => p.chargeType)
                        .Where(p => p.chargeType.chargeTypeCode == "EWC")
                        .OrderBy(p => p.maximumTransactionAmount)
                        .FirstOrDefault();
                        if (tier == null)
                        {
                            throw new ApplicationException("No Charging Tiers Found");
                        }
                    }

                }
                if (takeHomeAmount > sav.principalBalance + sav.interestBalance)
                {
                    throw new ApplicationException("Not Enough Balance for the withdrawal");
                }

                try
                {
                    CalcBalance(le, sav);
                }
                catch (Exception) { }

                if (sav.maturityDate <= withdrawalDate)
                {
                    withModel.principalCharges = 0;
                    withModel.interestCharges = 0;
                    withModel.principalWithdrawal = (takeHomeAmount <= sav.principalBalance) ? takeHomeAmount : sav.principalBalance;
                    withModel.interestWithdrawal = takeHomeAmount - withModel.principalWithdrawal;

                    withModel.chargeTypeTier = tier;
                    return withModel;
                }
                else
                {
                    //Not matured, now take away the charges
                    var chargeAmount = 0.0;
                    if (takeHomeAmount <= sav.availablePrincipalBalance)
                    {
                        chargeAmount = sav.interestBalance * (tier.percentCharge / 100.0);
                    }
                    else
                    {
                        chargeAmount = sav.interestBalance * ((tier.percentCharge+tier.percentCharge) / 100.0);
                    }
                    //
                    if (chargeAmount < tier.minChargeAmount)
                    {
                        chargeAmount = tier.minChargeAmount;
                    }
                    //Check if he can afford the charges + take home
                    if (sav.principalBalance + sav.interestBalance < takeHomeAmount + chargeAmount)
                    {
                        throw new ApplicationException("Not Enough Balance for the withdrawal");
                    }
                    //Split up the charges 
                    withModel.principalWithdrawal = (takeHomeAmount <= sav.principalBalance) ? takeHomeAmount : sav.principalBalance;
                    withModel.interestCharges = (chargeAmount <= sav.interestBalance) ? chargeAmount : sav.interestBalance;
                    withModel.principalCharges = 0.0;
                    withModel.interestWithdrawal = (takeHomeAmount - withModel.principalWithdrawal + chargeAmount
                                                    <= sav.interestBalance)
                        ? takeHomeAmount - withModel.principalWithdrawal
                        : sav.interestBalance - chargeAmount;
                }

                withModel.chargeTypeTier = tier;
            }
            return withModel;
        }

        public SavingWithdrawalCalcModel EmptyAccount(int savingId)
        {
            var withModel = new SavingWithdrawalCalcModel();

            using (var le = new coreLoansEntities())
            {
                var sav = le.savings.First(p => p.savingID == savingId);
                var tier = le.chargeTypeTiers
                    .FirstOrDefault(p => p.chargeType.chargeTypeCode == "EWC"
                        && p.minimumTransactionAmount <= sav.principalBalance && p.maximumTransactionAmount >= sav.principalBalance);

                if (tier == null)
                {
                    tier = le.chargeTypeTiers
                    .Where(p => p.chargeType.chargeTypeCode == "EWC"
                        && p.minimumTransactionAmount <= sav.principalBalance)
                    .OrderByDescending(p => p.maximumTransactionAmount)
                    .FirstOrDefault();
                    if (tier == null)
                    {
                        tier = le.chargeTypeTiers
                        .Where(p => p.chargeType.chargeTypeCode == "EWC")
                        .OrderBy(p => p.maximumTransactionAmount)
                        .FirstOrDefault();
                        if (tier == null)
                        {
                            throw new ApplicationException("No Charging Tiers Found");
                        }
                    }

                }

                try
                {
                    CalcBalance(le, sav);
                }
                catch (Exception) { }

                if (sav.maturityDate <= DateTime.Today)
                {
                    withModel.principalCharges = 0;
                    withModel.interestCharges = 0;
                    withModel.interestWithdrawal = sav.interestBalance;
                    withModel.principalWithdrawal = sav.principalBalance;

                    return withModel;
                }
                else
                {
                    //Not matured, now take away the charges
                    var chargeAmount = sav.interestBalance * ((tier.percentCharge + tier.percentCharge) / 100.0);
                    return ComputeCharges(savingId, sav.principalBalance + sav.interestBalance - chargeAmount, DateTime.Today);
                }

                withModel.chargeTypeTier = tier;
            }
            return withModel;
        }

        private void CalcBalance(coreLoansEntities le, saving ln)
        {
            var config = le.savingConfigs.FirstOrDefault(p => p.savingTypeID == ln.savingTypeID);
            if (config == null) config = le.savingConfigs.FirstOrDefault();
            if (config == null) return;

            using (var rent = new coreReports.reportEntities())
            {
                var date = DateTime.Now.Date.AddDays(-config.principalBalanceLatency);
                var date2 = DateTime.Now.Date.AddDays(-config.interestBalanceLatency);
                var curPrincBal = rent.vwSavingStatements
                    .Where(p => p.loanID == ln.savingID).Sum(p => p.depositAmount)
                        - rent.vwSavingStatements.Where(p => p.loanID == ln.savingID)
                         .Sum(p => p.princWithdrawalAmount + p.chargeAmount);
                var availPrincBal = rent.vwSavingStatements.Where(p => p.loanID == ln.savingID
                    && ((p.date <= date && p.maturityDate > date) || (p.maturityDate <= date))).Sum(p => p.depositAmount)
                        - rent.vwSavingStatements.Where(p => p.loanID == ln.savingID)
                         .Sum(p => p.princWithdrawalAmount);
                var curIntBal = rent.vwSavingStatements.Where(p => p.loanID == ln.savingID).Sum(p => p.interestAccruedAmount)
                        - rent.vwSavingStatements.Where(p => p.loanID == ln.savingID)
                         .Sum(p => p.intWithdrawalAmount);
                var availIntBal = rent.vwSavingStatements.Where(p => p.loanID == ln.savingID
                    && ((p.date <= date && p.maturityDate > date) || (p.maturityDate <= date))).Sum(p => p.interestAccruedAmount)
                        - rent.vwSavingStatements.Where(p => p.loanID == ln.savingID)
                         .Sum(p => p.intWithdrawalAmount);
                ln.availableInterestBalance = availIntBal;
                ln.availablePrincipalBalance = availPrincBal;
                ln.clearedInterestBalance = curIntBal;
                ln.clearedPrincipalBalance = curPrincBal;
                ln.interestBalance = curIntBal;
                ln.principalBalance = curPrincBal;

                le.SaveChanges();
            }
        }
    }
}