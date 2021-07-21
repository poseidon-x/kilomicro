using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class SalaryLoansController : Controller
    {
        // GET: Config
        public ActionResult Config()
        {
            return View();
        }

        // GET: PendingApproval
        public ActionResult PendingApproval()
        {
            return View();
        }

        // GET: PendingDisbursement
        public ActionResult PendingDisbursement()
        {
            return View();
        }

        // GET: Apply
        public ActionResult Apply(int? id)
        {
            if (id == null)
            {
                ViewBag.Id = 0;
            }
            else
            {
                ViewBag.Id = id.Value;
            }
            return View();
        }

        // GET: Apply
        public ActionResult Approve(int id)
        {
            ViewBag.Id = id; 
            return View();
        }

        // GET: Deny
        public ActionResult Deny(int id)
        {
            ViewBag.Id = id;
            return View();
        }


    }
}