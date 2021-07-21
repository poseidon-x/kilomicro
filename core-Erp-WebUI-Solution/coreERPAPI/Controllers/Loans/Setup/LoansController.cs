using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using System.Text;
using coreData.Constants;
using coreErpApi;

namespace coreErpApi
{
    [AuthorizationFilter()]
    public class LoansController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        private string ErrorToReturn = "";

        public LoansController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoansController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        
        [HttpGet]
        public IEnumerable<loan> GetAllRunningLoan()
        {
            var data = le.loans
                .Where(p => p.loanStatusID == 4 && p.balance > 5)
                .OrderBy(p => p.loanNo)
                .ToList();
            return data;
        }



    }
}
