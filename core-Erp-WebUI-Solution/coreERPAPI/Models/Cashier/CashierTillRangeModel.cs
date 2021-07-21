using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.Cashier
{
    public class CashierTillRangeModel
    {
        public int cashierTillId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}