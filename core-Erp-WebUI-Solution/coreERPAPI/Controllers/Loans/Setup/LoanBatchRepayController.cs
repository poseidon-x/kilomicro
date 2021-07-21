using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreData.Constants;
using coreErpApi.Controllers.Models;


namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    public class LoanBatchRepayController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        public LoanBatchRepayController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanBatchRepayController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        [HttpPost]
        public IEnumerable<BatchRepaymentViewModel> Get(BatchRepayModel repayM)
        {
            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == repayM.groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            var data = le.loans
                .Where(p => groupClients.Contains(p.clientID) && p.loanTypeID == 10
                && p.disbursementDate != null)
                .Join(le.clients, l => l.clientID, c => c.clientID, (l, c) => new BatchRepaymentViewModel
                //.Join(le.repaymentSchedules, p => p.l.loanID, rs => rs.loanID, (p, rs) => new BatchRepaymentViewModel
                {
                    loanId = l.loanID,
                    loanNumberWithClient = c.surName + " "+ c.otherNames+", " + l.loanNo,
                    amountDisbursed = l.amountDisbursed,
                    //amountDue = rs.interestPayment + rs.principalPayment
                })
                //.Distinct()
                .OrderBy(p => p.loanNumberWithClient)
                .ToList();

            List<BatchRepaymentViewModel> dataToReturn = new List<BatchRepaymentViewModel>();

            foreach (var record in data)
            {
                var schd = le.repaymentSchedules.FirstOrDefault(p => p.loanID == record.loanId);
                if (schd != null)
                {
                    record.amountDue = schd.principalPayment + schd.interestPayment;
                    dataToReturn.Add(record);
                }
                
            }
            return dataToReturn;
        }

        


    }
}
