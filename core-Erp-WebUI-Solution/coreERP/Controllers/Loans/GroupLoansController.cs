using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class GroupLoansController : Controller
    {

        public ActionResult GroupLoanDisbursement(long? id)
        {
            return View();
        }

        public ActionResult GroupLoanRepayment()
        {
            return View();
        }

        public ActionResult GroupCashLoanRepayment()
        {
            return View();
        }
        public ActionResult GroupLoanBatchCheckList()
        {
            return View();
        }
        public ActionResult GroupLoanBatchApproval()
        {
            return View();
        }

        public ActionResult OutstandingRepayment()
        {
            return View();
        }

        public ActionResult OverPaidRepayment()
        {
            return View();
        }


        public ActionResult BatchSavingsPayment()
        {
            return View();
        }

        
        public ActionResult CombinedPayment()
        {
            return View();
        }

        public ActionResult CombinedBatchDisburseChecklist()
        {
            return View();
        }

        public ActionResult GroupLoanFeesRepayment()
        {
            return View();
        }
        public ActionResult GroupClientServiceCharge()
        {
            return View();
        }

        public ActionResult GroupLoanFeesRepayDay()
        {
            return View();
        }

        public ActionResult GroupLoanDayRepayment()
        {
            return View();
        }

        public ActionResult ArrearsReportFull()
        {
            return View();
        }
        
        public ActionResult OutstandingLoan()
        {
            return View();
        }

        public ActionResult BatchGroupLoanDisbursement()
        {
            return View();
        }
        
    }
}