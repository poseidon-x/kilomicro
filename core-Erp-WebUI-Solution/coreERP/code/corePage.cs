using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using coreLogic;
using coreLicenseLib;
using coreERP;
using coreERP.code;

public abstract class corePage : Page
{
    public corePage()
    {
        try
        {
            coreSecurityEntities ent = new coreSecurityEntities(); 
            var mod = ent.modules.FirstOrDefault(p => p.url == URL); 
            if (mod != null)
            {
                module = mod;
                if (!CanView)
                {
                    Server.Transfer("~/security/unauthorized.aspx");
                } 
            }
        }
        catch (Exception) { }

        if ((URL.Contains(".aspx") || URL.Contains(".ASPX")
         || URL.Contains(".Aspx")
         ) && (!URL.Contains("invalid_lic.aspx")) && (!URL.Contains("login.aspx")))
        {
            BitField bf = new BitField();
            if (URL.Contains("/gl/"))
            {
                bf.SetOn(BitField.Flag.f1);
            }
            if (URL.Contains("/ar/") && !URL.EndsWith("/cust.aspx"))
            {
                bf.SetOn(BitField.Flag.f2);
            }
            if (URL.Contains("/ap/") && !URL.EndsWith("/sup.aspx"))
            {
                bf.SetOn(BitField.Flag.f3);
            }
            if (URL.Contains("/tr/"))
            {
                bf.SetOn(BitField.Flag.f4);
            }
            if (URL.Contains("/ln/"))
            {
                bf.SetOn(BitField.Flag.f11);
            }
            coreLicense lic = new coreLicense();
            var d1 = "";
            var d2 = "";
            var licSt = lic.IsAuthorized(Settings.companyName, bf.Mask, ref d1, ref d2);
            if (licSt == LicenseState.ValidLicense)
            {
            }
            else if (URL.EndsWith(".css") || URL.EndsWith(".js")
                 || URL.EndsWith(".axd")
            || URL.EndsWith(".jpg") || URL.Contains("invalid_lic.aspx")
            )
            {
            }
            else if (URL.Contains("/security/") || URL.Contains("prof.aspx"))
            {
                if (licSt != LicenseState.NotForCompany && licSt != LicenseState.NoLicenseFile)
                {
                    Server.Transfer("~/security/invalid_lic.aspx?licSt=" + licSt.ToString());
                }
            }
            else
            {
                Server.Transfer("~/security/invalid_lic.aspx?licSt=" + licSt.ToString());
            }
        }
    }

    public abstract string URL
    {
        get;
    }
    private modules module = null; 

    public bool CanView
    {
        get
        {
            return Authorizer.IsUserAuthorized(Context.User.Identity.Name,
                "V", module.module_id);
        }
    }

}
