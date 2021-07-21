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
//    public class SalaryLoanController : ApiController
//    {
//        IcoreLoansEntities le;

//        public SalaryLoanController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public SalaryLoanController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/depositType
//        public IEnumerable<salaryLoan> Get()
//        {
//            return le.salaryLoans
//                .OrderBy(p => p.applicationDate)
//                .ToList();
//        }

//        // GET: api/depositType/5
//        [HttpGet]
//        public salaryLoan Get(int id)
//        {
//            var salaryLoan =  le.salaryLoans
//                .FirstOrDefault(p => p.salaryLoanId == id) ?? new salaryLoan();

//            return salaryLoan;
//        }

//        [HttpPost]
//        public void Post(salaryLoan input)
//        {
//            if (input.salaryLoanId == 0)
//            {
//                input.status = "N";
//                le.salaryLoans
//                    .Add(input);
//                le.SaveChanges();
//            }
//            else
//            {
//                var toBeSaved = le.salaryLoans.First(p => p.salaryLoanId == input.salaryLoanId);
//                if (toBeSaved.status == "N")
//                {
//                    toBeSaved.applicationDate = input.applicationDate;
//                    toBeSaved.employerId = input.employerId;
//                    toBeSaved.approvingDirectorId = input.approvingDirectorId;
//                    toBeSaved.applicationAmount = input.applicationAmount;
//                    toBeSaved.totalAllowances = input.totalAllowances;
//                    toBeSaved.nominalDeductions = input.nominalDeductions;
//                    toBeSaved.basicSalary = input.basicSalary;
//                    toBeSaved.clientId = input.clientId;
//                    toBeSaved.salaryLoanConfigId = input.salaryLoanConfigId;
//                    le.SaveChanges();
//                }
//            }
//        }

//        [HttpPost]
//        public void Approve(salaryLoan input)
//        {
//            if (input.salaryLoanId == 0)
//            {
//                throw new ApplicationException("Wrong Parameters");
//            }
//            else
//            {
//                var toBeSaved = le.salaryLoans.First(p => p.salaryLoanId == input.salaryLoanId);
//                if (toBeSaved.status == "N")
//                {
//                    toBeSaved.approvalDate = input.approvalDate;
//                    toBeSaved.approvedAmount = input.approvedAmount;
//                    toBeSaved.status = "A";
//                    le.SaveChanges();
//                }
//                else
//                {
//                    throw new ApplicationException("Wrong Parameters: Cannot reapprove");
//                }
//            }
//        }

//        [HttpPost]
//        public void Deny(salaryLoan input)
//        {
//            if (input.salaryLoanId == 0)
//            {
//                throw new ApplicationException("Wrong Parameters");
//            }
//            else
//            {
//                var toBeSaved = le.salaryLoans.First(p => p.salaryLoanId == input.salaryLoanId);
//                if (toBeSaved.status == "N")
//                {
//                    toBeSaved.approvalDate = input.approvalDate; 
//                    toBeSaved.status = "D";
//                    le.SaveChanges();
//                }
//                else
//                {
//                    throw new ApplicationException("Wrong Parameters: Cannot reject");
//                }
//            }
//        }

//        [HttpPost] 
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "applicationDate";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<salaryLoan>(req, parameters);

//            var query = le.salaryLoans.AsQueryable();
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

//        [HttpPost]
//        public KendoResponse GetForEdit([FromBody]KendoRequest req)
//        {
//            string order = "applicationDate";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<salaryLoan>(req, parameters);

//            var query = le.salaryLoans
//                .Where(p => p.status == "N")
//                .AsQueryable();
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

//        [HttpPost]
//        public KendoResponse GetForApproval([FromBody]KendoRequest req)
//        {
//            string order = "applicationDate";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<salaryLoan>(req, parameters);

//            var query = le.salaryLoans
//                .Where(p=> p.status == "N")
//                .AsQueryable();
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

//        [HttpPost]
//        public KendoResponse GetForDisbursement([FromBody]KendoRequest req)
//        {
//            string order = "applicationDate";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<salaryLoan>(req, parameters);

//            var query = le.salaryLoans
//                .Where(p => p.status == "A")
//                .AsQueryable();
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
//        public KendoResponse Put([FromBody]salaryLoan value)
//        {
//            var toBeUpdated = le.salaryLoans.First(p => p.salaryLoanId == value.salaryLoanId);

//            toBeUpdated.applicationAmount = value.applicationAmount;
//            toBeUpdated.applicationDate = value.applicationDate;
//            toBeUpdated.basicSalary = value.basicSalary;
//            toBeUpdated.employmentStartDate = value.employmentStartDate;
//            toBeUpdated.directorApprovalDate = value.directorApprovalDate;
//            toBeUpdated.nominalDeductions = value.nominalDeductions;
//            toBeUpdated.salaryLoanConfigId = value.salaryLoanConfigId;
//            toBeUpdated.totalAllowances = value.totalAllowances;
//            toBeUpdated.approvingDirectorId = value.approvingDirectorId;
//            toBeUpdated.clientId = value.clientId;
            
//            toBeUpdated.employerId = value.employerId;

//            le.SaveChanges();

//            return new KendoResponse { Data = new salaryLoan[] { toBeUpdated } };
//        }

//        [HttpDelete]
//        // DELETE: api/depositType/5
//        public void Delete([FromBody]salaryLoan value)
//        {
//            var forDelete = le.salaryLoans.FirstOrDefault(p => p.salaryLoanId == value.salaryLoanId);
//            if (forDelete != null)
//            {
//                le.salaryLoans.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }
         
//        [HttpPut]
//        // PUT: api/depositType/5
//        public KendoResponse Return([FromBody]salaryLoan value)
//        {
//            var toBeUpdated = le.salaryLoans.First(p => p.salaryLoanId == value.salaryLoanId);

//            toBeUpdated.status = "R"; 

//            le.SaveChanges();

//            return new KendoResponse { Data = new salaryLoan[] { toBeUpdated } };
//        }

//        [HttpPut]
//        // PUT: api/depositType/5
//        public KendoResponse Disburse([FromBody]salaryLoan value)
//        {
//            var toBeUpdated = le.salaryLoans.First(p => p.salaryLoanId == value.salaryLoanId);

//            toBeUpdated.status = "A";
//            toBeUpdated.approvedAmount = value.approvedAmount;
//            toBeUpdated.approvalDate = value.approvalDate;

//            le.SaveChanges();

//            return new KendoResponse { Data = new salaryLoan[] { toBeUpdated } };
//        }

//    }
//}
