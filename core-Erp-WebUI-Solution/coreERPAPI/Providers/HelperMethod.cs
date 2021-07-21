using coreLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace coreERP.Providers
{
    public class HelperMethod
    {
        coreLoansEntities le;
        coreSecurityEntities secEnt;
        public HelperMethod()
        {
            le = new coreLoansEntities();
            secEnt = new coreSecurityEntities();
        }

        public string GetBranchNameForLoggedInUser(string userName)
        {
            try
            {
                var staff = le.staffs.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.Trim().ToLower());
                string branchName = le.branches.FirstOrDefault(e => e.branchID == staff.branchID).branchName;
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

        public int? GetBranchIdForUser(string userName)
        {
            try
            {
                var staffId = le.staffs.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.Trim().ToLower())?.branchID;
                return staffId;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int GetStaffIdForUser(string userName)
        {
            try
            {
                var staffId = le.staffs.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.Trim().ToLower()).staffID;
                return staffId;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public string GetStaffUserName(string userName)
        {
            try
            {
                var staffUserName = le.staffs.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.Trim().ToLower()).userName;
                return staffUserName;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public bool IsOwner(string userName)
        {
            try
            {
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => p.roles.role_name.Trim().ToLower() == "owner" && p.users.user_name.Trim().ToLower() == userName).ToList();
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

        public bool IsBranchAdminOwner(string userName)
        {
            try
            {
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => p.roles.role_name.Trim().ToLower() == "branchadmin" && p.users.user_name.Trim().ToLower() == userName).ToList();
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

        public bool IsOwnerOrAdmin(string userName)
        {
            try
            {
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => (p.roles.role_name.Trim().ToLower() == "owner" || p.roles.role_name.Trim().ToLower() == "admin") &&
                    p.users.user_name.Trim().ToLower() == userName).ToList();
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

        public string GetBranchNameForClient(int clientId)
        {
            try
            {
                var client = le.clients.SingleOrDefault(e => e.clientID == clientId);
                string branchName = le.branches.FirstOrDefault(e => e.branchID == client.branchID).branchName;
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

        public int GetBranchIdForClient(int clientId)
        {
            try
            {
                var client = le.clients.SingleOrDefault(e => e.clientID == clientId);
                int branchId = le.branches.FirstOrDefault(e => e.branchID == client.branchID).branchID;
                return branchId;

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public string GetGroupOfficerFullName(int loanGroupId)
        {
            var loanGroup = le.loanGroups.Where(p => p.loanGroupId == loanGroupId).FirstOrDefault();
            if (loanGroup == null)
                return string.Empty;
            var groupOfficer = le.staffs.FirstOrDefault(r => r.staffID == loanGroup.relationsOfficerStaffId);

            var officerName = groupOfficer?.surName + " " + groupOfficer?.otherNames;
            //Convert the string to CamelCase
            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            if (!string.IsNullOrWhiteSpace(officerName))
                officerName = txtInfo.ToTitleCase(officerName);
            return officerName;
        }

        public string GetGroupOfficerUserName(int loanGroupId)
        {
            var loanGroup = le.loanGroups.Where(p => p.loanGroupId == loanGroupId).FirstOrDefault();
            if (loanGroup == null)
                return string.Empty;
            var groupOfficer = le.staffs.FirstOrDefault(r => r.staffID == loanGroup.relationsOfficerStaffId);
            var officerName = groupOfficer.userName;
            //Convert the string to CamelCase
            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            if (!string.IsNullOrWhiteSpace(officerName))
                officerName = txtInfo.ToTitleCase(officerName);
            return officerName;
        }

        public string NameToCamelCase(string name)
        {
           
            //Convert the string to CamelCase
            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            if (!string.IsNullOrWhiteSpace(name))
                name = txtInfo.ToTitleCase(name);
            return name;
        }

        public string GetBranchNameForGroup(int groupId)
        {
            try
            {
                var group = le.loanGroups
                    .Include(p => p.loanGroupClients)
                    .Include(p => p.client)
                    .FirstOrDefault(p => p.loanGroupId == groupId);
                string branchName = le?.branches?.FirstOrDefault(e => e.branchID == group.client.branchID).branchName;
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

        public string GetGroupNameForClient(int clientId)
        {
            try
            {
                var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .Include(p => p.loanGroupClients.Select(v => v.client))
                .Where(p => p.loanGroupClients.Any(k => k.client.clientID == clientId)).FirstOrDefault();
                if (group != null && !string.IsNullOrWhiteSpace(group?.loanGroupName))
                {
                    return group.loanGroupName.ToUpper();
                }
                return "No Group";

            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public string GetGroupOfficerName(int loanGroupId)
        {
            var loanGroup = le.loanGroups.Where(p => p.loanGroupId == loanGroupId).FirstOrDefault();
            if (loanGroup == null)
                return string.Empty;
            var groupOfficer = le?.staffs?.FirstOrDefault(r => r.staffID == loanGroup.relationsOfficerStaffId);
            var officerName = (groupOfficer?.surName + " " + groupOfficer?.otherNames);
            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            if (!string.IsNullOrWhiteSpace(officerName))
                officerName = txtInfo.ToTitleCase(officerName);
            return officerName;
        }

        public int? GetGroupIdForClient(int clientId)
        {
            var loanGroup = le.loanGroups
                .Include(p => p.loanGroupClients)
                .Include(p => p.loanGroupClients.Select(v => v.client))
                .Where(p => p.loanGroupClients.Any(k => k.client.clientID == clientId)).FirstOrDefault();
            var group = loanGroup?.loanGroupId;
            if (group == null)
                return null;
            return group.Value;
        }

        public string GetClientPhoneNumber(int clientId)
        {
            //phone type ID 2 is for mobile and 3 is for home
            var clientPhones = le.clientPhones
                .Include(p=>p.phone)
                .Where(r => r.clientID == clientId).ToList();
            var mobilePhone = clientPhones.FirstOrDefault(r => r.phoneTypeID == 2)?.phone?.phoneNo;
            if (!string.IsNullOrWhiteSpace(mobilePhone))
                return mobilePhone;
            else
            {
                var homePhone = clientPhones.FirstOrDefault(r => r.phoneTypeID == 3)?.phone?.phoneNo;
                return homePhone ?? string.Empty;
            }
        }

    }
}