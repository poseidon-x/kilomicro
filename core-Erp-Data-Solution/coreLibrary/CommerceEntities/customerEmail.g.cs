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
    
    public partial class customerEmail
    {
        public long customerEmailId { get; set; }
        public long customerId { get; set; }
        public string emailAddress1 { get; set; }
        public string emailAddress2 { get; set; }
        public string emailAddress3 { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
    
        public virtual customer customer { get; set; }
    }
}