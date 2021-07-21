using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(coreERP.Startup))]

namespace coreERP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ControllerBuilder.Current.DefaultNamespaces.Add(
                "coreErpApi.Controllers.Controllers");
        }
    }
}
