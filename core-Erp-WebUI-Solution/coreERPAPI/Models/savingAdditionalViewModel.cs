using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class savingAdditionalViewModel
    {
        public savingAdditional savingAdditional { get; set; }
        public saving saving { get; set; }
        public List<savingAdditionalNote> savingAdditionalNotes { get; set; }
        public List<savingAdditionalCoin> savingAdditionalCoins { get; set; }
    }

    public class savingAdditionalNote
    {
        public int savingAdditionalNoteId { get; set; }
        public int savingAdditionalID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityDeposited { get; set; }
        public float totalDeposited { get; set; }
    }

    public class savingAdditionalCoin
    {
        public int savingAdditionalCoinId { get; set; }
        public int savingAdditionalID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityDeposited { get; set; }
        public float totalDeposited { get; set; }
    }
}