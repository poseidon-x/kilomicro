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
    
    public partial class jnl_batch_tmp
    {
        public jnl_batch_tmp()
        {
            this.jnl_tmp = new HashSet<jnl_tmp>();
        }
    
        public int jnl_batch_id { get; set; }
        public string batch_no { get; set; }
        public string source { get; set; }
        public bool posted { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
        public bool multi_currency { get; set; }
        public bool is_adj { get; set; }
        public byte[] version { get; set; }
    
        public virtual ICollection<jnl_tmp> jnl_tmp { get; set; }
    }
}
