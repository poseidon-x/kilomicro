using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Borrowing
{
    public class BorrowingReportsController : Controller
    {
       
        public ActionResult BorrowingAccount()
        {
            return View();
        }

        public ActionResult ClientBorrowingAccounts()
        {
            return View();
        }

        public ActionResult BorrowingAccountsSummary()
        {
            return View();
        }

    }
}