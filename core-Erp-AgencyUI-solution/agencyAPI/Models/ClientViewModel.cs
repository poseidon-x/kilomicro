using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace agencyAPI.Models
{
    public class ClientViewModel
    {
        //client id
        public int clientID { get; set; }

        //client name
        public string clientName { get; set; }
        public int clientTypeId { get; set; }
        //account number
        public string accountNO { get; set; }
        public string clientNameWithAccountNO { get; set; }
        public byte[] clntImage { get; set; }
        public int savingId { get; set; }
        public IEnumerable<clientImage> clntImages { get; set; }
    }
}