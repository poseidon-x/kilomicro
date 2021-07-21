using System.Web;
using Telerik.Reporting.Cache.Interfaces;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;

namespace coreERP.Controllers.Reporting
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
