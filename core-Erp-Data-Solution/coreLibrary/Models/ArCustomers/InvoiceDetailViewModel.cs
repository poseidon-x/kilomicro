using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.NumberGenerator
{
    public class InvoiceDetailViewModel
    {
        public long? arInvoiceId { get; set; }
        public string invoiceNumber { get; set; }
        public double totalTotal { get; set; }
        public double balance { get; set; }
        public string invoiceNumberNBalance { get; set; }

    }
}
