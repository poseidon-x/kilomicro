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
    
    public interface IshrinkageReason
    {
        int shrinkageReasonId { get; set; }
        string shrinkageReasonCode { get; set; }
        string shrinkageReasonName { get; set; }
    
        ICollection<Ishrinkage> shrinkages { get; set; }
    } 
}
