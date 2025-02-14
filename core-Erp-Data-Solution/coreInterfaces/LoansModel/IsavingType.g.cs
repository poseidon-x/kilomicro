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
    public interface IsavingType
    {
        int savingTypeID { get; set; }
        string savingTypeName { get; set; }
        double interestRate { get; set; }
        int defaultPeriod { get; set; }
        bool allowsInterestWithdrawal { get; set; }
        bool allowsPrincipalWithdrawal { get; set; }
        Nullable<int> vaultAccountID { get; set; }
        Nullable<int> accountsPayableAccountID { get; set; }
        Nullable<int> interestExpenseAccountID { get; set; }
        int fxUnrealizedGainLossAccountID { get; set; }
        int fxRealizedGainLossAccountID { get; set; }
        int interestCalculationScheduleID { get; set; }
        int interestPayableAccountID { get; set; }
        Nullable<int> chargesIncomeAccountID { get; set; }
        Nullable<double> minPlanAmount { get; set; }
        Nullable<double> maxPlanAmount { get; set; }
        Nullable<byte> planID { get; set; }
        Nullable<double> earlyWithdrawalChargeRate { get; set; }
        double minDaysBeforeInterest { get; set; }
        double minimumBalance { get; set; }
    
        ICollection<Isaving> savings { get; set; }
    } 
    
}
