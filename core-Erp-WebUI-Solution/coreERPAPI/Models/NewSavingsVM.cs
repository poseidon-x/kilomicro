using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class NewSavingsVM
    {
        public string savingNo{ get; set; }
        public int savingID { get; set; }
        public double principalBalance { get; set; }
        public int clientID { get; set; }
        public double savingPlanAmount { get; set; }
        public double interestBalance { get; set; }
        public string clientName { get; set; }
        public string clientAccNum { get; set; }
        public bool paid { get; set; }
        public string Branch { get; set; }
        public int? branchID { get; set; }
    }
}