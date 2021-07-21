using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(agency.Startup))]
namespace agency
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
