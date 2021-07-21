using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace agencyAPI.Models
{
    public class CSViewModel
    {
        public int clientID { get; set; }
        public string clientName { get; set; }
        public string clientNameWithAccountNO { get; set; }
        public string accountNo { get; set; }
        public string savingNo { get; set; }
        public string branch { get; set; }
        public string savingTypeName { get; set; }
        public DateTime firstDepositDate { get; set; }
        public double currentBalance { get; set; }
    }
}
