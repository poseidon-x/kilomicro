//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Diagnostics;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Threading.Tasks;
//using coreData.Constants;
//using coreData.ErrorLog;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;
//using System.IO;




//namespace coreErpApi.Controllers.Controllers.Loans.Document
//{
//    [AuthorizationFilter()]
//    public class LoanDocumentPlaceHolderTypeController : ApiController
//    {
//        IcoreLoansEntities le;
//        ErrorMessages error = new ErrorMessages();

//        private string ErrorToReturn = "";


//        public LoanDocumentPlaceHolderTypeController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public LoanDocumentPlaceHolderTypeController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/
//        public async Task<IEnumerable<loanDocumentPlaceHolderType>> Get()
//        {
//            return await le.loanDocumentPlaceHolderTypes
//                .OrderBy(p => p.loanDocumentPlaceHolderTypeId)
//                .ToListAsync();

//        }

//        // GET: api/
//        [HttpGet]
//        public loanDocumentPlaceHolderType Get(int id)
//        {
//            loanDocumentPlaceHolderType value = le.loanDocumentPlaceHolderTypes
//                .FirstOrDefault(p => p.loanDocumentPlaceHolderTypeId == id);

//            if (value == null)
//            {
//                value = new loanDocumentPlaceHolderType();
//            }
//            return value;
//        }

//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "placeHolderTypeCode";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.loanDocumentPlaceHolderTypes.AsQueryable();
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
//        public loanDocumentPlaceHolderType Post(loanDocumentPlaceHolderType input)
//        {

//            le.loanDocumentPlaceHolderTypes.Add(input);
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
//        // PUT: api/
//        public loanDocumentPlaceHolderType Put(loanDocumentPlaceHolderType input)
//        {
//            var toBeUpdated = le.loanDocumentPlaceHolderTypes
//                .First(p => p.loanDocumentPlaceHolderTypeId == input.loanDocumentPlaceHolderTypeId);

//            if (toBeUpdated == null)
//            {
//                ErrorToReturn = error.LoanDocumentInvalidDocPUT;
//                Logger.logError(ErrorToReturn);
//                throw new ApplicationException(ErrorToReturn);
//            }

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
//        // DELETE: api/
//        public void Delete(loanDocumentPlaceHolderType input)
//        {
//            var forDelete = le.loanDocumentPlaceHolderTypes
//                .FirstOrDefault(p => p.loanDocumentPlaceHolderTypeId == input.loanDocumentPlaceHolderTypeId);
//            if (forDelete != null)
//            {
//                le.loanDocumentPlaceHolderTypes.Remove(forDelete);
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

         
//    }
//}
