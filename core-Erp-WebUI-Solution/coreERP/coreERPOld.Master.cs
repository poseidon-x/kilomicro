using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;
//using Telerik.Web.Device.Detection;

namespace coreERP
{
    public partial class coreERPMasterOld : System.Web.UI.MasterPage
    { 
        coreSecurityEntities ent = new coreSecurityEntities();
        private modules module = null;
        public string RedirectUrl = "";

        protected void Page_Init()
        {
            
        }

        private void ShowNote(string text, string title, string icon, string redirectUrl)
        {
            rn1.Text = text;
            rn1.Title = title;
            rn1.ContentIcon = icon;
            rn1.TitleIcon = "info";
            if (redirectUrl != "" && redirectUrl != null)
            {
                RedirectUrl = redirectUrl;
            }
            else
            {
                RedirectUrl = "";
            }
            rn1.Show();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
           
        }
         
        protected void Page_LoadComplete(object sender, EventArgs args)
        {
            try
            {
                if (Session["Notification"] != null)
                {
                    var not = Session["Notification"] as Notification;
                    if (not != null)
                    {
                        ShowNote(not.Text, not.Title, not.Icon.ToString(), not.RedirectUrl);
                        Session["Notification"] = null;
                    }
                }
            }
            catch (Exception) { }
        }

        public string getToken()
        {
            string token = "INVALID_TOKEN";

            try
            {
                using (var ent = new coreSecurityEntities())
                {
                    var tkn = ent.authTokens.Where(p => p.userName.Trim() == Context.User.Identity.Name.Trim()
                        && p.expiryDate > DateTime.Now
                        && Context.Request.UserHostName == p.clientHostName)
                        .OrderByDescending(p => p.expiryDate)
                        .FirstOrDefault();
                    if (tkn != null)
                    {
                        token = tkn.token;
                    }
                }
            }
            catch (Exception) { }

            return token;
        }

        private string kendoTheme = "default";
        public string getKendoTheme()
        {
            return kendoTheme;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (getToken() == "INVALID_TOKEN") Response.Redirect("/security/login.aspx");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Page.LoadComplete -= new EventHandler(this.Page_LoadComplete);
            this.Page.LoadComplete += new EventHandler(this.Page_LoadComplete);
            this.Page.PreLoad +=  new EventHandler(Page_PreInit);
            this.Page.Error += Page_Error;
            if (!Page.IsPostBack)
            {
                try
                { 
                    var usr = ent.users.FirstOrDefault(p => p.user_name.ToLower() == Context.User.Identity.Name.ToLower());
                    if (usr != null && (usr.last_password_changed_date==null || usr.last_password_changed_date<
                        DateTime.Now.AddMonths(-1)) && Request.Url.LocalPath.ToLower().Contains("settings.aspx")==false)
                    {
                        Response.Redirect("~/profile/settings.aspx?ch=1");
                    }
                    if (usr != null)
                    {
                        usr.last_activity_date = DateTime.Now;
                        usr.is_onLine = true;
                        ent.SaveChanges();
                    }
                }
                catch (Exception) { }
                try
                {
                    this.RadSkinManager1.Skin = System.Configuration.ConfigurationManager.AppSettings["UI.Skin"];
                }
                catch (Exception ex) { }
                try
                {
                    var url = Request.Url.LocalPath.ToLower();
                    var fullurl = "~" + url;
                    var mod = ent.modules.FirstOrDefault(p => p.url.ToLower() == fullurl);
                    if (mod == null) mod = ent.modules.FirstOrDefault(p => p.url.ToLower() == url);  
                    var allowed=true;
                    if (mod != null)
                    {
                        module = mod;
                        if (url.ToLower()!="/" && url.ToLower()!="/default.aspx" && !CanView && fullurl != "~/security/unauthorized.aspx")
                        {
                            allowed = false;
                            Response.Redirect("~/security/unauthorized.aspx");
                        }
                        else
                        { 
                            ent.SaveChanges();
                        }

                    }
                    else if (url.Contains("/admin/"))
                    {
                        if (!Context.User.IsInRole("admin") && !Context.User.IsInRole("itadmin"))
                        {
                            allowed = false;
                            Response.Redirect("~/security/unauthorized.aspx");
                        }
                    }
                    try
                    {
                        int? moduleID = null;
                        if (module != null) moduleID = module.module_id;
                        var ua = new userAudit
                        {
                            moduleID = moduleID,
                            userName = Context.User.Identity.Name,
                            actionDateTime = DateTime.Now,
                            allowed = allowed,
                            url = url
                        };
                        ent.userAudits.Add(ua);
                        ent.SaveChanges();                    }
                    catch (Exception) { }
                    try
                    {
                        using (var en = new core_dbEntities())
                        {
                            var ip = en.interfacePreferences.FirstOrDefault(p => p.userName.ToLower().Trim() == HttpContext.Current.User.Identity.Name.Trim().ToLower());
                            if (ip != null)
                            {
                                if (
                                    ip.skinName.ToLower().Contains("hay")
                                    || ip.skinName.ToLower().Contains("forest")
                                    || ip.skinName.ToLower().Contains("transparent")
                                    )
                                {
                                    RadSkinManager1.Skin = ip.skinName;
                                    SetES(this.Page, false);
                                    (new SkinManager(RadStyleSheetManager1)).Register(ip.skinName);
                                }
                                else
                                {
                                    RadSkinManager1.Skin = ip.skinName;
                                }
                                if (ip.skinName.ToLower() == "metro"
                                    || ip.skinName.ToLower() == "metrotouch")
                                    kendoTheme = "metro";
                                else if (ip.skinName.ToLower() == "black"
                                    || ip.skinName.ToLower() == "blackmetrotouch")
                                    kendoTheme = "metroblack";
                                else if (ip.skinName.ToLower() == "outlook"
                                    || ip.skinName.ToLower() == "office2010blue"
                                    || ip.skinName.ToLower() == "vista"
                                    || ip.skinName.ToLower() == "windows7"
                                    || ip.skinName.ToLower() == "webblue"
                                    || ip.skinName.ToLower() == "web20")
                                    kendoTheme = "blueopal";
                                else if (ip.skinName.ToLower() == "office2010silver"
                                    || ip.skinName.ToLower() == "silk"
                                    || ip.skinName.ToLower() == "simple" )
                                    kendoTheme = "silver";
                                else if (ip.skinName.ToLower() == "glow"
                                    || ip.skinName.ToLower() == "office2010black")
                                    kendoTheme = "black";
                                else if (ip.skinName.ToLower() == "sunset" )
                                    kendoTheme = "highcontrast";
                                else if (ip.skinName.ToLower() == "telerik")
                                    kendoTheme = "bootstrap";
                            }
                            else
                            {
                                RadSkinManager1.Skin = "Metro";
                            }
                        }
                    }
                    catch (Exception) { }
                }
                catch (Exception) { }
            }           
        }

