using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Loans
{
    public class DisbursedLoanDetailViewModel
    {
        public int no { get; set; }
        public int clientId { get; set; }
        public int loanId { get; set; }
        public string loanNumber { get; set; }
        public string clientName { get; set; }
        public double amountDisbursed { get; set; }
        public DateTime? date { get; set; }
        public string disbursementDate { get; set; }
        public double amountPayable { get; set; }
        public double amountPaid { get; set; }
        public double amountInArears { get; set; }
        public int numberOfPayments { get; set; }
        public int totalRepaymentDays { get; set; }
        public double repaymentScheduleAmount { get; set; }
        public int numberOfPaymentsInArears { get; set; }
        public double repaymentAmountInArears { get; set; }

        public double loanTenure { get; set; }
    }
}
