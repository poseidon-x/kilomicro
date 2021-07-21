using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models.Deposit
{
    public class DepositViewModel: deposit
    {
        public int clientInvestmentReceiptDetailId { get; set; }
        public List<depositNote> depositNotes { get; set; }
        public List<depositCoin> depositCoins { get; set; }
    }

    public class depositNote
    {
        public int depositNoteId { get; set; }
        public int depositID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityDeposited { get; set; }
        public float totalDeposited { get; set; }
    }

    public class depositCoin
    {
        public int depositCoinId { get; set; }
        public int depositID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityDeposited { get; set; }
        public float totalDeposited { get; set; }
    }
}