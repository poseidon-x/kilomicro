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
    
    public interface IagentNextOfKin
    {
        int nextOfKinID { get; set; }
        int agentID { get; set; }
        Nullable<int> phoneID { get; set; }
        Nullable<int> emailID { get; set; }
        string surName { get; set; }
        string otherNames { get; set; }
        string relationship { get; set; }
        Nullable<int> idNoID { get; set; }
        Nullable<int> imageID { get; set; }
        byte[] version { get; set; }
    
        Iagent agent { get; set; }
        Iemail email { get; set; }
        Iphone phone { get; set; }
        IidNo idNo { get; set; }
        Iimage image { get; set; }
    } 
    
}