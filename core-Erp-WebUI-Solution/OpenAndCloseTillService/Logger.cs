
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using System.IO;

namespace OpenAndCloseTillService
{
    public class Logger
        {
            public static string logName = "OpenAndCloseTill_Errors";
            public static bool VerboseLogging { get; set; }
            private static object locker = new object();

            public static void logError(Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                lock (locker)
                {
                    var path = System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"];
                    CheckDir(path);
                    path = Path.Combine(path, logName);
                    CheckDir(path);
                    path = Path.Combine(path, "Error");
                    CheckDir(path);
                    var logFile = Path.Combine(path,
                        "OpenAndCloseTill_Errors_" + DateTime.Today.ToString("yyyyMMdd") + ".log");
                    var message = VerboseLogging
                        ? x.ToString() + "\n"
                          + x.StackTrace
                        : x.Message;
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
                    var path = System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"];
                    CheckDir(path);
                    path = Path.Combine(path, logName);
                    CheckDir(path);
                    path = Path.Combine(path, "Info");
                    CheckDir(path);
                    var logFile = Path.Combine(path,
                        "OpenAndCloseTill_Errors_" + DateTime.Today.ToString("yyyyMMdd") + ".log");
                    using (var sw = new StreamWriter(logFile, true))
                    {
                        sw.WriteLine(message);
                    }
                }
            }

            public static void logInfo(string message)
            {
                lock (locker)
                {
                    var path = System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"];
                    CheckDir(path);
                    path = Path.Combine(path, logName);
                    CheckDir(path);
                    path = Path.Combine(path, "Info");
                    CheckDir(path);
                    var logFile = Path.Combine(path,
                        "OpenAndCloseTill_Errors_" + DateTime.Today.ToString("yyyyMMdd") + ".log");
                    using (var sw = new StreamWriter(logFile, true))
                    {
                        sw.WriteLine(message);
                    }
                }
            }

            public static void CheckDir(string path)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }
    }

