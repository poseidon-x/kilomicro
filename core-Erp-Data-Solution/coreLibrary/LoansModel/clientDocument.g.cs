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
    
    public partial class clientDocument
    {
        public int clientDocumentID { get; set; }
        public int documentID { get; set; }
        public int clientID { get; set; }
        public byte[] version { get; set; }
    
        public virtual client client { get; set; }
        public virtual document document { get; set; }
    }
}
