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
    
    public partial class group
    {
        public group()
        {
            this.groupCategories = new HashSet<groupCategory>();
            this.groupExecs = new HashSet<groupExec>();
        }
    
        public int groupID { get; set; }
        public string groupName { get; set; }
        public Nullable<int> groupSize { get; set; }
        public Nullable<int> addressID { get; set; }
        public Nullable<int> groupTypeID { get; set; }
        public byte[] version { get; set; }
    
        public virtual address address { get; set; }
        public virtual ICollection<groupCategory> groupCategories { get; set; }
        public virtual ICollection<groupExec> groupExecs { get; set; }
    }
}