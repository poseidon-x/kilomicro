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
    
    public interface IsavingRollOver
    {
        int savingRollOverID { get; set; }
        int savingID { get; set; }
        double interestPayment { get; set; }
        double principalPayment { get; set; }
        System.DateTime rollOverDate { get; set; }
    
        Isaving saving { get; set; }
    } 
    
}