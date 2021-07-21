using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreErpPointOfSale.Models;
using coreLogic;

namespace coreErpPointOfSale.ViewModel
{
    public class SalesReceiptViewModel
    {
        public static ObservableCollection<Sales> SetReceiptGrid(arInvoice invoice)
        {
            ObservableCollection<Sales> sales = new ObservableCollection<Sales>();

            foreach (var record in invoice.arInvoiceLines)
            {
                var item = new Sales(record);
                sales.Add(item);
            }
            var total = new Sales("Total Amount", "", "", invoice.totalAmount);
            sales.Add(total);

            if (invoice.isNHIL && invoice.isVat)
            {
                var unitPrice = invoice.totalAmount * (invoice.vatRate + invoice.nhilRate);
                var item = new Sales("VAT & NHIL", "", "", unitPrice);
                sales.Add(item);
            }
            if (invoice.isWith)
            {
                var unitPrice = invoice.totalAmount * (invoice.withRate);
                var item = new Sales("WITHHOLDING TAX", "", "", unitPrice);
                sales.Add(item);
            }
            var cashTendered = new Sales("Cash Tendered", "", "", invoice.totalAmount);
            sales.Add(cashTendered);

            return sales;            
        }

    }
}
