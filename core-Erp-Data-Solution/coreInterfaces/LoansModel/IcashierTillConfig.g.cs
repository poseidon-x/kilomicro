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
    
    public interface IcashierTillConfig
    {
        int cashierTillConfigId { get; set; }
        System.DateTime opendate { get; set; }
        bool open { get; set; }
    
        ICollection<IcashierTillConfigDetail> cashierTillConfigDetails { get; set; }
    } 
    
}