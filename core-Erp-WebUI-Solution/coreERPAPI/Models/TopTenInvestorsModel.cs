using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class TopTenInvestorsModel
    {
        /*
         * TopTenInvestorsModel properties
         */

        //client id -> a/c no
        public string clientID { get; set; }

        //investment number
        public string investmentNo { get; set; }

        //client name
        public string clientName { get; set; }

        //amount invested
        public double amountInvested { get; set; }

        //start date
        public DateTime firstDepositDate { get; set; }

        //interest accrued
        public double interestAccrued { get; set; }

        //principal balance
        public double principalBalance { get; set; }

        //interest balance
        public double interestBalance { get; set; }

        //amount widthdrawn
        public double amountWithdrawn { get; set; }

        //phone number
        public string phoneNumber { get; set; }

        //email
        public string email { get; set; }

        //address
        public string address { get; set; }

        //current balance
        public double currentBalance { get; set; }

        //staffName
        public string staffName { get; set; }

        //client pk id
        public int clientPKID { get; set;}
    }
}