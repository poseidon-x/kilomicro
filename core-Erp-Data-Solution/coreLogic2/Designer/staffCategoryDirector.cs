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
    
    public partial class staffCategoryDirector
    {
        public int staffCategoryDirectorID { get; set; }
        public int staffCategoryID { get; set; }
        public int employerDirectorID { get; set; }
    
        public virtual employerDirector employerDirector { get; set; }
        public virtual staffCategory1 staffCategory1 { get; set; }
    }
}
