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
    
    public interface IcreditLine1
    {
        int creditLineId { get; set; }
        int clientId { get; set; }
        Nullable<int> loanId { get; set; }
        int tenure { get; set; }
        double amountRequested { get; set; }
        Nullable<System.DateTime> expiryDate { get; set; }
        double amountDisbursed { get; set; }
        System.DateTime applicationDate { get; set; }
        bool isApproved { get; set; }
        Nullable<System.DateTime> approvalDate { get; set; }
        string approvedBy { get; set; }
        bool closed { get; set; }
        string creator { get; set; }
        System.DateTime creationDate { get; set; }
        string modifier { get; set; }
        Nullable<System.DateTime> modified { get; set; }
        string creditLineNumber { get; set; }
        double amountApproved { get; set; }
    
        Iclient client { get; set; }
        Iloan loan { get; set; }
    } 
    
}