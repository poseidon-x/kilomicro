using System;

namespace coreErp.Models.Loan
{
    public class LoanModel
    {
        public int loanId { get; set; }
        public int clientId { get; set; }
        public string clientFullName { get; set; }
        public string loanNumber { get; set; }
        public string loanTypeName { get; set; }
        public double loanTenure { get; set; }
        public double amountRequested { get; set; }
        public double amountDisbursed { get; set; }
        public double amountApproved { get; set; }
        public DateTime applicationDate { get; set; }
        public DateTime disbursementDate { get; set; }
        public string repaymentModeName { get; set; }
        public double interestRate { get; set; }
        public string interestTypeName { get; set; }
        public string fielAgentName { get; set; }
        public string staffName { get; set; }
        public double interestBalance { get; set; }
        public double principalBalance { get; set; }
        public double penaltyBalance { get; set; }
    }
}