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
    using System.Collections.Generic;
    
    public partial class creditUnionShareTransaction
    {
        public long creditUnionShareTransactionID { get; set; }
        public long creditUnionMemberID { get; set; }
        public System.DateTime transactionDate { get; set; }
        public string transactionType { get; set; }
        public int modeOfPaymentID { get; set; }
        public string checkNumber { get; set; }
        public Nullable<int> bankID { get; set; }
        public double numberOfShares { get; set; }
        public double sharePrice { get; set; }
        public bool posted { get; set; }
        public Nullable<System.DateTime> postingDate { get; set; }
        public string enteredBy { get; set; }
        public string postedBy { get; set; }
        public System.DateTime entryDate { get; set; }
    
        public virtual creditUnionMember creditUnionMember { get; set; }
    }
}