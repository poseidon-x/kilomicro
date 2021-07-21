using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;

namespace coreService
{
    public class LoanDeductionModule
    {
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        public bool StopFlag;
        public bool Stopped;

        public void Main()
        {
            Stopped = false;
            StopFlag = false;

            while (!StopFlag)
            {
                try
                {
                    coreLoansEntities le = new coreLoansEntities();
                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var pro = ent.comp_prof.FirstOrDefault();
                    if (/*pro.disburseLoansToSavingsAccount*/false == true)
                    {
                        var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddHours(15);
                        var lns = le.repaymentSchedules.Where(p => p.loan.balance > 5 && (p.principalBalance > 0 || p.interestBalance > 0)
                            && (p.repaymentDate <= date)).ToList();
                        foreach (var ln in lns)
                        {
                            ////ln.loanReference.Load();
                            ////ln.loan.clientReference.Load();
                            //ln.loan.loanTypeReference.Load();
                            var sav = le.savings.FirstOrDefault(p => p.principalBalance + p.interestBalance
                                >= ln.interestBalance + ln.principalBalance);
                            if (sav != null)
                            {
                                var cr = new coreLogic.cashierReceipt
                                {
                                    amount = ln.principalBalance + ln.interestBalance,
                                    principalAmount = ln.principalBalance,
                                    interestAmount = ln.interestBalance,
                                    addInterestAmount = 0,
                                    feeAmount = 0,
                                    paymentModeID = 4,
                                    posted = false,
                                    loan = ln.loan,
                                    txDate = date,
                                    writeOff = false,
                                    closed = false,
                                    repaymentTypeID = 1,
                                    client=ln.loan.client
                                };

                                rpmtMgr.CashierReceipt(le, ln.loan, cr, ent, "SYSTEM");
                            }
                        }
                    }
                    le.SaveChanges();
                    ent.SaveChanges();
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "LoanDeductionModule.Main");
                }

                System.Threading.Thread.Sleep(30000);
                if (StopFlag == true) break;

            }
            Stopped = true;
        }
    }
}
