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
    
    public partial class jnl_tmp
    {
        public int jnl_id { get; set; }
        public string ref_no { get; set; }
        public System.DateTime tx_date { get; set; }
        public Nullable<int> acct_period { get; set; }
        public string description { get; set; }
        public double rate { get; set; }
        public double dbt_amt { get; set; }
        public double crdt_amt { get; set; }
        public double frgn_dbt_amt { get; set; }
        public double frgn_crdt_amt { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
        public string recipient { get; set; }
        public string check_no { get; set; }
        public byte[] version { get; set; }
    
        public virtual accts accts { get; set; }
        public virtual currencies currencies { get; set; }
        public virtual gl_ou gl_ou { get; set; }
        public virtual jnl_batch_tmp jnl_batch_tmp { get; set; }
    }
}