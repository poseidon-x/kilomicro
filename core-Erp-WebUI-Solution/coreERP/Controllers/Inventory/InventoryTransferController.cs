using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Inventory
{
    public class InventoryTransferController : Controller
    {
        // GET: InventoryShrinkage
        [HttpGet]
        public ActionResult EnterTransfer(long? Id)
        {
            if (Id == null)
            {
                ViewBag.Id = -1;
            }
            else
            {
                ViewBag.Id = Id.Value;
            }
            return View();
        }


        public ActionResult ApproveTransfer(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult PostTransfer(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult DeliverTransfer(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Transfers()
        {
            return View();
        }
    }
}