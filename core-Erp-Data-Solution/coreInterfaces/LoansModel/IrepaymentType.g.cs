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
    
     // Condition: ^[^v].*Type$
    public interface IrepaymentType
    {
        int repaymentTypeID { get; set; }
        string repaymentTypeName { get; set; }
    
        ICollection<IloanRepayment> loanRepayments { get; set; }
        ICollection<IborrowingRepayment> borrowingRepayments { get; set; }
    } 
    
}