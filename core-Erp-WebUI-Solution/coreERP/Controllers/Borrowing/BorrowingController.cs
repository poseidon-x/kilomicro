using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Borrowing
{
    public class BorrowingController : Controller
    {
        // GET: 
        public ActionResult CreateBorrowing(long? id)
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

        // GET: 
        public ActionResult ApproveBorrowing()
        {
            return View();
        }

        // GET: 
        public ActionResult EnterRepaymentSchedule()
        {
            return View();
        }

        // GET: 
        public ActionResult DisburseBorrowing()
        {
            return View();
        }

        // GET: 
        public ActionResult BorrowingRepayment()
        {
            return View();
        }

        public ActionResult EnterBorrowingPayment(long? id)
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