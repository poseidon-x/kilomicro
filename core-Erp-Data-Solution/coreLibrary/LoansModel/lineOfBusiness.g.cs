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
    
    public partial class lineOfBusiness
    {
        public lineOfBusiness()
        {
            this.microBusinessCategories = new HashSet<microBusinessCategory>();
        }
    
        public int lineOfBusinessID { get; set; }
        public string lineOfBusinessName { get; set; }
    
        public virtual ICollection<microBusinessCategory> microBusinessCategories { get; set; }
    }
}
