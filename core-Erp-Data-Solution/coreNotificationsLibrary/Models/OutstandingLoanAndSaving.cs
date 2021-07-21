using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsLibrary.Models
{
    public class OutstandingLoanAndSaving
    {
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public int LoanId { get; set; }
        public double LoanAmount { get; set; }
        public double LoanOutstandingAmount { get; set; }
        public double Collateral { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string LoanGroupName { get; set; }
        public string LoanNo { get; set; }
        public double WriteOffAmount { get; set; }
        public DateTime DisbursementDate { get; set; }
        public double SavingBalance { get; set; }
        public string SavingNo { get; set; }
        public string ClientPhoneNo { get; set; }
        public double TotalPaid { get; set; }
        public int RepaymentCount { get; set; }
        public double Interest { get; set; }
        public double Fee { get; set; }

    }

}
