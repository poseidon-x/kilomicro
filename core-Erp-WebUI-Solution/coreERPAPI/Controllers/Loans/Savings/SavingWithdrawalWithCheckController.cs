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
using Telerik.Reporting;

namespace coreERP.Controllers
{
    [AuthorizationFilter()]
    public class SavingWithdrawalWithCheckController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        IIDGenerator idGen = new IDGenerator();

        public SavingWithdrawalWithCheckController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        public SavingWithdrawalWithCheckController(IcoreLoansEntities lent)
        {
            le = lent;
        }


        //Get a savingWithdrawal account
        public savingWithdrawalViewModel Get(int id)
        {
            savingWithdrawalViewModel dataToReturn = new savingWithdrawalViewModel();
            var data = le.savingWithdrawals
                .Include(p => p.saving)
                .FirstOrDefault(p => p.savingWithdrawalID == id);
            if (data == null)
            {
                dataToReturn.savingWithdrawal = new savingWithdrawal();
                dataToReturn.savingWithdrawalCoins = new List<savingWithdrawalCoin>();
                dataToReturn.savingWithdrawalNotes = new List<savingWithdrawalNote>();
                
            }
            else
            {
                dataToReturn.savingWithdrawal = data;
            }
             
            return dataToReturn;
        }

        [HttpPost]
        // POST: api/Category
        public savingWithdrawal PostSavingsWithdrawal(savingWithdrawalViewModel input)
        {
            if (input == null) return null;
            var savin = le.savings.FirstOrDefault(p => p.savingID == input.saving.savingID);
            if (savin == null) throw new ApplicationException("Saving Account doesn't exist");

            //Check if cashier till is defined.
            cashierCheck(input.savingWithdrawal.withdrawalDate);

            savingWithdrawal sav = populateSavingsWithdrawalFields(input, savin);
            //string transactionId;

             var currenCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());

            var tranParam=new ObjectParameter("transactionId", typeof(string));
            var withParam = new ObjectParameter("savingWithdrawalId", typeof(int));

            var withDrawal = input.savingWithdrawal;
            var res = le.sp_attempt_reservation(savin.savingID, withDrawal.interestWithdrawal + withDrawal.principalWithdrawal,1, currenCashier,
                input.savingWithdrawal.naration, tranParam).ToList();
            var transacId = tranParam.Value.ToString();

            var with = le.sp_withdraw_fund(savin.savingID, withDrawal.interestWithdrawal, withDrawal.principalWithdrawal,
                currenCashier, withDrawal.withdrawalDate, withDrawal.bankID, withDrawal.checkNo,
                withDrawal.modeOfPaymentID, withDrawal.naration, transacId).ToList();
            var withdrawalId = withParam.Value.ToString();



            //savin.savingWithdrawals.Add(sav);
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

            //Transaction notes if its cash transaction
            if (sav.modeOfPaymentID == 1)
            {
                var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                //var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
                var cashierTill = le.cashiersTills
                    .Include(p => p.cashierRemainingNotes)
                    .Include(p => p.cashierRemainingCoins)
                    .FirstOrDefault(p => p.userName == currentCashier);

                cashierTransactionWithdrawal transacToBesaved = new cashierTransactionWithdrawal
                {
                    transactionId = sav.savingWithdrawalID
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
            
            return sav;
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
                    source.withType, sav, source.savingWithdrawal.interestWithdrawal+ source.savingWithdrawal.principalWithdrawal, 
                    source.savingWithdrawal.withdrawalDate,
                    source.savingWithdrawal.checkNo, source.savingWithdrawal.naration, loggedInUser);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured");
            }
            return dw;
        }


        private void populateTransactionWithdrawalFields(cashierTransactionWithdrawal tobeSaved, savingWithdrawalViewModel input, cashiersTill till)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            var with = input.savingWithdrawal;
            tobeSaved.receiptDate = input.savingWithdrawal.withdrawalDate;//TODO change receiptDate to withdrawalDate
            tobeSaved.transactionTypeId = 1;
            tobeSaved.totalReceiptAmount = with.principalWithdrawal + with.interestWithdrawal;//TODO change totalReceiptAmount to withdrawalAmount
            tobeSaved.cashierTillId = cashierTill.cashiersTillID;
            tobeSaved.balanceBD = cashierTill.currentBalance;
            tobeSaved.balanceCD = cashierTill.currentBalance - tobeSaved.totalReceiptAmount;
            tobeSaved.creator = currentCashier;
            tobeSaved.created = DateTime.Now;
            //Save withdrawal Notes
            foreach (var note in input.savingWithdrawalNotes)
            {
                cashierTransactionWithdrawalCurrency ntCurrency = new cashierTransactionWithdrawalCurrency();
                var cashierNote = till.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateCurrencyNoteFields(ntCurrency,note, ref cashierNote);
                tobeSaved.cashierTransactionWithdrawalCurrencies.Add(ntCurrency);
            }
            //Save withdrawal Coins
            foreach (var coin in input.savingWithdrawalCoins)
            {
                cashierTransactionWithdrawalCurrency ntCurrency = new cashierTransactionWithdrawalCurrency();
                var cashierCoin = till.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionWithdrawalCurrencies.Add(ntCurrency);
            }
            //Reduce Cashier's balance
            cashierTill.currentBalance -= tobeSaved.totalReceiptAmount;
        }

        private void cashierCheck(DateTime input)
        {
            var loginuser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            CashiersTillCheck ctc = new CashiersTillCheck(loginuser);
            ctc.CheckForDefinedTill();
            ctc.CheckForOpenedTill(input);
        }

        private void populateCurrencyNoteFields(cashierTransactionWithdrawalCurrency tobeSaved, savingWithdrawalNote input, ref cashierRemainingNote noteToBeUpdated)
        {
            var currency = le.currencyNotes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId);
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantityWithdrawn;
            tobeSaved.total = input.quantityWithdrawn*currency.value;
            tobeSaved.quantityDB = input.quantityBD;
            tobeSaved.quantityCD = input.quantityCD;
            tobeSaved.totalDB = input.quantityBD* currency.value;
            tobeSaved.totalCD = input.quantityCD * currency.value;

            noteToBeUpdated.quantity -= input.quantityWithdrawn;
            noteToBeUpdated.total -= getNoteTotal(noteToBeUpdated.currencyNoteId, input.quantityWithdrawn);
        }

        private void populateCurrencyCoinFields(cashierTransactionWithdrawalCurrency tobeSaved, savingWithdrawalCoin input, ref cashierRemainingCoin coinToBeUpdated)
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
            return cur.value*quantity;
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
