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
    
    public partial class controllerFile
    {
        public controllerFile()
        {
            this.controllerFileDetails = new HashSet<controllerFileDetail>();
        }
    
        public int fileID { get; set; }
        public int fileMonth { get; set; }
        public System.DateTime uploadDate { get; set; }
        public string fileName { get; set; }
    
        public virtual ICollection<controllerFileDetail> controllerFileDetails { get; set; }
    }
}
