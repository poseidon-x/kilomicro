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
    
    public partial class categoryCheckList
    {
        public categoryCheckList()
        {
            this.loanCheckLists = new HashSet<loanCheckList>();
        }
    
        public int categoryCheckListID { get; set; }
        public int categoryID { get; set; }
        public string description { get; set; }
        public bool isMandatory { get; set; }
    
        public virtual category category { get; set; }
        public virtual ICollection<loanCheckList> loanCheckLists { get; set; }
    }
}
