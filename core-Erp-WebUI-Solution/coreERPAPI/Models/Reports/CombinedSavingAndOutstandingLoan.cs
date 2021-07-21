using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.Reports
{
    public class CombinedSavingAndOutstandingLoan
    {
        //Savings props

        public double AvailableInterestBalance { get; set; }
        public double AvailablePrincipalBalance { get; set; }
        public string SavingNo { get; set; }
        public int SavingId { get; set; }
        public int? StaffId { get; set; }
        public double InterestBalance { get; set; }
        public double PrincipalBalance { get; set; }
        public double SavingBalance { get; set; }

        //OutstandingLoans props
        public double? AmountDisbursed { get; set; }        
        public DateTime? DisbursementDate { get; set; }
        public DateTime? LastDueDate { get; set; }
        public DateTime? LastRepaymentDate { get; set; }
        public int? LoanGroupId { get; set; }
        public string LoanGroupName { get; set; }
        public string LoanGroupNumber { get; set; }
        public string LoanNo { get; set; }
        public double? Paid { get; set; }
        public double? Payable { get; set; }
        public double? WriteOffAmount { get; set; }
        public DateTime? WriteOffDate { get; set; }
        public double Outstanding { get; set; }
        public string Officer { get; set; }

        public string DisbursedDateToString { get; set; }
        //Common props
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string Branch { get; set; }
        public int? BranchId { get; set; }
    }
}