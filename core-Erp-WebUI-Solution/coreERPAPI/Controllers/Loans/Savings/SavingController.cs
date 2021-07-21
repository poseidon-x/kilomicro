using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreData.Constants;
using coreLogic;
using coreERP.Providers;
using coreErpApi.Controllers.Models;
using coreErpApi.Controllers.Models.Deposit;

namespace coreERP.Controllers
{
    [AuthorizationFilter()]
    public class SavingController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();
        ErrorMessages error = new ErrorMessages();
        IIDGenerator idGen = new IDGenerator();

        public SavingController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<saving> Get()
        {
            return le.savings
                .OrderBy(p => p.savingNo)
                .ToList();
        }

        // GET: api/Category
        public IEnumerable<saving> Get(int id)
        {
            return le.savings
                .Where(p => p.clientID == id && (p.savingAdditionals.Count > 0))
                .OrderBy(p => p.savingNo)
                .ToList();
        }

        //Get a saving account
        [HttpGet]
        public saving GetSavingAccount(int id)
        {
            var data = le.savings
                 .Include(p => p.savingAdditionals)
                 .Include(p => p.savingWithdrawals)
                 .Include(p => p.client.clientImages.Select(q => q.image))
                 .FirstOrDefault(p => p.savingID == id);
            if (data == null) { data = new saving(); }
            return data;
        }

        //Get a saving account
        [HttpGet]
        public saving GetNewSavingAccount()
        {
            SavingViewModel data = new SavingViewModel
            {
                savingNotes = new List<savingNote>(),
                savingCoins = new List<savingCoin>()
            };
            return data;
        }

        //Get a savingAdditional account
        public savingAdditional GetSavingAdditional(int id)
        {
            var data = le.savingAdditionals
                .Include(p => p.saving)
                .FirstOrDefault(p => p.savingAdditionalID == id);
            if (data == null) { data = new savingAdditional(); }
            return data;
        }

        //Get a savingAdditional account
        public savingWithdrawal GetSavingWithdrawal(int id)
        {
            var data = le.savingWithdrawals
                .Include(p => p.saving)
                .FirstOrDefault(p => p.savingWithdrawalID == id);
            if (data == null) { data = new savingWithdrawal(); }
            return data;
        }

