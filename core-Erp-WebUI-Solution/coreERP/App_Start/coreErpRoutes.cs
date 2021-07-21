using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Infrastructure;

[assembly: WebActivator.PostApplicationStartMethod(typeof(coreERP.AppStart_RegisterRoutesAreasFilters), "Start")]

namespace coreERP {
    public static class AppStart_RegisterRoutesAreasFilters {
        public static void Start() {
            // Set everything up with you having to do any work.
            // I'm doing this because it means that
            // your app will just run. You might want to get rid of this 
            // and integrate with your own Global.asax. 
            // It's up to you. 
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");

            //1 (/iv/)
            routes.MapRoute(
                "DefaultInventory", // Route name
                "iv/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //2 (/ln/)
            routes.MapRoute(
                "DefaultLoans", // Route name
                "ln/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //3 (/hc/)
            routes.MapRoute(
                "DefaultHumanCapital", // Route name
                "hc/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //4 (/fa/)
            routes.MapRoute(
                "DefaultFixedAssets", // Route name
                "fa/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //5 (/gl/)
            routes.MapRoute(
                "DefaultGeneralLedger", // Route name
                "gl/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //6 (/ar/)
            routes.MapRoute(
                "DefaultAccountsReceivable", // Route name
                "ar/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //7 (/ap/)
            routes.MapRoute(
                "DefaultAccountsPayable", // Route name
                "ap/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //8 (/so/)
            routes.MapRoute(
                "DefaultSalesOrder", // Route name
                "so/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //9 (/jc/)
            routes.MapRoute(
                "DefaultJobCard", // Route name
                "jc/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //10 (/po/)
            routes.MapRoute(
                "DefaultPurchaseOrder", // Route name
                "po/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    
    }

}