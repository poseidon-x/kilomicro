using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models.Deposit
{
    public class SavingViewModel : saving
    {
        public List<savingNote> savingNotes { get; set; }
        public List<savingCoin> savingCoins { get; set; }
    }

    public class savingNote
    {
        public int savingNoteId { get; set; }
        public int savingID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityDeposited { get; set; }
        public float totalDeposited { get; set; }
    }

    public class savingCoin
    {
        public int savingCoinId { get; set; }
        public int savingID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityDeposited { get; set; }
        public float totalDeposited { get; set; }
    }
}