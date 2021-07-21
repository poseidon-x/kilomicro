using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreERP;

namespace coreErpApi.Controllers.Models
{
    public class customResquest : KendoRequest
    {
        public DateTime provisionDate { get; set; }
    }
}