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
    public class InvestmentInterestModule
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;

        public bool StopFlag;
        public bool Stopped;

        public InvestmentInterestModule(Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            le = lent;
            ent = sent;
        }

        public InvestmentInterestModule()
        {
            le = new coreLoansEntities();
            ent = new core_dbEntities();
        }


        public void Main()
        {
            IJournalExtensions journalextensions = new JournalExtensions();
            Stopped = false;
            StopFlag = false;

            while (!StopFlag && System.Configuration.ConfigurationManager.AppSettings["InvestmentInterestEnabled"] == "Y")
            {
                try
                {
                    var pro = ent.comp_prof.FirstOrDefault();
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var lns = le.investments.Where(p => p.principalBalance > 0 && (p.lastInterestDate == null
                        || (p.lastInterestDate < date && p.lastInterestDate < p.maturityDate))).ToList();
                    foreach (var ln in lns)
                    {
                        var processor = new InvestmentInterestProcessor(ln.investmentID, ent, le);
                        processor.Process();
                    }
                    le.SaveChanges();
                    ent.SaveChanges();
                     
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "InvestmentInterestModule.Main");
                }

                System.Threading.Thread.Sleep(10000);
                if (StopFlag == true) break;

            }
            Stopped = true;
        }
    }
}
