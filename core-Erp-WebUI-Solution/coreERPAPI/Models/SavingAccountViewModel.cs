using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi
{
    public class SavingAccountViewModel
    {
        //client id
        public int clientId { get; set; }
        public int savingId { get; set; }
        public string savingAccountNo { get; set; }
    }
}