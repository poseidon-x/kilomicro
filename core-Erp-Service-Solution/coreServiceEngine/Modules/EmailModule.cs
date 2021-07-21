using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;
using coreReports;
using coreService;
using coreService.Processors;

namespace coreServiceEngine
{
    public class EmailModule
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;

        public bool StopFlag;
        public bool Stopped;
        
        public EmailModule(Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            le = lent;
            ent = sent;
        }

        public EmailModule()
        {
            le = new coreLoansEntities();
            ent = new core_dbEntities();
        }

        public void Main()
        {
            Stopped = false;
            StopFlag = false;

            while (!StopFlag && System.Configuration.ConfigurationManager.AppSettings["borrowingNotificationEnabled"] == "Y")
            {
                try
                {
                    var date = DateTime.Now.Date.AddDays(-1);
                    if (date.DayOfWeek == DayOfWeek.Sunday)
                        date = date.AddDays(-1);
                    if (date.DayOfWeek == DayOfWeek.Saturday)
                        date = date.AddDays(-1);

                    //ExceptionManager.LogInformation("Before Loop");
                    var brws = le.borrowings
                            .Where(p => p.amountDisbursed > 0 && p.disbursedDate != null
                            && p.balance > 0)
                            .Where(p => !p.closed)
                            .ToList();
                        foreach (var brw in brws)
                        {
                            //ExceptionManager.LogInformation("Inside Loop"); 
                            var processor = new BorrowingsNotificationProcessor(brw.borrowingId, le, ent, date);
                            processor.Process();
                        }
                          
                    
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "EmailModule.Main");
                }

                Thread.Sleep(10000);
            }

            Stopped = true;
        }
    }

}
