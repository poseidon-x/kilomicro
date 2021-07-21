//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Threading.Tasks;

//namespace coreErpApi.Controllers.Controllers.Deposits
//{
//    [AuthorizationFilter()]
//    public class DepositController : ApiController
//    {
//        IcoreLoansEntities le;

//        public DepositController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public DepositController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/depositType
//        public async Task<IEnumerable<deposit>> GetClientDeposit(int id)
//        {
//            return await le.deposits
//                .Where(p => p.clientID == id && p.principalBalance > 0)
//                .OrderBy(p => p.depositID)
//                .ToListAsync();
//        }

//        // GET: api/depositType/5
//        [HttpGet]
//        public depositType Get(int id)
//        {
//            return le.depositTypes
//                .FirstOrDefault(p => p.depositTypeID == id);
//        }

//        [HttpPost]
//        public deposit PostDeposit(deposit input)
//        {
//            if (input == null) return null;
//            populateAdditionalFields(input);

//            le.deposits.Add(input);
//            le.SaveChanges();

//            return input;
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

//        private void populateAdditionalFields(deposit input)
//        {
//            input.principalBalance = input.amountInvested;
//            input.interestBalance = 0;
//            input.interestAccumulated = 0;
//            input.interestBalance = 0;
//            input.principalAuthorized = 0;
//            input.interestAuthorized = 0;
//            input.interestCalculationScheduleID = 0;
//            input.fxRate = 0;
//            input.currencyID = 0;
//            input.localAmount = 0;
//            input.modern = false;
//            input.depositNo = IDGenerator.nextClientDepositNumber(le, input.clientID);
//            input.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            input.creation_date = DateTime.Now;
//            //input.interestExpected = 

//            //depositAdditional da = null;
//            //if (input.depositID <= 0)
//            //{
//            //    //dp.principalBalance = txtAmountInvested.Value.Value;
//            //    //dp.maturityDate = dtMaturyityDate.SelectedDate;
//            //    var periodDays = 0;

//            //    if (input.period == 3)
//            //    {
//            //        periodDays = 91;
//            //    }
//            //    else if (input.period == 6)
//            //    {
//            //        periodDays = 182;
//            //    }
//            //    else if (input.period == 12)
//            //    {
//            //        periodDays = 365;
//            //    }
//            //    input.interestExpected = input.amountInvested * (periodDays / 365.0) * (input.interestRate * 12 / 100.0);

//            //    int mopID = 1;
//            //    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);

//            //    da = new coreLogic.depositAdditional
//            //    {
//            //        checkNo = txtCheckNo.Text,
//            //        depositAmount = txtAmountInvested.Value.Value,
//            //        bankID = input.b,
//            //        interestBalance = 0,
//            //        depositDate = dtAppDate.SelectedDate.Value,
//            //        creation_date = DateTime.Now,
//            //        creator = User.Identity.Name,
//            //        principalBalance = txtAmountInvested.Value.Value,
//            //        modeOfPayment = mop,
//            //        posted = false,
//            //        naration = txtNaration.Text
//            //    };
//            //    dp.depositAdditionals.Add(da);
//            //    CalculateSchedule();
//            //}
//        }
//    }
//}