        void Page_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            Exception exc = Server.GetLastError();
            if(exc.InnerException != null){
                exc=exc.InnerException;
            }
            if (exc.InnerException != null)
            {
                exc = exc.InnerException;
            }
            if (exc.InnerException != null)
            {
                exc = exc.InnerException;
            }
            if (exc.GetType() != typeof(System.Threading.ThreadAbortException))
            { 
                if (exc.GetType() == typeof(HttpException))
                {  
                    if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                        return;

                    var hex = exc as HttpException;
                    if (hex.GetHttpCode() == 404)
                    {
                        Server.Transfer("/errors/Http404ErrorPage.aspx");
                    }
                    else
                    {
                        Server.Transfer("/errors/HttpErrorPage.aspx");
                    }

                    return;
                }
            }
             
            Server.Transfer("/errors/GenericErrorPage.aspx");
            Context.ClearError();
        }

        private string GetToolTip(Control c)
        {
            string toolTip = "";
            try
            {
                var nameOfProperty = "ToolTip";
                var propertyInfo = c.GetType().GetProperty(nameOfProperty);
                var value = propertyInfo.GetValue(c, null);
                if (value != null)
                {
                    toolTip = value.ToString();
                }
            }
            catch (Exception) { }

            return toolTip;
        }
        private string ClearToolTip(Control c)
        {
            string toolTip = "";
            try
            {
                var nameOfProperty = "ToolTip";
                var propertyInfo = c.GetType().GetProperty(nameOfProperty);
                propertyInfo.SetValue(c, "", null); 
            }
            catch (Exception) { }

            return toolTip;
        }
        public void SetES(Control pc, bool flag)
        {
            string str = "";
            try
            {
                foreach (Control ctl in pc.Controls)
                {
                    var c = ctl.GetType().GetProperty("EnableEmbeddedSkins");
                    if (c != null)
                    {
                        try
                        {
                            c.SetValue(ctl, flag, null);
                        }
                        catch (Exception) { }
                    }
                    SetES(ctl, flag);
                }
            }
            catch (Exception) { } 
        } 
        public bool CanView
        {
            get
            {
                return Authorizer.IsUserAuthorized(Context.User.Identity.Name,
                    "V", module.module_id);
            }
        }
        public string LogOutPrompt()
        {
            try
            {
                return "Logout ";// +Context.User.Identity.Name;
            }
            catch (Exception x)
            {
                return "Logout";
            }
        }

        private void ManageException(Exception ex)
        {
             
        } 


        protected void OnAjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void OnAjaxRequest2(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void ajax1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            ajax1.Controls.Add(Page.LoadControl("/uc/quicMenu.ascx"));
        }

        protected void ajax2_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            ajax2.Controls.Add(Page.LoadControl("/uc/quicMenuGL.ascx"));
        }

        protected void RadScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {

        }
    }
}
