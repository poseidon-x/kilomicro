using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic.Models.CompanyProfile;

namespace coreLogic.Models.Loans
{
    public class DisbursedLoanViewModel
    {
        public string date { get; set; }
        public string repaymentType { get; set; }
        public double totalAmountDisbursed { get; set; }
        public double totalAmountPayable { get; set; }
        public int totalNumberOfPayments { get; set; }
        public double totalRepaymentScheduleAmount { get; set; }
        public double totalAmountpaid { get; set; }
        public int totalNumberOfPaymentsInArears { get; set; }
        public double totalRepaymentAmountInArears { get; set; }
        public double totalAmountInArears { get; set; }
        public double totalLoanTenure { get; set; }
        public IEnumerable<DisbursedLoanDetailViewModel> disbursedLoans { get; set; }
        public CompanyProfileViewModel company { get; set; }
    }
}
