using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http; 
using iTextSharp.text;
using iTextSharp.text.pdf;
using coreERP;
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;

namespace coreERP.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class CashierController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public CashierController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        [HttpGet]
        public IEnumerable<Models.LookupEntry> Get()
        {
            List<Models.LookupEntry> list = new List<Models.LookupEntry>();

            var tills =  le.cashiersTills 
                .AsNoTracking() 
                .ToList();
            using (var ent = new coreSecurityEntities())
            {
                foreach (var item in tills)
                {
                    var fullName = item.userName;
                    var user = ent.users.FirstOrDefault(p => p.user_name == item.userName);
                    if (user != null)
                    {
                        fullName = user.full_name;
                    }
                    list.Add(new Models.LookupEntry
                    {
                        Description = fullName,
                        ID = item.cashiersTillID
                    });
                }
            }

            return list;
        }

        // GET: api/Category
        public string Get(int id)
        {
            var acc = le.cashiersTills
                .FirstOrDefault(p => p.cashiersTillID == id);
            if (acc == null)
            {
                return "";
            }
            using (var ent = new coreSecurityEntities())
            {
                var user = ent.users.FirstOrDefault(p => p.user_name == acc.userName);
                if (user != null)
                {
                    return user.full_name;
                }
            }

            return "";
        }
         
    }
}
