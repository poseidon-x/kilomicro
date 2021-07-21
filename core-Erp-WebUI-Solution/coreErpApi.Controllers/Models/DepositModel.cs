using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class DepositModel:deposit
    {
        public int paymentModeId { get; set; }
        public int? bankId { get; set; }
        public string chequeNumber { get; set; }
        public string naration { get; set; }

    }
}