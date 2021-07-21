using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class savingWithdrawalViewModel
    {
        public string withType { get; set; }
        public double withdrawalAmount { get; set; }
        public savingWithdrawal savingWithdrawal { get; set; }
        public saving saving { get; set; }
        public List<savingWithdrawalNote> savingWithdrawalNotes { get; set; }
        public List<savingWithdrawalCoin> savingWithdrawalCoins { get; set; }

    }

    public class savingWithdrawalNote
    {
        public int savingWithdrawalNoteId { get; set; }
        public int savingWithdrawalID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityCD { get; set; }
        public int quantityWithdrawn { get; set; }
        public int quantityBD { get; set; }
        public float totalWithdrawn { get; set; }

    }

    public class savingWithdrawalCoin
    {
        public int savingWithdrawalCoinId { get; set; }
        public int savingWithdrawalID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityCD { get; set; }
        public int quantityWithdrawn { get; set; }
        public int quantityBD { get; set; }
        public float totalWithdrawn { get; set; }
    }
}