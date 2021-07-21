using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;

namespace coreService
{
    public class SavingsBalanceModule
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        public bool StopFlag;
        public bool Stopped;

        public void Main()
        {
            Stopped = false;
            StopFlag = false;

            while (!StopFlag && System.Configuration.ConfigurationManager.AppSettings["savingsInterestEnabled"]=="Y")
            {
                try
                {
                    coreLoansEntities le = new coreLoansEntities(); 
                    coreReports.reportEntities rent = new coreReports.reportEntities();
                     
                    var lns = le.savings.Where(p=> p.principalBalance>1).ToList();
                    foreach (var ln in lns)
                    {

                        try
                        {
                            var config = le.savingConfigs.FirstOrDefault(p => p.savingTypeID == ln.savingTypeID);
                            if (config == null) config = le.savingConfigs.FirstOrDefault();
                            if (config == null) continue;

                            var date = DateTime.Now.Date.AddDays(-config.principalBalanceLatency);
                            var date2 = DateTime.Now.Date.AddDays(-config.interestBalanceLatency);
                            var curPrincBal = rent.vwSavingStatements.Where(p => p.loanID == ln.savingID).Sum(p => p.depositAmount)
                                    - rent.vwSavingStatements.Where(p => p.loanID == ln.savingID)
                                     .Sum(p => p.princWithdrawalAmount + p.chargeAmount);
                            var availPrincBal = rent.vwSavingStatements.Where(p => p.loanID == ln.savingID 
                                && ((p.date <= date && p.maturityDate>date)||(p.maturityDate<=date))).Sum(p => p.depositAmount)
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
                        catch (Exception )
                        {
                            //ExceptionManager.LogException(x2, "SavingsBalanceModule.Main");
                        }
                    }
                    
                    lns = null;
                    le.Dispose();
                    le = null;
                    GC.Collect();
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "SavingsBalanceModule.Main");
                }

                System.Threading.Thread.Sleep(20000);
                if (StopFlag == true) break;

            }
            Stopped = true;
        } 
    }
}
