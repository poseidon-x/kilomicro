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
    
     // Condition: ^[^v].*Type$
    public interface IidNoType
    {
        int idNoTypeID { get; set; }
        string idNoTypeName { get; set; }
        bool isNational { get; set; }
    
        ICollection<IidNo> idNoes { get; set; }
        ICollection<IsavingNextOfKin> savingNextOfKins { get; set; }
        ICollection<IdepositNextOfKin> depositNextOfKins { get; set; }
    } 
    
}