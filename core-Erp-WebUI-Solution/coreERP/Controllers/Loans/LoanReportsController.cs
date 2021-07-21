using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class LoanReportsController : Controller
    {
        // GET: LoanReports
        public ActionResult OutstaningScheduleItems()
        {
            return View();
        }

        public ActionResult DisbursedLoans()
        {
            return View();
        }

        public ActionResult LoanDetails()
        {
            return View();
        }

        public ActionResult LoanDocuments()
        {
            return View();
        }

        public ActionResult DailyCollectionSheet()
        {
            return View();
        }

        public ActionResult AdminCashierReport()
        {
            return View();
        }

        public ActionResult IndividualCashierReport()
        {
            return View();
        }
    }
}