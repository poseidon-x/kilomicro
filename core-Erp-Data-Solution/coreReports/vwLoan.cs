//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreReports
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwLoan
    {
        public int clientID { get; set; }
        public string clientName { get; set; }
        public string accountNumber { get; set; }
        public int loanID { get; set; }
        public string loanNo { get; set; }
        public System.DateTime disbursementDate { get; set; }
        public double amountDisbursed { get; set; }
        public double amountApproved { get; set; }
        public double amountRequested { get; set; }
        public int loanTypeID { get; set; }
        public string loanTypeName { get; set; }
        public double principalPayment { get; set; }
        public double interestPayment { get; set; }
        public int year0 { get; set; }
        public int month0 { get; set; }
        public double riskRatio { get; set; }
        public double affordabilityRatio { get; set; }
        public System.DateTime expiryDate { get; set; }
        public double loanTenure { get; set; }
        public System.DateTime approvalDate { get; set; }
        public double totalPayment { get; set; }
        public string checkedBy { get; set; }
        public string approvedBy { get; set; }
        public string enteredBy { get; set; }
    }
}