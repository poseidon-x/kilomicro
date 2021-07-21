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

namespace coreErpApi.Controllers.Controllers.Deposits
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    //[AuthorizationFilter()]
    public class DepositUpgradeController : ApiController
    {
        IcoreLoansEntities le;
        Icore_dbEntities ctx;
        ErrorMessages error = new ErrorMessages();
        private readonly IJournalExtensions journalextensions;


        public DepositUpgradeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx = new core_dbEntities();
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;

            journalextensions = new JournalExtensions();
        }

        public DepositUpgradeController(IcoreLoansEntities lent, Icore_dbEntities ent)
        {
            le = lent;
            ctx = ent;
        }

        [HttpGet]
        public IEnumerable<deposit> GetRunningDeposit()
        {
            var data = le.deposits
                .Where(p => p.principalBalance > 1 || p.interestBalance > 1)
                .OrderBy(p => p.depositNo)
                .ToList();
            return data;
        }

        [AuthorizationFilter()]
        [HttpGet]
        public DepositSearchModel GetDepositSearchModel()
        {
            return new DepositSearchModel();
        }

        [AuthorizationFilter()]
        [HttpGet]
        public depositUpgrade Get(int id)
        {
            var data = le.depositUpgrades
                .FirstOrDefault(p => p.depositUpgradeId == id);
            if (data == null)
            {
                return new depositUpgrade();
            }
            return data;
        }


        [AuthorizationFilter()]
        [HttpGet]
        public IEnumerable<deposit> GetUpgradeableDeposits()
        {
            List<depositType> depositTypes = le.depositTypes.ToList();
            var data = le.deposits
                .Where(p => p.depositType.upgradeable && p.interestBalance > 0 && p.principalBalance > 0)
                .Include(p => p.depositType)
                .OrderBy(p => p.depositID)
                .Take(20);
            var dataToReturn = new List<deposit>();

            foreach (var dep in data)
            {
                var depTypes = depositTypes.Where(p => p.ordinal > dep.depositType.ordinal).ToList();
                if (depTypes.Count == 0)
                { /*dataToReturn.Remove(dep); */continue; }

                List<double> depositTypesMinUpgradeAmts = depTypes
                .Select(p => p.upgradeMinimumAmount).ToList();
                double minUpgAmt = depositTypesMinUpgradeAmts.Min();
                double maxUpgAmt = depositTypesMinUpgradeAmts.Max();
                if (IsBeyondMin(dep.interestBalance + dep.principalBalance, minUpgAmt))
                { dataToReturn.Add(dep); }
            }
            return dataToReturn;
        }

        [AuthorizationFilter()]
        [HttpPost]
        public IEnumerable<deposit> GetUpgradeableDepositsBySearch(DepositSearchModel search)
        {
            List<depositType> depositTypes = le.depositTypes.ToList();
            List<deposit> data;
            if (search.criteria == 'P')
            {
                data = le.deposits
                    .Where(p => p.depositType.upgradeable && p.interestBalance > 0 || p.principalBalance > 0
                        && p.depositTypeID == search.depositTypeId)
                    .Include(p => p.depositType)
                    .OrderBy(p => p.depositID)
                    .Take(50)
                    .ToList();
            }
            else if (search.criteria == 'C')
            {
                data = le.deposits
                    .Where(p => p.depositType.upgradeable && p.interestBalance > 0 || p.principalBalance > 0
                        && p.clientID == search.clientId)
                    .Include(p => p.depositType)
                    .OrderBy(p => p.depositID)
                    .ToList();
            }
            else if (search.criteria == 'D')
            {
                data = le.deposits
                    .Where(p => p.depositType.upgradeable && p.interestBalance > 0 || p.principalBalance > 0
                                && p.depositID == search.depositId)
                    .Include(p => p.depositType)
                    .OrderBy(p => p.depositID)
                    .ToList();
            }
            else return null;


            var dataToReturn = new List<deposit>();

            foreach (var dep in data)
            {
                var depTypes = depositTypes.Where(p => p.ordinal > dep.depositType.ordinal).ToList();
                if (depTypes.Count == 0)
                { continue; }

                List<double> depositTypesMinUpgradeAmts = depTypes
                .Select(p => p.upgradeMinimumAmount).ToList();
                double minUpgAmt = depositTypesMinUpgradeAmts.Min();
                if (IsBeyondMin(dep.interestBalance + dep.principalBalance, minUpgAmt))
                { dataToReturn.Add(dep); }
            }
            return dataToReturn;
        }

        [AuthorizationFilter()]
        [HttpPost]
        public deposit PostDepositUpgrade(depositUpgrade input)
        {
            if (input == null) return null;
            var oldDepositToClose = le.deposits
                .Include(p => p.depositType)
                .Include(p => p.client)
                .FirstOrDefault(p => p.depositID == input.previousDepositId);
            if (oldDepositToClose == null) { throw new ApplicationException("Deposit to close does not exist"); }
            var comp = ctx.comp_prof.FirstOrDefault();


            if (oldDepositToClose.depositType.isCompoundInterest && input.upgradeDate.Date <= oldDepositToClose.lastInterestDate)
            {
                var interest = (oldDepositToClose.annualInterestRate / 100) / 365 * 1 * oldDepositToClose.interestBalance;
                if (interest > 0 && comp.comp_name.ToLower().Contains("ttl"))
                {
                    var inte = new depositInterest
                    {
                        principal = oldDepositToClose.principalBalance,
                        interestAmount = interest,
                        interestRate = oldDepositToClose.interestRate,
                        creation_date = DateTime.Now,
                        creator = "SYSTEM",
                        interestDate = input.upgradeDate,
                        fromDate = input.upgradeDate,
                        toDate = input.upgradeDate,
                        proposedAmount = interest
                    };
                    oldDepositToClose.depositInterests.Add(inte);
                    oldDepositToClose.interestBalance += interest;
                    input.balanceCD += interest;
                    oldDepositToClose.lastInterestDate = input.upgradeDate;

                    var jb = journalextensions.Post("LN", oldDepositToClose.depositType.interestExpenseAccountID.Value,
                        oldDepositToClose.depositType.interestPayableAccountID.Value, (interest),
                        "Interest Calculated on Deposit - " + (interest).ToString("#,###.#0")
                        + " - " + oldDepositToClose.client.accountNumber + " - " + oldDepositToClose.client.surName + "," + oldDepositToClose.client.otherNames,
                        comp.currency_id.Value, input.upgradeDate, oldDepositToClose.depositNo, ctx, "SYSTEM", oldDepositToClose.client.branchID);

                    ctx.jnl_batch.Add(jb);
                }
            }

            depositUpgrade upgradeToBeSaved = new depositUpgrade();
            deposit newDepositUpgrage = new deposit();
            populateUpgradeFieldsFields(upgradeToBeSaved, input, newDepositUpgrage, oldDepositToClose);
            oldDepositToClose.principalBalance = 0;
            oldDepositToClose.interestBalance = 0;

            le.deposits.Add(newDepositUpgrage);
            upgradeToBeSaved.newDepositId = newDepositUpgrage.depositID;
            le.depositUpgrades.Add(upgradeToBeSaved);
            try
            {
                le.SaveChanges();
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);               
            }

            return newDepositUpgrage;
        }


        private void populateUpgradeFieldsFields(depositUpgrade tobeSaved, depositUpgrade input, deposit newDepositUpgrage, deposit depToBeClosed)
        {
            tobeSaved.previousDepositId = input.previousDepositId;
            tobeSaved.balanceCD = input.balanceCD;
            tobeSaved.topUpAmount = input.topUpAmount;
            tobeSaved.creator = input.creator;
            tobeSaved.created = input.created;
            tobeSaved.interestRate = input.interestRate;
            tobeSaved.annualInterestRate = input.annualInterestRate;
            tobeSaved.depositPeriodInDays = input.depositPeriodInDays;
            tobeSaved.maturityDate = input.maturityDate;
            tobeSaved.upgradeDate = input.upgradeDate;
            tobeSaved.upgradeDepositTypeID = input.upgradeDepositTypeID;
            tobeSaved.previousDepositTypeID = depToBeClosed.depositTypeID;
            tobeSaved.topupPaymentModeId = input.topupPaymentModeId;
            if (input.topupPaymentModeId == 2)
            {
                tobeSaved.topupBankId = input.topupBankId;
                tobeSaved.topupCheckNo = input.topupCheckNo;
            }
            tobeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            tobeSaved.created = DateTime.Now;

            var dw = new depositWithdrawal
            {
                principalWithdrawal = depToBeClosed.principalBalance,
                interestWithdrawal = depToBeClosed.interestBalance,
                interestBalance = 0,
                withdrawalDate = tobeSaved.upgradeDate,
                creation_date = DateTime.Now,
                creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                principalBalance = 0,
                naration = "System withdrawal for Upgrade",
                modeOfPaymentID = 1,
                isDisInvestment = false,
                disInvestmentCharge = 0
            };
            depToBeClosed.depositWithdrawals.Add(dw);
            populateAdditionalFields(newDepositUpgrage, depToBeClosed, input);
            
        }

        private void populateAdditionalFields(deposit tobeSaved, deposit tobeClosed, depositUpgrade input)
        {
            tobeSaved.clientID = tobeClosed.clientID;
            tobeSaved.depositTypeID = input.upgradeDepositTypeID;
            tobeSaved.amountInvested = input.balanceCD + input.topUpAmount;
            tobeSaved.localAmount = input.balanceCD + input.topUpAmount;
            tobeSaved.interestAccumulated = 0;
            tobeSaved.interestBalance = 0;
            tobeSaved.principalBalance = input.balanceCD + input.topUpAmount;
            tobeSaved.interestRate = input.interestRate;
            tobeSaved.annualInterestRate = input.annualInterestRate;
            tobeSaved.depositPeriodInDays = input.depositPeriodInDays;
            tobeSaved.firstDepositDate = input.upgradeDate;
            tobeSaved.period = input.depositPeriodInDays/30;
            tobeSaved.maturityDate = input.maturityDate;
            tobeSaved.autoRollover = tobeClosed.autoRollover;
            tobeSaved.depositNo = IDGenerator.nextClientDepositNumber(le, tobeClosed.clientID);
            tobeSaved.interestMethod = false;
            tobeSaved.interestRepaymentModeID = tobeClosed.interestRepaymentModeID;
            tobeSaved.principalRepaymentModeID = tobeClosed.principalRepaymentModeID;
            tobeSaved.principalAuthorized = 0;
            tobeSaved.interestAuthorized = 0;
            tobeSaved.fxRate = tobeClosed.fxRate;
            tobeSaved.currencyID = 0;
            tobeSaved.localAmount = input.balanceCD + input.topUpAmount;
            tobeSaved.staffID = tobeClosed.staffID;
            tobeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            tobeSaved.creation_date = DateTime.Now;
            tobeSaved.modern = false;

            depositAdditional da = new depositAdditional
            {
                depositAmount = input.balanceCD,
                interestBalance = 0,
                depositDate = input.upgradeDate,
                creation_date = DateTime.Now,
                creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                principalBalance = input.balanceCD,
                modeOfPaymentID = 1,
                posted = false,
                naration = "Invt. No. " + tobeClosed.depositNo + "upgraded to " + tobeSaved.depositNo,
                fxRate = 1
            };
            tobeSaved.depositAdditionals.Add(da);
            if (input.topUpAmount>0) { 
                depositAdditional topAdditional = new depositAdditional
                {
                    checkNo = input.topupCheckNo,
                    depositAmount = input.topUpAmount,
                    bankID = input.topupBankId,
                    interestBalance = 0,
                    depositDate = input.upgradeDate,
                    creation_date = DateTime.Now,
                    creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                    principalBalance = input.topUpAmount,
                    modeOfPaymentID = input.topupPaymentModeId,
                    posted = false,
                    naration = "Topup for upgrade on Invt. No. " + tobeClosed.depositNo,
                    fxRate = 1
                };
            tobeSaved.depositAdditionals.Add(topAdditional);
            }

        CalculateSchedule(tobeSaved);
        }

        private void CalculateSchedule(deposit tobeSaved)
        {
            if (tobeSaved.depositSchedules.Count == 0 && tobeSaved.autoRollover)
            {
                List<DateTime> listInt = new List<DateTime>();
                List<DateTime> listPrinc = new List<DateTime>();
                List<DateTime> listAll = new List<DateTime>();

                DateTime date = tobeSaved.firstDepositDate;
                int i = 1;
                var totalInt = tobeSaved.amountInvested * (tobeSaved.period) * (tobeSaved.interestRate) / 100.0;
                var intererst = 0.0;
                var princ = 0.0;
                var interestRepaymentMode = le.depositRepaymentModes
                    .FirstOrDefault(p => p.repaymentModeDays == tobeSaved.interestRepaymentModeID);
                var principalRepaymentMode = le.depositRepaymentModes
                    .FirstOrDefault(p => p.repaymentModeDays == tobeSaved.interestRepaymentModeID);

                while (date < tobeSaved.maturityDate)
                {
                    date = date.AddMonths(1);
                    if (date >= tobeSaved.maturityDate) break;
                    if ((interestRepaymentMode.repaymentModeDays == 30)
                        || (interestRepaymentMode.repaymentModeDays == 90 && i % 3 == 0)
                        || (interestRepaymentMode.repaymentModeDays == 180 && i % 6 == 0)
                        )
                    {
                        listInt.Add(date);
                        if (listAll.Contains(date) == false) listAll.Add(date);
                    }
                    if ((principalRepaymentMode.repaymentModeDays == 30)
                        || (principalRepaymentMode.repaymentModeDays == 90 && i % 3 == 0)
                        || (principalRepaymentMode.repaymentModeDays == 180 && i % 6 == 0)
                        )
                    {
                        listPrinc.Add(date);
                        if (listAll.Contains(date) == false) listAll.Add(date);
                    }
                    i += 1;
                }
                listPrinc.Add(tobeSaved.maturityDate.Value);
                listInt.Add(tobeSaved.maturityDate.Value);
                listAll.Add(tobeSaved.maturityDate.Value);

                tobeSaved.modern = true;
                foreach (DateTime date2 in listAll)
                {
                    if (listPrinc.Contains(date2))
                    {
                        princ = tobeSaved.amountInvested / listPrinc.Count;
                    }
                    if (listInt.Contains(date2))
                    {
                        intererst = totalInt / listInt.Count;
                    }
                    tobeSaved.depositSchedules.Add(new coreLogic.depositSchedule
                    {
                        interestPayment = intererst,
                        principalPayment = princ,
                        repaymentDate = date2,
                        authorized = false,
                        expensed = false,
                        temp = false
                    });
                }
            }
        }

        public static bool IsBeyondMin(double value, double min)
        {
            return (value > min);
        }

    }
}
