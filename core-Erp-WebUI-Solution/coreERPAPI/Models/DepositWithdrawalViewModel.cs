using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class DepositWithdrawalViewModel
    {
        public depositWithdrawal depositWithdrawal { get; set; }
        public deposit deposit { get; set; }
        public char withdrawalType { get; set; }
        public bool isDisInvstment { get; set; }
        public double disInvstmentCharge { get; set; }
        public List<depositWithdrawalNote> depositWithdrawalNotes { get; set; }
        public List<depositWithdrawalCoin> depositWithdrawalCoins { get; set; }
    }

    public class depositWithdrawalNote
    {
        public int depositWithdrawalNoteId { get; set; }
        public int depositWithdrawalID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityWithdrawn { get; set; }
        public float totalWithdrawn { get; set; }
    }

    public class depositWithdrawalCoin
    {
        public int depositWithdrawalCoinId { get; set; }
        public int depositWithdrawalID { get; set; }
        public int currencyNoteId { get; set; }
        public int quantityBD { get; set; }
        public int quantityCD { get; set; }
        public int quantityWithdrawn { get; set; }
        public float totalWithdrawn { get; set; }
    }
}