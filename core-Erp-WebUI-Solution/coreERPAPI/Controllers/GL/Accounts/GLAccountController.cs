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
    public class GLAccountController : ApiController
    {
        core_dbEntities le = new core_dbEntities();

        public GLAccountController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<accts> Get()
        {
            return le.accts
                .AsNoTracking()
                .OrderBy(p=> p.acc_name)
                .ToList();
        }

        // GET: api/Category
        public string Get(int id)
        {
            var acc = le.accts
                .FirstOrDefault(p => p.acct_id == id);
            if (acc == null)
            {
                return "";
            }

            return acc.acc_name;
        }

        // GET: api/Category
        [HttpGet]
        public IEnumerable<accts> GetByCategory(int categoryId)
        {
            return le.accts
                .Include(p =>  p.acct_heads)
                .Include(p => p.acct_heads.acct_cats)
                .Where(p => p.acct_heads.acct_cats.cat_code == categoryId)
                .ToList();
        }

         
    }
}
