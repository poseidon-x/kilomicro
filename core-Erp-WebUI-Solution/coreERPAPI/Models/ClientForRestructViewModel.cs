using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class ClientForRestructViewModel
    {
        //client id
        public int clientID { get; set; }

        //client name
        public string clientName { get; set; }
        //account number
        public string accountNO { get; set; }
        public string clientNameWithAccountNO { get; set; }
        public byte[] clntImage { get; set; }
        public IEnumerable<clientImage> clntImages { get; set; }
        public DateTime? disbursementDate { get; set; }
        public double loanTenure { get; set; }
    }
}