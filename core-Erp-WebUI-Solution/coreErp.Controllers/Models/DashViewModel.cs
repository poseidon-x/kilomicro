﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class DashViewModel
    {
        //client id
        public string clientID { get; set; }

        //client name
        public string clientName { get; set; }

        //due amount
        public double amountDue { get; set; }

        //due date
        public DateTime dateDue { get; set; }

        //loan id
        public int loanID { get; set; }

        //staff name
        public string staffName { get; set; }
        
        //staff name
        public string loanNo { get; set; }

    }
}