using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;
using coreService.Processors;

namespace coreService
{
    public class ClientActivityModule
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;

        public bool StopFlag;
        public bool Stopped;

        public ClientActivityModule(Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            le = lent;
            ent = sent;
        }

        public ClientActivityModule()
        {
            le = new coreLoansEntities();
            ent = new core_dbEntities();
        }


        public void Main()
        {
            IJournalExtensions journalextensions = new JournalExtensions();
            Stopped = false;
            StopFlag = false;

            while (!StopFlag && System.Configuration.ConfigurationManager.AppSettings["interestEnabled"] == "Y")
            {
                try
                { 
                    var pro = ent.comp_prof.FirstOrDefault();
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    //check for due activities that are due
                    var activities = le.clientActivityLogs
                        .Where(p => p.activityDate >= DateTime.Today && !p.notificationSent)
                        .ToList();
                    foreach (var activity in activities)
                    {
                        var processor = new ClientActivityNotificationProcessor(activity.clientActivityLogID);
                        processor.Process();
                    }
                    le.SaveChanges();
                       
                    GC.Collect();
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "ClientActivityModule.Main");
                }

                System.Threading.Thread.Sleep(10000);
                if (StopFlag == true) break;
            }
            Stopped = true;
        }
    }
}
