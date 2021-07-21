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

namespace coreService
{
    public class DepositMaturityNotificationModule
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;

        public bool StopFlag;
        public bool Stopped;

        public DepositMaturityNotificationModule(Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            le = lent;
            ent = sent;
        }

        public DepositMaturityNotificationModule()
        {
            le = new coreLoansEntities();
            ent = new core_dbEntities();
        }


        public void Main()
        {
            Stopped = false;
            StopFlag = false;

            System.Threading.Thread.Sleep(20000);
            //if (StopFlag == true) break;

            while (!StopFlag && System.Configuration.ConfigurationManager.AppSettings["depositMaturityNotificationEnabled"] == "Y")
            {
                try
                { 
                    var pro = ent.comp_prof.FirstOrDefault();
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    if (date.DayOfWeek == DayOfWeek.Thursday && pro.comp_name.ToLower().Contains("eclipse"))
                    {
                        DateTime nextWeekStarts = date.AddDays(7);
                        DateTime nextWeekEnds = date.AddDays(14);

                        var lns = le.deposits.Where(p => date < p.maturityDate && p.principalBalance > 0
                                                         && p.maturityNotificationSent == false && 
                                                         (p.maturityDate >= nextWeekStarts && p.maturityDate <= nextWeekEnds)).ToList();
                        var depositIds = lns.Select(p => p.depositID).ToList();
                        
                        var processor = new DepositMaturityNotificationProcessor(depositIds, le, nextWeekStarts, nextWeekEnds);
                        processor.Process();
                        le.SaveChanges();
                    }
                    
                       
                    GC.Collect();
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "DepositMaturityNotificationModule.Main");
                }

                System.Threading.Thread.Sleep(20000);
                if (StopFlag == true) break;

            }
            Stopped = true;
        }
    }
}
