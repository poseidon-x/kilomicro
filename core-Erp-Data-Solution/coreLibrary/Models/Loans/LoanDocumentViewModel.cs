using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic.Models.CompanyProfile;


namespace coreLogic.Models.Loans
{
    public class LoanDocumentViewModel
    {
        public byte[] companyLogo { get; set; }
        public int loanDocumentTemplateId { get; set; }
        public string templateName { get; set; }
        public string lendingCompanyName { get; set; }
        public string lendingCompanyCeo { get; set; }
        public string lendingCompanyOperManager { get; set; }
        public string lendingCompanyOfficeAddress { get; set; }
        public string lendingCompanyPostalAddress { get; set; }
        public string lendingCompanyRegion { get; set; }
        public string clientWorkRegion { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; }
        public string clientOccupation { get; set; }
        public string clientOfficeLocation { get; set; }
        public string clientRegion { get; set; }
        public string clientResidentialAddress { get; set; }
        public string clientPostalAddress { get; set; }
        public string clientLocation { get; set; }
        public int loanId { get; set; }
        public string loanNumber { get; set; }
        public string loanProduct { get; set; }
        public string loanTenure { get; set; }
        public string repaymentMode { get; set; }
        public double interestRate { get; set; }
        public string applicationFee { get; set; }
        public string processingFee { get; set; }
        public string insuranceCharges { get; set; }
        public string loanMaturitySum { get; set; }
        public string loanPurpose { get; set; }
        public string loanDisbursementCondition { get; set; }
        public DateTime maturityDate { get; set; }   
        public string loanAmountInFigures { get; set; }
        public string loanAmountInWords { get; set; }
        public IEnumerable<LoanDocumentPageViewModel> pages { get; set; }
        public string guarantorName { get; set; }
        public string relationshipWithClient { get; set; }
        public string guarantorOccupation { get; set; }
        public string guarantorWorkLocation { get; set; }
        public string guarantorAddress { get; set; }
        public string guarantorRegion { get; set; }
        public string guarantorWitness { get; set; }
        public string collateralValueInFigures { get; set; }
        public string collateralForcedSaleValue { get; set; }

        public string loancollateral { get; set; }
        public string loanBusinessAssetValue { get; set; }

        public string date { get; set; }
    }
}
