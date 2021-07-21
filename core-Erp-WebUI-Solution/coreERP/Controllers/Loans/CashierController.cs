using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class CashierController : Controller
    {
        [HttpGet]
        public ActionResult CashierFund(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        [HttpGet]
        public ActionResult CashierCashup(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        [HttpGet]
        public ActionResult UploadCashup()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CashierFunding()
        {
            return View();
        }

        public ActionResult CashierReceiptWithCheck(int? Id, int? lnId)
        {
            ViewBag.Id = Id ?? -1;
            ViewBag.lnId = lnId ?? -1;
            return View();
        }

        [HttpGet]
        public ActionResult MyFunds()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MyTransactions()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CashierTillRange()
        {
            return View();
        }

        



    }
}
