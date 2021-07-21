using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLibrary;
using coreLogic;
using coreLogic.Models;
using coreLogic.Models.Borrowing;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Inventory;

namespace coreLogic.Models
{
    public class DepositCertificateViewModel
    {
        public CompanyProfileViewModel compamy { get; set; }
        public string depositNumber { get; set; }
        public string clientName { get; set; }
        public double depositAmount { get; set; }
        public string amountInWords { get; set; }
        public string interestRate { get; set; }
        public double interestExpected { get; set; }
        public string depositPeriod { get; set; }
        public string depositPeriodInMonths { get; set; }
        public string depositType { get; set; }
        public string depositDate { get; set; }
        public string maturityDate { get; set; }
        public double maturitySum { get; set; }
        public string earlyRedemptionText { get; set; }
        public string trustText { get; set; }
        public string authorityText { get; set; }
        public string riskDisclosureText { get; set; }

    }
}
