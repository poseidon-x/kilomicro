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
    
    public interface Iconfig
    {
        int configId { get; set; }
        bool postInterestUnIntOnDisb { get; set; }
        bool securityDepositEnabled { get; set; }
        bool transactionalBankingEnabled { get; set; }
        int loanPenaltyGracePeriodInDays { get; set; }
    } 
    
}
