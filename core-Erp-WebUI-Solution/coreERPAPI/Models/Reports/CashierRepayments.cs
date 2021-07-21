using coreReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.Reports
{
    public class CashierRepayments:vwCashierRepaymentsGrouped
    {
        public string Branch { get; set; }
        public string GroupName { get; set; }
        public int? BranchId { get; set; }

    }

    public class OtherCashierRepayment : vwCashierRepayment
    {
        public string Branch { get; set; }
        public string GroupName { get; set; }
        public int? BranchId { get; set; }

    }

    public class CashierDisbursement : vwCashierDisb
    {
        public string Branch { get; set; }
        public string GroupName { get; set; }
        public int? BranchId { get; set; }

    }


}