//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic
{
    using System;
    
    public partial class getLoanBalances1_Result
    {
        public int loanID { get; set; }
        public string loanNo { get; set; }
        public string invoiceNo { get; set; }
        public Nullable<System.DateTime> disbursementDate { get; set; }
        public double principalOutstanding { get; set; }
        public double processingFee { get; set; }
        public double interestOutstanding { get; set; }
        public double totalOutstanding { get; set; }
        public int amountPaid { get; set; }
        public string description { get; set; }
    }
}