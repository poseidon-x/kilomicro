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
    
    public interface IsavingPlanFlag
    {
        long savingPlanFlagID { get; set; }
        int savingID { get; set; }
        System.DateTime flagDate { get; set; }
        byte currentPlanId { get; set; }
        byte proposedPlanId { get; set; }
        Nullable<bool> approved { get; set; }
        string approvedBy { get; set; }
        Nullable<System.DateTime> approvedDate { get; set; }
        Nullable<bool> applied { get; set; }
        Nullable<System.DateTime> appliedDate { get; set; }
        string appliedBy { get; set; }
    
        Isaving saving { get; set; }
    } 
    
}
