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
    
    public interface IsavingBalance
    {
        int savingBalanceId { get; set; }
        System.DateTime balanceDate { get; set; }
        double beginningOfDayBalance { get; set; }
        Nullable<double> endOfDayBalance { get; set; }
        Nullable<double> totalCredit { get; set; }
        double totalDebit { get; set; }
        string creator { get; set; }
        System.DateTime created { get; set; }
        string modifier { get; set; }
        System.DateTime modified { get; set; }
    } 
    
}