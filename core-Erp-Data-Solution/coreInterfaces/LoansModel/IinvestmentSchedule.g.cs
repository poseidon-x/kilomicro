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
    
    public interface IinvestmentSchedule
    {
        int investmentScheduleID { get; set; }
        int investmentID { get; set; }
        double interestPayment { get; set; }
        double principalPayment { get; set; }
        System.DateTime repaymentDate { get; set; }
        bool authorized { get; set; }
        bool expensed { get; set; }
        bool temp { get; set; }
    
        Iinvestment investment { get; set; }
    } 
    
}