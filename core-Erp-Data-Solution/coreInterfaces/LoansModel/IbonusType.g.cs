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
    
     // Condition: ^[^v].*Type$
    public interface IbonusType
    {
        int bonusTypeID { get; set; }
        string bonusTypeName { get; set; }
        int bonusCalculationTypeID { get; set; }
        double amount { get; set; }
        bool isTaxable { get; set; }
    
        IbonusCalculationType bonusCalculationType { get; set; }
    } 
    
}
