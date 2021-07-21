using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreERP.Models.Image;
using coreLogic;

namespace coreERP.Models.Client
{
    public class LoanFeePaymentMultiModel
    {
        public int loanId { get; set; }
        public DateTime paymentDate { get; set; }
        public List<loanPayment> payments { get; set; }
    }


    public class loanPayment
    {
        public int paymentId { get; set; }
        public int repaymentTypeID { get; set; }
        public double amount { get; set; }
    }

    public class GroupLoanFeePaymentMultiModel
    {
        public int loanId { get; set; }
        public DateTime paymentDate { get; set; }
        public List<GroupLoanPayment> payments { get; set; }
    }

    public class GroupLoanPayment
    {
        public int paymentId { get; set; }
        public int repaymentTypeID { get; set; }
        public double amount { get; set; }
    }


}