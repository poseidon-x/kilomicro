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
    
    public partial class depositNextOfKin
    {
        public int depositNextOfKinId { get; set; }
        public int depositId { get; set; }
        public string otherNames { get; set; }
        public string surName { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public int relationshipTypeId { get; set; }
        public int idTypeId { get; set; }
        public string idNumber { get; set; }
        public string phoneNumber { get; set; }
        public double percentageAllocated { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public Nullable<System.DateTime> modified { get; set; }
    
        public virtual deposit deposit { get; set; }
        public virtual idNoType idNoType { get; set; }
        public virtual relationshipType relationshipType { get; set; }
    }
}