//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic.Designer
{
    using System;
    using System.Collections.Generic;
    
    public partial class phone
    {
        public phone()
        {
            this.clientPhones = new HashSet<clientPhone>();
            this.loanGurantors = new HashSet<loanGurantor>();
            this.employerDirectors = new HashSet<employerDirector>();
            this.smeDirectors = new HashSet<smeDirector>();
            this.staffPhones = new HashSet<staffPhone>();
            this.groupExecs = new HashSet<groupExec>();
            this.nextOfKins = new HashSet<nextOfKin>();
            this.agentPhones = new HashSet<agentPhone>();
            this.agentNextOfKins = new HashSet<agentNextOfKin>();
            this.clientCompanies = new HashSet<clientCompany>();
        }
    
        public int phoneID { get; set; }
        public Nullable<int> phoneTypeID { get; set; }
        public string phoneNo { get; set; }
        public byte[] version { get; set; }
    
        public virtual ICollection<clientPhone> clientPhones { get; set; }
        public virtual ICollection<loanGurantor> loanGurantors { get; set; }
        public virtual phoneType phoneType { get; set; }
        public virtual ICollection<employerDirector> employerDirectors { get; set; }
        public virtual ICollection<smeDirector> smeDirectors { get; set; }
        public virtual ICollection<staffPhone> staffPhones { get; set; }
        public virtual ICollection<groupExec> groupExecs { get; set; }
        public virtual ICollection<nextOfKin> nextOfKins { get; set; }
        public virtual ICollection<agentPhone> agentPhones { get; set; }
        public virtual ICollection<agentNextOfKin> agentNextOfKins { get; set; }
        public virtual ICollection<clientCompany> clientCompanies { get; set; }
    }
}