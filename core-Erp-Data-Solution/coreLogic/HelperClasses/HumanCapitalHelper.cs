using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace coreLogic
{
    public class HumanCapitalHelper : coreLogic.IHumanCapitalHelper
    {  
        public  List<staffLoanSchedule> calculateSchedule(
            double amount, double rate, DateTime loanDate,
            double tenure)
        {
            List<coreLogic.staffLoanSchedule> sched = new List<coreLogic.staffLoanSchedule>();

            DateTime date = loanDate; 
            if (tenure >= 1)
            { 
                double princ = Math.Round(amount / tenure, 5);
                double runningPrinc = princ * tenure; 
                var totalInt = tenure * Math.Round(amount * (rate / 100), 2);
                var md = (amount + totalInt) / tenure;
                var md2 = md;
                do
                {

                    coreLogic.staffLoanSchedule s = new coreLogic.staffLoanSchedule();
                    s.interestDeduction = md2-princ;
                    s.principalDeduction = princ;
                    s.deductionDate = date.AddMonths(1);
                    runningPrinc -= princ;
                    date = date.AddMonths(1); 
                    s.balanceAfter = Math.Round(runningPrinc, 4); 

                    sched.Add(s);
                }
                while (runningPrinc > 0.1);
            } 

            return sched;
        }
    }
}
