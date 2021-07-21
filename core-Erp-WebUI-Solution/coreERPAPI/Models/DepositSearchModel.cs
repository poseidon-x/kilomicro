using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class DepositSearchModel
    {
        public char criteria { get; set; }
        public int depositTypeId { get; set; }
        public int depositId { get; set; }
        public int clientId { get; set; }

    }
}