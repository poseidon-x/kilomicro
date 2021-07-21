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
using coreErpApi.Controllers.Models.Deposit;

namespace coreERP.Controllers
{
    [AuthorizationFilter()]
    public class CashierFundsTransferController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        IIDGenerator idGen = new IDGenerator();

        public CashierFundsTransferController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public CashierFundsTransferController(IcoreLoansEntities lent)
        {
            le = lent;
        }


        //Get a account
        public InterCashierTransferViewModel Get(int id)
        {
            var data = le.cashierFundsTransfers
                .FirstOrDefault(p => p.cashierFundsTransferId == id);


            InterCashierTransferViewModel dataToReturn = new InterCashierTransferViewModel
            {
                transferNotes = new List<cashierTransferNote>(),
                transferCoins = new List<cashierTransferCoin>()
            };

            if (data == null) dataToReturn.cashierFundsTransfer = new cashierFundsTransfer();
            else dataToReturn.cashierFundsTransfer = data;
            return dataToReturn;
        }

        [HttpPost]
        // POST: api/Category
        public InterCashierTransferViewModel PostDepositAdditional(InterCashierTransferViewModel input)
        {
            if (input == null) return null;
            var recCash = le.cashiersTills
                .Include(p => p.cashierRemainingNotes)
                .Include(p => p.cashierRemainingCoins)
                .FirstOrDefault(p => p.cashiersTillID == input.cashierFundsTransfer.receivingCashierTillId);
            if (recCash == null) throw new ApplicationException("Receiving cashier does not exist");
            var loginCashierName = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var loginCashier = le.cashiersTills
                .Include(p => p.cashierRemainingNotes)
                .Include(p => p.cashierRemainingCoins)
                .FirstOrDefault(p => p.userName.ToLower() == loginCashierName.ToLower());
            if (loginCashier == null) throw new ApplicationException("Sending cashier does not exist");

            //Check if cashier till is defined.
            cashierCheck(DateTime.Today, recCash.userName);

            cashierFundsTransfer cashFunTransToBeSaved = new cashierFundsTransfer();
            populateDepositAdditionalFields(cashFunTransToBeSaved, input.cashierFundsTransfer, recCash);
            le.cashierFundsTransfers.Add(cashFunTransToBeSaved);

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            
            cashierTransactionWithdrawal transacWithToBesaved = new cashierTransactionWithdrawal
            {
                transactionId = cashFunTransToBeSaved.cashierFundsTransferId
            };
            populateTransactionWithdrawalFields(transacWithToBesaved, input, loginCashier);
            le.cashierTransactionWithdrawals.Add(transacWithToBesaved);

            cashierTransactionReceipt transacToBesaved = new cashierTransactionReceipt
            {
                transactionId = cashFunTransToBeSaved.cashierFundsTransferId
            };
            populateTransactionAdditionalFields(transacToBesaved, input, recCash);
            le.cashierTransactionReceipts.Add(transacToBesaved);

                

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

        private void populateDepositAdditionalFields(cashierFundsTransfer target, cashierFundsTransfer source, cashiersTill till)
        {
            target.sendingCashierTillId = till.cashiersTillID;
            target.receivingCashierTillId = source.receivingCashierTillId;
            target.transferAmount = source.transferAmount;
            target.transferDate = DateTime.Now;
        }

        private void populateTransactionWithdrawalFields(cashierTransactionWithdrawal tobeSaved, InterCashierTransferViewModel input, cashiersTill sendersTill)
        {
            //var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            //var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            //var with = input.depositWithdrawal;
            tobeSaved.receiptDate = input.cashierFundsTransfer.transferDate;
            tobeSaved.transactionTypeId = 8;
            tobeSaved.totalReceiptAmount = input.cashierFundsTransfer.transferAmount;
            tobeSaved.cashierTillId = sendersTill.cashiersTillID;
            tobeSaved.balanceBD = sendersTill.currentBalance;
            tobeSaved.balanceCD = sendersTill.currentBalance - input.cashierFundsTransfer.transferAmount;
            tobeSaved.creator = sendersTill.userName;
            tobeSaved.created = DateTime.Now;
            //Save withdrawal Notes
            foreach (var note in input.transferNotes)
            {
                cashierTransactionWithdrawalCurrency ntCurrency = new cashierTransactionWithdrawalCurrency();
                var cashierNote = sendersTill.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateWithdrawalCurrencyNoteFields(ntCurrency, note, ref cashierNote);
                tobeSaved.cashierTransactionWithdrawalCurrencies.Add(ntCurrency);
            }
            //Save withdrawal Coins
            foreach (var coin in input.transferCoins)
            {
                cashierTransactionWithdrawalCurrency ntCurrency = new cashierTransactionWithdrawalCurrency();
                var cashierCoin = sendersTill.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateWithdrawalCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionWithdrawalCurrencies.Add(ntCurrency);
            }
            //Reduce Cashier's balance
            sendersTill.currentBalance -= tobeSaved.totalReceiptAmount;
        }


        private void populateTransactionAdditionalFields(cashierTransactionReceipt tobeSaved, InterCashierTransferViewModel input, cashiersTill receiversTill)
        {
            tobeSaved.receiptDate = input.cashierFundsTransfer.transferDate;
            tobeSaved.transactionTypeId = 8;
            tobeSaved.totalReceiptAmount = input.cashierFundsTransfer.transferAmount;
            tobeSaved.cashierTillId = receiversTill.cashiersTillID;
            tobeSaved.balanceBD = receiversTill.currentBalance;
            tobeSaved.balanceCD = receiversTill.currentBalance + input.cashierFundsTransfer.transferAmount;
            tobeSaved.creator = receiversTill.userName;
            tobeSaved.created = DateTime.Now;
            //Save withdrawal Notes
            foreach (var note in input.transferNotes)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierNote = receiversTill.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateAdditionalCurrencyNoteFields(ntCurrency, note, ref cashierNote);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Save withdrawal Coins
            foreach (var coin in input.transferCoins)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierCoin = receiversTill.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateAdditionalCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Reduce Cashier's balance
            receiversTill.currentBalance += tobeSaved.totalReceiptAmount;
        }

        private void populateAdditionalCurrencyNoteFields(cashierTransactionReceiptCurrency tobeSaved, cashierTransferNote input, ref cashierRemainingNote noteToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityWithdrawn;
            tobeSaved.total = input.quantityWithdrawn * currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD * currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            noteToBeUpdated.quantity += input.quantityWithdrawn;
            noteToBeUpdated.total += getNoteTotal(noteToBeUpdated.currencyNoteId, input.quantityWithdrawn);
        }

        private void populateAdditionalCurrencyCoinFields(cashierTransactionReceiptCurrency tobeSaved, cashierTransferCoin input, ref cashierRemainingCoin coinToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityWithdrawn;
            tobeSaved.total = input.quantityWithdrawn * currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD * currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            coinToBeUpdated.quantity += input.quantityWithdrawn;
            coinToBeUpdated.total += getNoteTotal(coinToBeUpdated.currencyNoteId, input.quantityWithdrawn);
        }

        private void populateWithdrawalCurrencyNoteFields(cashierTransactionWithdrawalCurrency tobeSaved, cashierTransferNote input, ref cashierRemainingNote noteToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityWithdrawn;
            tobeSaved.total = input.quantityWithdrawn * currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD * currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            noteToBeUpdated.quantity -= input.quantityWithdrawn;
            noteToBeUpdated.total -= getNoteTotal(noteToBeUpdated.currencyNoteId, input.quantityWithdrawn);
        }

        private void populateWithdrawalCurrencyCoinFields(cashierTransactionWithdrawalCurrency tobeSaved, cashierTransferCoin input, ref cashierRemainingCoin coinToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityWithdrawn;
            tobeSaved.total = input.quantityWithdrawn * currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD * currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            coinToBeUpdated.quantity -= input.quantityWithdrawn;
            coinToBeUpdated.total -= getNoteTotal(coinToBeUpdated.currencyNoteId, input.quantityWithdrawn);
        }

        private double getNoteTotal(int currencyNoteId, int quantity)
        {
            var cur = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == currencyNoteId);
            return cur.value * quantity;
        }

        private void cashierCheck(DateTime input,string receivingCashier = "")
        {
            var loginuser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            CashiersTillCheck ctc = new CashiersTillCheck(loginuser);
            ctc.CheckForDefinedTill();
            ctc.CheckForOpenedTill(input);
            if(!String.IsNullOrEmpty(receivingCashier))
            ctc.CheckForInterTransferReceivingCashier(receivingCashier);
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
