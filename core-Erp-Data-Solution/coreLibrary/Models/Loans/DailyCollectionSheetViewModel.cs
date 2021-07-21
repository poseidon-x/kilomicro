using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Loans
{
    public class DailyCollectionSheetViewModel
    {
        public string branchName { get; set; }
        public string collectionDate { get; set; }
        public double totalSecurityDeposit { get; set; }
        public double totalAdmissionFee { get; set; }
        public double totalPassbookPurchased { get; set; }
        public double totalInsurance { get; set; }
        public double totalBadDebt { get; set; }
        public double totalPaymentExpected { get; set; }
        public double totalCashPayment { get; set; }
        public double totalAdjustment { get; set; }
        public double totalOutstandingOverDue { get; set; }
        public double totalReceipt { get; set; }
        public List<DailyCollectionSheetDetailViewModel> details { get; set; }

    }

    public class DailyCollectionSheetDetailViewModel
    {
        public int no { get; set; }
        public string staffName { get; set; }
        public int groupId { get; set; }
        public string groupName { get; set; }
        public double securityDeposit { get; set; }
        public double admissionFee { get; set; }
        public double passbookPurchased { get; set; }
        public double insurance { get; set; }
        public double badDebt { get; set; }
        public double paymentExpected { get; set; }
        public double cashPayment { get; set; }
        public double adjustment { get; set; }
        public double outstanding { get; set; }
        public double receipt { get; set; }
    }
}
