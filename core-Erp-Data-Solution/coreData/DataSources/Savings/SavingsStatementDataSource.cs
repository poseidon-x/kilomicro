using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using coreLibrary;
using coreLogic;
using coreLogic.Models;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Payment;
using Telerik.Reporting;


namespace coreData.DataSources.Savings
{
    [DataObject]
    public class SavingsStatementDataSource
    {
            private readonly IcoreLoansEntities le;
            private readonly Icore_dbEntities ctx;

        //call a constructor to instialize a the  context 
            public SavingsStatementDataSource()
        {
            var db2 = new coreLoansEntities();
            var db3 = new core_dbEntities();

            le = db2;
            ctx = db3;

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public SavingsStatementViewModel GetClientStatement(int savingId, DateTime startDate, DateTime endDate)
        {
            var sav = le.savings.FirstOrDefault(p => p.savingID == savingId);
            var cln = le.clients.Where(p => p.clientID == sav.clientID).Select(p => new ClientViewModel
            {
                clientName = p.surName+" "+p.otherNames,
                accountNO = p.accountNumber
            }).FirstOrDefault();
            SavingsStatementViewModel statementDetail = new SavingsStatementViewModel
            {
                startDate = startDate.ToString("dd-MMM-yyyy"),
                endDate = endDate.ToString("dd-MMM-yyyy"),
                compamy = GetCompanyProfile(),
                clientDet = cln
            };
            List<SavingsTransactionsViewModel> lastTransactions = new List<SavingsTransactionsViewModel>();
            var lstAdd = le.savingAdditionals
                .Where(p => p.savingID == savingId && p.savingDate < startDate)
                .OrderByDescending(p =>p.savingDate)
                .Select(p => new SavingsTransactionsViewModel
                {
                    date = p.savingDate,
                    creationDate = p.creation_date,
                    description = p.naration,
                    withdrawalAmount = 0,
                    depositAmount = p.savingAmount,
                    balance = p.principalBalance + p.interestBalance
                })
                .FirstOrDefault();
            var lastWith = le.savingWithdrawals
                .Where(p => p.savingID == savingId && p.withdrawalDate < startDate)
                .OrderByDescending(p => p.withdrawalDate)
                .Select(p => new SavingsTransactionsViewModel
                {
                    date = p.withdrawalDate,
                    creationDate = p.creation_date,
                    description = p.naration,
                    withdrawalAmount = p.principalWithdrawal + p.interestWithdrawal,
                    depositAmount = 0,
                    balance = p.principalBalance + p.interestBalance
                })
                .FirstOrDefault();

            if(lstAdd != null) lastTransactions.Add(lstAdd);
            if (lastWith != null) lastTransactions.Add(lastWith);


            statementDetail.transactions = le.savingAdditionals
                .Where(p => p.savingID == savingId && p.savingDate>= startDate && p.savingDate<=endDate)
                .Select(p => new SavingsTransactionsViewModel
                {
                    date = p.savingDate,
                    description = p.naration,
                    withdrawalAmount = 0,
                    depositAmount = p.savingAmount,
                    balance = p.principalBalance + p.interestBalance
                }).OrderBy(p => p.date)
                .ToList();

        var dataWithdrawal = le.savingWithdrawals
                .Where(p => p.savingID == savingId && p.withdrawalDate >= startDate && p.withdrawalDate <= endDate)
                .Select(p => new SavingsTransactionsViewModel
                {
                    date = p.withdrawalDate,
                    description = p.naration,
                    withdrawalAmount = p.principalWithdrawal + p.interestWithdrawal,
                    depositAmount = 0,
                    balance = p.principalBalance + p.interestBalance
                }).OrderBy(p => p.date)
                .ToList();
            statementDetail.transactions.AddRange(dataWithdrawal);
            statementDetail.totalDeposit = statementDetail.transactions.Sum(p => p.depositAmount);
            statementDetail.totalWithdrawal = statementDetail.transactions.Sum(p => p.withdrawalAmount);
            if (statementDetail.transactions.Count>0)
            {
                if (lastTransactions.Count() > 0)
                {
                    statementDetail.balanceAtStart =
                        lastTransactions.OrderByDescending(p => p.date).FirstOrDefault().balance;
                }
                else { statementDetail.balanceAtStart = 0; }
                statementDetail.balanceAtEnd = statementDetail.transactions.LastOrDefault().balance;
            }


            foreach (var transac in statementDetail.transactions)
            {
                transac.transactionDate = transac.date.ToString("dd-MMM-yyyy");
                if (transac.depositAmount < 1) { transac.strDepositAmount = "";}
                else{transac.strDepositAmount = transac.depositAmount.ToString("N2");}
                if (transac.withdrawalAmount < 1) { transac.strWithdrawalAmount = "";}
                else { transac.strWithdrawalAmount = transac.withdrawalAmount.ToString("N2"); }
            }

            return statementDetail;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public CompanyProfileViewModel GetCompanyProfile()
        {
            var data = ctx.comp_prof
                            .Select(p => new CompanyProfileViewModel
                            {
                                companyProfileId = p.comp_prof_id,
                                companyName = p.comp_name,
                                companyLogo = p.logo,
                                companyAddressLine = p.addr_line_1,
                                companyPhoneNumber = p.phon_num,
                                companyEmail = p.email,
                                companyCityId = p.city_id,
                                companyCountryId = p.country_id
                            }).First();
                            
               data.companyCity = ctx.cities.FirstOrDefault(p => p.city_id == data.companyCityId).city_name;
               data.companyCountry = ctx.countries.FirstOrDefault(p => p.country_id == data.companyCountryId).country_name;
            
            return data;
        }


    }
}

