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
    
    public interface IloanIncentive
    {
        int loanIncentiveID { get; set; }
        int loanID { get; set; }
        int agentID { get; set; }
        double loanAmount { get; set; }
        double incentiveAmount { get; set; }
        bool posted { get; set; }
        Nullable<System.DateTime> incetiveDate { get; set; }
        Nullable<System.DateTime> postedDate { get; set; }
        Nullable<int> modeOfPaymentID { get; set; }
        Nullable<int> bankID { get; set; }
        string checkNo { get; set; }
        Nullable<System.DateTime> creation_date { get; set; }
        string creator { get; set; }
        bool approved { get; set; }
        double withHoldingAmount { get; set; }
        double commissionAmount { get; set; }
        bool paid { get; set; }
        Nullable<System.DateTime> paidDate { get; set; }
        bool commPaid { get; set; }
        Nullable<System.DateTime> commPaidDate { get; set; }
        bool commPosted { get; set; }
        Nullable<System.DateTime> commPostedDate { get; set; }
        double netCommission { get; set; }
        byte[] version { get; set; }
    
        Iagent agent { get; set; }
        Iloan loan { get; set; }
    } 
    
}