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
    
    public interface IrepaymentMode
    {
        int repaymentModeID { get; set; }
        string repaymentModeName { get; set; }
    
        ICollection<IloanFinancial> loanFinancials { get; set; }
        ICollection<Iborrowing> borrowings { get; set; }
        ICollection<Iloan> loans { get; set; }
    } 
    
}