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
    
    public interface Imodules
    {
        int module_id { get; set; }
        string module_name { get; set; }
        string url { get; set; }
        Nullable<int> parent_module_id { get; set; }
        System.DateTime creation_date { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modification_date { get; set; }
        string last_modifier { get; set; }
        byte sort_value { get; set; }
        bool visible { get; set; }
        string module_code { get; set; }
    
        ICollection<Irole_perms> role_perms { get; set; }
        ICollection<Iuser_perms> user_perms { get; set; }
        ICollection<IuserAudit> userAudits { get; set; }
    } 
}
