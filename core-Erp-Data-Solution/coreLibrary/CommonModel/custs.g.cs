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
    
    public partial class custs
    {
        public int cust_id { get; set; }
        public Nullable<int> rep_emp_id { get; set; }
        public string acc_num { get; set; }
        public string cust_name { get; set; }
        public string contact_person { get; set; }
        public string credit_terms { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
    
        public virtual accts ar_accts { get; set; }
        public virtual accts vat_accts { get; set; }
        public virtual currencies currencies { get; set; }
        public virtual cust_types cust_types { get; set; }
    }
}
