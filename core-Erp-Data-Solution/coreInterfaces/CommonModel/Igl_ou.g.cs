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
    
    
    public interface Igl_ou
    {
        int ou_id { get; set; }
        string ou_name { get; set; }
        Nullable<int> parent_ou_id { get; set; }
        Nullable<System.DateTime> creation_date { get; set; }
        Nullable<int> ou_manager_id { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modification_date { get; set; }
        string last_modifier { get; set; }
    
        Igl_ou_cat gl_ou_cat { get; set; }
        ICollection<Ijnl_tmp> jnl_tmp { get; set; }
        ICollection<Iuser_gl_ou_gl_ou> user_gl_ou_gl_ou { get; set; }
        ICollection<Ibudget> budgets { get; set; }
        ICollection<Ijnl> jnl { get; set; }
    }
    
}