using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace coreERP.Models
{
    public class LoggedUser
    {
        public static string UserName  => getCurrentUser();

        
        public static string getCurrentUser()
        {
            var db = new coreSecurityEntities();
            var claims = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims.ToList();
            var userName = claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value;            
            return userName.Trim().ToLower();
        }
    }
}