using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.AccountsRecievable
{
    public class InvoiceReportsController : Controller
    {
        // GET: LoanReports
        public ActionResult CustomerInvoice()
        {
            return View();
        }
    }
}