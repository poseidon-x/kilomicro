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
    
    public interface IborrowingDocument
    {
        int borrowingDocumentId { get; set; }
        int documentId { get; set; }
        int borrowingId { get; set; }
        byte[] version { get; set; }
    
        Iborrowing borrowing { get; set; }
        Idocument document { get; set; }
    } 
    
}