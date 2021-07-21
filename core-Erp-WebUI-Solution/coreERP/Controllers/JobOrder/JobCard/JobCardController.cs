using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.JobOrder.JobCard
{
    public class JobCardController : Controller
    {
        // GET: OpeningBalances
        public ActionResult EnterJobCard(long? id)
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

        public ActionResult ApproveJobCard(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult FulfillJobCard()
        {
            return View();
        }

        public ActionResult SignJobCard()
        {
            return View();
        }

        public ActionResult JobCards(long? id)
        {
            ViewBag.id = id;
            return View();
        }


    }
}