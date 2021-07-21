using System;
using System.IO;
using System.Web;
using CrystalDecisions.Web;


namespace WebReporting.Code
{
    /// <summary>
    /// Rewrite of the CrystalImageHandler to work around a CR bug.
    /// </summary>
    public class CrystalImageHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }


        public void ProcessRequest(HttpContext Context)
        {
            string fileName = null;
            try
            {
                fileName = string.Format("{0}{1}", ViewerGlobal.GetImageDirectory(), Context.Request.QueryString["dynamicimage"]);


                var i = fileName.LastIndexOf('.');
                var contentType = i >= 0 ? string.Format("image/{0}", fileName.Substring(i + 1)) : "image/gif";


                Context.Response.Clear();
                Context.Response.ContentType = contentType;
                Context.Response.AppendHeader("Expires", DateTime.Today.AddYears(1).ToString("r"));
                Context.Response.WriteFile(fileName);
                Context.Response.Flush();
            }
            catch
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 404;
                Context.Response.Flush();
                Context.Response.End();
            }
            finally
            {
                if (fileName != null)
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}