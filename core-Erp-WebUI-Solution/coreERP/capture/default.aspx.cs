using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.capture
{
    public partial class _default : System.Web.UI.Page
    {
        public string posted = "false";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["d"] != null && Request.Params["d"] == "1")
            {
                if (Session["imageFromCamera"] != null)
                {
                    byte[] b = Convert.FromBase64String(Session["imageFromCamera"].ToString());
                    Response.Clear();
                    Response.ContentType = "image/png";
                    Response.OutputStream.Write(b, 0, (int)b.Length);
                    Response.End();
                    return;
                }
            }
            if (IsPostBack)
            {
                if (pictureB64.Value != null && pictureB64.Value != "")
                {
                    byte[] b = Convert.FromBase64String(pictureB64.Value);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                    System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
                    i2 = i2.GetThumbnailImage(480, 480, null, IntPtr.Zero);
                    ms = null;
                    ms = new System.IO.MemoryStream();
                    i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    b = ms.ToArray();
                    i2 = null;
                    ms = null;
                    Session["imageFromCamera"] = Convert.ToBase64String(b);  
                    posted = "true";
                    pictureB64.Value = "";
                }
            }
        }
    }
}