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
    
    public partial class performanceArea
    {
        public performanceArea()
        {
            this.performanceContractItems = new HashSet<performanceContractItem>();
        }
    
        public int performanceAreaID { get; set; }
        public string performanceAreaName { get; set; }
        public string createdBy { get; set; }
        public System.DateTime creationDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    
        public virtual ICollection<performanceContractItem> performanceContractItems { get; set; }
    }
}
