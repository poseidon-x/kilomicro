using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Savings
{
    public class SavingsTermSheetData
    {
        public string clientName { get; set; }
        public string accountNumber { get; set; }
        public double planAmount { get; set; }
        public DateTime firstDepositDate { get; set; }
        public DateTime maturityDate { get; set; }
        public double rate { get; set; }
        public double tenure { get; set; }
        public double firstAmountInvested { get; set; } 
        public string frequency { get; set; }
        public DateTime plannedDate { get; set; }
        public double plannedAmount { get; set; }
        public byte[] companyLogo { get; set; }
        public string companyAddress { get; set; }
        public string companyPhone { get; set; }
    }
}
