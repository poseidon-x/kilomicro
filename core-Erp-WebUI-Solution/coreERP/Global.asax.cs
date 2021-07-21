using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using coreLogic;
using System.Linq;
using System.Data.Entity;
using coreLicenseLib;
using System.Web.Mvc;
using System.Web.Routing;
using Quartz;
using Quartz.Impl;
using coreERP.code;
using coreERP.Models;

namespace coreERP
{
    public class Global : System.Web.HttpApplication
    {
        public static Dictionary<string, string> htUsers_ConIds = new Dictionary<string, string>();

        private class LicenseInfo
        {
            public string Path { get; set; }
            public LicenseState LicenseState { get; set; }
            public DateTime RefreshedDate { get; set; }
        }

        public class UserNoteRecord
        {
            public string Username { get; set; }
            public string UserClientHostname { get; set; }
            public DateTime lastNotified { get; set; }
        }

        public static Dictionary<string, UserNoteRecord> UserNoteRecords = new Dictionary<string, UserNoteRecord>();

        private List<LicenseInfo> License;
        public void ControllerNamespaces()
        {
            ControllerBuilder.Current.DefaultNamespaces.Add(
                "coreErp.UiControllers");
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            ControllerNamespaces();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*allasmx}", new { allasmx = @".*\.asmx(/.*)?" });
            routes.IgnoreRoute("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
            routes.IgnoreRoute("{*allico}", new { allico = @".*\.ico(/.*)?" });
            routes.IgnoreRoute("{*alljpg}", new { alljpg = @".*\.jpg(/.*)?" });
            routes.IgnoreRoute("{*allpng}", new { allpng = @".*\.png(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapPageRoute("Default2", String.Empty, "~/Default.aspx");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var p = Request.Path.ToLower().Trim();
            if (p.EndsWith("/crystalimagehandler.aspx") && p != "/crystalimagehandler.aspx")
            {
                var fullPath = Request.Url.AbsoluteUri.ToLower();
                var index = fullPath.IndexOf("/crystalimagehandler.aspx");
                Response.Redirect(fullPath.Substring(index));
            }
        }

        public string getToken(IcoreSecurityEntities ent)
        {
            string token = "INVALID_TOKEN";

            try
            {
                var userProp = LoggedUser.UserName;
                var user = User?.Identity.Name?.ToLower()?.Trim();
                var userName = string.IsNullOrWhiteSpace(userProp) ? user : userProp;
                var tkn = ent.authTokens.Where(p => p.userName.Trim() == userName //Context.User.Identity.Name.Trim()
                                                    && p.expiryDate > DateTime.Now
                                                    && Context.Request.UserHostName == p.clientHostName)
                    .OrderByDescending(p => p.expiryDate)
                    .FirstOrDefault();
                if (tkn != null)
                {
                    token = tkn.token;
                }
            }
            catch (Exception x)
            {
            }

            return token;
        }

        private void PreprocessRequest()
        {
            using (var ent = new coreSecurityEntities())
            {
                var userProp = LoggedUser.UserName;
                var logUser = User?.Identity?.Name?.ToLower()?.Trim();
                var userName = string.IsNullOrWhiteSpace(userProp) ? logUser : userProp;
                var token = getToken(ent);
                if (token == "INVALID_TOKEN") { Response.Redirect("/Security/Login"); }
                try
                {

                    var usr =
                        ent.users.FirstOrDefault(p => p.user_name.ToLower() == userName /*Context.User.Identity.Name.ToLower()*/);
                    if (usr != null && (usr.last_password_changed_date == null || usr.last_password_changed_date <
                                        DateTime.Now.AddMonths(-1)) &&
                        Request.Url.LocalPath.ToLower().Contains("settings.aspx") == false)
                    {
                        Response.Redirect("~/profile/settings.aspx?ch=1");
                    }
                    if (usr != null)
                    {
                        usr.last_activity_date = DateTime.Now;
                        usr.is_onLine = true;
                        //Application["userName"] = userName;
                        ent.SaveChanges();
                    }
                }
                catch (Exception x)
                {
                }

                var url = Request.Url.LocalPath.ToLower();
                var fullurl = "~" + url;
                var mod = ent.modules.FirstOrDefault(p => p.url.ToLower() == fullurl);
                if (mod == null) mod = ent.modules.FirstOrDefault(p => p.url.ToLower() == url);
                var allowed = true;
                var module = mod;
                if (mod != null)
                {
                    if (url.ToLower() != "/" && (!url.Contains("/admin/"))
                        && url.ToLower() != "/default.aspx" &&
                        !CanView(module.module_id) && fullurl != "~/security/unauthorized.aspx")
                    {
                        allowed = false;
                        Response.Redirect("~/security/unauthorized.aspx");
                    }
                    else
                    {
                        ent.SaveChanges();
                    }

                }
                try
                {
                    int? moduleID = null;
                    if (module != null) moduleID = module.module_id;
                    var ua = new userAudit
                    {
                        moduleID = moduleID,
                        userName = userName, //Context.User.Identity.Name,
                        actionDateTime = DateTime.Now,
                        allowed = allowed,
                        url = url
                    };
                    ent.userAudits.Add(ua);
                    ent.SaveChanges();
                }
                catch (Exception x) { }

            }
        }

        public bool CanView(int moduleId)
        {
            return Authorizer.IsUserAuthorized(Context.User.Identity.Name,
                "V", moduleId);
        }
        ///Security/Login
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                var p = Request.Path.ToLower().Trim();
                if (p.EndsWith(".axd")
                    || p.EndsWith(".jpg")
                    || p.EndsWith(".jpeg")
                    || p.EndsWith(".css")
                    || p.EndsWith(".js")
                    || p.EndsWith(".png")
                    || p.EndsWith(".asmx")
                    || Request.Path.Contains("invalid_lic.aspx")
                    || Request.Path.Contains("login.aspx")
                    || Request.Path.Contains("Login")
                    || p == "/")
                {
                    return;
                }
                PreprocessRequest();
                License = Application["License"] as List<LicenseInfo>;
                var path = "";
                if (License == null)
                {
                    License = new List<LicenseInfo>();
                }
                BitField bf = new BitField();
                if (Request.Path.Contains("/gl/"))
                {
                    bf.SetOn(BitField.Flag.f1);
                    path = "/gl/";
                }
                else if (Request.Path.Contains("/ar/") && !Request.Path.EndsWith("/cust.aspx"))
                {
                    bf.SetOn(BitField.Flag.f2);
                    path = "/ar/";
                }
                else if (Request.Path.Contains("/ap/") && !Request.Path.EndsWith("/sup.aspx"))
                {
                    bf.SetOn(BitField.Flag.f3);
                    path = "/ap/";
                }
                else if (Request.Path.Contains("/tr/"))
                {
                    bf.SetOn(BitField.Flag.f4);
                    path = "/tr/";
                }
                else if (Request.Path.Contains("/hc/"))
                {
                    bf.SetOn(BitField.Flag.f5);
                    path = "/hc/";
                }
                else if (Request.Path.Contains("/iv/"))
                {
                    bf.SetOn(BitField.Flag.f6);
                    path = "/iv/";
                }
                else if (Request.Path.Contains("/so/"))
                {
                    bf.SetOn(BitField.Flag.f7);
                    path = "/so/";
                }
                else if (Request.Path.Contains("/po/"))
                {
                    bf.SetOn(BitField.Flag.f8);
                    path = "/po/";
                }
                else if (Request.Path.Contains("/fa/") && Request.Path.ToLower().Contains("staff.aspx") == false)
                {
                    bf.SetOn(BitField.Flag.f9);
                    path = "/fa/";
                }
                else if (Request.Path.Contains("/ln/") && !Request.Path.Contains("/susu/"))
                {
                    bf.SetOn(BitField.Flag.f11);
                    path = "/ln/";
                }
                else if (Request.Path.Contains("/ln/client/") || (Request.Path.Contains("/ln/susu/")) || Request.Path.Contains("/ln/setup/") || Request.Path.Contains("/ln/cashier/"))
                {
                    bf.SetOn(BitField.Flag.f10);
                    path = "/ln/susu/";
                }
                var Lic = License.FirstOrDefault(q => q.Path == path);
                if (Lic == null || (DateTime.Now - Lic.RefreshedDate).TotalMinutes > 182)
                {
                    Lic = new LicenseInfo();
                    if ((Request.Path.Contains(".aspx") || Request.Path.Contains(".ASPX")
                        || Request.Path.Contains(".Aspx")
                        ) && (!Request.Path.Contains("invalid_lic.aspx")) && (!Request.Path.Contains("login.aspx"))
                        && (!(new core_dbEntities()).comp_prof.FirstOrDefault().comp_name.Contains("Pinnacle"))
                        && (Environment.OSVersion.Version >= Version.Parse("6.1"))
                        )
                    {
                        coreLicense lic = new coreLicense();
                        var d1 = "";
                        var d2 = "";
                        var licSt = lic.IsAuthorized(Settings.companyName, bf.Mask, ref d1, ref d2);
                        Lic.LicenseState = licSt;
                        Lic.RefreshedDate = DateTime.Now;
                        License.Add(Lic);
                        Application["License"] = License;
                    }
                    else if ((!Request.Path.Contains("invalid_lic.aspx")) && (!Request.Path.Contains("login.aspx")) && (Request.Path != "/"))
                    {
                        Lic.LicenseState = LicenseState.ValidLicense;
                        Lic.RefreshedDate = DateTime.Now;
                        License.Add(Lic);
                        Application["License"] = License;
                    }
                }

                if (Lic.LicenseState == LicenseState.ValidLicense)
                {
                }
                else if (Request.Path.EndsWith(".css") || Request.Path.EndsWith(".js")
                     || Request.Path.EndsWith(".axd")
                || Request.Path.EndsWith(".jpg") || Request.Path.Contains("invalid_lic.aspx")
                )
                {
                }
                else if (Request.Path.Contains("/security/") || Request.Path.Contains("prof.aspx"))
                {
                    if (Lic.LicenseState != LicenseState.NotForCompany && Lic.LicenseState != LicenseState.NoLicenseFile)
                    {
                        Response.Redirect("~/security/invalid_lic.aspx?licSt=" + Lic.LicenseState.ToString());
                    }
                }
                else
                {
                    Response.Redirect("~/security/invalid_lic.aspx?licSt=" + Lic.LicenseState.ToString());
                }
            }
            catch (Exception ex) { }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            try
            {
                // Get the exception object.
                Exception exc = Server.GetLastError();
                if (exc.GetType() != typeof(System.Threading.ThreadAbortException))
                {
                    // Handle HTTP errors
                    if (exc.GetType() == typeof(HttpException))
                    {
                        // The Complete Error Handling Example generates
                        // some errors using URLs with "NoCatch" in them;
                        // ignore these here to simulate what would happen
                        // if a global.asax handler were not implemented.
                        if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                            return;

                        //Redirect HTTP errors to HttpError page
                        var hex = exc as HttpException;
                        if (hex.GetHttpCode() == 404)
                        {
                            Server.Transfer("/errors/Http404ErrorPage.aspx");
                        }
                        else
                        {
                            Server.Transfer("/errors/HttpErrorPage.aspx");
                        }
                    }

                    Server.Transfer("/errors/GenericErrorPage.aspx");

                }
            }
            catch (Exception x) { Response.Write("An Error Occured"); }
        }

