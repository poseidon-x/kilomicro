using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using coreLogic.Models;
using coreLogic.Models.Invoice;

namespace coreData.DataSources.Invoice
{
    [DataObject]
    public class InvoiceDataSource
    {
            private readonly ICommerceEntities le;
            private readonly Icore_dbEntities ctx;

        //call a constructor to instialize a the  context 
        public InvoiceDataSource()
        {
            var db2 = new CommerceEntities();
            var db3 = new core_dbEntities();

            le = db2;
            ctx = db3;

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InvoiceViewModel> GetInvoice(long customerId)
        {
            var todaysDate = DateTime.Now;
            var data = le.arInvoices
                .Where(p => p.customerId == customerId
                            && p.paid == false)
                .Select(p => new InvoiceViewModel
                {
                    invoiceId = p.arInvoiceId,
                    salesOrderId = p.salesOrderId,
                   }).OrderBy(p => p.invoiceId)
                .ToList();

            foreach (var record in data)
            {
                record.salesOrderShippingmethodId =
                    le.salesOrderShippings.FirstOrDefault(i => i.salesOrderId == record.salesOrderId).shippingMethodId;
                record.invoiceNumber = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).invoiceNumber;
                record.customerId = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).customerId;
                record.customerName = le.customers.FirstOrDefault(p => p.customerId == record.customerId).customerName;
                var invDate = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).invoiceDate;
                record.invoiceDate = string.Format("{0:dd/MMM/yyyy}", invDate);
                record.packingSlipDate = record.invoiceDate;
                record.invoiceCurrencyId = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).currencyId; 
                record.invoiceTotal = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).totalAmount;
                var salDate = le.salesOrders.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).salesDate;
                record.salesOrderDate = string.Format("{0:dd/MMM/yyyy}", salDate);
                record.salesOrderNumber = le.salesOrders.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).orderNumber;
                record.billTo = le.salesOrderBillings.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).billTo;
                record.billToAddressLine1 = le.salesOrderBillings.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).addressLine1;
                record.billToCity = le.salesOrderBillings.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).cityName;
                record.shipTo = le.salesOrderShippings.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).shipTo;
                record.shipToAddressLine1 = le.salesOrderShippings.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).addressLine1;
                record.shipToCity = le.salesOrderShippings.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).cityName;
                record.shipToCountry = le.salesOrderShippings.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).countryName;
                record.salesOrderShippingMethodName = le.shippingMethods.FirstOrDefault(p => p.shippingMethodID == record.salesOrderShippingmethodId).shippingMethodName;
                var salRequiredDate = le.salesOrders.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).requiredDate;
                record.salesOrderRequiredDate = string.Format("{0:dd/MMM/yyyy}", salRequiredDate);
                record.invoiceTotalAmount = le.salesOrders.FirstOrDefault(p => p.salesOrderId == record.salesOrderId).totalAmount;
                record.totalDiscount = 0.00;
                record.subTotal = 0.00;

                record.invoiceLines = GetInvoiceLines(record.invoiceId);

                foreach (var line in record.invoiceLines)
                {
                    record.totalDiscount += line.totalLineDiscount;
                    record.subTotal += line.lineSubTotal;
                }
                record.withholdingRatePercentage = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).withRate * 100;
                record.withholding = record.subTotal * le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).withRate;
                record.vatRatePercentage = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).vatRate * 100;
                record.vat = record.subTotal * le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).vatRate;
                record.nhilRatePercentage = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).nhilRate * 100;
                record.nhil = record.subTotal * le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId).nhilRate;
                record.totalDue = record.subTotal;
                record.amountPaid = 0.00;
                record.balanceDue = record.totalDue - record.amountPaid;

                record.companyProfileId = ctx.comp_prof.First().comp_prof_id;
                record.companyName = ctx.comp_prof.First().comp_name;
                record.customerWebsite = ctx.comp_prof.First().web;
                record.companyLogo = ctx.comp_prof.First().logo;
                record.companyAddressLine = ctx.comp_prof.First().addr_line_1;
                record.companyPhoneNumber = ctx.comp_prof.First().phon_num;
                record.companyEmail = ctx.comp_prof.First().email;
                record.companyCityId = ctx.comp_prof.First().city_id;
                record.companyCountryId = ctx.comp_prof.First().country_id;
                record.companyCity = ctx.cities.FirstOrDefault(p => p.city_id == record.companyCityId).city_name;
                record.companyCountry = ctx.countries.FirstOrDefault(p => p.country_id == record.companyCountryId).country_name;
                record.invoiceCurrency = ctx.currencies.FirstOrDefault(p => p.currency_id == record.invoiceCurrencyId).major_name;

            }


            return data;
        }




        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InvoiceLinesViewModel> GetInvoiceLines(long invoiceId)
        {
            var data = le.arInvoiceLines
                .Where(p => p.arInvoiceId == invoiceId)
                        .Select(p => new InvoiceLinesViewModel
                        {
                            invoiceLineId = p.arInvoiceLineId,
                            lineNumber = p.lineNumber,
                            description = p.description,
                            quantity = p.quantity,
                            inventoryItemId = le.arInvoiceLines.FirstOrDefault(i => i.arInvoiceId == p.arInvoiceId).inventoryItemId,
                            quantityAvialable = 0,
                            unitOfMeasurementId = p.unitOfMeasurementId,
                            unitOfMeasurementName = le.unitOfMeasurements.FirstOrDefault(i => i.unitOfMeasurementId == p.unitOfMeasurementId ).unitOfMeasurementName,
                            unitPrice = p.unitPrice,
                            lineTotalAmount = p.totalAmount,
                            totalLineDiscount = p.discountAmount,
                            lineSubTotal = p.netAmount,
                        }).OrderBy(p => p.invoiceLineId)
                        .ToList();
            
            //retrieve the quantity avialable for each inventory Item
            foreach (var record in data)
            {
                var inventoryItemData = le.inventoryItemDetails
                    .Where(p => p.inventoryItemId == record.inventoryItemId && p.quantityOnHand > 0)
                    .ToList();
                foreach (var row in inventoryItemData)
                {
                    record.quantityAvialable += row.quantityOnHand;
                }
                if (record.quantityAvialable >= record.quantity)
                {
                    record.shipQuantity = record.quantity;
                }
                else
                {
                    record.shipQuantity = record.quantityAvialable;
                    if (record.comments == null)
                    {
                        record.comments = "Backorder items will ship as they become avaialable";
                    }
                }


            }

            return data;
        }

        

    }
}

