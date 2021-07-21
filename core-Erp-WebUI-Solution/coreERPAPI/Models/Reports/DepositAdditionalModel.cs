using coreReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.Reports
{
    public class DepositAdditionalModel : vwDepositAdditional
    {
        public string GroupName { get; set; }
        public int? BranchId { get; set; }
    }
}