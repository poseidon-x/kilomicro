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
    
    public interface IopenningBalanceBatch
    {
        long openningBalanceBatchId { get; set; }
        System.DateTime balanceDate { get; set; }
        int locationId { get; set; }
        string creator { get; set; }
        System.DateTime created { get; set; }
        string modifier { get; set; }
        System.DateTime modified { get; set; }
        string enteredBy { get; set; }
        bool posted { get; set; }
        Nullable<System.DateTime> postedDate { get; set; }
        bool approved { get; set; }
        string approvedBy { get; set; }
        string postedBy { get; set; }
        string postingComments { get; set; }
        string approvalComments { get; set; }
        Nullable<System.DateTime> approvalDate { get; set; }
    
        ICollection<IopenningBalance> openningBalances { get; set; }
    } 
}