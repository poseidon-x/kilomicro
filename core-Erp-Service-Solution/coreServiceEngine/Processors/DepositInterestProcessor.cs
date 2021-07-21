using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreData.ErrorLog;
using coreLogic;

namespace coreService.Processors
{
    public class DepositInterestProcessor
    {
        private readonly IJournalExtensions journalextensions;
        private readonly Icore_dbEntities ent;
        private readonly IcoreLoansEntities le;
        private readonly int depositId;
        private readonly comp_prof comp;
        public readonly DateTime date;


        public DepositInterestProcessor(int depositId,DateTime date, Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            Logger.serviceError("Deposit Interest Processor Contructor initialized");

            journalextensions = new JournalExtensions();
            this.depositId = depositId;
            this.date = date;
            comp = sent.comp_prof.FirstOrDefault();
            ent = sent;
            le = lent;
        }

        public void Process()
        {
            try
            {
                //Logger.serviceError("Deposit Interest Processor initialized");
                //Logger.serviceError("Calculating for deposit Id :" + this.depositId);

                var date = this.date;

                var ln = le.deposits
                .FirstOrDefault(p => p.depositID == depositId);
                ln.depositAdditionals = le.depositAdditionals.Where(p => p.depositID == ln.depositID && p.posted).ToList();
                ln.depositWithdrawals = le.depositWithdrawals.Where(p => p.depositID == ln.depositID && p.posted).ToList();

                var pro = ent.comp_prof.First();

                DateTime? date2 = null;
                if (ln.lastInterestDate == null)
                    date2 = ln.firstDepositDate;
                else
                    date2 = ln.lastInterestDate;

                if (ln.maturityDate < date) date = ln.maturityDate.Value;
                if (date2 > date) return;


                var interest = 0.0;
                var diff = (double)(date - date2.Value).TotalDays;
                var duration = (double)(ln.maturityDate.Value - ln.firstDepositDate).TotalDays;
                double principalBalance = getPrincipalBalanceAsAtToday(ln, date);
                var comName = comp.comp_name.ToLower();
                if (comName.Contains("ttl") || comName.Contains("eclipse"))
                {
                    interest = getDepositInterestAsAtToday(ln, date2.Value, date);
                }
                else
                {
                    interest += Math.Round((diff / duration) * (ln.period / 12.0) * (ln.interestRate * 12.0) * ln.principalBalance / 100.0, 6);
                }

                Logger.serviceError("Interest for deposit Id :" + interest);
                if (interest > 0 && comp.comp_name.ToLower().Contains("ttl"))
                {
                    var inte = new depositInterest
                    {
                        principal = principalBalance,
                        interestAmount = interest,
                        interestRate = ln.interestRate,
                        creation_date = DateTime.Now,
                        creator = "SYSTEM",
                        interestDate = date,
                        fromDate = date2.Value,
                        toDate = date,
                        proposedAmount = interest
                    };
                    ln.depositInterests.Add(inte);

                    var jb = journalextensions.Post("LN", ln.depositType.interestExpenseAccountID.Value,
                        ln.depositType.interestPayableAccountID.Value, (interest),
                        "Interest Calculated on Deposit - " + (interest).ToString("#,###.#0")
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, date, ln.depositNo, ent, "SYSTEM", ln.client.branchID);

                    ent.jnl_batch.Add(jb);

                    ln.lastInterestDate = date;
                    ln.interestAccumulated += interest;
                    ln.interestBalance += interest;
                }
                else if (interest > 0)
                {
                    var inte = new depositInterest
                    {
                        principal = ln.principalBalance,
                        interestAmount = interest,
                        interestRate = ln.interestRate,
                        creation_date = DateTime.Now,
                        creator = "SYSTEM",
                        interestDate = date,
                        fromDate = date2.Value,
                        toDate = date,
                        proposedAmount = interest
                    };
                    ln.depositInterests.Add(inte);

                    var jb = journalextensions.Post("LN", ln.depositType.interestExpenseAccountID.Value,
                        ln.depositType.interestPayableAccountID.Value, (interest),
                        "Interest Calculated on Deposit - " + (interest).ToString("#,###.#0")
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, date, ln.depositNo, ent, "SYSTEM", ln.client.branchID);

                    ent.jnl_batch.Add(jb);

                    ln.lastInterestDate = date;
                    ln.interestAccumulated += interest;
                    ln.interestBalance += interest;

                }
                
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
            }


            
        }

        private double getPrincipalBalanceAsAtToday(deposit dp, DateTime todaysDate)
        {
            var deposit = dp;

            double totalDeposit = 0;
            double totalPrincipalWithdrawal = 0;

            foreach (var add in deposit.depositAdditionals.Where(p => p.posted))
            {
                if (add.depositDate.Date <= todaysDate.Date)
                    totalDeposit += add.depositAmount;
            }

            foreach (var withd in deposit.depositWithdrawals.Where(p => p.posted))
            {
                if (withd.withdrawalDate.Date <= todaysDate.Date)
                    totalPrincipalWithdrawal += withd.principalWithdrawal;
            }
            double balance = totalDeposit - totalPrincipalWithdrawal;
            if (balance > 0) return balance;
            return 0;
        }





        private double getDepositInterestAsAtToday(deposit dp, DateTime startDate, DateTime todaysDate)
        {
            double totalDeposit = 0;
            double totalPrincipalWithdrawal = 0;

            double interestForToday = 0;
            var totalWithdrawal = dp.depositWithdrawals
                                    .Where(p => p.posted && p.withdrawalDate.Date < todaysDate.Date)
                                    .Sum(p => p.principalWithdrawal);

            foreach (var add in dp.depositAdditionals.Where(p => p.posted))
            {
                if (todaysDate.Date < add.depositDate.Date) continue;

                if (totalWithdrawal >= add.depositAmount)
                {
                    totalWithdrawal -= add.depositAmount;
                    continue;
                }

                var additionalBalance = add.depositAmount - totalWithdrawal;
                if (additionalBalance <= 0) continue;

                DateTime dateToCalcFrom = add.depositDate;
                if (add.lastInterestDate != null) dateToCalcFrom = add.lastInterestDate.Value;
                var d = todaysDate.Date - dateToCalcFrom.Date;
                var interestForDays = (double)d.TotalDays;

                var a = (dp.annualInterestRate / 100);
                var b = (dp.annualInterestRate / 100) / 365;
                var c = interestForDays * additionalBalance;
                var dd = b * c;
                var interest = (dp.annualInterestRate / 100) / 365 * interestForDays * additionalBalance;

                interestForToday += interest;
                add.lastInterestDate = todaysDate;
            }

            if (dp.depositType.isCompoundInterest)
            {
                var eom = new DateTime(todaysDate.Year, todaysDate.Month, DateTime.DaysInMonth(todaysDate.Year, todaysDate.Month));

                DateTime dateToCalcFrom = startDate;
                var d = todaysDate.Date - dateToCalcFrom.Date;
                var interestForDays = (double)d.TotalDays;

                //Calculate compound interest if its end of month && maturity Date
                if (todaysDate.Date == eom || (dp.maturityDate != null && todaysDate.Date == dp.maturityDate.Value.Date))
                {
                    var interest = (dp.annualInterestRate / 100) / 365 * interestForDays * dp.interestBalance;
                    interestForToday += interest;
                }


            }

            return interestForToday;
        }
    }
}
