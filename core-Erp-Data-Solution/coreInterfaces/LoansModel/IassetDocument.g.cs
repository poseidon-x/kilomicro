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
    
    public interface IassetDocument
    {
        int assetDocumentID { get; set; }
        int documentID { get; set; }
        int assetID { get; set; }
        byte[] version { get; set; }
    
        Iasset asset { get; set; }
        Idocument document { get; set; }
    } 
    
}