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
    
    public partial class assemblyLineType
    {
        public assemblyLineType()
        {
            this.assemblyLines = new HashSet<assemblyLine>();
        }
    
        public short assemblyLineTypeId { get; set; }
        public string assemblyLineTypeName { get; set; }
    
        public virtual ICollection<assemblyLine> assemblyLines { get; set; }
    }
}