using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace agencyAPI.Models
{
    public class savingWithdrawalViewModel : savingWithdrawal
    {
        public string withType { get; set; }
        public double withdrawalAmount { get; set; }

    }
}