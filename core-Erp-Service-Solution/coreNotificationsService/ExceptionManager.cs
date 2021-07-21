using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsService
{
    public class ExceptionManager
    {
        public static void LogException(Exception x, string methodName)
        {
            try
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                try
                {
                    if (!System.Diagnostics.EventLog.Exists("coreERP2")) System.Diagnostics.EventLog.CreateEventSource("coreERP2", "Application");
                }
                catch (Exception x2) { }
                System.Diagnostics.EventLog.WriteEntry("coreERP2", methodName + ": " + x.Message, System.Diagnostics.EventLogEntryType.Error,
                    1, 1, ASCIIEncoding.ASCII.GetBytes(x.StackTrace));
            }
            catch (Exception x2)
            {
            }
        }
        public static void LogInformation(string methodName)
        {
            try
            {
                //if (!System.Diagnostics.EventLog.Exists("coreERP2")) System.Diagnostics.EventLog.CreateEventSource("coreERP2", "Application");
                System.Diagnostics.EventLog.WriteEntry("coreERP2", methodName, System.Diagnostics.EventLogEntryType.Information,
                    1, 1);
            }
            catch (Exception x2)
            {
            }
        }
    }
}
