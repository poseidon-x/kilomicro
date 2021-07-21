using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(coreERP.Startup))]
namespace coreERP
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ControllerBuilder.Current.DefaultNamespaces.Add(
                "coreErp.Controllers.Loans");
        }
    }
}