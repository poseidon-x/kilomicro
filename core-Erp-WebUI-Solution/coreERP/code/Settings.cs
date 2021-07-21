using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.code
{
    public class Settings
    {
        private static string kendoTheme = "";

        public static string getKendoTheme()
        {
            loadTheme();
            return kendoTheme + ".css";
        }

        public static string getKendoTheme2()
        {
            loadTheme();
            return kendoTheme + ".min.css";
        }

        private static void loadTheme()
        {
            try
            {
                using (var en = new coreLogic.core_dbEntities())
                {
                    var ip = en.interfacePreferences.FirstOrDefault(p => p.userName.ToLower().Trim() == HttpContext.Current.User.Identity.Name.Trim().ToLower());
                    if (ip != null)
                    {
                        if (ip.skinName.ToLower() == "metro")
                            kendoTheme = "material";
                        if (ip.skinName.ToLower() == "metrotouch")
                            kendoTheme = "metro";
                        else if (ip.skinName.ToLower() == "black")
                            kendoTheme = "materialblack";
                        if (ip.skinName.ToLower() == "blackmetrotouch")
                            kendoTheme = "metroblack";
                        else if (ip.skinName.ToLower() == "outlook")
                            kendoTheme = "highContrast";
                        else if (ip.skinName.ToLower() == "office2010blue"
                            || ip.skinName.ToLower() == "vista")
                            kendoTheme = "moonlight";
                        else if (ip.skinName.ToLower() == "windows7"
                            || ip.skinName.ToLower() == "webblue")
                            kendoTheme = "blueopal";
                        else if (ip.skinName.ToLower() == "web20")
                            kendoTheme = "bootstrap";
                        else if (ip.skinName.ToLower() == "office2010silver")
                            kendoTheme = "uniform";
                        else if (ip.skinName.ToLower() == "silk"
                            || ip.skinName.ToLower() == "simple")
                            kendoTheme = "silver";
                        else if (ip.skinName.ToLower() == "glow"
                            || ip.skinName.ToLower() == "office2010black")
                            kendoTheme = "black";
                        else if (ip.skinName.ToLower() == "sunset")
                            kendoTheme = "highcontrast";
                        else if (ip.skinName.ToLower() == "telerik")
                            kendoTheme = "bootstrap";
                    }
                    else
                    {
                        kendoTheme = "metro";
                    }
                }
            }
            catch (Exception x) { }
        }
        public static string getToken()
        {
            string token = "INVALID_TOKEN";

            try
            {
                using (var ent = new coreSecurityEntities())
                {
                    var tkn = ent.authTokens.Where(p => p.userName.Trim() == HttpContext.Current.User.Identity.Name.Trim()
                        && p.expiryDate > DateTime.Now
                        && HttpContext.Current.Request.UserHostName == p.clientHostName)
                        .OrderByDescending(p => p.expiryDate)
                        .FirstOrDefault();
                    if (tkn != null)
                    {
                        token = tkn.token;
                    }
                }
            }
            catch (Exception x) { }

            return token;
        }

        public static string companyName
        {
            get
            {
                try
                {
                    var profile = (new coreLogic.core_dbEntities()).comp_prof.FirstOrDefault();
                    if (profile != null) return profile.comp_name;
                }
                catch (Exception x) { return x.ToString(); }
                return "Demo Company Ltd";
            }
        }
        public static string userFullName
        {
            get
            {
                var profile = (new coreLogic.coreSecurityEntities()).users.FirstOrDefault(p=>p.user_name.ToLower().Trim()==HttpContext.Current.User.Identity.Name.Trim().ToLower());
                if (profile != null) return "Hello, " + profile.full_name;
                return "Welcome";
            }
        }
        public static string compAddr
        {
            get
            {
                var profile = (new coreLogic.core_dbEntities()).comp_prof.FirstOrDefault();
                if (profile != null && profile.addr_line_1 != null) return profile.addr_line_1;
                return "";
            }
        }
        public static string compPhone
        {
            get
            {
                var profile = (new coreLogic.core_dbEntities()).comp_prof.FirstOrDefault();
                if (profile != null && profile.phon_num != null) return profile.phon_num;
                return "";
            }
        }
        public static string compCity
        {
            get
            {
                using (var ent = new coreLogic.core_dbEntities())
                {
                    var profile = (ent).comp_prof.FirstOrDefault();
                    if (profile != null)
                    {
                        var city = ent.cities.FirstOrDefault(p => p.city_id == profile.city_id);
                        if (city != null)
                        {
                            return city.city_name;
                        }
                    }
                    return "";
                }
            }
        }

    }

    public class Res
    {
        public Res(string batch_no,
         DateTime tx_date,
         string recipient,
         int? sup_id,
         int? cust_id,
         int? emp_id,
         string v_type)
        {
            this.batch_no = batch_no;
            this.cust_id = cust_id;
            this.emp_id = emp_id;
            this.recipient = recipient;
            this.sup_id = sup_id;
            this.tx_date = tx_date;
            this.v_type = v_type;
        }
        public Res() { }

        public string batch_no { get; set; }
        public DateTime tx_date { get; set; }
        public string recipient { get; set; }
        public int? sup_id { get; set; }
        public int? cust_id { get; set; }
        public int? emp_id { get; set; }
        public string v_type { get; set; }
    }

}
