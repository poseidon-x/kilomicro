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
    
    public interface Ideposit
    {
        int depositID { get; set; }
        int clientID { get; set; }
        int depositTypeID { get; set; }
        double amountInvested { get; set; }
        double interestAccumulated { get; set; }
        double interestBalance { get; set; }
        double principalBalance { get; set; }
        double interestRate { get; set; }
        System.DateTime firstDepositDate { get; set; }
        int period { get; set; }
        Nullable<System.DateTime> maturityDate { get; set; }
        bool autoRollover { get; set; }
        Nullable<System.DateTime> creation_date { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modification_date { get; set; }
        string last_modifier { get; set; }
        string depositNo { get; set; }
        Nullable<System.DateTime> lastInterestDate { get; set; }
        bool interestMethod { get; set; }
        int interestRepaymentModeID { get; set; }
        int principalRepaymentModeID { get; set; }
        double principalAuthorized { get; set; }
        double interestAuthorized { get; set; }
        int interestCalculationScheduleID { get; set; }
        double fxRate { get; set; }
        int currencyID { get; set; }
        double localAmount { get; set; }
        double lastPrincipalFxGainLoss { get; set; }
        double lastInterestFxGainLoss { get; set; }
        int interestPayableAccountID { get; set; }
        double interestExpected { get; set; }
        byte[] version { get; set; }
        Nullable<int> staffID { get; set; }
        bool modern { get; set; }
        Nullable<int> agentId { get; set; }
        double annualInterestRate { get; set; }
        int depositPeriodInDays { get; set; }
        bool maturityNotificationSent { get; set; }
        bool closed { get; set; }
    
        Iclient client { get; set; }
        IdepositType depositType { get; set; }
        ICollection<IdepositAdditional> depositAdditionals { get; set; }
        ICollection<IdepositCharge> depositCharges { get; set; }
        ICollection<IdepositInterest> depositInterests { get; set; }
        ICollection<IdepositSchedule> depositSchedules { get; set; }
        ICollection<IdepositSignatory> depositSignatories { get; set; }
        ICollection<IdepositWithdrawal> depositWithdrawals { get; set; }
        ICollection<IdepositAuthorization> depositAuthorizations { get; set; }
        ICollection<IdepositNextOfKin> depositNextOfKins { get; set; }
        ICollection<IdepositRateUpgrade> depositRateUpgrades { get; set; }
        ICollection<IdepositUpgrade> depositUpgrades { get; set; }
        ICollection<IdepositUpgrade> depositUpgrades1 { get; set; }
    } 
    
}