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
    
    public interface IcreditUnionShareTransaction
    {
        long creditUnionShareTransactionID { get; set; }
        long creditUnionMemberID { get; set; }
        System.DateTime transactionDate { get; set; }
        string transactionType { get; set; }
        int modeOfPaymentID { get; set; }
        string checkNumber { get; set; }
        Nullable<int> bankID { get; set; }
        double numberOfShares { get; set; }
        double sharePrice { get; set; }
        bool posted { get; set; }
        Nullable<System.DateTime> postingDate { get; set; }
        string enteredBy { get; set; }
        string postedBy { get; set; }
        System.DateTime entryDate { get; set; }
    
        IcreditUnionMember creditUnionMember { get; set; }
     
    } 
    
}
