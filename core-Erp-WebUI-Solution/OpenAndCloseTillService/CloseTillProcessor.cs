using System;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace OpenAndCloseTillService
{
    public class CloseTillProcessor
    {
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        public bool Stopped { get; set; }

        public bool StopFlag { get; set; }
        

        public void Main()
        {
            int closeHour = Convert.ToInt32(ConfigurationManager.AppSettings["ClosingHour"]);


            while (!StopFlag)
            {
                try
                {

                    Thread.Sleep(60000);
                    DateTime currentTime = DateTime.Now;
                    DateTime curr = Convert.ToDateTime(currentTime.ToString("hh:mm tt"));
              

                    if (DateTime.Now.Hour == closeHour)
                    {
                        var checkConfig = le.cashierTillConfigs.FirstOrDefault(p => p.opendate == DateTime.Today && p.open == true);
                        if(checkConfig!=null)
                        {
                            Logger.logError("About to Open till");
                            ProcessingClosingTill(le);
                        }
                    }

                    if((DateTime.Now.Hour > closeHour) && (DateTime.Now < DateTime.Today.AddDays(1)))
                    {
                        var RecheckConfig = le.cashierTillConfigs.FirstOrDefault(p => p.opendate == DateTime.Today && p.open == true);
                        if (RecheckConfig != null)
                        {
                            ProcessingClosingTill(le);
                        }
                    }
                     
                }
                catch (Exception x)
                {
                    Logger.logError(x);
                }
                Stopped = true;
                StopFlag = true;
            }
        }
        private void ProcessingClosingTill(coreLoansEntities le)
        {
            IInvestmentManager ivMgr = new InvestmentManager();
            IRepaymentsManager rpmtMgr = new RepaymentsManager();

            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.jnl_batch batch = null;
            try
            {
                var date = DateTime.Today;
                var changed = false;
                var username = "";
                var Openedtills = le.cashiersTillDays.Where(p=>p.tillDay == DateTime.Today && p.open == true).ToList();
                if (Openedtills.Count !=0)
                {
                    foreach (var till in Openedtills)
                    {

         var u = le.cashiersTills.FirstOrDefault(p => p.cashiersTillID == till.cashiersTillID);
                        if(u != null)
                        {
                            username = u.userName;
                        }

                        var rcpt = le.cashierReceipts.Where(p => p.txDate == DateTime.Today && p.cashiersTill.userName.ToLower() == username.ToLower() && p.posted == true && p.closed == false && p.paymentModeID == 1);
                        foreach (var r in rcpt)
                        {
                            r.closed = true;
                            rpmtMgr.CloseCashierReceipt(le, r.loan, r, ent, r.cashiersTill.userName, ref batch);
                        }

            var das = le.savingAdditionals.Where(p => p.savingDate == DateTime.Today && p.creator.ToLower()
                            == username.ToLower() && p.posted == true && p.closed == false && p.modeOfPaymentID == 1);
                        foreach (var r in das)
                        {
                            ivMgr.CloseSavingAdditional(r, r.creator, ent, le, u, ref batch);
                            r.closed = true;
                        }

                        var dws = le.savingWithdrawals.Where(p => p.withdrawalDate == DateTime.Today && p.creator.ToLower()
                            == username.ToLower() && p.posted == true && p.closed == false && p.modeOfPaymentID == 1);
                        foreach (var r in dws)
                        {
                            ivMgr.CloseSavingsWithdrawal(r, r.creator, ent, le, u, ref batch);
                            r.closed = true;
                        }

                        till.open = false;
                        if (batch != null) ent.jnl_batch.Add(batch);
                        changed = true;
                    }
                    date = date.AddDays(1);
                    var updateConfig = le.cashierTillConfigs.FirstOrDefault(p => p.opendate == DateTime.Today && p.open == true);
                    if (updateConfig != null)
                    {
                        updateConfig.open = false;
                    }
                }
                if (changed == true)
                {
                    le.SaveChanges();
                    ent.SaveChanges();
                }
                
            }
            catch (Exception x)
            {
                Logger.logError(x);
            }

        
        }
    }
}

