using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;

namespace coreERP
{
    public class HtmlHelper
    {
        public static DateTime stamp = DateTime.Now;
        public static void MessageBox(string message, string Title = "coreERP© powered by ACS", IconType icon = IconType.info)
        {
            if (stamp==DateTime.Now || (DateTime.Now-stamp).TotalMilliseconds>100)
            {
                Notification not = new Notification();
                not.Text = message;
                not.Title = Title;
                not.Icon = icon;
                //HttpContext.Current.Response.Write("<script>alert('" + message + "');</script>");
                HttpContext.Current.Session["Notification"] = not;
                stamp = DateTime.Now;
            }
        }
        public static void MessageBox2(string message, string redirectUrl, string Title = "coreERP© powered by ACS", 
            IconType icon = IconType.info)
        {
            Notification not = new Notification();
            not.Text = message;
            not.Title = Title;
            not.Icon = icon;
            not.RedirectUrl = redirectUrl;
            //HttpContext.Current.Response.Write("<script>alert('" + message + "');"+
            //    " window.location='" + redirectUrl + "';</script>");
            HttpContext.Current.Session["Notification"] = not;
        }
    }

    public enum IconType
    {
        info,
        delete,
        deny,
        edit,
        ok,
        warning,
        none,
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public IconType Icon { get; set; }
        public string RedirectUrl { get; set; }
    }
}
