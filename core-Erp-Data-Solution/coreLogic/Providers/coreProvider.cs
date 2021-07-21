using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography; 
using System.Web.Configuration;
using System.Web.Security;
using coreLogic;
using System.Web;
using System.Data.Entity;

namespace coreSecurity
{
    namespace Provider{
    class coreMembershipProvider : MembershipProvider
    {       

        #region Class Variables

        private int newPasswordLength = 6;
        private string connectionString;
        private string applicationName;
        private bool enablePasswordReset = false;
        private bool enablePasswordRetrieval = false;
        private bool requiresQuestionAndAnswer = false;
        private bool requiresUniqueEmail = false;
        private int maxInvalidPasswordAttempts = 3;
        private int passwordAttemptWindow = 30;
        private MembershipPasswordFormat passwordFormat;
        private int minRequiredNonAlphanumericCharacters=0;
        private int minRequiredPasswordLength = 6;
        private string passwordStrengthRegularExpression;
        private MachineKeySection machineKey; //Used when determining encryption key values.

        #endregion

        #region Enums

        private enum FailureType
        {
            Password = 1,
            PasswordAnswer = 2
        }

        #endregion

        #region Properties

        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return enablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return enablePasswordRetrieval;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return requiresQuestionAndAnswer;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return requiresUniqueEmail;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return maxInvalidPasswordAttempts;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return passwordAttemptWindow;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return passwordFormat;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return minRequiredNonAlphanumericCharacters;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return minRequiredPasswordLength;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return passwordStrengthRegularExpression;
            }
        }

        #endregion

