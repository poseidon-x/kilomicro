using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class DashUndModel
    {
        //client id
        public string clientID { get; set; }
        
        //loan id
        public string loanNo { get; set; }

        //client name
        public string clientName { get; set; }

        //amount requested 
        public double amountRequested { get; set; }

        //amount approved
        public double amountApproved { get; set; }

        //final appproval date
        public  DateTime finalApprovalDate { get; set; }

        //loan number
        public int loanID { get; set; }

        //staff name 
        public string staffName{get;set;}

        //application Date
        public DateTime applicationDate { get; set; }

        //categoryID
        public int categoryID { get; set; }
    }
}