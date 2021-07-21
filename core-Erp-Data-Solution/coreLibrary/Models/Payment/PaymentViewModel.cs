using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic.Models.CompanyProfile;

namespace coreLogic.Models.Payment
{
    public class PaymentViewModel
    {
        public long paymentId { get; set; }
        public long? customerId { get; set; }
        public string customerName { get; set; }
        public DateTime? date { get; set; }
        public string paymentDate { get; set; }
        public int paymentCurrencyId { get; set; }
        public string paymentCurrency { get; set; }
        public int paymentMethodId { get; set; }
        public string paymentMethod { get; set; }
        public bool isCheck { get; set; }
        public bool isCreditCard { get; set; }
        public bool isMobileMoney { get; set; }
        public string checkNumber { get; set; }
        public string creditCardNumber { get; set; }
        public string mobileMoneyNumber { get; set; }
        public double paymentTotal { get; set; }
        public double invoiceTotal { get; set; }
        public double overPaymentAmount { get; set; }

        public List<PaymentLinesViewModel> paymentLines { get; set; }
        public CompanyProfileViewModel compamy { get; set; }



    }
}
