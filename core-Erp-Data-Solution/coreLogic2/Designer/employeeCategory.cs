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
    
    public partial class employeeCategory
    {
        public int employeeCategoryID { get; set; }
        public Nullable<int> employerID { get; set; }
        public int clientID { get; set; }
        public Nullable<int> employerDirectorID { get; set; }
        public Nullable<int> employmentTypeID { get; set; }
        public byte[] version { get; set; }
    
        public virtual client client { get; set; }
        public virtual employer employer { get; set; }
        public virtual employerDirector employerDirector { get; set; }
    }
}
