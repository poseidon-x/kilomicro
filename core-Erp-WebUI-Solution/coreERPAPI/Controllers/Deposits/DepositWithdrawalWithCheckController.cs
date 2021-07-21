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
    public class DepositWithdrawalWithCheckController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        IIDGenerator idGen = new IDGenerator();

        public DepositWithdrawalWithCheckController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public DepositWithdrawalWithCheckController(IcoreLoansEntities lent)
        {
            le = lent;
        }


        //Get a depositWithdrawal account
        public DepositWithdrawalViewModel Get(int id)
        {
            DepositWithdrawalViewModel dataToReturn = new DepositWithdrawalViewModel();
            var data = le.depositWithdrawals
                .Include(p => p.deposit)
                .FirstOrDefault(p => p.depositWithdrawalID == id);
            if (data == null)
            {
                dataToReturn.depositWithdrawal = new depositWithdrawal();
                dataToReturn.depositWithdrawalCoins = new List<depositWithdrawalCoin>();
                dataToReturn.depositWithdrawalNotes = new List<depositWithdrawalNote>();

            }
            else
            {
                dataToReturn.depositWithdrawal = data;
            }

            return dataToReturn;
        }

        [HttpPost]
        //POST: api/Deposit Withdrawal
        public depositWithdrawal PostDepositWithdrawal(DepositWithdrawalViewModel input)
        {
            if (input == null) return null;
            var dep = le.deposits.FirstOrDefault(p => p.depositID == input.deposit.depositID);
            if (dep == null) throw new ApplicationException("Investment Account to withdraw from doesn't exist");

            //Check if cashier till is defined.
            cashierCheck(input.depositWithdrawal.withdrawalDate);

            depositWithdrawal depWithToBeSaved = new depositWithdrawal();
            
            
            populateDepositWithdrawalFields(depWithToBeSaved, input.depositWithdrawal,input);
            dep.depositWithdrawals.Add(depWithToBeSaved);
            dep.amountInvested -= depWithToBeSaved.principalWithdrawal;
            dep.principalBalance -= depWithToBeSaved.principalWithdrawal;
            dep.interestBalance -= depWithToBeSaved.interestWithdrawal;
            dep.principalAuthorized -= depWithToBeSaved.principalWithdrawal;
            dep.interestAuthorized -= depWithToBeSaved.interestWithdrawal;
            dep.last_modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            dep.modification_date = DateTime.Now;

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            le.SaveChanges();

            

            if (depWithToBeSaved.modeOfPaymentID == 1)
            {
                var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                var cashierTill = le.cashiersTills
                    .Include(p => p.cashierRemainingNotes)
                    .Include(p => p.cashierRemainingCoins)
                    .FirstOrDefault(p => p.userName == currentCashier);

                cashierTransactionWithdrawal transacToBesaved = new cashierTransactionWithdrawal
                {
                    transactionId = depWithToBeSaved.depositWithdrawalID
                };
                populateTransactionWithdrawalFields(transacToBesaved, input, cashierTill);
                le.cashierTransactionWithdrawals.Add(transacToBesaved);

                try
                {
                    le.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                }
            }
                        
            return depWithToBeSaved;
        }

        private void populateDepositWithdrawalFields(depositWithdrawal target, depositWithdrawal source, DepositWithdrawalViewModel dep)
        {
            target.checkNo = source.checkNo;
            target.principalWithdrawal = source.principalWithdrawal;
            target.interestWithdrawal = source.interestWithdrawal;
            target.bankID = source.bankID;
            target.interestBalance = source.interestBalance;
            target.withdrawalDate = source.withdrawalDate;
            target.creation_date = DateTime.Now;
            target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            target.principalBalance = dep.deposit.principalBalance;
            target.naration = source.naration;
            target.modeOfPaymentID = source.modeOfPaymentID;
            target.posted = false;
            target.isDisInvestment = dep.isDisInvstment;
            target.disInvestmentCharge = dep.isDisInvstment ? dep.disInvstmentCharge : 0;
        }


        private void populateTransactionWithdrawalFields(cashierTransactionWithdrawal tobeSaved, DepositWithdrawalViewModel input, cashiersTill till)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            var with = input.depositWithdrawal;
            tobeSaved.receiptDate = input.depositWithdrawal.withdrawalDate;//TODO change receiptDate to withdrawalDate
            tobeSaved.transactionTypeId = 2;
            tobeSaved.totalReceiptAmount = input.depositWithdrawal.principalWithdrawal + input.depositWithdrawal.interestWithdrawal;//TODO change totalReceiptAmount to withdrawalAmount
            tobeSaved.cashierTillId = cashierTill.cashiersTillID;
            tobeSaved.balanceBD = cashierTill.currentBalance;
            tobeSaved.balanceCD = cashierTill.currentBalance - tobeSaved.totalReceiptAmount;
            tobeSaved.creator = currentCashier;
            tobeSaved.created = DateTime.Now;
            //Save withdrawal Notes
            foreach (var note in input.depositWithdrawalNotes)
            {
                cashierTransactionWithdrawalCurrency ntCurrency = new cashierTransactionWithdrawalCurrency();
                var cashierNote = till.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateCurrencyNoteFields(ntCurrency, note, ref cashierNote);
                tobeSaved.cashierTransactionWithdrawalCurrencies.Add(ntCurrency);
            }
            //Save withdrawal Coins
            foreach (var coin in input.depositWithdrawalCoins)
            {
                cashierTransactionWithdrawalCurrency ntCurrency = new cashierTransactionWithdrawalCurrency();
                var cashierCoin = till.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionWithdrawalCurrencies.Add(ntCurrency);
            }
            //Reduce Cashier's balance
            cashierTill.currentBalance -= tobeSaved.totalReceiptAmount;
        }

        private void populateCurrencyNoteFields(cashierTransactionWithdrawalCurrency tobeSaved, depositWithdrawalNote input, ref cashierRemainingNote noteToBeUpdated)
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

        private void populateCurrencyCoinFields(cashierTransactionWithdrawalCurrency tobeSaved, depositWithdrawalCoin input, ref cashierRemainingCoin coinToBeUpdated)
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
