using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class SavingController : Controller
    {
        
        [HttpGet]
        public ActionResult SavingsAdditional(int? Id)
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

        [HttpGet]
        public ActionResult SavingsStatement()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Savings(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        [HttpGet]
        public ActionResult SavingsWithdrawalWithCheck(int? id,int? savId)
        {
            ViewBag.Id = id ?? -1;
            ViewBag.savId = savId ?? -1;
            return View();
        }

        [HttpGet]
        public ActionResult SavingsAdditionalWithCheck(int? id, int? savId)
        {
            ViewBag.Id = id ?? -1;
            ViewBag.savId = savId ?? -1;
            return View();
        }

        [HttpGet]
        public ActionResult SavingWithCheck()
        {
            return View();
        }

    }
}
