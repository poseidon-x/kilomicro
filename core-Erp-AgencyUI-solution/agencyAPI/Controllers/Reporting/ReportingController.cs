using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Telerik.Reporting.Cache.Interfaces;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;

namespace agencyAPI.Controllers.Reporting
{
    public class ReportingController : ReportsControllerBase
    {
        static readonly IReportResolver resolvers = 
                                new ReportTypeResolver()
                                .AddFallbackResolver(new ReportFileResolver());

        protected override IReportResolver CreateReportResolver()
        {
            var reportsPath = HttpContext.Current.Server.MapPath("~/Reports");

            return resolvers;
        }

        protected override ICache CreateCache()
        {
            return Telerik.Reporting.Services.Engine.CacheFactory.CreateFileCache();
        }
    }
}
