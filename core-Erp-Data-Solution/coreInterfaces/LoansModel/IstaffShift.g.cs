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
    
    public interface IstaffShift
    {
        int staffShiftID { get; set; }
        int staffID { get; set; }
        int yearID { get; set; }
        int shiftID { get; set; }
        System.DateTime startDate { get; set; }
        System.DateTime endDate { get; set; }
    
        Ishift shift { get; set; }
        Iyear year { get; set; }
        Istaff staff { get; set; }
    } 
    
}
