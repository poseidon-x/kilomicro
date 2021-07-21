using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using agencyDAL.Context;

namespace agency
{
    public class Settings
    {
        public static string getToken()
        {
            string token = "INVALID_TOKEN";

            try
            {
                using (var ent = new coreDBEntities())
                {
                    var tkn = ent.authTokens.Where(p => p.userName.ToLower().Trim() == HttpContext.Current.User.Identity.Name.Trim()
                        && p.expiryDate > DateTime.Now
                        && HttpContext.Current.Request.UserHostName == p.clientHostName
                        )
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
    }
}