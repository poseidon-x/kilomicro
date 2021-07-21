using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Inventory
{
    public class InventoryShrinkageController : Controller
    {
        // GET: InventoryShrinkage
        [HttpGet]
        public ActionResult EnterShrinkage(long? Id)
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


        public ActionResult ApproveShrinkage(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult PostShrinkage(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Shrinkages()
        {
            return View();
        }
    }
}