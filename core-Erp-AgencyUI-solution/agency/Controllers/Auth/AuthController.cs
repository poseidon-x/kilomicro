using System;
using System.Linq;
using System.Web.Mvc;
using agency.Models;
using agencyDAL.Context;
using System.Security.Cryptography;
using System.Web.Security;
using System.Text;
using System.Web;

namespace agency.Controllers.Auth
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Auth");
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated && agency.Settings.getToken() != "INVALID_TOKEN")
                return RedirectToAction("Dashboard", "Agent");

            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(SignInModel model, string returnUrl)
        {
            if (model.username == null || model.username == "")
            {
                ViewBag.Error = "Credentials EMPTY";
                return View();
            }

            coreDBEntities db = new coreDBEntities();
            
            var user = db.users.FirstOrDefault(p => p.user_name.ToLower() == model.username.ToLower());

            if (user == null)
            {
                ViewBag.Error = "Account NOT FOUND!";
                return View();
            }
            if (user.login_failure_count > 3 || user.is_locked_out == true)
            {
                ViewBag.Error = "Your account has been locked. Please call support.";
                return View();
            }
            var encodedPassword = EncodePassword(model.password);
            if (user.password != encodedPassword)
            {
                ViewBag.Error = "Password INCORRECT!";
                user.login_failure_count += 1;
                if (user.login_failure_count > 3)
                {
                    user.is_locked_out = true;
                }

                db.SaveChanges();
                return View();
            }
            user.last_login_date = DateTime.Now;
            user.login_failure_count = 0;

            var token = System.Guid.NewGuid().ToString();

            user.authTokens.Add(new authToken
            {
                token = token,
                expiryDate = DateTime.Now.AddHours(2),
                grantedDate = DateTime.Now,
                clientHostName = System.Web.HttpContext.Current.Request.UserHostName.ToString()
            });
            db.SaveChanges();

            //Successfully saved now log me in
            FormsAuthentication.SetAuthCookie(model.username, true);

            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Dashboard", "Agent");
            }
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

        public ActionResult Logout()
        {
            try
            {
                agencyDAL.Context.coreDBEntities db = new agencyDAL.Context.coreDBEntities();
                var user = db.users.First(p => p.user_name == User.Identity.Name);
                var tokens = user.authTokens
                    .Where(p => p.expiryDate > DateTime.Now);
                foreach (var token in tokens)
                {
                    token.expiryDate = DateTime.Now.AddMinutes(-1);
                }
                db.SaveChanges();
            }
            catch (Exception) { }
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}