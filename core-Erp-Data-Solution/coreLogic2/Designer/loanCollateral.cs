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
    
    public partial class loanCollateral
    {
        public loanCollateral()
        {
            this.collateralImages = new HashSet<collateralImage>();
        }
    
        public int loanCollateralID { get; set; }
        public int loanID { get; set; }
        public int collateralTypeID { get; set; }
        public double fairValue { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
        public string legalOwner { get; set; }
        public string collateralDescription { get; set; }
        public byte[] version { get; set; }
    
        public virtual ICollection<collateralImage> collateralImages { get; set; }
        public virtual collateralType collateralType { get; set; }
        public virtual loan loan { get; set; }
    }
}