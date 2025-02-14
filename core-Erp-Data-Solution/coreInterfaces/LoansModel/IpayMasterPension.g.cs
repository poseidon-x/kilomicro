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
    
    public interface IpayMasterPension
    {
        int payMasterPensionID { get; set; }
        int payMasterID { get; set; }
        int pensionTypeID { get; set; }
        double employeeAmount { get; set; }
        double employerAmount { get; set; }
        bool isPercent { get; set; }
        bool isBeforeTax { get; set; }
        string description { get; set; }
        byte[] version { get; set; }
    
        IpayMaster payMaster { get; set; }
        IpensionType pensionType { get; set; }
    } 
    
}
