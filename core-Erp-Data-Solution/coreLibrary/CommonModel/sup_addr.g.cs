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
    
    public partial class sup_addr
    {
        public int sup_addr_id { get; set; }
        public bool is_default { get; set; }
        public int sup_id { get; set; }
        public string addr_line_1 { get; set; }
        public string addr_line_2 { get; set; }
        public Nullable<int> city_id { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
    
        public virtual addr_types addr_types { get; set; }
    }
}