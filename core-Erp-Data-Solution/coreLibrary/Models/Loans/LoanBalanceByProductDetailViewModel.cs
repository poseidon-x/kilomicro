using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Loans
{
    public class LoanBalanceByProductDetailViewModel
    {
        public int no { get; set; }
        public double amountDisbursed { get; set; }
        public double loans { get; set; }
        public double principal { get; set; }
        public double interest { get; set; }
        public double writeOff { get; set; }
        public double payable { get; set; }
        public double principalPayment { get; set; }
        public double interestPayment { get; set; }
        public double amountPaid { get; set; }
        public double oustanding { get; set; }
    }
}
