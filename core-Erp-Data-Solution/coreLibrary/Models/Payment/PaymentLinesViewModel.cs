using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Payment
{
    public class PaymentLinesViewModel
    {
        public long paymentLineId { get; set; }
        public int lineNumber { get; set; }
        public long invoiceId { get; set; }
        public string invoiceNumber { get; set; }
        public string invoiceDate { get; set; }
        public double invoiceTotal { get; set; }
        public double invoiceBalance { get; set; }
        public double amountPaid { get; set; }
      
    }
}
