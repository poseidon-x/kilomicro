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
    
    public partial class savingBalance
    {
        public int savingBalanceId { get; set; }
        public System.DateTime balanceDate { get; set; }
        public double beginningOfDayBalance { get; set; }
        public Nullable<double> endOfDayBalance { get; set; }
        public Nullable<double> totalCredit { get; set; }
        public double totalDebit { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
    }
}
