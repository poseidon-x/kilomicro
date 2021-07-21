using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreData.ErrorLog;
using coreLogic;
using coreReports;
using coreService.Processors;

namespace coreService
{
    public class LoanPenaltyModule
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ent;

        public bool StopFlag;
        public bool Stopped;
        
        public LoanPenaltyModule(Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            le = lent;
            ent = sent;
        }

        public LoanPenaltyModule()
        {
            le = new coreLoansEntities();
            ent = new core_dbEntities();
        }

        public void Main()
        {
            Stopped = false;
            StopFlag = false;

            while (!StopFlag && System.Configuration.ConfigurationManager.AppSettings["penaltyEnabled"] == "Y")
            {
                try
                {
                    //ExceptionManager.LogInformation("Main Starting"); 
                    var cfg = le.loanConfigs.FirstOrDefault();
                    var todaysDate = DateTime.Today;
                    var sysDate = le.systemDates.FirstOrDefault();
                    if (System.Configuration.ConfigurationManager.AppSettings["useSystemDateForLoanPenalytInterestCalculation"] == "Y"
                        && sysDate != null && sysDate.loanSystemDate != null)
                    {
                        todaysDate = sysDate.loanSystemDate.Value.Date;
                    }
                    
                     

                    //if (sysDate != null && sysDate.loanSystemDate != null)
                    //{
                        var date = todaysDate.Date.AddDays(-1);
                        if (date.DayOfWeek == DayOfWeek.Sunday)
                            date = date.AddDays(-1);
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                            date = date.AddDays(-1);

                        if (cfg != null && cfg.automaticInterestCalculation == true)
                        {
                            //ExceptionManager.LogInformation("Before Loop");
                            var lns = le.loans
                                .Where(p => !p.closed && !p.penaltyDisabled && p.amountDisbursed > 0 && p.disbursementDate != null)
                                .Include(p => p.repaymentSchedules)
                                .Include(p => p.loanRepayments)
                                .Include(p => p.loanPenalties)
                                .ToList();
                            foreach (var ln in lns)
                            {
                                ExceptionManager.LogInformation("Inside Loop"); 
                                var rent = new reportEntities();
                                var processor = new LoanPenaltyProcessor(ln.loanID, le, ent, date, rent);
                                Logger.serviceError("Calculating for loan penalty Id :" + ln.loanID);
                                processor.Process();
                                Logger.serviceError("done Calculating for loan penalty Id :" + ln.loanID);
                            }

                        }
                    //}
                    
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "LoanPenaltyModule.Main");
                }

                Thread.Sleep(10000);
            }

            Stopped = true;
        }
    }

}
