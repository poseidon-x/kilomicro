using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Deposits
{
    [AuthorizationFilter()]
    public class CashierCashupController : ApiController
    {
        IcoreLoansEntities le;

        public CashierCashupController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public CashierCashupController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        public IEnumerable<cashierCashup> Get()
        {
            return le.cashierCashups
                .OrderBy(p => p.cashupDate)
                .ToList();
        }

        // GET: api/depositType/5
        [HttpGet]
        public cashierCashup Get(int id)
        {
            var data = le.cashierCashups
                .FirstOrDefault(p => p.cashierCashupId == id);
            if (data == null)
            {
                data = new cashierCashup();
            }
            return data;
        }

        [HttpPost]
        public cashierCashup Post(cashierCashup input)
        {
            if (input == null) return null;

            if (input.cashierCashupId > 0)
            {
                var tobeSaved = le.cashierCashups.FirstOrDefault(p => p.cashierCashupId == input.cashierCashupId);
                populateField(tobeSaved, input);
                var cashierTill = le.cashiersTills.FirstOrDefault(p => p.cashiersTillID == input.cashierTillId);
                cashierTill.currentBalance -= tobeSaved.transferAmount;
            }
            else
            {
                cashierCashup tobeSaved = new cashierCashup();
                populateField(tobeSaved, input);
                le.cashierCashups.Add(tobeSaved);
                var cashierTill = le.cashiersTills.FirstOrDefault(p => p.cashiersTillID == input.cashierTillId);
                cashierTill.currentBalance -= tobeSaved.transferAmount;
            }
            le.SaveChanges();
            return input;
        }
        

        private void populateField(cashierCashup tobeSaved, cashierCashup input)
        {
            tobeSaved.cashierTillId = input.cashierTillId;
            tobeSaved.transferAmount = input.transferAmount;
            tobeSaved.cashupDate = input.cashupDate;
            if (tobeSaved.cashierCashupId > 0)
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
