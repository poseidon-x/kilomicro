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
    
    public partial class SpecialDay
    {
        public short specialDayId { get; set; }
        public string specialDayName { get; set; }
        public short specialDayTypeId { get; set; }
        public string specialDayValue { get; set; }
    
        public virtual SpecialDayType SpecialDayType { get; set; }
    }
}
