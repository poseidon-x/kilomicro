using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace agency.Controllers.Account
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && agency.Settings.getToken() != "INVALID_TOKEN")
            {
                return RedirectToAction("Dashboard", "Agent");
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        // GET: Account: Client Account
        [HttpGet]
        public ActionResult ClientAccount(int? id)
        {
            ViewBag.Id = id == null ? -1 : id.Value;
            return View();
        }

        // GET: Account: Savings Account
        [HttpGet]
        public ActionResult SavingsAccount(int? id)
        {
            ViewBag.Id = id == null ? -1 : id.Value;
            return View();
        }

        // GET: Account: Check Balance
        [HttpGet]
        public ActionResult CheckBalance()
        {

            return View();
        }

        // GET: Account: Print Statement
        [HttpGet]
        public ActionResult PrintStatement()
        {
            return View();
        }


        // GET: Account: View Statement
        [HttpGet]
        public ActionResult ViewStatement()
        {
            return View();
        }

        // GET: Account: View Detail
        [HttpGet]
        public ActionResult ViewDetail()
        {
            return View();
        }
    }
}