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
    
    public partial class acct_bals
    {
        public int acct_bal_id { get; set; }
        public int acct_id { get; set; }
        public int acct_period { get; set; }
        public double buy_rate { get; set; }
        public double sell_rate { get; set; }
        public double loc_bal { get; set; }
        public double frgn_bal { get; set; }
        public int currency_id { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
        public byte[] version { get; set; }
    
        public virtual accts acct { get; set; }
        public virtual acct_period acct_period1 { get; set; }
    }
}
