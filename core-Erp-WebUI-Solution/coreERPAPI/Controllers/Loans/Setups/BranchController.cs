using coreERP.Providers;
using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace coreERP.Controllers.Loans.Setups
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class BranchController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();
        private HelperMethod helper;

        public BranchController()
        {
            helper = new HelperMethod();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api//branch
        [HttpGet]
        public IEnumerable<branch> Get()
        {
            var branches = le.branches
                .OrderBy(p => p.branchName)
                .ToList();
            if (!helper.IsOwner(User.Identity.Name.ToLower()))
            {
                var userBranchId = helper.GetBranchIdForUser(User.Identity.Name.ToLower());
                if (userBranchId != null)
                {
                    branches = branches
                    .Where(r => r.branchID == userBranchId.Value)
                .OrderBy(p => p.branchName).ToList();
                }
            }

            return branches;

        }

        [HttpPost]
        // POST: api/branch
        public void Post([FromBody]branch value)
        {
            le.branches.Add(value);
            le.SaveChanges();
        }

        [HttpPut]
        // PUT: api/branch
        public void Put([FromBody]branch value)
        {
            var toBeUpdated = new branch
            {
                branchID = value.branchID,
                branchName = value.branchName,
                vaultAccountID = value.vaultAccountID,
                cashierTillAccountID = value.cashierTillAccountID,
                unearnedInterestAccountID = value.unearnedInterestAccountID,
                interestIncomeAccountID = value.interestIncomeAccountID,
                unpaidCommissionAccountID = value.unpaidCommissionAccountID,
                commissionAndFeesAccountID = value.commissionAndFeesAccountID,
                accountsReceivableAccountID = value.accountsReceivableAccountID,
                unearnedExtraChargesAccountID = value.unearnedExtraChargesAccountID,
                gl_ou_id = value.gl_ou_id,
                branchCode = value.branchCode
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/branch
        public void Delete([FromBody]branch value)
        {
            var forDelete = le.branches.FirstOrDefault(p => p.branchID == value.branchID);
            if (forDelete != null)
            {
                le.branches.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
