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
    public interface IoneTimeDeductionType
    {
        int oneTimeDeductionTypeID { get; set; }
        string oneTimeDeductionTypeName { get; set; }
        bool isPercent { get; set; }
        double amount { get; set; }
        bool isBeforeTax { get; set; }
        bool isBeforePension { get; set; }
    
        ICollection<IpayMasterOneTimeDeduction> payMasterOneTimeDeductions { get; set; }
        ICollection<IstaffOneTimeDeduction> staffOneTimeDeductions { get; set; }
    } 
    
}
