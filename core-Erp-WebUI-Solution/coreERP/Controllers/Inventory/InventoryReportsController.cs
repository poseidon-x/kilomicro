using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Inventory
{
    public class InventoryReportsController : Controller
    {

        public ActionResult StockMasterReport()
        {
            return View();
        }

        public ActionResult InventoryItemsReport()
        {
            return View();
        }

        public ActionResult InventoryItemsByLocationReport()
        {
            return View();
        }
    }
}