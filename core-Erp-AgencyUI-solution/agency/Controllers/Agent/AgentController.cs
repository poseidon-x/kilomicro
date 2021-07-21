using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using agencyAPI.Providers;

namespace agency.Controllers.Agent
{
    //[AuthorizationFilter()]
    public class AgentController : Controller
    {
        // GET: Agent
        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Agent");
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        // GET: Agent Dashboard
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}