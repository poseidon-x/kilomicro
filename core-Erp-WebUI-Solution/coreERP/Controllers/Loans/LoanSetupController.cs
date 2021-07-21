using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class LoanSetupController : Controller
    {
        // GET: LoanSetup
        public ActionResult LoanClosureReason()
        {
            return View();
        }

        public ActionResult RestructureLoan()
        {
            return View();
        } 

        public ActionResult EnterAdditionalLoanInfo(int? Id)
        {
            if (Id == null)
            {
                ViewBag.Id = -1;
            }
            else
            {
                ViewBag.Id = Id.Value;
            }
            return View();
        }

        public ActionResult CreateLoanGroup(int? Id)
        {
            if (Id == null)
            {
                ViewBag.Id = -1;
            }
            else
            {
                ViewBag.Id = Id.Value;
            }
            return View();
        }

        public ActionResult LoanGroups(int? Id)
        {
            return View();
        }

        public ActionResult LoanTypeApproval(int? Id)
        {
            if (Id == null)
            {
                ViewBag.Id = -1;
            }
            else
            {
                ViewBag.Id = Id.Value;
            }
            return View();
        }

        public ActionResult LoansPendingMyApproval()
        {
            return View();
        }

        public ActionResult LoanApproval(int? Id)
        {
            if (Id == null)
            {
                ViewBag.Id = -1;
            }
            else
            {
                ViewBag.Id = Id.Value;
            }
            return View();
        }

        public ActionResult LoanProvision()
        {
            return View();
        }
        


    }
}