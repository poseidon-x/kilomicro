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
    
    
    public interface Isup_phons
    {
        int sup_phon_id { get; set; }
        int sup_id { get; set; }
        bool is_default { get; set; }
        string phon_num { get; set; }
        Nullable<System.DateTime> creation_date { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modification_date { get; set; }
        string last_modifier { get; set; }
    
        Iphon_types phon_types { get; set; }
    }
    
}
