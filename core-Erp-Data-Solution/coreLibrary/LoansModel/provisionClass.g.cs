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
    
    public partial class provisionClass
    {
        public provisionClass()
        {
            this.loanProvisions = new HashSet<loanProvision>();
        }
    
        public int provisionClassID { get; set; }
        public string provisionClassName { get; set; }
        public int minDays { get; set; }
        public int maxDays { get; set; }
        public double provisionPercent { get; set; }
    
        public virtual ICollection<loanProvision> loanProvisions { get; set; }
    }
}