using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models.Deposit
{
    public class InterCashierTransferViewModel
    {
        public cashierFundsTransfer cashierFundsTransfer { get; set; }
        public List<cashierTransferNote> transferNotes { get; set; }
        public List<cashierTransferCoin> transferCoins { get; set; }
    }

    public class cashierTransferNote
    {
        public int cashierTransferNoteId { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityWithdrawn { get; set; }
        public float totalWithdrawn { get; set; }
    }

    public class cashierTransferCoin
    {
        public int cashierTransferCoinId { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityWithdrawn { get; set; }
        public float totalWithdrawn { get; set; }
    }
}