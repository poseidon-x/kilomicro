using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace coreNotificationsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG

            notificationService not = new notificationService();
            not.OnDebug();
            not.OnDebugStop();
            Thread.Sleep(Timeout.Infinite);


#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new notificationService() 
            };
            ServiceBase.Run(ServicesToRun);

#endif
        }
    }
}
