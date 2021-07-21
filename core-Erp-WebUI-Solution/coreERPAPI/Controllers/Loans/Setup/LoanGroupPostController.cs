using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Text;
using System.Web.Http.Cors;
using coreData.Constants;
using System.Data.Entity.Validation;
using System.Globalization;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class LoanGroupPostController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        public LoanGroupPostController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanGroupPostController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        [HttpPost]
        public  loanGroup Post(loanGroup value)
        {
            if (value == null) return null;
            //Validate Input
            Validate(value);

            if (value.loanGroupId > 0)
            {
                var toBeUpdated =  le.loanGroups
                    .Include(p => p.loanGroupClients)
                    .FirstOrDefault(p => p.loanGroupId == value.loanGroupId);

                cleanClients(toBeUpdated.loanGroupClients.ToList(), value.loanGroupClients.ToList());
                populateFields(toBeUpdated, value);
            }
            else
            {
                loanGroup toBeSaved = new loanGroup();
                populateFields(toBeSaved, value);
                le.loanGroups.Add(toBeSaved);
            }

            try
            {
                le.SaveChanges();
            }
            catch (DbEntityValidationException dex)
            {
                var dbEntityValidationResult = dex.EntityValidationErrors.FirstOrDefault();
                if (dbEntityValidationResult != null)
                {
                    var er = dbEntityValidationResult.ValidationErrors.FirstOrDefault();
                    if (er != null) throw new ApplicationException(string.Format("{0} | {1}", er.ErrorMessage,er.PropertyName));
                }
            }
            catch (Exception x)
            {
                throw x;
            }
            return value;
        }

        private void populateFields(loanGroup target, loanGroup source)
        {
            target.loanGroupName = source.loanGroupName;
            if (source.loanGroupId < 1)
            {
                target.loanGroupNumber = IDGenerator.newGroupLoanNumber(le);
                target.loanGroupDayId = source.loanGroupDayId;
                target.relationsOfficerStaffId = source.relationsOfficerStaffId;
                target.leaderClientId = source.leaderClientId;
                target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                target.created = DateTime.Now;
                target.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                target.modified = DateTime.Now;
                //get the user branch and add the prefix
                var logUser = User?.Identity?.Name?.ToLower();
                if (!IsOwner(logUser))
                {
                    var userBranch = GetBranchNameForLoggedInUser(logUser);
                    var brnchPrefx = userBranch.Substring(0, 2).ToUpper();
                    target.loanGroupName = brnchPrefx + "-" + source.loanGroupName;
                    target.loanGroupNumber = brnchPrefx + "-" + target.loanGroupNumber;
                }
            }
            else
            {
                target.loanGroupDayId = source.loanGroupDayId;
                target.relationsOfficerStaffId = source.relationsOfficerStaffId;
                target.leaderClientId = source.leaderClientId;
                target.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                target.modified = DateTime.Now;
            }

            if(source.loanGroupClients.Count < 2) 
                throw new ApplicationException(error.LoanGroupClientsBelowMin);

            foreach (var client in source.loanGroupClients)
            {
                if (client.loanGroupClientId > 0)
                {
                    var tobeUpdated = target.loanGroupClients.FirstOrDefault(p => p.loanGroupClientId == client.loanGroupClientId);
                    populateGroupClientFields(tobeUpdated, client);
                }
                else
                {
                    loanGroupClient tobeSaved = new loanGroupClient();
                    populateGroupClientFields(tobeSaved, client);
                    target.loanGroupClients.Add(tobeSaved);
                
                }    
            }
        }

        private void populateGroupClientFields(loanGroupClient target, loanGroupClient source)
        {
            target.clientId = source.clientId;
            target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            target.created = DateTime.Now;
        }

        private void cleanClients(List<loanGroupClient> saved, IEnumerable<loanGroupClient> toSave)
        {
            for (var i = saved.Count - 1; i >= 0; i--)
            {
                var client = saved[i];
                if (!toSave.Any(p => p.loanGroupClientId == client.loanGroupClientId))
                {
                    le.loanGroupClients.Remove(client);
                }
            }
        }

        private void Validate(loanGroup input)
        {
            if (!clientExist(input.leaderClientId, input.loanGroupClients) || (String.IsNullOrEmpty(input.loanGroupName)
                || String.IsNullOrWhiteSpace(input.loanGroupName)) || !groupDayExist(input.loanGroupDayId)
                || !relationsOfficerExist(input.relationsOfficerStaffId))
            {
                StringBuilder error = new StringBuilder();
                if (!clientExist(input.leaderClientId, input.loanGroupClients))
                    error.Append(ErrorMessages.InvalidGroupLeader);
                if (String.IsNullOrEmpty(input.loanGroupName) || String.IsNullOrWhiteSpace(input.loanGroupName))
                    error.Append(ErrorMessages.EmptyGroupName);
                if (!groupDayExist(input.loanGroupDayId))
                    error.Append(ErrorMessages.InvalidGroupDay);
                if (!relationsOfficerExist(input.relationsOfficerStaffId))
                    error.Append(ErrorMessages.InvalidRelationsOfficer);
                error.Append(ErrorMessages.CorrectData);
                throw new ApplicationException(error.ToString());
            }
        }


        private bool clientExist(int clientId, IEnumerable<loanGroupClient> clients)
        {
            if (le.clients.Any(p => p.clientID == clientId) && clients.Any(p => p.clientId == clientId))
            {
                return true;
            }
            return false;
        }

        private bool groupDayExist(int groupDayId)
        {
            if (le.loanGroupDays.Any(p => p.loanGroupDayId == groupDayId))
            {
                return true;
            }
            return false;
        }

        private bool relationsOfficerExist(int staffId)
        {
            if (le.staffs.Any(p => p.staffID == staffId))
            {
                return true;
            }
            return false;
        }


        #region OTHER HELPERS


        private string GetBranchNameForLoggedInUser(string userName)
        {
            try
            {
                var staff = le.staffs.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.Trim().ToLower());
                string branchName = le?.branches?.FirstOrDefault(e => e.branchID == staff.branchID).branchName;
                TextInfo thisTxtInfo = new CultureInfo("en-us", false).TextInfo;
                if (!string.IsNullOrWhiteSpace(branchName))
                    branchName = thisTxtInfo.ToTitleCase(branchName);
                return branchName;

            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private bool IsOwner(string userName)
        {
            try
            {
                var secEnt = new coreSecurityEntities();
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => p.roles.role_name.Trim().ToLower() == "owner" && p.users.user_name.Trim().ToLower() == userName.ToLower()).ToList();
                if (userRoles != null && userRoles.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        #endregion OTHER HELPERS

    }
}