        #region Initialization

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name == null || name.Length == 0)
            {
                name = "coreMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "coreMembershipProvider for coreERP");
            }

            //Initialize the abstract base class.
            base.Initialize(name, config);

            applicationName = "coreERP";
            passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], String.Empty));
            
            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }

            switch (temp_format)
            {
                case "Hashed":
                    passwordFormat = MembershipPasswordFormat.Hashed;
                    break;  
                default:
                    throw new ProviderException("Password format not supported.");
            }
             
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
            {
                return defaultValue;
            }

            return configValue;
        }

        #endregion

        #region Implemented Abstract Methods from MembershipProvider

        /// <summary>
        /// Change the user password.
        /// </summary>
        /// <param name="username">UserName</param>
        /// <param name="oldPwd">Old password.</param>
        /// <param name="newPwd">New password.</param>
        /// <returns>T/F if password was changed.</returns>
        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {

            if (!ValidateUser(username, oldPwd))
            {
                return false;
            }

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }
                else
                {
                    throw new Exception("Change password canceled due to new password validation failure.");
                }
            }

            coreSecurityEntities ent = new coreSecurityEntities();
            try
            {
                var user = ent.users.FirstOrDefault(p => p.user_name == username);
                user.password = EncodePassword(newPwd);
                user.last_password_changed_date = DateTime.Now;
                ent.SaveChanges(); 
            }
            catch (SqlException e)
            {
                //Add exception handling here.
                return false;
            }
            finally
            { 
            }

            return true;

        }

        /// <summary>
        /// Change the question and answer for a password validation.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="newPwdQuestion">New question text.</param>
        /// <param name="newPwdAnswer">New answer text.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool ChangePasswordQuestionAndAnswer(
        string username,
          string password,
         string newPwdQuestion,
          string newPwdAnswer)
        {
            return false;
        }
        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="email">Email address.</param>
        /// <param name="passwordQuestion">Security quesiton for password.</param>
        /// <param name="passwordAnswer">Security quesiton answer for password.</param>
        /// <param name="isApproved"></param>
        /// <param name="userID">User ID</param>
        /// <param name="status"></param>
        /// <returns>MembershipUser</returns>

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if ((RequiresUniqueEmail && (GetUserNameByEmail(email) != String.Empty)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser membershipUser = GetUser(username, false);

            if (membershipUser == null)
            {
                System.DateTime createDate = DateTime.Now;
                 
                users user = new users();
                user.user_name = username;
                user.password = EncodePassword(password);
                user.is_active = true;
                user.is_locked_out = false;

                try
                {
                    coreSecurityEntities ent = new coreSecurityEntities();
                    ent.users.Add(user);
                    ent.SaveChanges();
                }
                catch (SqlException e)
                {
                    //Add exception handling here.

                    status = MembershipCreateStatus.ProviderError;
                }
                finally
                { 
                }

                status = MembershipCreateStatus.Success;
                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }
        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="deleteAllRelatedData">Whether to delete all related data.</param>
        /// <returns>T/F if the user was deleted.</returns>
        public override bool DeleteUser(
         string username,
         bool deleteAllRelatedData
        )
        {

             
            try
            {
                coreSecurityEntities ent = new coreSecurityEntities();
                var user = ent.users.FirstOrDefault(p => p.user_name == username);
                ent.users.Remove(user);
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            { 
            }

            return true;

        }
        /// <summary>
        /// Get a collection of users.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="totalRecords">Total # of records to retrieve.</param>
        /// <returns>Collection of MembershipUser objects.</returns>

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
             
            MembershipUserCollection users = new MembershipUserCollection();
             
            totalRecords = 0;

            try
            {
                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                coreSecurityEntities ent = new coreSecurityEntities();
                foreach (var user in ent.users)
                {
                    if (counter >= startIndex)
                    {
                        users.Add(GetUserFromEntity(user));
                    }

                    if (counter >= endIndex) { break; }
                    counter += 1;
                }
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            { 
            }

            return users;

        }
        /// <summary>
        /// Gets the number of users currently on-line.
        /// </summary>
        /// <returns>  /// # of users on-line.</returns>
        public override int GetNumberOfUsersOnline()
        {

            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);
             
            int numOnline = 0;

            try
            {
                coreSecurityEntities ent = new coreSecurityEntities();
                numOnline = (from user in ent.users
                             where user.is_onLine == true
                              && user.last_activity_date >= compareTime
                             select user.user_name).Count();
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            {
            }

            return numOnline;

        }
        /// <summary>
        /// Get the password for a user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="answer">Answer to security question.</param>
        /// <returns>Password for the user.</returns>
        public override string GetPassword(
         string username,
         string answer
        )
        {
            throw new ProviderException("Password Retrieval Not Enabled.");
        }

        public override MembershipUser GetUser(
        string username,
         bool userIsOnline
        )
        {
                MembershipUser membershipUser = null;
            try
            {
                coreSecurityEntities ent = new coreSecurityEntities();
                var user = ent.users.FirstOrDefault(p => p.user_name == username);
                if (user != null)
                { 
                    membershipUser = GetUserFromEntity(user);

                    if (userIsOnline)
                    {
                        user.last_activity_date = DateTime.Now;
                        ent.SaveChanges();
                    }
                }
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            { 
            }

            return membershipUser;
        }
        /// <summary>
        /// Get a user based upon provider key and if they are on-line.
        /// </summary>
        /// <param name="userID">Provider key.</param>
        /// <param name="userIsOnline">T/F whether the user is on-line.</param>
        /// <returns></returns>
        public override MembershipUser GetUser(
         object userID,
         bool userIsOnline
        )
        {
            MembershipUser membershipUser = null; 

            try
            {
                coreSecurityEntities ent = new coreSecurityEntities();

                var user = ent.users.FirstOrDefault(p => p.user_name == userID.ToString());
                membershipUser = GetUserFromEntity(user);
                if (userIsOnline == true)
                {
                    user.last_activity_date = DateTime.Now;
                    ent.SaveChanges();
                }
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            { 
            }

            return membershipUser;

        }

        /// <summary>
        /// Unlock a user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>T/F if unlocked.</returns>
        public override bool UnlockUser(
         string username
        )
        {
             
            try
            { 
                coreSecurityEntities ent = new coreSecurityEntities();

                var user = ent.users.FirstOrDefault(p => p.user_name == username);
                if (user == null)
                {
                    return false;
                }
                else
                {
                    user.is_locked_out = false;
                    ent.SaveChanges();
                }
            }
            catch (SqlException e)
            {
                //Add exception handling here.
                return false;
            }
            finally
            { 
            }

            return true;

        }


        public override string GetUserNameByEmail(string email)
        { 
            return String.Empty;

        }
        /// <summary>
        /// Reset the user password.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="answer">Answer to security question.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ResetPassword(
         string username,
         string answer
        )
        {
            throw new NotSupportedException("Password Reset is not enabled.");            
        }

        /// <summary>
        /// Update the user information.
        /// </summary>
        /// <param name="_membershipUser">MembershipUser object containing data.</param>
        public override void UpdateUser(MembershipUser membershipUser)
        { 
            try
            { 
                coreSecurityEntities ent = new coreSecurityEntities();

                var user = ent.users.FirstOrDefault(p => p.user_name == membershipUser.UserName);

                if (user == null)
                {
                    user.is_active = membershipUser.IsApproved;
                    ent.SaveChanges();
                }
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            { 
            }
        }

        /// <summary>
        /// Validate the user based upon username and password.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>T/F if the user is valid.</returns>
        public override bool ValidateUser(
         string username,
         string password
        )
        {
            DateTime attemptTime = DateTime.Now;
            bool isValid = false;

            string storedPassword = String.Empty;
            coreSecurityEntities ent = new coreSecurityEntities();

            try
            {
                var user = ent.users.FirstOrDefault(p => p.user_name == username);

                if (user == null)
                {
                   return false;
                }
                 
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
                                && HttpContext.Current.Request.UserHostName == p.clientHostName)
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
                            clientHostName = HttpContext.Current.Request.UserHostName
                        });
                        ent.SaveChanges();

                        HttpContext.Current.Session["userName"] = username.Trim();
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
        /// Find all users matching a search string.
        /// </summary>
        /// <param name="usernameToMatch">Search string of user name to match.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Total records found.</param>
        /// <returns>Collection of MembershipUser objects.</returns>

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        { 
            MembershipUserCollection membershipUsers = new MembershipUserCollection();
             int counter = 0;

            try
            { 
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;
                coreSecurityEntities ent = new coreSecurityEntities();
                var items = (from user in ent.users
                             where user.user_name.Contains(usernameToMatch) == true
                             select user);
                foreach(var user in items)
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser membershipUser = GetUserFromEntity(user);
                        membershipUsers.Add(membershipUser);
                    }

                    if (counter >= endIndex) { break; }

                    counter += 1;
                }
            }
            catch (SqlException e)
            {
                //Add exception handling here.
            }
            finally
            { 
            }

            totalRecords = counter;

            return membershipUsers;
        }

        /// <summary>
        /// Find all users matching a search string of their email.
        /// </summary>
        /// <param name="emailToMatch">Search string of email to match.</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Total records found.</param>
        /// <returns>Collection of MembershipUser objects.</returns>

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0; 
            return new MembershipUserCollection();
        }

        #endregion

        #region "Utility Functions"
        /// <summary>
        /// Create a MembershipUser object from a data reader.
        /// </summary>
        /// <param name="sqlDataReader">Data reader.</param>
        /// <returns>MembershipUser object.</returns>
        private MembershipUser GetUserFromEntity(
      users user
        )
        {

            object userID = user.user_name;
            string username = user.user_name;
            string email = "";

            string passwordQuestion = String.Empty;
             

            string comment = String.Empty;


            bool isActive = user.is_active;
            bool isLockedOut = user.is_locked_out.Value;
            DateTime creationDate = user.creation_date;

            DateTime lastLoginDate = new DateTime();
            if (user.last_login_date != null)
            {
                lastLoginDate = user.last_login_date.Value;
            }

            DateTime lastActivityDate = ((user.last_activity_date==null)?DateTime.MinValue:user.last_activity_date.Value);
            DateTime lastPasswordChangedDate = ((user.last_password_changed_date == null) ? DateTime.MinValue : user.last_password_changed_date.Value);

            DateTime lastLockedOutDate = new DateTime();
            if (user.last_locked_out_date != null)
            {
                lastLockedOutDate = user.last_locked_out_date.Value;
            }

            MembershipUser membershipUser = new MembershipUser(
              this.Name,
             username,
             userID,
             email,
             passwordQuestion,
             comment,
             isActive,
             isLockedOut,
             creationDate,
             lastLoginDate,
             lastActivityDate,
             lastPasswordChangedDate,
             lastLockedOutDate
              ); 
            return membershipUser;

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
        #endregion
        
    }

    public class coreRoleProvider : RoleProvider
    {

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            foreach (var username in usernames)
            {
                users user = ent.users.FirstOrDefault(u => u.user_name == username);
                if (user != null)
                {
                    foreach (var rolename in roleNames)
                    {
                        var role = ent.roles.FirstOrDefault(r => r.role_name == rolename);
                        if (role != null)
                        {
                            var ur = ent.user_roles.FirstOrDefault(p => p.users.user_name == user.user_name
                                && p.roles.role_name == role.role_name);
                            if (ur == null)
                            {
                                user_roles userRole = new user_roles();
                                userRole.users = user;
                                userRole.roles = role;
                                userRole.creation_date = DateTime.Now;
                                ent.user_roles.Add(userRole); 
                            } 
                        }
                    }
                }
            }
            ent.SaveChanges();
        }

        public override string ApplicationName
        {
            get
            {
                return "coreERP";
            }
            set
            {
                 
            }
        }

        public override void CreateRole(string roleName)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            roles role = new roles();
            role.role_name = roleName;
            role.description = roleName;
            role.creation_date = DateTime.Now;
            ent.roles.Add(role);
            ent.SaveChanges();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            roles role = ent.roles.FirstOrDefault(r => r.role_name == roleName);
            if (role != null)
            {
                ent.roles.Remove(role);
                ent.SaveChanges();
                return true;
            }

            return false;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            var role = ent.roles.FirstOrDefault(p => p.role_name == roleName);
            if (role != null)
            {
                var userRoles = (from user in ent.users
                                 from ur in ent.user_roles
                                 where user.user_name == ur.users.user_name
                                   && ur.roles.role_name == roleName
                                   && user.user_name.Contains(usernameToMatch)
                                   && user.is_active==true
                                 orderby user.user_name
                                 select user
                            ).ToList();
                if (userRoles == null || userRoles.Any() == false) return new string[0];
                var output = new string[userRoles.Count()];
                for (var i = 0; i < userRoles.Count(); i++)
                {
                    output[i] = userRoles[i].user_name;
                }
                return output;
            }
            return new string[0];
        }

        public override string[] GetAllRoles()
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            var query = ent.roles.ToList();
            var output = new string[query.Count()];
            for (var i = 0; i < query.Count(); i++)
            {
                output[i] = query.ElementAt(i).role_name;
            }
            return output;
        }

        public override string[] GetRolesForUser(string username)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            var user = ent.users.FirstOrDefault(p => p.user_name == username);
            if (user != null)
            {
                var userRoles = (from role in ent.roles
                             from ur in ent.user_roles
                             from u in ent.users
                             where role.role_name == ur.roles.role_name
                                && u.user_name == ur.users.user_name
                               && ur.users.user_name == username
                               && u.is_active==true 
                             orderby role.role_name
                             select role
                            ).ToList();
                if (userRoles == null || userRoles.Any() == false) return new string[0];
                var output = new string[userRoles.Count()];
                for (var i = 0; i < userRoles.Count(); i++)
                {
                    output[i] = userRoles[i].role_name;
                }
                return output;
            }
            return new string[0];
        }

        public override string[] GetUsersInRole(string roleName)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            var role = ent.roles.FirstOrDefault(p => p.role_name == roleName);
            if (role != null)
            {
                var userRoles = (from user in ent.users
                                 from ur in ent.user_roles
                                 where user.user_name == ur.users.user_name
                                   && ur.roles.role_name == roleName
                                   && user.is_active==true
                                 orderby user.user_name
                                 select user
                            ).ToList();
                if (userRoles == null || userRoles.Any() == false) return new string[0];
                var output = new string[userRoles.Count()];
                for (var i = 0; i < userRoles.Count(); i++)
                {
                    output[i] = userRoles[i].user_name;
                }
                return output;
            }
            return new string[0];
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            var role = ent.roles.FirstOrDefault(p => p.role_name == roleName);
            if (role != null)
            {
                var userRoles = (from user in ent.users
                                 from ur in ent.user_roles
                                 where user.user_name == ur.users.user_name
                                   && ur.roles.role_name == roleName
                                   && user.user_name==username
                                   && user.is_active==true
                                 orderby user.user_name
                                 select user
                            ).ToList();
                if (userRoles == null || userRoles.Any() == false) return false;
                else return true;
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            foreach (var username in usernames)
            {
                users user = ent.users.FirstOrDefault(u => u.user_name == username);
                if (user != null)
                {
                    foreach (var rolename in roleNames)
                    {
                        var role = ent.roles.FirstOrDefault(r => r.role_name == rolename);
                        if (role != null)
                        {
                            var ur = ent.user_roles.FirstOrDefault(p => p.users.user_name == user.user_name
                                && p.roles.role_name == role.role_name);
                            if (ur != null)
                            {
                                ent.user_roles.Remove(ur);
                            }
                        }
                    }
                }
            }
            ent.SaveChanges();
        }

        public override bool RoleExists(string roleName)
        {
            coreSecurityEntities ent = new coreSecurityEntities();
            return ent.roles.FirstOrDefault(p => p.role_name == roleName) != null;
        }
    }
}

}

