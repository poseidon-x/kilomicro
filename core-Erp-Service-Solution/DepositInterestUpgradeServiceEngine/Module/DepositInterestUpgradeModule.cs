using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;
using DepositInterestUpgradeServiceEngine.Processor;
using coreLogic.HelperClasses;
using coreData.ErrorLog;


namespace DepositInterestUpgradeServiceEngine.Module
{
    public class DepositInterestUpgradeModule
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;
        private Logger errorLogger;

        public bool StopFlag;
        public bool Stopped;

        
        public DepositInterestUpgradeModule(Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            le = lent;
            ent = sent;
        }

        public DepositInterestUpgradeModule()
        {
            le = new coreLoansEntities();
            ent = new core_dbEntities();
        }

        
        public void Main()
        {
            try
            {
                Logger.serviceError("Deposit Interest Upgrade Module main Initialize");

                Stopped = false;
                StopFlag = false;
                var sysDate = le.systemDates.FirstOrDefault();

                System.Threading.Thread.Sleep(5000);
                Logger.serviceError("Deposit Interest Upgrade Module main about to Initialize");


                while (sysDate != null && sysDate.depositSystemDate != null &&
                        !StopFlag && System.Configuration.ConfigurationManager.AppSettings["interestEnabled"] == "Y")
                {
                    try
                    {
                        Logger.serviceError("Deposit Interest Upgrade Module main Initialize");

                        var pro = ent.comp_prof.FirstOrDefault();
                        var date = sysDate.depositSystemDate.Value;
                        var lns = le.deposits.Where(p => (!p.closed) &&
                        (p.depositType.depositInterestUpgradeable && (p.principalBalance > 0 && p.maturityDate > date))).ToList();
                        foreach (var ln in lns)
                        {
                            Logger.serviceError(ln.depositID.ToString());
                            var processor = new DepositInterestUpgradeProcessor(ln.depositID, date, ent, le);
                            processor.Process();
                        }
                        le.SaveChanges();
                        ent.SaveChanges();

                        GC.Collect();
                    }
                    catch (Exception x)
                    {
                        Logger.logError(x);
                    }

                    System.Threading.Thread.Sleep(20000);
                    if (StopFlag == true) break;

                }
                Stopped = true;
            }
            catch (Exception x)
            {
                Logger.logError(x);
            }




            
        }
    }
}
