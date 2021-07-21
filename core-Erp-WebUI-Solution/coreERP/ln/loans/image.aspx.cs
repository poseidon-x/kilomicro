using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace coreERP.ln.loans
{
    public partial class image : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["id"] != null)
            {
                int id = int.Parse(Request.Params["id"]);
                var d = (new coreLogic.coreLoansEntities()).images.FirstOrDefault(p =>
                    p.imageID == id);
                if (d != null)
                {
                    var img = d.image1;
                    if (Request.Params["h"] != null && Request.Params["w"] != null)
                    {
                        img = Resize(img, int.Parse(Request.Params["w"]), int.Parse(Request.Params["h"]));
                    }
                    Response.Clear();
                    Response.ContentType = d.content_type;
                    Response.OutputStream.Write(img, 0, img.Length);
                    Response.End();
                }
                else
                {
                    try
                    {
                        var fs = new FileStream(Server.MapPath("/images/noimage.jpg"), FileMode.Open);
                        Response.Clear();
                        Response.ContentType = "image/jpeg";
                        byte[] img = new byte[fs.Length];
                        fs.Read(img, 0, (int)fs.Length);
                        fs.Close();
                        Response.OutputStream.Write(img, 0, img.Length);
                        Response.End();
                    }
                    catch (Exception) { }
                }
            }
            else if (Request.Params["cid"] != null)
            {
                try
                {
                    int id = int.Parse(Request.Params["cid"]);
                    var c = (new coreLogic.coreLoansEntities()).clients.FirstOrDefault(p => p.clientID == id);
                    //c.clientImages.Load();
                    var ci = c.clientImages.FirstOrDefault();

                    if (ci != null)
                    {
                        //c//i.imageReference.Load();
                        var d = ci.image;
                        var img = d.image1;
                        if (Request.Params["h"] != null && Request.Params["w"] != null)
                        {
                            img = Resize(img, int.Parse(Request.Params["w"]), int.Parse(Request.Params["h"]));
                        }
                        Response.Clear();
                        Response.ContentType = d.content_type;
                        Response.OutputStream.Write(img, 0, img.Length);
                        Response.End();
                    }
                    else
                    {
                        try
                        {
                            var fs = new FileStream(Server.MapPath("/images/noimage.jpg"), FileMode.Open);
                            Response.Clear();
                            Response.ContentType = "image/jpeg";
                            byte[] img = new byte[fs.Length];
                            fs.Read(img, 0, (int)fs.Length);
                            fs.Close();
                            if (Request.Params["h"] != null && Request.Params["w"] != null)
                            {
                                img = Resize(img, int.Parse(Request.Params["w"]), int.Parse(Request.Params["h"]));
                            }
                            Response.OutputStream.Write(img, 0, img.Length);
                            Response.End();
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception) { }
            }
        }

        private byte[] Resize(byte[] ib, int h, int w)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(ib);
            System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
            i2 = i2.GetThumbnailImage(w, h, null, IntPtr.Zero);
            ms = null;
            ms = new System.IO.MemoryStream();
            i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] b = ms.ToArray();
            i2 = null;
            ms = null;

            return b;
        }
    }
}