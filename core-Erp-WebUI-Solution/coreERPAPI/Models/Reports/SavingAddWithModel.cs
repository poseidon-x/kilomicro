using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.Reports
{
    public class SavingAddWithModel
    {
        public string savingNo { get; set; }
        public int savingID { get; set; }
        public int? StaffId { get; set; }
        public string clientName { get; set; }
        public int clientID { get; set; }
        public double interestBalance { get; set; }
        public double principalBalance { get; set; }
        public string creator { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime? TransDate { get; set; }
        public string SavingTypeName { get; set; }
        public string modeOfPaymentName { get; set; }
        public double interestRate { get; set; }
        public double savingAmount { get; set; }
        public double amountWithdrawn { get; set; }
        public double SavingBalance { get; set;}
        public string Branch { get; set; }
        public string GroupName { get; set; }
        public int? BranchId { get; set; }
    }
}