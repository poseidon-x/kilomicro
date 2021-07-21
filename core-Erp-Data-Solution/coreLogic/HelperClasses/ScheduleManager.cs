using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.VisualBasic;
using coreLogic.Models;

namespace coreLogic
{
    public class ScheduleManager : IScheduleManager
    {
        IJournalExtensions journalextensions = new JournalExtensions();

        public List<repaymentSchedule> calculateSchedule(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, int repaymentModeID, client cl)
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
                        tenure, interestTypeID, cl);
                    break;
                case -1:
                    sched = calculateScheduleOneOff(amount, rate, loanDate, gracePeriod,
                        tenure, interestTypeID);
                    break;
            }

            return sched;
        }

        public List<repaymentSchedule> calculateScheduleTTL(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, int repaymentModeID, loan ln)
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
                    if (ln.loanTypeID == 10)
                    {
                        sched = calculateScheduleWeeklyTrustLine(amount, rate, loanDate, gracePeriod,
                            tenure, interestTypeID, repaymentModeID, ln);
                    }
                    else
                    {
                        client cl = null;
                        sched = calculateScheduleWeekly(amount, rate, loanDate, gracePeriod,
                            tenure, interestTypeID, cl);
                    }
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

            var le = new coreLoansEntities();
            var cfg = le.loanConfigs.FirstOrDefault();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
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
                var comp = (new core_dbEntities()).comp_prof.First().comp_name;
                do
                {
                    if (interestTypeID == 4)
                    {
                        if (comp.ToLower().Contains("link") || comp.ToLower().Contains("jireh"))
                            interest = Math.Round(amount * (rate / 100), 0);
                        else
                            interest = Math.Round(amount * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    else if (interestTypeID == 3)
                    {
                        if (comp.ToLower().Contains("link") || comp.ToLower().Contains("jireh"))
                            interest = Math.Round(runningPrinc * (rate / 100), 0);
                        else
                            interest = Math.Round(runningPrinc * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    if (interestTypeID == 2 || interestTypeID == 6 || interestTypeID == 7)
                    {
                        if (comp.ToLower().Contains("link") || comp.ToLower().Contains("jireh"))
                            interest = Math.Round(runningPrinc * (rate / 100), 0);
                        else
                            interest = Math.Round(runningPrinc * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    else if (interestTypeID == 1)
                    {
                        if (comp.ToLower().Contains("link") || comp.ToLower().Contains("jireh"))
                            interest = Math.Round(amount * (rate / 100), 0);
                        else
                            interest = Math.Round(amount * (rate / 100), 2);
                        runningPrinc -= princ;
                    }
                    princRem += princR;

                    coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                    s.interestPayment = Math.Round(interest, 4);
                    s.principalPayment = (interestTypeID == 4) ? 0 : Math.Round(princ, 4);

                    if ((cfg != null && cfg.automaticInterestCalculation == true && cfg.penaltyScheme == 30)
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
                if (sch != null && interestTypeID == 4)
                {
                    sch.principalPayment += amount;
                    sch.principalBalance += amount;
                }
                for (int i = 0; i < sched.Count && interestTypeID != 4; i++)
                {
                    if (princRem >= 0.5 && (new core_dbEntities()).comp_prof.First().comp_name.Contains("Link"))
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

        public List<repaymentSchedule> calculateScheduleM(
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
                    s.interestPayment = md2 - princ;
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

        private List<repaymentSchedule> calculateScheduleDaily(
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
            DateTime date2 = loanDate;

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

        private List<repaymentSchedule> calculateScheduleFortnightly(
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


        private List<repaymentSchedule> calculateScheduleWeekly(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, client cl)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            core_dbEntities ent = new core_dbEntities();
            var prof = ent.comp_prof.FirstOrDefault();
            if (prof.traditionalLoanNo == false)
            {

                if (prof.comp_name.ToLower().Contains(AppContants.Lendzee) || prof.comp_name.ToLower().Contains(AppContants.Kilo))
                {
                    if (cl != null)
                    {
                        var group = cl.loanGroupClients.FirstOrDefault();
                        if (group != null)
                        {
                            var repaymentDayId = group.loanGroup.loanGroupDayId;
                            var differencialToGroupPaymentDate = repaymentDayId - (int)loanDate.DayOfWeek;
                            date = date.AddDays(differencialToGroupPaymentDate).AddDays(7);

                        }

                    }
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
                else
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
            var straightLinePrinAmt = amount / dates.Count;
            var straightLineIntAmt = amount * (rate / 100) * tenure / dates.Count;
            var straightLineRunninPrinc = amount;


            foreach (var d in dates)
            {
                var diffDays = (d - date).Days;
                if ((d - date2).Days >= 30)
                {
                    runningPrinc2 = runningPrinc;
                    date2 = d;
                }
                coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                if (interestTypeID == 1)
                {
                    s.interestPayment = Math.Round(straightLineIntAmt, 4);
                    s.principalPayment = Math.Round(straightLinePrinAmt, 4);
                    s.interestBalance = Math.Round(straightLineIntAmt, 4);
                    s.principalBalance = Math.Round(straightLinePrinAmt, 4);
                    s.origInterestPayment = Math.Round(straightLineIntAmt, 4);
                    straightLineRunninPrinc -= straightLinePrinAmt;
                    s.balanceCD = Math.Round(straightLineRunninPrinc, 4);
                    s.origPrincipalBF = Math.Round(straightLineRunninPrinc, 4) + Math.Round(straightLinePrinAmt, 4);
                    s.origPrincipalCD = Math.Round(straightLineRunninPrinc, 4);
                }
                else
                {
                    interest = Math.Round(runningPrinc2 * (rate / 100)
                        * (diffDays / 30.0), 2);
                    intR = interest - Math.Floor(interest);
                    interest = Math.Floor(interest);
                    s.interestPayment = Math.Round(interest, 4);
                    s.principalPayment = Math.Round(princ, 4);
                    s.interestBalance = Math.Round(interest, 4);
                    s.principalBalance = Math.Round(princ, 4);
                    s.origInterestPayment = Math.Round(interest, 4);
                    runningPrinc -= princ;
                    s.balanceCD = Math.Round(runningPrinc, 4);
                    s.origPrincipalBF = Math.Round(runningPrinc, 4) + Math.Round(princ, 4);
                    s.origPrincipalCD = Math.Round(runningPrinc, 4);
                }
                princRem += princR;
                intRem += intR;


                s.repaymentDate = d;

                s.additionalInterest = 0;
                s.penaltyAmount = 0;
                s.additionalInterestBalance = 0;

                sched.Add(s);

                date = d;
            }
            if (!prof.comp_name.ToLower().Contains(AppContants.Lendzee) && !prof.comp_name.ToLower().Contains(AppContants.Kilo))
            {
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
            }

            return sched;
        }

        private List<repaymentSchedule> calculateScheduleOneOff(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;

            comp_prof comp;
            using (core_dbEntities le = new core_dbEntities())
            {
                comp = le.comp_prof.FirstOrDefault();
            }


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

            if (comp.comp_name.ToLower().Contains("jireh"))
            {
                if (tenure == 0)
                {
                    interest = 0;
                    princ = runningPrinc;
                }
                else
                {
                    interest = Math.Round(runningPrinc * (rate / 100), 0);
                    if (interestTypeID == 2)
                    {
                        princ = pmt - interest;
                    }
                }

                //Monthly interest repayment
                var tenur = (int)tenure;
                if (tenur > 1)
                {
                    var schedDate = loanDate;
                    for (int i = 0; i < (tenure); i++)
                    {
                        coreLogic.repaymentSchedule s = new coreLogic.repaymentSchedule();
                        s.interestPayment = Math.Round(interest, 4);
                        s.principalPayment = 0;
                        s.repaymentDate = schedDate;
                        runningPrinc = Math.Round(princ, 4);
                        s.balanceCD = Math.Round(runningPrinc, 4);
                        s.interestBalance = Math.Round(interest, 4);
                        s.principalBalance = 0;

                        s.origPrincipalBF = 0;
                        s.origPrincipalCD = Math.Round(princ, 4);
                        s.origInterestPayment = Math.Round(interest, 4);
                        s.additionalInterest = 0;
                        s.penaltyAmount = 0;
                        s.additionalInterestBalance = 0;

                        schedDate = schedDate.AddMonths(1);
                        sched.Add(s);
                        if (i == tenur - 1)
                        {
                            s.principalPayment = amount;
                            s.principalBalance = amount;
                            s.origPrincipalPayment = amount;
                        }
                    }
                }
            }
            //For all other MFI
            else
            {
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
            }

            return sched;

        }

        public List<repaymentSchedule> calculateScheduleSusu(susuAccount account, string userName)
        {
            List<coreLogic.repaymentSchedule> sched = new List<coreLogic.repaymentSchedule>();
            var tint = account.interestAmount;
            if (tint < 0) tint = 0;
            var bal = account.amountEntitled - account.commissionAmount - account.interestAmount;
            foreach (var sc in account.susuContributionSchedules.OrderBy(p => p.plannedContributionDate))
            {
                var interest = sc.amount;
                if (tint > 0 && tint < interest)
                {
                    interest = tint;
                }
                else if (tint == 0)
                {
                    interest = 0;
                }

                tint = tint - interest;
                var interestBal = interest;
                var princ = sc.amount - interest;
                var princBal = princ;
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
                    origPrincipalCD = bal - princ,
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

        public List<repaymentSchedule> calculateSchedule(
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

        private List<repaymentSchedule> calculateScheduleMonthly(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, List<repaymentSchedule> oldSched)
        {
            List<coreLogic.repaymentSchedule> sched = oldSched;
            DateTime date = loanDate;
            if (gracePeriod != null) date = date.AddDays(gracePeriod.Value);
            double interest = 0;
            double princ = Math.Floor(amount / (sched.Where(p => p.repaymentDate > loanDate).Count()));
            double runningPrinc = princ * (sched.Where(p => p.repaymentDate >= loanDate).Count());
            double princR = (amount / (sched.Where(p => p.repaymentDate >= loanDate).Count())
                - Math.Floor(amount / (sched.Where(p => p.repaymentDate >= loanDate).Count())));
            double princRem = 0;
            double pmt = -Microsoft.VisualBasic.Financial.Pmt(rate / 100, tenure, runningPrinc, 0);
            bool second = false;
            var totalInterest = Math.Round(runningPrinc * tenure * (rate / 100), 0);
            var runningInterest = 0.0;

            repaymentSchedule sch = null;
            foreach (var r in sched)
            {
                var diff = (r.repaymentDate - loanDate).Days;
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
            for (int i = 0; i < sched.Count && interestTypeID != 4; i++)
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

        private List<repaymentSchedule> calculateScheduleDaily(
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

        //Calculate Schedule GFI
        public List<repaymentSchedule> calculateScheduleDailyGFI(
                    double amount, double rate, DateTime loanDate, int? gracePeriod,
                    double tenure, int interestTypeID, int repaymentModeID, int loanProductId)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            double totalInterest = amount * (rate / 100.0);
            double runningPrinc = amount;
            DateTime startDate = loanDate.AddDays(2);
            date = startDate;
            List<coreLogic.repaymentSchedule> sched = new List<repaymentSchedule>();

            if (loanProductId == 1)
            {

                if (loanDate.DayOfWeek == DayOfWeek.Thursday)
                {
                    date = loanDate.AddDays(4);
                }
                else if (loanDate.DayOfWeek == DayOfWeek.Friday)
                {
                    date = loanDate.AddDays(3);
                }

                double dailyPrincipal = amount / 50;
                double dailyInterest = totalInterest / 50;


                int addedItems = 0;
                while (addedItems < 50)
                {
                    while (isSpecialDay(date))
                    {
                        date = date.AddDays(1);
                    }

                    sched.Add(new coreLogic.repaymentSchedule
                    {
                        repaymentDate = date,
                        principalPayment = dailyPrincipal,
                        interestPayment = dailyInterest,
                        principalBalance = dailyPrincipal,
                        interestBalance = dailyInterest,
                        additionalInterest = 0,
                        additionalInterestBalance = 0,
                        origInterestPayment = dailyInterest,
                        origPrincipalBF = runningPrinc,
                        balanceBF = runningPrinc,
                        creation_date = DateTime.Now,
                        creator = "System",
                        balanceCD = runningPrinc - dailyPrincipal,
                        penaltyAmount = 0,
                        proposedInterestWriteOff = 0,
                        interestWritenOff = 0,
                        edited = false,
                        origPrincipalCD = runningPrinc - dailyPrincipal,
                        origPrincipalPayment = dailyPrincipal
                    });
                    runningPrinc = runningPrinc - dailyPrincipal;
                    date = date.AddDays(1);
                    addedItems += 1;
                }
            }
            else if (loanProductId == 2)
            {
                double thisWeekPrincipal = 0.0;
                double thisWeekInterest = 0.0;
                int i = 0;


                if (loanDate.DayOfWeek == DayOfWeek.Thursday)
                {
                    date = loanDate.AddDays(4);
                }
                else if (loanDate.DayOfWeek == DayOfWeek.Friday)
                {
                    date = loanDate.AddDays(3);
                }

                double dailyPrincipal = amount / 10;
                double dailyInterest = totalInterest / 10;
                int addedItems = 0;

                //Do n tinmes
                while (addedItems < 10)
                {
                    //Skip holidays
                    while (isSpecialDay(date))
                    {
                        date = date.AddDays(1);
                    }

                    //Accrue repayments
                    thisWeekPrincipal += dailyPrincipal;
                    thisWeekInterest += dailyInterest;

                    sched.Add(new coreLogic.repaymentSchedule
                    {
                        repaymentDate = date,
                        principalPayment = dailyPrincipal,
                        interestPayment = dailyInterest,
                        principalBalance = thisWeekPrincipal,
                        interestBalance = thisWeekInterest,
                        additionalInterest = 0,
                        additionalInterestBalance = 0,
                        origInterestPayment = thisWeekInterest,
                        origPrincipalBF = runningPrinc,
                        balanceBF = runningPrinc,
                        creation_date = DateTime.Now,
                        creator = "System",
                        balanceCD = runningPrinc - thisWeekPrincipal,
                        penaltyAmount = 0,
                        proposedInterestWriteOff = 0,
                        interestWritenOff = 0,
                        edited = false,
                        origPrincipalCD = runningPrinc - thisWeekPrincipal,
                        origPrincipalPayment = thisWeekPrincipal
                    });
                    runningPrinc = runningPrinc - thisWeekPrincipal;
                    thisWeekPrincipal = 0;
                    thisWeekInterest = 0;

                    addedItems += 1;
                }
                date = date.AddDays(7);
            }
            return sched;
        }

        //Calculate Schedule Trust line
        public List<repaymentSchedule> calculateScheduleWeeklyTrustLine(
                    double amount, double rate, DateTime loanDate, int? gracePeriod,
                    double tenure, int interestTypeID, int repaymentModeID, loan ln)
        {
            var le = new core_dbEntities();
            List<DateTime> dates = new List<DateTime>();
            DateTime date = loanDate;
            double totalInterest = amount * ((rate / 100.0) * (int)tenure);
            double runningPrinc = amount;

            var groupDay = getClientGroupDay(ln);

            DateTime startDate = getFirstRepaymentDate(groupDay, date); ;
            date = startDate;
            List<coreLogic.repaymentSchedule> sched = new List<repaymentSchedule>();
            var compName = le.comp_prof.First().comp_name.ToLower();
            if (ln.loanTypeID == 10 && (compName.Contains(AppContants.Lendzee) || compName.Contains(AppContants.Kilo)))
            {
                int totalSchedule = (int)(tenure * 4);
                double weeklyPrincipal = amount / totalSchedule;
                double weeklyInterest = totalInterest / totalSchedule;
                int addedItems = 0;

                double trialingPrinDif = 0.00;
                double trialingInterDif = 0.00;
                double negPrinc = amount - (2 * amount);

                double thisWeekPrincipal = 0.00;
                double thisWeekInterest = 0.00;
                double remainingPrin = amount;
                double remainingIntrst = totalInterest;
                double thisWkIntrst = 0;
                double thisWkprinc = 0;

                //Do n tinmes
                while (addedItems < (tenure * 4))
                {
                    //Skip holidays
                    while (isSpecialDay(date))
                    {
                        date = date.AddDays(7);
                    }
                    thisWeekPrincipal = weeklyPrincipal;
                    thisWeekInterest = weeklyInterest;

                    remainingPrin -= thisWeekPrincipal;
                    remainingIntrst -= thisWeekInterest;

                    sched.Add(new coreLogic.repaymentSchedule
                    {
                        repaymentDate = date,
                        principalPayment = thisWeekPrincipal,
                        interestPayment = thisWeekInterest,
                        principalBalance = remainingPrin,
                        interestBalance = remainingIntrst,
                        additionalInterest = 0,
                        additionalInterestBalance = 0,
                        origInterestPayment = thisWeekPrincipal,
                        origPrincipalBF = thisWeekInterest,
                        balanceBF = remainingPrin,
                        creation_date = DateTime.Now,
                        creator = "System",
                        balanceCD = remainingPrin,
                        penaltyAmount = 0,
                        proposedInterestWriteOff = 0,
                        interestWritenOff = 0,
                        edited = false,
                        origPrincipalCD = remainingPrin,
                        origPrincipalPayment = thisWeekPrincipal
                    });

                    addedItems += 1;
                    date = date.AddDays(7);
                }
                //if (sched.Any())
                //{
                //    var firstSchd = sched.FirstOrDefault();
                //    trialingPrinDif = Math.Round(trialingPrinDif, 0);
                //    trialingInterDif = Math.Round(trialingInterDif, 0);
                //    firstSchd.principalPayment += trialingPrinDif;
                //    firstSchd.interestPayment += trialingInterDif;
                //    firstSchd.principalBalance += trialingPrinDif;
                //    firstSchd.interestBalance += trialingInterDif;
                //}
            }

            return sched;
        }

        private List<repaymentSchedule> calculateScheduleFortnightly(
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

        //Implement Special day check
        private bool isSpecialDay(DateTime date)
        {
            List<SpecialDay> specialDays = new coreLoansEntities().SpecialDays.ToList();
            //check for day of week
            var day = ((int)date.DayOfWeek).ToString();

            if (specialDays.Any(p => p.specialDayTypeId == 1 && p.specialDayValue == day))
            {
                return true;
            }

            //check for day of month
            string dateStringMonth = date.ToString("dd-MMM");
            if (specialDays.Any(p => p.specialDayTypeId == 2 && p.specialDayValue == dateStringMonth))
            {
                return true;
            }

            //check for day of year
            string dateStringYear = date.ToString("dd-MMM-yyyy");
            if (specialDays.Any(p => p.specialDayTypeId == 3 && p.specialDayValue == dateStringYear))
            {
                return true;
            }

            return false;
        }

        private bool islastDayOfWeek(DateTime date)
        {
            int dayOfWeek = (int)date.DayOfWeek;
            int diffFridays = 6 - dayOfWeek;

            if (diffFridays < 0)
            {
                diffFridays = 6;
            }
            DateTime fridayDate = date.AddDays(diffFridays);
            for (DateTime dt = fridayDate; dt >= date; dt = dt.AddDays(-1))
            {
                if ((isSpecialDay(dt) == false) && (dt != date))
                {
                    return false;
                }
            }
            return true;
        }

        private bool islastDueDate(DateTime date, DayOfWeek dowPayment)
        {
            int dayOfWeek = (int)date.DayOfWeek;
            int diffFridays = ((int)dowPayment) - dayOfWeek;

            if (diffFridays < 0)
            {
                diffFridays = ((int)dowPayment);
            }
            DateTime fridayDate = date.AddDays(diffFridays);
            for (DateTime dt = fridayDate; dt >= date; dt = dt.AddDays(-1))
            {
                if ((isSpecialDay(dt) == false) && (dt != date))
                {
                    return false;
                }
            }
            return true;
        }

        private List<repaymentSchedule> calculateScheduleWeekly(
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

        private List<repaymentSchedule> calculateScheduleOneOff(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, List<repaymentSchedule> oldSched)
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


        // function to retrieve first day of the week
        private DateTime getFirstRepaymentDate(int userGroup, DateTime date)
        {
            DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int offset = fdow - date.DayOfWeek;
            DateTime fdowDate = date.AddDays(offset);
            //return fdowDate;

            return fdowDate.AddDays(userGroup + 14);


        }

        private int getClientGroupDay(loan ln)
        {
            using (coreLoansEntities le = new coreLoansEntities())
            {
                //var client = le.clients.FirstOrDefault(p => p.clientID == ln.clientID);
                var clientGroupId = le.loanGroupClients.FirstOrDefault(p => p.clientId == ln.clientID).loanGroupId;
                return le.loanGroups.FirstOrDefault(p => p.loanGroupId == clientGroupId).loanGroupDayId;
            }

        }





    }
}
