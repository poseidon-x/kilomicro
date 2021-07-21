using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreReports;

namespace coreServiceEngine.Tests.Initializers
{
    public static class SavingsInterestInitializer
    {
        public static void Init(ref IcoreLoansEntities le, ref Icore_dbEntities ent,
            ref IreportEntities rent)
        {
            int PRINCIPAL_PAYABLE_ACCOUNT_ID = 1;
            int INTEREST_PAYABLE_ACCOUNT_ID = 2;
            int INTEREST_EXPENSE_ACCOUNT_ID = 3;

            le = new MockcoreLoansEntities();
            ent = new Mockcore_dbEntities();
            rent = new MockreportEntities();

            ent.comp_prof.Add(new comp_prof
            {
                currency_id = 1,
            });
            var cur = ent.currencies.Add(new currencies
            {
                currency_id = 1,
                major_name = "Cedi"
            });
            ent.accts.Add(new accts
            {
                acct_id = PRINCIPAL_PAYABLE_ACCOUNT_ID,
                acc_name = "Regular Deposits Payable Account",
                currencies = cur
            });
            ent.accts.Add(new accts
            {
                acct_id = INTEREST_EXPENSE_ACCOUNT_ID,
                acc_name = "Regular Deposits Interest Expense Account",
                currencies = cur
            });
            ent.accts.Add(new accts
            {
                acct_id = INTEREST_PAYABLE_ACCOUNT_ID,
                acc_name = "Regular Deposits Interest Payable Account",
                currencies = cur
            });
            le.savingConfigs.Add(new savingConfig
            {
                accrueInterestToPrincipal = false,
                principalBalanceLatency = 0,
                interestDecimalPlaces = 6,
                savingTypeID = 1,
            });
            var client = le.clients.Add(new client
            {
                clientID = 1,
                surName = "Adama",
                otherNames = "Aba",
                branchID = 1
            });
            var ou = ent.gl_ou.Add(new gl_ou
            {
                ou_id = 1,
                ou_name = "Testing Department"
            });
            var br = le.branches.Add(new branch
            {
                branchID = 1,
                gl_ou_id = 1,
                branchName = "Testing Branch"
            });
            client.branch = br;
            var firstSaving = new saving
            {
                savingID = 1,
                firstSavingDate = DateTime.Now.AddMonths(-3),
                principalBalance = 100,
                interestBalance = 0,
                period = 6,
                interestRate = 1.2,
                maturityDate = DateTime.Now.AddMonths(3),
                savingNo = "SAV1001",
                client = client,
                clientID = client.clientID
            };
            le.savings.Add(firstSaving);
            rent.vwSavingStatements.Add(new vwSavingStatement
            {
                depositAmount = 100,
                firstSavingDate = DateTime.Now.AddMonths(-3),
                loanID = 1,
            });

            var savType = le.savingTypes.Add(new savingType
            {
                savingTypeID = 1,
                accountsPayableAccountID = PRINCIPAL_PAYABLE_ACCOUNT_ID,
                interestPayableAccountID = INTEREST_PAYABLE_ACCOUNT_ID,
                interestExpenseAccountID = INTEREST_EXPENSE_ACCOUNT_ID
            });
            firstSaving.savingType = savType;   
        }
    }
}
