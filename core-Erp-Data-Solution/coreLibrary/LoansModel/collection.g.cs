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
    
    public partial class collection
    {
        public int collectionID { get; set; }
        public int loanProductID { get; set; }
        public int month { get; set; }
        public double collection1 { get; set; }
    
        public virtual loanProduct loanProduct { get; set; }
    }
}