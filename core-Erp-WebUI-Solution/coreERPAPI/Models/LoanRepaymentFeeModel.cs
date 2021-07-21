using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreERP.Models.Client;

namespace coreERP.Models
{
    public class LoanRepaymentFeeModel/*: GroupLoanPayment*/
    {
        public int paymentId { get; set; }
        public int loanId { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; }
        public string loanNo { get; set; }
        public bool paid { get; set; }
        public double insuranceAmount { get; set; }
        public double processingFeeAmount { get; set; }
        public double totalFees { get; set; }

    }

    public class LoanRepaymentFeePostModel
    {
        public int dayId { get; set; }

        public DateTime repaymentDate { get; set; }
        public List<LoanRepaymentFeeModel> repaymentFees { get; set; }
    }

    public class LoanFeePostModel
    {
        public DateTime repaymentDate { get; set; }
        public List<LoanFeesRepayment> repaymentFees { get; set; }
    }

    public class LoanFeesRepayment
    {
        public int paymentId { get; set; }
        public int loanId { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; }
        public string groupName { get; set; }

        public string loanNo { get; set; }
        public bool paid { get; set; }
        public double insuranceAmount { get; set; }
        public double processingFeeAmount { get; set; }
        public double totalFees { get; set; }

    }
}