using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using coreErpApi.Controllers.Models;

namespace coreErpApi.Controllers.Controllers.Cashier
{
    //[AuthorizationFilter()]
    public class CashierFundingController : ApiController
    {
        IcoreLoansEntities le;

        public CashierFundingController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public CashierFundingController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        [HttpPost]
        public CashierFundingViewModel Get(CashierFundingViewModel model)
        {
            var fundsData =  le.cashierFunds
                .Where(p => p.fundDate == model.fundingDate.Date)
                .Include(p => p.cashierFundNotes)
                .Include(p => p.cashierFundCoins)
                .OrderBy(p => p.fundDate)
                .ToList();
            
            var cashupData = le.cashierCashups
                .Where(p => p.cashupDate == model.fundingDate.Date)
                .Include(p => p.cashierCashupNotes)
                .Include(p => p.cashierCashupCoins)
                .OrderBy(p => p.cashupDate)
                .ToList();

            List<cashierFundNote> cfn = new List<cashierFundNote>();
            List<cashierCashupNote> ccn = new List<cashierCashupNote>();
            var currencyNotes = le.currencyNotes.Where(p =>p.currencyNoteTypeId == 1).ToList();
            foreach (var note in currencyNotes)
            {
                cashierFundNote fn = new cashierFundNote
                {
                    currencyNoteId = note.currencyNoteId,
                    quantity = 0,
                    total = 0
                };
                cfn.Add(fn);
                cashierCashupNote cn = new cashierCashupNote
                {
                    currencyNoteId = note.currencyNoteId,
                    quantity = 0,
                    total = 0
                };
                ccn.Add(cn);
            }

            List<cashierFundCoin> cfc = new List<cashierFundCoin>();
            List<cashierCashupCoin> ccc = new List<cashierCashupCoin>();
            var currencyCoins = le.currencyNotes.Where(p => p.currencyNoteTypeId == 2).ToList();
            foreach (var note in currencyCoins)
            {
                cashierFundCoin fc = new cashierFundCoin
                {
                    currencyNoteId = note.currencyNoteId,
                    quantity = 0,
                    total = 0
                };
                cfc.Add(fc);
                cashierCashupCoin cn = new cashierCashupCoin
                {
                    currencyNoteId = note.currencyNoteId,
                    quantity = 0,
                    total = 0
                };
                ccc.Add(cn);
            }

            if (fundsData.Count > 0)
            { model.cashierFunds = fundsData; }
            else
            { model.cashierFunds = new List<cashierFund>(); }

            if (cashupData.Count > 0)
            { model.cashierCashups = cashupData; }
            else
            { model.cashierCashups = new List<cashierCashup>(); }

            return model;
        }

