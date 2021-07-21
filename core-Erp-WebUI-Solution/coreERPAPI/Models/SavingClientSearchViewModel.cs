using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi
{
    public class SavingClientSearchViewModel
    {
        //client id
        public bool isName { get; set; }
        public bool isAccountNumber { get; set; }
        public string searchValue { get; set; }
    }
}