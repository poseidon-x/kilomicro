﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coreERP.Controllers.AccountsRecievable
{
    public class ArInvoiceController : Controller
    {
        // GET: OpeningBalances
        public ActionResult EnterInvoice(long? id)
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

        public ActionResult PostInvoice(long? id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Invoices()
        {
            return View();
        }
    }
}