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
    
    public interface IwebAppEmp
    {
        int webAppID { get; set; }
        Nullable<int> employerID { get; set; }
        Nullable<int> departmentID { get; set; }
        string authOfficerName { get; set; }
        string authOfficerPosition { get; set; }
        string authOfficerPhone { get; set; }
        Nullable<int> contractTypeID { get; set; }
    
        IwebApp webApp { get; set; }
    } 
}
