using System;
using System.Collections.Generic;
using coreErpApi.Models;
using coreLogic;

namespace coreErpApi.Models.Loan
{
    public class LoanRepaymentModel 
    {
        public int loanRepaymentID { get; set; }
        public int loanID { get; set; }

        
        public string modeOfPaymentName { get; set; }
        public string repaymentTypeName { get; set; }
        public DateTime repaymentDate { get; set; }
        public double amountPaid { get; set; }
        public double interestPaid { get; set; }
        public double principalPaid { get; set; }
    }
}