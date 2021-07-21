using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;
using coreLogic.Processors;
using coreReports;

namespace coreService
{
    public class SavingsInterestModule
    { 
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;
        private readonly IreportEntities rent;
        private readonly string serviceFlag ;
        public bool StopFlag;
        public bool Stopped;
        
        public SavingsInterestModule(Icore_dbEntities sent, IcoreLoansEntities lent, IreportEntities rrent,
            string serviceEnabledFlag)
        {
            le = lent;
            ent = sent;
            rent = rrent;
            serviceFlag = serviceEnabledFlag;
        }

        public SavingsInterestModule()
        {
            le = new coreLoansEntities();
            ent = new core_dbEntities();
            rent = new reportEntities();
            serviceFlag = System.Configuration.ConfigurationManager.AppSettings["savingsInterestEnabled"];
        }

        public void Main()
        {
            Stopped = false;
            StopFlag = false;

            while (!StopFlag && serviceFlag=="Y")
            {
                try
                {  
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); 
                    var lns = le.savings.Where(p=> ((p.lastInterestDate == null || p.lastInterestDate < date) 
                        && (p.interestBalance+p.principalBalance>3)))
                        .ToList();
                    foreach (var ln in lns)
                    {
                        DateTime? date2 = null;
                        if (ln.lastInterestDate == null)
                            date2 = ln.firstSavingDate.AddDays(1);
                        else
                            date2 = ln.lastInterestDate.Value.AddDays(1);

                        var processor = new SavingsInterestProcessor(ln.savingID, date, date2.Value, le, ent, rent);
                        processor.Process();
                        processor.ProcessMonthEnd();
                    }
                    

                    GC.Collect();
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "SavingsInterestModule.Main");
                }

                System.Threading.Thread.Sleep(3000);
                if (StopFlag == true) break;

            }
            Stopped = true;
        }
    }
}
