using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class DashOverMaturedInvestmentModel
    {
        //investment id
        public int investmentId { get; set; }

        //client id
        public string clientID { get; set; }

        //client id
        public string clientName { get; set; }

        //investment number
        public string investmentNo { get; set; }

        //amount deposited
        public double amountDeposited { get; set; }

        //interest accrued
        public double interestAccrued { get; set; }

        //amount widthdrawn
        public double amountWithdrawn { get; set; }

        //first deposit date
        public DateTime firstDepositDate { get; set; }

        //maturity deposit date
        public DateTime maturityDate { get; set; }

        //interest rate
        public double interestRate { get; set; }

        //staff name
        public string staffName { get; set; }
    }
}