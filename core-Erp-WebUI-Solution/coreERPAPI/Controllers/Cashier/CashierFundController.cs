using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi
{
    [AuthorizationFilter()]
    public class CashierFundController : ApiController
    {
        IcoreLoansEntities le;

        public CashierFundController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public CashierFundController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        public IEnumerable<cashierFund> Get()
        {
            return le.cashierFunds
                .OrderBy(p => p.fundDate)
                .ToList();
        }

        [HttpGet]
        public cashierFund Get(int id)
        {
            var data = le.cashierFunds
                .FirstOrDefault(p => p.cashierFundId == id);
            if (data == null)
            {
                data = new cashierFund();
            }
            return data;
        }

        [HttpPost]
        public cashierFund Post(cashierFund input)
        {
            if (input == null) return null;

            if (input.cashierFundId > 0)
            {
                var tobeSaved = le.cashierFunds.FirstOrDefault(p => p.cashierFundId == input.cashierFundId);
                populateField(tobeSaved, input);
                var cashierTill = le.cashiersTills.FirstOrDefault(p => p.cashiersTillID == input.cashierTillId);
                cashierTill.currentBalance += tobeSaved.transferAmount;
            }
            else
            {
                cashierFund tobeSaved = new cashierFund();
                populateField(tobeSaved, input);
                le.cashierFunds.Add(tobeSaved);
                var cashierTill = le.cashiersTills.FirstOrDefault(p => p.cashiersTillID == input.cashierTillId);
                cashierTill.currentBalance += tobeSaved.transferAmount;
            }
            le.SaveChanges();
            return input;
        }

        //[HttpPost] 
        //public KendoResponse Get([FromBody]KendoRequest req)
        //{
        //    string order = "depositTypeName";

        //    KendoHelper.getSortOrder(req, ref order);
        //    var parameters = new List<object>();
        //    var whereClause = KendoHelper.getWhereClause<depositType>(req, parameters);

        //    var query = le.depositTypes.AsQueryable();
        //    if (whereClause != null && whereClause.Trim().Length > 0)
        //    {
        //        query = query.Where(whereClause, parameters.ToArray());
        //    }

        //    var data = query 
        //        .OrderBy(order.ToString())
        //        .Skip(req.skip)
        //        .Take(req.take)
        //        .ToArray();

        //    return new KendoResponse(data, query.Count());
        //}

        // PUT: api/depositType/5
        private void populateField(cashierFund tobeSaved, cashierFund input)
        {
            tobeSaved.cashierTillId = input.cashierTillId;
            tobeSaved.transferAmount = input.transferAmount;
            tobeSaved.fundDate = input.fundDate;
            if (tobeSaved.cashierFundId > 0)
            {
                tobeSaved.modified = DateTime.Now;
                tobeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
            else
            {
                tobeSaved.created = DateTime.Now;
                tobeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
        }

        
    }
}
