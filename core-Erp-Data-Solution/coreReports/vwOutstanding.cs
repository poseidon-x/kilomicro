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
    
    public partial class vwOutstanding
    {
        public int clientID { get; set; }
        public string clientName { get; set; }
        public string accountNumber { get; set; }
        public int loanID { get; set; }
        public string loanNo { get; set; }
        public System.DateTime disbursementDate { get; set; }
        public double fairValue { get; set; }
        public double principalBalance { get; set; }
        public double interestBalance { get; set; }
        public System.DateTime repaymentDate { get; set; }
        public double amountDisbursed { get; set; }
        public double totalPaid { get; set; }
        public double feePaid { get; set; }
        public double penaltyPaid { get; set; }
        public double penalty { get; set; }
        public double fee { get; set; }
        public double writtenOff { get; set; }
        public double interest { get; set; }
        public int daysDue { get; set; }
        public System.DateTime expiryDate { get; set; }
        public string LoanGroupName { get; set; }
    }
}