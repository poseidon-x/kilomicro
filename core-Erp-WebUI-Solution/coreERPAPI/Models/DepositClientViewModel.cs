using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi
{
    public class DepositClientViewModel
    {
        public int clientID { get; set; }
        public string clientName { get; set; }
        public int depositId { get; set; }
        public string clientNameWithDepositNO { get; set; }
        public byte[] clntImage { get; set; }
    }
}