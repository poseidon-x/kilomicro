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
    
    public interface IpaymentTerm
    {
        int paymentTermID { get; set; }
        string paymentTerms { get; set; }
        int netDays { get; set; }
        int discountIfBeforeDays { get; set; }
        double discountPercent { get; set; }
    
        ICollection<IarInvoice> arInvoices { get; set; }
    } 
}
