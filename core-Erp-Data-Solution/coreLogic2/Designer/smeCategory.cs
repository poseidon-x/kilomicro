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
    
    public partial class smeCategory
    {
        public smeCategory()
        {
            this.smeDirectors = new HashSet<smeDirector>();
        }
    
        public int smeCategoryID { get; set; }
        public int clientID { get; set; }
        public string companyName { get; set; }
        public string regNo { get; set; }
        public Nullable<int> registeredAddressID { get; set; }
        public Nullable<int> physicalAddressID { get; set; }
        public Nullable<System.DateTime> regDate { get; set; }
        public Nullable<System.DateTime> incDate { get; set; }
        public byte[] version { get; set; }
    
        public virtual client client { get; set; }
        public virtual address address { get; set; }
        public virtual address address1 { get; set; }
        public virtual ICollection<smeDirector> smeDirectors { get; set; }
    }
}