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
    
    public interface IregularSusuContributionSchedule
    {
        int regularSusuContributionScheduleID { get; set; }
        int regularSusuAccountID { get; set; }
        System.DateTime plannedContributionDate { get; set; }
        Nullable<System.DateTime> actualContributionDate { get; set; }
        double balance { get; set; }
        double amount { get; set; }
        byte[] version { get; set; }
    
        IregularSusuAccount regularSusuAccount { get; set; }
    } 
    
}