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
    
    public partial class collateralType
    {
        public collateralType()
        {
            this.loanCollaterals = new HashSet<loanCollateral>();
        }
    
        public int collateralTypeID { get; set; }
        public string collateralTypeName { get; set; }
    
        public virtual ICollection<loanCollateral> loanCollaterals { get; set; }
    }
}