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
    
    public partial class vwLoanHistory
    {
        public int clientID { get; set; }
        public int loanID { get; set; }
        public string loanNo { get; set; }
        public int year0 { get; set; }
        public int month0 { get; set; }
        public System.DateTime disbursementDate { get; set; }
        public double amountDisbursed { get; set; }
        public double amountApproved { get; set; }
        public double amountRequested { get; set; }
        public double riskRatio { get; set; }
        public double affordabilityRatio { get; set; }
        public string approvalComments { get; set; }
        public string creditOfficerNotes { get; set; }
        public double interestRate { get; set; }
        public double loanTenure { get; set; }
        public System.DateTime startDate { get; set; }
        public System.DateTime lastPaymentDate { get; set; }
        public string staffName { get; set; }
        public int staffID { get; set; }
        public int daysDelta { get; set; }
        public double amountPaid { get; set; }
        public double totalBalance { get; set; }
    }
}
