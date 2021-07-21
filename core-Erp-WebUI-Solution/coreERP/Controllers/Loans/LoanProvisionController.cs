using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class LoanProvisionController : Controller
    {
        
        [HttpGet]
        public ActionResult LoanProvision(int? Id)
        {
            ViewBag.Id = Id ?? -1;
            return View();
        }

        

    }
}
