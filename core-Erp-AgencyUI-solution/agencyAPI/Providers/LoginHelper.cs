using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using coreLogic;

namespace agencyAPI.Providers
{
    public static class LoginHelper
    {

        public static string getCurrentUser(IcoreSecurityEntities db)
        {
            var claims=((ClaimsIdentity)HttpContext.Current.User.Identity).Claims.ToList();
            var authToken = claims.First(p => p.Type == ClaimTypes.UserData).Value;
            var token = db.authTokens
                .First(p => p.token == authToken
                    && p.expiryDate > DateTime.Now
                );
            return token.userName;
        }


        public static string getClaim(string claimType)
        {
            return ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims.First(p => p.Type == claimType).Value;           
        }

    }
}