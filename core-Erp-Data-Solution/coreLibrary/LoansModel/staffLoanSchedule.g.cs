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
    
    public partial class staffLoanSchedule
    {
        public int staffLoanScheduleID { get; set; }
        public int staffLoanID { get; set; }
        public double principalDeduction { get; set; }
        public double interestDeduction { get; set; }
        public System.DateTime deductionDate { get; set; }
        public double balanceAfter { get; set; }
        public byte[] version { get; set; }
    
        public virtual staffLoan staffLoan { get; set; }
    }
}
