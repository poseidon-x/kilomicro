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
    
    public interface Itenor
    {
        int tenorid { get; set; }
        int tenor1 { get; set; }
        Nullable<double> defaultInterestRate { get; set; }
        Nullable<double> defaultPenaltyRate { get; set; }
        Nullable<int> defaultGracePeriod { get; set; }
        Nullable<int> defaultRepaymentModeID { get; set; }
        Nullable<double> defaultApplicationFeeRate { get; set; }
        Nullable<double> defaultProcessingFeeRate { get; set; }
        Nullable<double> defaultCommissionRate { get; set; }
        Nullable<int> loanTypeID { get; set; }
    } 
    
}
