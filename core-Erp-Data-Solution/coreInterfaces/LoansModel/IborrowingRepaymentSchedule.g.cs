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
    
    public interface IborrowingRepaymentSchedule
    {
        int borrowingRepaymentScheduleId { get; set; }
        int borrowingId { get; set; }
        System.DateTime repaymentDate { get; set; }
        double interestPayment { get; set; }
        double principalPayment { get; set; }
        double interestBalance { get; set; }
        double principalBalance { get; set; }
        double balanceBF { get; set; }
        double balanceCD { get; set; }
        bool edited { get; set; }
        Nullable<double> originalInterestPayment { get; set; }
        Nullable<double> originalPrincipalPayment { get; set; }
        System.DateTime created { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modified { get; set; }
        string modifier { get; set; }
    
        Iborrowing borrowing { get; set; }
    } 
    
}
