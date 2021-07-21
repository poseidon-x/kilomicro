using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Web.Http.Cors;
using coreErpApi.Controllers.Models.CashierTransaction;

namespace coreErpApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]

    public class CashierTransactionController : ApiController
    {
        IcoreLoansEntities le;

        public CashierTransactionController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public CashierTransactionController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        public IEnumerable<cashierFund> Get()
        {
            return le.cashierFunds
                .OrderBy(p => p.fundDate)
                .ToList();
        }

        [HttpGet]
        public cashierFund Get(int id)
        {
            var data = le.cashierFunds
                .FirstOrDefault(p => p.cashierFundId == id);
            if (data == null)
            {
                data = new cashierFund();
            }
            return data;
        }

        [HttpPost]
        public cashierFund Post(cashierFund input)
        {
            if (input == null) return null;

            if (input.cashierFundId > 0)
            {
                var tobeSaved = le.cashierFunds.FirstOrDefault(p => p.cashierFundId == input.cashierFundId);
                populateField(tobeSaved, input);
                var cashierTill = le.cashiersTills.FirstOrDefault(p => p.cashiersTillID == input.cashierTillId);
                cashierTill.currentBalance += tobeSaved.transferAmount;
            }
            else
            {
                cashierFund tobeSaved = new cashierFund();
                populateField(tobeSaved, input);
                le.cashierFunds.Add(tobeSaved);
                var cashierTill = le.cashiersTills.FirstOrDefault(p => p.cashiersTillID == input.cashierTillId);
                cashierTill.currentBalance += tobeSaved.transferAmount;
            }
            le.SaveChanges();
            return input;
        }

        // PUT: api/depositType/5
        private void populateField(cashierFund tobeSaved, cashierFund input)
        {
            tobeSaved.cashierTillId = input.cashierTillId;
            tobeSaved.transferAmount = input.transferAmount;
            tobeSaved.fundDate = input.fundDate;
            if (tobeSaved.cashierFundId > 0)
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

        [HttpPost]
        public CTransViewModel GetTransactions(CTRequestViewModel request)
        {
       
            DateTime date = request.selectedDate;

            var min = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var max = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashiersTill = le.cashiersTills
                                .FirstOrDefault(p => p.userName.ToLower() == currentCashier.ToLower());

            cashierFund cashierFund = le.cashierFunds
                        .FirstOrDefault(p => p.cashierTillId == cashiersTill.cashiersTillID 
                        && p.fundDate >= min && p.fundDate <= max);

            if (cashierFund != null)
            {
                CTransViewModel data = new CTransViewModel
                {
                    cashierName = getFullName(currentCashier),

                    fundDate = date,

                    fundAmount = le.cashierFunds
                           .FirstOrDefault(p => p.cashierTillId == cashiersTill.cashiersTillID //&& p.fundDate == date
                           && p.fundDate >= min && p.fundDate <= max)
                           .transferAmount,

                    fundBalance = le.cashiersTills
                           .FirstOrDefault(p => p.userName.ToLower() == cashiersTill.userName.ToLower())
                           .currentBalance,

                    cashierReceivals = le.cashierTransactionReceipts
                           .Include(p => p.cashierTransactionReceiptCurrencies)
                           .Where(p => p.cashierTillId == cashiersTill.cashiersTillID
                           && p.receiptDate >= min && p.receiptDate <= max)
                           .ToList(),

                    cashierWithdrawals = le.cashierTransactionWithdrawals
                          .Include(p => p.cashierTransactionWithdrawalCurrencies)
                          .Where(p => p.cashierTillId == cashiersTill.cashiersTillID
                          && p.receiptDate >= min && p.receiptDate <= max)
                          .ToList()
                };

                return data;
            }
            else 
            {
                CTransViewModel data = new CTransViewModel
                {
                    cashierName = getFullName(currentCashier),
                    fundDate = date,
                    fundBalance = le.cashiersTills
                           .FirstOrDefault(p => p.userName.ToLower() == cashiersTill.userName.ToLower())
                           .currentBalance,
                    cashierReceivals = new List<cashierTransactionReceipt>(),
                    cashierWithdrawals = new List<cashierTransactionWithdrawal>()
                };

                return data;
            }
        }

        [HttpPost]
        public List<AllCTransViewModel> getAllCashierTransactions(CTAllModel req)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            List<AllCTransViewModel> data = new List<AllCTransViewModel>();

            DateTime date = req.transactionDate.Date;

            var min = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var max = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            // get all receival transactions for selected / current cashier(default)
            var cashierReceivals = le.cashierTransactionReceipts
                                     .Where(p => p.creator.ToLower() == currentCashier.ToLower()
                                     && p.receiptDate >= min && p.receiptDate <= max).ToList();

            // get all withdrawal transactions for selected / current cashier(default)
            var cashierWithdrawals = le.cashierTransactionWithdrawals
                                     .Where(p => p.creator.ToLower() == currentCashier.ToLower()
                                     && p.receiptDate >= min && p.receiptDate <= max);

            // loop through all cashier receivals and add them to the DATA collection
            if (cashierReceivals.Count() > 0)
            {
                foreach (var item in cashierReceivals)
                {
                    int transTypeID = item.transactionTypeId,
                        transID = item.transactionId;
                    string accountNo = getTransAccountNo(transTypeID, transID);

                    AllCTransViewModel transationInfo = new AllCTransViewModel
                    {
                        transTypeID = transTypeID,
                        transID = transID,
                        accountNo = accountNo
                    };
                    data.Add(transationInfo);
                }
            }

            if (cashierWithdrawals.Count() > 0)
            {
                // loop through all cashier withdrawals and add them to the DATA collection
                foreach (var item in cashierWithdrawals)
                {
                    int transTypeID = item.transactionTypeId,
                        transID = item.transactionId;
                    string accountNo = getTransAccountNo(transTypeID, transID);

                    AllCTransViewModel transactionInfo = new AllCTransViewModel
                    {
                        transTypeID = transTypeID,
                        transID = transID,
                        accountNo = accountNo
                    };
                    data.Add(transactionInfo);
                }
            }

            return data;
        }

        [HttpGet]
        public string getCurrentCashier()
        {
            return LoginHelper.getCurrentUser(new coreSecurityEntities());
        }

        public string getTransactionType(int transTypeID)
        {
            return le.transactionTypes
                .FirstOrDefault(p => p.transactionTypeId == transTypeID)
                .transactionTypeName;
        }

        [HttpPost]
        public string getTransAccountNo(int transTypeID, int transID)
        {
            var accountNo = "";

            switch (transTypeID)
            {
                case 1:
                    //Savings Withdrawal
                    var sav = le.savingWithdrawals
                        .Include(p => p.saving)
                        .FirstOrDefault(p => p.savingWithdrawalID == transID);
                    int savingID = sav.savingID;
                    accountNo = sav.saving.savingNo;
                    break;
                case 2:
                    //Investment Withdrawal
                    var depAdd = le.depositWithdrawals
                        .Include(p => p.deposit)
                        .FirstOrDefault(p => p.depositWithdrawalID == transID);
                    int DWID = depAdd.depositID;
                    accountNo = depAdd.deposit.depositNo;
                    break;
                case 3:
                    //Loan Disbursement
                    accountNo = "Check SRC Code";
                    break;
                case 4:
                    //Savings Deposit
                    var savAdd = le.savingAdditionals
                        .Include(p => p.saving)
                        .FirstOrDefault(p => p.savingAdditionalID == transID);
                    int SAID = savAdd.savingID;
                    accountNo = savAdd.saving.savingNo;
                    break;
                case 5:
                    //Investment Deposit
                    var depAddit = le.depositAdditionals
                        .Include(p => p.deposit)
                        .FirstOrDefault(p => p.depositAdditionalID == transID);
                    int DAID = depAddit.deposit.depositID;
                    accountNo = depAddit.deposit.depositNo;
                    break;
                case 6:
                    //Loan Repayment
                    var cashRecpt = le.cashierReceipts
                        .Include(p => p.loan)
                        .FirstOrDefault(p => p.cashierReceiptID == transID);
                    int LRID = cashRecpt.loanID;
                    accountNo = cashRecpt.loan.loanNo;
                    break;
            }

            return accountNo;
        }

        public string getFullName(string username)
        {
            coreSecurityEntities db = new coreSecurityEntities();

            string fullName = db.users
                .FirstOrDefault(p => p.user_name == username)
                .full_name.ToString();

            return fullName;
        }        
    }

    public class CTRequestViewModel
    {
        public DateTime selectedDate { get; set; }
        public string cashierName { get; set; }
    }

    public class CTAllModel
    {
        public string cashierName { get; set; }
        public DateTime transactionDate { get; set; }
    }

    public class CTAccountNoRequestViewModel
    {
        public int transTypeID { get; set; }
        public int transID { get; set; }
    }
}
