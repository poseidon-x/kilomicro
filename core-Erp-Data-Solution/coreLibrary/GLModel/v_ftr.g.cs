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
    
    public partial class v_ftr
    {
        public int v_ftr_id { get; set; }
        public int v_head_id { get; set; }
        public int acct_id { get; set; }
        public bool is_perc { get; set; }
        public string description { get; set; }
        public double amount { get; set; }
        public double tot_amount { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
    
        public virtual v_head v_head { get; set; }
    }
}
