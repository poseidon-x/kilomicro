using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using System.Web.Http.WebHost;
using coreERP.Providers;

namespace coreERP
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // config.EnableCors();
            //For Telerik Reporting
            RegisterReportingRoutes(config);

            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "crud/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiUI",
                routeTemplate: "ui/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "GetMenus", controller = "UIAPI" }
            );

            config.Routes.MapHttpRoute(
                name: "ExportAPI",
                routeTemplate: "export/{controller}/{action}"
            );

            //1 (/iv/)
            config.Routes.MapHttpRoute(
                "DefaultInventory", // Route name
                "iv/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //2 (/ln/)
            config.Routes.MapHttpRoute(
                "DefaultLoans", // Route name
                "ln/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //3 (/hc/)
            config.Routes.MapHttpRoute(
                "DefaultHumanCapital", // Route name
                "hc/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //4 (/fa/)
            config.Routes.MapHttpRoute(
                "DefaultFixedAssets", // Route name
                "fa/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //5 (/gl/)
            config.Routes.MapHttpRoute(
                "DefaultGeneralLedger", // Route name
                "gl/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //6 (/ar/)
            config.Routes.MapHttpRoute(
                "DefaultAccountsReceivable", // Route name
                "ar/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //7 (/ap/)
            config.Routes.MapHttpRoute(
                "DefaultAccountsPayable", // Route name
                "ap/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //8 (/so/)
            config.Routes.MapHttpRoute(
                "DefaultSalesOrder", // Route name
                "so/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //9 (/jc/)
            config.Routes.MapHttpRoute(
                "DefaultJobCard", // Route name
                "jc/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            //10 (/po/)
            config.Routes.MapHttpRoute(
                "DefaultPurchaseOrder", // Route name
                "po/{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = RouteParameter.Optional } // Parameter defaults
            );

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //config.Filters.Add(new Providers.AuthorizationFilter());
        }

        private static void RegisterReportingRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(name: "Clients",
             routeTemplate: "{controller}/clients/{clientID}",
             defaults: new { controller = "Reporting", action = "Clients", clientID = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "Instances",
                routeTemplate: "{controller}/clients/{clientID}/instances/{instanceID}",
                defaults: new { controller = "Reporting", action = "Instances", instanceID = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "DocumentResources",
                routeTemplate: "{controller}/clients/{clientID}/instances/{instanceID}/documents/{documentID}/resources/{resourceID}",
                defaults: new { controller = "Reporting", action = "DocumentResources" });

            config.Routes.MapHttpRoute(
                name: "DocumentActions",
                routeTemplate: "{controller}/clients/{clientID}/instances/{instanceID}/documents/{documentID}/actions/{actionID}",
                defaults: new { controller = "Reporting", action = "DocumentActions" });

            config.Routes.MapHttpRoute(
                name: "DocumentPages",
                routeTemplate: "{controller}/clients/{clientID}/instances/{instanceID}/documents/{documentID}/pages/{pageNumber}",
                defaults: new { controller = "Reporting", action = "DocumentPages" });

            config.Routes.MapHttpRoute(
                name: "DocumentInfo",
                routeTemplate: "{controller}/clients/{clientID}/instances/{instanceID}/documents/{documentID}/info",
                defaults: new { controller = "Reporting", action = "DocumentInfo" });

            config.Routes.MapHttpRoute(
                name: "Documents",
                routeTemplate: "{controller}/clients/{clientID}/instances/{instanceID}/documents/{documentID}",
                defaults: new { controller = "Reporting", action = "Documents", documentID = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "Parameters",
                routeTemplate: "{controller}/clients/{clientID}/parameters",
                defaults: new { controller = "Reporting", action = "Parameters" });

            config.Routes.MapHttpRoute(
                name: "Formats",
                routeTemplate: "{controller}/clients/{clientID}/formats",
                defaults: new
                {
                    controller = "Reporting",
                    action = "Formats",
                    clientID = RouteParameter.Optional,
                    clients = RouteParameter.Optional
                });

            config.Routes.MapHttpRoute(
                name: "Formats2",
                routeTemplate: "{controller}/formats",
                defaults: new
                {
                    controller = "Reporting",
                    action = "Formats",
                    clientID = RouteParameter.Optional,
                    clients = RouteParameter.Optional
                });
        }
    }
}
