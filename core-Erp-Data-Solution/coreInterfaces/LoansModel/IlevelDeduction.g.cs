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
    
    public interface IlevelDeduction
    {
        int levelDeductionID { get; set; }
        int levelID { get; set; }
        int deductionTypeID { get; set; }
    
        IdeductionType deductionType { get; set; }
        Ilevel level { get; set; }
    } 
    
}
