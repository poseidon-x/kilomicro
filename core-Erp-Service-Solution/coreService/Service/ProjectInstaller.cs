using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace coreService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.serviceInstaller1.Description = GetServiceNameAppConfig("ServiceDescription"); ;
            this.serviceInstaller1.DisplayName = GetServiceNameAppConfig("ServiceDescription"); ;
            this.serviceInstaller1.ServiceName = GetServiceNameAppConfig("ServiceName"); ;
        }

        public string GetServiceNameAppConfig(string serviceName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(ProjectInstaller)).Location);
            return config.AppSettings.Settings[serviceName].Value;
        }
    }
}
