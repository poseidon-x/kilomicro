using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class CashierFundingViewModel
    {
        public DateTime fundingDate { get; set; }
        public IEnumerable<cashierFund> cashierFunds { get; set; }
        public IEnumerable<cashierCashup> cashierCashups { get; set; }
        public List<cashiersTill> cashierTills { get; set; }

    }
}