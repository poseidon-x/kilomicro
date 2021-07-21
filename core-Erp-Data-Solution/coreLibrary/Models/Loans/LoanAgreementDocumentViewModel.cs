using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Loans
{
    public class LoanAgreementDocumentViewModel
    {
        public string companyId { get; set; }
        public string companyName { get; set; }
        public string companyAddress { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; }
        public string clientResidentialAddress { get; set; }
        public string clientMailingAddress { get; set; }
        public string clientLocation { get; set; }
        public int loanId { get; set; }
        public double loanPrincipalAmount { get; set; }
        public string loanPrincipalAmountInWords { get; set; }
        public List<string> loanCollateralTypeNames { get; set; }
        public string loanCurrencyName { get; set; }
        public string loanFacilityType { get; set; }
        public double loanPenaltyRate { get; set; }
        public DateTime loanDisbursementDate{ get; set; }
        public double loanTenure { get; set; }
        public string loanTenorInWords { get; set; }
        public string loanPurpose { get; set; }
        public string loanRepaymentMode { get; set; }
        public string loanInterestRate { get; set; }
        public string loanProcessingFeeInWords { get; set; }
        public double loanApplicationFee { get; set; }
        public string loanApplicationFeeInWords { get; set; }
        public string loanInsuranceFeeInWords { get; set; }
        public double loanMaturityValue { get; set; }
        public string loanMaturityValueInWords { get; set; }
        public DateTime loanMaturityDate { get; set; }
    }
}
