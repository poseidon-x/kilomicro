using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web.Http; 
using coreLogic; 
using coreERP.Providers;

namespace coreERP.Controllers
{
    [AuthorizationFilter()]
    public class SavingTypeController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public SavingTypeController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<savingType> Get()
        {
            return le.savingTypes
                .OrderBy(p=> p.savingTypeName)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public savingType Get(int id)
        {
            return le.savingTypes
                .FirstOrDefault(p => p.savingTypeID == id);
        }

        [HttpPost]
        // POST: api/Category
        public savingType Post([FromBody]savingType value)
        {
            le.savingTypes.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public savingType Put([FromBody]savingType value)
        {
            var toBeUpdated = new savingType
            {
                accountsPayableAccountID = value.accountsPayableAccountID,
                allowsInterestWithdrawal = value.allowsInterestWithdrawal,
                allowsPrincipalWithdrawal = value.allowsPrincipalWithdrawal,
                chargesIncomeAccountID = value.chargesIncomeAccountID,
                interestCalculationScheduleID = value.interestCalculationScheduleID,
                fxRealizedGainLossAccountID = value.fxRealizedGainLossAccountID,
                fxUnrealizedGainLossAccountID = value.fxUnrealizedGainLossAccountID,
                defaultPeriod = value.defaultPeriod,
                interestExpenseAccountID = value.interestExpenseAccountID,
                interestPayableAccountID = value.interestPayableAccountID,
                interestRate = value.interestRate,
                maxPlanAmount = value.maxPlanAmount,
                minPlanAmount = value.minPlanAmount,
                vaultAccountID = value.vaultAccountID,
                planID = value.planID,
                savingTypeID = value.savingTypeID,
                savingTypeName = value.savingTypeName,
                earlyWithdrawalChargeRate = value.earlyWithdrawalChargeRate,
                minDaysBeforeInterest = value.minDaysBeforeInterest
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]savingType value)
        {
            var forDelete = le.savingTypes.FirstOrDefault(p => p.savingTypeID == value.savingTypeID);
            if (forDelete != null)
            {
                le.savingTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
