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
    
    public partial class loanApprovalStageOfficer
    {
        public int loanApprovalStageOfficerId { get; set; }
        public int loanApprovalStageId { get; set; }
        public string profileType { get; set; }
        public string profileValue { get; set; }
    
        public virtual loanApprovalStage loanApprovalStage { get; set; }
    }
}