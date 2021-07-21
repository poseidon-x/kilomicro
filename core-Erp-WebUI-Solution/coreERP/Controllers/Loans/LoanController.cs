using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class LoanController : Controller
    {
        
        [HttpGet]
        public ActionResult NewPrivateCompanyStaff(int? Id)
        {
            ViewBag.Id = Id ?? -1;
            return View();
        }

        [HttpGet]
        public ActionResult StaffLoanRepaymentUpload()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Loan(int? Id)
        {
            ViewBag.Id = Id ?? -1;
            return View();
        }

        [HttpGet]
        public ActionResult LoanRepaymentMulti()
        {
            return View();
        }

        public ActionResult EditLoanSchedule(long? id)
        {
            ViewBag.id = id == null ? -1 : id.Value;
            return View();
            
        }

        public ActionResult EditControllerFile(long? id)
        {
            ViewBag.id = id == null ? -1 : id.Value;
            return View();

        }

        public ActionResult ControllerFileEditMonthlyDed(long? id)
        {
            ViewBag.id = id == null ? -1 : id.Value;
            return View();

        }

    }
}
