using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.security
{

    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var username = User.Identity.Name;
            FormsAuthentication.SignOut();
            try
            {
                coreSecurityEntities ent = new coreSecurityEntities();
                users user = ent.users.FirstOrDefault(p => p.user_name == username);
                if (user != null)
                {
                    user.is_onLine = false;

                    //Expire All Unexpired Tokens
                    var unexpiredTokens = ent.authTokens
                            .Where(p => p.userName == user.user_name
                                && p.expiryDate > DateTime.Now
                                && Context.Request.UserHostName == p.clientHostName)
                            .ToList();
                    foreach (var token in unexpiredTokens)
                    {
                        token.expiryDate = DateTime.Now;
                    }

                    ent.SaveChanges();
                }
            }
            catch (Exception) { }
            Session.Abandon();
            //Response.Redirect("~/security/login.aspx?url=");
            Response.Redirect("/Security/Login");

        }
    }
}