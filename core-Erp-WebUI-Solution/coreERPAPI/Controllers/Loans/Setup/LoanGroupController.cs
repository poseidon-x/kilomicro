using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using System.Web.Http.Cors;
using coreData.Constants;
using coreData.ErrorLog;
using coreErpApi.Controllers.Models;
using coreErp.Models.Loan;
using System.Globalization;
using coreERP.Models.Loan;
using System.Web;
using coreERP;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    //[AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class LoanGroupController : ApiController
    {
        IcoreLoansEntities le;
        //IcoreSecurityEntities secEnt;
        ErrorMessages error = new ErrorMessages();

        public LoanGroupController()
        {
            le = new coreLoansEntities();
            //secEnt = new coreSecurityEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanGroupController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/
        [AuthorizationFilter()]
        [HttpGet]
        public IEnumerable<LoanGroupExtend> Get()
        {
            List<LoanGroupExtend> groups = new List<LoanGroupExtend>();
            try
            {
                var user = User?.Identity?.Name?.ToLower();
                if (!IsOwner(user))
                {
                    var userBranchId = GetUserStaffBranchId(user);
                    var userStaff = GetUserStaff(user);
                    var selGroups = le.loanGroups
                        .Include(p => p.loanGroupClients)
                        .Include(p => p.loanGroupClients.Select(c => c.client))
                         .Include(p => p.staff)
                        .Where(p => (p.client.branchID == userBranchId || p.staff.branchID == userBranchId) && p.staff.staffID == userStaff.staffID)
                       .ToList();
                    foreach (var p in selGroups)
                    {
                        var newGroup = new LoanGroupExtend
                        {
                            staff = p.staff == null ? null : new staff
                            {
                                staffID = p.staff.staffID,
                                staffNo = p.staff.staffNo,
                                surName = p.staff.surName,
                                otherNames = p.staff.otherNames,
                                userName = p.staff.userName,
                                branchID = p.staff.branchID
                            },
                            created = p.created,
                            leaderClientId = p.leaderClientId,
                            loanGroupClients = new List<loanGroupClient>(),
                            loanGroupDay = p.loanGroupDay,
                            creator = p.creator,
                            loanGroupDayId = p.loanGroupDayId,
                            loanGroupId = p.loanGroupId,
                            loanGroupName = p.loanGroupName,
                            loanGroupNumber = p.loanGroupNumber,
                            modified = p.modified,
                            modifier = p.modifier,
                            relationsOfficerStaffId = p.relationsOfficerStaffId
                        };
                        foreach (var lg in p.loanGroupClients)
                        {
                            var newlg = new loanGroupClient
                            {
                                loanGroupClientId = lg.loanGroupClientId,
                                client = new client
                                {
                                    clientID = lg.clientId,
                                    surName = lg.client.surName,
                                    otherNames = lg.client.otherNames,
                                    accountName = lg.client.accountName,
                                    accountNumber = lg.client.accountNumber,
                                    branchID = lg.client.branchID
                                },
                                clientId = lg.clientId
                            };
                            newGroup.loanGroupClients.Add(newlg);
                        }
                        groups.Add(newGroup);

                    }

                }
                else
                {
                    var selectedGroups = le.loanGroups
                         .Include(p => p.loanGroupClients)
                         .Include(p => p.loanGroupClients.Select(c => c.client))
                         .Include(p => p.staff)
                         .ToList();
                    foreach (var p in selectedGroups)
                    {
                        var newGroup = new LoanGroupExtend
                        {
                            staff = p.staff == null ? null : new staff
                            {
                                staffID = p.staff.staffID,
                                staffNo = p.staff.staffNo,
                                surName = p.staff.surName,
                                otherNames = p.staff.otherNames,
                                userName = p.staff.userName,
                                branchID = p.staff.branchID
                            },
                            created = p.created,
                            leaderClientId = p.leaderClientId,
                            loanGroupClients = new List<loanGroupClient>(),
                            loanGroupDay = p.loanGroupDay,
                            creator = p.creator,
                            loanGroupDayId = p.loanGroupDayId,
                            loanGroupId = p.loanGroupId,
                            loanGroupName = p.loanGroupName,
                            loanGroupNumber = p.loanGroupNumber,
                            modified = p.modified,
                            modifier = p.modifier,
                            relationsOfficerStaffId = p.relationsOfficerStaffId
                        };
                        foreach (var lg in p.loanGroupClients)
                        {
                            var newlg = new loanGroupClient
                            {
                                loanGroupClientId = lg.loanGroupClientId,
                                client = new client
                                {
                                    clientID = lg.clientId,
                                    surName = lg.client.surName,
                                    otherNames = lg.client.otherNames,
                                    accountName = lg.client.accountName,
                                    accountNumber = lg.client.accountNumber,
                                    branchID = lg.client.branchID
                                },
                                clientId = lg.clientId
                            };
                            newGroup.loanGroupClients.Add(newlg);
                        }
                        groups.Add(newGroup);

                    }

                }
                foreach (var group in groups)
                {
                    group.BranchName = GetBranchNameForGroup(group.loanGroupId);
                }
            }
            catch (Exception e)
            {

            }
            return groups;
        }

        // GET: api/
        [HttpGet]
        public loanGroup Get(int id)
        {
            loanGroup value = le.loanGroups
                    .Include(p => p.loanGroupClients)
                    .FirstOrDefault(p => p.loanGroupId == id);

            if (value == null)
            {
                value = new loanGroup();
            }
            return value;
        }

        [AuthorizationFilter()]
        [HttpPost]
        public List<LoanGroupExtend> GetLoanGroups([FromBody]KendoRequest req)
        {
            List<LoanGroupExtend> groups = new List<LoanGroupExtend>();
            try
            {
                var user = User?.Identity?.Name?.ToLower();
                if (!IsOwner(user))
                {
                    var userBranchId = GetUserStaffBranchId(user);
                    var userStaff = GetUserStaff(user);
                    var selGroups = le.loanGroups
                        .Include(p => p.loanGroupClients)
                        .Include(p => p.loanGroupClients.Select(c => c.client))
                         .Include(p => p.staff)
                        .Where(p => (p.client.branchID == userBranchId || p.staff.branchID == userBranchId) && p.staff.staffID == userStaff.staffID)
                       .ToList();
                    foreach (var p in selGroups)
                    {
                        var newGroup = new LoanGroupExtend
                        {
                            staff = p.staff == null ? null : new staff
                            {
                                staffID = p.staff.staffID,
                                staffNo = p.staff.staffNo,
                                surName = p.staff.surName,
                                otherNames = p.staff.otherNames,
                                userName = p.staff.userName,
                                branchID=p.staff.branchID
                            },
                            created = p.created,
                            leaderClientId = p.leaderClientId,
                            loanGroupClients = new List<loanGroupClient>(),
                            loanGroupDay = p.loanGroupDay,
                            creator = p.creator,
                            loanGroupDayId = p.loanGroupDayId,
                            loanGroupId = p.loanGroupId,
                            loanGroupName = p.loanGroupName,
                            loanGroupNumber = p.loanGroupNumber,
                            modified = p.modified,
                            modifier = p.modifier,
                            relationsOfficerStaffId = p.relationsOfficerStaffId
                        };
                        foreach (var lg in p.loanGroupClients)
                        {
                            var newlg = new loanGroupClient
                            {
                                loanGroupClientId = lg.loanGroupClientId,
                                client = new client
                                {
                                    clientID = lg.clientId,
                                    surName = lg.client.surName,
                                    otherNames = lg.client.otherNames,
                                    accountName = lg.client.accountName,
                                    accountNumber = lg.client.accountNumber,
                                    branchID = lg.client.branchID
                                },
                                clientId = lg.clientId
                            };
                            newGroup.loanGroupClients.Add(newlg);
                        }
                        groups.Add(newGroup);

                    }

                }
                else
                {
                    var selectedGroups = le.loanGroups
                         .Include(p => p.loanGroupClients)
                         .Include(p => p.loanGroupClients.Select(c => c.client))
                         .Include(p=>p.staff)
                         .ToList();
                    foreach (var p in selectedGroups)
                    {
                        var newGroup = new LoanGroupExtend
                        {
                            staff =p.staff==null ? null :  new staff {
                                staffID=p.staff.staffID,
                                staffNo=p.staff.staffNo,
                                surName=p.staff.surName,
                                otherNames=p.staff.otherNames,
                                userName=p.staff.userName,
                                branchID = p.staff.branchID
                            },
                            created = p.created,
                            leaderClientId = p.leaderClientId,
                            loanGroupClients =new List<loanGroupClient>(),
                            loanGroupDay = p.loanGroupDay,
                            creator = p.creator,
                            loanGroupDayId = p.loanGroupDayId,
                            loanGroupId = p.loanGroupId,
                            loanGroupName = p.loanGroupName,
                            loanGroupNumber = p.loanGroupNumber,
                            modified = p.modified,
                            modifier = p.modifier,
                            relationsOfficerStaffId = p.relationsOfficerStaffId
                        };
                        foreach (var lg in p.loanGroupClients)
                        {
                            var newlg = new loanGroupClient {
                                loanGroupClientId=lg.loanGroupClientId,
                                client=new client {
                                    clientID=lg.clientId,
                                    surName=lg.client.surName,
                                    otherNames=lg.client.otherNames,
                                    accountName=lg.client.accountName,
                                    accountNumber=lg.client.accountNumber,
                                    branchID=lg.client.branchID
                                },
                                clientId=lg.clientId
                            };
                            newGroup.loanGroupClients.Add(newlg);
                        }
                        groups.Add(newGroup);

                    }
                        
                }
                foreach (var group in groups)
                {
                    group.BranchName = GetBranchNameForGroup(group.loanGroupId);
                }
            }
            catch (Exception e)
            {

            }
            return groups;
        }


        // GET: api/
        [HttpGet]
        public disbursementDateModel GetDisbursementDate()
        {
            return new disbursementDateModel();
        }

        // GET: api/
        [HttpPost]
        public IEnumerable<loanGroup> GetGroupsByDate(disbursementDateModel disburseDate)
        {
            if (disburseDate == null) return null;
            var grps = le.loanGroups
                .Where(p => p.loanGroupDayId == (int)disburseDate.date.DayOfWeek)
                .Include(p => p.loanGroupClients)
                .ToList();
            return grps;
        }



        [AuthorizationFilter()]
        [HttpPost]
        public loanGroup Post(loanGroup value)
        {
            if (value == null) return null;

            if (value.loanGroupId > 0)
            {
                var toBeUpdated = le.loanGroups
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

            if (source.loanGroupClients.Count < 2)
                throw new ApplicationException(error.LoanGroupClientsBelowMin);

            foreach (var client in source.loanGroupClients)
            {
                //Remove deleted clients from group
                List<int> sourceClientIds = source.loanGroupClients.Select(p => p.loanGroupClientId).ToList();
                foreach (loanGroupClient cl in target.loanGroupClients)
                {
                    if (!sourceClientIds.Contains(cl.loanGroupClientId))
                        le.loanGroupClients.Remove(cl);
                }

                if (client.loanGroupClientId < 1)
                {
                    var tobeSaved = new loanGroupClient();
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

        [AuthorizationFilter()]
        [HttpDelete]
        // DELETE: api/
        public void Delete(loanGroup input)
        {
            loanGroup forDelete = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == input.loanGroupId);
            if (forDelete != null)
            {
                foreach (var client in forDelete.loanGroupClients)
                {
                    le.loanGroupClients.Remove(client);
                }
                le.loanGroups.Remove(forDelete);
                try
                {
                    le.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.logError(ex);
                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                }
            }
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

        private int GetUserStaffBranchId(string logginUser)
        {
            var staffBranchId = le?.staffs?.FirstOrDefault(e => e.userName.ToLower() == logginUser.ToLower())?.branchID;
            return staffBranchId.Value;
        }

        private staff GetUserStaff(string logginUser)
        {
            var userStaff = le?.staffs?.FirstOrDefault(e => e.userName.Trim().ToLower() == logginUser.Trim().ToLower());
            return userStaff;
        }

        private string GetBranchNameForStaff(staff staff)
        {
            try
            {
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

        private string GetBranchNameForGroup(int groupId)
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
        #endregion OTHER HELPERS

    }
}
