using System;
using System.Linq;
using coreLogic;
using coreLogic.Processors;
using coreReports;
using coreServiceEngine.Tests.Initializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace coreServiceEngine.Tests.Processors
{
    [TestClass]
    public class SavingsInterestProcessorTests
    {
        private IcoreLoansEntities le;
        private Icore_dbEntities ent;
        private IreportEntities rent;

        private const int PRINCIPAL_PAYABLE_ACCOUNT_ID = 1;
        private const int INTEREST_PAYABLE_ACCOUNT_ID = 2;
        private const int INTEREST_EXPENSE_ACCOUNT_ID = 3;

        [TestInitialize]
        public void TestInit()
        {
            SavingsInterestInitializer.Init(ref le,ref ent, ref rent);
        }

        [TestMethod]
        public void Process_Interest_For_Null_Last_Interest_Date_Works()
        {
            var firstSaving = le.savings.First();
            var processor = new SavingsInterestProcessor(firstSaving.savingID, DateTime.Now.AddDays(-1),
                DateTime.Now.AddMonths(-3).AddDays(1),
                le, ent, rent);
            processor.Process();
            Assert.IsTrue(le.savingDailyInterests.Any());
            Assert.IsTrue(Math.Abs(Math.Round(le.savingDailyInterests.First().interestAmount, 2) - 0.03945) < 0.001);
            Assert.IsTrue(firstSaving.lastInterestDate == firstSaving.firstSavingDate.AddDays(1));
        }

        [TestMethod]
        public void Process_Interest_For_Non_Null_Last_Interest_Date_Works()
        {
            var firstSaving = le.savings.First();
            firstSaving.lastInterestDate = firstSaving.firstSavingDate.AddDays(1);
            var processor = new SavingsInterestProcessor(firstSaving.savingID, DateTime.Now.AddDays(-1),
                DateTime.Now.AddMonths(-3).AddDays(1),
                le, ent, rent);
            processor.Process();
            Assert.IsTrue(le.savingDailyInterests.Any());
            Assert.IsTrue(Math.Abs(Math.Round(le.savingDailyInterests.First().interestAmount, 2) - 0.03945) < 0.001);
            Assert.IsTrue(firstSaving.lastInterestDate==firstSaving.firstSavingDate.AddDays(2));
        }

        [TestMethod]
        public void Process_Interest_At_Month_End_Generates_Journal_Postings_Correctly()
        {
            var firstSaving = le.savings.First();
            var monthEnd = DateTime.Now.AddMonths(-3);
            monthEnd = new DateTime(monthEnd.Year, monthEnd.Month, 1).AddMonths(1).AddDays(-2);
            firstSaving.lastInterestDate = monthEnd;
            var processor = new SavingsInterestProcessor(firstSaving.savingID, DateTime.Now.AddDays(-1),
                monthEnd,
                le, ent, rent);
            processor.Process();
            processor.ProcessMonthEnd();
            Assert.IsTrue(le.savingDailyInterests.Any());
            Assert.IsTrue(Math.Abs(Math.Round(le.savingDailyInterests.First().interestAmount, 2) - 0.03945) < 0.001);
            Assert.IsTrue(firstSaving.lastInterestDate == monthEnd.AddDays(1));
            Assert.IsTrue(ent.jnl_batch.Any());
            Assert.IsTrue(Math.Abs(Math.Round(ent.jnl_batch.First().jnl
                .First(p=> p.accts.acct_id==INTEREST_PAYABLE_ACCOUNT_ID).crdt_amt, 2) - 0.03945) < 0.001);
        }

    }
}
