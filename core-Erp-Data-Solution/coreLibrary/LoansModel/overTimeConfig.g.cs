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
    
    public partial class overTimeConfig
    {
        public int overTimeConfigID { get; set; }
        public int levelID { get; set; }
        public double saturdayHoursRate { get; set; }
        public double sundayHoursRate { get; set; }
        public double holidayHoursRate { get; set; }
        public double weekdayAfterWorkHoursRate { get; set; }
        public double overTime5PerTax { get; set; }
        public double overTime10PerTax { get; set; }
        public double maxOvertimeHours { get; set; }
        public double maxOvertimePercentOfBasic { get; set; }
    
        public virtual level level { get; set; }
    }
}
