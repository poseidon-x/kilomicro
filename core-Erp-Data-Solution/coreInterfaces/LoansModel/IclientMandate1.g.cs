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
    
    public interface IclientMandate1
    {
        int clientMandateId { get; set; }
        int clientId { get; set; }
        string authorizeName { get; set; }
        int idNumber { get; set; }
        int idNoTypeID { get; set; }
        byte[] photoImage { get; set; }
        byte[] signatureImage { get; set; }
        int mandateTypeId { get; set; }
        int clientMandateTypeId { get; set; }
    
        Iclient client { get; set; }
        IclientMandateType clientMandateType { get; set; }
        IidNoType idNoType { get; set; }
    } 
    
}
