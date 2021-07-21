using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using coreErpApi.Controllers.Models;

namespace coreErpApi.Controllers.Controllers.Deposits
{
    [AuthorizationFilter()]
    public class CashierTillController : ApiController
    {
        IcoreLoansEntities le;
        coreSecurityEntities ctx;

        public CashierTillController()
        {
            le = new coreLoansEntities();
            ctx = new coreSecurityEntities();

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        public CashierTillController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        public IEnumerable<cashiersTill> Get()
        {
            return le.cashiersTills
                .Include(p => p.cashierRemainingNotes)
                .Include(p => p.cashierRemainingCoins)
                .OrderBy(p => p.userName)
                .ToList();
        }

        public IEnumerable<CashierTillViewModel> GetCashiersName()
        {
            var data = le.cashiersTills
                .Join(ctx.users.Where(r=>r.is_active), c => c.userName, u => u.user_name, (c,u) => new CashierTillViewModel
                {
                    cashierTillId = c.cashiersTillID,
                    cashierFullName = u.full_name
                })
                .OrderBy(p => p.cashierFullName)
                .ToList();
            return data;
        }

        [HttpGet]
        public cashiersTill Get(int id)
        {
            return le.cashiersTills
                .FirstOrDefault(p => p.cashiersTillID == id);
        }

        [HttpGet]
        public cashiersTill GetCashierFunds()
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTillFund = le.cashiersTills
                .Include(p => p.cashierRemainingNotes)
                .Include(p => p.cashierRemainingCoins)
                .FirstOrDefault(p => p.userName == currentCashier);
            if (cashierTillFund == null) return null;
            return cashierTillFund;
        }

        [HttpGet]
        public cashierFund GetCashierFundToday()
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTillFund = le.cashiersTills
                .FirstOrDefault(p => p.userName == currentCashier);
            if (cashierTillFund == null) return null;
            var data = le.cashierFunds
                .FirstOrDefault(p => p.cashierTillId == cashierTillFund.cashiersTillID
                && p.fundDate == DateTime.Today);
            return data;
        }
        


        [HttpGet]
        public MyFundViewModel GetMyFunds()
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTillFund = le.cashiersTills
                                    .Include(p => p.cashierRemainingCoins)
                                    .Include(p => p.cashierRemainingNotes)
                                    .FirstOrDefault(p => p.userName.ToLower() == currentCashier.ToLower());
            if (cashierTillFund == null) return null;
            var funds = le.cashierFunds
                .FirstOrDefault(p => p.cashierTillId == cashierTillFund.cashiersTillID && p.fundDate == DateTime.Today);
            if (funds == null)
            {
                throw new ApplicationException(currentCashier+" has not been funded for today.");
            }

            MyFundViewModel data = new MyFundViewModel
            {
                cashierName = getFullName(currentCashier),
                fundDate = DateTime.Today,
                fundAmount = funds.transferAmount,
                tillData = cashierTillFund
            };

            return data;
        }

        public string getFullName(string username)
        {
            coreSecurityEntities db = new coreSecurityEntities();

            string fullName = db.users
                .FirstOrDefault(p => p.user_name == username)
                .full_name;

            return fullName;
        }

    }
}
