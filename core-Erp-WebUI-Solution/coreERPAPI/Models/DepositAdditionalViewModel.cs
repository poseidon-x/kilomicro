using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class DepositAdditionalViewModel
    {
        public depositAdditional depositAdditional { get; set; }
        public deposit deposit { get; set; }
        public List<depositAdditionalNote> depositAdditionalNotes { get; set; }
        public List<depositAdditionalCoin> depositAdditionalCoins { get; set; }
    }

    public class depositAdditionalNote
    {
        public int depositAdditionalNoteId { get; set; }
        public int depositAdditionalID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityDeposited { get; set; }
        public float totalDeposited { get; set; }
    }

    public class depositAdditionalCoin
    {
        public int depositAdditionalCoinId { get; set; }
        public int depositAdditionalID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityDeposited { get; set; }
        public float totalDeposited { get; set; }
    }
}