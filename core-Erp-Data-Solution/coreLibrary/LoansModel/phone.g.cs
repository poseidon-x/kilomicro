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
    
    public partial class phone
    {
        public phone()
        {
            this.staffPhones = new HashSet<staffPhone>();
            this.agentNextOfKins = new HashSet<agentNextOfKin>();
            this.agentPhones = new HashSet<agentPhone>();
            this.clientCompanies = new HashSet<clientCompany>();
            this.clientPhones = new HashSet<clientPhone>();
            this.employerDirectors = new HashSet<employerDirector>();
            this.groupExecs = new HashSet<groupExec>();
            this.loanGurantors = new HashSet<loanGurantor>();
            this.nextOfKins = new HashSet<nextOfKin>();
            this.smeDirectors = new HashSet<smeDirector>();
        }
    
        public int phoneID { get; set; }
        public Nullable<int> phoneTypeID { get; set; }
        public string phoneNo { get; set; }
        public byte[] version { get; set; }
    
        public virtual ICollection<staffPhone> staffPhones { get; set; }
        public virtual ICollection<agentNextOfKin> agentNextOfKins { get; set; }
        public virtual ICollection<agentPhone> agentPhones { get; set; }
        public virtual ICollection<clientCompany> clientCompanies { get; set; }
        public virtual ICollection<clientPhone> clientPhones { get; set; }
        public virtual ICollection<employerDirector> employerDirectors { get; set; }
        public virtual ICollection<groupExec> groupExecs { get; set; }
        public virtual ICollection<loanGurantor> loanGurantors { get; set; }
        public virtual ICollection<nextOfKin> nextOfKins { get; set; }
        public virtual phoneType phoneType { get; set; }
        public virtual ICollection<smeDirector> smeDirectors { get; set; }
    }
}