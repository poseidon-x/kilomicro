using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreErpApi.Models.Loan;
using coreLogic;

namespace coreERP.Models.Loan
{
    public class LoanDetailViewModel : loan
    {
        public int loanId { get; set; }
        public string clientName { get; set; }
        public string loanNo { get; set; }
        public double interestBalance { get; set; }
        public double principalBalance { get; set; }
        public double penaltyBalance { get; set; }
        public IEnumerable<LoanGurantorModel> lnGurantors { get; set; }
        public List<LoanDocumentModel> lnDocuments { get; set; }
        public List<LoanCollateralModel> lnCollateral { get; set; }
    }
}