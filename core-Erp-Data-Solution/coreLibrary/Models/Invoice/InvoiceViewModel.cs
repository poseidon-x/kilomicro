using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Invoice
{
    public class InvoiceViewModel
    {
        public int companyProfileId { get; set; }
        public string companyName { get; set; }
        public byte[] companyLogo { get; set; }
        public string companyAddressLine { get; set; }
        public string companyPhoneNumber { get; set; }
        public string companyEmail { get; set; }
        public int? companyCityId { get; set; }
        public string companyCity { get; set; }
        public int? companyCountryId { get; set; }
        public string companyCountry { get; set; }
        public long invoiceId { get; set; }
        public string invoiceNumber { get; set; }
        public long? customerId { get; set; }
        public string customerName { get; set; }
        public string customerWebsite { get; set; }
        public string invoiceDate { get; set; }
        public int invoiceCurrencyId { get; set; }
        public string invoiceCurrency { get; set; }
        public double invoiceTotal { get; set; }
        public long? salesOrderId { get; set; }
        public string salesOrderDate { get; set; }
        public string salesOrderNumber { get; set; }
        public string billTo { get; set; }
        public string billToAddressLine1 { get; set; }
        public string billToCity { get; set; }
        public string shipTo { get; set; }
        public string shipToAddressLine1 { get; set; }
        public string shipToCity { get; set; }
        public string shipToCountry { get; set; }
        public long salesOrderShippingmethodId { get; set; }
        public string salesOrderShippingMethodName { get; set; }
        public string salesOrderRequiredDate { get; set; }
        public double invoiceTotalAmount { get; set; }
        public double totalDiscount { get; set; }
        public double subTotal { get; set; }
        public double withholdingRatePercentage { get; set; }
        public double withholding { get; set; }
        public double vatRatePercentage { get; set; }
        public double vat { get; set; }
        public double nhilRatePercentage { get; set; }
        public double nhil { get; set; }
        public double totalDue { get; set; }
        public double amountPaid { get; set; }
        public double balanceDue { get; set; }
        public string packingSlipDate { get; set; }



        public List<InvoiceLinesViewModel> invoiceLines { get; set; }


        
    }
}
