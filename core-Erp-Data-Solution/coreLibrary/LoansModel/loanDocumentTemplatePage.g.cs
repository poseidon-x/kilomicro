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
    
    public partial class loanDocumentTemplatePage
    {
        public loanDocumentTemplatePage()
        {
            this.loanDocumentTemplatePagePlaceHolders = new HashSet<loanDocumentTemplatePagePlaceHolder>();
        }
    
        public int loanDocumentTemplatePageId { get; set; }
        public int loanDocumentTemplateId { get; set; }
        public int pageNumber { get; set; }
        public string content { get; set; }
        public Nullable<bool> isNew { get; set; }
    
        public virtual loanDocumentTemplate loanDocumentTemplate { get; set; }
        public virtual ICollection<loanDocumentTemplatePagePlaceHolder> loanDocumentTemplatePagePlaceHolders { get; set; }
    }
}