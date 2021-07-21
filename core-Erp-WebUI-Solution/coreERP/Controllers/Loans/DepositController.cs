using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class DepositController: Controller
    {
        [HttpGet]
        public ActionResult DepositType()
        {
            return View();
        }

        public ActionResult EnterDepositTypeTenure(long? Id)
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

        [HttpGet]
        public ActionResult EnterDepositCheck(long? Id)
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

        public ActionResult DueDepositChecks(long? Id)
        {
            return View();
        }

        public ActionResult ApplyClientCheck(long? Id)
        {
            return View();
        }

        public ActionResult ProcessAppliedClientCheck(long? ch, long? cl)
        {
            if (ch == null || cl == null)
            {
                ViewBag.ClientCheck = -1;
                ViewBag.ClientId = -1;
            }
            else
            {
                ViewBag.ClientCheck = ch.Value;
                ViewBag.ClientId = cl.Value;
            }
            return View();
        }

        [HttpGet]
        public ActionResult InvestmentReceipt(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        public ActionResult ApplyReceivedInvestment(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        public ActionResult ApplyInvestment(int? Id, int? receiptDetailId)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            ViewBag.reciptDetId = receiptDetailId == null ? -1 : receiptDetailId.Value;
            return View();
        }

        public ActionResult ClientInvestmentReceipts()
        {
            return View();
        }

        public ActionResult UnAppliedInvestmentCheques()
        {
            return View();
        }

        public ActionResult UpgradeInvestmentAccount()
        {
            return View();
        }

        public ActionResult InvestmentUpgradeAccount(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        public ActionResult DepositUpgrade(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        public ActionResult DepositCertificateConfig(int? Id)
        {
            ViewBag.Id = Id == null ? -1 : Id.Value;
            return View();
        }

        public ActionResult DepositCertificate()
        {
            return View();
        }

        public ActionResult DepositAdditionalWithCheck(int? Id, int? depId)
        {
            ViewBag.Id = Id ?? -1 ;
            ViewBag.depId = depId ?? -1 ;
            return View();
        }

        public ActionResult DepositWithdrawalWithCheck(int? Id, int? depId)
        {
            ViewBag.Id = Id ?? -1 ;
            ViewBag.depId = depId ?? -1;
            return View();
        }

        public ActionResult DepositCertificate1()
        {
            return View();
        }

        public ActionResult Deposit(int? Id)
        {
            ViewBag.Id = Id ?? -1;
            return View();
        }

        

        public ActionResult DepositInterestUpgrade()
        {
            return View();
        }


    }
}