        public static void LogException(Exception x)
        {
            try
            {
                var username = LoggedUser.UserName;

                Exception ex = x;
                if (x.InnerException != null)
                {
                    ex = x.InnerException;
                    if (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                }
                var url = HttpContext.Current.Request.Url.LocalPath.ToLower();
                coreSecurityEntities sec = new coreSecurityEntities();
                var log = new userError
                {
                    errorDate = DateTime.Now,
                    exceptionMessage = ex.Message +
                        ((ex.InnerException != null) ? "Inner: " + ex.InnerException.Message : ""),
                    exceptionStackTrace = ex.StackTrace,
                    url = url,
                    userName = HttpContext.Current?.User?.Identity?.Name ?? username
                };
                sec.userErrors.Add(log);
                sec.SaveChanges();
            }
            catch (Exception x2)
            {
            }
        }
        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                string username = "";
                if (Session["userName"] != null)
                {
                    username = Session["userName"].ToString();
                }
                else
                {
                    var userProp = LoggedUser.UserName;
                    var logUser = User?.Identity?.Name?.ToLower()?.Trim();
                    username = string.IsNullOrWhiteSpace(userProp) ? logUser : userProp;
                }
                coreSecurityEntities ent = new coreSecurityEntities();
                users user = ent.users.FirstOrDefault(p => p.user_name == username);
                if (user != null)
                {
                    user.is_onLine = false;
                    ent.SaveChanges();
                }
            }
            catch (Exception) { }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private static Dictionary<string, Ack> acks =
            new Dictionary<string, Ack>();

        public static Dictionary<string, Ack> Acks
        {
            get
            {
                return acks;
            }
        }

        public class Ack
        {
            public string UserName { get; set; }
            public DateTime LastAck { get; set; }
        }
    }
}