//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic.Designer
{
    using System;
    using System.Collections.Generic;
    
    public partial class loanGurantor
    {
        public int loanGurantorID { get; set; }
        public int loanID { get; set; }
        public string surName { get; set; }
        public string otherNames { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> idNoID { get; set; }
        public Nullable<int> addressID { get; set; }
        public Nullable<int> phoneID { get; set; }
        public Nullable<int> emailID { get; set; }
        public Nullable<int> imageID { get; set; }
        public string creditOfficerNotes { get; set; }
        public string creditCommitteeNotes { get; set; }
        public string loanUpdateNotes { get; set; }
        public Nullable<System.DateTime> creation_date { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> modification_date { get; set; }
        public string last_modifier { get; set; }
        public byte[] version { get; set; }
    
        public virtual address address { get; set; }
        public virtual email email { get; set; }
        public virtual image image { get; set; }
        public virtual loan loan { get; set; }
        public virtual phone phone { get; set; }
        public virtual idNo idNo { get; set; }
    }
}
