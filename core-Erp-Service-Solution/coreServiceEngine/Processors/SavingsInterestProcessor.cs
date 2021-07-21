using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreReports;
using coreService;

namespace coreLogic.Processors
{
    public class SavingsInterestProcessor
    {
        private int savingId;
        private DateTime currentDate;
        private DateTime todaysDate;
        private IcoreLoansEntities le;
        private Icore_dbEntities ent;
        private IreportEntities rent;
        public SavingsInterestProcessor(int savingId, DateTime date, DateTime date2, IcoreLoansEntities le, Icore_dbEntities ent,
            IreportEntities rent)
        {
            this.savingId = savingId;
            currentDate = date2;
            todaysDate = date;
            this.le = le;
            this.ent = ent;
            this.rent = rent;
        }

        public void Process()
        {
            var sysDate = le.systemDates.FirstOrDefault();
            var ln = le.savings.First(p => p.savingID == savingId);
            DateTime? date2 = null;
            if (ln.lastInterestDate == null)
                date2 = ln.firstSavingDate.AddDays(1);
            else
                date2 = ln.lastInterestDate.Value.AddDays(1);

            if (sysDate != null && sysDate.savingSystemDate < date2) //skip if moved beyond system date
            {
                return;
            }
            if ((ln.maturityDate < date2)) //Skip if moved past maturity date
            {
                return;
            }
            if (currentDate < todaysDate)
            {
                var config = le.savingConfigs.FirstOrDefault(p => p.savingTypeID == ln.savingTypeID) ??
                             le.savingConfigs.FirstOrDefault();
                if (config == null) return;
                var balanceAsAt = GetBalanceAsAt(date2.Value, ln.savingID, config);

                var interest = 0.0;
                interest = (1.0/365.0)*(ln.interestRate*12.0)
                           *balanceAsAt/100.0;
                //var cur = ent.currencies.FirstOrDefault(p => p.currency_id == ln.currencyID);

                ln.lastInterestDate = date2;
                le.savingDailyInterests.Add(new savingDailyInterest
                {
                    interestDate = date2.Value,
                    applied = false,
                    balanceAsAt = balanceAsAt,
                    interestAmount = interest,
                    saving = ln,
                    savingID = savingId
                });
            }

            le.SaveChanges();
            ent.SaveChanges();
        }

