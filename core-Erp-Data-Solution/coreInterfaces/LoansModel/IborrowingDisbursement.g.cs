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
    
    public interface IborrowingDisbursement
    {
        int borrowingDisbursementId { get; set; }
        Nullable<int> borrowingId { get; set; }
        System.DateTime dateDisbursed { get; set; }
        double amountDisbursed { get; set; }
        Nullable<int> modeOfPaymentId { get; set; }
        Nullable<int> bankId { get; set; }
        string chequeNumber { get; set; }
    
        Iborrowing borrowing { get; set; }
        ImodeOfPayment modeOfPayment { get; set; }
    } 
    
}
