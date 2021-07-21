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
    public class InvoiceViewModel
    {
        public static ObservableCollection<SalesItems> SetInvoiceGrid(salesOrder sale)
        {
            ObservableCollection<SalesItems> sales = new ObservableCollection<SalesItems>();
            
            foreach (var record in sale.salesOrderlines)
            {
                var item = new SalesItems(record);
                sales.Add(item);
            }
            return sales;            
        }

    }
}
