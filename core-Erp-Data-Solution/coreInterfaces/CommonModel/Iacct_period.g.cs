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
    
    
    public interface Iacct_period
    {
        int acct_period1 { get; set; }
        System.DateTime close_date { get; set; }
        Nullable<System.DateTime> creation_date { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modification_date { get; set; }
        string last_modifier { get; set; }
    
        ICollection<Iacct_bals> acct_bals { get; set; }
    }
    
}