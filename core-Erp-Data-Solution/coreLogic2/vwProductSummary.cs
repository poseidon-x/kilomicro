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
    
    public partial class vwProductSummary
    {
        public string productName { get; set; }
        public string productCategory { get; set; }
        public Nullable<int> allAccounts { get; set; }
        public Nullable<int> distinctAccount { get; set; }
        public Nullable<int> allActiveAccounts { get; set; }
        public Nullable<int> distinctActiveAccounts { get; set; }
        public Nullable<double> totalPrincipal { get; set; }
        public Nullable<double> totalInterest { get; set; }
        public Nullable<double> remainingBalance { get; set; }
        public Nullable<double> principalBalance { get; set; }
    }
}
