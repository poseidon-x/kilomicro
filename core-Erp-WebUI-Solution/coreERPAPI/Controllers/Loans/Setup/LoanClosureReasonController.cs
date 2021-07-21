using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Loans.SalaryLoans
{
    [AuthorizationFilter()]
    public class LoanClosureReasonController : ApiController
    {
        IcoreLoansEntities le;

        public LoanClosureReasonController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanClosureReasonController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/depositType
        public IEnumerable<loanClosureReason> Get()
        {
            return le.loanClosureReasons
                .OrderBy(p => p.loanClosureReasonName)
                .ToList();
        }

        // GET: api/depositType/5
        [HttpGet]
        public loanClosureReason Get(int id)
        {
            return le.loanClosureReasons
                .FirstOrDefault(p => p.loanClosureReasonId == id);
        }
         
        [HttpPost]
        public KendoResponse Post(loanClosureReason input)
        {
            le.loanClosureReasons
                .Add(input);
            le.SaveChanges();

            return new KendoResponse { Data = new loanClosureReason[] { input } };
        }

        [HttpPost] 
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "loanClosureReasonName";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<loanClosureReason>(req, parameters);

            var query = le.loanClosureReasons.AsQueryable();
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray());
            }

            var data = query 
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [HttpPut]
        // PUT: api/depositType/5
        public KendoResponse Put([FromBody]loanClosureReason value)
        {
            var toBeUpdated = le.loanClosureReasons.First(p => p.loanClosureReasonId == value.loanClosureReasonId);

            toBeUpdated.loanClosureReasonName = value.loanClosureReasonName;

            le.SaveChanges();

            return new KendoResponse { Data = new loanClosureReason[] { toBeUpdated } };
        }

        [HttpDelete]
        // DELETE: api/depositType/5
        public void Delete([FromBody]loanClosureReason value)
        {
            var forDelete = le.loanClosureReasons.FirstOrDefault(p => p.loanClosureReasonId == value.loanClosureReasonId);
            if (forDelete != null)
            {
                le.loanClosureReasons.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
