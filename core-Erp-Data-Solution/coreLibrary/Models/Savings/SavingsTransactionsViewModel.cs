using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic.Models.Inventory;

namespace coreLogic.Models
{
    public class SavingsTransactionsViewModel
    {
        /*
         * StockTransactionsViewModel properties
         */

        public int lineNo { get; set; }
        public string transactionDate { get; set; }
        public DateTime date { get; set; }
        public DateTime? creationDate { get; set; }
        public string description { get; set; }
        public double withdrawalAmount { get; set; }
        public double depositAmount { get; set; }
        public string strDepositAmount { get; set; }
        public string strWithdrawalAmount { get; set; }
        public double balance { get; set; }

    }
}
