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
    
    public partial class clientEmail
    {
        public int clientEmailID { get; set; }
        public int clientID { get; set; }
        public Nullable<int> emailID { get; set; }
        public Nullable<int> emailTypeID { get; set; }
        public byte[] version { get; set; }
    
        public virtual client client { get; set; }
        public virtual email email { get; set; }
    }
}
