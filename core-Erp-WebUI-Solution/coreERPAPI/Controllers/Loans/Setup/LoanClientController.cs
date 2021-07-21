using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using System.Threading.Tasks;
using coreErpApi.Controllers.Models;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    //[AuthorizationFilter()]
    public class LoanClientController : ApiController
    {
        IcoreLoansEntities le;

        public LoanClientController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanClientController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/ Returns Clients with Loan
        [HttpGet]
        public  IEnumerable<ClientForRestructViewModel> Get()
        {
            var data = le.loans
                .Where(p => p.closed == false && p.balance > 0 && p.repaymentModeID == 30
                && p.disbursementDate != null
                //&& p.disbursementDate.Value.AddMonths(Convert.ToInt32(p.loanTenure)) > DateTime.Now
                )
                .Join(le.clients, l => l.clientID, c => c.clientID, (l,c) => new ClientForRestructViewModel
                {
                    clientID = l.clientID,
                    clientName = c.surName + " " + c.otherNames,
                    disbursementDate = l.disbursementDate,
                    loanTenure =  l.loanTenure
                })
                .OrderBy(p => p.clientID)
                .ToList();

            List<ClientForRestructViewModel> dat = new List<ClientForRestructViewModel>();
            foreach (var ln in data)
            {
                if (ln.disbursementDate.Value.AddMonths((int) ln.loanTenure) > DateTime.Today.AddMonths(-6))
                {
                    dat.Add(ln);
                }
            }

            return dat;
        }


        // GET: api/ Returns Clients with Loan
        public IEnumerable<ClientViewModel> GetAllLoanClients()
        {
            return le.loans.AsEnumerable()
                .Where(p => p.closed == false && p.balance > 0
                && p.disbursementDate.Value
                .AddMonths(Convert.ToInt32(p.loanTenure)) > DateTime.Now)
                .Join(le.clients, l => l.clientID , c => c.clientID, (l,c) => new ClientViewModel
                {
                    clientID = l.clientID,
                    clientName = c.surName + " " + c.otherNames
                })
                .Distinct()
                .OrderBy(p => p.clientName)
                .ToList();
        }



    }
}
