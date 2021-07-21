using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreReports.Models
{
    public class OutstandingRepaymentModel
    {
        public double outstanding { get; set; }
        public double amountDisbursed { get; set; }
        public string clientName { get; set; }
        public DateTime disbursementDate { get; set; }
        public DateTime LastDueDate { get; set; }
        public DateTime LastRepaymentDate { get; set; }
        public int loanGroupId { get; set; }
        public string loanGroupName { get; set; }
        public string loanGroupNumber { get; set; }
        public string loanNo { get; set; }
        public double Paid { get; set; }
        public double Payable { get; set; }
    }
}
