using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.profile
{
    public partial class settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Params["ch"] == "1")
                {
                    h2.InnerText = "Your password has expired please change it";
                }
            }
        }

        protected void chPassword_ChangingPassword(object sender, LoginCancelEventArgs e)
        {
            var strength=PasswordAdvisor.CheckStrength(chPassword.NewPassword);
            if (chPassword.UserName == chPassword.NewPassword)
            {
                lblMessage.Text = "Password cannot be the same as the username.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                e.Cancel = true;
            }
            else if (strength == PasswordScore.Blank || strength == PasswordScore.VeryWeak)
            {
                lblMessage.Text = "Password too weak. Try a stronger password with numbers and special symbols.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                e.Cancel = true;
            }
            else if ((new coreLogic.coreSecurityEntities()).users.FirstOrDefault(p=>p.user_name.ToLower()==User.Identity.Name.ToLower()).last_password_changed_date!=null &&
                chPassword.NewPassword.Trim() == chPassword.CurrentPassword.Trim())
            {
                lblMessage.Text = "You cannot re-use your last password";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                e.Cancel = true;
            }
        }

        protected void chPassword_ChangedPassword(object sender, EventArgs e)
        {

        }
    }
}
