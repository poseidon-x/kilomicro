using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using agencyDAL.Context;
using agencyAPI.Models;
using System.Security.Cryptography;
using System.Web;
using System.Text;
using System.Web.Http.Cors;


namespace agencyAPI.Controllers.Auth
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class AuthController : ApiController
    {
        private readonly coreDBEntities db;

        public AuthController()
        {// constructor
            db = new coreDBEntities();
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
        }

        [HttpPost]
        public LoginResponse Login(UserLogin input)
        {
            var user = db.users.FirstOrDefault(p => p.user_name == input.username);

            if (user != null)
            {
                if (checkPassword(input.password, user.password))
                {
                    var token = System.Guid.NewGuid().ToString();
                    user.authTokens.Add(new authToken
                    {
                        token = token,
                        expiryDate = DateTime.Now.AddHours(2),
                        grantedDate = DateTime.Now,
                        clientHostName = HttpContext.Current.Request.UserHostName
                    });

                    db.SaveChanges();

                    return new LoginResponse { 
                        token = token,
                        name = user.full_name
                    };
                }
            } 
            return null;
        }

        private bool checkPassword(string password, string dbpassword)
        {
            string pswd1 = password;
            pswd1 = EncodePassword(password);

            if (pswd1 == dbpassword) { return true; }
            return false;
        }

        private string EncodePassword(string password)
        {
            string encodedPassword = password;
            HMACSHA1 hash = new HMACSHA1();
            hash.Key = HexToByte(hashKey);
            encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));

            return encodedPassword;
        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        private const string hashKey = "C50B3C89CB21F4F1422FF158A5B42D0E8DB8CB5CDA1742572A487D9401E3400267682B202B746511891C1BAF47F8D25C07F6C39A104696DB51F17C529AD3CABE";

        [HttpGet]
        public LoginResponse Logout(string username)
        {
            var user = db.authTokens.FirstOrDefault(p => p.userName == username);
            return null;
        }
    }
}
