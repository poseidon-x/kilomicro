using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Inventory
{
    public class OpeningBalancesController : Controller
    {
        // GET: OpeningBalances
        public ActionResult EnterBalance(long? id)
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

        public ActionResult ApproveBalances(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult PostBalance(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult OpenningBalances()
        {
            return View();
        }
    }
}