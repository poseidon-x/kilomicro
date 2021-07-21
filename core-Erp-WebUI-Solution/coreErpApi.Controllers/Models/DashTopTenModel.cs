using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class DashTopTenModel
    {
        //loan id
        public string loanID { get; set; }

        //client name
        public string clientName { get; set; }

        //amount disbursed
        public double amountDisbursed { get; set; }

        //total due
        public double totalDue { get; set; }

        //last payment date
        public DateTime lastPaymentDate { get; set; }

        //staff name
        public string staffName{get;set;}

        //loan number
        public string loanNo { get; set; }

        //client id
        public string clientID { get; set; }

        //amount requested
        public double amountRequested { get; set; }

        //application date
        public DateTime applicationDate { get; set; }

        //amount approved
        public double amountApproved { get; set; }

        //final amount approaved
        public DateTime finalApprovalDate { get; set; }

        //category i
        public int categoryID { get; set; }

        // disbursement Date
        public DateTime disbursementDate { get; set; }

        //amount Paid
        public double amountPaid { get; set; }

        //email
        public string email { get; set; }

        //phone
        public string phoneNumber { get; set; }

        //address
        public string address { get; set; }

        //clientPKID
        public int clientPKID { get; set; }
    }
}