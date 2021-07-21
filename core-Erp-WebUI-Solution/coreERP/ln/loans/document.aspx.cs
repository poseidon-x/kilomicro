using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.loans
{
    public partial class document : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["id"] != null)
            {
                int id = int.Parse(Request.Params["id"]);
                var d = (new coreLogic.coreLoansEntities()).documents.FirstOrDefault(p =>
                    p.documentID == id);
                if (d != null)
                {
                    Response.Clear();
                    Response.ContentType = d.contentType;
                    //Response.Headers.Add("Content-Disposition", "inline; filename=\"" + d.fileName + "\"");
                    Response.OutputStream.Write(d.documentImage, 0, d.documentImage.Length);
                    Response.End();
                }
            }
        }
    }
}