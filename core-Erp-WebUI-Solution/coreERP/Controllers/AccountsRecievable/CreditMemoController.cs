using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.AccountsRecievable
{
    public class CreditMemoController : Controller
    {
        // GET: OpeningBalances
        public ActionResult EnterCreditMemo(long? id)
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

        public ActionResult ApproveCreditMemo(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult PostCreditMemo(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult CreditMemoes()
        {
            return View();
        }
    }
}