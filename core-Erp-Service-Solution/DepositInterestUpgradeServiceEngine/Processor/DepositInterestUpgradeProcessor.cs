using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreData.ErrorLog;
using coreLogic;

namespace DepositInterestUpgradeServiceEngine.Processor
{
    public class DepositInterestUpgradeProcessor
    {
        private readonly Icore_dbEntities ent;
        private readonly IcoreLoansEntities le;
        private readonly int depositId;
        public readonly DateTime date;



        public DepositInterestUpgradeProcessor(int depositId, DateTime date, Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            Logger.serviceError("Deposit Interest Upgrade Processor Contructor initialized");

            this.depositId = depositId;
            this.date = date;
            ent = sent;
            le = lent;
        }

        public void Process()
        {
            Logger.serviceError("Deposit Interest Upgrade Processor process method initialized");

            var date = this.date;

            var ln = le.deposits
            .Include(p => p.depositAdditionals)
            .Include(p => p.depositWithdrawals)
            .Include(p => p.depositType)
            .Include(p => p.depositType.depositTypePlanRates)
            .Include(p => p.depositRateUpgrades)
            .FirstOrDefault(p => p.depositID == depositId);
            if (ln == null || !ln.depositType.depositInterestUpgradeable) return;
            
            double principalBalance = getPrincipalBalAsAtDate(ln, date);
            if ((ln.maturityDate == null || ln.maturityDate <= date) || principalBalance < 1) return;

            var planRate = getPlanRate(ln,principalBalance);

            if (planRate != null && planRate.proposedRate > 0)
            {
                DateTime? lastProposed = null;
                depositRateUpgrade lastupUpgrade = null;


                if (ln.depositRateUpgrades.Any())
                {
                    lastupUpgrade = ln.depositRateUpgrades.OrderByDescending(p => p.created).First();
                    lastProposed = lastupUpgrade.created;
                }

                if (lastProposed == null || lastupUpgrade.approved || (lastupUpgrade.rejected.HasValue && lastupUpgrade.rejected.Value) )
                {
                    ln.depositRateUpgrades.Add(planRate);
                }
                
            }
        }

        //Get Principal Balance as @ Specified date
        private double getPrincipalBalAsAtDate(deposit dep, DateTime date)
        {
            double principalBalance = 0;
            if (dep.depositAdditionals.Any())
            {
                principalBalance = dep.depositAdditionals
                    .Where(p => p.depositDate.Date <= date.Date)
                    .Sum(p => p.depositAmount);
                if (dep.depositWithdrawals.Any())
                {
                    principalBalance -= dep.depositWithdrawals
                        .Where(p => p.withdrawalDate.Date <= date.Date)
                        .Sum(p => p.principalWithdrawal);
                }
            }
            return principalBalance;
        }

        private depositRateUpgrade getProposedUpgrade(depositTypePlanRate depPlan, deposit dep, double principalBalance,DateTime date)
        {
            var upgrade = new depositRateUpgrade
            {
                depositId = dep.depositID,
                currentRate = dep.annualInterestRate,
                proposedRate = depPlan.interestRate,
                currentPrincipalBalance = principalBalance,
                approved = false,
                created = DateTime.Now,
                creator = "SYSTEM"
            };
            return upgrade;
        }

        private depositRateUpgrade getPlanRate(deposit dep, double principalBalance)
        {
            depositRateUpgrade upgrade = new depositRateUpgrade();
            var tbr = le.treasuryBillRates.FirstOrDefault();

            //For Treasury Bill
            if (tbr != null && dep.depositType.depositTypePlanRates.Any(p => p.useTreasuryBillRate && principalBalance >= p.minimumAmount
            && dep.annualInterestRate < tbr.treasuryBillRate1 && !dep.depositRateUpgrades.Any(q => q.isTreasuryBillUpgrade)) )
            {
                var dtpr = dep.depositType.depositTypePlanRates
                    .FirstOrDefault(p => p.useTreasuryBillRate && principalBalance >= p.minimumAmount
                        && dep.annualInterestRate < tbr.treasuryBillRate1 && !dep.depositRateUpgrades.Any(q => q.isTreasuryBillUpgrade));
                
                if (dtpr != null)
                {
                    upgrade.depositId = dep.depositID;
                    upgrade.depositTypePlanRateId = dtpr.depositTypePlanRateId;
                    upgrade.currentRate = dep.annualInterestRate;
                    upgrade.proposedRate = tbr.treasuryBillRate1;
                    upgrade.isTreasuryBillUpgrade = true;
                    upgrade.currentPrincipalBalance = principalBalance;
                    upgrade.approved = false;
                    upgrade.created = DateTime.Now;
                    upgrade.creator = "SYSTEM";
                }
                
            //For Incremental Interest rate
            }else if (dep.depositType.depositTypePlanRates.Any(p => p.useIncrement && principalBalance >= p.minimumAmount
                && !(dep.depositRateUpgrades.Where(q => q.depositTypePlanRateId != null).Select(q => q.depositTypePlanRateId).ToList())
                .Contains(p.depositTypePlanRateId)))
            {
                var dtpr = dep.depositType.depositTypePlanRates
                    .FirstOrDefault(p => p.useIncrement && principalBalance >= p.minimumAmount
                && dep.annualInterestRate < p.interestRate);
                if (dtpr != null)
                {
                    upgrade.depositId = dep.depositID;
                    upgrade.depositTypePlanRateId = dtpr.depositTypePlanRateId;
                    upgrade.currentRate = dep.annualInterestRate+dep.annualInterestRate;
                    upgrade.proposedRate = dtpr.interestRate;
                    upgrade.currentPrincipalBalance = principalBalance;
                    upgrade.approved = false;
                    upgrade.created = DateTime.Now;
                    upgrade.creator = "SYSTEM";
                }
            //For New Interest rate;
            }
            else if (dep.depositType.depositTypePlanRates.Any(p => principalBalance >= p.minimumAmount
               && principalBalance <= p.maximumAmount && dep.annualInterestRate < p.interestRate))
            {
                var dtpr = dep.depositType.depositTypePlanRates
                    .FirstOrDefault(p => principalBalance >= p.minimumAmount
               && principalBalance <= p.maximumAmount && dep.annualInterestRate < p.interestRate);
                if (dtpr != null)
                {
                    upgrade.depositId = dep.depositID;
                    upgrade.depositTypePlanRateId = dtpr.depositTypePlanRateId;
                    upgrade.currentRate = dep.annualInterestRate;
                    upgrade.proposedRate = dtpr.interestRate;
                    upgrade.currentPrincipalBalance = principalBalance;
                    upgrade.approved = false;
                    upgrade.created = DateTime.Now;
                    upgrade.creator = "SYSTEM";
                }
            }

            return upgrade;
        }

    }
}
