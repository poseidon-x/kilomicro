//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Deposits
//{
//    [AuthorizationFilter()]
//    public class DepositTypeController : ApiController
//    {
//        IcoreLoansEntities le;

//        public DepositTypeController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public DepositTypeController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/depositType
//        public IEnumerable<depositType> Get()
//        {
//            return le.depositTypes
//                .OrderBy(p => p.depositTypeName)
//                .ToList();
//        }

//        // GET: api/depositType/5
//        [HttpGet]
//        public depositType Get(int id)
//        {
//            return le.depositTypes
//                .FirstOrDefault(p => p.depositTypeID == id);
//        }

//        [HttpPost]
//        public KendoResponse Post(depositType input)
//        {
//            le.depositTypes
//                .Add(input);
//            le.SaveChanges();

//            return new KendoResponse { Data = new depositType[] { input } };
//        }

//        [HttpPost] 
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "depositTypeName";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<depositType>(req, parameters);

//            var query = le.depositTypes.AsQueryable();
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
//        public KendoResponse Put([FromBody]depositType value)
//        {
//            var toBeUpdated = le.depositTypes.First(p => p.depositTypeID == value.depositTypeID);

//            toBeUpdated.depositTypeID = value.depositTypeID;
//            toBeUpdated.depositTypeName = value.depositTypeName;
//            toBeUpdated.interestRate = value.interestRate;
//            toBeUpdated.defaultPeriod = value.defaultPeriod;
//            toBeUpdated.allowsInterestWithdrawal = value.allowsInterestWithdrawal;
//            toBeUpdated.allowsPrincipalWithdrawal = value.allowsPrincipalWithdrawal;
//            toBeUpdated.vaultAccountID = value.vaultAccountID;
//            toBeUpdated.accountsPayableAccountID = value.accountsPayableAccountID;
//            toBeUpdated.fxUnrealizedGainLossAccountID = value.fxUnrealizedGainLossAccountID;
//            toBeUpdated.fxRealizedGainLossAccountID = value.fxRealizedGainLossAccountID;
//            toBeUpdated.interestCalculationScheduleID = value.interestCalculationScheduleID;
//            toBeUpdated.chargesIncomeAccountID = value.chargesIncomeAccountID;
//            toBeUpdated.interestPayableAccountID = value.interestPayableAccountID;

//            le.SaveChanges();

//            return new KendoResponse { Data = new depositType[] { toBeUpdated } };
//        }

//        [HttpDelete]
//        // DELETE: api/depositType/5
//        public void Delete([FromBody]depositType value)
//        {
//            var forDelete = le.depositTypes.FirstOrDefault(p => p.depositTypeID == value.depositTypeID);
//            if (forDelete != null)
//            {
//                le.depositTypes.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }
//    }
//}
