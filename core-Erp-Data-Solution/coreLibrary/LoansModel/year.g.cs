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
    
    public partial class year
    {
        public year()
        {
            this.publicHolidays = new HashSet<publicHoliday>();
            this.staffAttendances = new HashSet<staffAttendance>();
            this.staffLeaveBalances = new HashSet<staffLeaveBalance>();
            this.staffShifts = new HashSet<staffShift>();
        }
    
        public int yearID { get; set; }
        public short year1 { get; set; }
    
        public virtual ICollection<publicHoliday> publicHolidays { get; set; }
        public virtual ICollection<staffAttendance> staffAttendances { get; set; }
        public virtual ICollection<staffLeaveBalance> staffLeaveBalances { get; set; }
        public virtual ICollection<staffShift> staffShifts { get; set; }
    }
}