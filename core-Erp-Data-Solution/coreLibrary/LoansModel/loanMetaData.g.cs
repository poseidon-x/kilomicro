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
    
    public partial class loanMetaData
    {
        public int loanMetaDataId { get; set; }
        public int loanAdditionalInfoId { get; set; }
        public int metaDataTypeId { get; set; }
        public string content { get; set; }
    
        public virtual loanAdditionalInfo loanAdditionalInfo { get; set; }
        public virtual metaDataType metaDataType { get; set; }
    }
}
