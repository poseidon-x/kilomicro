using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class DepositAdditionalWithCheckController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        IIDGenerator idGen = new IDGenerator();

        public DepositAdditionalWithCheckController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public DepositAdditionalWithCheckController(IcoreLoansEntities lent)
        {
            le = lent;
        }


        //Get a depositAdditional account
        public DepositAdditionalViewModel Get(int id)
        {
            DepositAdditionalViewModel dataToReturn = new DepositAdditionalViewModel();
            var data = le.depositAdditionals
                .Include(p => p.deposit)
                .FirstOrDefault(p => p.depositAdditionalID == id);
            if (data == null)
            {
                dataToReturn.depositAdditional = new depositAdditional();
                dataToReturn.depositAdditionalCoins = new List<depositAdditionalCoin>();
                dataToReturn.depositAdditionalNotes = new List<depositAdditionalNote>();

            }
            else
            {
                dataToReturn.depositAdditional = data;
            }

            return dataToReturn;
        }

        [HttpPost]
        // POST: api/Category
        public depositAdditional PostDepositAdditional(DepositAdditionalViewModel input)
        {
            if (input == null) return null;
            var dep = le.deposits.FirstOrDefault(p => p.depositID == input.deposit.depositID);
            if (dep == null) throw new ApplicationException("Investment Account to deposit into doesn't exist");

            //Check if cashier till is defined.
            cashierCheck(input.depositAdditional.depositDate);

            depositAdditional depositAddToBeSaved = new depositAdditional();
            populateDepositAdditionalFields(depositAddToBeSaved, input.depositAdditional, dep);
            dep.depositAdditionals.Add(depositAddToBeSaved);
            dep.amountInvested += depositAddToBeSaved.depositAmount;
            dep.principalBalance += depositAddToBeSaved.depositAmount;

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            if (depositAddToBeSaved.modeOfPaymentID == 1)
            {
                var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                var cashierTill = le.cashiersTills
                    .Include(p => p.cashierRemainingNotes)
                    .Include(p => p.cashierRemainingCoins)
                    .FirstOrDefault(p => p.userName == currentCashier);

                cashierTransactionReceipt transacToBesaved = new cashierTransactionReceipt
                {
                    transactionId = depositAddToBeSaved.depositAdditionalID
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
            return depositAddToBeSaved;
        }

        private void populateDepositAdditionalFields(depositAdditional target, depositAdditional source, deposit dep)
        {
            target.depositDate = source.depositDate;
            target.depositAmount = source.depositAmount;
            target.principalBalance = source.depositAmount + dep.principalBalance;
            target.balanceBD = dep.principalBalance;
            target.interestBalance = source.interestBalance;
            target.naration = source.naration;
            target.localAmount = source.localAmount;
            target.modeOfPaymentID = source.modeOfPaymentID;
            if (target.modeOfPaymentID == 2)
            {
                target.bankID = source.bankID;
                target.checkNo = source.checkNo;
            }
            target.posted = false;
            target.fxRate = 0;
            target.creation_date = DateTime.Now;
            target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
        }


        private void populateTransactionAdditionalFields(cashierTransactionReceipt tobeSaved, DepositAdditionalViewModel input, cashiersTill till)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            var add = input.depositAdditional;
            tobeSaved.receiptDate = input.depositAdditional.depositDate;//TODO change receiptDate to withdrawalDate
            tobeSaved.transactionTypeId = 5;
            tobeSaved.totalReceiptAmount = add.depositAmount;//TODO change totalReceiptAmount to withdrawalAmount
            tobeSaved.cashierTillId = cashierTill.cashiersTillID;
            tobeSaved.balanceBD = cashierTill.currentBalance;
            tobeSaved.balanceCD = cashierTill.currentBalance + add.depositAmount;
            tobeSaved.creator = currentCashier;
            tobeSaved.created = DateTime.Now;
            //Save withdrawal Notes
            foreach (var note in input.depositAdditionalNotes)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierNote = till.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateCurrencyNoteFields(ntCurrency, note, ref cashierNote);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Save withdrawal Coins
            foreach (var coin in input.depositAdditionalCoins)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierCoin = till.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Reduce Cashier's balance
            cashierTill.currentBalance += tobeSaved.totalReceiptAmount;
        }

        private void populateCurrencyNoteFields(cashierTransactionReceiptCurrency tobeSaved, depositAdditionalNote input, ref cashierRemainingNote noteToBeUpdated)
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

        private void populateCurrencyCoinFields(cashierTransactionReceiptCurrency tobeSaved, depositAdditionalCoin input, ref cashierRemainingCoin coinToBeUpdated)
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
