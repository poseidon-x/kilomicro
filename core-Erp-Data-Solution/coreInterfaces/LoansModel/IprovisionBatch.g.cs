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
    
    public interface IprovisionBatch
    {
        int provisionBatchId { get; set; }
        int provisionYear { get; set; }
        int provisionMonth { get; set; }
        System.DateTime provisionDate { get; set; }
        bool edited { get; set; }
        bool posted { get; set; }
        bool reversed { get; set; }
        Nullable<int> provisionJournalBatchId { get; set; }
        Nullable<int> reversalJournalBatchId { get; set; }
        Nullable<System.DateTime> postedValueDate { get; set; }
        Nullable<System.DateTime> reversalValueDate { get; set; }
        string initiatedBy { get; set; }
        System.DateTime initiationDate { get; set; }
        string postedBy { get; set; }
        string reversedBy { get; set; }
    
        ICollection<IloanProvision> loanProvisions { get; set; }
    } 
    
}