        [HttpPost]
        // POST: api/Category
        public saving Post([FromBody]saving value)
        {
            le.savings.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPost]
        // POST: api/Category
        public saving PostSavings(SavingViewModel input)
        {
            if (input == null) return null;
            saving savingToBeSaved = new saving
            {
                
            };

            if (input.savingID > 0)
            {
                var sav = le.savings
                    .Include(p => p.savingNextOfKins)
                    .FirstOrDefault(p => p.savingID == input.savingID);
                populateSavingsFields(sav, input);
            }
            else
            {
                populateSavingsFields(savingToBeSaved, input);
                le.savings.Add(savingToBeSaved);
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            if (le.configs.First().transactionalBankingEnabled && savingToBeSaved.savingAdditionals.Count == 1 &&
                savingToBeSaved.savingAdditionals.First().modeOfPaymentID == 1)
            {
                var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                var cashierTill = le.cashiersTills
                    .Include(p => p.cashierRemainingNotes)
                    .Include(p => p.cashierRemainingCoins)
                    .FirstOrDefault(p => p.userName == currentCashier);

                cashierTransactionReceipt transacToBesaved = new cashierTransactionReceipt
                {
                    transactionId = savingToBeSaved.savingAdditionals.First().savingAdditionalID
                };
                populateTransactionAdditionalFields(transacToBesaved, input, cashierTill);
                le.cashierTransactionReceipts.Add(transacToBesaved);

                try
                {
                    le.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                }
            }
            return savingToBeSaved;
        }

        [HttpPost]
        // POST: api/savingAdditional
        public savingAdditional PostSavingsAddtional(savingAdditional input)
        {
            if (input == null) return null;
            var savin = le.savings.FirstOrDefault(p => p.savingID == input.saving.savingID);
            if (savin == null) throw new ApplicationException("Saving Account doesn't exist");

            savingAdditional savAddToBeSaved = new savingAdditional();
            populateSavingsAdditionalFields(savAddToBeSaved, input);
            le.savingAdditionals.Add(savAddToBeSaved);
            savin.amountInvested += savAddToBeSaved.savingAmount;
            savin.principalBalance += savAddToBeSaved.savingAmount;
            savin.availablePrincipalBalance += savAddToBeSaved.savingAmount;

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            return input;
        }

        [HttpPost]
        // POST: api/Category
        public savingWithdrawal PostSavingsWithdrawal(savingWithdrawalViewModel input)
        {
            if (input == null) return null;
            var savin = le.savings.FirstOrDefault(p => p.savingID == input.saving.savingID);
            if (savin == null) throw new ApplicationException("Saving Account doesn't exist");

            savingWithdrawal sav = populateSavingsWithdrawalFields(input, savin);
            //le.savingWithdrawals.Add(sav);
            //savin.principalBalance -= input.savingWithdrawal.principalWithdrawal;
            //savin.availablePrincipalBalance -= input.savingWithdrawal.principalWithdrawal;
            //savin.interestBalance -= input.savingWithdrawal.interestWithdrawal;
            //savin.availableInterestBalance -= input.savingWithdrawal.interestWithdrawal;

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            return sav;
        }

        [HttpPut]
        public savingType Put([FromBody]savingType value)
        {
            var toBeUpdated = new savingType
            {
                accountsPayableAccountID = value.accountsPayableAccountID,
                allowsInterestWithdrawal = value.allowsInterestWithdrawal,
                allowsPrincipalWithdrawal = value.allowsPrincipalWithdrawal,
                chargesIncomeAccountID = value.chargesIncomeAccountID,
                interestCalculationScheduleID = value.interestCalculationScheduleID,
                fxRealizedGainLossAccountID = value.fxRealizedGainLossAccountID,
                fxUnrealizedGainLossAccountID = value.fxUnrealizedGainLossAccountID,
                defaultPeriod = value.defaultPeriod,
                interestExpenseAccountID = value.interestExpenseAccountID,
                interestPayableAccountID = value.interestPayableAccountID,
                interestRate = value.interestRate,
                maxPlanAmount = value.maxPlanAmount,
                minPlanAmount = value.minPlanAmount,
                vaultAccountID = value.vaultAccountID,
                planID = value.planID,
                savingTypeID = value.savingTypeID,
                savingTypeName = value.savingTypeName,
                earlyWithdrawalChargeRate = value.earlyWithdrawalChargeRate,
                minDaysBeforeInterest = value.minDaysBeforeInterest
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]savingType value)
        {
            var forDelete = le.savingTypes.FirstOrDefault(p => p.savingTypeID == value.savingTypeID);
            if (forDelete != null)
            {
                le.savingTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }

        private void populateSavingsFields(saving target, saving source)
        {
            target.clientID = source.clientID;
            target.savingTypeID = source.savingTypeID;
            target.amountInvested = 0.0;
            target.interestAccumulated = 0.0;
            target.interestBalance = 0.0;
            target.principalBalance = 0.0;
            target.interestRate = source.interestRate;
            target.autoRollover = false;
            target.interestRate = source.interestRate;
            if (source.savingID < 1)
            {
                var svTyp = le.savingTypes.FirstOrDefault(p => p.savingTypeID == source.savingTypeID);
                var cl = le.clients.FirstOrDefault(p => p.clientID == source.clientID);
                target.savingNo = idGen.NewSavingsNumber(cl.branchID.Value,
                            cl.clientID, source.savingID, svTyp.savingTypeName.Substring(0, 2).ToUpper());
                target.period = svTyp.defaultPeriod;
                target.agentId = source.agentId;
                target.maturityDate = source.firstSavingDate.AddMonths(svTyp.defaultPeriod);
                target.interestExpected = 0;
                target.availableInterestBalance = 0;
                target.availablePrincipalBalance = 0;
                target.clearedInterestBalance = 0;
                target.clearedPrincipalBalance = 0;
                target.currencyID = 1;
                target.firstSavingDate = source.firstSavingDate;
                target.fxRate = 1;
                target.interestExpected = 0;
                target.localAmount = 0;
                target.principalAuthorized = 0;
                target.principalAuthorized = 0;
                target.status = "A";
                target.interestMethod = true;
                target.principalRepaymentModeID = source.principalRepaymentModeID;
                target.interestRepaymentModeID = source.interestRepaymentModeID;
                target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                target.creation_date = DateTime.Now;
                target.savingPlanID = source.savingPlanID;

                var dep = source.savingPlans.Where(p => p.deposited == false).ToList();
                for (int i = dep.Count - 1; i >= 0; i--)
                {
                    var d = dep[i];
                    le.savingPlans.Remove(d);
                }
                var plan = source.savingPlans.FirstOrDefault();
                target.savingPlanID = source.savingPlanID;
                target.savingPlanAmount = source.savingPlanAmount;
                var date = target.firstSavingDate;
                var endDate = date.AddMonths(target.period);
                while (date < endDate && source.savingPlanID > 0)
                {
                    target.savingPlans.Add(new coreLogic.savingPlan
                    {
                        plannedAmount = source.savingPlanAmount,
                        plannedDate = date,
                        deposited = false,
                        creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                        creationDate = DateTime.Now,
                        amountDeposited = 0,
                    });
                    if (target.savingPlanID == 5 || target.savingPlanID == 6)
                    {
                        date = date.AddDays(1);
                    }
                    else if (target.savingPlanID == 7)
                    {
                        date = date.AddDays(7);
                    }
                    else if (target.savingPlanID == 30)
                    {
                        date = date.AddMonths(1);
                    }
                    else if (target.savingPlanID == 90)
                    {
                        date = date.AddMonths(3);
                    }
                    else if (target.savingPlanID == 180)
                    {
                        date = date.AddMonths(6);
                    }
                    else
                    {
                        break;
                    }
                    if (target.savingPlanID != 6 && date.DayOfWeek == DayOfWeek.Saturday)
                    {
                        date = date.AddDays(1);
                    }
                    if (date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        date = date.AddDays(1);
                    }

                }
            }

            foreach (var additioal in source.savingAdditionals)
            {
                savingAdditional savAddToBeSaved = new savingAdditional();
                populateSavingsAdditionalFields(savAddToBeSaved, additioal);
                target.savingAdditionals.Add(savAddToBeSaved);
                target.amountInvested += savAddToBeSaved.savingAmount;
                target.principalBalance += savAddToBeSaved.savingAmount;
                target.availablePrincipalBalance += savAddToBeSaved.savingAmount;
                target.savingAdditionals.Add(savAddToBeSaved);
            }

            foreach (var savNextofKin in source.savingNextOfKins)
            {
                if (savNextofKin.savingNextOfKinId > 0)
                {
                    var nextOfKinToBeSaved =
                        le.savingNextOfKins.FirstOrDefault(p => p.savingNextOfKinId == savNextofKin.savingNextOfKinId);
                    populateNextOfKinFields(nextOfKinToBeSaved, savNextofKin);
                }
                else
                {
                    savingNextOfKin nextOfKinToBeSaved = new savingNextOfKin();
                    populateNextOfKinFields(nextOfKinToBeSaved, savNextofKin);
                    target.savingNextOfKins.Add(nextOfKinToBeSaved);
                }
            }
        }

        private void populateNextOfKinFields(savingNextOfKin target, savingNextOfKin source)
        {
            //target.dob = source.dob;
            target.otherNames = source.otherNames;
            target.surName = source.surName;
            target.relationshipType = source.relationshipType;
            target.idTypeId = source.idTypeId;
            target.idNumber = source.idNumber;
            target.percentageAllocated = source.percentageAllocated;
            target.phoneNumber = source.phoneNumber;
        }

        private void populateSavingsAdditionalFields(savingAdditional target, savingAdditional source)
        {
            //target.savingID = source.saving.savingID;
            target.savingDate = source.savingDate;
            target.savingAmount = source.savingAmount;
            target.principalBalance = source.savingAmount;// + source.saving.principalBalance;
            target.balanceBD = 0;//source.saving.principalBalance + source.saving.interestBalance;
            target.interestBalance = source.interestBalance;
            target.naration = source.naration;
            if (source.modeOfPaymentID == 2)
            {
                target.checkNo = source.checkNo;
                target.bankID = source.bankID;
            }
            target.localAmount = source.localAmount;
            target.modeOfPaymentID = source.modeOfPaymentID;
            target.posted = false;
            target.closed = false;
            target.fxRate = 0;
            target.creation_date = DateTime.Now;
            target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
        }

        private savingWithdrawal populateSavingsWithdrawalFields(savingWithdrawalViewModel source, saving sav)
        {
            coreLogic.IInvestmentManager ivMgr = new coreLogic.InvestmentManager();
            var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == source.savingWithdrawal.modeOfPaymentID);
            var loggedInUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            savingWithdrawal dw = null;
            try
            {
                double pAmount = source.savingWithdrawal.principalWithdrawal;
                double iAmount = source.savingWithdrawal.interestWithdrawal;
                dw = ivMgr.WithdrawalOthers(ref pAmount, ref iAmount, mop, source.savingWithdrawal.bankID,
                    source.withType, sav, source.withdrawalAmount, source.savingWithdrawal.withdrawalDate,
                    source.savingWithdrawal.checkNo, source.savingWithdrawal.naration, loggedInUser);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured");
            }
            return dw;
        }



        //For Transactional Banking
        private void populateTransactionAdditionalFields(cashierTransactionReceipt tobeSaved, SavingViewModel input, cashiersTill till)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            tobeSaved.receiptDate = input.firstSavingDate;
            tobeSaved.transactionTypeId = 9;
            tobeSaved.cashierTillId = cashierTill.cashiersTillID;
            tobeSaved.balanceBD = cashierTill.currentBalance;
            tobeSaved.balanceCD = cashierTill.currentBalance + input.amountInvested;
            tobeSaved.creator = currentCashier;
            tobeSaved.created = DateTime.Now;
            //Save Saving Notes
            foreach (var note in input.savingNotes)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierNote = till.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateCurrencyNoteFields(ntCurrency, note, ref cashierNote);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
                tobeSaved.totalReceiptAmount += ntCurrency.total;
            }
            //Save Saving Coins
            foreach (var coin in input.savingCoins)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierCoin = till.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
                tobeSaved.totalReceiptAmount += ntCurrency.total;
            }
            //Increase Cashier's balance
            cashierTill.currentBalance += tobeSaved.totalReceiptAmount;
        }

        private void populateCurrencyNoteFields(cashierTransactionReceiptCurrency tobeSaved, savingNote input, ref cashierRemainingNote noteToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityDeposited;
            tobeSaved.total = input.quantityDeposited * currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD * currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            noteToBeUpdated.quantity += input.quantityDeposited;
            noteToBeUpdated.total += getNoteTotal(noteToBeUpdated.currencyNoteId, input.quantityDeposited);
        }

        private void populateCurrencyCoinFields(cashierTransactionReceiptCurrency tobeSaved, savingCoin input, ref cashierRemainingCoin coinToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityDeposited;
            tobeSaved.total = input.quantityDeposited * currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD * currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            coinToBeUpdated.quantity += input.quantityDeposited;
            coinToBeUpdated.total += getNoteTotal(coinToBeUpdated.currencyNoteId, input.quantityDeposited);
        }

        private double getNoteTotal(int currencyNoteId, int quantity)
        {
            var cur = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == currencyNoteId);
            return cur.value * quantity;
        }

        private void cashierCheck(DateTime input)
        {
            var loginuser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            CashiersTillCheck ctc = new CashiersTillCheck(loginuser);
            ctc.CheckForDefinedTill();
            ctc.CheckForOpenedTill(input);
            if (le.configs.FirstOrDefault().transactionalBankingEnabled)
                ctc.CheckForcashierFunding(input);
        }
    }
}
