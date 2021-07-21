using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace coreERP.Providers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizationFilter : AuthorizationFilterAttribute,  IAuthenticationFilter 
    {
        public AuthorizationFilter(string roleName = null)
        {
            //TODO: Validate Access to the API
        }

        public System.Threading.Tasks.Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken cancellationToken)
        {
            var req = context.Request;
            // Get credential from the Authorization header 
            //(if present) and authenticate
            var creds = "";
            if (req.RequestUri.AbsolutePath.ToLower().Contains("menu"))
            {
                var claims = new List<Claim>()
                              {
                                new Claim(ClaimTypes.Name, "User") 
                              };
                var principal = new ClaimsPrincipal(new[] { 
                            new ClaimsIdentity(claims)
                        });
                // The request message contains valid credential
                context.Principal = principal;
            }
            else
            {
                if (req.Headers.Authorization != null && req.Headers.Authorization.Scheme!=null &&  
                  "coreBearer".Equals(req.Headers.Authorization.Scheme,
                    StringComparison.OrdinalIgnoreCase))
                {
                    creds = req.Headers.Authorization.Parameter;
                }
                else
                {
                    try
                    {
                        var qs = req.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "token");
                        creds = qs.Value;
                    }
                    catch (Exception) { }
                }
                var auth = false;
                if (creds != "" && creds != null)
                {
                    using (var ent = new coreLogic.coreSecurityEntities())
                    {
                        var token = ent.authTokens.Where(p => p.token == creds ).FirstOrDefault();
                        coreLogic.users usr = null;
                        if (token != null)
                        {
                            usr = token.user;
                            auth = true;
                            var claims = new List<Claim>()
                            { 
                                new Claim(ClaimTypes.Name, token.userName),
                                new Claim(ClaimTypes.UserData, token.token),
                                new Claim(ClaimTypes.Version, "Employee")
                            };
                            foreach (var r in usr.user_roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, r.roles.role_name));
                            }
                            var principal = new ClaimsPrincipal(new[] { 
                            new ClaimsIdentity(claims)
                        });
                            // The request message contains valid credential
                            context.Principal = principal;
                        }
                    }
                }
                if (false == auth)
                {
                    // The request message contains invalid credential
                    context.ErrorResult = new UnauthorizedResult(
                      new AuthenticationHeaderValue[0], context.Request);
                }
            }
            return Task.FromResult(0);
        }

        public System.Threading.Tasks.Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }
        public class ResultWithChallenge : IHttpActionResult
        {
            private readonly IHttpActionResult next;
            public ResultWithChallenge(IHttpActionResult next)
            {
                this.next = next;
            }
            public async Task<HttpResponseMessage> ExecuteAsync(
              CancellationToken cancellationToken)
            {
                if (next != null)
                {
                    await next.ExecuteAsync(cancellationToken);
                    
                    var response = await next.ExecuteAsync(cancellationToken);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        response.Headers.WwwAuthenticate.Add(
                            new AuthenticationHeaderValue("coreBearer", "You must log on first"));
                    }

                    return response;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool AllowMultiple
        {
            get { return true; }
        }
    }
}