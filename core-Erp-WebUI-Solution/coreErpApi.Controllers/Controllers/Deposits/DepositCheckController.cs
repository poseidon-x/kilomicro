////*******************************************
////***   CLIENT DEPOSIT CHECK API - CONTROLLER                
////***   CREATOR: EMMANUEL OWUSU(MAN)    	   
////***   WEEK: AUG 28TH, 2015  	
////*******************************************

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
//using coreData.Constants;
//using coreData.ErrorLog;

//namespace coreErpApi.Controllers.Controllers.Deposits
//{
//    [AuthorizationFilter()]
//    public class DepositCheckController : ApiController
//    {
//        IcoreLoansEntities le;
//        private readonly ErrorMessages error = new ErrorMessages();

//        //Default constructor
//        public DepositCheckController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        //Constructor for unit testing
//        public DepositCheckController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/clientCheck
//        public async Task<IEnumerable<clientCheck>> Get()
//        {
//            return await le.clientChecks
//                .Include(p => p.clientCheckDetails)
//                .OrderBy(p => p.clientCheckId)
//                .ToListAsync();
//        }

//        // GET: api/clientCheck/
//        [HttpGet]
//        public clientCheck Get(int id)
//        {
//            var check =  le.clientChecks
//                .FirstOrDefault(p => p.clientId == id);

//            if (check == null)
//            {
//                check = new clientCheck();
//            }

//            return check;
//        }


//        //Get all due checks
//        //[HttpPost]
//        //public async Task<KendoResponse> GetAllDueChecks(KendoResponse)
//        //{
//        //    //string order = "checkDate";

//        //    //KendoHelper.getSortOrder(req, ref order);
//        //    //var parameters = new List<object>();
//        //    //var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//        //    var query = le.clientCheckDetails
//        //        .Where(p => p.checkDate <= DateTime.Today
//        //        && p.balance < 1 && !p.cashed)
//        //        .OrderBy(p => p.checkDate)
//        //        .ToListAsync();

//        //    return await query;

//        //    //return new KendoResponse(data, query.Count());

//        //}

//        //Get all due checks
//        [HttpPost]
//        public KendoResponse GetAllDueChecksKen([FromBody]KendoRequest req)
//        {

//            string order = "checkDate";
//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.clientCheckDetails
//                .Where(p => p.checkDate <= DateTime.Today
//                && p.balance < 1 && !p.cashed)
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

//        //Get all due checks
//        [HttpPost]
//        public async Task<IEnumerable<clientCheckDetail>> GetAllDueChecks([FromBody]KendoRequest req)
//        {

//            var query = le.clientCheckDetails
//                .Where(p => p.checkDate <= DateTime.Today
//                && p.balance < 1 && !p.cashed)
//                .OrderBy(p => p.checkDate)
//                .ToListAsync();

//           return await query;

//        }

//        // POST: api/clientCheck/
//        [HttpPost]
//        public async Task<clientCheck> Post(clientCheck input)
//        {
//            clientCheck inputToBeSaved;

//            inputToBeSaved = le.clientChecks
//                    .FirstOrDefault(p => p.clientId == input.clientId);

//            if (inputToBeSaved == null)
//            {
//                //Create a new instance of client Check to save
//                inputToBeSaved = new clientCheck();
//                populateClientCheckFields(inputToBeSaved, input);
//                le.clientChecks.Add(inputToBeSaved);
//            }
            
            

//            //Save all borrowingFees for borrowing
//            foreach (var check in input.clientCheckDetails)
//            {
//                //Create a new instance of Check to save
//                clientCheckDetail checkToBeSaved = new clientCheckDetail();
//                populateClientCheckDetail(checkToBeSaved, check);
//                inputToBeSaved.clientCheckDetails.Add(checkToBeSaved);
//            }

//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                //If saving fails, Log the the Exception and display message to user.
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }
//            return inputToBeSaved;
//        }

//        // POST: api/clientCheck/ProcessCheck
//        [HttpPost]
//        public clientCheck ProcessClearedCheck(clientCheck input)
//        {
//            jnl_batch jnlBatchToBePosted;

//            if(input != null && input.clientCheckDetails.Any())
//            foreach (var ch in input.clientCheckDetails)
//            {
//                var check = le.clientCheckDetails
//                .FirstOrDefault(p => p.clientCheckDetailId == ch.clientCheckDetailId
//                                && !p.cashed);

