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
    
    public interface IstaffQualification
    {
        int staffQualificationID { get; set; }
        int staffID { get; set; }
        int qualificationTypeID { get; set; }
        int qualificationSubjectID { get; set; }
        Nullable<System.DateTime> startDate { get; set; }
        Nullable<System.DateTime> endDate { get; set; }
        Nullable<System.DateTime> expiryDate { get; set; }
        string institutionName { get; set; }
        byte[] version { get; set; }
    
        IqualificationSubject qualificationSubject { get; set; }
        IqualificationType qualificationType { get; set; }
        Istaff staff { get; set; }
    } 
    
}