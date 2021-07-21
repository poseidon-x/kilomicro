using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Web.Http;
using coreData.Constants;
using coreErpApi;
using coreLogic;
using coreERP.Providers;
using coreErpApi.Controllers.Models;

namespace coreERP.Controllers
{
    [AuthorizationFilter()]
    public class SavingAdditionalWithCheckController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        IIDGenerator idGen = new IDGenerator();

        public SavingAdditionalWithCheckController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public SavingAdditionalWithCheckController(IcoreLoansEntities lent)
        {
            le = lent;
        }


        //Get a savingAdditional account
        public savingAdditionalViewModel Get(int id)
        {
            savingAdditionalViewModel dataToReturn = new savingAdditionalViewModel();
            var data = le.savingAdditionals
                .Include(p => p.saving)
                .FirstOrDefault(p => p.savingAdditionalID == id);
            if (data == null)
            {
                dataToReturn.savingAdditional = new savingAdditional();
                dataToReturn.savingAdditionalCoins = new List<savingAdditionalCoin>();
                dataToReturn.savingAdditionalNotes = new List<savingAdditionalNote>();

            }
            else
            {
                dataToReturn.savingAdditional = data;
            }

            return dataToReturn;
        }

        [HttpPost]
        // POST: api/Category
        public savingAdditional PostSavingsAdditional(savingAdditionalViewModel input)
        {
            if (input == null) return null;
            var savin = le.savings.FirstOrDefault(p => p.savingID == input.saving.savingID);
            if (savin == null) throw new ApplicationException("Saving Account doesn't exist");
            input.savingAdditional.saving = savin;

            //Check if cashier till is defined.
            cashierCheck(input.savingAdditional.savingDate);

            savingAdditional savAddToBeSaved = new savingAdditional();
            populateSavingsAdditionalFields(savAddToBeSaved, input.savingAdditional);

            var additionalParam = new ObjectParameter("savingAdditionalId", typeof(int));
            var currenCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());


            //var additional = input.savingAdditional;
            //var res = le.sp_attempt_deposit(savin.savingID, additional.savingAmount, currenCashier, additional.savingDate,
            //    additional.bankID,additional.checkNo,additional.modeOfPaymentID,additional.naration,additionalParam);
            //var additionalId = additionalParam.Value.ToString();
            
            //savin.savingAdditionals.Add(savAddToBeSaved);
            //savin.amountInvested += savAddToBeSaved.savingAmount;
            //savin.principalBalance += savAddToBeSaved.savingAmount;
            //savin.availablePrincipalBalance += savAddToBeSaved.savingAmount;


            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            le.SaveChanges();

            if (savAddToBeSaved.modeOfPaymentID == 1)
            {
                var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                var cashierTill = le.cashiersTills
                    .Include(p => p.cashierRemainingNotes)
                    .Include(p => p.cashierRemainingCoins)
                    .FirstOrDefault(p => p.userName == currentCashier);

                cashierTransactionReceipt transacToBesaved = new cashierTransactionReceipt
                {
                    transactionId = savAddToBeSaved.savingAdditionalID
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
            
            return savAddToBeSaved;
        }

        private void populateSavingsAdditionalFields(savingAdditional target, savingAdditional source)
        {
            target.savingDate = source.savingDate;
            target.savingAmount = source.savingAmount;
            target.principalBalance = source.savingAmount + source.saving.principalBalance;
            target.balanceBD = source.saving.principalBalance + source.saving.interestBalance;
            target.interestBalance = source.interestBalance;
            target.naration = source.naration;
            target.localAmount = source.localAmount;
            target.modeOfPaymentID = source.modeOfPaymentID;
            target.posted = false;
            target.closed = false;
            target.fxRate = 0;
            target.creation_date = DateTime.Now;
            target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
        }


        private void populateTransactionAdditionalFields(cashierTransactionReceipt tobeSaved, savingAdditionalViewModel input, cashiersTill till)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            var add = input.savingAdditional;
            tobeSaved.receiptDate = input.savingAdditional.savingDate;//TODO change receiptDate to withdrawalDate
            tobeSaved.transactionTypeId = 4;
            tobeSaved.totalReceiptAmount = add.savingAmount;//TODO change totalReceiptAmount to withdrawalAmount
            tobeSaved.cashierTillId = cashierTill.cashiersTillID;
            tobeSaved.balanceBD = cashierTill.currentBalance;
            tobeSaved.balanceCD = cashierTill.currentBalance + add.savingAmount;
            tobeSaved.creator = currentCashier;
            tobeSaved.created = DateTime.Now;
            //Save withdrawal Notes
            foreach (var note in input.savingAdditionalNotes)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierNote = till.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateCurrencyNoteFields(ntCurrency, note, ref cashierNote);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Save withdrawal Coins
            foreach (var coin in input.savingAdditionalCoins)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierCoin = till.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Reduce Cashier's balance
            cashierTill.currentBalance += tobeSaved.totalReceiptAmount;
        }

