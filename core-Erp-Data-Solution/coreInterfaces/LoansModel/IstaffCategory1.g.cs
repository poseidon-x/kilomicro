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
    
    public interface IstaffCategory1
    {
        int staffCategoryID { get; set; }
        int clientID { get; set; }
        Nullable<int> employerID { get; set; }
        string ssn { get; set; }
        string employeeNumber { get; set; }
        Nullable<System.DateTime> employmentStartDate { get; set; }
        double lengthOfService { get; set; }
        string position { get; set; }
        Nullable<int> employeeContractTypeID { get; set; }
        Nullable<int> employerDepartmentID { get; set; }
        string employeeNumberOld { get; set; }
        Nullable<int> regionID { get; set; }
        string empAddress1 { get; set; }
        string empAddress2 { get; set; }
        string empAddressCity { get; set; }
        string authOfficerName { get; set; }
        string authOfficerPosition { get; set; }
        string authOfficerPhone { get; set; }
        byte[] version { get; set; }
    
        Iclient client { get; set; }
        IemployeeContractType employeeContractType { get; set; }
        Iemployer employer { get; set; }
        IemployerDepartment employerDepartment { get; set; }
        Iregion region { get; set; }
        ICollection<IstaffCategoryDirector> staffCategoryDirectors { get; set; }
    } 
    
}
