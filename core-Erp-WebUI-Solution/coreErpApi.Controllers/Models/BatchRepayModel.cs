using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class BatchRepayModel
    {
        public int groupId { get; set; }
        public DateTime repaymentDate { get; set; }
        public List<BatchRepaymentViewModel> repayments { get; set; }
    }
}