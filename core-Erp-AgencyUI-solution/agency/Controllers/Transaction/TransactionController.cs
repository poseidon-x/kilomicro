using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace agency.Controllers.Transaction
{
    public class TransactionController : Controller
    {
        // GET: Transaction
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Agent");
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }
        [HttpGet]
        public ActionResult SavingsDeposit(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        [HttpGet]
        public ActionResult SavingsWithdrawal(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }
    }
}