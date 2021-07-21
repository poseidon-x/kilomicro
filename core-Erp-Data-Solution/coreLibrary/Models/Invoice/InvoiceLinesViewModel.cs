using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Invoice
{
    public class InvoiceLinesViewModel
    {
        public long invoiceLineId { get; set; }
        public int lineNumber { get; set; }
        public string description { get; set; }
        public double quantity { get; set; }
        public double inventoryItemId { get; set; }
        public double? quantityAvialable { get; set; }
        public double? shipQuantity { get; set; }  
        public long unitOfMeasurementId { get; set; }
        public string unitOfMeasurementName { get; set; }
        public double unitPrice { get; set; }
        public double lineTotalAmount { get; set; }
        public double totalLineDiscount { get; set; }
        public double lineSubTotal { get; set; }
        public string comments { get; set; }

      
    }
}
