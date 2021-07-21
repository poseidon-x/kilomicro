using coreERP.Models;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace coreERP.Controllers.Security
{
    public class SecurityController : Controller
    {
        
        private int maxInvalidPasswordAttempts = 3;
        private MembershipPasswordFormat PasswordFormat;


        public ActionResult Login()
        {
            var sessions = Request.RequestContext.HttpContext.Session;
            if (User.Identity.IsAuthenticated && Request.IsAuthenticated )
            {
                var username=User?.Identity?.Name?.ToLower();
                FormsAuthentication.GetRedirectUrl(username,false);
                return Redirect("/default.aspx");
            }

            var queryString = Request.QueryString["ReturnUrl"];
            var home = "/";
            if (!string.IsNullOrWhiteSpace(queryString) && queryString != home)
            {
                return Redirect("/dash/home.aspx");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            var isUserValid = ValidateUser(username, password);
            if (isUserValid)
            {
                FormsAuthentication.SetAuthCookie(username.ToLower(), false);
                return Redirect("/default.aspx");
            }
            else
                return Redirect("/Security/Login");
        }


        /// <summary>
        /// Validate the user based upon username and password.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>T/F if the user is valid.</returns>
        public bool ValidateUser(string username,string password)
        {
            DateTime attemptTime = DateTime.Now;
            bool isValid = false;
            PasswordFormat = MembershipPasswordFormat.Hashed;
            string storedPassword = String.Empty;
            coreSecurityEntities ent = new coreSecurityEntities();

            try
            {
                var user = ent.users.FirstOrDefault(p => p.user_name == username);

                if (user == null)
                {
                    return false;
                }
                //Get the current http request
                var httpContext = HttpContext.Request;

                if (CheckPassword(password, user.password))
                {
                    if (user.is_locked_out == true)
                    {
                        throw new ApplicationException("Account is locked out");
                    }
                    else if (user.is_active == true)
                    {
                        isValid = true;
                        user.last_login_date = DateTime.Now;
                        user.login_failure_count = 0;
                        user.is_onLine = true;
                        user.is_locked_out = false;
                        var unexpiredTokens = ent.authTokens
                            .Where(p => p.userName == user.user_name
                                && p.expiryDate > DateTime.Now
                                && httpContext.UserHostName == p.clientHostName)
                            .ToList();
                        foreach (var token in unexpiredTokens)
                        {
                            token.expiryDate = DateTime.Now.AddSeconds(-1);
                        }
                        ent.authTokens.Add(new authToken
                        {
                            expiryDate = DateTime.Now.AddHours(9),
                            grantedDate = DateTime.Now,
                            token = Guid.NewGuid().ToString(),
                            userName = user.user_name,
                            clientHostName = httpContext.UserHostName
                        });
                        ent.SaveChanges();
                        Session["userName"] = username.Trim();
                    }
                }
                else
                {
                    user.login_failure_count += 1;
                    if (user.login_failure_count > maxInvalidPasswordAttempts) user.is_locked_out = true;
                    ent.SaveChanges();
                }
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            {
                LogAttempt(username, password, isValid, attemptTime);
            }

            return isValid;
        }
        private void LogAttempt(string username, string password, bool isValid, DateTime attemptTime)
        {
            try
            {
                coreSecurityEntities ent = new coreSecurityEntities();
                string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                user_login_attempts at = new user_login_attempts();
                at.user_name = username;
                at.ip_address = ip;
                at.was_successfull = isValid;
                at.login_attempt_date = attemptTime;
                at.creation_date = attemptTime;
                at.password = isValid ? "XXXXXXXXXXXXXXXX" : password;
                ent.user_login_attempts.Add(at);
                ent.SaveChanges();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Encode password.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <returns>Encoded password.</returns>
        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(hashKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        /// <summary>
        /// UnEncode password.
        /// </summary>
        /// <param name="encodedPassword">Password.</param>
        /// <returns>Unencoded password.</returns>
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }
        private const string hashKey = "C50B3C89CB21F4F1422FF158A5B42D0E8DB8CB5CDA1742572A487D9401E3400267682B202B746511891C1BAF47F8D25C07F6C39A104696DB51F17C529AD3CABE";
    }
}