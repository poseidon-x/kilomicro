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
    
    public partial class employerDirector
    {
        public employerDirector()
        {
            this.employeeCategories = new HashSet<employeeCategory>();
            this.staffCategoryDirectors = new HashSet<staffCategoryDirector>();
        }
    
        public int employerDirectorID { get; set; }
        public Nullable<int> employerID { get; set; }
        public Nullable<int> signatureImageID { get; set; }
        public Nullable<int> phoneID { get; set; }
        public Nullable<int> emailID { get; set; }
        public string surName { get; set; }
        public string otherNames { get; set; }
        public Nullable<int> idNoID { get; set; }
        public byte[] version { get; set; }
    
        public virtual email email { get; set; }
        public virtual ICollection<employeeCategory> employeeCategories { get; set; }
        public virtual employer employer { get; set; }
        public virtual idNo idNo { get; set; }
        public virtual phone phone { get; set; }
        public virtual ICollection<staffCategoryDirector> staffCategoryDirectors { get; set; }
        public virtual image image { get; set; }
    }
}