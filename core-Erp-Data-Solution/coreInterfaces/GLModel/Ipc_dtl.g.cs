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
    
    public interface Ipc_dtl
    {
        int pc_dtl_id { get; set; }
        int pc_head_id { get; set; }
        int acct_id { get; set; }
        string ref_no { get; set; }
        System.DateTime tx_date { get; set; }
        string description { get; set; }
        Nullable<int> gl_ou_id { get; set; }
        double amount { get; set; }
        Nullable<System.DateTime> creation_date { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modification_date { get; set; }
        string last_modifier { get; set; }
        string check_no { get; set; }
    
        Ipc_head pc_head { get; set; }
     
    } 
    
}
