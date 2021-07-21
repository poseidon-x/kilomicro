using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreERP.Models.Image;

namespace coreERP.Models.Client
{
    public class ClientServiceChargeMultiModel
    {
        public int clientId { get; set; }
        public DateTime chargeDate { get; set; }
        public List<clientPayment> payments { get; set; }
    }


    public class clientPayment
    {
        public int paymentId { get; set; }
        public int chargeTypeId { get; set; }
        public double amount { get; set; }
    }

    public class GroupclientPayment
    {
        public int clientId { get; set; }
        public int paymentId { get; set; }
        public int chargeTypeId { get; set; }
        public double amount { get; set; }
    }

    public class GroupClientServiceChargeModel
    {
        public int groupId { get; set; }
        public DateTime chargeDate { get; set; }
        public List<GroupclientPayment> payments { get; set; }
    }
}