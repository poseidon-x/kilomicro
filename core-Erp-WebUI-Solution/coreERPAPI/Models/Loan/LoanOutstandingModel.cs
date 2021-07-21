using System;

namespace coreERP.Models.Loan
{
    public class LoanOutstandingModel
    {
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public int LoanId { get; set; }
        public double LoanAmount { get; set; }
        public double OutstandingAmount { get; set; }
        public double Collateral { get; set; }
        public int DaysDefault { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string LoanGroupName { get; set; }
        public string LoanNo { get; set; }
        public double WriteOffAmount { get; set; }
        public string Officer { get; set; }
        public DateTime DisbursementDate { get; set; }
        public string BranchName { get; set; }
        public double Paid { get; set; }
        public int LoanGroupId { get; set; }
        public int StaffId { get; set; }
        public int? BranchId { get; set; }

    }

    public class LoanOutstandingInputModel : KendoRequest
    {
        public int? BranchID { get; set; }
        public int? ClientId { get; set; }
        public DateTime EndDate { get; set; }
        public int ExpiredFlag { get; set; }
    }
}