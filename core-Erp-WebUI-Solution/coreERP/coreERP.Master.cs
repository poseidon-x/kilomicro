using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;
using coreERP.Models;
//using Telerik.Web.Device.Detection;

namespace coreERP
{
    public partial class coreERPMaster : System.Web.UI.MasterPage
    { 
        coreSecurityEntities ent = new coreSecurityEntities();
        private modules module = null;
        public string RedirectUrl = "";
        public string showMessage = "false";
        public string showNote = "false";
        protected void Page_Init()
        {
            rn1.Text = "";
            rn1.Title = "";
            rn1.VisibleOnPageLoad = false;
        }

        protected string UserFullname()
        {
            string userName = HttpContext.Current.User.Identity.Name;

            try
            {
                using (var ent = new coreSecurityEntities())
                {
                    var user = ent.users.FirstOrDefault(p => p.user_name == userName);
                    userName = user.full_name;
                }
            }
            catch (Exception ) { }
            return userName;
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
            rn1.VisibleOnPageLoad = true;
            rn1.Show();
            showMessage = "true";
            showNote = "true";
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            ClearNotes();
        }

        private void ClearNotes()
        {
            Session["Notification"] = null;
        }

        protected void Page_LoadComplete(object sender, EventArgs args)
        {
            ProcessNotes();
        }

        private void ProcessNotes()
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
            catch (Exception x) { }
        }
        public string getToken()
        {
            string token = "INVALID_TOKEN";

            try
            {
                var user = Context?.User?.Identity?.Name?.Trim()?.ToLower();
                var userProp = LoggedUser.UserName;
                var userName = string.IsNullOrWhiteSpace(userProp) ? user.ToString() : userProp;
                using (var ent = new coreSecurityEntities())
                {
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
            }
            catch (Exception x) { }

            return token;
        }

        private string kendoTheme = "bootstrap";
        public string getKendoTheme()
        {
            if (kendoTheme == "metro" && Session["kendoTheme"] != null)
            {
                kendoTheme = Session["kendoTheme"].ToString();
            }
            return kendoTheme;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // if (getToken() == "INVALID_TOKEN") Response.Redirect("/security/login.aspx");
            if (getToken() == "INVALID_TOKEN")
            {
                Response.Redirect("/Security/Login");
            }
            

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Page.LoadComplete -= new EventHandler(this.Page_LoadComplete);
            this.Page.LoadComplete += new EventHandler(this.Page_LoadComplete);
            this.Page.PreLoad +=  new EventHandler(Page_PreInit);
            this.Page.Error += Page_Error;
            if (!Page.IsPostBack)
            {

                try
                {
                    this.RadSkinManager1.Skin = System.Configuration.ConfigurationManager.AppSettings["UI.Skin"];
                }
                catch (Exception ex)
                {
                }
                try
                {
                    
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
                                    (new SkinManager(styleManager)).Register(ip.skinName);
                                }
                                else
                                {
                                    RadSkinManager1.Skin = ip.skinName;
                                }
                                if (ip.skinName.ToLower() == "metro")
                                    kendoTheme = "material";
                                if (ip.skinName.ToLower() == "metrotouch")
                                    kendoTheme = "metro";
                                else if (ip.skinName.ToLower() == "black")
                                    kendoTheme = "materialblack";
                                if (ip.skinName.ToLower() == "blackmetrotouch")
                                    kendoTheme = "metroblack";
                                else if (ip.skinName.ToLower() == "outlook")
                                    kendoTheme = "highContrast";
                                else if (ip.skinName.ToLower() == "office2010blue"
                                    || ip.skinName.ToLower() == "vista")
                                    kendoTheme = "moonlight";
                                else if (ip.skinName.ToLower() == "windows7"
                                    || ip.skinName.ToLower() == "webblue")
                                    kendoTheme = "blueopal";
                                else if (ip.skinName.ToLower() == "web20")
                                    kendoTheme = "bootstrap";
                                else if (ip.skinName.ToLower() == "office2010silver")
                                    kendoTheme = "uniform";
                                else if (ip.skinName.ToLower() == "silk"
                                    || ip.skinName.ToLower() == "simple")
                                    kendoTheme = "silver";
                                else if (ip.skinName.ToLower() == "glow"
                                    || ip.skinName.ToLower() == "office2010black")
                                    kendoTheme = "black";
                                else if (ip.skinName.ToLower() == "sunset")
                                    kendoTheme = "highcontrast";
                                else if (ip.skinName.ToLower() == "telerik")
                                    kendoTheme = "bootstrap";
                                Session["kendoTheme"] = kendoTheme;
                            }
                            else
                            {
                                RadSkinManager1.Skin = "Bootstrap";
                            }
                        }
                    }
                    catch (Exception x) { }
                }
                catch (Exception x) { }
                
            }   
            ProcessNotes();
        }

        private void Page_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            // Get the exception object.
            Exception exc = Server.GetLastError();
            if (exc.GetType() is ThreadAbortException)
            {
                return;
            }

    if (exc.InnerException != null){
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
            catch (Exception x) { }

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
            catch (Exception x) { }

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
                        catch (Exception x) { }
                    }
                    SetES(ctl, flag);
                }
            }
            catch (Exception x) { } 
        } 
        
        public string LogOutPrompt()
        {
            try
            {
                return "Logout";// +Context.User.Identity.Name;
            }
            catch (Exception x)
            {
                return "Logout";
            }
        }

        private void ManageException(Exception ex)
        {
             
        } 
 
        protected void RadScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {

        }
    }
}