        [HttpPost]
        public CashierFundingViewModel GetS(CashierFundingViewModel model)
        {
            var fundsData = le.cashierFunds
                .Where(p => p.fundDate == model.fundingDate.Date)
                .Include(p => p.cashierFundNotes)
                .Include(p => p.cashierFundCoins)
                .OrderBy(p => p.fundDate)
                .ToList();

            var cashupData = le.cashierCashups
                .Where(p => p.cashupDate == model.fundingDate.Date)
                .Include(p => p.cashierCashupNotes)
                .Include(p => p.cashierCashupCoins)
                .OrderBy(p => p.cashupDate)
                .ToList();

            //var cashRemaining = le.cashiersTills
            //    .Include(p => p.cashierRemainingNotes)
            //    .Include(p => p.cashierRemainingCoins)
            //    .FirstOrDefault();

            List<cashierFundNote> cfn = new List<cashierFundNote>();
            List<cashierCashupNote> ccn = new List<cashierCashupNote>();
            var currencyNotes = le.currencyNotes.Where(p => p.currencyNoteTypeId == 1).ToList();
            foreach (var note in currencyNotes)
            {
                cashierFundNote fn = new cashierFundNote
                {
                    currencyNoteId = note.currencyNoteId,
                    quantity = 0,
                    total = 0
                };
                cfn.Add(fn);
                cashierCashupNote cn = new cashierCashupNote
                {
                    currencyNoteId = note.currencyNoteId,
                    quantity = 0,
                    total = 0
                };
                ccn.Add(cn);
            }

            List<cashierFundCoin> cfc = new List<cashierFundCoin>();
            List<cashierCashupCoin> ccc = new List<cashierCashupCoin>();
            var currencyCoins = le.currencyNotes.Where(p => p.currencyNoteTypeId == 2).ToList();
            foreach (var note in currencyCoins)
            {
                cashierFundCoin fc = new cashierFundCoin
                {
                    currencyNoteId = note.currencyNoteId,
                    quantity = 0,
                    total = 0
                };
                cfc.Add(fc);
                cashierCashupCoin cn = new cashierCashupCoin
                {
                    currencyNoteId = note.currencyNoteId,
                    quantity = 0,
                    total = 0
                };
                ccc.Add(cn);
            }

            if (fundsData.Count > 0)
            {
                model.cashierFunds = fundsData;
                //model.cashierTills = 

                foreach (var fund in fundsData)
                {
                    var cashierTil = le.cashiersTills
                    .Include(p => p.cashierRemainingNotes)
                    .Include(p => p.cashierRemainingCoins)
                    .FirstOrDefault(p => p.cashiersTillID == fund.cashierTillId);

                    model.cashierTills.Add(cashierTil);
                }
            }
            else
            { model.cashierFunds = new List<cashierFund>(); }

            if (cashupData.Count > 0)
            { model.cashierCashups = cashupData; }
            else
            { model.cashierCashups = new List<cashierCashup>(); }

            return model;
        }

        [HttpGet]
        public CashierFundingViewModel GetNew()
        {
            return new CashierFundingViewModel
            {
                cashierFunds = new List<cashierFund>(),
                cashierCashups = new List<cashierCashup>()
            };
        }

        [AuthorizationFilter()]
        [HttpPost]
        public CashierFundingViewModel Post(CashierFundingViewModel input)
        {
            if (input == null) return null;
            foreach (var fund in input.cashierFunds)
            {
                if (fund.cashierFundId > 0)
                {
                    /*var tobeSaved = le.cashierFunds.FirstOrDefault(p => p.cashierFundId == fund.cashierFundId);
                    var cashierTill = le.cashiersTills
                        .Include(p => p.cashierRemainingNotes)
                        .Include(p => p.cashierRemainingCoins)
                        .FirstOrDefault(p => p.cashiersTillID == fund.cashierTillId);
                    populateFundField(tobeSaved, fund, cashierTill);
                    cashierTill.currentBalance += tobeSaved.transferAmount;*/
                }
                else
                {
                    cashierFund tobeSaved = new cashierFund();
                    var cashierTill = le.cashiersTills
                        .Include(p => p.cashierRemainingNotes)
                        .Include(p => p.cashierRemainingCoins)
                        .FirstOrDefault(p => p.cashiersTillID == fund.cashierTillId);
                    populateFundField(tobeSaved, fund, cashierTill);
                    le.cashierFunds.Add(tobeSaved);
                    cashierTill.currentBalance += tobeSaved.transferAmount;
                }
            }
            //Save Cashier Cashup
            foreach (var cashup in input.cashierCashups)
            {
                if (cashup.cashierCashupId > 0)
                {
                    /*var tobeSaved = le.cashierCashups.FirstOrDefault(p => p.cashierCashupId == cashup.cashierCashupId);
                    var cashierTill = le.cashiersTills
                        .Include(p => p.cashierRemainingNotes)
                        .Include(p => p.cashierRemainingCoins)
                        .FirstOrDefault(p => p.cashiersTillID == cashup.cashierTillId);
                    populateCashupField(tobeSaved, cashup, cashierTill);
                    cashierTill.currentBalance -= tobeSaved.transferAmount;*/
                }
                else
                {
                    cashierCashup tobeSaved = new cashierCashup();
                    var cashierTill = le.cashiersTills
                        .Include(p => p.cashierRemainingNotes)
                        .Include(p => p.cashierRemainingCoins)
                        .FirstOrDefault(p => p.cashiersTillID == cashup.cashierTillId);
                    populateCashupField(tobeSaved, cashup, cashierTill);
                    le.cashierCashups.Add(tobeSaved);
                    cashierTill.currentBalance -= tobeSaved.transferAmount;
                }
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                //throw new ApplicationException(x.InnerException.ToString());
                throw new ApplicationException("Error saving to server.");
            }
            return input;
        }