        private void cashierCheck(DateTime input)
        {
            var loginuser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            CashiersTillCheck ctc = new CashiersTillCheck(loginuser);
            ctc.CheckForDefinedTill();
            ctc.CheckForOpenedTill(input);
        }

        private void populateCurrencyNoteFields(cashierTransactionReceiptCurrency tobeSaved, savingAdditionalNote input, ref cashierRemainingNote noteToBeUpdated)
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

        private void populateCurrencyCoinFields(cashierTransactionReceiptCurrency tobeSaved, savingAdditionalCoin input, ref cashierRemainingCoin coinToBeUpdated)
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


        private void validateSavingsWithdrawal(savingWithdrawalViewModel input)
        {
            //if (String.IsNullOrEmpty(input.withType) || String.IsNullOrWhiteSpace(input.withType)
            //   || SavingExist(input.saving.savingID) || WithdrawalHasCurrency(input)
            //   || AllCurrencyNotesValid(input.savingWithdrawalNotes) || AllCurrencyCoinsValid(input.savingWithdrawalCoins))
            //{
            //    StringBuilder errors = new StringBuilder();
            //    //if (!productExists(purchOrdDet.productId))
            //    //    errors.Append(ErrorMessages.InvalidPurOrdProduct);
            //    //if (purchOrdDet.unitCost < 1)
            //    //    errors.Append(ErrorMessages.InvalidPurOrdUnitCost);
            //    //if (purchOrdDet.quantityOrdered < 1)
            //    //    errors.Append(ErrorMessages.InvalidPurOrdQuantOred);
            //    //if (purchOrdDet.totalAmount < 1)
            //    //    errors.Append(ErrorMessages.InvalidPurOrdTotalAmount);

            //    throw new ApplicationException(errors.ToString());
            //}
        }

        private bool SavingExist(int id)
        {
            if (le.savings.Any(p => p.savingID == id))
            {
                return true;
            }
            return false;

        }

        private bool WithdrawalHasCurrency(savingWithdrawalViewModel input)
        {
            if (input.savingWithdrawalCoins.Count < 1 && input.savingWithdrawalNotes.Count < 1)
            {
                return false;
            }
            return true;
        }

        private bool AllCurrencyNotesValid(List<savingWithdrawalNote> notes)
        {
            foreach (var note in notes)
            {
                if (!currencyExist(note.currencyNoteId))
                {
                    return false;
                }
            }
            return true;
        }

        private bool AllCurrencyCoinsValid(List<savingWithdrawalCoin> coins)
        {
            foreach (var coin in coins)
            {
                if (!currencyExist(coin.currencyNoteId))
                {
                    return false;
                }
            }
            return true;
        }

        private bool currencyExist(int id)
        {
            if (le.currencyNotes.Any(p => p.currencyNoteId == id))
            {
                return true;
            }
            return false;

        }
    }
}