public class Authorizer
{
    private static coreSecurityEntities ent = new coreSecurityEntities();
    public static bool IsUserAuthorized(string username, string perm_code, int module_id)
    { 
        bool isAuthorized = false;
        bool isDenied = IsUserDenied(username, perm_code, module_id);
        if (isDenied == true) return false;
        var urs = (from p in ent.user_roles
                   from r in ent.roles
                   from u in ent.users
                   where p.roles.role_name==r.role_name &&
                    u.user_name==p.users.user_name &&
                    p.users.user_name.ToLower().Trim() == username.ToLower().Trim()
                    && u.is_active==true
                    select new {
                        r.role_name
                    }
                  );
        foreach (var ur in urs)
        {
            isAuthorized = IsRoleAuthorized(ur.role_name, perm_code, module_id);
            if (isAuthorized == true)
            {
                break;
            }
        }

        return isAuthorized;
    }

    public static bool IsUserDenied(string username, string perm_code, int module_id)
    {
        bool isDenied = false;
        var urs = (from p in ent.user_roles
                   from r in ent.roles
                   from u in ent.users
                   where p.roles.role_name == r.role_name &&
                    u.user_name == p.users.user_name &&
                    p.users.user_name.ToLower().Trim() == username.ToLower().Trim()
                    && u.is_active == true
                   select new
                   {
                       r.role_name
                   }
                  );
        foreach (var ur in urs)
        {
            isDenied = IsRoleDenied(ur.role_name, perm_code, module_id);
            if (isDenied == true)
            {
                break;
            }
        }

        return isDenied;
    }

