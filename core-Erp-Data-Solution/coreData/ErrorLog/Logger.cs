using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace coreData.ErrorLog
{
    public class Logger
    {
        public static bool VerboseLogging { get; set; }
        private static object locker = new object();

        public static void logError(Exception x)
        {
            lock (locker)
            {
                var logFile = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"],
                    "coreErpErrors_" + DateTime.Today.ToString("yyyyMMdd") + ".log");
                var message = true
                    ? x.ToString() + "\n"
                      + x.StackTrace
                    : x.Message;
                using (var sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public static void serviceError(string message)
        {
            lock (locker)
            {
                var logFile = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["SERVICE_LOG_DIRECTORY"],
                    "coreErpServiceLogs_" + DateTime.Today.ToString("yyyyMMdd") + ".log");
                using (var sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public static void informationLog(string message)
        {
            lock (locker)
            {
                var logFile = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["COREERP_LOG_DIRECTORY"],
                    "coreErpInfoLogs_" + DateTime.Today.ToString("yyyyMMdd") + ".log");
                using (var sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public static void logError(string message)
        {
            lock (locker)
            {
                var logFile = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"],
                    "coreErpValidationErrors" + DateTime.Today.ToString("yyyyMMdd") + ".log");
                using (var sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(message);
                }
            }
        }

        public static void logError(List<string> message)
        {
            lock (locker)
            {
                var logFile = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"],
                    "coreErpValidationErrors" + DateTime.Today.ToString("yyyyMMdd") + ".log");
                using (var sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}
