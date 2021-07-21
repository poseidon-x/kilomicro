using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class LoanVM
    {
        public int loanID { get; set; }
        public string Description { get; set; }
        public List<repaymentSchedule> repaymentSchedules { get; set; }
    }
}