using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class SystemDateController : Controller
    {
        
        [HttpGet]
        public ActionResult SystemDate()
        {
            return View();
        }
        

    }
}
