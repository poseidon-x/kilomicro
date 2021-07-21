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
    public class CashierUserController : ApiController
    {
        IcoreLoansEntities le;
        IcoreSecurityEntities ctx;


        public CashierUserController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx = new coreSecurityEntities();
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        public CashierUserController(IcoreLoansEntities lent, IcoreSecurityEntities cent)
        {
            le = lent;
            ctx = cent;
        }

        [HttpGet]
        public IEnumerable<CashierUserViewModel> Get()
        {
            var data = le.cashiersTills.ToList();
            List<CashierUserViewModel> datatoReturn = new List<CashierUserViewModel>();

            foreach (var till in data)
            {
                var d = ctx.users.FirstOrDefault(p => p.user_name == till.userName);
                if (d != null)
                {
                    CashierUserViewModel cashierUser = new CashierUserViewModel
                    {
                        cashierUserName= d.user_name,
                        cashierFullName = d.full_name
                    };
                    datatoReturn.Add(cashierUser);
                }
            }
            return datatoReturn;
        }

        

        
    }
}
