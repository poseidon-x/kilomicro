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
    
    public partial class interestType
    {
        public interestType()
        {
            this.borrowings = new HashSet<borrowing>();
            this.loans = new HashSet<loan>();
        }
    
        public int interestTypeID { get; set; }
        public string interestTypeName { get; set; }
    
        public virtual ICollection<borrowing> borrowings { get; set; }
        public virtual ICollection<loan> loans { get; set; }
    }
}
