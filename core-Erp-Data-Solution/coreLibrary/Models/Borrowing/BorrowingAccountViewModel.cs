using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Borrowing
{
    public class BorrowingAccountViewModel
    {
        public int clientId { get; set; }
        public string clientName { get; set; }
        public int borrowingId { get; set; }
        public string borrowingNumber { get; set; }
        public double amountDisbursed { get; set; }
        public string disbursementDate { get; set; }
        public string expiryDate { get; set; }
        public double borrowingInterest { get; set; }
        public double totalAmountPaid { get; set; }
        public double totalInterestPaid { get; set; }
        public double totalPrincipalPaid { get; set; }
        public double outstandingInterest { get; set; }
        public double outstandingPrincipal { get; set; }
        public double outstandingTotal { get; set; }

        public IEnumerable<BorrowingJnEntriesViewModel> JnlEntries { get; set; }
    }
}
