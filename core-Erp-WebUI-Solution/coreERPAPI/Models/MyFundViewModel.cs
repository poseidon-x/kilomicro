using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class MyFundViewModel
    {
        public string cashierName { get; set; }
        public DateTime fundDate { get; set; }
        public double fundAmount { get; set; }
        public cashiersTill tillData { get; set; }
    }
}