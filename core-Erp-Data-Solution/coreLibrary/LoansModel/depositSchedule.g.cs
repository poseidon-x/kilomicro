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
    
    public partial class depositSchedule
    {
        public int depositScheduleID { get; set; }
        public int depositID { get; set; }
        public double interestPayment { get; set; }
        public double principalPayment { get; set; }
        public System.DateTime repaymentDate { get; set; }
        public bool authorized { get; set; }
        public bool expensed { get; set; }
        public bool temp { get; set; }
        public byte[] version { get; set; }
    
        public virtual deposit deposit { get; set; }
    }
}
