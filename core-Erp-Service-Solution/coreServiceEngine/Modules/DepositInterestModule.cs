using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;
using coreService.Processors;
using coreData.ErrorLog;

namespace coreService
{
    public class DepositInterestModule
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;

        public bool StopFlag;
        public bool Stopped;

        public DepositInterestModule(Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            le = lent;
            ent = sent;
        }

        public DepositInterestModule()
        {
            le = new coreLoansEntities();
            ent = new core_dbEntities();
        }


        public void Main()
        {
            try
            {
                //Logger.serviceError("Deposit Interest Module main Initialize");

                IJournalExtensions journalextensions = new JournalExtensions();
                Stopped = false;
                StopFlag = false;
                var sysDate = le.systemDates.FirstOrDefault();
                var todaysDate = DateTime.Today;
                if (StopFlag && System.Configuration.ConfigurationManager.AppSettings["useSystemDateForDepositInterestCalculation"] == "Y"
                    && sysDate != null && sysDate.depositSystemDate != null)
                {
                    todaysDate = sysDate.depositSystemDate.Value.Date;
                }

                System.Threading.Thread.Sleep(20000);
                //if (StopFlag == true) break;

                while (!StopFlag && System.Configuration.ConfigurationManager.AppSettings["interestEnabled"] == "Y")
                {
                    try
                    {
                        //Logger.serviceError("Calculating for each Deposit");
                        var pro = ent.comp_prof.FirstOrDefault();

                        var date = todaysDate;
                        var lns = le.deposits.Where(p => (!p.closed) && (p.principalBalance > 1 && p.lastInterestDate == null
                            || (p.principalBalance > 1 && p.lastInterestDate < date && p.lastInterestDate <= p.maturityDate))).ToList();
                        foreach (var ln in lns)
                        {
                            if (ln.lastInterestDate == null && ln.firstDepositDate.AddDays(1) >= date) continue;
                            var processor = new DepositInterestProcessor(ln.depositID, date, ent, le);
                            processor.Process();
                        }
                        le.SaveChanges();
                        ent.SaveChanges();

                        GC.Collect();
                    }
                    catch (Exception x)
                    {
                        ExceptionManager.LogException(x, "DepositInterestModule.Main");
                    }

                    System.Threading.Thread.Sleep(20000);
                    if (StopFlag == true) break;

                }
                Stopped = true;
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
            }

            
        }
    }
}
