using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLibrary;
using coreLogic.Models.Borrowing;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Inventory;

namespace coreLogic.Models
{
    public class SavingsStatementViewModel
    {
        public client clientDetail { get; set; }

        public CompanyProfileViewModel compamy { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public double balanceAtStart { get; set; }
        public double balanceAtEnd { get; set; }
        public double totalDeposit { get; set; }
        public double totalWithdrawal { get; set; }
        public ClientViewModel clientDet { get; set; }


        public List<SavingsTransactionsViewModel> transactions { get; set; }

    }
}
