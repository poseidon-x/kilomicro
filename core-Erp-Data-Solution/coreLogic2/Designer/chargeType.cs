//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic.Designer
{
    using System;
    using System.Collections.Generic;
    
    public partial class chargeType
    {
        public chargeType()
        {
            this.depositCharges = new HashSet<depositCharge>();
            this.savingCharges = new HashSet<savingCharge>();
            this.investmentCharges = new HashSet<investmentCharge>();
            this.chargeTypeTiers = new HashSet<chargeTypeTier>();
        }
    
        public int chargeTypeID { get; set; }
        public string chargeTypeName { get; set; }
        public string chargeTypeCode { get; set; }
        public bool automatic { get; set; }
    
        public virtual ICollection<depositCharge> depositCharges { get; set; }
        public virtual ICollection<savingCharge> savingCharges { get; set; }
        public virtual ICollection<investmentCharge> investmentCharges { get; set; }
        public virtual ICollection<chargeTypeTier> chargeTypeTiers { get; set; }
    }
}
