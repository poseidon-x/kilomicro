using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.SalesOrder
{
    public class SalesOrderController : Controller
    {
        // GET: OpeningBalances
        public ActionResult EnterSalesOrder(long? id)
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


        public ActionResult SalesOrders()
        {
            return View();
        }
    }
}