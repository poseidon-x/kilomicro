using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.JobOrder.WorkOrder
{
    public class WorkOrderController : Controller
    {
        // GET: OpeningBalances
        public ActionResult EnterWorkOrder(long? id)
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

        public ActionResult WorkOrders(long? id)
        {
            return View();
        }

    }
}