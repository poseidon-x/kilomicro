using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using coreReports;
using Microsoft.VisualBasic.ApplicationServices;

namespace coreLogic
{
    public class BorrowingScheduleManager
    {
        private IJournalExtensions journalextensions = new JournalExtensions();
        private IScheduleManager schedMgr = new ScheduleManager();

        public IEnumerable<borrowingRepaymentSchedule> GenerateBorrowingRepaymentSch(coreLoansEntities le, borrowing brw,
            double amountRequested, DateTime borrowingDate, int tenure, int interestRate)
        {
            List<coreLogic.borrowingRepaymentSchedule> sched = new List<borrowingRepaymentSchedule>();
            DateTime maturityDate = borrowingDate.AddMonths((int) brw.borrowingTenure);

            if (amountRequested > 0 && tenure > 0)
               {
                //Set variable to used for schedule
                double monthlyInterest = amountRequested * (interestRate / 100.0) * (30 / 365.0);
                double totalInterest = monthlyInterest * tenure;

                double totalPrincipal = amountRequested;
                double monthlyPrincipal = totalPrincipal / tenure;

                double balanceBF = totalPrincipal + totalInterest;
                double balanceCD = balanceBF - (monthlyPrincipal + totalInterest);

                double currentPrinBalance = totalPrincipal - monthlyPrincipal;
                double currentIntrBalance = totalInterest - monthlyInterest;

                //Generate the schedule by tenure length
                for (var i = 0; i < tenure; i++)
                {
                    var sh = new borrowingRepaymentSchedule
                    {
                        repaymentDate = maturityDate.AddMonths(i + 1),
                        principalPayment = monthlyPrincipal,
                        interestPayment = monthlyInterest,
                        principalBalance = currentPrinBalance,
                        interestBalance = currentIntrBalance,
                        balanceBF = balanceBF,
                        balanceCD = balanceCD
                    };
                    sched.Add(sh);

                    //Reset the balances on the schedule
                    currentPrinBalance -= monthlyPrincipal;
                    currentIntrBalance -= monthlyInterest;
                    balanceBF -= currentPrinBalance + currentIntrBalance;
                    balanceCD -= currentPrinBalance + currentIntrBalance;
                }
            }
                
            return sched;
        }

        



 
    }
}
