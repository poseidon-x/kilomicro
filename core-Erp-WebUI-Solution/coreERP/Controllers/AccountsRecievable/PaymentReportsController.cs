using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.AccountsRecievable
{
    public class PaymentReportsController : Controller
    {
        // GET: LoanReports
        public ActionResult CustomerPayments()
        {
            return View();
        }
    }
}