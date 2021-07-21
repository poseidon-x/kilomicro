using coreLogic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace coreLogic
{
    public class RepaymentAmountDistributor
    {
        private IcoreLoansEntities le;

        public RepaymentAmountDistributor()
        {
            le = new coreLoansEntities();
        }

        public RepaymentAmountDistributor(IcoreLoansEntities ctx)
        {
            le = ctx;
        }

        public loanRepayment receiveRepayment( int loanId,
            double amountPaid, DateTime paymentDate, string paymentTypeID)
        {
            loanRepayment lr = new loanRepayment();
            var ln = le.loans
                .Include(p => p.repaymentSchedules)
                .Include(p => p.loanRepayments)
                .FirstOrDefault(p => p.loanID == loanId);
            switch (paymentTypeID)
            {
                case "1":
                    lr = recieveInterestPlusPrincipal(ln, amountPaid, paymentDate);
                    break;
                case "2":
                    lr = recievePrincipalOnly(ln, amountPaid, paymentDate);
                    break;
                case "3":
                    lr = recieveInterestOnly(ln, amountPaid, paymentDate);
                    break;
            }

            return lr;
        }


        public loanRepayment recieveInterestPlusPrincipal(loan ln,
            double amountPaid, DateTime paymentDate)
        {
            var bal = computeBalance(ln, paymentDate);
            double interestToBePaid = (amountPaid* (bal.totalInterestExpected/(bal.totalInterestExpected + ln.amountDisbursed)));
            double principalToBePaid = amountPaid - interestToBePaid;
            double penaltyToBePaid = 0;
            var prof = (new core_dbEntities()).comp_prof.FirstOrDefault();


            if (prof.comp_name.ToLower().Contains(AppContants.Lendzee) || prof.comp_name.ToLower().Contains(AppContants.Kilo))
            {
                interestToBePaid = Math.Round(interestToBePaid, 4);
                principalToBePaid = Math.Round(principalToBePaid, 4);
                bal.totalInterestBalace = Math.Round(bal.totalInterestBalace, 4);
                bal.totalPrincipalBalance = Math.Round(bal.totalPrincipalBalance, 4);
            }
            else
            {
                interestToBePaid = Math.Round(interestToBePaid, 0);
                principalToBePaid = Math.Round(principalToBePaid, 0);
                bal.totalInterestBalace = Math.Round(bal.totalInterestBalace, 0);
                bal.totalPrincipalBalance = Math.Round(bal.totalPrincipalBalance, 0);
            }

            if ((interestToBePaid + principalToBePaid) > (bal.totalPrincipalBalance + bal.totalInterestBalace))
            {
                throw new ApplicationException("Sorry amount paid is more than interest and principal balance for loan No.: " + ln.loanNo + 
                    "Over payment of " + ((interestToBePaid + principalToBePaid)-(bal.totalPrincipalBalance + bal.totalInterestBalace)).ToString("#,##0.#0"));
            }

            if (interestToBePaid > bal.totalInterestBalace)
            {
                interestToBePaid = bal.totalInterestBalace;
                principalToBePaid = amountPaid - interestToBePaid;
            }
            else if (interestToBePaid>bal.interestDue && principalToBePaid < bal.totalPrincipalBalance)
            {
                interestToBePaid = bal.interestDue;
                principalToBePaid = amountPaid - interestToBePaid;
            }

            if (principalToBePaid > bal.totalPrincipalBalance)
            {
                principalToBePaid = bal.totalPrincipalBalance;
                interestToBePaid = amountPaid - principalToBePaid;
                if (amountPaid > (bal.totalPrincipalBalance+bal.totalInterestBalace))
                {
                    principalToBePaid = bal.totalPrincipalBalance;
                    interestToBePaid = bal.totalInterestBalace;
                    penaltyToBePaid = amountPaid - (principalToBePaid + interestToBePaid);
                }
            }

            if (prof.comp_name.ToLower().Contains(AppContants.Lendzee) || prof.comp_name.ToLower().Contains(AppContants.Kilo))
            {
                loanRepayment lr = new loanRepayment
                {
                    loanID = ln.loanID,
                    amountPaid = amountPaid,
                    interestPaid = Math.Round(interestToBePaid, 4),
                    principalPaid = Math.Round(principalToBePaid, 4),
                    penaltyPaid = Math.Round(penaltyToBePaid, 4)
                };
                return lr;
            }
            else
            {
                loanRepayment lr = new loanRepayment
                {
                    loanID = ln.loanID,
                    amountPaid = amountPaid,
                    interestPaid = Math.Round(interestToBePaid, 0),
                    principalPaid = Math.Round(principalToBePaid, 0),
                    penaltyPaid = Math.Round(penaltyToBePaid, 0)
                };
                return lr;
            }

            
        }

        public loanRepayment recievePrincipalOnly(loan ln,
            double amountPaid, DateTime paymentDate)
        {
            var bal = computeBalance(ln, paymentDate);
            double principalToBePaid = amountPaid;

            if (principalToBePaid > bal.totalPrincipalBalance)
            {
                throw new ApplicationException("Sorry principal payment is more than principal balance for loan No.: " + ln.loanNo);
            }
            var prof = (new core_dbEntities()).comp_prof.FirstOrDefault();

            if (prof.comp_name.ToLower().Contains(AppContants.Lendzee) || prof.comp_name.ToLower().Contains(AppContants.Kilo))
            {
                loanRepayment lr = new loanRepayment
                {
                    loanID = ln.loanID,
                    amountPaid = amountPaid,
                    interestPaid = 0,
                    principalPaid = Math.Round(principalToBePaid, 4),
                    penaltyPaid = 0
                };
                return lr;
            }
            else
            {
                loanRepayment lr = new loanRepayment
                {
                    loanID = ln.loanID,
                    amountPaid = amountPaid,
                    interestPaid = 0,
                    principalPaid = Math.Round(principalToBePaid, 0),
                    penaltyPaid = 0
                };
                return lr;
            }
            
            
        }

        public loanRepayment recieveInterestOnly(loan ln,
            double amountPaid, DateTime paymentDate)
        {
            var bal = computeBalance(ln, paymentDate);
            double interestToBePaid = amountPaid;
            var prof = (new core_dbEntities()).comp_prof.FirstOrDefault();

            if (interestToBePaid > bal.totalInterestBalace)
            {
                throw new ApplicationException("Sorry interest payment is more than interest balance for loan No.: "+ln.loanNo);
            }


            if (prof.comp_name.ToLower().Contains(AppContants.Lendzee) || prof.comp_name.ToLower().Contains(AppContants.Kilo))
            {
                loanRepayment lr = new loanRepayment
                {
                    loanID = ln.loanID,
                    amountPaid = amountPaid,
                    interestPaid = Math.Round(interestToBePaid, 4),
                    principalPaid = 0,
                    penaltyPaid = 0
                };
                return lr;
            }
            else 
            {
                loanRepayment lr = new loanRepayment
                {
                    loanID = ln.loanID,
                    amountPaid = amountPaid,
                    interestPaid = Math.Round(interestToBePaid, 0),
                    principalPaid = 0,
                    penaltyPaid = 0
                };
                return lr;
            }

            
        }

        public balanceModule computeBalance(loan ln, DateTime paymentDate)
        {
            double totalInterestExpected = ln.repaymentSchedules.Sum(p => p.interestPayment);
            double totalInterestPaid = ln.loanRepayments.Any()? ln.loanRepayments.Sum(p => p.interestPaid):0;
            double totalInterestBalance = totalInterestExpected - totalInterestPaid;

            double totalPrincipalPaid = ln.loanRepayments.Any() ? ln.loanRepayments.Sum(p => p.principalPaid) : 0;
            double totalPrincipalBalance = ln.amountDisbursed - totalPrincipalPaid;

            double interestExpectTiilNowDue =
                ln.repaymentSchedules.Where(p => p.repaymentDate.Date <= paymentDate.Date).Sum(p => p.interestPayment);

            double interestDue = interestExpectTiilNowDue - totalInterestPaid;
            interestDue = interestDue < 0 ? 0 : interestDue;

            balanceModule balance = new balanceModule
            {
                totalInterestExpected = totalInterestExpected,
                totalInterestBalace = totalInterestBalance,
                totalPrincipalBalance = totalPrincipalBalance,
                interestDue = interestDue
            };

            return balance;
        }
    }

    public class balanceModule
    {
        public double totalInterestExpected { get; set; }
        public double totalInterestBalace { get; set; }
        public double totalPrincipalBalance { get; set; }
        public double totalPenaltyBalance { get; set; }
        public double interestDue { get; set; }
    }
}
