using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Customers
{
    public class CustomersController : Controller
    {
        // GET: OpeningBalances
        public ActionResult EnterCustomer(long? id)
        {
            if (id == null)
            {
                ViewBag.id = -1;
            }
            else
            {
                ViewBag.id = id.Value;
            }
            return View();
        }


        public ActionResult Customers()
        {
            return View();
        }
    }
}