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
    public class CashierReceiptWithCheckController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        IIDGenerator idGen = new IDGenerator();

        public CashierReceiptWithCheckController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public CashierReceiptWithCheckController(IcoreLoansEntities lent)
        {
            le = lent;
        }


        //Get a Loan repayment account
        [HttpGet]
        public CashierReceiptViewModel Get(int id)
        {
            CashierReceiptViewModel dataToReturn = new CashierReceiptViewModel();
            var data = le.cashierReceipts
                .Include(p => p.loan)
                .FirstOrDefault(p => p.cashierReceiptID == id);
            if (data == null)
            {
                dataToReturn.cashierReceipt = new cashierReceipt();
                dataToReturn.cashierReceiptCoins = new List<cashierReceiptCoin>();
                dataToReturn.cashierReceiptNotes = new List<cashierReceiptNote>();

            }
            else
            {
                dataToReturn.cashierReceipt = data;
            }

            return dataToReturn;
        }

        [HttpPost]
        // POST: api/Category
        public cashierReceipt PostCashierReceipt(CashierReceiptViewModel input)
        {
            if (input == null) return null;
            var ln = le.loans.FirstOrDefault(p => p.loanID == input.cashierReceipt.loanID);
            if (ln == null) throw new ApplicationException("Loan Account to pay for doesn't exist");

            //Check if cashier till is defined.
            cashierCheck(input.cashierReceipt.txDate);

            cashierReceipt cashierReceiptToBeSaved = new cashierReceipt();
            populateCashierReceiptFields(cashierReceiptToBeSaved, input.cashierReceipt, ln);
            le.cashierReceipts.Add(cashierReceiptToBeSaved);

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            if (cashierReceiptToBeSaved.paymentModeID == 1)
            {
                var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                var cashierTill = le.cashiersTills
                    .Include(p => p.cashierRemainingNotes)
                    .Include(p => p.cashierRemainingCoins)
                    .FirstOrDefault(p => p.userName == currentCashier);

                cashierTransactionReceipt transacToBesaved = new cashierTransactionReceipt
                {
                    transactionId = cashierReceiptToBeSaved.cashierReceiptID
                };
                populateTransactionAdditionalFields(transacToBesaved, input, cashierTill);
                le.cashierTransactionReceipts.Add(transacToBesaved);

                try
                {
                    le.SaveChanges();
                }
                catch (Exception ex)
                {
                    //throw new ApplicationException(ex.InnerException.ToString());
                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                }
            }            
            return cashierReceiptToBeSaved;
        }

        private void populateCashierReceiptFields(cashierReceipt target, cashierReceipt source, loan ln)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            target.clientID = ln.clientID;
            target.loanID = ln.loanID;
            target.txDate = source.txDate;
            target.amount = source.amount;
            target.client = ln.client;
            target.cashierTillID = cashierTill.cashiersTillID;
            target.repaymentTypeID = source.repaymentTypeID;
            target.paymentModeID = source.paymentModeID;
            target.posted = false;

            if (target.paymentModeID == 2)
            {
                target.bankID = source.bankID;
                target.checkNo = source.checkNo;
            }
            if (target.repaymentTypeID == 2) target.principalAmount = source.amount;
            else if (target.repaymentTypeID == 3) target.interestAmount = source.amount;
            else if (target.repaymentTypeID == 6) target.feeAmount = source.amount;
            else if (target.repaymentTypeID == 7) target.addInterestAmount = source.amount;
        }


        private void populateTransactionAdditionalFields(cashierTransactionReceipt tobeSaved, CashierReceiptViewModel input, cashiersTill till)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            var receipt = input.cashierReceipt;
            tobeSaved.receiptDate = receipt.txDate;//TODO change receiptDate to withdrawalDate
            tobeSaved.transactionTypeId = 6;
            tobeSaved.totalReceiptAmount = receipt.amount;
            tobeSaved.cashierTillId = cashierTill.cashiersTillID;
            tobeSaved.balanceBD = cashierTill.currentBalance;
            tobeSaved.balanceCD = cashierTill.currentBalance + receipt.amount;
            tobeSaved.creator = currentCashier;
            tobeSaved.created = DateTime.Now;
            //Save withdrawal Notes
            foreach (var note in input.cashierReceiptNotes)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierNote = till.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateCurrencyNoteFields(ntCurrency, note, ref cashierNote);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Save withdrawal Coins
            foreach (var coin in input.cashierReceiptCoins)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierCoin = till.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Reduce Cashier's balance
            cashierTill.currentBalance += tobeSaved.totalReceiptAmount;
        }

        private void populateCurrencyNoteFields(cashierTransactionReceiptCurrency tobeSaved, cashierReceiptNote input, ref cashierRemainingNote noteToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityReceived;
            tobeSaved.total = input.quantityReceived * currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD * currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            noteToBeUpdated.quantity += input.quantityReceived;
            noteToBeUpdated.total += getNoteTotal(noteToBeUpdated.currencyNoteId, input.quantityReceived);
        }

        private void populateCurrencyCoinFields(cashierTransactionReceiptCurrency tobeSaved, cashierReceiptCoin input, ref cashierRemainingCoin coinToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityReceived;
            tobeSaved.total = input.quantityReceived * currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD * currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            coinToBeUpdated.quantity += input.quantityReceived;
            coinToBeUpdated.total += getNoteTotal(coinToBeUpdated.currencyNoteId, input.quantityReceived);
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
