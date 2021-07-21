using coreErp.Models.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class BatchCashPaymentVM
    {
        public DateTime repaymentDate { get; set; }
        public List<LoanBatchCashRepaymetModel> repaymentItemsList { get; set; }
    }

    public class BatchCheckListVM
    {
        public DateTime approvalDate { get; set; }
        public int groupId { get; set; }
        public List<LoanBatchCheckListModel> approvalItemsList { get; set; }
    }
}