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
    
    public interface IdepositWithdrawal
    {
        int depositWithdrawalID { get; set; }
        int depositID { get; set; }
        System.DateTime withdrawalDate { get; set; }
        double interestWithdrawal { get; set; }
        double principalWithdrawal { get; set; }
        double principalBalance { get; set; }
        double interestBalance { get; set; }
        Nullable<System.DateTime> creation_date { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modification_date { get; set; }
        string last_modifier { get; set; }
        Nullable<int> bankID { get; set; }
        string checkNo { get; set; }
        int modeOfPaymentID { get; set; }
        double fxRate { get; set; }
        double localAmount { get; set; }
        bool posted { get; set; }
        string naration { get; set; }
        byte[] version { get; set; }
        Nullable<bool> isDisInvestment { get; set; }
        Nullable<double> disInvestmentCharge { get; set; }
        double balanceBD { get; set; }
    
        Ideposit deposit { get; set; }
        ImodeOfPayment modeOfPayment { get; set; }
    } 
    
}
