using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic.Models.CompanyProfile;

namespace coreLogic.Models.Loans
{
    public class LoanBalanceByProductViewModel
    {
        public double totalAmountDisbursed { get; set; }
        public double totalLoans { get; set; }
        public double totalPrincipal { get; set; }
        public double totalInterest { get; set; }
        public double totalWriteOff { get; set; }
        public double totalPayable { get; set; }
        public double totalPrincipalPayment { get; set; }
        public double totalInterestPayment { get; set; }
        public double totalAmountPaid { get; set; }
        public double totalOustanding { get; set; }
        public CompanyProfileViewModel company { get; set; }
        private IEnumerable<LoanBalanceByProductDetailViewModel> details { get; set; }
    }
}
