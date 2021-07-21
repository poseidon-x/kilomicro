using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models.Deposit
{
    public class DepositInterestUpgradeVM 
    {
        public int depositRateUpgradeId { get; set; }
        public int depositId { get; set; }
        public string client { get; set; }
        public string depositNo { get; set; }
        public double currentPrincipalBalance { get; set; }
        public double currentRate { get; set; }
        public double proposedRate { get; set; }
        public bool approved { get; set; }
    }

 
}