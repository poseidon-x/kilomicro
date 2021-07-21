using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class ClientController : Controller
    {
        [HttpGet]
        public ActionResult ClientServiceCharge(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        [HttpGet]
        public ActionResult ClientServiceChargeMulti()
        {
            using (var le = new coreLogic.coreLoansEntities())
            {
                var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                if (ct == null)
                {
                    return RedirectToAction("/");
                }
            }
            return View();
        }
        

        [HttpGet]
        public ActionResult Client(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        [HttpGet]
        public ActionResult ClientStatus(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        [HttpGet]
        public ActionResult NewClient()
        {
            return View();
        }

    }
}
