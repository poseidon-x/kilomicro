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
    
    public interface IloanProductHistory
    {
        int loanProductHistoryID { get; set; }
        Nullable<int> loanProductID { get; set; }
        string loanProductName { get; set; }
        double loanTenure { get; set; }
        double rate { get; set; }
        Nullable<System.DateTime> archiveDate { get; set; }
        double minAge { get; set; }
        double maxAge { get; set; }
    
        IloanProduct loanProduct { get; set; }
    } 
    
}
