using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Web;


namespace coreData.ErrorLog
{
    public class CoreInfoLogger
    {
        //public static bool VerboseLogging { get; set; }
        private static object locker = new object();

        //public static void logError(Exception x)
        //{
        //    lock (locker)
        //    {
        //        var logFile = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"],
        //            "coreErpInfo_" + DateTime.Today.ToString("yyyyMMdd") + ".log");
        //        var message = true
        //            ? x.ToString() + "\n"
        //              + x.StackTrace
        //            : x.Message;
        //        using (var sw = new StreamWriter(logFile, true))
        //        {
        //            sw.WriteLine(message);
        //        }
        //    }
        //}
        

        public void logInfo(string message)
        {
            lock (locker)
            {
                var logFile = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"],
                    "coreErpInfo" + DateTime.Today.ToString("yyyyMMdd") + ".log");

                var isRelease = System.Configuration.ConfigurationManager.AppSettings["release"];
                // This text is added only once to the file. 
                if (!File.Exists(logFile))
                {
                    if (isRelease == "Y")
                    {
                        using (StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(logFile), true))
                        {
                            sw.WriteLine(message); // Write the text
                        }
                    }
                    else
                    {
                        // Create a file to write to. 
                        using (var sw = new StreamWriter(logFile, true))
                        {
                            sw.WriteLine(message);
                        }
                    } 
                }
                else {
                    // This text is always added, making the file longer over time 
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(logFile))
                    {
                        sw.WriteLine(message);
                    } 
                }                
            }
        }

        public void logError(Exception message)
        {
            lock (locker)
            {
                var logFile = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LOG_DIRECTORY"],
                    "coreErpError" + DateTime.Today.ToString("yyyyMMdd") + ".log");

                var isRelease = System.Configuration.ConfigurationManager.AppSettings["release"];
                // This text is added only once to the file. 
                if (!File.Exists(logFile))
                {
                    if (isRelease == "Y")
                    {
                        using (StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(logFile), true))
                        {
                            sw.WriteLine(message); // Write the text
                        }
                    }
                    else
                    {
                        // Create a file to write to. 
                        using (var sw = new StreamWriter(logFile, true))
                        {
                            sw.WriteLine(message);
                        }
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time 
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(logFile))
                    {
                        sw.WriteLine(message);
                    }
                }
            }
        }
        
    }
}
