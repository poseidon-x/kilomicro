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
    
    public partial class loanDocumentTemplate
    {
        public loanDocumentTemplate()
        {
            this.loanDocumentTemplatePages = new HashSet<loanDocumentTemplatePage>();
        }
    
        public int loanDocumentTemplateId { get; set; }
        public string templateName { get; set; }
        public string creator { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
    
        public virtual ICollection<loanDocumentTemplatePage> loanDocumentTemplatePages { get; set; }
    }
}
