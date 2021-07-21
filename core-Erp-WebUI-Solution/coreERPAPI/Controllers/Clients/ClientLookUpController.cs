using coreERP.Models;
using coreERP.Providers;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using coreErpApi;
using System.Data.Entity;

namespace coreERP.Controllers.Clients
{
    [AuthorizationFilter()]
    public class ClientLookUpController : ApiController
    {

        IcoreLoansEntities le;

        public ClientLookUpController()
        {
            var le2 = new coreLoansEntities();

            le2.Configuration.LazyLoadingEnabled = false;
            le2.Configuration.ProxyCreationEnabled = false;
            le = le2;
        }

        public ClientLookUpController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        [HttpGet]
        [HttpOptions]
        // GET: api/clients
        public IEnumerable<ClientViewModel> GetSusuClients()
        {
            var query = (from c in le.clients
                         from s in le.susuAccounts
                         where c.clientID == s.clientID
                         select new ClientViewModel { 
                             clientID= c.clientID,
                             clientName = c.surName + ", " + c.otherNames + " (" + c.accountNumber+ ")",
                             accountNO = "",
                         })
                         .Distinct()
                         .OrderBy(p => p.clientName)
                         .ToList();
            return query;
        }

        [HttpGet]
        [HttpOptions]
        // GET: api/clients
        public IEnumerable<ClientViewModel> GetNormalSusuClients()
        {
            var query = (from c in le.clients
                         from s in le.regularSusuAccounts
                         where c.clientID == s.clientID
                         select new ClientViewModel
                         {
                             clientID = c.clientID,
                             clientName = c.surName + ", " + c.otherNames + " (" + c.accountNumber+ ")",
                             accountNO = c.accountNumber,
                         })
                         .Distinct()
                         .OrderBy(p => p.clientName)
                         .ToList();
            return query;
        }

        [HttpGet]
        [HttpOptions]
        // GET: api/clients
        public IEnumerable<ClientViewModel> GetSavingsClients()
        {
            var query = (from c in le.clients
                         from s in le.savings
                         where c.clientID == s.clientID
                         select new ClientViewModel
                         {
                             clientID = c.clientID,
                             clientName = c.surName + ", " + c.otherNames + " (" + c.accountNumber + ")",
                             accountNO = c.accountNumber,
                         })
                         .Distinct()
                         .OrderBy(p => p.clientName)
                         .ToList();
            return query;
        }

        [HttpGet]
        [HttpOptions]
        // GET: api/clients
        public IEnumerable<ClientViewModel> GetLoanClients()
        {
            var query = (from c in le.clients
                         from s in le.loans
                         where c.clientID == s.clientID
                         select new ClientViewModel
                         {
                             clientID = c.clientID,
                             clientName = c.surName + ", " + c.otherNames + " (" + c.accountNumber + ")",
                             accountNO = c.accountNumber,
                         }) 
                         .Distinct()
                         .OrderBy(p => p.clientName)
                         .ToList();
            return query;
        }

        //[HttpPost]

        //// GET: api/clients
        //public IEnumerable<vwActiveClient> GetActiveClients()
        //{
        //    var query = le.vwActiveClients.ToList();
        //    return query;
        //}

        [HttpPost]
        public KendoResponse GetActiveClients([FromBody]KendoRequest req)
        {

            try
            {
                var user = User?.Identity?.Name?.Trim()?.ToLower();
                var result = le.vwActiveClients.ToList();                
                if (!IsOwner(user))
                {
                    //get branch name for user
                    var userBranch = GetBranchNameForLoggedInUser(user);
                    result = result.Where(p => p.branchName.Trim().ToLower() == userBranch.Trim().ToLower()).ToList();
                }
                var query = result.OrderBy(p => p.branchName).AsQueryable();
                var data = query
                    .Skip(req.skip)
                    .Take(req.take)
                    .ToArray();
                return new KendoResponse(data, query.Count());

            }
            catch (Exception e)
            {
                return new KendoResponse(new vwActiveClient[0], 0);
            }
        }

        [HttpPost]
        public KendoResponse GetFlaggedClients([FromBody]KendoRequest req)
        {
            try
            {
                var user = User?.Identity?.Name?.Trim()?.ToLower();
                var result = le.vwFlaggedClients.ToList();
                if (!IsOwner(user))
                {
                    //get branch name for user
                    var userBranch = GetBranchNameForLoggedInUser(user);
                    result = result.Where(p => p.branchName.Trim().ToLower() == userBranch.Trim().ToLower()).ToList();
                }
                var query = result.OrderBy(p => p.branchName).AsQueryable();

                var data = query
                    .Skip(req.skip)
                    .Take(req.take)
                    .ToArray();
                return new KendoResponse(data, query.Count());

            }
            catch (Exception e)
            {
                return new KendoResponse(new vwFlaggedClient[0], 0);
            }
        }

       
        [HttpGet]
        [HttpOptions]
        // GET: api/clients
        public IEnumerable<ClientViewModel> GetEmployeeClients()
        {
            var query = (from c in le.clients 
                         where c.employeeCategories.Count>0
                         select new ClientViewModel
                         {
                             clientID = c.clientID,
                             clientName = c.surName + ", " + c.otherNames + " (" + c.accountNumber + ")",
                             accountNO = c.accountNumber,
                         })
                         .Distinct()
                         .OrderBy(p => p.clientName)
                         .ToList();
            return query;
        }

        #region OTHER HELPERS

        private string GetBranchNameForLoggedInUser(string userName)
        {
            try
            {
                var staff = le.staffs.FirstOrDefault(p => p.userName.Trim().ToLower() == userName.Trim().ToLower());
                string branchName = le?.branches?.FirstOrDefault(e => e.branchID == staff.branchID)?.branchName?.ToLower();                
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


        #endregion OTHER HELPERS

    }
}