        public void ProcessMonthEnd()
        {
            IJournalExtensions journalextensions = new JournalExtensions(ent, le);
            var pro = ent.comp_prof.First();
            var unapp = le.savingDailyInterests.Where(p => p.applied == false && p.interestDate <= todaysDate
                                                           && p.savingID == savingId)
                .Select(p => p.savingID)
                .Distinct()
                .ToList();
            foreach (var una in unapp)
            {
                var firsttUnAccumDate = le.savingDailyInterests.Where(p => p.applied == false && p.savingID == una)
                    .Min(p => p.interestDate);
                var lasttUnAccumDate = le.savingDailyInterests.Where(p => p.applied == false && p.savingID == una)
                    .Max(p => p.interestDate);
                if (firsttUnAccumDate != null)
                {
                    var sv = le.savings.FirstOrDefault(p => p.savingID == una);
                    var lastIntDate = sv.firstSavingDate;
                    var lastInterest =
                        sv.savingInterests.OrderByDescending(p => p.interestDate).FirstOrDefault();
                    if (lastInterest != null)
                    {
                        lastIntDate = lastInterest.interestDate;
                    }
                    var eomDate =
                        (new DateTime(firsttUnAccumDate.Year, firsttUnAccumDate.Month, 1)).AddMonths(1)
                            .AddSeconds(-1)
                            .Date;
                    if (eomDate > sv.maturityDate) eomDate = sv.maturityDate.Value;
                    if (eomDate < todaysDate
                        && firsttUnAccumDate <= eomDate
                        && lasttUnAccumDate >= eomDate
                        && ((eomDate - lastIntDate).TotalDays >= sv.savingType.minDaysBeforeInterest)
                        || (eomDate == sv.maturityDate)
                        )
                    {
                        try
                        {
                            var qry=le.savingDailyInterests.Where(p => p.savingID == una
                                                                                    && p.applied == false
                                                                                    && p.interestDate <= eomDate);
                            if (qry.Any())
                            {
                                var totalUnApplied = Math.Round(
                                    qry.Sum(p => p.interestAmount), 2);

                                if (totalUnApplied >= 0)
                                {
                                    var config =
                                        le.savingConfigs.FirstOrDefault(p => p.savingTypeID == sv.savingTypeID);
                                    if (config == null) config = le.savingConfigs.FirstOrDefault();
                                    if (config == null) return;
                                    var inte = new coreLogic.savingInterest
                                    {
                                        interestAmount = Math.Round(totalUnApplied, config.interestDecimalPlaces),
                                        fromDate = firsttUnAccumDate,
                                        toDate = eomDate,
                                        creation_date = DateTime.Now,
                                        creator = "SYSTEM",
                                        fxRate = 1.0,
                                        interestDate = eomDate,
                                        principal = 0,
                                        proposedAmount = sv.principalBalance,
                                        localAmount = totalUnApplied,
                                        savingID = una,
                                        interestBalance = totalUnApplied,
                                        interestRate = sv.interestRate
                                    };
                                    sv.savingInterests.Add(inte);

                                    sv.interestAccumulated = sv.interestAccumulated + totalUnApplied;
                                    if (config.accrueInterestToPrincipal == true)
                                    {
                                        sv.interestAccumulated = sv.interestAccumulated + totalUnApplied;
                                        sv.principalBalance = sv.principalBalance + totalUnApplied;
                                    }
                                    else
                                    {
                                        sv.interestBalance = sv.interestBalance + totalUnApplied;
                                        sv.interestAuthorized = sv.interestAuthorized + totalUnApplied;
                                    }
                                    foreach (
                                        var r in
                                            sv.savingDailyInterests.Where(
                                                p =>
                                                    p.savingID == una && p.applied == false &&
                                                    p.interestDate <= eomDate).ToList())
                                    {
                                        r.applied = true;
                                    }

                                    var jb = journalextensions.Post("LN",
                                        sv.savingType.interestExpenseAccountID.Value,
                                        sv.savingType.interestPayableAccountID, totalUnApplied,
                                        "Interest Accummulated on Deposits & Savings - " + sv.client.surName + "," +
                                        sv.client.otherNames,
                                        pro.currency_id.Value, eomDate, sv.savingNo, ent, "SYSTEM",
                                        sv.client.branchID);
                                    ent.jnl_batch.Add(jb);

                                    le.SaveChanges();
                                    ent.SaveChanges();
                                }
                            }
                        }
                        catch (Exception xi)
                        {
                            ExceptionManager.LogException(xi, "SavingsInterestModule.Process");
                            throw xi;
                        }
                    }
                }
            }

        }

        private double GetBalanceAsAt(DateTime datei, int savingId, savingConfig config)
        {
            var date = datei.Date.AddDays(-config.principalBalanceLatency);
            var date2 = datei.Date.AddDays(-config.interestBalanceLatency);
            var bal = 0.0;

            var availIntBalQuery = rent.vwSavingStatements.Where(p => p.loanID == savingId && p.date < date2);
            var savingsTranQuery = rent.vwSavingStatements.Where(p => p.loanID == savingId);
            var balQuery = rent.vwSavingStatements.Where(p => p.loanID == savingId && p.date < datei);

            var curPrincBal = (savingsTranQuery.Any() ? savingsTranQuery.Sum(p => p.depositAmount) : 0.0)
                              -
                              (savingsTranQuery.Any()
                                  ? savingsTranQuery.Sum(p => p.princWithdrawalAmount + p.chargeAmount)
                                  : 0.0);
            var availIntBal = (availIntBalQuery.Any() ? availIntBalQuery.Sum(p => p.interestAccruedAmount) : 0.0)
                              - (savingsTranQuery.Any() ? savingsTranQuery.Sum(p => p.intWithdrawalAmount) : 0);
            bal = (balQuery.Any() ? balQuery.Sum(p => p.depositAmount - p.princWithdrawalAmount - p.chargeAmount) : 0.0);
            if (config.accrueInterestToPrincipal == true) bal = bal + availIntBal;

            if (bal < 0) bal = 0;

            return bal;
        }
    }
}
