using coreErp.Models.Loan;
using coreErpApi.Controllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class CombinedBatchDisburseChecklistVM
    {
        public DateTime disbursementDate { get; set; }
        public DateTime approvalDate { get; set; }
        public int groupId { get; set; }
        public List<CombinedLoanBatchDisburseCheckListModel> approvalItemsList { get; set; }
        
    }
}