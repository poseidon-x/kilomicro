using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreLibrary
{
    public class ClientChargeSummary
    {
        public double totalCharges { get; set; }
        //public CompanyProfileViewModel company { get; set; }
        public List<ClientServiceChargeViewModel> charges { get; set; }
    }

    public class ClientServiceChargeViewModel
    {
        public int clientID { get; set; }
        public string date { get; set; }
        public string dateString { get; set; }
        public string clientName { get; set; }
        public string accountNO { get; set; }
        public string chargeType { get; set; }
        public double amount { get; set; }
    }
}