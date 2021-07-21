using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace coreERP.Controllers.Loans.Setups
{
    public class LoanTypeController : ApiController
    {
         coreLoansEntities le = new coreLoansEntities();

        public LoanTypeController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

       // GET: api//loanType
       public IEnumerable<loanType> Get()
       {
           return le.loanTypes
               .OrderBy(p => p.loanTypeName)
               .ToList();
       }

       // POST: api/loanType
       public void Post([FromBody]loanType value)
       {
           le.loanTypes.Add(value);
           le.SaveChanges();
       }

       [HttpPut]
       // PUT: api/loanType
       public void Put([FromBody]loanType value)
       {
           var toBeUpdated = new loanType
           {
               loanTypeID = value.loanTypeID,

               loanTypeName = value.loanTypeName,
               vaultAccountID = value.vaultAccountID,
               bankAccountID = value.bankAccountID,
               writeOffAccountID = value.writeOffAccountID,
               unearnedInterestAccountID = value.unearnedInterestAccountID,
               interestIncomeAccountID = value.interestIncomeAccountID,
               unpaidCommissionAccountID = value.unpaidCommissionAccountID,
               commissionAndFeesAccountID = value.commissionAndFeesAccountID,
               accountsReceivableAccountID = value.accountsReceivableAccountID,
               unearnedExtraChargesAccountID = value.unearnedExtraChargesAccountID,
               tagPrefix = value.tagPrefix,
               incentiveAccountID = value.incentiveAccountID,
               holdingAccountID = value.holdingAccountID,
               refundAccountID = value.refundAccountID,
               withHoldingAccountID = value.withHoldingAccountID,
               apIncentiveAccountID = value.apIncentiveAccountID,
               apCommissionAccountID = value.apCommissionAccountID,
               commissionAccountID = value.commissionAccountID,
               provisionExpenseAccountID = value.provisionExpenseAccountID,
               provisionsAccountID = value.provisionsAccountID 
           };
           le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
           le.SaveChanges();
       }

       [HttpDelete]
       // DELETE: api/loanType
       public void Delete([FromBody]loanType value)
       {
           var forDelete = le.loanTypes.FirstOrDefault(p => p.loanTypeID == value.loanTypeID);
           if (forDelete != null)
           {
               le.loanTypes.Remove(forDelete);
               le.SaveChanges();
           }
       }
    }
}
