using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using coreErpApi.Controllers.Models.Deposit;
using System.Web.Http.Cors;
using coreData.Constants;
using coreERP.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace coreErpApi.Controllers.Controllers.Deposits
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class DepositController : ApiController
    {
        IcoreLoansEntities le;
        Icore_dbEntities ctx;

        ErrorMessages error = new ErrorMessages();

        public DepositController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;

            ctx = new core_dbEntities();
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        public DepositController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/depositType
        [HttpGet]
        public async Task<IEnumerable<deposit>> GetRunningDeposit()
        {
            return await le.deposits
                .Where(p => p.principalBalance > 1 || p.interestBalance > 1)
                .OrderBy(p => p.depositNo)
                .ToListAsync();
        }

        // GET: api/depositType
        public async Task<IEnumerable<deposit>> GetClientDeposit(int id)
        {
            return await le.deposits
                .Where(p => p.clientID == id && p.principalBalance > 0)
                .OrderBy(p => p.depositID)
                .ToListAsync();
        }

        [HttpGet]
        public IEnumerable<DepositAccountViewModel> GetAllClientDepositAccounts(int id)
        {
            var data = le.deposits
                .Where(p => p.clientID == id)
                .Select(p => new DepositAccountViewModel
                {
                    clientId = p.clientID,
                    depositId = p.depositID,
                    depositAccountNo = p.depositNo
                })
                .OrderBy(p => p.depositAccountNo)
                .ToList();
            return data;
        }

        // GET: api/depositType/5
        [HttpGet]
        public depositType Get(int id)
        {
            return le.depositTypes
                .FirstOrDefault(p => p.depositTypeID == id);
        }

        // GET: api/depositType/5
        [HttpGet]
        public deposit GetDeposit(int id)
        {
            var data = le.deposits
                .Include(p => p.client)
                .FirstOrDefault(p => p.depositID == id);
            
            return data;
        }

        // GET: api/depositType/5
        [HttpGet]
        public DepositModel GetNewDeposit()
        {
            return new DepositModel
            {
                depositNextOfKins = new List<depositNextOfKin>(),
                depositSignitoriesModel = new List<DepositSignitoryModel>()
            };
        }

        // GET: api/deposit
        [HttpGet]
        public IEnumerable<deposit> GetClientDeposits(int id)
        {
            var data = le.deposits.Where(p => p.clientID == id).ToList();
            return data;
        }

        //Reserved for Trust Line only
        [HttpPost]
        public deposit PostDeposit(DepositViewModel input)
        {
            if (input == null) return null;

            //Check if cashier till is defined. but skip if its TTL investment application
            if(!(ctx.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("ttl") 
                && input.clientInvestmentReceiptDetailId > 0))
            cashierCheck(input.firstDepositDate);

            deposit toBeSaved = new deposit();
            string creator = "";
            if (input.clientInvestmentReceiptDetailId >0)
            {
                var receiptDetail = le.clientInvestmentReceiptDetails
                    .FirstOrDefault(p => p.clientInvestmentReceiptDetailId == input.clientInvestmentReceiptDetailId);
                if (receiptDetail != null)
                {
                    receiptDetail.invested = true;
                    receiptDetail.investedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
                    receiptDetail.investedDate = DateTime.Now;
                    creator = receiptDetail.receivedBy;
                }
            }
            else
            {
                creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
            populateAdditionalFields(toBeSaved, input, creator);
            le.deposits.Add(toBeSaved);

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            if (le.configs.First().transactionalBankingEnabled && toBeSaved.depositAdditionals.First().modeOfPaymentID == 1)
            {
                var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                var cashierTill = le.cashiersTills
                    .Include(p => p.cashierRemainingNotes)
                    .Include(p => p.cashierRemainingCoins)
                    .FirstOrDefault(p => p.userName == currentCashier);

                cashierTransactionReceipt transacToBesaved = new cashierTransactionReceipt
                {
                    transactionId = toBeSaved.depositAdditionals.First().depositAdditionalID
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

            return toBeSaved;
        }

        //Reserved for Trust Line only
        [HttpPost]
        public deposit PostNewDeposit(DepositModel input)
        {
            if (input == null) return null;

            deposit toBeSaved = new deposit();
            populateDepositFields(toBeSaved, input);
            le.deposits.Add(toBeSaved);

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return toBeSaved;
        }

        [HttpPost]
        public KendoResponse Post(depositType input)
        {
            le.depositTypes
                .Add(input);
            le.SaveChanges();

            return new KendoResponse { Data = new depositType[] { input } };
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "depositTypeName";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<depositType>(req, parameters);

            var query = le.depositTypes.AsQueryable();
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray());
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [HttpPut]
        // PUT: api/depositType/5
        public KendoResponse Put([FromBody]depositType value)
        {
            var toBeUpdated = le.depositTypes.First(p => p.depositTypeID == value.depositTypeID);

            toBeUpdated.depositTypeID = value.depositTypeID;
            toBeUpdated.depositTypeName = value.depositTypeName;
            toBeUpdated.interestRate = value.interestRate;
            toBeUpdated.defaultPeriod = value.defaultPeriod;
            toBeUpdated.allowsInterestWithdrawal = value.allowsInterestWithdrawal;
            toBeUpdated.allowsPrincipalWithdrawal = value.allowsPrincipalWithdrawal;
            toBeUpdated.vaultAccountID = value.vaultAccountID;
            toBeUpdated.accountsPayableAccountID = value.accountsPayableAccountID;
            toBeUpdated.fxUnrealizedGainLossAccountID = value.fxUnrealizedGainLossAccountID;
            toBeUpdated.fxRealizedGainLossAccountID = value.fxRealizedGainLossAccountID;
            toBeUpdated.interestCalculationScheduleID = value.interestCalculationScheduleID;
            toBeUpdated.chargesIncomeAccountID = value.chargesIncomeAccountID;
            toBeUpdated.interestPayableAccountID = value.interestPayableAccountID;

            le.SaveChanges();

            return new KendoResponse { Data = new depositType[] { toBeUpdated } };
        }

        [HttpDelete]
        // DELETE: api/depositType/5
        public void Delete([FromBody]depositType value)
        {
            var forDelete = le.depositTypes.FirstOrDefault(p => p.depositTypeID == value.depositTypeID);
            if (forDelete != null)
            {
                le.depositTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }

        private void populateAdditionalFields(deposit tobeSaved, DepositViewModel input, string receivedBy)
        {
            var comp = ctx.comp_prof.First();
            tobeSaved.clientID = input.clientID;
            tobeSaved.interestExpected = input.interestExpected;
            tobeSaved.depositTypeID = input.depositTypeID;
            tobeSaved.amountInvested = input.amountInvested;
            tobeSaved.localAmount = input.localAmount;
            tobeSaved.interestAccumulated = 0;
            tobeSaved.interestBalance = 0;
            tobeSaved.principalBalance = input.amountInvested;
            tobeSaved.interestRate = input.interestRate;
            tobeSaved.annualInterestRate = input.annualInterestRate;
            tobeSaved.firstDepositDate = input.firstDepositDate;
            tobeSaved.period = input.period;
            tobeSaved.maturityDate = input.maturityDate;
            tobeSaved.autoRollover = input.autoRollover;
            tobeSaved.interestMethod = false;
            tobeSaved.interestRepaymentModeID = input.interestRepaymentModeID;
            tobeSaved.principalRepaymentModeID = input.principalRepaymentModeID;
            tobeSaved.principalAuthorized = 0;
            tobeSaved.interestAuthorized = input.interestAuthorized;
            tobeSaved.fxRate = input.fxRate;
            tobeSaved.currencyID = 0;
            tobeSaved.localAmount = input.amountInvested;
            tobeSaved.staffID = input.staffID;
            tobeSaved.creator = receivedBy;
            tobeSaved.creation_date = DateTime.Now;
            input.modern = false;
            if (input.agentId != null && input.agentId == 0)
            {
                tobeSaved.agentId = input.agentId;
            }
            if (comp.comp_name.ToLower().Contains("eclipse"))
            {
                var depTyp = le.depositTypes.FirstOrDefault(p => p.depositTypeID == input.depositTypeID);
                var cln = le.clients
                    .Include(p => p.branch)
                    .FirstOrDefault(p => p.clientID == input.clientID);
                IDGenerator idGen = new IDGenerator();
                tobeSaved.depositNo = idGen.NewDepositNumber(cln.branch.branchID,
                        input.clientID, tobeSaved.depositID,
                        depTyp.depositTypeName.Substring(0, 2).ToUpper());
            }
            else
            {
                tobeSaved.depositNo = IDGenerator.nextClientDepositNumber(le, input.clientID);
            }
            switch (tobeSaved.period)
            {
                case 1:
                    tobeSaved.depositPeriodInDays = 30;
                    break;
                case 2:
                    tobeSaved.depositPeriodInDays = 60;
                    break;
                case 3:
                    tobeSaved.depositPeriodInDays = 91;
                    break;
                case 4:
                    tobeSaved.depositPeriodInDays = 121;
                    break;
                case 5:
                    tobeSaved.depositPeriodInDays = 151;
                    break;
                case 6:
                    tobeSaved.depositPeriodInDays = 182;
                    break;
                case 7:
                    tobeSaved.depositPeriodInDays = 212;
                    break;
                case 8:
                    tobeSaved.depositPeriodInDays = 242;
                    break;
                case 9:
                    tobeSaved.depositPeriodInDays = 274;
                    break;
                case 10:
                    tobeSaved.depositPeriodInDays = 304;
                    break;
                case 11:
                    tobeSaved.depositPeriodInDays = 334;
                    break;
                case 12:
                    tobeSaved.depositPeriodInDays = 365;
                    break;
            }

            foreach (var add in input.depositAdditionals)
            {
                if (add.depositAdditionalID < 1)
                {
                    depositAdditional da = new depositAdditional
                    {
                        checkNo = add.checkNo,
                        depositAmount = add.depositAmount,
                        bankID = add.bankID,
                        interestBalance = 0,
                        depositDate = input.firstDepositDate,
                        creation_date = DateTime.Now,
                        creator = receivedBy,
                        principalBalance = add.depositAmount,
                        modeOfPaymentID = add.modeOfPaymentID,
                        posted = false,
                        naration = add.naration,
                        fxRate = add.fxRate
                    };
                    tobeSaved.depositAdditionals.Add(da);
                }
            }

            CalculateSchedule(tobeSaved);
        }


        private void populateDepositFields(deposit tobeSaved, DepositModel input)
        {
            var comp = ctx.comp_prof.First();
            tobeSaved.clientID = input.clientID;
            tobeSaved.interestExpected = 0;
            tobeSaved.depositTypeID = input.depositTypeID;
            tobeSaved.amountInvested = 0;
            tobeSaved.localAmount = 0;
            tobeSaved.interestAccumulated = 0;
            tobeSaved.interestBalance = 0;
            tobeSaved.principalBalance = 0;
            tobeSaved.interestRate = input.annualInterestRate/12;
            tobeSaved.annualInterestRate = input.annualInterestRate;
            tobeSaved.firstDepositDate = DateTime.Now;
            tobeSaved.period = input.period;
            tobeSaved.maturityDate = DateTime.Today.Date.AddDays(input.depositPeriodInDays);
            tobeSaved.autoRollover = false;
            tobeSaved.interestMethod = false;
            tobeSaved.interestRepaymentModeID = -1;
            tobeSaved.principalRepaymentModeID = -1;
            tobeSaved.principalAuthorized = 0;
            tobeSaved.interestAuthorized = 0;
            tobeSaved.fxRate = 1;
            tobeSaved.currencyID = 1;
            tobeSaved.localAmount = 0;
            tobeSaved.staffID = input.staffID;
            tobeSaved.agentId = input.agentId;
            tobeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
            tobeSaved.creation_date = DateTime.Now;
            input.modern = false;
            tobeSaved.depositPeriodInDays = input.depositPeriodInDays;
            if (comp.comp_name.ToLower().Contains("eclipse"))
            {
                var depTyp = le.depositTypes.FirstOrDefault(p => p.depositTypeID == input.depositTypeID);
                var cln = le.clients
                    .Include(p => p.branch)
                    .FirstOrDefault(p => p.clientID == input.clientID);
                IDGenerator idGen = new IDGenerator();
                tobeSaved.depositNo = idGen.NewDepositNumber(cln.branch.branchID,
                        input.clientID, tobeSaved.depositID,
                        depTyp.depositTypeName.Substring(0, 2).ToUpper());
            }
            else
            {
                tobeSaved.depositNo = IDGenerator.nextClientDepositNumber(le, input.clientID);
            }

            foreach (var nok in input.depositNextOfKins)
            {
                if (nok.depositNextOfKinId > 0)
                {
                    var nokToBeUpdated = le.depositNextOfKins.FirstOrDefault(p => p.depositNextOfKinId == nok.depositNextOfKinId);
                    populateNextOfKinFields(nokToBeUpdated, nok);
                }
                else
                {
                    var nokToBeSaved = new depositNextOfKin();
                    populateNextOfKinFields(nokToBeSaved, nok);
                    tobeSaved.depositNextOfKins.Add(nokToBeSaved);
                }
            }

            foreach (var signature in input.depositSignitoriesModel)
            {
                if (signature.depositSignatoryID > 0)
                {
                    var sigfToBeUpdated = le.depositSignatories.FirstOrDefault(p => p.depositSignatoryID == signature.depositSignatoryID);
                    populateSignatoryFields(sigfToBeUpdated, signature);
                }
                else
                {
                    var sigToBeSaved = new depositSignatory();
                    populateSignatoryFields(sigToBeSaved, signature);
                    tobeSaved.depositSignatories.Add(sigToBeSaved);
                }
            }
        }

        private void CalculateSchedule(deposit tobeSaved)
        {
            if (tobeSaved.depositSchedules.Count == 0 && tobeSaved.autoRollover)
            {
                List<DateTime> listInt = new List<DateTime>();
                List<DateTime> listPrinc = new List<DateTime>();
                List<DateTime> listAll = new List<DateTime>();

                DateTime date = tobeSaved.firstDepositDate;
                int i = 1;
                var totalInt = tobeSaved.amountInvested * (tobeSaved.period) * (tobeSaved.interestRate) / 100.0;
                var intererst = 0.0;
                var princ = 0.0;
                var interestRepaymentMode = le.depositRepaymentModes
                    .FirstOrDefault(p => p.repaymentModeDays == tobeSaved.interestRepaymentModeID);
                var principalRepaymentMode = le.depositRepaymentModes
                    .FirstOrDefault(p => p.repaymentModeDays == tobeSaved.interestRepaymentModeID);

                while (date < tobeSaved.maturityDate)
                {
                    date = date.AddMonths(1);
                    if (date >= tobeSaved.maturityDate) break;
                    if ((interestRepaymentMode.repaymentModeDays == 30)
                        || (interestRepaymentMode.repaymentModeDays == 90 && i % 3 == 0)
                        || (interestRepaymentMode.repaymentModeDays == 180 && i % 6 == 0)
                        )
                    {
                        listInt.Add(date);
                        if (listAll.Contains(date) == false) listAll.Add(date);
                    }
                    if ((principalRepaymentMode.repaymentModeDays == 30)
                        || (principalRepaymentMode.repaymentModeDays == 90 && i % 3 == 0)
                        || (principalRepaymentMode.repaymentModeDays == 180 && i % 6 == 0)
                        )
                    {
                        listPrinc.Add(date);
                        if (listAll.Contains(date) == false) listAll.Add(date);
                    }
                    i += 1;
                }
                listPrinc.Add(tobeSaved.maturityDate.Value);
                listInt.Add(tobeSaved.maturityDate.Value);
                listAll.Add(tobeSaved.maturityDate.Value);

                tobeSaved.modern = true;
                foreach (DateTime date2 in listAll)
                {
                    if (listPrinc.Contains(date2))
                    {
                        princ = tobeSaved.amountInvested / listPrinc.Count;
                    }
                    if (listInt.Contains(date2))
                    {
                        intererst = totalInt / listInt.Count;
                    }
                    tobeSaved.depositSchedules.Add(new coreLogic.depositSchedule
                    {
                        interestPayment = intererst,
                        principalPayment = princ,
                        repaymentDate = date2,
                        authorized = false,
                        expensed = false,
                        temp = false
                    });
                }
            }
        }




        //For Transactional Banking
        private void populateTransactionAdditionalFields(cashierTransactionReceipt tobeSaved, DepositViewModel input, cashiersTill till)
        {
            var currentCashier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashierTill = le.cashiersTills.FirstOrDefault(p => p.userName == currentCashier);
            var add = input.depositAdditionals.First();
            tobeSaved.receiptDate = input.firstDepositDate;
            tobeSaved.transactionTypeId = 7;
            tobeSaved.totalReceiptAmount = input.amountInvested;//TODO change totalReceiptAmount to withdrawalAmount
            tobeSaved.cashierTillId = cashierTill.cashiersTillID;
            tobeSaved.balanceBD = cashierTill.currentBalance;
            tobeSaved.balanceCD = cashierTill.currentBalance + add.depositAmount;
            tobeSaved.creator = currentCashier;
            tobeSaved.created = DateTime.Now;
            //Save Deposit Notes
            foreach (var note in input.depositNotes)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierNote = till.cashierRemainingNotes.FirstOrDefault(p => p.currencyNoteId == note.currencyNoteId);
                populateCurrencyNoteFields(ntCurrency, note, ref cashierNote);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Save Deposit Coins
            foreach (var coin in input.depositCoins)
            {
                cashierTransactionReceiptCurrency ntCurrency = new cashierTransactionReceiptCurrency();
                var cashierCoin = till.cashierRemainingCoins.FirstOrDefault(p => p.currencyNoteId == coin.currencyNoteId);
                populateCurrencyCoinFields(ntCurrency, coin, ref cashierCoin);
                tobeSaved.cashierTransactionReceiptCurrencies.Add(ntCurrency);
            }
            //Increase Cashier's balance
            cashierTill.currentBalance += tobeSaved.totalReceiptAmount;
        }

        private void populateNextOfKinFields(depositNextOfKin tobeSaved, depositNextOfKin input)
        {
            tobeSaved.otherNames = input.otherNames;
            tobeSaved.surName = input.surName;
            tobeSaved.dateOfBirth = input.dateOfBirth;
            tobeSaved.relationshipTypeId = input.relationshipTypeId;
            tobeSaved.idTypeId = input.idTypeId;
            tobeSaved.idNumber = input.idNumber;
            tobeSaved.phoneNumber = input.phoneNumber;
            tobeSaved.percentageAllocated = input.percentageAllocated;
            if (tobeSaved.depositNextOfKinId > 0)
            {
                tobeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                tobeSaved.modified = DateTime.Now;
            }
            else
            {
                tobeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                tobeSaved.created = DateTime.Now;
            }           
         }

        private void populateSignatoryFields(depositSignatory tobeSaved, DepositSignitoryModel input)
        {
            tobeSaved.fullName = input.signatoryName;
            if (input.signatures.FirstOrDefault() != null)
            {
                var sig = input.signatures.FirstOrDefault();
                if (sig.imageId > 0)
                {
                    var imgToBeUpdated = le.images.FirstOrDefault(p => p.imageID == sig.imageId);
                    populateImage(imgToBeUpdated, sig);
                }
                else
                {
                    image imgToBeSaved = new image();
                    populateImage(imgToBeSaved, sig);
                    tobeSaved.image = imgToBeSaved;
                }
            }

            
        }

        private void populateImage(image toBeSaved, DepositImage input)
        {
            toBeSaved.description = input.fileName;
            toBeSaved.image1 = Convert.FromBase64String(input.image);
            toBeSaved.content_type = input.mimeType;
        }

        private void populateCurrencyNoteFields(cashierTransactionReceiptCurrency tobeSaved, depositNote input, ref cashierRemainingNote noteToBeUpdated)
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

        private void populateCurrencyCoinFields(cashierTransactionReceiptCurrency tobeSaved, depositCoin input, ref cashierRemainingCoin coinToBeUpdated)
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
            if(le.configs.FirstOrDefault().transactionalBankingEnabled)
            ctc.CheckForcashierFunding(input);
        }

    }
}