    public static bool IsRoleAuthorized(string roleName, string perm_code, int module_id)
    {
        var perms = (from p in ent.perms
                     from rp in ent.role_perms
                     from m in ent.modules
                     where p.perm_code == rp.perms.perm_code
                        && rp.modules.module_id == m.module_id
                        && rp.roles.role_name.ToLower().Trim() == roleName.ToLower().Trim()
                        && (p.perm_code == perm_code || p.perm_code == "A")
                        && m.module_id == module_id
                        && rp.allow == true
                     select new
                     {
                         rp.role_perm_id
                     }).Count();
        if (perms > 0) return true;
        var mod = (from m1 in ent.modules
                     from m2 in ent.modules
                     where m1.parent_module_id == m2.module_id
                        && m1.module_id == module_id
                     select new
                     {
                         m2.module_id
                     }).FirstOrDefault();
        if (mod != null)
        {
            return IsRoleAuthorized(roleName, perm_code, mod.module_id);
        }

        return false;
    }

    public static bool IsRoleDenied(string roleName, string perm_code, int module_id)
    {
        var perms = (from p in ent.perms
                     from rp in ent.role_perms
                     from m in ent.modules
                     where p.perm_code == rp.perms.perm_code
                        && rp.modules.module_id == m.module_id
                        && rp.roles.role_name.ToLower().Trim() == roleName.ToLower().Trim()
                        && (p.perm_code == perm_code || p.perm_code == "A")
                        && m.module_id == module_id
                        && rp.allow == false
                     select new
                     {
                         rp.role_perm_id
                     }).Count();
        if (perms > 0) return true;
        var mod = (from m1 in ent.modules
                   from m2 in ent.modules
                   where m1.parent_module_id == m2.module_id
                      && m1.module_id == module_id
                      && m2.parent_module_id!=null
                   select new
                   {
                       m2.module_id
                   }).FirstOrDefault();
        if (mod != null)
        {
            return IsRoleDenied(roleName, perm_code, mod.module_id);
        }

        return false;
    }

}