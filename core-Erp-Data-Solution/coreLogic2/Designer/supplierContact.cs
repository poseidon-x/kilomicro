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
    
    public partial class supplierContact
    {
        public int supplierContactID { get; set; }
        public string contactName { get; set; }
        public string workPhone { get; set; }
        public string mobilePhone { get; set; }
        public string email { get; set; }
        public string department { get; set; }
        public int supplierID { get; set; }
    
        public virtual supplier supplier { get; set; }
    }
}
