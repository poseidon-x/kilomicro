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
    
    public interface ImodeOfPayment
    {
        int modeOfPaymentID { get; set; }
        string modeOfPaymentName { get; set; }
    
        ICollection<IcashierDisbursement> cashierDisbursements { get; set; }
        ICollection<IcashierReceipt> cashierReceipts { get; set; }
        ICollection<IdepositAdditional> depositAdditionals { get; set; }
        ICollection<IdepositWithdrawal> depositWithdrawals { get; set; }
        ICollection<IinvestmentAdditional> investmentAdditionals { get; set; }
        ICollection<IinvestmentWithdrawal> investmentWithdrawals { get; set; }
        ICollection<IloanRepayment> loanRepayments { get; set; }
        ICollection<IloanTranch> loanTranches { get; set; }
        ICollection<IregularSusuContribution> regularSusuContributions { get; set; }
        ICollection<IsavingAdditional> savingAdditionals { get; set; }
        ICollection<IsavingWithdrawal> savingWithdrawals { get; set; }
        ICollection<IsusuContribution> susuContributions { get; set; }
        ICollection<IborrowingDisbursement> borrowingDisbursements { get; set; }
        ICollection<IborrowingRepayment> borrowingRepayments { get; set; }
        ICollection<IclientInvestmentReceiptDetail> clientInvestmentReceiptDetails { get; set; }
        ICollection<IdepositUpgrade> depositUpgrades { get; set; }
    } 
    
}