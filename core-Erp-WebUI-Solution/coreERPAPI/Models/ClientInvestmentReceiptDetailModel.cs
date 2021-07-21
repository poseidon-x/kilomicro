using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class ClientInvestmentReceiptDetailModel : clientInvestmentReceiptDetail
    {
        public int clientId { get; set; }
    }
}