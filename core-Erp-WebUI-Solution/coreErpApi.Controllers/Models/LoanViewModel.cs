using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class LoanViewModel
    {
        //client id
        public int loanId { get; set; }
        //client name
        public int clientId { get; set; }
        public string clientName { get; set; }
        public string loanNumberWithName { get; set; }
        public double amountApproved { get; set; }
        public double amountDisbursed { get; set; }
        public int paymentModeId { get; set; }
        public int? bankId { get; set; }
        public string chequeNumber { get; set; }


    }
}