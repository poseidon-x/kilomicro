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
    
    public interface ImaritalStatu
    {
        int maritalStatusID { get; set; }
        string maritalStatusName { get; set; }
    
        ICollection<Iclient> clients { get; set; }
        ICollection<Istaff> staffs { get; set; }
    } 
    
}