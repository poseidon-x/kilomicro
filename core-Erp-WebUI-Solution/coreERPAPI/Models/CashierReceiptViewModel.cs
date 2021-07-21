using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class CashierReceiptViewModel
    {
        public cashierReceipt cashierReceipt { get; set; }
        //public loan loan { get; set; }
        public List<cashierReceiptNote> cashierReceiptNotes { get; set; }
        public List<cashierReceiptCoin> cashierReceiptCoins { get; set; }
    }

    public class cashierReceiptNote
    {
        public int cashierReceiptNoteId { get; set; }
        public int cashierReceiptID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityReceived { get; set; }
        public float totalReceived { get; set; }
    }

    public class cashierReceiptCoin
    {
        public int cashierReceiptCoinId { get; set; }
        public int cashierReceiptID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityReceived { get; set; }
        public float totalReceived { get; set; }
    }
}