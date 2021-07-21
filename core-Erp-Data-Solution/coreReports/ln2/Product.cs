using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace coreReports.ln2
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double qualifiedAmount { get; set; }
        public double qualifiedAmountMax { get; set; }
        public double monthlyDeduction { get; set; }
        public double processingFee { get; set; }
        public double netLoanAmount { get; set; }
        public string tenure { get; set; }
        public List<coreLogic.repaymentSchedule> schedules { get; set; }
    }

    public class ProductList : List<Product>
    {
    }

    public class Incentive
    {
        public string invoiceDescription { get; set; }
        public string agentName { get; set; }
        public string clientName { get; set; }
        public double amount { get; set; }
        public string receiptID { get; set; }
        public DateTime invoiceDate { get; set; }
        public double loanAmount { get; set; }
        public double netLoanAmount { get; set; }
        public double commission { get; set; }
        public double witholding { get; set; }
    }
}
