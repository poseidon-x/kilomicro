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
    
    public partial class getLoanCashflow_Result
    {
        public int clientID { get; set; }
        public int branchID { get; set; }
        public string branchname { get; set; }
        public string clientName { get; set; }
        public System.DateTime month { get; set; }
        public double principalExpected { get; set; }
        public double principalReceived { get; set; }
        public double interestExpected { get; set; }
        public double interestReceived { get; set; }
        public double depositPrincipalPayable { get; set; }
        public double depositPrincipalPaid { get; set; }
        public double depositInterestPayable { get; set; }
        public double depositInterestPaid { get; set; }
    }
}