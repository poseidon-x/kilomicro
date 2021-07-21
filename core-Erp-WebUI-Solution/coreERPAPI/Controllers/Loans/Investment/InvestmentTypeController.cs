using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;

namespace coreERP.Controllers.Loans.Investment
{
    [AuthorizationFilter()]
    public class InvestmentTypeController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public InvestmentTypeController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/investmentType
        public IEnumerable<investmentType> Get()
        {
            return le.investmentTypes
                .OrderBy(p => p.investmentTypeName)
                .ToList();
        }

        // GET: api/investmentType/5
        [HttpGet]
        public investmentType Get(int id)
        {
            return le.investmentTypes
                .FirstOrDefault(p => p.investmentTypeID == id);
        }

        [HttpPost]
        // POST: api/investmentType
        public investmentType Post([FromBody]investmentType value)
        {
            le.investmentTypes.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/investmentType/5
        public investmentType Put([FromBody]investmentType value)
        {
            var toBeUpdated = new investmentType
            {
                investmentTypeID = value.investmentTypeID,
                investmentTypeName = value.investmentTypeName,
                interestRate = value.interestRate,
                defaultPeriod = value.defaultPeriod,
                allowsInterestWithdrawal = value.allowsInterestWithdrawal,
                allowsPrincipalWithdrawal = value.allowsPrincipalWithdrawal,
                vaultAccountID = value.vaultAccountID,
                accountsPayableAccountID = value.accountsPayableAccountID,
                fxUnrealizedGainLossAccountID = value.fxUnrealizedGainLossAccountID,
                fxRealizedGainLossAccountID = value.fxRealizedGainLossAccountID,
                interestCalculationScheduleID = value.interestCalculationScheduleID,
                chargesIncomeAccountID = value.chargesIncomeAccountID,
                interestReceivableAccountID = value.interestReceivableAccountID,
                interestExpenseAccountID = value.interestExpenseAccountID,
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/investmentType/5
        public void Delete([FromBody]investmentType value)
        {
            var forDelete = le.investmentTypes.FirstOrDefault(p => p.investmentTypeID == value.investmentTypeID);
            if (forDelete != null)
            {
                le.investmentTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}

