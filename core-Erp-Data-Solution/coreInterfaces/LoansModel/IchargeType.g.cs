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
    public interface IchargeType
    {
        int chargeTypeID { get; set; }
        string chargeTypeName { get; set; }
        string chargeTypeCode { get; set; }
        bool automatic { get; set; }
        double chargeDefaultAmount { get; set; }
        bool @fixed { get; set; }
        Nullable<int> accountsPayableAccountID { get; set; }
        Nullable<int> accountsReceivableAccountID { get; set; }
    
        ICollection<IchargeTypeTier> chargeTypeTiers { get; set; }
        ICollection<IclientServiceCharge> clientServiceCharges { get; set; }
        ICollection<IdepositCharge> depositCharges { get; set; }
        ICollection<IinvestmentCharge> investmentCharges { get; set; }
        ICollection<IsavingCharge> savingCharges { get; set; }
    } 
    
}