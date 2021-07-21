using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Loans
{
    public class OutstandingScheduleItemsViewModel
    {
        public int clientId { get; set; }
        public int loanId { get; set; }
        public string loanNumber { get; set; }
        public string clientNumber { get; set; }
        public string clientName { get; set; }
        public string address { get; set; }
        public string directions { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public double amountDisbursed { get; set; }
        public double amountInArears { get; set; }
        public int numberOfPaymentsInArears { get; set; }
        public DateTime lastPaymentDate { get; set; }
        public byte[] companyLogo { get; set; }
        public string companyAddress { get; set; }
        public DateTime expiryDate { get; set; }
        public double loanTenure { get; set; }
        public DateTime? disbursementDate { get; set; }
    }
}
