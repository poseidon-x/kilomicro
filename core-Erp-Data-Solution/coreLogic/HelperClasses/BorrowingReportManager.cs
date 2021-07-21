using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using coreLogic.Models.Borrowing;

namespace coreLogic.HelperClasses
{
    public class BorrowingReportManager
    {
        private coreLoansEntities le;
        private core_dbEntities ctx;

        private borrowing brw { get; set; }
        private IEnumerable<borrowing> brws { get; set; }
        public string clientName { get; set; }

        public BorrowingReportManager(int borrowingId, coreLoansEntities le)
        {
            this.le = le;
            ctx = new core_dbEntities();
            brw = le.borrowings
                .Include(p => p.borrowingRepayments)
                .Include(p => p.borrowingFees)
                .Include(p => p.borrowingDisbursements)
                .FirstOrDefault(p => p.borrowingId == borrowingId);
        }

        public BorrowingReportManager(int clientId, int borrowingId, coreLoansEntities le)
        {
            this.le = le;
            ctx = new core_dbEntities();

            var client = le.clients.FirstOrDefault(p => p.clientID == clientId);
            clientName = client.surName + " " + client.otherNames;

            brws = le.borrowings
                .Include(p => p.borrowingRepayments)
                .Include(p => p.borrowingFees)
                .Include(p => p.borrowingDisbursements)
                .Where(p => p.clientId == clientId)
                .ToList();
        }

        public BorrowingReportManager(coreLoansEntities le)
        {
            this.le = le;
            ctx = new core_dbEntities();

           brws = le.borrowings
                .Include(p => p.borrowingRepayments)
                .Include(p => p.borrowingFees)
                .Include(p => p.borrowingDisbursements)
                .Where(p => p.amountDisbursed > 0 && p.disbursedDate != null && p.balance > 0)
                .ToList();
        }


        public IEnumerable<borrowing> getBorrowings()
        {
            return brws;
        }

        public string getBorrowingNo()
        {
            return brw.borrowingNo;
        }

        public string getclientName()
        {
            var client = le.clients
                .FirstOrDefault(p => p.clientID == brw.clientId);

            return client.surName + " " + client.otherNames;
        }

        public double getAmountDisbursed()
        {
            return brw.amountDisbursed;
        }

        public string getDisbursementDate()
        {
            return string.Format("{0: dd-MMM-yyyy}", brw.disbursedDate);
        }

        public string getExpiryDate()
        {
            var expiryDate = brw.disbursedDate.Value.AddMonths((int) brw.borrowingTenure);
            return string.Format("{0: dd-MMM-yyyy}", expiryDate);
        }
        
        public double getBorrowingInterest()
        {
            return brw.amountDisbursed * (brw.interestRate/100.0) * (30 / 365.0);
        }

        public double getTotalInterestPaid()
        {
            return brw.borrowingRepayments.Sum(p => p.interestPaid);
        }

        public double getTotalPrincipalPaid()
        {
            return brw.borrowingRepayments.Sum(p => p.principalPaid);
        }

        public double getTotalAmountPaid()
        {
            double totalInterestPaid = brw.borrowingRepayments.Sum(p => p.interestPaid);
            double totalPrincipalPaid = brw.borrowingRepayments.Sum(p => p.principalPaid);

            return totalInterestPaid+ totalPrincipalPaid;
        }

        public double getOutstandingInterest()
        {
            double totalInterest = brw.amountDisbursed * (brw.interestRate / 100.0) * (30 / 365.0);
            double totalInterestPaid = brw.borrowingRepayments.Sum(p => p.interestPaid);

            return totalInterest - totalInterestPaid;
        }

        public double getOustandingPrincipal()
        {
            double totalPrincipalPaid = brw.borrowingRepayments.Sum(p => p.principalPaid);

            return brw.amountDisbursed - totalPrincipalPaid;
        }

        public double getOutstandingTotal()
        {
            return brw.balance;
        }

        public IEnumerable<BorrowingJnEntriesViewModel> getBorrowingTransactions()
        {
            List<BorrowingJnEntriesViewModel> list = new List<BorrowingJnEntriesViewModel>();

            var brwDisbursement = brw.borrowingDisbursements.First();
            if(brwDisbursement != null) { 
                var disbursement = new BorrowingJnEntriesViewModel
                {
                    date = brwDisbursement.dateDisbursed,
                    desc = "Borrowing Disbursement",
                    drAmount = 0.00,
                    crAmount = brwDisbursement.amountDisbursed
                };
                list.Add(disbursement);
            }

            var brwFees = brw.borrowingFees.ToList();
            if (brwFees.Any())
            {
                foreach (var fee in brwFees)
                {
                    var disbursement = new BorrowingJnEntriesViewModel
                    {
                        date = fee.created,
                        //desc = fee.loanFeeType.feeTypeName,
                        drAmount = 0.00,
                        crAmount = fee.feeAmount
                    };
                    disbursement.desc = le.loanFeeTypes.FirstOrDefault(p => p.feeTypeID == fee.feeTypeId).feeTypeName;
                    list.Add(disbursement);
                }             
            }


            IEnumerable<BorrowingJnEntriesViewModel> transc = list;

            int num = 1;
            foreach (var record in transc)
            {
                record.no = num;
                record.dateString = string.Format("{0: dd-MMM-yyyy}", record.date);
                num++;
            }

            return transc;
        }

    }
}