        private void populateFundField(cashierFund tobeSaved, cashierFund input, cashiersTill till)
        {
            double totalFundedNotes = 0;
            double totalFundedCoins = 0;

            tobeSaved.cashierTillId = input.cashierTillId;
            
            tobeSaved.fundDate = DateTime.Today;
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

            foreach (var note in input.cashierFundNotes)
            {
                if (note.cashierFundNoteId > 0)
                {
                }
                else
                {
                    //retrieve notes and use their value to calculate total
                    var noteCurrencies = le.currencyNotes.Where(p => p.currencyNoteTypeId == 1).ToList();
                    var remainingOfCurrency = till.cashierRemainingNotes
                        .FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);

                    if (remainingOfCurrency == null)
                    {
                        remainingOfCurrency = new cashierRemainingNote();
                        till.cashierRemainingNotes.Add(remainingOfCurrency);
                    }
                    
                    cashierFundNote noteToBeSaved = new cashierFundNote();
                    populateNoteFields(noteToBeSaved,note, noteCurrencies, ref totalFundedNotes, remainingOfCurrency);
                    tobeSaved.cashierFundNotes.Add(noteToBeSaved);

                }
            }
            foreach (var coin in input.cashierFundCoins)
            {
                if (coin.cashierFundNoteId > 0)
                {
                }
                else
                {
                    //retrieve coins and use their value to calculate total
                    var noteCurrencies = le.currencyNotes.Where(p => p.currencyNoteTypeId == 2).ToList();

                    var remainingOfCurrency = till.cashierRemainingCoins
                        .FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);

                    if (remainingOfCurrency == null)
                    {
                        remainingOfCurrency = new cashierRemainingCoin();
                        till.cashierRemainingCoins.Add(remainingOfCurrency);
                    }

                    cashierFundCoin coinToBeSaved = new cashierFundCoin();
                    populateCoinFields(coinToBeSaved, coin, noteCurrencies, ref totalFundedCoins, remainingOfCurrency);
                    tobeSaved.cashierFundCoins.Add(coinToBeSaved);
                }
            }
            tobeSaved.transferAmount = totalFundedNotes + totalFundedCoins;
        }

        private void populateCashupField(cashierCashup tobeSaved, cashierCashup input, cashiersTill till)
        {
            double totalCashupNotes = 0;
            double totalCashupCoins = 0;

            tobeSaved.cashierTillId = input.cashierTillId;
            tobeSaved.transferAmount = input.transferAmount;
            tobeSaved.cashupDate = DateTime.Today;
            if (tobeSaved.cashierCashupId > 0)
            {
                tobeSaved.modified = DateTime.Now;
                tobeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
            else
            {
                tobeSaved.created = DateTime.Now;
                tobeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }


            foreach (var note in input.cashierCashupNotes)
            {
                if (note.cashierCashupNoteId > 0)
                {
                }
                else
                {
                    //retrieve notes and use their value to calculate total
                    var noteCurrencies = le.currencyNotes.Where(p => p.currencyNoteTypeId == 1).ToList();
                    var remainingOfCurrency = till.cashierRemainingNotes
                        .FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);

                    

                    cashierCashupNote noteToBeSaved = new cashierCashupNote();
                    populateCashupNoteFields(noteToBeSaved, note, noteCurrencies, ref totalCashupNotes, remainingOfCurrency);
                    tobeSaved.cashierCashupNotes.Add(noteToBeSaved);

                    //if (remainingOfCurrency != null)
                    //{
                    //    remainingOfCurrency = new cashierRemainingNote();
                    //    till.cashierRemainingNotes.Add(remainingOfCurrency);
                    //}

                }
            }
            foreach (var coin in input.cashierCashupCoins)
            {
                if (coin.cashierCashupNoteId > 0)
                {
                }
                else
                {
                    //retrieve coins and use their value to calculate total
                    var noteCurrencies = le.currencyNotes.Where(p => p.currencyNoteTypeId == 2).ToList();

                    var remainingOfCurrency = till.cashierRemainingCoins
                        .FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);

                    //if (remainingOfCurrency != null)
                    //{
                    //    remainingOfCurrency = new cashierRemainingCoin();
                    //    till.cashierRemainingCoins.Add(remainingOfCurrency);
                    //}

                    cashierCashupCoin coinToBeSaved = new cashierCashupCoin();
                    populateCashupCoinFields(coinToBeSaved, coin, noteCurrencies, ref totalCashupCoins, remainingOfCurrency);
                    tobeSaved.cashierCashupCoins.Add(coinToBeSaved);
                }
            }
            tobeSaved.transferAmount = totalCashupNotes + totalCashupCoins;
            till.currentBalance -= totalCashupNotes + totalCashupCoins;

        }

        private void populateNoteFields(cashierFundNote tobeSaved, cashierFundNote input,List<currencyNote> notes,
            ref double totalNotes, cashierRemainingNote remainingOfNote)
        {
            double currNoteValue = notes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId).value;
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantity;
            tobeSaved.total = input.quantity * currNoteValue;
            totalNotes += tobeSaved.total;

            remainingOfNote.currencyNoteId = input.currencyNoteId;
            remainingOfNote.quantity += input.quantity;
            remainingOfNote.total += input.quantity*currNoteValue;
        }

        private void populateCoinFields(cashierFundCoin tobeSaved, cashierFundCoin input, List<currencyNote> notes,
            ref double totalCoins, cashierRemainingCoin remainingOfCoin)
        {
            double currNoteValue = notes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId).value;
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantity;
            tobeSaved.total = input.quantity * currNoteValue;
            totalCoins += tobeSaved.total;

            remainingOfCoin.currencyNoteId = input.currencyNoteId;
            remainingOfCoin.quantity += input.quantity;
            remainingOfCoin.total += input.quantity * currNoteValue;
        }


        private void populateCashupNoteFields(cashierCashupNote tobeSaved, cashierCashupNote input, List<currencyNote> notes,
            ref double totalNotes, cashierRemainingNote remainingOfNote)
        {
            double currNoteValue = notes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId).value;
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantity;
            tobeSaved.total = input.quantity * currNoteValue;
            totalNotes += tobeSaved.total;

            //remainingOfNote.currencyNoteId = input.currencyNoteId;
            remainingOfNote.quantity -= input.quantity;
            remainingOfNote.total -= input.quantity * currNoteValue;
        }

        private void populateCashupCoinFields(cashierCashupCoin tobeSaved, cashierCashupCoin input, List<currencyNote> notes,
            ref double totalCoins, cashierRemainingCoin remainingOfCoin)
        {
            double currNoteValue = notes.FirstOrDefault(p => p.currencyNoteId == input.currencyNoteId).value;
            tobeSaved.currencyNoteId = input.currencyNoteId;
            tobeSaved.quantity = input.quantity;
            tobeSaved.total = input.quantity * currNoteValue;
            totalCoins += tobeSaved.total;

            //remainingOfCoin.currencyNoteId = input.currencyNoteId;
            remainingOfCoin.quantity -= input.quantity;
            remainingOfCoin.total -= input.quantity * currNoteValue;
        }

        //private bool validateInpit()
        //{
        //}
    }
}
