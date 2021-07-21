using System; 
using System.Linq; 
using System.Web.Security; 

namespace coreERP.security
{
    public partial class logMeOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Session.Clear();
            }
            catch (Exception) { }
        }
    }
}