using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using coreModels;
using coreModels.Login;


namespace coreErpApi.Controllers.Controllers.Login
{
    public class LoginController : ApiController
    {
        IcoreSecurityEntities ctx;
        public LoginController()
        {
           ctx = new coreSecurityEntities();
           ctx.Configuration.LazyLoadingEnabled = false;
           ctx.Configuration.ProxyCreationEnabled = false;
        }

        public LoginController(IcoreSecurityEntities lent)
        {
            ctx = lent;
        }

        [HttpPost]
        public LoginResponse Post(LoginRequest value)
        {
            var user = ctx.users.FirstOrDefault(p => p.user_name == value.userName);
            if (user != null)
            {
                if (CheckPassword(value.password, user.password))
                {
                    var token = System.Guid.NewGuid().ToString();
                    user.authTokens.Add(new authToken
                    {
                        token = token,
                        expiryDate = DateTime.Now.AddHours(9),
                        grantedDate = DateTime.Now,
                        clientHostName = HttpContext.Current.Request.UserHostName
                    });
                    ctx.SaveChanges();
                    return new LoginResponse
                    {
                        token = token,
                        name = user.full_name
                    };
                }
            }
            return null;
        }




        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            pass1 = EncodePassword(password); 

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
        HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(hashKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                
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
             
                    throw new ApplicationException("Unsupported password format."); 

            return password;
        }
        private const string hashKey = "C50B3C89CB21F4F1422FF158A5B42D0E8DB8CB5CDA1742572A487D9401E3400267682B202B746511891C1BAF47F8D25C07F6C39A104696DB51F17C529AD3CABE";


        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

    }
}
