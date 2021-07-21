using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.Loans
{
    public class DocumentTemplateController : Controller
    {

        public ActionResult CreateLoanDocumentTemplate(long? id)
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

        public ActionResult LoanDocumentTemplates()
        {
            return View();
        }

        public ActionResult CreateLoanDocumentPlaceHolderType(long? id)
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



        //public ActionResult ApproveCreditLine(long? id)
        //{
        //    if (id == null)
        //    {
        //        ViewBag.id = -1;
        //    }
        //    else
        //    {
        //        ViewBag.id = id.Value;
        //    }
        //    return View();
        //}
    }
}