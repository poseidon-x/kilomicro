using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using coreERP.code;
using Telerik.Web.UI;

namespace coreERP
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            form1.Attributes.Add("autocomplete", "off");            
            Login1.TitleText = Settings.companyName;
            if (User.Identity.IsAuthenticated == true && Request.QueryString["ReturnUrl"] != null)
            {
                Response.Redirect("~/security/unauthorized.aspx?url=" + Request.QueryString["ReturnUrl"]);
            }
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            if (Request.QueryString["ReturnUrl"] != null && Request.QueryString["ReturnUrl"] != "")
            {
                Response.Redirect(Request.QueryString["ReturnUrl"]);
            }
        }
    }
}