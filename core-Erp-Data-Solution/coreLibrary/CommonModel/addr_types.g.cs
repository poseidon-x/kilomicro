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
    
    public partial class addr_types
    {
        public addr_types()
        {
            this.cust_addr = new HashSet<cust_addr>();
            this.sup_addr = new HashSet<sup_addr>();
        }
    
        public string addr_type { get; set; }
        public string addr_type_name { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
    
        public virtual ICollection<cust_addr> cust_addr { get; set; }
        public virtual ICollection<sup_addr> sup_addr { get; set; }
    }
}