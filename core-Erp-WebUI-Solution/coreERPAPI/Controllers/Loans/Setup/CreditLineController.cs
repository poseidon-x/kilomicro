//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using coreData.Constants;
//using coreData.ErrorLog;

//namespace coreErpApi.Controllers.Controllers.Loans.Setup
//{
//    [AuthorizationFilter()]
//    public class CreditLineController : ApiController
//    {
//        IcoreLoansEntities le;
//        ErrorMessages error = new ErrorMessages();

//        private string ErrorToReturn = "";


//        public CreditLineController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public CreditLineController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/
//        public IEnumerable<creditLine> Get()
//        {
//            return le.creditLines
//                .OrderBy(p => p.creditLineId)
//                .ToList();

//        }

//        // GET: api/creditLine
//        [HttpGet]
//        public creditLine Get(int id)
//        {
//            creditLine value = le.creditLines
//                .FirstOrDefault(p => p.creditLineId == id);

//            //Creates a new creditLine if id is null
//            if (value == null)
//            {
//                value = new creditLine();
//            }
//            return value;
//        }

//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "creditLineNumber";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.creditLines.AsQueryable();
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
//        public creditLine Post(creditLine input)
//        {
//            if (!ValidateCreditLine(input))
//            {
//                Logger.logError(ErrorToReturn);
//                throw new ApplicationException(ErrorToReturn);
//            }
//            input.creditLineNumber = IDGenerator.newCreditLineNumber(le, input, "CRL", 8);
//            input.isApproved = false;
//            input.closed = false;
//            input.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            input.creationDate = DateTime.Now;
//            le.creditLines.Add(input);
//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return input;
//        }

//        [HttpPut]
//        // PUT: api/creditLine
//        public creditLine Put(creditLine input)
//        {
//            if (!ValidateCreditLine(input))
//            {
//                Logger.logError(ErrorToReturn);
//                throw new ApplicationException(ErrorToReturn);
//            }
//            var toBeUpdated = le.creditLines.First(p => p.creditLineId == input.creditLineId);

//            toBeUpdated.clientId = input.clientId;
//            toBeUpdated.tenure = input.tenure;
//            toBeUpdated.amountRequested = input.amountRequested;
//            toBeUpdated.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeUpdated.modified = DateTime.Now;

//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return toBeUpdated;
//        }

//        [HttpPut]
//        public creditLine ApproveCreditLine(creditLine input)
//        {
//            if (!ValidateCreditLine(input))
//            {
//                Logger.logError(ErrorToReturn);
//                throw new ApplicationException(ErrorToReturn);
//            }
//            var toBeUpdated = le.creditLines.First(p => p.creditLineId == input.creditLineId);

//            toBeUpdated.isApproved = true;
//            toBeUpdated.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeUpdated.approvalDate = DateTime.Now;
//            toBeUpdated.amountApproved = input.amountApproved;

//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return toBeUpdated;
//        }

//        [HttpDelete]
//        // DELETE: api/creditLine
//        public void Delete(creditLine input)
//        {
//            var forDelete = le.creditLines.FirstOrDefault(p => p.creditLineId == input.creditLineId);
//            if (forDelete != null)
//            {
//                le.creditLines.Remove(forDelete);
//                try
//                {
//                    le.SaveChanges();
//                }
//                catch (Exception ex)
//                {
//                    Logger.logError(ex);
//                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//                }
//            }
//        }







//        ////****** VALIDATION ******\\\\
        

//        //Validate Fields in Assembly Line
//        private bool ValidateCreditLine(creditLine credLine)
//        {
           
//            //Execute the List of Checks
//            ValidateCreditLineDropDown(credLine);
//            ValidateCreditLineDataSize(credLine);

//            //If errorMessage is empty test Pass
//            if (ErrorToReturn == "")
//            {
//                return true;
//            }

//            ErrorToReturn += error.GridWithoutDataErrorMessage;
//            return false;
//        }

//        private void ValidateCreditLineDropDown(creditLine credLin)
//        {
//            if (!clientExist(credLin.clientId))
//            {
//                ErrorToReturn += error.CreditLineInvalidClient;
//            }
//        }

//        private void ValidateCreditLineDataSize(creditLine credLin)
//        {
//            if (credLin.amountRequested < 1 || credLin.tenure < 1)
//            {
//                ErrorToReturn = error.CreditLineNumericFieldError;
//            }
//            if (credLin.applicationDate == DateTime.MinValue)
//            {
//                ErrorToReturn += error.CreditLineInvalidDate;
//            }
//        }

//        private bool clientExist(int clientId)
//        {
//            if (le.clients.Any(p => p.clientID == clientId))
//            {
//                return true;
//            }

//            return false;
//        }


         
//    }
//}
