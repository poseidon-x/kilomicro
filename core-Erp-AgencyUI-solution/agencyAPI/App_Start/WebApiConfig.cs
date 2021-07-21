using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;

namespace agencyAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();
            RegisterReportingRoutes(config);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
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
                defaults: new { controller = "Reporting", action = "Formats" });
        }
    }
}
