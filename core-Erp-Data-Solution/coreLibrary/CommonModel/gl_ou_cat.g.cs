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
    
    public partial class gl_ou_cat
    {
        public gl_ou_cat()
        {
            this.gl_ou = new HashSet<gl_ou>();
        }
    
        public int ou_cat_id { get; set; }
        public string cat_name { get; set; }
        public Nullable<int> parent_ou_cat_id { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
    
        public virtual ICollection<gl_ou> gl_ou { get; set; }
    }
}