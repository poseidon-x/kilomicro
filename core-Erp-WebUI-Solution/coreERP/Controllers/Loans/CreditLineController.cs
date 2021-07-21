using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class CreditLineController : Controller
    {

        public ActionResult EnterCreditLine(long? id)
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

        public ActionResult AssignLoanToCreditLine()
        {
            
            return View();
        }

        public ActionResult CreditLines()
        {
            return View();
        }

        public ActionResult ApproveCreditLine(long? id)
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
    }
}