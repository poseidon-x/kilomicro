using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Data.Entity;
using coreLogic;
using System.Linq;
using System.Web.Services;

public partial class SLDash : System.Web.UI.Page
{
    coreLogic.coreLoansEntities le;
    List<CachedModule> mods;
    coreSecurityEntities ent = new coreSecurityEntities();
    public class LiveTileData
    {
        public int imageID { get; set; }
        public string imageName { get; set; }
        public string details { get; set; }
        public string navigateUrl { get; set; }

        public int nextIndex { get; set; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        le = new coreLogic.coreLoansEntities();
        coreReports.reportEntities rent = new coreReports.reportEntities();

        if (!Page.IsPostBack)
        {
            var alerts = rent.vwAlerts.ToList();
            var alerts1 = alerts.Where(p => p.AlertType == "No Checklist" || p.AlertType == "Undisbursed Loan" || p.AlertType ==
                "Unapproved Loan").ToList();
            Session["vwAlerts1"] = alerts1;
            var alerts2 = alerts.Where(p => p.AlertType == "Activity Log").ToList();
            Session["vwAlerts2"] = alerts2;
            var alerts3 = alerts.Where(p => p.AlertType == "Due Payment").ToList();
            Session["vwAlerts3"] = alerts3;
            var alerts4 = alerts.Where(p => p.AlertType == "Matured Deposit").ToList();
            Session["vwAlerts4"] = alerts4;
        }
        else
        {

        }
    }
    [WebMethod]
    public static LiveTileData GetTileData(object context)
    {
        var index = 0;
        if (context != null)
        {
            IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
            index = Int32.Parse((string)contextDictionary["Value"]);
        }
        var thold = 0;
        if (index < 15)
        {
            thold = 0;
        }
        else if (index < 30)
        {
            thold = 15;
        }
        else if (index < 45)
        {
            thold = 30;
        }
        else if (index < 60)
        {
            thold = 45;
        }
        var alerts = new List<coreReports.vwAlert>();
        if (thold == 0)
        {
            alerts = HttpContext.Current.Session["vwAlerts1"] as List<coreReports.vwAlert>;
        }
        else if (thold == 15)
        {
            alerts = HttpContext.Current.Session["vwAlerts2"] as List<coreReports.vwAlert>;
        }
        else if (thold == 30)
        {
            alerts = HttpContext.Current.Session["vwAlerts3"] as List<coreReports.vwAlert>;
        }
        else if (thold == 45)
        {
            alerts = HttpContext.Current.Session["vwAlerts4"] as List<coreReports.vwAlert>;
        }
        if (alerts != null && alerts.Count>0)
        {
            index = index - thold;
            if (index >= alerts.Count)
            {
                index = 0;
                coreReports.reportEntities rent = new coreReports.reportEntities();
                if (thold == 0)
                {
                    alerts = rent.vwAlerts.Where(p => p.AlertType == "No Checklist" || p.AlertType == "Undisbursed Loan" || p.AlertType ==
                           "Unapproved Loan").ToList();
                    HttpContext.Current.Session["vwAlerts1"] = alerts;
                }
                else if (thold == 15)
                {
                    alerts = rent.vwAlerts.Where(p => p.AlertType == "Activity Log").ToList();
                    HttpContext.Current.Session["vwAlerts2"] = alerts;
                }
                else if (thold == 30)
                {
                    alerts = rent.vwAlerts.Where(p => p.AlertType == "Due Payment").ToList();
                    HttpContext.Current.Session["vwAlerts3"] = alerts;
                }
                else if (thold == 45)
                {
                    alerts = rent.vwAlerts.Where(p => p.AlertType == "Matured Deposit").ToList();
                    HttpContext.Current.Session["vwAlerts4"] = alerts;
                }
            }
            var result = alerts[index];
            coreLoansEntities le = new coreLoansEntities();
            var client = le.clients.FirstOrDefault(p => p.clientID == result.clientID);
            //client.clientImages.Load();
            var img = client.clientImages.FirstOrDefault();
            return new LiveTileData
            {
                imageID = (img == null) ? 0 : img.imageID.Value,
                imageName = client.surName + ", " + client.otherNames,
                details = result.alert,
                navigateUrl = result.url,
                nextIndex = index + 1
            };
        }

        return null;
    }
}
