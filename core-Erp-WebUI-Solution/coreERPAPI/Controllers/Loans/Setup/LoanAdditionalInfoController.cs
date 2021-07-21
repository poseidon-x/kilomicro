using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using coreData.Constants;
using coreData.ErrorLog;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    public class LoanAdditionalInfoController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        private string ErrorToReturn = "";


        public LoanAdditionalInfoController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanAdditionalInfoController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<loanAdditionalInfo> Get()
        {
            return le.loanAdditionalInfoes
                .Include(p => p.loanMetaDatas)
                .OrderBy(p => p.loanAdditionalInfoId)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public loanAdditionalInfo Get(int id)
        {
            loanAdditionalInfo value = le.loanAdditionalInfoes
                                .Include(p => p.loanMetaDatas)
                                .FirstOrDefault(p => p.loanId == id);

            //Creates a new loanAdditionalInfo if id is null
            if (value == null)
            {
                value = new loanAdditionalInfo();
            }
            return value;
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "loanAdditionalInfoId";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

            var query = le.loanAdditionalInfoes.AsQueryable();
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

        [HttpPost]
        public loanAdditionalInfo Post(loanAdditionalInfo input)
        {
            //if (!ValidateCreditLine(input))
            //{
            //    Logger.logError(ErrorToReturn);
            //    throw new ApplicationException(ErrorToReturn);
            //}
            le.loanAdditionalInfoes.Add(input);
            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return input;
        }

        [HttpPut]
        // PUT: api/
        public loanAdditionalInfo Put(loanAdditionalInfo input)
        {
            var toBeUpdated = le.loanAdditionalInfoes
                .First(p => p.loanAdditionalInfoId == input.loanAdditionalInfoId);

            toBeUpdated.loanId = input.loanId;

            foreach (var metaData in input.loanMetaDatas)
            {
                var metaDataToBeUpdated = le.loanMetaDatas
                .FirstOrDefault(p => p.loanMetaDataId == metaData.loanMetaDataId);

                if (metaDataToBeUpdated == null)
                {
                    metaDataToBeUpdated = new loanMetaData
                    {
                        metaDataTypeId = metaData.metaDataTypeId,
                        content = metaData.content
                    };
                    toBeUpdated.loanMetaDatas.Add(metaDataToBeUpdated);
                }
                else
                {
                    metaDataToBeUpdated.metaDataTypeId = metaData.metaDataTypeId;
                    metaDataToBeUpdated.content = metaData.content;
                }

                
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/
        public void Delete(loanAdditionalInfo input)
        {
            var forDelete = le.loanAdditionalInfoes
                .Include(p => p.loanMetaDatas)
                .FirstOrDefault(p => p.loanAdditionalInfoId == input.loanAdditionalInfoId);
            if (forDelete != null)
            {
                le.loanAdditionalInfoes.Remove(forDelete);
                try
                {
                    le.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.logError(ex);
                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                }
            }
        }


         
    }
}
