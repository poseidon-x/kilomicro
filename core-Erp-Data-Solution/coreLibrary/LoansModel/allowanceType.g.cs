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
    
    public partial class allowanceType
    {
        public allowanceType()
        {
            this.levelAllowances = new HashSet<levelAllowance>();
            this.payMasterAllowances = new HashSet<payMasterAllowance>();
            this.shiftAllowances = new HashSet<shiftAllowance>();
            this.staffAllowances = new HashSet<staffAllowance>();
        }
    
        public int allowanceTypeID { get; set; }
        public string alllowanceTypeName { get; set; }
        public bool isPercent { get; set; }
        public double amount { get; set; }
        public bool isTaxable { get; set; }
        public double taxPercent { get; set; }
        public bool addToBasicAndTax { get; set; }
    
        public virtual ICollection<levelAllowance> levelAllowances { get; set; }
        public virtual ICollection<payMasterAllowance> payMasterAllowances { get; set; }
        public virtual ICollection<shiftAllowance> shiftAllowances { get; set; }
        public virtual ICollection<staffAllowance> staffAllowances { get; set; }
    }
}