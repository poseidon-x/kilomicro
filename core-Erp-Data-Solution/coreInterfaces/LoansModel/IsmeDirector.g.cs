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
    
    public interface IsmeDirector
    {
        int smeDirectorID { get; set; }
        Nullable<int> smeCategoryID { get; set; }
        Nullable<int> imageID { get; set; }
        Nullable<int> phoneID { get; set; }
        Nullable<int> emailID { get; set; }
        string surName { get; set; }
        string otherNames { get; set; }
        Nullable<int> idNoID { get; set; }
        byte[] version { get; set; }
    
        Iemail email { get; set; }
        IidNo idNo { get; set; }
        Iphone phone { get; set; }
        IsmeCategory smeCategory { get; set; }
        Iimage image { get; set; }
    } 
    
}