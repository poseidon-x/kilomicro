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
    
    public interface IwebAppEmp2
    {
        int webAppID { get; set; }
        string employeeNumber { get; set; }
        string oldEmployeeNumber { get; set; }
        string socialSecurityNo { get; set; }
        string position { get; set; }
        Nullable<System.DateTime> employmentStartDate { get; set; }
        string empAddr1 { get; set; }
        string empAddr2 { get; set; }
        string empAddrCity { get; set; }
    
        IwebApp webApp { get; set; }
    } 
}
