using System;
using System.Linq;
using System.Threading;
using coreLogic;
using coreReports;
using coreService;
using coreServiceEngine.Tests.Initializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace coreServiceEngine.Tests.Modules
{
    [TestClass]
    public class SavingsInterestModuleTests
    {
        private IcoreLoansEntities le;
        private Icore_dbEntities ent;
        private IreportEntities rent;

        private const int PRINCIPAL_PAYABLE_ACCOUNT_ID = 1;
        private const int INTEREST_PAYABLE_ACCOUNT_ID = 2;
        private const int INTEREST_EXPENSE_ACCOUNT_ID = 3;

        [TestInitialize]
        public void InitTests()
        {
            SavingsInterestInitializer.Init(ref le, ref ent, ref rent);      
        }

        [TestMethod]
        public void Module_Starts_And_Stops_Correctly()
        {
            SavingsInterestModule module = new SavingsInterestModule(ent, le, rent, "Y");
            Thread th = new Thread(new ThreadStart(module.Main));
            th.Start();
            Assert.IsFalse(module.Stopped);
            module.StopFlag = true;
            DateTime time = DateTime.Now;
            while ((DateTime.Now - time).TotalSeconds < 15 && module.Stopped == false)
            {
                Thread.Sleep(100);
            }
            Assert.IsTrue(module.Stopped);
        }

        [TestMethod]
        public void Module_Wont_Start_If_Flag_Not_Enabled_In_Config_File()
        {
            SavingsInterestModule module = new SavingsInterestModule(ent, le, rent, null);
            Thread th = new Thread(new ThreadStart(module.Main));
            th.Start();
            Thread.Sleep(100);
            Assert.IsTrue(module.Stopped); 
        }

        [TestMethod]
        public void Module_Processes_Correctly()
        {
            SavingsInterestModule module = new SavingsInterestModule(ent, le, rent, "Y");
            Thread th = new Thread(new ThreadStart(module.Main));
            th.Start();
            Assert.IsFalse(module.Stopped);
            module.StopFlag = true;
            DateTime time = DateTime.Now;
            while ((DateTime.Now - time).TotalSeconds < 15 && module.Stopped == false)
            {
                Thread.Sleep(100);
            }
            Assert.IsTrue(module.Stopped); 
            Assert.IsTrue(le.savingDailyInterests.Any());

        }

    }
}
