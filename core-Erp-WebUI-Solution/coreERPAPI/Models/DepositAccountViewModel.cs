using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi
{
    public class DepositAccountViewModel
    {
        //client id
        public int clientId { get; set; }
        public int depositId { get; set; }
        public string depositAccountNo { get; set; }
    }
}