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
    
    public partial class depositSignatory
    {
        public int depositSignatoryID { get; set; }
        public int depositID { get; set; }
        public string fullName { get; set; }
        public Nullable<int> signatureImageID { get; set; }
        public byte[] version { get; set; }
    
        public virtual deposit deposit { get; set; }
        public virtual image image { get; set; }
    }
}
