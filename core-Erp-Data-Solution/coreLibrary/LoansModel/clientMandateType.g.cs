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
    
    public partial class clientMandateType
    {
        public clientMandateType()
        {
            this.clients = new HashSet<client>();
        }
    
        public int clientMandateTypeId { get; set; }
        public string mandate { get; set; }
    
        public virtual ICollection<client> clients { get; set; }
    }
}