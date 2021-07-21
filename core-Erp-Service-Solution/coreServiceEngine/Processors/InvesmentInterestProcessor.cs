using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreService.Processors
{
    public class InvestmentInterestProcessor
    {
        private readonly IJournalExtensions journalextensions;
        private readonly Icore_dbEntities ent;
        private readonly IcoreLoansEntities le;
        private readonly int investmentId ;

        public InvestmentInterestProcessor(int investmentId, Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            journalextensions = new JournalExtensions();
            this.investmentId = investmentId;
            ent = sent;
            le = lent;
        }

        public void Process()
        {
            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var ln = le.investments.First(p => p.investmentID == investmentId);
            var pro = ent.comp_prof.First();

            DateTime? date2 = null;
            if (ln.lastInterestDate == null)
                date2 = ln.firstinvestmentDate;
            else
                date2 = ln.lastInterestDate;
            if (ln.maturityDate < date) date = ln.maturityDate.Value;
            if (date2 > date) return;

            if (date2.Value.Date.AddMonths(1) <= date) date = date2.Value.Date.AddMonths(1);
            var interest = 0.0;
            var diff = (double)(date - date2.Value).TotalDays;
            var duration = (double)(ln.maturityDate.Value - ln.firstinvestmentDate).TotalDays;
            interest += Math.Round((diff / duration) * (ln.period / 12.0) * (ln.interestRate * 12.0) * ln.principalBalance / 100.0, 6);

            if (interest > 0)
            {
                var inte = new investmentInterest
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
                ln.investmentInterests.Add(inte);

                var jb = journalextensions.Post("LN", ln.investmentType.interestReceivableAccountID.Value,
                    ln.investmentType.interestExpenseAccountID.Value,  (interest),
                    "Interest Calculated on Company Investment - " + (interest).ToString("#,###.#0")
                    + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, date, ln.investmentNo, ent, "SYSTEM", ln.client.branchID);

                ent.jnl_batch.Add(jb);

            }
            ln.lastInterestDate = date;
            ln.interestAccumulated += interest;
            ln.interestBalance += interest;
        }
    }
}
