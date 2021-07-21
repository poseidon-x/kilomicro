using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace coreERP.Controllers.Loans.Setups
{
    public class ModeOfPaymentController : ApiController
    {
        [HttpGet]
        public IEnumerable<Models.LookupEntry> Get()
        {
            var mps = new Models.LookupEntry[]{
                new Models.LookupEntry{
                    ID=1,
                    Description="Cash"
                },
                new Models.LookupEntry{
                    ID=2,
                    Description="Cheque"
                },
                new Models.LookupEntry{
                    ID=3,
                    Description="Bank Transfer"
                }
            };

            return mps;
        }
    }
}
