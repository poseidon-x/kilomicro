using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models.CashierTransaction
{
    public class CTransViewModel
    {
        public string cashierName { get; set; }
        public string transactionAccount { get; set; }
        public DateTime fundDate { get; set; }
        public double fundAmount { get; set; }
        public double fundBalance { get; set; }
        public List<cashierTransactionReceipt> cashierReceivals { get; set; }
        public List<cashierTransactionWithdrawal> cashierWithdrawals { get; set; }
    }
}