//                if (check != null && ch.cashed)
//                {
//                    check.cashed = true;
//                    check.cashedDate = DateTime.Now;
//                    //check.sourceBankAccountId = ch.sourceBankAccountId;
//                    check.balance = check.checkAmount;
//                    check.modified = DateTime.Now;
//                    check.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                }
//            }




//            //var inputToUpdate = le.borrowings
//            //    .Include(p => p.borrowingType)
//            //    .Include(p => p.client)
//            //    .FirstOrDefault(p => p.borrowingId == input.borrowingId);

//            ////If borrowing exist update then disburse
//            //if (inputToUpdate != null)
//            //{
               
//            //   //retrieve  information to post
//            //    var ba = ctx.bank_accts.FirstOrDefault(p => p.bank_acct_id == brwDis.bankId);
//            //    int cmpCurrency = ctx.comp_prof.First().currency_id.Value;
//            //    var user = LoginHelper.getCurrentUser(new coreSecurityEntities());

//            //    //post disbursement Amount
//            //    jnlBatchToBePosted = (new JournalExtensions()).Post("BRW", inputToUpdate.borrowingType.accountsPayableAccountId, ba.gl_acct_id,
//            //        brwDis.amountDisbursed, "Borrowing Disbursement", cmpCurrency, brwDis.dateDisbursed,
//            //                inputToUpdate.borrowingNo, ctx, user, null);
//            //    ctx.jnl_batch.Add(jnlBatchToBePosted);

//            //}

            
//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                //If saving fails, Log the the Exception and display message to user.
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return input;
//        }

//        // Get: api/get all clientCashedChecks
//        [HttpGet]
//        public clientCheck GetClientCashedChecks(int id)
//        {
//            var clChks = le.clientChecks
//                .FirstOrDefault(p => p.clientId == id);

//            var cashedChecks = le.clientCheckDetails
//                .Where(p => p.clientCheckId == clChks.clientCheckId 
//                && ( p.cashed && p.balance > 0)).ToList();
//            return clChks;
//        }

//        // Get: api/clientCashedCheck
//        [HttpGet]
//        public clientCheckDetail GetClientCashedCheck(int id)
//        {
//            return le.clientCheckDetails
//                .Include(p => p.clientCheck)
//                .FirstOrDefault(p => p.clientCheckDetailId == id);
//        }

//        //process applied checks
//        [HttpPost]
//        public clientCheckDetail ProcessAppliedChecks(clientCheckDetail input)
//        {
//            if (input == null) return null;
//            var check = le.clientCheckDetails
//                 .FirstOrDefault(p => p.clientCheckDetailId == input.clientCheckDetailId
//                                 && p.cashed);

//            foreach (var application in input.checkApplies)
//            {
//                //Create a new instance of checkApplication to save
//                checkApply applicationToBeSaved = new checkApply();
//                populateCheckApplication(applicationToBeSaved, application);

//                //reduce the balance on the check
//                check.balance -= applicationToBeSaved.amountApplied;
//                check.checkApplies.Add(applicationToBeSaved);
//            }

//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                //If saving fails, Log the the Exception and display message to user.
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return check;
//        }





//        //populate clientCheck the fields to save
//        private void populateClientCheckFields(clientCheck toBeSaved, clientCheck input)
//        {
//            toBeSaved.clientId = input.clientId;
//            toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.created = DateTime.Now;
//        }

//        //populate clientCheck the fields to save
//        private void populateClientCheckDetail(clientCheckDetail toBeSaved, clientCheckDetail input)
//        {
//            toBeSaved.checkNumber = input.checkNumber;
//            toBeSaved.bankId = input.bankId;
//            toBeSaved.checkAmount = input.checkAmount;
//            toBeSaved.checkDate = input.checkDate.Date;
//            toBeSaved.checkNumber = input.checkNumber;
//            toBeSaved.balance = 0;
//            toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.created = DateTime.Now;
//        }

//        //populate clientCheck the fields to save
//        private void populateCheckApplication(checkApply toBeSaved, checkApply input)
//        {
//            toBeSaved.depositId = input.depositId;
//            toBeSaved.amountApplied = input.amountApplied;
//            toBeSaved.appliedDate = DateTime.Now; 
//            toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.created = DateTime.Now;
//        }



        
//    }
//}
