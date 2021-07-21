using coreLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace coreLogic
{
    public class LoansHelper : coreLogic.ILoansHelper
    {         
        IJournalExtensions journalextensions = new JournalExtensions();
        public List<repaymentSchedule> calculateSchedule(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, int repaymentModeID)
        {
            List<coreLogic.repaymentSchedule> sched = null;
            
            switch (repaymentModeID)
            {
                case 30:
                    sched = calculateScheduleMonthly(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID);
                    break;
                case 6:
                    sched = calculateScheduleDaily(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID, repaymentModeID);
                    break;
                case 5:
                    sched = calculateScheduleDaily(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID, repaymentModeID);
                    break;
                case 14:
                    sched = calculateScheduleFortnightly(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID);
                    break;
                case 7:
                    sched = calculateScheduleWeekly(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID);
                    break;
                case -1:
                    sched = calculateScheduleOneOff(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID);
                    break;
            }

            return sched;
        }

        private List<repaymentSchedule> calculateScheduleMonthly(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID)
        {
            List<coreLogic.repaymentSchedule> sched = new List<coreLogic.repaymentSchedule>();
           
            
                    var le=new coreLoansEntities();
                    var cfg = le.loanConfigs.FirstOrDefault();
                     DateTime date = loanDate;
            if (gracePeriod!=null) date = date.AddDays(gracePeriod.Value);
            if (tenure >= 1)
            {
                double interest = 0;
                double princ = Math.Floor(amount / tenure);
                double runningPrinc = princ * tenure;
                double princR = (amount / tenure - Math.Floor(amount / tenure));
                double princRem = 0;
                double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate / 100, tenure, runningPrinc, 0);
                double pmt2 = -Microsoft.VisualBasic.Financial.Pmt(rate / 100, tenure, amount, 0);
                bool second = false;
                repaymentSchedule sch = null;
                int j = 0;
                do
                {
                    if (interestTypeID == 4)
                    {
                        if ((new core_dbEntities()).comp_prof.First().comp_name.Contains(AppContants.LinkExchange))
                            interest = Math.Round(amount * (rate / 100), 0);
                        else
                            interest = Math.Round(amount * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    else if (interestTypeID == 3)
                    {
                        if ((new core_dbEntities()).comp_prof.First().comp_name.Contains(AppContants.LinkExchange))
                            interest = Math.Round(runningPrinc * (rate / 100), 0);
                        else
                            interest = Math.Round(runningPrinc * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    if (interestTypeID == 2 || interestTypeID == 6 || interestTypeID == 7)
                    {
                        if ((new core_dbEntities()).comp_prof.First().comp_name.Contains(AppContants.LinkExchange))
                            interest = Math.Round(runningPrinc * (rate / 100), 0);
                        else
                            interest = Math.Round(runningPrinc * (rate / 100), 2);
                        princ = pmt2 - interest;
                        runningPrinc -= princ;
                    }
                    else if (interestTypeID == 1)
                    {
                        if ((new core_dbEntities()).comp_prof.First().comp_name.Contains(AppContants.LinkExchange))
                            interest = Math.Round(amount * (rate / 100), 0);
                        else
                            interest = Math.Round(amount * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    princRem += princR;

                    coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                    s.interestPayment = Math.Round(interest, 4);
                    s.principalPayment = (interestTypeID == 4) ? 0 : Math.Round(princ, 4);
                    
                    if ((cfg != null && cfg.automaticInterestCalculation == true && cfg.penaltyIsAdditionalInterest == true)  
                        && (j == 0))
                    {
                        date = date.AddMonths(-1);
                    }
                    s.repaymentDate = date.AddMonths(1);
                    date = date.AddMonths(1);

                    if (second == false)
                    {
                        if (gracePeriod != null) date = date.AddDays(-gracePeriod.Value);
                        second = true;
                    }
                    s.balanceCD = (interestTypeID == 4) ? 0 : Math.Round(princ, 4);
                    s.interestBalance = Math.Round(interest, 4);
                    s.principalBalance = (interestTypeID == 4) ? 0 : Math.Round(princ, 4);


                    s.origPrincipalBF = Math.Round(runningPrinc, 4) + Math.Round(princ, 4);
                    s.origPrincipalCD = Math.Round(runningPrinc, 4);
                    s.origInterestPayment = Math.Round(interest, 4);
                    s.additionalInterest = 0;
                    s.additionalInterestBalance = 0;
                    s.penaltyAmount = 0;

                    sched.Add(s);
                    sch = s;
                    j++;
                }
                while (j < tenure);
                if (interestTypeID == 2 || interestTypeID == 6 || interestTypeID == 7)
                {
                    var totalPrinc = sched.Sum(p => p.principalPayment);
                    var diff = Math.Round(totalPrinc - amount, 4);
                    sched[sched.Count - 1].principalPayment -= diff;
                    sched[sched.Count - 1].principalBalance -= diff;
                    sched[sched.Count - 1].interestBalance += diff;
                    sched[sched.Count - 1].interestPayment += diff;
                }
                if (sch != null && interestTypeID==4)
                {
                    sch.principalPayment += amount;
                    sch.principalBalance += amount;
                }
                for (int i = 0; i < sched.Count && interestTypeID!=4; i++)
                {
                    if (princRem >= 0.5 && (new core_dbEntities()).comp_prof.First().comp_name.Contains(AppContants.LinkExchange))
                    {
                        sched[i].principalBalance += 1;
                        sched[i].principalPayment += 1;
                        princRem -= 1;
                    }
                    else
                        break;
                }
            }
            else
            {
                double interest = 0;
                double princ = amount;
                interest = Math.Round(amount * (rate / 100) * tenure, 2);

                coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                s.interestPayment = Math.Round(interest, 4);
                s.principalPayment = Math.Round(princ, 4);
                s.repaymentDate = date.AddDays((int)tenure * 30);
                s.balanceCD = 0;
                s.interestBalance = Math.Round(interest, 4);
                s.principalBalance = Math.Round(princ, 4);

                s.origPrincipalBF = Math.Round(princ, 4);
                s.origPrincipalCD = 0;
                s.origInterestPayment = Math.Round(interest, 4);
                s.additionalInterest = 0;
                s.penaltyAmount = 0;


                sched.Add(s);
            }
            return sched;
        }

        public  List<repaymentSchedule> calculateScheduleM(
            double amount, double rate, DateTime loanDate,
            double tenure)
        {
            List<coreLogic.repaymentSchedule> sched = new List<coreLogic.repaymentSchedule>();

            DateTime date = loanDate; 
            if (tenure >= 1)
            { 
                double princ = Math.Round(amount / tenure, 5);
                double runningPrinc = princ * tenure; 
                var totalInt = tenure * Math.Round(amount * (rate / 100), 2);
                var md = (amount + totalInt) / tenure;
                var md2 = Math.Ceiling(md);
                do
                { 

                    coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                    s.interestPayment = md2-princ;
                    s.principalPayment = princ;
                    s.repaymentDate = date.AddMonths(1);
                    runningPrinc -= princ;
                    date = date.AddMonths(1); 
                    s.balanceCD = Math.Round(runningPrinc, 4);
                    s.interestBalance = s.interestPayment;
                    s.principalBalance = s.principalPayment;

                    s.origPrincipalBF = Math.Round(runningPrinc, 4) + Math.Round(princ, 4);
                    s.origPrincipalCD = Math.Round(runningPrinc, 4);
                    s.origInterestPayment = s.interestPayment;
                    s.additionalInterest = 0;
                    s.penaltyAmount = 0;


                    sched.Add(s);
                }
                while (runningPrinc > 0.1);
            } 

            return sched;
        }
        
        private  List<repaymentSchedule> calculateScheduleDaily(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, int repaymentModeID)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            do
            {
                date = date.AddDays(1);
                if ((date.DayOfWeek != DayOfWeek.Sunday)
                    && ((repaymentModeID == 5 && date.DayOfWeek != DayOfWeek.Saturday) ||
                    (repaymentModeID == 6)))
                {
                    dates.Add(date);
                }
            } while (date <= loanDate.AddMonths((int)tenure));
             
            double interest = 0;
            double princ = Math.Floor(amount / dates.Count);
            double runningPrinc = princ * dates.Count;
            double runningPrinc2 = princ * dates.Count;
            double princR = (amount / dates.Count - Math.Floor(amount / dates.Count));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate / 30 / 100, dates.Count, runningPrinc, 0);
            interest = Math.Floor(amount * (rate / 100) * tenure / dates.Count);
            double intR = (amount * (rate / 100) * tenure / dates.Count) -
                Math.Floor(amount * (rate / 100) * tenure / dates.Count);
            double intRem = 0;
            DateTime date2=loanDate;

            List<coreLogic.repaymentSchedule> sched = new List<coreLogic.repaymentSchedule>();
            date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            foreach (var d in dates)
            {
                var diffDays = (d - date).Days;
                if ((d - date2).Days >= 30)
                {
                    runningPrinc2 = runningPrinc;
                    date2 = d;
                }
                if (interestTypeID == 1) ;
                else
                {
                    interest = Math.Round(runningPrinc2 * (rate / 100)
                        * (diffDays / 30.0), 2);
                    intR = interest - Math.Floor(interest);
                    interest = Math.Floor(interest);
                } 
                princRem += princR;
                intRem += intR;
                coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                s.interestPayment = Math.Round(interest, 4);
                s.principalPayment = Math.Round(princ, 4);
                s.repaymentDate = d;
                runningPrinc -= princ;
                s.interestBalance = Math.Round(interest, 4);
                s.principalBalance = Math.Round(princ, 4);

                s.balanceCD = Math.Round(runningPrinc, 4);

                s.origPrincipalBF = Math.Round(runningPrinc, 4) + Math.Round(princ, 4);
                s.origPrincipalCD = Math.Round(runningPrinc, 4);
                s.origInterestPayment = Math.Round(interest, 4);
                s.additionalInterest = 0;
                s.penaltyAmount = 0;
                s.additionalInterestBalance = 0;

                sched.Add(s);

                date = d;
            }

            for (int i = 0; i < sched.Count; i++)
            {
                if (princRem >= 0.5)
                {
                    sched[i].principalBalance += 1;
                    sched[i].principalPayment += 1;
                    princRem -= 1;
                }
                else
                    break;
            }

            for (int i = 0; i < sched.Count; i++)
            {
                if (intRem >= 0.5)
                {
                    sched[i].interestBalance += 1;
                    sched[i].interestPayment += 1;
                    intRem -= 1;
                }
                else
                {
                    sched[i].interestBalance += intRem;
                    sched[i].interestPayment += intRem;
                    break;
                }
            }
            return sched;
        }

        private  List<repaymentSchedule> calculateScheduleFortnightly(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            do
            {
                date = date.AddDays(14);
                if (date > loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure))
                {
                    date = loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure);
                }
                dates.Add(date);
            } while (date < loanDate.AddMonths((int)tenure));
             
            double interest = 0;
            double princ = Math.Floor(amount / dates.Count);
            double runningPrinc = princ * dates.Count;
            double runningPrinc2 = princ * dates.Count;
            double princR = (amount / dates.Count - Math.Floor(amount / dates.Count));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate * 14.0 / 30 / 100, dates.Count, runningPrinc, 0);
            interest = Math.Floor(amount * (rate / 100) * tenure / dates.Count);
            double intR = (amount * (rate / 100) * tenure / dates.Count) -
                Math.Floor(amount * (rate / 100) * tenure / dates.Count);
            double intRem = 0;

            List<coreLogic.repaymentSchedule> sched = new List<coreLogic.repaymentSchedule>();
            date = loanDate;
            DateTime date2 = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            foreach (var d in dates)
            {
                var diffDays = (d - date).Days;

                if ((d - date2).Days >= 30)
                {
                    runningPrinc2 = runningPrinc;
                    date2 = d;
                }
                if (interestTypeID == 1) ;
                else
                {
                    interest = Math.Round(runningPrinc2 * (rate / 100)
                        * (diffDays / 30.0), 2);
                    intR = interest - Math.Floor(interest);
                    interest = Math.Floor(interest);
                }
                princRem += princR;
                intRem += intR;
                 
                coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                s.interestPayment = Math.Round(interest, 4);
                s.principalPayment = Math.Round(princ, 4);
                s.repaymentDate = d;
                runningPrinc -= princ;
                s.balanceCD = Math.Round(runningPrinc, 4);
                s.interestBalance = Math.Round(interest, 4);
                s.principalBalance = Math.Round(princ, 4);

                s.origPrincipalBF = Math.Round(runningPrinc, 4) + Math.Round(princ, 4);
                s.origPrincipalCD = Math.Round(runningPrinc, 4);
                s.origInterestPayment = Math.Round(interest, 4);
                s.additionalInterest = 0;
                s.penaltyAmount = 0;
                s.additionalInterestBalance = 0;

                sched.Add(s);

                date = d;
            }

            for (int i = 0; i < sched.Count; i++)
            {
                if (princRem >= 0.5)
                {
                    sched[i].principalBalance += 1;
                    sched[i].principalPayment += 1;
                    princRem -= 1;
                }
                else
                    break;
            }

            for (int i = 0; i < sched.Count; i++)
            {
                if (intRem >= 0.5)
                {
                    sched[i].interestBalance += 1;
                    sched[i].interestPayment += 1;
                    intRem -= 1;
                }
                else
                {
                    sched[i].interestBalance += intRem;
                    sched[i].interestPayment += intRem;
                    break;
                }
            }
            return sched;
        }

        private  List<repaymentSchedule> calculateScheduleWeekly(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            core_dbEntities ent = new core_dbEntities();
            var prof = ent.comp_prof.FirstOrDefault();
            if (prof.traditionalLoanNo == false)
            {
                do
                {
                    date = date.AddDays(7);
                    if (date > loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure))
                    {
                        date = loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure);
                    }
                    dates.Add(date);
                } while (date < loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure));
            }
            else
            {
                for (int i = 0; i < tenure * 4; i++)
                {
                    date = date.AddDays(7);
                    if (date > loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure))
                    {
                        date = loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure);
                    }
                    dates.Add(date);
                } 
            }
            double interest = 0;
            double princ = Math.Floor(amount / dates.Count);
            double runningPrinc = princ * dates.Count;
            double runningPrinc2 = princ * dates.Count;
            double princR = (amount / dates.Count - Math.Floor(amount / dates.Count));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate * 7.0 / 30 / 100, dates.Count, runningPrinc, 0);
            interest = Math.Floor(amount * (rate / 100) * tenure / dates.Count);
            double intR = (amount * (rate / 100) * tenure / dates.Count) -
                Math.Floor(amount * (rate / 100) * tenure / dates.Count);
            double intRem = 0;

            List<coreLogic.repaymentSchedule> sched = new List<coreLogic.repaymentSchedule>();
            date = loanDate;
            DateTime date2 = loanDate;

            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            foreach (var d in dates)
            {
                var diffDays = (d - date).Days;
                if ((d - date2).Days >= 30)
                {
                    runningPrinc2 = runningPrinc;
                    date2 = d;
                }
                if (interestTypeID == 1) ;
                else
                {
                    interest = Math.Round(runningPrinc2 * (rate / 100)
                        * (diffDays / 30.0), 2);
                    intR = interest - Math.Floor(interest);
                    interest = Math.Floor(interest);
                }
                princRem += princR;
                intRem += intR;

                coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                s.interestPayment = Math.Round(interest, 4);
                s.principalPayment = Math.Round(princ, 4);
                s.repaymentDate = d;
                runningPrinc -= princ;
                s.balanceCD = Math.Round(runningPrinc, 4);
                s.interestBalance = Math.Round(interest, 4);
                s.principalBalance = Math.Round(princ, 4);

                s.origPrincipalBF = Math.Round(runningPrinc, 4) + Math.Round(princ, 4);
                s.origPrincipalCD = Math.Round(runningPrinc, 4);
                s.origInterestPayment = Math.Round(interest, 4);
                s.additionalInterest = 0;
                s.penaltyAmount = 0;
                s.additionalInterestBalance = 0;

                sched.Add(s);

                date = d;
            }

            for (int i = 0; i < sched.Count; i++)
            {
                if (princRem >= 0.5)
                {
                    sched[i].principalBalance += 1;
                    sched[i].principalPayment += 1;
                    princRem -= 1;
                }
                else
                    break;
            }

            for (int i = 0; i < sched.Count; i++)
            {
                if (intRem >= 0.5)
                {
                    sched[i].interestBalance += 1;
                    sched[i].interestPayment += 1;
                    intRem -= 1;
                }
                else
                {
                    sched[i].interestBalance += intRem;
                    sched[i].interestPayment += intRem;
                    break;
                }
            }
            return sched;
        }

        private  List<repaymentSchedule> calculateScheduleOneOff(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            do
            {
                date = date.AddMonths((tenure < 1) ? 0 : (int)tenure);
                date = date.AddDays((tenure < 1) ? (int)tenure * 28 : 0);
                dates.Add(date);
            } while (date < loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure));

            double runningPrinc = amount;
            double interest = 0;
            double princ = Math.Floor(amount / dates.Count);
            double princR = (amount / tenure - Math.Floor(amount / tenure));
            double princRem = 0;
            var tenure2 = tenure == 0 ? 1 : tenure;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate / 100, tenure2, runningPrinc, 0);

            List<coreLogic.repaymentSchedule> sched = new List<coreLogic.repaymentSchedule>();
            date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);

            date = date.AddMonths((tenure < 1) ? 0 : (int)tenure);
            date = date.AddDays((tenure < 1) ? (int)tenure * 30 : 0);

            if (tenure == 0)
            {
                interest = 0;
                princ = runningPrinc;
            }
            else
            {
                interest = Math.Round(runningPrinc * (rate / 100)
                    * tenure, 0);
                if (interestTypeID == 2)
                {
                    princ = pmt - interest;
                }
            }

            coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
            s.interestPayment = Math.Round(interest, 4);
            s.principalPayment = Math.Round(princ, 4);
            s.repaymentDate = date;
            runningPrinc -= princ;
            s.balanceCD = Math.Round(runningPrinc, 4);
            s.interestBalance = Math.Round(interest, 4);
            s.principalBalance = Math.Round(princ, 4);

            s.origPrincipalBF = Math.Round(runningPrinc, 4) + Math.Round(princ, 4);
            s.origPrincipalCD = Math.Round(runningPrinc, 4);
            s.origInterestPayment = Math.Round(interest, 4);
            s.additionalInterest = 0;
            s.penaltyAmount = 0;
            s.additionalInterestBalance = 0;

            sched.Add(s);                     

            return sched;
        }

        public  List<repaymentSchedule> calculateScheduleSusu(susuAccount account, string userName)
        {  
            List<coreLogic.repaymentSchedule> sched = new List<coreLogic.repaymentSchedule>();
            var tint = account.interestAmount;
            if (tint < 0) tint = 0;
            var bal = account.amountEntitled - account.commissionAmount - account.interestAmount; 
            foreach (var sc in account.susuContributionSchedules.OrderBy(p=> p.plannedContributionDate))
            {
                var interest = sc.amount;
                if (tint>0 && tint < interest)
                {
                    interest = tint;
                }
                else if (tint == 0)
                {
                    interest = 0;
                }

                tint = tint - interest;
                var interestBal = interest;
                var princ=sc.amount-interest;
                var princBal= princ;
                repaymentSchedule s = new repaymentSchedule
                {
                    creation_date = DateTime.Now,
                    creator = userName,
                    edited = false,
                    interestBalance = interestBal,
                    interestPayment = interest,
                    principalBalance = princBal,
                    principalPayment = princ,
                    repaymentDate = sc.plannedContributionDate,
                    balanceBF = bal,
                    balanceCD = bal - princ,

                    origPrincipalBF = bal,
                    origPrincipalCD = bal-princ,
                    origInterestPayment = Math.Round(interest, 4),
                    additionalInterest = 0,
                    penaltyAmount = 0,

                };
                bal = bal - princ;
                sched.Add(s);
                if (bal <= 0) break;
            }
            return sched;
        }
        
        public  List<repaymentSchedule> calculateSchedule(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, int repaymentModeID, List<repaymentSchedule> oldSched)
        {
            List<coreLogic.repaymentSchedule> sched = null;

            switch (repaymentModeID)
            {
                case 30:
                    sched = calculateScheduleMonthly(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID, oldSched);
                    break;
                case 6:
                    sched = calculateScheduleDaily(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID, repaymentModeID, oldSched);
                    break;
                case 5:
                    sched = calculateScheduleDaily(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID, repaymentModeID, oldSched);
                    break;
                case 14:
                    sched = calculateScheduleFortnightly(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID, oldSched);
                    break;
                case 7:
                    sched = calculateScheduleWeekly(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID, oldSched);
                    break;
                case -1:
                    sched = calculateScheduleOneOff(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID, oldSched);
                    break;
            }

            return sched;
        }

        private  List<repaymentSchedule> calculateScheduleMonthly(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, List<repaymentSchedule> oldSched)
        {
            List<coreLogic.repaymentSchedule> sched = oldSched; 
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            double interest = 0;
            double princ = Math.Floor(amount / (sched.Where(p => p.repaymentDate > loanDate).Count()));
            double runningPrinc = princ * (sched.Where(p => p.repaymentDate >= loanDate).Count()); 
            double princR = (amount /  (sched.Where(p => p.repaymentDate >= loanDate).Count())
                - Math.Floor(amount /  (sched.Where(p => p.repaymentDate >= loanDate).Count())));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate / 100, tenure, runningPrinc, 0);
            bool second = false;
            var totalInterest = Math.Round(runningPrinc * tenure * (rate / 100), 0);
            var runningInterest = 0.0;

            repaymentSchedule sch = null;
            foreach (var r in sched)
            {
                var diff  = (r.repaymentDate-loanDate).Days;
                if (diff < 0)
                {
                }
                else if (diff >= 0)
                {
                    if (interestTypeID == 4)
                    {
                        interest = Math.Round(amount * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    else if (interestTypeID == 3)
                    {
                        interest = Math.Round(runningPrinc * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    if (interestTypeID == 2 || interestTypeID == 6 || interestTypeID == 7)
                    {
                        interest = Math.Round(runningPrinc * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    else if (interestTypeID == 1)
                    {
                        interest = Math.Round(amount * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    date = r.repaymentDate.AddMonths(1);
                    if (second == false)
                    {
                        if (gracePeriod != null) date = date.AddDays(-gracePeriod.Value);
                        second = true;
                    }
                }
                else
                {
                    if (interestTypeID == 2 || interestTypeID == 6 || interestTypeID == 7)
                    {
                        interest = Math.Round(runningPrinc * (rate / 100), 4);
                        princ = pmt - interest;
                    }
                    else if (interestTypeID == 1)
                        interest = Math.Round(runningPrinc * (rate / 100), 4);
                    date = r.repaymentDate.AddMonths(1);
                    if (second == false)
                    {
                        if (gracePeriod != null) date = date.AddDays(-gracePeriod.Value);
                        second = true;
                    }
                }

                if (diff >= 0)
                {
                    r.interestPayment += Math.Round(interest, 4);
                    r.principalPayment += (interestTypeID == 4) ? 0 : Math.Round(princ, 4);
                    runningPrinc -= princ;
                    r.balanceCD = Math.Round(runningPrinc, 4); 
                    r.interestBalance = Math.Round(interest, 4);
                    r.principalBalance = Math.Round(princ, 4);
                    runningInterest += Math.Round(interest, 4);
                    r.origInterestPayment = Math.Round(interest, 4);
                    r.origPrincipalBF = Math.Round(runningPrinc, 4) + ((interestTypeID == 4) ? 0 : Math.Round(princ, 4));
                    r.origPrincipalCD = Math.Round(runningPrinc, 4);
                    r.additionalInterest = 0;
                    r.penaltyAmount = 0;
                    sch = r;
                }
            }
            if (sch != null && interestTypeID == 4)
            {
                sch.principalPayment += amount;
                sch.principalBalance += amount;
            }
            foreach (var r in sched)
            {
                if (r.repaymentDate >= loanDate)
                {
                    var balanceCD = sched.Where(p => p.repaymentDate > r.repaymentDate).Sum(p => p.principalPayment);
                    r.balanceCD = balanceCD;
                    princRem += princR;
                }
            }
            for (int i = 0; i < sched.Count && interestTypeID!=4; i++)
            {
                if (sched[i].repaymentDate >= loanDate)
                {
                    if (princRem >= 0.5)
                    {
                        sched[i].principalBalance += 1;
                        sched[i].principalPayment += 1;
                        princRem -= 1;
                    }
                    else
                        break;
                }
            }
            return sched;
        }

        private  List<repaymentSchedule> calculateScheduleDaily(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, int repaymentModeID, List<repaymentSchedule> oldSched)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            do
            {
                date = date.AddDays(1);
                if ((date.DayOfWeek != DayOfWeek.Sunday)
                    && ((repaymentModeID == 5 && date.DayOfWeek != DayOfWeek.Saturday) ||
                    (repaymentModeID == 6)))
                {
                    dates.Add(date);
                }
            } while (date <= loanDate.AddMonths((int)tenure));

            List<coreLogic.repaymentSchedule> sched = oldSched;
            double runningPrinc = amount;
            double interest = 0;
            double princ = Math.Floor(amount / (sched.Where(p => p.repaymentDate >= loanDate).Count()));
            double princR = (amount / (sched.Where(p => p.repaymentDate >= loanDate).Count())
                - Math.Floor(amount / (sched.Where(p => p.repaymentDate >= loanDate).Count())));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate / 30 / 100, dates.Count, runningPrinc, 0);

            date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);

            foreach (var r in sched)
            {
                var diff = (r.repaymentDate - date).Days;
                if (diff <= 0)
                {
                }
                else if (diff == 1)
                {
                    if (interestTypeID == 2 || interestTypeID == 6 || interestTypeID == 7)
                    {
                        interest = Math.Round(runningPrinc * 1.0 / 30.0 * (rate / 100), 0);
                        //princ = pmt - interest;
                    }
                    else if (interestTypeID == 1)
                        interest = Math.Round(amount * 1.0 / 30.0 * (rate / 100), 0);
                    date = r.repaymentDate.AddDays(1);

                    r.interestPayment += Math.Round(interest, 4);
                    r.principalPayment += Math.Round(princ, 4);
                    runningPrinc -= princ;
                    r.interestBalance += Math.Round(interest, 4);
                    r.principalBalance += Math.Round(princ, 4);
                    r.origInterestPayment = Math.Round(interest, 4);
                    r.origPrincipalBF = Math.Round(runningPrinc, 4) + ((interestTypeID == 4) ? 0 : Math.Round(princ, 4));
                    r.origPrincipalCD = Math.Round(runningPrinc, 4);
                    r.additionalInterest = 0;
                    r.penaltyAmount = 0;
                     
                }
            }
            foreach (var r in sched)
            {
                if (r.repaymentDate >= loanDate)
                {
                    var balanceCD = sched.Where(p => p.repaymentDate > r.repaymentDate).Sum(p => p.principalPayment);
                    r.balanceCD = balanceCD;
                    princRem += princR;
                }
            }
            for (int i = 0; i < sched.Count; i++)
            {
                if (sched[i].repaymentDate >= loanDate)
                {
                    if (princRem >= 0.5)
                    {
                        sched[i].principalBalance += 1;
                        sched[i].principalPayment += 1;
                        princRem -= 1;
                    }
                    else
                        break;
                }
            }
            return sched;
        }

        private  List<repaymentSchedule> calculateScheduleFortnightly(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, List<repaymentSchedule> oldSched)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            do
            {
                date = date.AddDays(14);
                if (date > loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure))
                {
                    date = loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure);
                }
                dates.Add(date);
            } while (date < loanDate.AddMonths((int)tenure));

            List<coreLogic.repaymentSchedule> sched = oldSched; 
            double interest = 0;
            double princ = Math.Floor(amount / (sched.Where(p => p.repaymentDate >= loanDate).Count()));
            double runningPrinc = princ * (sched.Where(p => p.repaymentDate >= loanDate).Count()); 
            double princR = (amount / (sched.Where(p => p.repaymentDate >= loanDate).Count())
                - Math.Floor(amount / (sched.Where(p => p.repaymentDate >= loanDate).Count())));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate * 14.0 / 30 / 100, dates.Count, runningPrinc, 0);

            date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);

            foreach (var r in sched)
            {
                var diff = (r.repaymentDate - loanDate).Days;
                if (diff < 0)
                {
                }
                else
                {
                    if (interestTypeID == 2 || interestTypeID == 6 || interestTypeID == 7)
                    {
                        interest = Math.Round(runningPrinc * diff / 30.0 * (rate / 100), 0);
                       // princ = pmt - interest;
                    }
                    else if (interestTypeID == 1)
                        interest = Math.Round(amount * 14 / 30.0 * (rate / 100), 0);
                    date = r.repaymentDate.AddDays(14);

                    r.interestPayment += Math.Round(interest, 4);
                    r.principalPayment += Math.Round(princ, 4);
                    runningPrinc -= princ;
                    r.interestBalance += Math.Round(interest, 4);
                    r.principalBalance += Math.Round(princ, 4);
                    r.origInterestPayment += Math.Round(interest, 4);
                    r.origPrincipalBF = Math.Round(runningPrinc, 4) + ((interestTypeID == 4) ? 0 : Math.Round(princ, 4));
                    r.origPrincipalCD = Math.Round(runningPrinc, 4);
                    r.additionalInterest = 0;
                    r.penaltyAmount = 0;                     
                }
            }

            foreach (var r in sched)
            {
                if (r.repaymentDate >= loanDate)
                {
                    var balanceCD = sched.Where(p => p.repaymentDate > r.repaymentDate).Sum(p => p.principalPayment);
                    r.balanceCD = balanceCD;
                    princRem += princR;
                }
            }
            for (int i = 0; i < sched.Count; i++)
            {
                if (sched[i].repaymentDate >= loanDate)
                {
                    if (princRem >= 0.5)
                    {
                        sched[i].principalBalance += 1;
                        sched[i].principalPayment += 1;
                        princRem -= 1;
                    }
                    else
                        break;
                }
            }
            return sched;
        }

        private  List<repaymentSchedule> calculateScheduleWeekly(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, List<repaymentSchedule> oldSched)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            core_dbEntities ent = new core_dbEntities();
            var prof = ent.comp_prof.FirstOrDefault();
            if (prof.traditionalLoanNo == false)
            {
                do
                {
                    date = date.AddDays(7);
                    if (date > loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure))
                    {
                        date = loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure);
                    }
                    dates.Add(date);
                } while (date < loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure));
            }
            else
            {
                for (int i = 0; i < tenure * 4; i++)
                {
                    date = date.AddDays(7);
                    if (date > loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure))
                    {
                        date = loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure);
                    }
                    dates.Add(date);
                }
            }

            List<coreLogic.repaymentSchedule> sched = oldSched; 
            double interest = 0;
            double princ = Math.Floor(amount / (sched.Where(p => p.repaymentDate >= loanDate).Count()));
            double runningPrinc = princ * (sched.Where(p => p.repaymentDate >= loanDate).Count()); 
            double princR = (amount / (sched.Where(p => p.repaymentDate >= loanDate).Count())
                - Math.Floor(amount / (sched.Where(p => p.repaymentDate >= loanDate).Count())));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate * 7.0 / 30 / 100, dates.Count, runningPrinc, 0);

            date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            foreach (var r in sched)
            {
                var diff = (r.repaymentDate - loanDate).Days;
                if (diff < 0)
                {
                }
                else
                {
                    if (interestTypeID == 2 || interestTypeID == 6 || interestTypeID == 7)
                    {
                        interest = Math.Round(runningPrinc * diff / 30.0 * (rate / 100), 0);
                        //princ = pmt - interest;
                    }
                    else if (interestTypeID == 1)
                        interest = Math.Round(amount * 7 / 30.0 * (rate / 100), 0);
                    r.interestPayment += Math.Round(interest, 4);
                    r.principalPayment += Math.Round(princ, 4);
                    runningPrinc -= princ;
                    r.interestBalance += Math.Round(interest, 4);
                    r.principalBalance += Math.Round(princ, 4);
                    r.origInterestPayment += Math.Round(interest, 4);
                    r.origPrincipalBF = Math.Round(runningPrinc, 4) + ((interestTypeID == 4) ? 0 : Math.Round(princ, 4));
                    r.origPrincipalCD = Math.Round(runningPrinc, 4);
                    r.additionalInterest = 0;
                    r.penaltyAmount = 0;                
                     
                }
                date = r.repaymentDate.AddDays(7);
            }

            foreach (var r in sched)
            {
                if (r.repaymentDate >= loanDate)
                {
                    var balanceCD = sched.Where(p => p.repaymentDate > r.repaymentDate).Sum(p => p.principalPayment);
                    r.balanceCD = balanceCD;
                    princRem += princR;
                }
            }
            for (int i = 0; i < sched.Count; i++)
            {
                if (sched[i].repaymentDate >= loanDate)
                {
                    if (princRem >= 0.5)
                    {
                        sched[i].principalBalance += 1;
                        sched[i].principalPayment += 1;
                        princRem -= 1;
                    }
                    else
                        break;
                }
            }
            return sched;
        }

        private  List<repaymentSchedule> calculateScheduleOneOff(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, List<repaymentSchedule> oldSched)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            do
            {
                date = date.AddMonths((tenure < 1) ? 0 : (int)tenure);
                date = date.AddDays((tenure < 1) ? (int)tenure*28: 0);
                dates.Add(date);
            } while (date < loanDate.AddDays((gracePeriod == null) ? 0 : gracePeriod.Value).AddMonths((int)tenure));

            double runningPrinc = amount;
            double interest = 0;
            double princ = Math.Round(amount / dates.Count, 4);
            double princR = (amount / dates.Count
                - Math.Floor(amount / dates.Count));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate / 100, tenure, runningPrinc, 0);

            List<coreLogic.repaymentSchedule> sched = oldSched;
            date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);

            date = date.AddMonths((tenure < 1) ? 0 : (int)tenure);
            date = date.AddDays((tenure < 1) ? (int)tenure * 30 : 0);

            if (tenure == 0)
            {
                interest = 0;
                princ = runningPrinc;
            }
            else
            {
                interest = Math.Round(runningPrinc * (rate / 100)
                    * tenure, 4);
                if (interestTypeID == 2)
                {
                    princ = pmt - interest;
                }
            }
            princRem += princR;

            coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
            s.interestPayment = Math.Round(interest, 4);
            s.principalPayment = Math.Round(princ, 4);
            s.repaymentDate = date;
            runningPrinc -= princ;
            s.balanceCD = Math.Round(runningPrinc, 4);
            s.interestBalance = Math.Round(interest, 4);
            s.principalBalance = Math.Round(princ, 4);
            s.origInterestPayment = Math.Round(interest, 4);
            s.origPrincipalBF = Math.Round(runningPrinc, 4) + ((interestTypeID == 4) ? 0 : Math.Round(princ, 4));
            s.origPrincipalCD = Math.Round(runningPrinc, 4);
            s.additionalInterest = 0;
            s.penaltyAmount = 0;                
            sched.Add(s);

            return sched;
        }

        public string ReceivePayment(coreLoansEntities le, loan ln,
            double amountPaid, DateTime paymentDate, string paymentTypeID,
            string bID, string bankName, string checkNo,
                coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID)
        {
            jnl_batch batch = null;
            return ReceivePayment(le, ln,
             amountPaid, paymentDate, paymentTypeID,
             bID, bankName, checkNo,
                 ent, userName, modeOfPaymentID, 0, null, ref batch);
        }
        public  string ReceivePayment(coreLoansEntities le, loan ln,
            double amountPaid, DateTime paymentDate, string paymentTypeID,
            string bID, string bankName, string checkNo,
                coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID, double amt2,
            int? accountID, ref jnl_batch batch)
        {
            var amount = amountPaid;
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();
            int? bankID = null;
            if (bID != "" && bID != null) bankID = int.Parse(bID);
            loanRepayment repayment = null;
            string batchNo = null;

            var pro = ent.comp_prof.FirstOrDefault();
            saving sav = le.savings.FirstOrDefault(p => p.clientID == ln.clientID 
                && (p.principalBalance + p.interestBalance >= amountPaid));

            var acctID = ln.loanType.vaultAccountID;
            var acctID2 = ln.loanType.vaultAccountID;
            if (modeOfPaymentID == 4 && sav != null && paymentTypeID == "1")
            { 
                acctID = sav.savingType.accountsPayableAccountID.Value;
            }
            else if (accountID != null)
            {
                acctID = accountID.Value;
            }
            else if ((modeOfPaymentID == 2 || modeOfPaymentID == 3) && bankID == null)
            {
                var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1046");
                if (gl != null)
                {
                    acctID = gl.acct_id;
                }
                gl = ent.accts.FirstOrDefault(p => p.acc_num == "1047");
                if (gl != null)
                {
                    acctID2 = gl.acct_id;
                }
            }
            else if ((modeOfPaymentID == 2 || modeOfPaymentID == 3) && bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                if (ba != null)
                {
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }

            switch (paymentTypeID)
            {
                case "1":
                    var iTotal = Math.Ceiling((ln.amountDisbursed) * (ln.interestRate / 100.0)
                                        * (paymentDate - ln.disbursementDate.Value).Days / 30.0);
                    var iPaid = ln.repaymentSchedules.Sum(p => p.interestPayment - p.interestBalance);
                    var pBal = ln.repaymentSchedules.Sum(p => p.principalBalance);
                    var iBal = ln.repaymentSchedules.Sum(p => p.interestBalance);
                    var rem = pBal + (iTotal - iPaid);
                    var pBal2 = ln.repaymentSchedules.Where(p => p.repaymentDate <= paymentDate).Sum(p => p.principalBalance);
                    var iBal2 = ln.repaymentSchedules.Where(p=> p.repaymentDate<= paymentDate).Sum(p => p.interestBalance);

                    var sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0 || p.principalBalance > 0).OrderBy(p => p.repaymentDate).ToList();
                    var totalInt = ln.repaymentSchedules.Sum(p => p.interestPayment);
                    var totalPrinc = ln.repaymentSchedules.Sum(p => p.principalPayment);
                    var intRatio = totalInt / (totalInt + totalPrinc);
                        var tinterest = 0.0;
                    var tprinc=0.0;
                    var tprinc2 = 0.0;
                    var tinterest2 = 0.0;
                    if (iBal2 > 0)
                    {
                        tinterest = Math.Round(intRatio * amountPaid, 2);
                        tprinc = amountPaid - tinterest;
                        if (tinterest > iBal2)
                        {
                            tinterest = iBal2;
                            tprinc = amountPaid - tinterest;
                        }
                        else if (tprinc > pBal)
                        {
                            tprinc = pBal;
                            tinterest = amountPaid - tprinc;
                        }
                    }
                    else if (amountPaid >= pBal)
                    {
                        tprinc = pBal;
                        tinterest = amountPaid - tprinc;
                    }
                    else
                    {
                        tinterest = 0.0;
                        tprinc = amountPaid - tinterest;
                    }
                    if (sched.Count > 0)
                    {
                        int i = 0;
                        amount = tprinc;
                        var amount2 = tinterest;
                        while ((amount > 0 || amount2 > 0) && i < sched.Count)
                        {
                            var s = sched[i];
                            var princ = 0.0;
                            var interest = 0.0;

                            if (amount >= s.principalBalance)
                            {
                                princ = s.principalBalance;
                            }
                            else
                            {
                                princ = amount;
                            }
                            if (amount2 >= s.interestBalance)
                            {
                                interest = s.interestBalance;
                            }
                            else
                            {
                                interest = amount2;
                            }
                            s.principalBalance -= princ;
                            s.interestBalance -= interest;

                            if (interest > s.additionalInterestBalance) s.additionalInterestBalance = 0;
                            else if(s.additionalInterestBalance!=null) s.additionalInterestBalance -= interest;

                            tprinc2 += princ;
                            tinterest2 += interest;
                            if (sav != null && modeOfPaymentID == 4)
                            {
                                sav.principalBalance -= princ;
                                sav.interestBalance -= interest;
                                sav.savingWithdrawals.Add(new savingWithdrawal
                                {
                                    localAmount = princ + interest,
                                    creation_date = DateTime.Now,
                                    creator = userName,
                                    fxRate = 1,
                                    interestBalance = sav.interestBalance,
                                    principalBalance = sav.principalBalance,
                                    interestWithdrawal = interest,
                                    principalWithdrawal = princ,
                                    posted = false,
                                    modeOfPaymentID = 4,
                                    naration = "Deduction to pay for loan",
                                    savingID = sav.savingID,
                                    withdrawalDate = paymentDate
                                });
                            }
                            i++;
                            amount = amount - princ;
                            amount2 = amount2 - interest;
                        }
                        if (pBal + (iTotal - iPaid) <= amountPaid)
                        {
                            WriteOffInterest(le, ent, ln, userName);
                        } 
                    }
                        repayment = new coreLogic.loanRepayment
                        {
                            amountPaid = amountPaid,
                            creation_date = DateTime.Now,
                            creator = userName,
                            feePaid = 0,
                            interestPaid = tinterest2,
                            principalPaid = tprinc2,
                            repaymentDate = paymentDate,
                            modeOfPaymentID = modeOfPaymentID,
                            commission_paid = 0,
                            repaymentTypeID = int.Parse(paymentTypeID),
                            checkNo = checkNo,
                            bankID = bankID,
                            bankName = bankName
                        };
                        ln.balance -= tprinc2;
                        list.Add(repayment);

                        coreLogic.jnl_batch jb = journalextensions.Post("LN", acctID,
                            ln.loanType.accountsReceivableAccountID, amountPaid,
                            "Loan Repayment for "  + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        if (le.loanConfigs.FirstOrDefault() == null)
                        {
                            coreLogic.jnl_batch jb2 = journalextensions.Post("LN", ln.loanType.unearnedInterestAccountID,
                                ln.loanType.interestIncomeAccountID, tinterest2,
                                "Loan repayment for " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                            var j = jb2.jnl.ToList();
                            if (j.Count > 1)
                            {
                                jb.jnl.Add(j[0]);
                                jb.jnl.Add(j[1]);
                            }
                        }
                        var jar = jb.jnl.ToList();
                        if (batch != null)
                        {
                            foreach (var j in jar)
                            {
                                batch.jnl.Add(j);
                            }
                            ent.Entry(jb).State = System.Data.Entity.EntityState.Detached;
                        }
                        else
                        {
                            ent.jnl_batch.Add(jb);
                            batch = jb;
                        }
                        batchNo = batch.batch_no;

                    break;
                case "2":
                    tprinc = 0.0;
                    sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0 || p.principalBalance > 0).OrderBy(p => p.repaymentDate).ToList();
                    if (sched.Count > 0)
                    {
                        int i = 0;
                        while (amount > 0 && i < sched.Count)
                        {
                            var s = sched[i];
                            var princ = 0.0;
                            if (amount >= s.principalBalance)
                            {
                                princ = s.principalBalance;
                            }
                            else
                            {
                                princ = amount;
                            }
                            s.principalBalance -= princ;
                            i++;
                            tprinc += princ;
                            amount = amount - princ;
                        }
                         
                    }
                    
                        repayment = new coreLogic.loanRepayment
                        {
                            amountPaid = amountPaid,
                            creation_date = DateTime.Now,
                            creator = userName,
                            feePaid = 0,
                            interestPaid = 0,
                            principalPaid = amountPaid,
                            repaymentDate = paymentDate,
                            modeOfPaymentID = modeOfPaymentID,
                            commission_paid = 0,
                            repaymentTypeID = int.Parse(paymentTypeID),
                            checkNo = checkNo,
                            bankID = bankID,
                            bankName = bankName
                        };
                        ln.balance -= amountPaid;
                        list.Add(repayment);
                    coreLogic.jnl_batch jb22 = journalextensions.Post("LN", acctID,
                        ln.loanType.accountsReceivableAccountID, amt2>0?amt2:amountPaid,
                        "Loan Repayment for "  + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var jar2 = jb22.jnl.ToList();
                        if (batch != null)
                        {
                            foreach (var j in jar2)
                            {
                                batch.jnl.Add(j);
                            }
                            ent.Entry(jb22).State = System.Data.Entity.EntityState.Detached;
                        }
                        else
                        {
                            ent.jnl_batch.Add(jb22);
                            batch = jb22;
                        }
                        batchNo = batch.batch_no;
                    break;
                case "3":
                        tinterest = 0.0;
                        jnl_batch jba = null;
                        if (ln.repaymentSchedules.Count == 1 && ln.repaymentSchedules.ToList()[0].interestBalance == 0
                            )
                        {
                            sched = ln.repaymentSchedules.ToList();
                            ln.repaymentSchedules.Add(new repaymentSchedule
                            {
                                interestPayment = amountPaid,
                                interestBalance = 0,
                                principalBalance = 0,
                                principalPayment = 0,
                                loan = ln,
                                loanID = ln.loanID,
                                creation_date = DateTime.Now,
                                creator = userName,
                                proposedInterestWriteOff = 0,
                                interestWritenOff = 0,
                                repaymentDate = paymentDate,
                                balanceCD = 0,
                                balanceBF = 0
                            });
                            jba = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, amountPaid,
                                "Loan Interest- " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, paymentDate, ln.loanNo, ent, "SYSTEM", ln.client.branchID);

                        }
                        else
                        {
                            sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0).OrderBy(p => p.repaymentDate).ToList();
                            if (sched.Count > 0)
                            {
                                int i = 0;
                                tprinc = 0.0;
                                while (amount > 0 && i < sched.Count)
                                {
                                    var s = sched[i];
                                    var princ = 0.0;
                                    var interest = 0.0;
                                    if (amount >= s.interestBalance)
                                    {
                                        interest = s.interestBalance;
                                    }
                                    else
                                    {
                                        interest = amount;
                                    }
                                    s.principalBalance -= princ;
                                    s.interestBalance -= interest;

                                    //if (interest > s.additionalInterestBalance) 
                                    //    s.additionalInterestBalance = 0;
                                    //else if (s.additionalInterestBalance != null)
                                    //    s.additionalInterestBalance -= interest;

                                    i++;
                                    tprinc += princ;
                                    tinterest += interest;
                                    amount -= princ + interest;
                                }
                            }
                        }
                    repayment = new coreLogic.loanRepayment
                    {
                        amountPaid = amountPaid,
                        creation_date = DateTime.Now,
                        creator = userName,
                        feePaid = 0,
                        interestPaid = amountPaid,
                        principalPaid = 0,
                        commission_paid = 0,
                        repaymentDate = paymentDate,
                        modeOfPaymentID = modeOfPaymentID,
                        repaymentTypeID = int.Parse(paymentTypeID),
                        checkNo = checkNo,
                        bankID = bankID,
                        bankName = bankName
                    };
                    list.Add(repayment);

                    if (le.loanConfigs.FirstOrDefault() == null)
                    {
                        jb = journalextensions.Post("LN", ln.loanType.unearnedInterestAccountID,
                            ln.loanType.interestIncomeAccountID, amountPaid,
                            "Interest Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);

                        coreLogic.jnl_batch jb10 = journalextensions.Post("LN", acctID,
                            ln.loanType.accountsReceivableAccountID, amountPaid,
                            "Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j5 = jb10.jnl.ToList();
                        if (j5.Count > 1)
                        {
                            jb.jnl.Add(j5[0]);
                            jb.jnl.Add(j5[1]);
                        }
                        var jar3 = jb.jnl.ToList();
                        if (batch != null)
                        {
                            foreach (var j in jar3)
                            {
                                batch.jnl.Add(j);
                            }
                            ent.Entry(jb).State = System.Data.Entity.EntityState.Detached;
                            if (jba != null)
                            {
                                jar3 = jba.jnl.ToList();
                                batch.jnl.Add(jar3[0]);
                                batch.jnl.Add(jar3[1]); 
                                ent.Entry(jba).State = System.Data.Entity.EntityState.Detached;
                            }
                        }
                        else
                        {
                            ent.jnl_batch.Add(jb);
                            batch = jb;
                            if (jba != null)
                            {
                                jar3 = jba.jnl.ToList();
                                batch.jnl.Add(jar3[0]);
                                batch.jnl.Add(jar3[1]);
                                ent.Entry(jba).State = System.Data.Entity.EntityState.Detached;
                            }
                        }
                        batchNo = batch.batch_no;
                    }
                    else
                    {
                        if (amt2 == 0)
                        {
                            jb = journalextensions.Post("LN", acctID,
                                ln.loanType.accountsReceivableAccountID, amountPaid,
                                "Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                            var jar3 = jb.jnl.ToList();
                            if (batch != null)
                            {
                                foreach (var j in jar3)
                                {
                                    batch.jnl.Add(j);
                                }
                                ent.Entry(jb).State = System.Data.Entity.EntityState.Detached;
                            }
                            else
                            {
                                ent.jnl_batch.Add(jb);
                                batch = jb;
                            }
                            batchNo = batch.batch_no;
                        }
                    }
                    break;
                case "6":
                    if (amount >= ln.processingFeeBalance)
                    {
                        ln.processingFeeBalance = 0;
                        amount -= ln.processingFeeBalance;
                    }
                    else
                    {
                        ln.processingFeeBalance -= amount;
                        amount = 0;
                    }
                    repayment = new coreLogic.loanRepayment
                    {
                        amountPaid = amountPaid,
                        creation_date = DateTime.Now,
                        creator = userName,
                        feePaid = amountPaid,
                        interestPaid = 0,
                        principalPaid = 0,
                        commission_paid = 0,
                        repaymentDate = paymentDate,
                        modeOfPaymentID = modeOfPaymentID,
                        repaymentTypeID = int.Parse(paymentTypeID),
                        checkNo = checkNo,
                        bankID = bankID,
                        bankName = bankName
                    };
                    list.Add(repayment);

                    var jb3 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                        ln.loanType.commissionAndFeesAccountID, amountPaid,
                        "Processing Fees for " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    if (amt2 == 0)
                    {
                        coreLogic.jnl_batch jb11 = journalextensions.Post("LN", acctID,
                                ln.loanType.accountsReceivableAccountID, amountPaid,
                                "Processing fees for " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j6 = jb11.jnl.ToList();
                        if (j6.Count > 1)
                        {
                            jb3.jnl.Add(j6[0]);
                            jb3.jnl.Add(j6[1]);
                        }
                    }
                    var jar4 = jb3.jnl.ToList();
                        if (batch != null)
                        {
                            foreach (var j in jar4)
                            {
                                batch.jnl.Add(j);
                            }
                            ent.Entry(jb3).State = System.Data.Entity.EntityState.Detached;
                        }
                        else
                        {
                            ent.jnl_batch.Add(jb3);
                            batch = jb3;
                        }
                        batchNo = batch.batch_no;
                    break;
                case "5":
                    if (amount >= ln.applicationFeeBalance)
                    {
                        ln.applicationFeeBalance = 0;
                        amount -= ln.applicationFeeBalance;
                    }
                    else
                    {
                        ln.applicationFeeBalance -= amount;
                        amount = 0;
                    }
                    repayment = new coreLogic.loanRepayment
                    {
                        amountPaid = amountPaid,
                        creation_date = DateTime.Now,
                        creator = userName,
                        feePaid = amountPaid,
                        interestPaid = 0,
                        principalPaid = 0,
                        commission_paid = 0,
                        repaymentDate = paymentDate,
                        modeOfPaymentID = modeOfPaymentID,
                        repaymentTypeID = int.Parse(paymentTypeID),
                        checkNo = checkNo,
                        bankID = bankID,
                        bankName = bankName
                    };
                    list.Add(repayment);

                    var jb5 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                        ln.loanType.commissionAndFeesAccountID, amountPaid,
                        "Loan Application Fees Payment - " + amountPaid.ToString("#,###.#0")
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    if (amt2 == 0)
                    {
                        coreLogic.jnl_batch jb6 = journalextensions.Post("LN", acctID,
                                ln.loanType.unpaidCommissionAccountID, amountPaid,
                                "Loan Repayment - "   + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j2 = jb6.jnl.ToList();
                        if (j2.Count > 1)
                        {
                            jb5.jnl.Add(j2[0]);
                            jb5.jnl.Add(j2[1]);
                        }
                    }
                    ent.jnl_batch.Add(jb5);
                        batchNo = jb5.batch_no;
                        batch = jb5;
                    break;
                case "4":
                    if (amount >= ln.commissionBalance)
                    {
                        ln.commissionBalance = 0;
                        amount -= ln.commissionBalance;
                    }
                    else
                    {
                        ln.commissionBalance -= amount;
                        amount = 0;
                    }
                    repayment = new coreLogic.loanRepayment
                    {
                        amountPaid = amountPaid,
                        creation_date = DateTime.Now,
                        creator = userName,
                        feePaid = 0,
                        interestPaid = 0,
                        principalPaid = 0,
                        repaymentDate = paymentDate,
                        modeOfPaymentID = modeOfPaymentID,
                        commission_paid = amountPaid,
                        repaymentTypeID = int.Parse(paymentTypeID),
                        checkNo = checkNo,
                        bankID = bankID,
                        bankName = bankName
                    };
                    list.Add(repayment);

                    var jb4 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                        ln.loanType.commissionAndFeesAccountID, amountPaid,
                        "Loan Commission Payment - " + amountPaid.ToString("#,###.#0")
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    if (amt2 == 0)
                    {
                        coreLogic.jnl_batch jb7 = journalextensions.Post("LN", acctID,
                                ln.loanType.unpaidCommissionAccountID, amountPaid,
                                "Loan Repayment - "   + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j3 = jb7.jnl.ToList();
                        if (j3.Count > 1)
                        {
                            jb4.jnl.Add(j3[0]);
                            jb4.jnl.Add(j3[1]);
                        }
                    }
                    ent.jnl_batch.Add(jb4);
                        batchNo = jb4.batch_no;
                        batch = jb4;

                    break;
                case "7":
                    var pen = ln.loanPenalties.Where(p => p.penaltyBalance > 0).OrderBy(p => p.penaltyDate).ToList();
                    if (pen.Count > 0)
                    {
                        int i = 0;
                        var tamount = 0.0;
                        while (amount > 0 && i < pen.Count)
                        {
                            var s = pen[i];
                            var amt = 0.0;
                            if (amount >= s.penaltyBalance)
                            {
                                amt = s.penaltyBalance;
                            }
                            else
                            {
                                amt = amount;
                            }
                            s.penaltyBalance -= amt;

                            i++;
                            tamount += amt;
                            amount -= amt;
                        }
                        repayment = new coreLogic.loanRepayment
                        {
                            amountPaid = tamount,
                            creation_date = DateTime.Now,
                            creator = userName,
                            feePaid = 0,
                            interestPaid = 0,
                            principalPaid = 0,
                            commission_paid = 0,
                            repaymentDate = paymentDate,
                            modeOfPaymentID = modeOfPaymentID,
                            repaymentTypeID = int.Parse(paymentTypeID),
                            penaltyPaid = tamount,
                            checkNo = checkNo,
                            bankID = bankID,
                            bankName = bankName
                        };
                        list.Add(repayment);

                        coreLogic.jnl_batch jb8 = journalextensions.Post("LN", ln.loanType.unearnedInterestAccountID,
                            ln.loanType.interestIncomeAccountID, tamount,
                            "Additional Interest Payment - " + tamount.ToString("#,###.#0")
                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        if (amt2 == 0)
                        {
                            coreLogic.jnl_batch jb9 = journalextensions.Post("LN", acctID,
                                ln.loanType.accountsReceivableAccountID, amountPaid,
                                "Additional Interest Payment - " + amountPaid.ToString("#,###.#0")
                                + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                            var j4 = jb9.jnl.ToList();
                            if (j4.Count > 1)
                            {
                                jb8.jnl.Add(j4[0]);
                                jb8.jnl.Add(j4[1]);
                            }
                        }
                        var jar5 = jb8.jnl.ToList();
                        if (batch != null)
                        {
                            foreach (var j in jar5)
                            {
                                batch.jnl.Add(j);
                            }
                            ent.Entry(jb8).State = System.Data.Entity.EntityState.Detached;
                        }
                        else
                        {
                            ent.jnl_batch.Add(jb8);
                            batch = jb8;
                        }
                        batchNo = batch.batch_no;

                    }
                    break;
            }
            foreach (var r in list)
            {
                ln.loanRepayments.Add(r);
            }

            return batchNo;
        }

        public  string ReceivePayment(coreLoansEntities le, loan ln,
            double amountPaid, DateTime paymentDate, string bID, string bankName, string checkNo,
                coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID, 
            int? accountID, repaymentSchedule sch, ref jnl_batch batch)
        {
            var amount = amountPaid;
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();
            int? bankID = null;
            if (bID != "" && bID != null) bankID = int.Parse(bID);
            loanRepayment repayment = null;
            string batchNo = null;

            var pro = ent.comp_prof.FirstOrDefault();

            var acctID = ln.loanType.vaultAccountID; 
            if (accountID != null)
            {
                acctID = accountID.Value;
            }
            else if ((modeOfPaymentID == 2 || modeOfPaymentID == 3) && bankID == null)
            {
                var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1046");
                if (gl != null)
                {
                    acctID = gl.acct_id;
                } 
            }
            else if ((modeOfPaymentID == 2 || modeOfPaymentID == 3) && bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                if (ba != null)
                {
                    ////ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }

            var iTotal = Math.Ceiling((ln.amountDisbursed) * (ln.interestRate / 100.0)
                                * (paymentDate - ln.disbursementDate.Value).Days / 30.0);
            var iPaid = ln.repaymentSchedules.Sum(p => p.interestPayment - p.interestBalance);
            var pBal = ln.repaymentSchedules.Sum(p => p.principalBalance);
            var iBal = ln.repaymentSchedules.Sum(p => p.interestBalance); 

            var sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0 || p.principalBalance > 0).OrderBy(p => p.repaymentDate).ToList();
            var totalInt = ln.repaymentSchedules.Sum(p => p.interestPayment);
            var totalPrinc = ln.repaymentSchedules.Sum(p => p.principalPayment);
            var intRatio = totalInt / (totalInt + totalPrinc);
            var tinterest = 0.0;
            var tprinc = 0.0;
            if (iBal > 0)
            {
                tinterest = Math.Round(intRatio * amountPaid, 2);
                tprinc = amountPaid - tinterest;
                if (tprinc > pBal)
                {
                    tprinc = pBal;
                    tinterest = amount - tprinc;
                }
            }
            else
            {
                tinterest = 0.0;
                tprinc = amountPaid - tinterest;
            }
            var amt = amountPaid;
            amount = tprinc;
            var amount2 = tinterest; 
            foreach(var s in sched)
            {
                int i = 0;
                var princ = 0.0;
                var interest = 0.0;
                if (pBal + (iTotal - iPaid) > amountPaid)
                {
                    if (amount + amount2 >= s.principalBalance + s.interestBalance)
                    {
                        princ = s.principalBalance;
                        interest = s.interestBalance;
                    }
                    else if (amount + amount2 >= s.principalBalance)
                    {
                        princ = s.principalBalance;
                        interest = amount + amount2 - s.principalBalance;
                    }
                    else
                    {
                        princ = amount + amount2;
                    }
                    s.principalBalance -= princ;
                    s.interestBalance -= interest;
                }
                else
                {
                    if (amount >= s.principalBalance)
                    {
                        princ = s.principalBalance;
                    }
                    else
                    {
                        princ = amount;
                    }
                    if (amount2 >= s.interestBalance)
                    {
                        interest = s.interestBalance;
                    }
                    else
                    {
                        interest = amount;
                    }
                    s.principalBalance -= princ;
                    s.interestBalance -= interest;
                }
                i++;
                //tprinc += princ;
                //tinterest += interest;
                amount = amount - princ;
                amount2 = amount2 - interest;
                amt = amt - princ - interest;
                if (amt <= 0.1)
                {
                    break;
                }
            }
            repayment = new coreLogic.loanRepayment
            {
                amountPaid = amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = tinterest,
                principalPaid = tprinc,
                repaymentDate = paymentDate,
                modeOfPaymentID = modeOfPaymentID,
                commission_paid = 0,
                repaymentTypeID = 1,
                checkNo = checkNo,
                bankID = bankID,
                bankName = bankName
            };
            ln.balance -= tprinc;
            list.Add(repayment);
             
            coreLogic.jnl_batch jb = journalextensions.Post("LN", ln.loanType.unearnedInterestAccountID,
                ln.loanType.interestIncomeAccountID, tinterest,
                "Loan repayment for " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID); 
            coreLogic.jnl_batch jb2 = journalextensions.Post("LN", acctID,
                ln.loanType.accountsReceivableAccountID, amountPaid,
                "Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, paymentDate, ln.loanNo, ent, userName, ln.client.branchID);
            var j = jb2.jnl.ToList();
            if (j.Count > 1)
            {
                if (batch != null)
                {
                    var j1 = j.FirstOrDefault(p => p.accts.acct_id == acctID && p.ref_no == ln.loanNo);
                    var j3 = j.FirstOrDefault(p => p.accts.acct_id == ln.loanType.accountsReceivableAccountID && p.ref_no == ln.loanNo);
                    var j4 = jb.jnl.FirstOrDefault(p => p.accts.acct_id == ln.loanType.interestIncomeAccountID && p.ref_no == ln.loanNo);
                    var j5 = jb.jnl.FirstOrDefault(p => p.accts.acct_id == ln.loanType.unearnedInterestAccountID && p.ref_no == ln.loanNo);
                    var j2 = batch.jnl.FirstOrDefault(p => p.accts.acct_id == acctID);
                    j2.crdt_amt += j1.crdt_amt;
                    j2.dbt_amt += j1.dbt_amt;
                    ent.Entry(j1).State = System.Data.Entity.EntityState.Detached; 
                    batch.jnl.Add(j3);
                    batch.jnl.Add(j4);
                    batch.jnl.Add(j5);
                }
                else
                {
                    jb.jnl.Add(j[0]);
                    jb.jnl.Add(j[1]);
                }
            }
            if (batch == null)
            {
                ent.jnl_batch.Add(jb);
                batch = jb;
            }
        
            batchNo = jb.batch_no;

            foreach (var r in list)
            {
                ln.loanRepayments.Add(r);
            }

            return batchNo;
        }

        public  void ReversePayment(coreLoansEntities le, loan ln,
                coreLogic.core_dbEntities ent, loanRepayment lrp, string userName)
        { 
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>(); 
            coreSecurityEntities sec =new coreSecurityEntities();

            var pro = ent.comp_prof.FirstOrDefault();

            var acctID = ln.loanType.vaultAccountID;
            var acctID2 = ln.loanType.vaultAccountID;
            var tlrp = le.cashierReceipts.Where(p => p.clientID == lrp.loan.clientID && p.amount == lrp.amountPaid && p.txDate == lrp.repaymentDate).FirstOrDefault();
            if ((lrp.modeOfPaymentID == 2 || lrp.modeOfPaymentID == 3) && lrp.bankID == null)
            {
                var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1046"); 
                if (tlrp.cashiersTill != null)
                {
                    acctID = tlrp.cashiersTill.accountID;
                }
                else if (gl != null)
                {
                    acctID = gl.acct_id;
                }
                gl = ent.accts.FirstOrDefault(p => p.acc_num == "1047");
                if (gl != null)
                {
                    acctID2 = gl.acct_id;
                }
            }
            else if ((lrp.modeOfPaymentID == 2 || lrp.modeOfPaymentID == 3) && lrp.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == lrp.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var amount = lrp.amountPaid;

            switch (lrp.repaymentTypeID.ToString())
            {
                case "1":
                    var sched = ln.repaymentSchedules.Where(p => p.interestBalance < p.interestPayment || p.principalBalance < p.principalPayment).OrderByDescending(p => p.repaymentDate).ToList();
                    if (sched.Count > 0)
                    {
                        int i = 0;
                        var tprinc = 0.0;
                        var tinterest = 0.0;
                        while (amount > 0 && i < sched.Count)
                        {
                            var s = sched[i];
                            var princ = 0.0;
                            var interest = 0.0;
                            if (amount >= s.principalPayment + s.interestPayment)
                            {
                                princ = s.principalPayment;
                                interest = s.interestPayment;
                            }
                            else if (amount >= s.principalPayment)
                            {
                                princ = s.principalPayment;
                                interest = amount - s.principalBalance;
                            }
                            else
                            {
                                princ = amount;
                            }
                            s.principalBalance += princ;
                            s.interestBalance += interest;
                             
                            i++;
                            tprinc += princ;
                            tinterest += interest;
                            amount = amount - princ - interest;
                        }
                        
                        ln.balance += tprinc;
                        le.loanRepayments.Remove(lrp);

                        coreLogic.jnl_batch jb = journalextensions.Post("LN",
                            ln.loanType.interestIncomeAccountID, ln.loanType.unearnedInterestAccountID, tinterest,
                            "RVSL: Loan repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        coreLogic.jnl_batch jb2 = journalextensions.Post("LN",
                            ln.loanType.accountsReceivableAccountID, acctID, (tprinc + tinterest),
                            "RVSL: Loan Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j = jb2.jnl.ToList();
                        if (j.Count > 1)
                        {
                            jb.jnl.Add(j[0]);
                            jb.jnl.Add(j[1]);
                        }
                        ent.jnl_batch.Add(jb);

                    }
                    break;
                case "2":
                    sched = ln.repaymentSchedules.Where(p => p.interestBalance < p.interestPayment || p.principalBalance < p.principalPayment).OrderByDescending(p => p.repaymentDate).ToList();
                    if (sched.Count > 0)
                    {
                        int i = 0;
                        var tprinc = 0.0;
                        while (amount > 0 && i < sched.Count)
                        {
                            var s = sched[i];
                            var princ = 0.0;
                            if (amount >= s.principalPayment)
                            {
                                princ = s.principalPayment;
                            }
                            else
                            {
                                princ = amount;
                            }
                            s.principalBalance += princ;
                            i++;
                            tprinc += princ;
                            amount = amount - princ;
                        }
                        ln.balance += tprinc;
                        le.loanRepayments.Remove(lrp);

                        coreLogic.jnl_batch jb2 = journalextensions.Post("LN",
                            ln.loanType.accountsReceivableAccountID,  acctID, (tprinc),
                            "RVSSL: Principal Only Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        ent.jnl_batch.Add(jb2);

                    }
                    break;
                case "3":
                    sched = ln.repaymentSchedules.Where(p => p.interestBalance < p.interestPayment).OrderByDescending(p => p.repaymentDate).ToList();
                    if (sched.Count > 0)
                    {
                        int i = 0;
                        var tprinc = 0.0;
                        var tinterest = 0.0;
                        while (amount > 0 && i < sched.Count)
                        {
                            var s = sched[i];
                            var princ = 0.0;
                            var interest = 0.0;
                            if (amount >= s.interestPayment)
                            {
                                interest = s.interestPayment;
                            }
                            else
                            {
                                interest = amount;
                            }
                            s.principalBalance += princ;
                            s.interestBalance += interest;

                            i++;
                            tprinc += princ;
                            tinterest += interest;
                            amount -= princ + interest;
                        }
                        le.loanRepayments.Remove(lrp);

                        coreLogic.jnl_batch jb = journalextensions.Post("LN",
                            ln.loanType.interestIncomeAccountID, ln.loanType.unearnedInterestAccountID,  lrp.amountPaid,
                            "RVSL: Interest Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        coreLogic.jnl_batch jb10 = journalextensions.Post("LN",
                            ln.loanType.accountsReceivableAccountID, acctID, lrp.amountPaid,
                            "RVSL: Interest Only Repayment for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j5 = jb10.jnl.ToList();
                        if (j5.Count > 1)
                        {
                            jb.jnl.Add(j5[0]);
                            jb.jnl.Add(j5[1]);
                        }
                        ent.jnl_batch.Add(jb);

                    }
                    break;
                case "6":
                    if (amount >= ln.processingFee)
                    {
                        ln.processingFeeBalance += ln.processingFee;
                        amount -= ln.processingFee;
                    }
                    else
                    {
                        ln.processingFeeBalance += amount;
                        amount = 0;
                    }
                    le.loanRepayments.Remove(lrp);

                    var jb3 = journalextensions.Post("LN",
                        ln.loanType.commissionAndFeesAccountID, ln.loanType.unpaidCommissionAccountID, lrp.amountPaid,
                        "RVSL: Processing Fees for " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    coreLogic.jnl_batch jb11 = journalextensions.Post("LN",
                            ln.loanType.accountsReceivableAccountID, acctID, lrp.amountPaid,
                            "RVSL: Processing fees for " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var j6 = jb11.jnl.ToList();
                    if (j6.Count > 1)
                    {
                        jb3.jnl.Add(j6[0]);
                        jb3.jnl.Add(j6[1]);
                    } 
                    ent.jnl_batch.Add(jb3);
                    //ln.loanFees.Load();
                    
                    var lf = ln.loanFees.FirstOrDefault();
                    if (lf != null)
                    {
                        le.loanFees.Remove(lf);
                    }

                    break;
                case "5":
                    if (amount >= ln.applicationFee)
                    {
                        ln.applicationFeeBalance =ln.applicationFee;
                        amount -= ln.applicationFee;
                    }
                    else
                    {
                        ln.applicationFeeBalance += amount;
                        amount = 0;
                    }
                    le.loanRepayments.Remove(lrp);

                    var jb5 = journalextensions.Post("LN", 
                        ln.loanType.commissionAndFeesAccountID, ln.loanType.unpaidCommissionAccountID,lrp.amountPaid,
                        "RVSL: Loan Application Fees Payment - " 
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    coreLogic.jnl_batch jb6 = journalextensions.Post("LN",
                            ln.loanType.unpaidCommissionAccountID, acctID2, lrp.amountPaid,
                            "RVSL: Loan Repayment - "  
                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var j2 = jb6.jnl.ToList();
                    if (j2.Count > 1)
                    {
                        jb5.jnl.Add(j2[0]);
                        jb5.jnl.Add(j2[1]);
                    }
                    ent.jnl_batch.Add(jb5);

                    break;
                case "4":
                    if (amount >= ln.commission)
                    {
                        ln.commissionBalance = ln.commission;
                        amount -= ln.commission;
                    }
                    else
                    {
                        ln.commissionBalance += amount;
                        amount = 0;
                    }
                    le.loanRepayments.Remove(lrp);

                    var jb4 = journalextensions.Post("LN",
                        ln.loanType.commissionAndFeesAccountID,  ln.loanType.unpaidCommissionAccountID,lrp.amountPaid,
                        "RVSL: Loan Commission Payment - "  
                        + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    coreLogic.jnl_batch jb7 = journalextensions.Post("LN", acctID2,
                            ln.loanType.unpaidCommissionAccountID, lrp.amountPaid,
                            "RVSL: Loan Repayment - "  
                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                    var j3 = jb7.jnl.ToList();
                    if (j3.Count > 1)
                    {
                        jb4.jnl.Add(j3[0]);
                        jb4.jnl.Add(j3[1]);
                    }
                    ent.jnl_batch.Add(jb4);

                    break;
                case "7":
                    var pen = ln.loanPenalties.Where(p => p.penaltyBalance < p.penaltyFee).OrderByDescending(p => p.penaltyDate).ToList();
                    if (pen.Count > 0)
                    {
                        int i = 0;
                        var tamount = 0.0;
                        while (amount > 0 && i < pen.Count)
                        {
                            var s = pen[i];
                            var amt = 0.0;
                            if (amount >= s.penaltyFee)
                            {
                                amt = s.penaltyFee;
                            }
                            else
                            {
                                amt = amount;
                            }
                            s.penaltyBalance += amt;

                            i++;
                            tamount += amt;
                            amount -= amt;
                        }
                        le.loanRepayments.Remove(lrp);

                        coreLogic.jnl_batch jb8 = journalextensions.Post("LN",
                            ln.loanType.interestIncomeAccountID,  ln.loanType.unearnedExtraChargesAccountID,tamount,
                            "RBSL: Additional Interest Payment - "  
                            + ln.client.surName + ", " + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        coreLogic.jnl_batch jb9 = journalextensions.Post("LN", 
                            ln.loanType.unearnedExtraChargesAccountID, acctID,lrp.amountPaid,
                            "RVSL: Additional Interest Payment - "  
                            + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, lrp.repaymentDate, ln.loanNo, ent, userName, ln.client.branchID);
                        var j4 = jb9.jnl.ToList();
                        if (j4.Count > 1)
                        {
                            jb8.jnl.Add(j4[0]);
                            jb8.jnl.Add(j4[1]);
                        }
                        ent.jnl_batch.Add(jb8);

                    }
                    break;
            }
            foreach (var r in list)
            {
                ln.loanRepayments.Add(r);
            }
            if (tlrp != null)
            {
                le.cashierReceipts.Remove(tlrp);
            }

            return;
        }

        public  void ReverseInterest(coreLoansEntities le, loan ln,
                coreLogic.core_dbEntities ent, loanPenalty lrp, string userName)
        {
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();

            var pro = ent.comp_prof.FirstOrDefault();

            var acctID = ln.loanType.vaultAccountID;
            var acctID2 = ln.loanType.vaultAccountID; 
            var amount = lrp.penaltyFee;
                                     
            var jb = journalextensions.Post("LN", lrp.loan.loanType.unearnedInterestAccountID, 
                lrp.loan.loanType.accountsReceivableAccountID, lrp.penaltyFee,
                "RVSL: Additional Interest for Loan - " + lrp.loan.client.surName + "," + lrp.loan.client.otherNames,
                pro.currency_id.Value, lrp.penaltyDate, lrp.loan.loanNo, ent, userName, ln.client.branchID);
            ent.jnl_batch.Add(jb);
            le.loanPenalties.Remove(lrp);

            return;
        }

        public  void ReverseFee(coreLoansEntities le, loan ln,
                coreLogic.core_dbEntities ent, loanFee lrp, string userName)
        {
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();

            var pro = ent.comp_prof.FirstOrDefault();

            var acctID = ln.loanType.vaultAccountID;
            var acctID2 = ln.loanType.vaultAccountID;
            var amount = lrp.feeAmount;

            ln.processingFee -= lrp.feeAmount;
            ln.processingFeeBalance -= lrp.feeAmount;
            var jb2 = journalextensions.Post("LN", ln.loanType.unpaidCommissionAccountID,
                        ln.loanType.accountsReceivableAccountID, lrp.feeAmount,
                        "RVSL: Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, lrp.feeDate, ln.loanNo, ent, userName, ln.client.branchID);
            ent.jnl_batch.Add(jb2);
            le.loanFees.Remove(lrp);
            
            return;
        }

        public  void ApplyCheck(coreLoansEntities le, int loanCheckID, string userName,
            int bankID, DateTime cashDate, loan ln)
        {
           
            var check = le.loanChecks.FirstOrDefault(p => p.loanCheckID == loanCheckID);
            if (check != null)
            { 
                check.cashed = true;
                check.cashDate = cashDate;
                core_dbEntities ent = new core_dbEntities();
                ReceivePayment(le, ln, check.checkAmount, cashDate, "1", bankID.ToString(), "", 
                    check.checkNumber,
                    ent, userName,2);
                le.SaveChanges();
                ent.SaveChanges();

            }
        }

        public  void PostLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo, saving sav=null, susuAccount sc=null)
        {
           
            if (amountApproved > 0)
            {
                var amount = amountPaid.Value;
                if (addFees)
                {
                    amount += ln.applicationFeeBalance + ln.processingFeeBalance;
                    ln.applicationFeeBalance = 0;
                    ln.processingFeeBalance = 0;
                }
                if (ln.disbursementDate == null) ln.disbursementDate = disbDate.Value;
                ln.modification_date = DateTime.Now;
                ln.last_modifier = userName; 
                ln.balance += amount; 

                var interest = 0.0; 
                bool first = false;
                if (ln.amountDisbursed == 0)
                {
                    first = true;
                    if (ln.edited == false)
                    {
                        foreach (var rs in ln.repaymentSchedules.ToList())
                        {
                            le.repaymentSchedules.Remove(rs);
                        }
                        if (ln.loanTypeID == 6)
                        {
                           
                            List<coreLogic.repaymentSchedule> sched =
                                this.calculateScheduleM(amount, ln.interestRate,
                                disbDate.Value,  ln.loanTenure );
                            foreach (var rs in sched)
                            {
                                rs.creation_date = DateTime.Now;
                                rs.creator = userName;
                                ln.repaymentSchedules.Add(rs);
                            }
                        } 
                        else if (sc != null)
                        {
                            ln.repaymentSchedules.Add(new repaymentSchedule
                            {
                                additionalInterest = 0,
                                additionalInterestBalance = 0,
                                penaltyAmount = 0,
                                balanceBF = ln.amountApproved,
                                balanceCD = 0,
                                creation_date = System.DateTime.Now,
                                creator = userName,
                                edited = false,
                                interestBalance = sc.interestAmount,
                                interestPayment = sc.interestAmount,
                                origInterestPayment = sc.interestAmount,
                                origPrincipalBF = ln.amountApproved,
                                origPrincipalCD = 0,
                                origPrincipalPayment = ln.amountApproved,
                                interestWritenOff = 0,
                                principalBalance = ln.amountApproved,
                                principalPayment = ln.amountApproved,
                                repaymentDate = disbDate.Value,
                                proposedInterestWriteOff = 0
                            });
                        }
                        else
                        {
                            List<coreLogic.repaymentSchedule> sched =
                               this.calculateSchedule(amount, ln.interestRate,
                                disbDate.Value, ln.gracePeriod, ln.loanTenure,
                                ln.interestTypeID.Value, ln.repaymentModeID);
                            foreach (var rs in sched)
                            {
                                rs.creation_date = DateTime.Now;
                                rs.creator = userName;
                                ln.repaymentSchedules.Add(rs);
                            }
                        }
                    }
                }
                else
                {
                    interest = ln.repaymentSchedules.Sum(p => p.interestPayment);
                    var sched = ln.repaymentSchedules.ToList();
                    sched =
                          this.calculateSchedule(amount, ln.interestRate,
                           disbDate.Value, ln.gracePeriod, ln.loanTenure,
                           ln.interestTypeID.Value, ln.repaymentModeID, sched);
                    foreach (var rs in sched)
                    {
                        rs.modification_date = DateTime.Now;
                        rs.last_modifier = userName;
                    }
                }
                ln.amountDisbursed += amount;

                ln.loanStatusID = 4;

                int? bankID = null;
                if (bank != "") bankID = int.Parse(bank);
                var da = ent.def_accts.FirstOrDefault(p => p.code == "DA");

                int? acctID = null;
                var pro = ent.comp_prof.FirstOrDefault();
                var acc = ent.accts.FirstOrDefault(p => p.acc_num == crAccountNo);
                if (((paymentMode == "2" || paymentMode == "3") && bankID != null)
                    && (pro.traditionalLoanNo==true))
                {
                    var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                    if (ba != null)
                    {
                        //ba.acctsReference.Load();
                        if (ba.accts != null)
                        {
                            acctID = ba.accts.acct_id;
                        }
                    }
                }
                else if (ln.loanTypeID == 7 || ln.loanTypeID==8)
                {
                    acctID = le.susuConfigs.FirstOrDefault().contributionsPayableAccountID;
                }
                else if (acc != null && acc != null && pro.traditionalLoanNo==true)
                {
                    acctID = acc.acct_id;
                }
                else if (da != null)
                {
                    //da.acctsReference.Load();
                    if (da.accts != null)
                    {
                        acctID = da.accts.acct_id;
                    }
                }
                else
                {
                    var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1001");
                    if (acc2 != null)
                    {
                        acctID = acc2.acct_id;
                    }
                }
                var ls = le.insuranceSetups.FirstOrDefault(p => p.loanTypeID == ln.loanTypeID);
                jnl_batch jb;
                var insuranceAmount = 0.0;
                var amountAtBank = amount; 
                if (pro.deductProcFee == true && ln.loanTypeID!= 7)
                {
                    amountAtBank -= ln.processingFee;
                }
                if (pro.deductInsurance == true && ls != null && ln.insuranceAmount>0)
                {
                    insuranceAmount = ln.insuranceAmount;
                    amountAtBank -= insuranceAmount;
                }
                if (sav != null && pro.disburseLoansToSavingsAccount == true)
                {
                    acctID = sav.savingType.accountsPayableAccountID;
                    sav.savingAdditionals.Add(new savingAdditional
                    {
                        localAmount = amountAtBank,
                        savingAmount = amountAtBank,
                        savingDate = disbDate.Value,
                        naration = "Loan Disbursed to Savings Account",
                        modeOfPaymentID = 1,
                        interestBalance = 0,
                        principalBalance = sav.principalBalance + amountAtBank,
                        posted = true,
                        fxRate = 1,
                        creation_date = DateTime.Now,
                        creator = userName
                    });
                    sav.principalBalance += amountAtBank;
                    sav.amountInvested += amountAtBank;
                }
                jb= journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                    acctID.Value, amountAtBank,
                    "Loan Disbursement Principal - " + amountAtBank.ToString("#,###.#0")
                    + " - " + ln.client.accountNumber + " - " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                if (!ln.addFeesToPrincipal  && first==true && ln.processingFee>0)
                { 
                    var lf = ln.loanFees.FirstOrDefault();
                    if (lf == null)
                    {
                        lf = new coreLogic.loanFee
                        {
                            feeAmount =ln.processingFee,
                            feeDate = disbDate.Value,
                            feeTypeID = 1,
                            creation_date = DateTime.Now,
                            creator = userName
                        };
                        ln.loanFees.Add(lf);
                    }
                    else
                    {
                        lf.last_modifier = userName;
                        lf.modification_date = DateTime.Now;
                        lf.feeAmount = ln.processingFee;
                    } 
                    
                    if (pro.deductProcFee == false || ln.loanTypeID == 7)
                    {
                        var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                            ln.loanType.unpaidCommissionAccountID, ln.processingFee,
                            "Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                        var list = jb2.jnl.ToList();
                        jb.jnl.Add(list[0]);
                        jb.jnl.Add(list[1]);
                    }
                    else if(ln.loanTypeID!=7)
                    {
                        var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                                    ln.loanType.commissionAndFeesAccountID, ln.processingFee,
                                                    "Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                                                    pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                        var list = jb2.jnl.ToList();
                        jb.jnl.Add(list[0]);
                        jb.jnl.Add(list[1]);

                        var repayment = new coreLogic.loanRepayment
                        {
                            amountPaid = ln.processingFee,
                            creation_date = DateTime.Now,
                            creator = userName,
                            feePaid = ln.processingFee,
                            interestPaid = 0,
                            principalPaid = 0,
                            commission_paid = 0,
                            repaymentDate = disbDate.Value,
                            modeOfPaymentID = 1,
                            repaymentTypeID = 6,
                            checkNo = checkNo,
                            bankID = null,
                            bankName = null
                        };
                        ln.loanRepayments.Add(repayment);
                    }
                }
                if (pro.deductInsurance==true && ls != null)
                {
                    var lf = ln.loanInsurances.FirstOrDefault();
                    if (lf == null)
                    {
                        lf = new coreLogic.loanInsurance
                        {
                            amount = insuranceAmount,
                            insuranceDate = disbDate.Value,
                            paid=false
                        };
                        ln.loanInsurances.Add(lf);
                    }
                    else
                    { 
                        lf.amount += insuranceAmount;
                    }

                    var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                            ls.insuranceAccountID.Value, insuranceAmount,
                            "Loan Disbursement Insurance - " + ln.client.surName + "," + ln.client.otherNames,
                            pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);

                    var repayment = new coreLogic.loanRepayment
                    {
                        amountPaid = insuranceAmount,
                        creation_date = DateTime.Now,
                        creator = userName,
                        feePaid = insuranceAmount,
                        interestPaid = 0,
                        principalPaid = 0,
                        commission_paid = 0,
                        repaymentDate = disbDate.Value,
                        modeOfPaymentID = 1,
                        repaymentTypeID = 8,
                        checkNo = checkNo,
                        bankID = null,
                        bankName = null
                    };
                    ln.loanRepayments.Add(repayment);
                    lf.paid = true;
                    lf.amount -= insuranceAmount;
                }
                interest = ln.repaymentSchedules.Sum(p => p.interestPayment) - interest;
                if (interest > 0 && le.loanConfigs.FirstOrDefault() == null)
                {
                    var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                        ln.loanType.unearnedInterestAccountID, interest,
                        "Loan Disbursement Interest- " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                    ent.jnl_batch.Add(jb2);
                }
                ent.jnl_batch.Add(jb);
            }
        }

        public  void DisburseLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved, double amountDisbursed,
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo, bool post, saving sav=null,
            susuAccount sc=null)
        {
           
            if (amountPaid != null 
                   && disbDate != null)
            {
                var interest = 0.0;
                var amount = amountPaid.Value;
                var ls = le.insuranceSetups.FirstOrDefault(p => p.loanTypeID == ln.loanTypeID); 
                var insuranceAmount = 0.0;
                var amountAtBank = amount;
                var pro = ent.comp_prof.FirstOrDefault();
                if (pro.deductProcFee == true && ln.loanTypeID!=7)
                {
                    amountAtBank -= ln.processingFee;
                }
                if (pro.deductInsurance == true && ls != null && ln.insuranceAmount > 0)
                {
                    insuranceAmount = ln.insuranceAmount;
                    amountAtBank -= insuranceAmount;
                }
                if (addFees)
                {
                    amount += ln.applicationFeeBalance + ln.processingFeeBalance; 
                    ln.applicationFeeBalance = 0;
                    ln.processingFeeBalance = 0;
                }
                if (ln.amountDisbursed == 0 && ln.edited==false)
                {
                    foreach (var rs in ln.repaymentSchedules.ToList())
                    {
                        le.repaymentSchedules.Remove(rs);
                    }
                    if (ln.loanTypeID == 6)
                    {
                        List<coreLogic.repaymentSchedule> sched =
                            this.calculateScheduleM(amount, ln.interestRate,
                            disbDate.Value, ln.loanTenure);
                        foreach (var rs in sched)
                        {
                            rs.creation_date = DateTime.Now;
                            rs.creator = userName;
                            ln.repaymentSchedules.Add(rs);
                        }
                    } 
                    else
                    {
                        List<coreLogic.repaymentSchedule> sched =
                            this.calculateSchedule(amount, ln.interestRate,
                            disbDate.Value, ln.gracePeriod, ln.loanTenure,
                            ln.interestTypeID.Value, ln.repaymentModeID);
                        foreach (var rs in sched)
                        {
                            rs.creation_date = DateTime.Now;
                            rs.creator = userName;
                            ln.repaymentSchedules.Add(rs);
                        }
                    }
                } 
                if (ln.disbursementDate == null) ln.disbursementDate = disbDate.Value;
                ln.modification_date = DateTime.Now;
                ln.last_modifier = userName;

                ln.loanStatusID = 4;

                int? bankID = null;
                if (bank != "") bankID = int.Parse(bank);
                var t = new coreLogic.loanTranch
                {
                    amountDisbursed = amountPaid.Value + (ln.addFeesToPrincipal ? ln.processingFee : 0),
                    creation_date = DateTime.Now,
                    creator = userName,
                    disbursementDate = disbDate.Value,
                    modeOfPaymentID = int.Parse(paymentType),
                    bankID = bankID,
                    checkNumber = checkNo
                };
                ln.loanTranches.Add(t);
                var da = ent.def_accts.FirstOrDefault(p => p.code == "DA");

                var a = ent.accts.FirstOrDefault(p => p.acc_num == "1001");
                int? acctID = (a == null) ? null : (int?)a.acct_id;
                if (da != null)
                {
                    //da.acctsReference.Load();
                    if (da.accts != null)
                    {
                        acctID = da.accts.acct_id;
                    }
                }
                var acctID2 = ln.loanType.vaultAccountID;
                if (crAccountNo!="" && crAccountNo!=null  && pro.traditionalLoanNo==true)
                {
                    var gl = ent.accts.FirstOrDefault(p => p.acc_num == crAccountNo);
                    if (gl != null)
                    {
                        acctID2=gl.acct_id;
                    }
                }
                else if ((paymentMode == "2" || paymentMode == "3") && bankID == null)
                {
                    var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1001");
                    if (gl != null)
                    {
                        acctID2 = gl.acct_id;
                    }
                }
                else if ((paymentMode == "2" || paymentMode == "3") && bankID != null)
                {
                    var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                    if (ba != null)
                    {
                        //ba.acctsReference.Load();
                        if (ba.accts != null)
                        {
                            acctID2 = ba.accts.acct_id;
                        }
                    }
                }
                if ((pro.traditionalLoanNo == false || paymentMode == "1") && sav == null && ln.loanTypeID != 7 && ln.loanTypeID!= 8)
                {
                    var jb = journalextensions.Post("LN", acctID.Value,
                        acctID2, amountAtBank,
                        "Loan Disbursement Principal- " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, disbDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                    ent.jnl_batch.Add(jb);
                }
            }
        }

        public  void PostDisbursement(coreLoansEntities le, double? amountPaid, 
            DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent,
            bool addFees, string userName, string paymentMode, string crAccountNo)
        {
            if (amountPaid >0)
            { 
                var acctID = ent.accts.FirstOrDefault(p => p.acc_num == "1001").acct_id;
                var acctID2 = ent.accts.FirstOrDefault(p => p.acc_num == "1000").acct_id;
                if (crAccountNo != "" && crAccountNo != null)
                {
                    var gl = ent.accts.FirstOrDefault(p => p.acc_num == crAccountNo);
                    if (gl != null)
                    {
                        acctID2 = gl.acct_id;
                    }
                }

                var pro = ent.comp_prof.FirstOrDefault();
                var jb = journalextensions.Post("LN", acctID,
                    acctID2, amountPaid.Value,
                    "Loan Disbursement" ,
                    pro.currency_id.Value, disbDate.Value, "", ent, userName, null);
                 
                ent.jnl_batch.Add(jb);
            }
        }

        public  void ReverseDisbursement(coreLoansEntities le, loan ln, loanTranch lt, core_dbEntities ent,
            string userName)
        {
         
            //ln.loanFees.Load();
            var interest = 0.0;
            loanTranch lt2 = null;
            lt2 = le.loanTranches.FirstOrDefault(p => p.loanID == ln.loanID && p.loanTranchID != lt.loanTranchID);
            var tlrp = le.cashierDisbursements.Where(p => p.clientID==lt.loan.clientID && p.amount == lt.amountDisbursed && p.txDate == lt.disbursementDate).FirstOrDefault();
            var amount = lt.amountDisbursed;
            if (ln.addFeesToPrincipal)
            {
                amount -= ln.applicationFee + ln.processingFee;
                ln.applicationFeeBalance = 0;
                ln.processingFeeBalance = 0;
            }


            ln.amountDisbursed -= lt.amountDisbursed;
            ln.disbursementDate = (lt2 != null) ? (DateTime?)lt2.disbursementDate : null;
            ln.modification_date = DateTime.Now;
            ln.last_modifier = userName;
            ln.balance -= lt.amountDisbursed;
            ln.processingFeeBalance = ln.processingFee;
            
            if (lt2 == null)
            {
                ln.loanStatusID = 3;
            }

            interest = ln.repaymentSchedules.Sum(p => p.interestPayment);
            foreach (var rs in ln.repaymentSchedules.ToList())
            {
                le.repaymentSchedules.Remove(rs);
            }
            if (lt2 != null)
            {
                List<coreLogic.repaymentSchedule> sched =
                    this.calculateSchedule(lt2.amountDisbursed, ln.interestRate,
                    lt2.disbursementDate, ln.gracePeriod, ln.loanTenure,
                    ln.interestTypeID.Value, ln.repaymentModeID);
                foreach (var rs in sched)
                {
                    rs.creation_date = DateTime.Now;
                    rs.creator = userName;
                    ln.repaymentSchedules.Add(rs);
                }
            }
            var acctID = ln.loanType.vaultAccountID;
            if ((lt.modeOfPaymentID == 2 || lt.modeOfPaymentID == 3) && lt.bankID == null)
            {
                var gl = ent.accts.FirstOrDefault(p => p.acc_num == "1046");
                if (tlrp.cashiersTill != null)
                {
                    acctID = tlrp.cashiersTill.accountID;
                }
                else if (gl != null)
                {
                    acctID = gl.acct_id;
                } 
            }
            else if ((lt.modeOfPaymentID == 2 || lt.modeOfPaymentID == 3) && lt.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == lt.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
             
            var pro = ent.comp_prof.FirstOrDefault();
            var jb = journalextensions.Post("LN",
                acctID, ln.loanType.accountsReceivableAccountID,lt.amountDisbursed,
                "RVSL: Loan Disbursement Principal- " 
                + " - " + ln.client.surName + "," + ln.client.otherNames,
                pro.currency_id.Value, lt.disbursementDate, ln.loanNo, ent, userName, ln.client.branchID);
            interest -= ln.repaymentSchedules.Sum(p => p.interestPayment);
            if (interest > 0)
            {
                var jb2 = journalextensions.Post("LN",
                    ln.loanType.unearnedInterestAccountID, ln.loanType.accountsReceivableAccountID, interest,
                    "RVSL: Loan Disbursement Interest- "
                        + " - " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, lt.disbursementDate, ln.loanNo, ent, userName, ln.client.branchID);
                var list = jb2.jnl.ToList();
                jb.jnl.Add(list[0]);
                jb.jnl.Add(list[1]);
            }
            if (ln.processingFee > 0)
            {
                var jb3 = journalextensions.Post("LN",
                    ln.loanType.unpaidCommissionAccountID, ln.loanType.accountsReceivableAccountID, 
                    ln.processingFee,
                    "RVSL: Loan Disbursement Fees- "
                    + " - " + ln.client.surName + "," + ln.client.otherNames,
                    pro.currency_id.Value, lt.disbursementDate, ln.loanNo, ent, userName, ln.client.branchID);
                var list2 = jb3.jnl.ToList();
                jb.jnl.Add(list2[0]);
                jb.jnl.Add(list2[1]);
            }
            ent.jnl_batch.Add(jb);
            if (tlrp != null)
            {
                le.cashierDisbursements.Remove(tlrp);
            }
            le.loanTranches.Remove(lt);
            var lf = ln.loanFees.FirstOrDefault();
            if (lf != null)
            {
                le.loanFees.Remove(lf);
            }
        }

        public  void WriteOffInterest(coreLoansEntities le, core_dbEntities ent,
            loan ln, string userName)
        {
            jnl_batch batch = null;
            var pro = ent.comp_prof.FirstOrDefault();
            var rps = ln.repaymentSchedules.Where(p => p.interestBalance > 0);
            foreach (var rp in rps)
            {
                rp.proposedInterestWriteOff = rp.interestBalance;
            /*
                if (batch == null)
                {
                    batch = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                        rp.loan.loanType.accountsReceivableAccountID, rp.proposedInterestWriteOff,
                        "Loan Interest Write off - " + rp.proposedInterestWriteOff.ToString("#,###.#0")
                        + " - " + rp.loan.client.accountNumber + " - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                        pro.currency_id.Value, DateTime.Now, rp.loan.loanNo, ent, userName);
                }
                else
                {
                    var batch2 = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                        rp.loan.loanType.accountsReceivableAccountID, rp.proposedInterestWriteOff,
                        "Loan Interest Write off - " + rp.proposedInterestWriteOff.ToString("#,###.#0")
                        + " - " + rp.loan.client.accountNumber + " - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                        pro.currency_id.Value, DateTime.Now, rp.loan.loanNo, ent, userName);
                    var j = batch2.jnl.ToList();
                    if (j.Count > 1)
                    {
                        batch.jnl.Add(j[0]);
                        batch.jnl.Add(j[1]);
                    }
                }*/
            }
            if (batch != null
                )
            {
                ent.jnl_batch.Add(batch);
            }
        }

        public  void ClearOffInterest(coreLoansEntities le, core_dbEntities ent,
            loan ln, string userName)
        {
            jnl_batch batch = null;
            var pro = ent.comp_prof.FirstOrDefault();
            var rps = ln.repaymentSchedules.Where(p => p.interestBalance > 0);
            foreach (var rp in rps)
            {
                if (ln.balance < 2)
                {
                    rp.proposedInterestWriteOff = 0;
                }
                /*
                    if (batch == null)
                    {
                        batch = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                            rp.loan.loanType.accountsReceivableAccountID, rp.proposedInterestWriteOff,
                            "Loan Interest Write off - " + rp.proposedInterestWriteOff.ToString("#,###.#0")
                            + " - " + rp.loan.client.accountNumber + " - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                            pro.currency_id.Value, DateTime.Now, rp.loan.loanNo, ent, userName);
                    }
                    else
                    {
                        var batch2 = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                            rp.loan.loanType.accountsReceivableAccountID, rp.proposedInterestWriteOff,
                            "Loan Interest Write off - " + rp.proposedInterestWriteOff.ToString("#,###.#0")
                            + " - " + rp.loan.client.accountNumber + " - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                            pro.currency_id.Value, DateTime.Now, rp.loan.loanNo, ent, userName);
                        var j = batch2.jnl.ToList();
                        if (j.Count > 1)
                        {
                            batch.jnl.Add(j[0]);
                            batch.jnl.Add(j[1]);
                        }
                    }*/
            }
            if (batch != null
                )
            {
                ent.jnl_batch.Add(batch);
            }
        }

        public  void CreatePenalty(coreLoansEntities le, core_dbEntities ent,
            loan ln, string userName, double amount, DateTime date)
        {
            ln.loanPenalties.Add(new loanPenalty { 
                proposedAmount=amount,
                creation_date=DateTime.Now,
                creator=userName,
                loanID=ln.loanID,
                penaltyBalance=amount,
                penaltyDate=date,
                penaltyFee=amount
            });
        }

        public  void CashierDisburse(coreLoansEntities le, loan ln,  core_dbEntities ent, cashierDisbursement d,
             string userName, susuAccount sc=null)
        {
          
            if (ln.loanTypeID != 5 )
            {
                string crAccNo = null;
                var cnfg = le.susuConfigs.FirstOrDefault();
                if (ln.loanTypeID == 7 || ln.loanTypeID==8)
                {
                    var acc = ent.accts.FirstOrDefault(p => p.acct_id == cnfg.contributionsPayableAccountID);
                    crAccNo = acc.acc_num;
                }
                else if (d.paymentModeID == 1 && ent.comp_prof.FirstOrDefault().traditionalLoanNo == true)
                {
                    var acc = ent.accts.FirstOrDefault(p => p.acct_id == d.cashiersTill.accountID);
                    crAccNo = acc.acc_num;
                }
                var pro = ent.comp_prof.FirstOrDefault();
                saving sav = null;
                if (pro.disburseLoansToSavingsAccount == true && d.postToSavingsAccount == true)
                {
                    sav = le.savings.FirstOrDefault(p => p.clientID == ln.client.clientID);
                    if (sav != null)
                    {
                        //sav.savingTypeReference.Load();
                        crAccNo = ent.accts.FirstOrDefault(p => p.acct_id == sav.savingType.accountsPayableAccountID).acc_num;
                    }
                }
                this.PostLoan(le, ln, d.amount, d.amount,
                        d.txDate, (d.bankID == null) ? "" : d.bankID.ToString(),
                       d.paymentModeID.ToString(), d.checkNo, ent, d.addFees, userName, d.paymentModeID.ToString(),
                        crAccNo, sav, sc);
                this.DisburseLoan(le, ln, d.amount, d.amount,
                    d.amount, d.txDate, (d.bankID == null) ? "" : d.bankID.ToString(),
                    d.paymentModeID.ToString(), d.checkNo, ent, d.addFees, userName, d.paymentModeID.ToString(),
                    crAccNo, true, sav, sc);
            }
            else
            {
                this.PostLoan(le, ln, d.amount, d.amount,
                    d.txDate, (d.bankID == null) ? "" : d.bankID.ToString(),
                    d.paymentModeID.ToString(), d.checkNo, ent, d.addFees,
                    userName, d.paymentModeID.ToString(), null, null);
                this.DisburseLoan(le, ln, d.amount, d.amount,
                    0, d.txDate, (d.bankID == null) ? "" : d.bankID.ToString(),
                    d.paymentModeID.ToString(), d.checkNo, ent, d.addFees, 
                    userName, d.paymentModeID.ToString(), 
                    null, false, null);
            }
            ln.disbursedBy = userName;
        }

        public  void CashierReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName)
        {           
            int? crAccNo = null;
            if (r.paymentModeID == 1)
            {
                var acc = ent.accts.FirstOrDefault(p => p.acct_id == r.cashiersTill.accountID);
                crAccNo = acc.acct_id;
                r.tillAccountID = crAccNo;
            }
            else if (r.paymentModeID == 2)
            {
                return;
            }
            else if (r.paymentModeID == 4)
            {
                var sv = le.savings.FirstOrDefault(p => p.clientID == ln.clientID);
                if (sv != null && sv.savingType.accountsPayableAccountID != null
                    && sv.principalBalance+sv.interestBalance>=r.amount
                    )
                { 
                    crAccNo = sv.savingType.accountsPayableAccountID;
                }
            }

            jnl_batch batch = null;
            if (r.principalAmount == 0 && r.interestAmount == 0 & r.feeAmount == 0 && r.addInterestAmount == 0)
            {
                string batchNo = this.ReceivePayment(le, ln, r.amount,
                    r.txDate, r.repaymentTypeID.ToString(), (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                r.batchNo1 = batchNo;
            }
            else
            {
                if (r.principalAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.principalAmount,
                        r.txDate, "2", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                        r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo2 = batchNo;
                }
                if (r.interestAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.interestAmount,
                        r.txDate, "3", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo3 = batchNo;
                }
                if (r.feeAmount > 0)
                {
                    string batchNo = this.ReceivePayment(le, ln, r.feeAmount,
                        r.txDate, "6", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo4 = batchNo;
                    if (ln.loanTypeID == 5)
                    {
                        var pro = ent.comp_prof.FirstOrDefault();
                        var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                           ln.loanType.unpaidCommissionAccountID, ln.processingFee,
                           "Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                           pro.currency_id.Value, ln.disbursementDate.Value, ln.loanNo, ent, userName, ln.client.branchID);
                        ent.jnl_batch.Add(jb2);
                    }
                }
                if (r.addInterestAmount > 0)
                {
                    if (ln.loanPenalties.FirstOrDefault(p => p.penaltyBalance == r.addInterestAmount) == null)
                    {
                        ln.loanPenalties.Add(new loanPenalty { 
                            proposedAmount=0,
                            penaltyFee=r.addInterestAmount,
                            loan=ln,
                            creation_date=DateTime.Now,
                            creator=userName,
                            penaltyBalance=r.addInterestAmount,
                            penaltyDate=r.txDate,
                            loanID=ln.loanID
                        });
                        var pro = ent.comp_prof.FirstOrDefault();
                        var jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                 ln.loanType.unearnedInterestAccountID, r.addInterestAmount,
                                 "Additional Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                 pro.currency_id.Value, r.txDate, ln.loanNo, ent, userName, ln.client.branchID);
                        ent.jnl_batch.Add(jb);
                    }
                    string batchNo = this.ReceivePayment(le, ln, r.addInterestAmount,
                        r.txDate, "7", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo5 = batchNo;
                }
            }
        }

        public  void CloseCashierReceipt(coreLoansEntities le,loan ln, cashierReceipt r, core_dbEntities ent, string userName, ref jnl_batch batch)
        {
            var credit = 0.0;
            var debit = 0.0;
            string description = "";

            if (r.batchNo1 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo1);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }
            else if (r.batchNo2 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo2);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }
            else if (r.batchNo3 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo3);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }
            else if (r.batchNo4 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo4);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }
            else if (r.batchNo5 != null)
            {
                var b = ent.jnl_batch.FirstOrDefault(p => p.batch_no == r.batchNo5);
                if (b != null)
                {
                    foreach (var j in b.jnl.Where(p => p.accts.acct_id == r.tillAccountID).ToList())
                    {
                        credit += j.crdt_amt;
                        debit += j.dbt_amt;
                        description = j.description;
                    }
                }
            }

            var prof = ent.comp_prof.FirstOrDefault();
            if (credit > 0 || debit > 0)
            {
                var amount = debit-credit;
                if (batch == null)
                {
                    if (amount > 0)
                        batch = journalextensions.Post("LN", ln.loanType.vaultAccountID, r.tillAccountID.Value, amount,
                            "Daily Posting - " + r.cashiersTill.userName + " - " + r.txDate.ToString("dd-MMM-yyyy"),
                            prof.currency_id.Value, r.txDate, r.cashiersTill.userName, ent, userName, ln.client.branchID);
                    else
                        batch = journalextensions.Post("LN", r.tillAccountID.Value, ln.loanType.vaultAccountID, -amount,
                            "Daily Posting - " + r.cashiersTill.userName + " - " + r.txDate.ToString("dd-MMM-yyyy"),
                            prof.currency_id.Value, r.txDate, r.cashiersTill.userName, ent, userName, ln.client.branchID);

                }
                else
                {
                    jnl j = journalextensions.Post("LN", "CR", r.tillAccountID.Value, amount,
                        description,
                        prof.currency_id.Value, r.txDate, r.cashiersTill.userName, ent, userName, ln.client.branchID);
                    batch.jnl.Add(j);
                    j = batch.jnl.FirstOrDefault(p => p.accts.acct_id == ln.loanType.vaultAccountID);
                    if (j == null)
                    {
                        j = journalextensions.Post("LN", "DR", ln.loanType.vaultAccountID, amount,
                           "Daily Posting - " + r.cashiersTill.userName + " - " + r.txDate.ToString("dd-MMM-yyyy"),
                           prof.currency_id.Value, r.txDate, r.cashiersTill.userName, ent, userName, ln.client.branchID);
                        batch.jnl.Add(j);
                    }
                    else
                    {
                        j.dbt_amt += amount;
                    }
                }
            }
        }

        public  void CashierCheckReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName, 
            int? crAccNo, ref jnl_batch batch)
        {
            multiPaymentClient mpc = (
                               from m in le.multiPaymentClients
                               from r2 in le.cashierReceipts 
                               where r2.txDate == r.txDate
                                   && r2.clientID == r.clientID 
                                   && m.cashierReceiptID == r2.cashierReceiptID
                               select m
                           ).FirstOrDefault();

            var acc = ent.def_accts.FirstOrDefault(p => p.code == "RF");
            int? ba = null; 
            var b = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == r.bankID);
            if (b != null) ba = b.accts.acct_id; 
            if (r.principalAmount == 0 && r.interestAmount == 0 & r.feeAmount == 0 && r.addInterestAmount == 0)
            { 
                string batchNo = this.ReceivePayment(le, ln, r.amount,
                    r.txDate, r.repaymentTypeID.ToString(), (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                r.batchNo1 = batchNo;
            }
            else
            {
                if (r.principalAmount > 0)
                { 
                    string batchNo = this.ReceivePayment(le, ln, r.principalAmount,
                        r.txDate, "2", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                        r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo2 = batchNo;
                }
                if (r.interestAmount > 0)
                { 
                    string batchNo = this.ReceivePayment(le, ln, r.interestAmount,
                        r.txDate, "3", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo3 = batchNo;
                }
                if (r.feeAmount > 0)
                { 
                    string batchNo = this.ReceivePayment(le, ln, r.feeAmount,
                        r.txDate, "6", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    r.batchNo4 = batchNo;
                }
                if (r.addInterestAmount > 0)
                {
                    jnl_batch jb = null;
                    if (ln.loanPenalties.FirstOrDefault(p => p.penaltyBalance == r.addInterestAmount) 
                        == null)
                    {
                        ln.loanPenalties.Add(new loanPenalty
                        {
                            proposedAmount = 0,
                            penaltyFee = r.addInterestAmount,
                            loan = ln,
                            creation_date = DateTime.Now,
                            creator = userName,
                            penaltyBalance = r.addInterestAmount,
                            penaltyDate = r.txDate,
                            loanID = ln.loanID
                        });
                        var pro = ent.comp_prof.FirstOrDefault();
                        jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                 ln.loanType.unearnedInterestAccountID, r.addInterestAmount,
                                 "Additional Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                 pro.currency_id.Value, r.txDate, ln.loanNo, ent, userName, ln.client.branchID);                        
                    } 
                    string batchNo = this.ReceivePayment(le, ln, r.addInterestAmount,
                        r.txDate, "7", (r.bankID == null) ? "" : r.bankID.ToString(), "",
                                    r.checkNo, ent, userName, r.paymentModeID, 0, crAccNo, ref batch);
                    if (jb != null && batch != null)
                    {
                        var js = jb.jnl.ToList();
                        batch.jnl.Add(js[0]);
                        batch.jnl.Add(js[1]);
                        ent.Entry(jb).State = System.Data.Entity.EntityState.Detached;
                    }
                    r.batchNo5 = batchNo;
                }
            }
            if (batch != null && ba != null && acc != null)
            {
                var js = batch.jnl.Where(p => p.accts.acct_id == ba
                                && p.description.Contains("Payment made by") == false).ToList();
                foreach (var j2 in js)
                {
                    ent.Entry(j2).State = System.Data.Entity.EntityState.Detached;
                }
            }
        }

        public  void PostDepositsWithdrawal(depositWithdrawal da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id ==da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", da.deposit.depositType.accountsPayableAccountID.Value,
                acctID, (da.interestWithdrawal + da.principalWithdrawal),
                "Withdrawal from Investment Account - " + (da.principalWithdrawal + da.interestWithdrawal).ToString("#,###.#0")
                + " - " + da.deposit.client.accountNumber + " - " + da.deposit.client.surName + "," + da.deposit.client.otherNames,
                pro.currency_id.Value,da.withdrawalDate, da.deposit.depositNo, ent, userName,
                 da.deposit.client.branchID);
            var js = jb.jnl.FirstOrDefault(p => p.accts.acct_id == da.deposit.depositType.accountsPayableAccountID);
            js.dbt_amt = da.principalWithdrawal;
            js.crdt_amt = 0;

            var jb2 = journalextensions.Post("LN", da.deposit.depositType.interestPayableAccountID.Value,
                acctID, (da.interestWithdrawal ),
                "Withdrawal from Investment Account - "
                + " - " + da.deposit.client.accountNumber + " - " + da.deposit.client.surName + "," + da.deposit.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.deposit.depositNo, ent, userName,
                 da.deposit.client.branchID);
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == acctID);
            ent.Entry(js).State = System.Data.Entity.EntityState.Detached;
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == da.deposit.depositType.interestPayableAccountID);
            jb.jnl.Add(js);

            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public  void PostDepositAdditional(depositAdditional da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", acctID,
                da.deposit.depositType.accountsPayableAccountID.Value, da.depositAmount,
                "Deposit into Investment - " + da.depositAmount
                + " - " + da.deposit.client.accountNumber + " - " + da.deposit.client.surName + "," + da.deposit.client.otherNames,
                pro.currency_id.Value, da.depositDate, da.deposit.depositNo, ent, userName,
                da.deposit.client.branchID);
            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public  void PostSavingsWithdrawal(savingWithdrawal da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            } 
            var jb = journalextensions.Post("LN", da.saving.savingType.accountsPayableAccountID.Value,
                acctID, (da.interestWithdrawal + da.principalWithdrawal),
                "Withdrawal from Savings Account - " + (da.principalWithdrawal + da.interestWithdrawal).ToString("#,###.#0")
                + " - " + da.saving.client.accountNumber + " - " + da.saving.client.surName + "," + da.saving.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.saving.savingNo, ent, userName,
                 da.saving.client.branchID);
            var js = jb.jnl.FirstOrDefault(p=> p.accts.acct_id==da.saving.savingType.accountsPayableAccountID);
            js.dbt_amt = da.principalWithdrawal;
            js.crdt_amt = 0;

            var jb2 = journalextensions.Post("LN", da.saving.savingType.interestPayableAccountID,
                acctID, (da.interestWithdrawal),
                "Withdrawal from Savings Account - " 
                + " - " + da.saving.client.accountNumber + " - " + da.saving.client.surName + "," + da.saving.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.saving.savingNo, ent, userName,
                 da.saving.client.branchID);
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == acctID);
            ent.Entry(js).State = System.Data.Entity.EntityState.Detached;
            js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == da.saving.savingType.interestPayableAccountID);
            jb.jnl.Add(js);

            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public  void PostSavingAdditional(savingAdditional da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", acctID,
                da.saving.savingType.accountsPayableAccountID.Value, da.savingAmount,
                "Deposit into Savings Account - " + da.savingAmount
                + " - " + da.saving.client.accountNumber + " - " + da.saving.client.surName + "," + da.saving.client.otherNames,
                pro.currency_id.Value, da.savingDate, da.saving.savingNo, ent, userName,
                da.saving.client.branchID);
            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public  void PostInvestmentWithdrawal(investmentWithdrawal da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", 
                acctID, da.investment.investmentType.accountsPayableAccountID.Value,(da.interestWithdrawal + da.principalWithdrawal),
                "Withdrawal from Investment - " + (da.principalWithdrawal + da.interestWithdrawal).ToString("#,###.#0")
                + " - " + da.investment.client.accountNumber + " - " + da.investment.client.surName + "," + da.investment.client.otherNames,
                pro.currency_id.Value, da.withdrawalDate, da.investment.investmentNo, ent, userName,
                 da.investment.client.branchID);

            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public  void PostInvestmentAdditional(investmentAdditional da, string userName, core_dbEntities ent,
            coreLoansEntities le, cashiersTill ct)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acctID = ct.accountID;
            if (da.modeOfPayment.modeOfPaymentID != 1 && da.bankID != null)
            {
                var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == da.bankID);
                if (ba != null)
                {
                    //ba.acctsReference.Load();
                    if (ba.accts != null)
                    {
                        acctID = ba.accts.acct_id;
                    }
                }
            }
            var jb = journalextensions.Post("LN", da.investment.investmentType.accountsPayableAccountID.Value, acctID,
                da.investmentAmount,
                "Investment Placed - " + da.investmentAmount
                + " - " + da.investment.client.accountNumber + " - " + da.investment.client.surName + "," + da.investment.client.otherNames,
                pro.currency_id.Value, da.investmentDate, da.investment.investmentNo, ent, userName,
                da.investment.client.branchID);
            ent.jnl_batch.Add(jb);
            da.posted = true;
        }

        public void PostSusuContribution(coreLoansEntities le, core_dbEntities ent, susuContribution sc,
            string userName)
        {
            var rem = sc.amount;
            foreach (var r in sc.susuAccount.susuContributionSchedules.Where(p => p.balance > 0).OrderBy(p => p.plannedContributionDate))
            {
                var amt = sc.susuAccount.contributionAmount;
                if (amt > rem)
                {
                    amt = rem;
                }
                if (amt <= 0) break;
                rem = rem - amt;
                r.balance = r.balance - amt;
                r.actualContributionDate = sc.contributionDate;
            }
            var conf = le.susuConfigs.FirstOrDefault();
            var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 7);
            if (lt == null) lt = le.loanTypes.FirstOrDefault(); 
            var pro = ent.comp_prof.FirstOrDefault();

            var proc = sc.susuAccount.commissionAmount-sc.susuAccount.commissionPaid;
            var it = sc.susuAccount.interestAmount-sc.susuAccount.interestPaid;
            var pr = sc.susuAccount.amountEntitled - proc - it - sc.susuAccount.principalPaid;
            var c = le.cashiersTills.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.ToLower().Trim());
            int? acctID = null;

            if (c != null) acctID = c.accountID;
            else acctID = lt.vaultAccountID;

            var procAmt = (proc / sc.susuAccount.amountEntitled) * sc.amount;
            var itAmt = (it / sc.susuAccount.amountEntitled) * sc.amount;
            var prAmt = sc.amount - itAmt - procAmt;

            if (procAmt > 0)
            {
                var jb3 = journalextensions.Post("LN", acctID.Value,
                    lt.commissionAndFeesAccountID, procAmt,
                    "Group Susu Commission for " + sc.susuAccount.client.surName + "," + sc.susuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.susuAccount.susuAccountNo, ent, userName, sc.susuAccount.client.branchID);
                coreLogic.jnl_batch jb4 = journalextensions.Post("LN", lt.unpaidCommissionAccountID,
                    conf.contributionsPayableAccountID.Value, procAmt,
                    "Group Susu Commission for " + sc.susuAccount.client.surName + "," + sc.susuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.susuAccount.susuAccountNo, ent, userName,
                    sc.susuAccount.client.branchID);
                var j = jb4.jnl.ToList();
                if (j.Count > 1)
                {
                    jb3.jnl.Add(j[0]);
                    jb3.jnl.Add(j[1]);
                }
                ent.jnl_batch.Add(jb3);
            }
            if (itAmt+prAmt > 0)
            {
                coreLogic.jnl_batch jb = journalextensions.Post("LN", lt.unearnedInterestAccountID,
                        lt.interestIncomeAccountID, itAmt,
                        "Group Susu Contribution for " + sc.susuAccount.client.surName + "," + sc.susuAccount.client.otherNames,
                        pro.currency_id.Value, sc.contributionDate, sc.susuAccount.susuAccountNo, ent, userName,
                        sc.susuAccount.client.branchID);
                coreLogic.jnl_batch jb2 = journalextensions.Post("LN",
                   acctID.Value, conf.contributionsPayableAccountID.Value, itAmt + prAmt,
                    "Group Susu Contribution for " + sc.susuAccount.client.surName + "," + sc.susuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.susuAccount.susuAccountNo, ent, userName,
                    sc.susuAccount.client.branchID);
                var j = jb2.jnl.ToList();
                if (j.Count > 1)
                {
                    jb.jnl.Add(j[0]);
                    jb.jnl.Add(j[1]);
                }
                ent.jnl_batch.Add(jb);
            }
            sc.appliedToLoan = true;
            sc.posted = true;
            if (sc.susuAccount.startDate == null)
            {
                sc.susuAccount.startDate = sc.contributionDate;
            }
        }

        public void PostRegularSusuContribution(coreLoansEntities le, core_dbEntities ent, regularSusuContribution sc,
            string userName)
        {
            var rem = sc.amount;
            foreach (var r in sc.regularSusuAccount.regularSusuContributionSchedules.Where(p => p.balance > 0).OrderBy(p => p.plannedContributionDate))
            {
                var amt = sc.regularSusuAccount.contributionAmount;
                if (amt > rem)
                {
                    amt = rem;
                }
                if (amt <= 0) break;
                rem = rem - amt;
                r.balance = r.balance - amt;
                r.actualContributionDate = sc.contributionDate;
            }
            var conf = le.susuConfigs.FirstOrDefault();
            var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 8);
            if (lt == null) lt = le.loanTypes.FirstOrDefault();
            var pro = ent.comp_prof.FirstOrDefault();

            var proc = sc.regularSusuAccount.regularSusCommissionAmount - sc.regularSusuAccount.commissionPaid;
            var it = sc.regularSusuAccount.interestAmount - sc.regularSusuAccount.interestPaid;
            var pr = sc.regularSusuAccount.amountEntitled - proc - it - sc.regularSusuAccount.principalPaid; 
            var c = le.cashiersTills.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.ToLower().Trim());
            int? acctID = null;

            if (c != null) acctID = c.accountID;
            else acctID = lt.vaultAccountID;

            var procAmt = (proc / sc.regularSusuAccount.amountEntitled) * sc.amount;
            var itAmt = (it / sc.regularSusuAccount.amountEntitled) * sc.amount;
            var prAmt = sc.amount - itAmt - procAmt;

            if (procAmt > 0)
            {
                var jb3 = journalextensions.Post("LN", acctID.Value,
                    lt.commissionAndFeesAccountID, procAmt,
                    "Regular Susu Commission for " + sc.regularSusuAccount.client.surName + "," + sc.regularSusuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.regularSusuAccount.regularSusuAccountNo, ent, userName, sc.regularSusuAccount.client.branchID);
                ent.jnl_batch.Add(jb3);
            }
            var amount = sc.amount - proc;
            if (amount > 0)
            {
                coreLogic.jnl_batch jb = journalextensions.Post("LN", lt.unearnedInterestAccountID,
                        lt.interestIncomeAccountID, itAmt,
                        "Regular Susu Contribution for " + sc.regularSusuAccount.client.surName + "," + sc.regularSusuAccount.client.otherNames,
                        pro.currency_id.Value, sc.contributionDate, sc.regularSusuAccount.regularSusuAccountNo, ent, userName, sc.regularSusuAccount.client.branchID);
                coreLogic.jnl_batch jb2 = journalextensions.Post("LN", conf.contributionsPayableAccountID.Value,
                    acctID.Value, itAmt+prAmt,
                    "Regular Susu Contribution for " + sc.regularSusuAccount.client.surName + "," + sc.regularSusuAccount.client.otherNames,
                    pro.currency_id.Value, sc.contributionDate, sc.regularSusuAccount.regularSusuAccountNo, ent, userName, sc.regularSusuAccount.client.branchID);
                var j = jb2.jnl.ToList();
                if (j.Count > 1)
                {
                    jb.jnl.Add(j[0]);
                    jb.jnl.Add(j[1]);
                }
                ent.jnl_batch.Add(jb);
            }
            sc.appliedToLoan = true;
            sc.posted = true;
            if (sc.regularSusuAccount.startDate == null)
            {
                sc.regularSusuAccount.startDate = sc.contributionDate;
            }
        }

        public  void PostSusuDisbursement(coreLoansEntities le, core_dbEntities ent, susuAccount sc,
            string userName)
        {
           
            var conf = le.susuConfigs.FirstOrDefault(); 
            sc.posted = true;
            var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 7);
            if (lt == null) lt = le.loanTypes.FirstOrDefault();
            var pro = ent.comp_prof.FirstOrDefault();
            var amountAtBank = sc.netAmountEntitled;
            var cr = le.cashiersTills.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.ToLower().Trim());
            int? acctID = null;

            if (cr != null) acctID = cr.accountID;
            else acctID = lt.vaultAccountID;

            var jb = journalextensions.Post("S/S", conf.contributionsPayableAccountID.Value,
                    acctID.Value, amountAtBank,
                    "Group Susu Disbursement Principal - " + amountAtBank.ToString("#,###.#0")
                    + " - " + sc.client.accountNumber + " - " + sc.client.surName + "," + sc.client.otherNames,
                    pro.currency_id.Value, sc.disbursementDate.Value, sc.susuAccountNo, ent, userName, sc.client.branchID);
            var jb2 = journalextensions.Post("S/S", conf.contributionsPayableAccountID.Value,
                            lt.unpaidCommissionAccountID, sc.commissionAmount,
                            "Group Susu Disbursement Fees- " + sc.client.surName + "," + sc.client.otherNames,
                            pro.currency_id.Value, sc.disbursementDate.Value, sc.susuAccountNo, ent, userName, sc.client.branchID);
            var list = jb2.jnl.ToList();
            jb.jnl.Add(list[0]);
            jb.jnl.Add(list[1]);

            jb2 = journalextensions.Post("S/S", conf.contributionsPayableAccountID.Value,
                  lt.unearnedInterestAccountID, sc.interestAmount,
                         "Group Susu Disbursement Interest- " + sc.client.surName + "," + sc.client.otherNames,
                         pro.currency_id.Value, sc.disbursementDate.Value, sc.susuAccountNo, ent, userName, sc.client.branchID);
            jb.jnl.Add(list[0]);
            jb.jnl.Add(list[1]);

            foreach (var c in sc.susuContributions.Where(p => p.appliedToLoan == false).OrderBy(p=> p.contributionDate))
            {
                var rem = c.amount; 
                foreach (var r in sc.susuContributionSchedules.Where(p=> p.balance>0).OrderBy(p => p.plannedContributionDate))
                {
                    var amt = sc.contributionAmount;
                    if (amt > rem)
                    {
                        amt = rem;
                    }
                    if (amt <= 0) break;
                    rem = rem - amt;
                    r.balance = r.balance - amt;
                    r.actualContributionDate = c.contributionDate;
                }

                var proc = sc.commissionAmount - sc.commissionPaid;
                var it = sc.interestAmount - sc.interestPaid;
                var pr = sc.amountEntitled - proc - it - sc.principalPaid; 
                var procAmt = (proc / c.susuAccount.amountEntitled) * c.amount;
                var itAmt = (it / c.susuAccount.amountEntitled) * c.amount;
                var prAmt = c.amount - itAmt - procAmt;

                if (procAmt > 0)
                {
                    var jb3 = journalextensions.Post("LN", acctID.Value,
                        lt.commissionAndFeesAccountID, procAmt,
                        "Group Susu Commission for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.susuAccountNo, ent, userName, sc.client.branchID);
                    coreLogic.jnl_batch jb4 = journalextensions.Post("LN", lt.unpaidCommissionAccountID,
                        conf.contributionsPayableAccountID.Value, procAmt,
                        "Group Susu Commission for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.susuAccountNo, ent, userName,
                    sc.client.branchID);
                    var j = jb4.jnl.ToList();
                    if (j.Count > 1)
                {
                        jb3.jnl.Add(j[0]);
                        jb3.jnl.Add(j[1]);
                }
                    ent.jnl_batch.Add(jb3);
                }
                if (itAmt + prAmt > 0)
                {
                    coreLogic.jnl_batch jb3 = journalextensions.Post("LN", lt.unearnedInterestAccountID,
                            lt.interestIncomeAccountID, itAmt,
                            "Group Susu Contribution for " + sc.client.surName + "," + sc.client.otherNames,
                            pro.currency_id.Value, c.contributionDate, sc.susuAccountNo, ent, userName,
                            sc.client.branchID);
                    coreLogic.jnl_batch jb4 = journalextensions.Post("LN",
                       acctID.Value, conf.contributionsPayableAccountID.Value, itAmt + prAmt,
                        "Group Susu Contribution for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.susuAccountNo, ent, userName,
                        sc.client.branchID);
                    var j = jb4.jnl.ToList();
                    if (j.Count > 1)
                    {
                        jb3.jnl.Add(j[0]);
                        jb3.jnl.Add(j[1]);
                    }
                    ent.jnl_batch.Add(jb3);
                }
                c.appliedToLoan = true;
            }
        }

        public void PostRegularSusuDisbursement(coreLoansEntities le, core_dbEntities ent, regularSusuAccount sc,
           string userName)
        {
            var conf = le.susuConfigs.FirstOrDefault();
            sc.posted = true;
            var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 8);
            if (lt == null) lt = le.loanTypes.FirstOrDefault();
            var pro = ent.comp_prof.FirstOrDefault();
            var amountAtBank = sc.netAmountEntitled;
            var cr = le.cashiersTills.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.ToLower().Trim());
            int? acctID = null;

            if (cr != null) acctID = cr.accountID;
            else acctID = lt.vaultAccountID;

            var jb = journalextensions.Post("S/S", conf.regularSusuContributionsPayableAccountID.Value,
                    acctID.Value, amountAtBank,
                    "Normal Susu Disbursement Principal - " + amountAtBank.ToString("#,###.#0")
                    + " - " + sc.client.accountNumber + " - " + sc.client.surName + "," + sc.client.otherNames,
                    pro.currency_id.Value, sc.disbursementDate.Value, sc.regularSusuAccountNo, ent, userName, sc.client.branchID);
            var jb2 = journalextensions.Post("S/S", conf.regularSusuContributionsPayableAccountID.Value,
                            lt.unpaidCommissionAccountID, sc.regularSusCommissionAmount,
                            "Normal Susu Disbursement Fees- " + sc.client.surName + "," + sc.client.otherNames,
                            pro.currency_id.Value, sc.disbursementDate.Value, sc.regularSusuAccountNo, ent, userName, sc.client.branchID);
            var list = jb2.jnl.ToList();
            jb.jnl.Add(list[0]);
            jb.jnl.Add(list[1]);

            jb2 = journalextensions.Post("S/S", conf.regularSusuContributionsPayableAccountID.Value,
                  lt.unearnedInterestAccountID, sc.interestAmount,
                         "Normal Susu Disbursement Interest- " + sc.client.surName + "," + sc.client.otherNames,
                         pro.currency_id.Value, sc.disbursementDate.Value, sc.regularSusuAccountNo, ent, userName, sc.client.branchID);
            jb.jnl.Add(list[0]);
            jb.jnl.Add(list[1]);

            foreach (var c in sc.regularSusuContributions.Where(p => p.appliedToLoan == false).OrderBy(p => p.contributionDate))
            {
                var rem = c.amount;
                foreach (var r in sc.regularSusuContributionSchedules.Where(p => p.balance > 0).OrderBy(p => p.plannedContributionDate))
                {
                    var amt = sc.contributionAmount;
                    if (amt > rem)
                    {
                        amt = rem;
                    }
                    if (amt <= 0) break;
                    rem = rem - amt;
                    r.balance = r.balance - amt;
                    r.actualContributionDate = c.contributionDate;
                }

                var proc = sc.regularSusCommissionAmount - sc.commissionPaid;
                var it = sc.interestAmount - sc.interestPaid;
                var pr = sc.amountEntitled - proc - it - sc.principalPaid;

                var procAmt = (proc / c.regularSusuAccount.amountEntitled) * c.amount;
                var itAmt = (it / c.regularSusuAccount.amountEntitled) * c.amount;
                var prAmt = c.amount - itAmt - procAmt;

                if (procAmt > 0)
                {
                    var jb3 = journalextensions.Post("LN", acctID.Value,
                        lt.commissionAndFeesAccountID, procAmt,
                        "Normal Susu Commission for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.regularSusuAccountNo, ent, userName, sc.client.branchID);
                    coreLogic.jnl_batch jb4 = journalextensions.Post("LN", lt.unpaidCommissionAccountID,
                        conf.regularSusuContributionsPayableAccountID.Value, procAmt,
                        "Normal Susu Commission for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.regularSusuAccountNo, ent, userName,
                    sc.client.branchID);
                    var j = jb4.jnl.ToList();
                    if (j.Count > 1)
                {
                        jb3.jnl.Add(j[0]);
                        jb3.jnl.Add(j[1]);
                }
                    ent.jnl_batch.Add(jb3);
                }
                if (itAmt + prAmt > 0)
                {
                    coreLogic.jnl_batch jb3 = journalextensions.Post("LN", lt.unearnedInterestAccountID,
                            lt.interestIncomeAccountID, itAmt,
                            "Normal Susu Contribution for " + sc.client.surName + "," + sc.client.otherNames,
                            pro.currency_id.Value, c.contributionDate, sc.regularSusuAccountNo, ent, userName,
                            sc.client.branchID);
                    coreLogic.jnl_batch jb4 = journalextensions.Post("LN", conf.regularSusuContributionsPayableAccountID.Value,
                       acctID.Value, itAmt + prAmt,
                        "Normal Susu Contribution for " + sc.client.surName + "," + sc.client.otherNames,
                        pro.currency_id.Value, c.contributionDate, sc.regularSusuAccountNo, ent, userName,
                        sc.client.branchID);
                    var j = jb4.jnl.ToList();
                    if (j.Count > 1)
                    {
                        jb3.jnl.Add(j[0]);
                        jb3.jnl.Add(j[1]);
                    }
                    ent.jnl_batch.Add(jb3);
                }
                c.appliedToLoan = true;
            }
        }

        public void ApplyNegativeBalanceToLoan(loan ln, coreLoansEntities le, core_dbEntities ent,
            double amountPaid, DateTime paymentDate, string userName, loan ln2)
        {
            var amount = -amountPaid;
            if (amount < 0) return;
            List<coreLogic.loanRepayment> list = new List<coreLogic.loanRepayment>();

            var iTotal = Math.Ceiling((ln.amountDisbursed) * (ln.interestRate / 100.0)
                                * (paymentDate - ln.disbursementDate.Value).Days / 30.0);
            var iPaid = ln.repaymentSchedules.Sum(p => p.interestPayment - p.interestBalance);
            var pBal = ln.repaymentSchedules.Sum(p => p.principalBalance);
            var iBal = ln.repaymentSchedules.Sum(p => p.interestBalance);
            var rem = pBal + (iTotal - iPaid);
            var pBal2 = ln.repaymentSchedules.Where(p => p.repaymentDate <= paymentDate).Sum(p => p.principalBalance);
            var iBal2 = ln.repaymentSchedules.Where(p => p.repaymentDate <= paymentDate).Sum(p => p.interestBalance);

            var sched = ln.repaymentSchedules.Where(p => p.interestBalance > 0 || p.principalBalance > 0).OrderBy(p => p.repaymentDate).ToList();
            var totalInt = ln.repaymentSchedules.Sum(p => p.interestPayment);
            var totalPrinc = ln.repaymentSchedules.Sum(p => p.principalPayment);
            var intRatio = totalInt / (totalInt + totalPrinc);
            var tinterest = 0.0;
            var tprinc = 0.0;
            var tprinc2 = 0.0;
            var tinterest2 = 0.0;
            if (iBal2 > 0)
            {
                tinterest = Math.Round(intRatio * amountPaid, 2);
                tprinc = amountPaid - tinterest;
                if (tinterest > iBal2)
                {
                    tinterest = iBal2;
                    tprinc = amountPaid - tinterest;
                }
                else if (tprinc > pBal)
                {
                    tprinc = pBal;
                    tinterest = amountPaid - tprinc;
                }
            }
            else if (amountPaid >= pBal)
            {
                tprinc = pBal;
                tinterest = amountPaid - tprinc;
            }
            else
            {
                tinterest = 0.0;
                tprinc = amountPaid - tinterest;
            }
            if (sched.Count > 0)
            {
                int i = 0;
                amount = tprinc;
                var amount2 = tinterest;
                while ((amount > 0 || amount2 > 0) && i < sched.Count)
                {
                    var s = sched[i];
                    var princ = 0.0;
                    var interest = 0.0;

                    if (amount >= s.principalBalance)
                    {
                        princ = s.principalBalance;
                    }
                    else
                    {
                        princ = amount;
                    }
                    if (amount2 >= s.interestBalance)
                    {
                        interest = s.interestBalance;
                    }
                    else
                    {
                        interest = amount2;
                    }
                    s.principalBalance -= princ;
                    s.interestBalance -= interest;

                    if (interest > s.additionalInterestBalance) s.additionalInterestBalance = 0;
                    else if (s.additionalInterestBalance != null) s.additionalInterestBalance -= interest;

                    tprinc2 += princ;
                    tinterest2 += interest;

                    i++;
                    amount = amount - princ;
                    amount2 = amount2 - interest;
                } 
            }
            ln2.loanRepayments.Add(new coreLogic.loanRepayment
           {
               amountPaid = amountPaid,
               creation_date = DateTime.Now,
               creator = userName,
               feePaid = 0,
               interestPaid = tinterest2,
               principalPaid = tprinc2,
               repaymentDate = paymentDate,
               modeOfPaymentID = 6,
               commission_paid = 0,
               repaymentTypeID = 1,
               checkNo = "",
               bankID = null,
               bankName = ""
           });
            ln.loanRepayments.Add(new coreLogic.loanRepayment
            {
                amountPaid = -amountPaid,
                creation_date = DateTime.Now,
                creator = userName,
                feePaid = 0,
                interestPaid = -tinterest2,
                principalPaid = -tprinc2,
                repaymentDate = paymentDate,
                modeOfPaymentID = 6,
                commission_paid = 0,
                repaymentTypeID = 1,
                checkNo = "",
                bankID = null,
                bankName = ""
            });
            ln.balance -= tprinc2; 
             
        }
    }
}
