using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.errors
{
    public partial class GenericErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            try
            {
                // Get the last error from the server
                Exception ex = Server.GetLastError();
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                // Create a safe message
                string safeMsg = "A problem has occurred in the web site. ";

                // Show Inner Exception fields for local access
                if (ex.InnerException != null)
                {
                    innerTrace.Text = ex.InnerException.StackTrace;
                    InnerErrorPanel.Visible = Request.IsLocal;
                    innerMessage.Text = ex.InnerException.Message;
                }
                // Show Trace for local access
                //if (User.IsInRole("admin"))
                    exTrace.Visible = true;

                // Fill the page fields
                exMessage.Text = ex.Message;
                exTrace.Text = ex.StackTrace;

                coreERP.Global.LogException(ex);

                // Clear the error from the server
                Server.ClearError();
            }
            catch (Exception) { }
        }
    }
}