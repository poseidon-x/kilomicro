using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using coreErpApi.Controllers.Models;
using coreData.Constants;
using coreData.ErrorLog;
using coreErpApi.Controllers.Models.Deposit;

namespace coreErpApi.Controllers.Controllers.Deposits
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class DepositInterestUpgradeController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        public DepositInterestUpgradeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public DepositInterestUpgradeController(IcoreLoansEntities lent)
        {
            le = lent;
        }

       


        [HttpPost]
        public DepositInterestUpgradeVM PostApproval(DepositInterestUpgradeVM input)
        {

            var dep = le.deposits
                .Include(p => p.depositRateUpgrades)
                .FirstOrDefault(p => p.depositID == input.depositId);

            if (dep != null)
            {
                dep.annualInterestRate = input.proposedRate;
                var dru = dep.depositRateUpgrades
                    .FirstOrDefault(p => p.depositRateUpgradeId == input.depositRateUpgradeId);
                if (dru != null)
                {
                    dru.approved = true;
                    dru.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
                    dru.approvalDate = DateTime.Now;
                    input.approved = true;
                }                
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                //If saving fails, Log the the Exception and display message to user.
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return input;
        }

        [HttpPost]
        public KendoResponse GetProposedPendingApproval([FromBody]KendoRequest req)
        {
            string order = "depositNo";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<depositRateUpgrade>(req, parameters);


            var query = le.depositRateUpgrades.Where(p => !p.approved)
                .Select(p => new DepositInterestUpgradeVM
                {
                    depositRateUpgradeId = p.depositRateUpgradeId,
                    depositId = p.depositId,
                    client = p.deposit.client.surName +", "+p.deposit.client.otherNames,
                    depositNo = p.deposit.depositNo,
                    currentPrincipalBalance = Math.Round(p.currentPrincipalBalance,2),
                    currentRate = p.currentRate,
                    proposedRate = p.proposedRate,
                    approved = p.approved
                })
                .AsQueryable();
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray());
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

    }
}
