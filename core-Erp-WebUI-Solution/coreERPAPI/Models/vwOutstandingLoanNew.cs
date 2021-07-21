using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class vwOutstandingLoanNew : vwOutstandingLoan
    {
        public double outstanding { get; set; }
        public string Officer { get; set; }
        public string BranchName { get; set; }
        public string OfficerUserName { get; set; }
        public int? BranchId { get; set; }
        public string ClientPhone { get; set; }
        public int DaysDefault { get; set; }

    }
}