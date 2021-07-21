using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models.CashierTransaction
{
    public class AllCTransViewModel
    {
        public int transTypeID { get; set; }
        public int transID { get; set; }
        public string accountNo { get; set; }
    }
}