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
    
    public partial class chargeTypeTier
    {
        public int chargeTypeTierId { get; set; }
        public int chargeTypeId { get; set; }
        public double percentCharge { get; set; }
        public double minChargeAmount { get; set; }
        public double minimumTransactionAmount { get; set; }
        public double maximumTransactionAmount { get; set; }
        public double maturityPercentCharge { get; set; }
        public double maturityPercentageCharge { get; set; }
    
        public virtual chargeType chargeType { get; set; }
    }
}