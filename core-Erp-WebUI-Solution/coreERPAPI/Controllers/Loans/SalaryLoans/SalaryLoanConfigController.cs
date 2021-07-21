//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Loans.SalaryLoans
//{
//    [AuthorizationFilter()]
//    public class SalaryLoanConfigController : ApiController
//    {
//        IcoreLoansEntities le;

//        public SalaryLoanConfigController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public SalaryLoanConfigController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/depositType
//        public IEnumerable<salaryLoanConfig> Get()
//        {
//            return le.salaryLoanConfigs
//                .OrderBy(p => p.productName)
//                .ToList();
//        }

//        // GET: api/depositType/5
//        [HttpGet]
//        public salaryLoanConfig Get(int id)
//        {
//            return le.salaryLoanConfigs
//                .FirstOrDefault(p => p.salaryLoanConfigId == id);
//        }

//        // GET: api/depositType/5
//        [HttpGet]
//        public IEnumerable<salaryLoanConfig> GetByClient(int clientId)
//        {
//            var client = le.clients
//                .Include(p => p.employeeCategories)
//                .Include(p => p.employeeCategories.Select(q => q.employer))
//                .Include(p => p.employeeCategories.Select(q => q.employer.employerDirectors))
//                .Include(p=> p.employeeCategories.Select(q=> q.employer.salaryLoanConfigs))
//                .First(p => p.employeeCategories.Count > 0 && p.clientID == clientId);
//            return client.employeeCategories
//                .First()
//                .employer
//                .salaryLoanConfigs
//                .ToList();
//        }

//        [HttpPost]
//        public KendoResponse Post(salaryLoanConfig input)
//        {
//            le.salaryLoanConfigs
//                .Add(input);
//            le.SaveChanges();

//            return new KendoResponse { Data = new salaryLoanConfig[] { input } };
//        }

//        [HttpPost] 
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "productName";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<salaryLoanConfig>(req, parameters);

//            var query = le.salaryLoanConfigs.AsQueryable();
//            if (whereClause != null && whereClause.Trim().Length > 0)
//            {
//                query = query.Where(whereClause, parameters.ToArray());
//            }

//            var data = query 
//                .OrderBy(order.ToString())
//                .Skip(req.skip)
//                .Take(req.take)
//                .ToArray();

//            return new KendoResponse(data, query.Count());
//        }

//        [HttpPut]
//        // PUT: api/depositType/5
//        public KendoResponse Put([FromBody]salaryLoanConfig value)
//        {
//            var toBeUpdated = le.salaryLoanConfigs.First(p => p.salaryLoanConfigId == value.salaryLoanConfigId);

//            toBeUpdated.productName = value.productName;
//            toBeUpdated.interestRate = value.interestRate;
//            toBeUpdated.penaltyRate = value.penaltyRate;
//            toBeUpdated.processingFeeRate = value.processingFeeRate;
//            toBeUpdated.isActive = value.isActive;
//            toBeUpdated.tenure = value.tenure;
//            toBeUpdated.employerId = value.employerId;

//            le.SaveChanges();

//            return new KendoResponse { Data = new salaryLoanConfig[] { toBeUpdated } };
//        }

//        [HttpDelete]
//        // DELETE: api/depositType/5
//        public void Delete([FromBody]salaryLoanConfig value)
//        {
//            var forDelete = le.salaryLoanConfigs.FirstOrDefault(p => p.salaryLoanConfigId == value.salaryLoanConfigId);
//            if (forDelete != null)
//            {
//                le.salaryLoanConfigs.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }
//    }
//}
