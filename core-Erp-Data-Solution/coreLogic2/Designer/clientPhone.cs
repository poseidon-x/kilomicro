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
    
    public partial class clientPhone
    {
        public int clientPhoneID { get; set; }
        public int clientID { get; set; }
        public Nullable<int> phoneID { get; set; }
        public Nullable<int> phoneTypeID { get; set; }
        public byte[] version { get; set; }
    
        public virtual client client { get; set; }
        public virtual phone phone { get; set; }
    }
}