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
    
    public partial class pc_head
    {
        public pc_head()
        {
            this.pc_dtl = new HashSet<pc_dtl>();
        }
    
        public int pc_head_id { get; set; }
        public int pc_acct_id { get; set; }
        public string batch_no { get; set; }
        public int currency_id { get; set; }
        public double rate { get; set; }
        public string recipient { get; set; }
        public Nullable<int> recipient_id { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
        public bool posted { get; set; }
    
        public virtual ICollection<pc_dtl> pc_dtl { get; set; }
    }
}