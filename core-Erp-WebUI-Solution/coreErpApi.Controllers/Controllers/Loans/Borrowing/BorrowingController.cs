//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Validation;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Text;
//using coreData.ErrorLog;
//using coreData.Constants;
//using System.Net.Mail;
//using coreLogic.HelperClasses;
//using coreLogic.Models.Email;


//namespace coreErpApi.Controllers.Controllers.Loans.Borrowing
//{
//    [AuthorizationFilter()]
//    public class BorrowingController : ApiController
//    {
//        //Declare a LoansEntities Interface variable 
//        IcoreLoansEntities le;
//        Icore_dbEntities ctx;


//        private ErrorMessages error = new ErrorMessages();
//        private StringBuilder errorMessage = new StringBuilder("");

//        //call a constructor to instialize the Interface context 
//        public BorrowingController()
//        {
//            le = new coreLoansEntities();
//            ctx = new core_dbEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//            ctx.Configuration.LazyLoadingEnabled = false;
//            ctx.Configuration.ProxyCreationEnabled = false;
//        }

//        //A constructor with a parameter for Mocking DB
//        public BorrowingController(IcoreLoansEntities lent, Icore_dbEntities cent)
//        {
//            le = lent;
//            ctx = cent;
//        }

//        // GET: api/Borrowings
//        public IEnumerable<borrowing> Get()
//        {
//            return le.borrowings
//                .Include(p => p.borrowingDisbursements)
//                .Include(p => p.borrowingRepaymentSchedules)
//                .Include(p => p.borrowingRepayments)
//                .Include(p => p.borrowingDocuments)
//                .OrderBy(p => p.borrowingNo)
//                .ToList();
//        }

//        // GET: api/Borrowings
//        [HttpGet]
//        public IEnumerable<borrowing> GetClientApprovedBrws(long id)
//        {
//            return le.borrowings
//                .Where(p => p.clientId == id && p.amountApproved > 0 && !p.closed)
//                .Include(p => p.borrowingDisbursements)
//                .Include(p => p.borrowingRepaymentSchedules)
//                .Include(p => p.borrowingRepayments)
//                .Include(p => p.borrowingDocuments)
//                .OrderBy(p => p.borrowingNo)
//                .ToList();
//        }

//        [HttpGet]
//        public IEnumerable<borrowing> GetClientDisbursedBrws(long id)
//        {
//            return le.borrowings
//                .Where(p => p.clientId == id && p.amountDisbursed > 0 && p.disbursedDate != null && !p.closed)
//                .Include(p => p.borrowingDisbursements)
//                .Include(p => p.borrowingRepaymentSchedules)
//                .Include(p => p.borrowingRepayments)
//                .Include(p => p.borrowingDocuments)
//                .OrderBy(p => p.borrowingNo)
//                .ToList();
//        }

//        // GET: api/Borrowings
//        [HttpGet]
//        public IEnumerable<borrowing> GetClientUnapprovedBrws(long clientId)
//        {
//            return le.borrowings
//                .Where(p => p.clientId == clientId && p.amountApproved < 1 && !p.closed)
//                .Include(p => p.borrowingDisbursements)
//                .Include(p => p.borrowingRepaymentSchedules)
//                .Include(p => p.borrowingRepayments)
//                .Include(p => p.borrowingDocuments)
//                .OrderBy(p => p.borrowingNo)
//                .ToList();
//        }

//        public borrowing Get(int id)
//        {
//            borrowing value = le.borrowings
//                .Include(p => p.borrowingDisbursements)
//                .Include(p => p.borrowingRepaymentSchedules)
//                .Include(p => p.borrowingRepayments)
//                .Include(p => p.borrowingDocuments)
//                .FirstOrDefault(p => p.borrowingId == id);

//            if (value == null)
//            {
//                value = new borrowing();
//            }
//            return value;
//        }


//        [HttpPost]
//        public borrowing Post(borrowing input)
//        {
//            //If validation fails throw exception
//            if (!ValidateBorrowingFields(input))
//            {
//                throw new ApplicationException(errorMessage.ToString());
//            }

//            borrowing inputToBeSaved = new borrowing();
//            populateBorrowingFields(inputToBeSaved, input);
//            le.borrowings.Add(inputToBeSaved);

//            //Save all borrowingFees for borrowing
//            foreach (var borrowingFee in inputToBeSaved.borrowingFees)
//            {
//                borrowingFee borrowingFeeToBeSaved = new borrowingFee();
//                populateBorrowingFeeFields(borrowingFeeToBeSaved, borrowingFee);
//                inputToBeSaved.borrowingFees.Add(borrowingFeeToBeSaved);
//            }
            
//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                //If saving fails, Log the the Exception and display message to user.
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return inputToBeSaved;
//        }

//        [HttpPut]
//        public borrowing ApproveBorrowing(borrowing input)
//        {
//            if (input.amountApproved < 1)
//            {
//                throw new ApplicationException(error.BorrowingAmountApproveError);
//            }

//            borrowing inputToUpdate = le.borrowings
//                .FirstOrDefault(p => p.borrowingId == input.borrowingId);

//            if (inputToUpdate != null)
//            {
//                inputToUpdate.amountApproved = input.amountApproved;
//                inputToUpdate.approvalComments = input.approvalComments;
//                inputToUpdate.aprovalDate = DateTime.Now;
//                inputToUpdate.appliedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            }

//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                //If saving fails, Log the the Exception and display message to user.
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return inputToUpdate;
//        }

//        [HttpPut]
//        public borrowing DisburseBorrowing(borrowing input)
//        {
//            if (input.amountApproved < 1 || input.aprovalDate == null)
//            {
//                throw new ApplicationException(error.BorrowingAmountApproveError);
//            }

//            jnl_batch jnlBatchToBePosted;

//            var inputToUpdate = le.borrowings
//                .Include(p => p.borrowingType)
//                .Include(p => p.client)
//                .FirstOrDefault(p => p.borrowingId == input.borrowingId);

//            //If borrowing exist update then disburse
//            if (inputToUpdate != null)
//            {
//                inputToUpdate.amountDisbursed = input.amountDisbursed;
//                inputToUpdate.disbursedDate = input.disbursedDate;
//                inputToUpdate.disbursedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                inputToUpdate.balance = input.amountDisbursed + (input.amountDisbursed * (inputToUpdate.interestRate/ 100.0)*12*(30/365.0));

//                //Add borrowing disbursement to borrowing
//                var disbursement = input.borrowingDisbursements.FirstOrDefault();
//                borrowingDisbursement brwDis = new borrowingDisbursement
//                {
//                    borrowingId = input.borrowingId,
//                    dateDisbursed = input.disbursedDate ?? DateTime.Now,
//                    amountDisbursed = input.amountDisbursed,
//                    modeOfPaymentId = disbursement.modeOfPaymentId,
//                    bankId = disbursement.bankId,
//                    chequeNumber = disbursement.chequeNumber
//                };
//                inputToUpdate.borrowingDisbursements.Add(brwDis);

//                //retrieve  information to post
//                var ba = ctx.bank_accts.FirstOrDefault(p => p.bank_acct_id == brwDis.bankId);
//                int cmpCurrency = ctx.comp_prof.First().currency_id.Value;
//                var user = LoginHelper.getCurrentUser(new coreSecurityEntities());

//                //post disbursement Amount
//                jnlBatchToBePosted = (new JournalExtensions()).Post("BRW", inputToUpdate.borrowingType.accountsPayableAccountId, ba.gl_acct_id,
//                    brwDis.amountDisbursed, "Borrowing Disbursement", cmpCurrency, brwDis.dateDisbursed,
//                            inputToUpdate.borrowingNo, ctx, user,null);
//                ctx.jnl_batch.Add(jnlBatchToBePosted);

//                //post Application Fee
//                jnlBatchToBePosted = (new JournalExtensions()).Post("BRW", inputToUpdate.borrowingType.commissionAndFeesAccountId, ba.gl_acct_id,
//                    inputToUpdate.applicationFee, "Application Fee", cmpCurrency, brwDis.dateDisbursed,
//                            inputToUpdate.borrowingNo, ctx, user, null);
//                ctx.jnl_batch.Add(jnlBatchToBePosted);

//                //Save borrowing Application Fee
//                borrowingFee brwAppFee = new borrowingFee
//                {
//                    borrowingId = inputToUpdate.borrowingId,
//                    feeDate = DateTime.Now,
//                    feeAmount = inputToUpdate.applicationFee,
//                    feeTypeId = le.loanFeeTypes.FirstOrDefault(p => p.feeTypeName.ToLower().Contains("application fee")).feeTypeID,
//                    creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
//                    created = DateTime.Now
//                };
//                inputToUpdate.borrowingFees.Add(brwAppFee);

//                //post Processing Fee
//                jnlBatchToBePosted = (new JournalExtensions()).Post("BRW", inputToUpdate.borrowingType.commissionAndFeesAccountId, ba.gl_acct_id,
//                    inputToUpdate.processingFee, "Processing Fee", cmpCurrency, brwDis.dateDisbursed,
//                            inputToUpdate.borrowingNo, ctx, user, null);
//                ctx.jnl_batch.Add(jnlBatchToBePosted);

//                //Save Processing Fee
//                borrowingFee brwProcFee = new borrowingFee
//                {
//                    borrowingId = inputToUpdate.borrowingId,
//                    feeDate = DateTime.Now,
//                    feeAmount = inputToUpdate.processingFee,
//                    feeTypeId = le.loanFeeTypes.FirstOrDefault(p => p.feeTypeName.ToLower().Contains("processing fee")).feeTypeID,
//                    creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
//                    created = DateTime.Now
//                };
//                inputToUpdate.borrowingFees.Add(brwProcFee);

//                //post Commission
//                jnlBatchToBePosted = (new JournalExtensions()).Post("BRW", inputToUpdate.borrowingType.commissionAndFeesAccountId, ba.gl_acct_id,
//                    inputToUpdate.commission, "Commission", cmpCurrency, brwDis.dateDisbursed,
//                            inputToUpdate.borrowingNo, ctx, user, null);
//                ctx.jnl_batch.Add(jnlBatchToBePosted);

//                //Save Commission
//                borrowingFee brwCommission = new borrowingFee
//                {
//                    borrowingId = inputToUpdate.borrowingId,
//                    feeDate = DateTime.Now,
//                    feeAmount = inputToUpdate.commission,
//                    feeTypeId = le.loanFeeTypes.FirstOrDefault(p => p.feeTypeName.ToLower().Contains("commission")).feeTypeID,
//                    creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
//                    created = DateTime.Now
//                };
//                inputToUpdate.borrowingFees.Add(brwCommission);

//            }

//            try
//            {
//                le.SaveChanges();
//                ctx.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                //If saving fails, Log the the Exception and display message to user.
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return inputToUpdate;
//        }


//        [HttpGet]
//        public borrowingRepayment NewBorrowingPayment()
//        {
//            return new borrowingRepayment();
//        }


//        [HttpPut]
//        public borrowing BorrowingPayment(borrowingRepayment input)
//        {
//            if (input.amountPaid < 1 || input.repayementDate == null)
//            {
//                throw new ApplicationException(error.BorrowingRepaymentAmountError);
//            }

//            borrowing inputToUpdate = le.borrowings
//                .FirstOrDefault(p => p.borrowingId == input.borrowingId);

//            borrowingRepayment repaymentToBeSaved = new borrowingRepayment();

//            if (inputToUpdate != null)
//            {
//                var modeOfPay =
//                    le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == input.modeOfPaymentId).modeOfPaymentName;

//                repaymentToBeSaved.borrowingId = input.borrowingId;
//                repaymentToBeSaved.modeOfPaymentId = input.modeOfPaymentId;
//                repaymentToBeSaved.repayementDate = input.repayementDate;
//                repaymentToBeSaved.repaymentTypeId = input.repaymentTypeId;
//                repaymentToBeSaved.amountPaid = input.amountPaid;
//                repaymentToBeSaved.interestPaid = 0;
//                repaymentToBeSaved.principalPaid = 0;
//                repaymentToBeSaved.feePaid = 0;
//                repaymentToBeSaved.penaltyPaid = 0;
//                repaymentToBeSaved.commissionPaid = 0;
//                repaymentToBeSaved.created = DateTime.Now;
//                repaymentToBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());

//                if (modeOfPay.ToLower().Contains("cheque"))
//                {
//                    repaymentToBeSaved.bankId = input.bankId;
//                    repaymentToBeSaved.checkNo = input.checkNo;
//                }

//                inputToUpdate.borrowingRepayments.Add(repaymentToBeSaved);
//                inputToUpdate.balance -= input.amountPaid;
//            }

//            jnl_batch toBePosted;
//            var ba = ctx.bank_accts.FirstOrDefault(p => p.bank_acct_id == repaymentToBeSaved.bankId);
//            int cmpCurrency = ctx.comp_prof.First().currency_id.Value;
//            var user = LoginHelper.getCurrentUser(new coreSecurityEntities());

//            toBePosted = (new JournalExtensions()).Post("BRW", ba.gl_acct_id, inputToUpdate.borrowingType.accountsPayableAccountId,
//                    repaymentToBeSaved.amountPaid, "Borrowing Repayment", cmpCurrency, repaymentToBeSaved.repayementDate,
//                            inputToUpdate.borrowingNo, ctx, user, null);
//            ctx.jnl_batch.Add(toBePosted);


            

//    try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                //If saving fails, Log the the Exception and display message to user.
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return inputToUpdate;
//        }

//        [HttpPut]
//        public borrowing Put(borrowing input)
//        {
//            //Validate INPUT and throw Exception if validate fails
//            if (!ValidateBorrowingFields(input))
//            {
//                throw new ApplicationException(errorMessage.ToString());
//            }

//            //Retrieve the borrowing record to update
//            borrowing inputToUpdate = le.borrowings
//                .Include(p => p.borrowingRepaymentSchedules)
//                .FirstOrDefault(p => p.borrowingId == input.borrowingId);
//            populateBorrowingFields(inputToUpdate, input);

//            foreach (var borrowingFee in inputToUpdate.borrowingFees)
//            {
//                //retrieve borrowingFee record to update
//                borrowingFee borrowingFeeToUpdate = inputToUpdate.borrowingFees
//                    .FirstOrDefault(p => p.borrowingfeeId == borrowingFee.borrowingfeeId);

//                if (borrowingFeeToUpdate == null)
//                {
//                    //If borrowingFeeToUpdate is null, its a new fee and we save
//                    borrowingFeeToUpdate = new borrowingFee();
//                    populateBorrowingFeeFields(borrowingFeeToUpdate, borrowingFee);
//                    inputToUpdate.borrowingFees.Add(borrowingFeeToUpdate);
//                }
//                else
//                {
//                    //Else we update the existing record
//                    populateBorrowingFeeFields(borrowingFeeToUpdate, borrowingFee);
//                }
//            }

//            foreach (var sched in input.borrowingRepaymentSchedules)
//            {
//                //retrieve borrowingSched record to update
//                borrowingRepaymentSchedule schedToUpdate;

//                if (sched.borrowingRepaymentScheduleId > 0)
//                {
//                    schedToUpdate = inputToUpdate.borrowingRepaymentSchedules
//                    .FirstOrDefault(p => p.borrowingRepaymentScheduleId == sched.borrowingRepaymentScheduleId);
//                    populateBorrowingRepaySchedFields(schedToUpdate, sched, input);
//                }
//                else
//                {
//                    //If borrowingSchedToUpdate is null, its a new fee and we save
//                    schedToUpdate = new borrowingRepaymentSchedule();
//                    populateBorrowingRepaySchedFields(schedToUpdate, sched, inputToUpdate);
//                    inputToUpdate.borrowingRepaymentSchedules.Add(schedToUpdate);
//                }
//            }

//            try
//            {
//                //Save changes to DB
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                //If saving fails, Log the the Exception and display message to user.
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return input;
//        }


//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "borrowingNo";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.borrowings.AsQueryable();
//            if (whereClause != null && whereClause.Trim().Length > 0)
//            {
//                query = query.Where(whereClause, parameters.ToArray());
//            }

//            var data = query
//                .OrderBy(order.ToString())
//                .Skip(req.skip)
//                .Take(req.take)
//                .ToArray();

//            return new KendoResponse(data, query.Count());
//        }
        
//        [HttpDelete]
//        // DELETE: api/borrowing
//        public void Delete([FromBody]borrowing input)
//        {
//            var forDelete = le.borrowings
//                .Include(p => p.borrowingDisbursements)
//                .Include(p => p.borrowingRepaymentSchedules)
//                .Include(p => p.borrowingRepayments)
//                .Include(p => p.borrowingDocuments)
//                .FirstOrDefault(p => p.borrowingId == input.borrowingId);
//            if (forDelete != null)
//            {
//                le.borrowings.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }

//        //populate Borrowing the fields to be saved
//        private void populateBorrowingFields(borrowing toBeSaved, borrowing input)
//        {
//            toBeSaved.clientId = input.clientId;
//            toBeSaved.borrowingTypeId = input.borrowingTypeId;
//            toBeSaved.borrowingTenure = input.borrowingTenure;
//            toBeSaved.tenureTypeId = input.tenureTypeId;
//            if (input.borrowingId < 1)
//            {
//                toBeSaved.borrowingNo = IDGenerator.newBorrowingNumber(le, input, "BRW", 7);
//            }
//            toBeSaved.balance = 0.00;
//            toBeSaved.amountRequested = input.amountRequested;
//            toBeSaved.applicationDate = input.applicationDate;
//            toBeSaved.borrowingStatusId = 1;
//            toBeSaved.interestTypeId = input.interestTypeId;
//            toBeSaved.interestRate = input.interestRate;
//            toBeSaved.repaymentModeId = input.repaymentModeId;
//            toBeSaved.applicationFee = input.applicationFee;
//            toBeSaved.processingFee = input.processingFee;
//            toBeSaved.creditOfficerNotes = "";
//            toBeSaved.approvalComments = "";
//            toBeSaved.closed = false;
//            toBeSaved.commission = input.commission;
//            toBeSaved.appliedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.created = DateTime.Now;
//            toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//        }

//        //Populate borrowing Fees
//        private void populateBorrowingFeeFields(borrowingFee toBeSaved, borrowingFee input)
//        {
//            toBeSaved.feeDate = input.feeDate;
//            toBeSaved.feeAmount = input.feeAmount;
//            toBeSaved.feeTypeId = input.feeTypeId;
//            toBeSaved.created = DateTime.Now;
//            toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//        }

//        //Populate borrowing Fees
//        private void populateBorrowingRepaySchedFields(borrowingRepaymentSchedule toBeSaved, borrowingRepaymentSchedule input, borrowing brw)
//        {
//            toBeSaved.borrowingId = input.borrowingId;
//            toBeSaved.repaymentDate = input.repaymentDate;
//            toBeSaved.interestPayment = input.interestPayment;
//            toBeSaved.principalPayment = input.principalPayment;
//            toBeSaved.interestBalance = GetInterestBalance(toBeSaved, brw);
//            toBeSaved.principalBalance = GetprincipalBalance(toBeSaved, brw);
//            toBeSaved.balanceBF = GetBalanceBF(brw);
//            toBeSaved.balanceCD = GetBalanceCD(toBeSaved, brw);
//            toBeSaved.created = DateTime.Now;
//            toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//        }

//        //Validate Fields in Borrowing Form
//        private bool ValidateBorrowingFields(borrowing input)
//        {
//            ValidateBorrowingDropDowns(input);
//            ValidateBorrowingInvalidDate(input);
//            ValidateBorrowingEmptyFields(input);

//            //If errorMessage is empty test Pass
//            if (errorMessage.ToString() == "")
//            {
//                return true;
//            }

//            return false;
//        }


//        //validate Borrowing Drop Down to ensure User selected from the Drop down
//        private void ValidateBorrowingDropDowns(borrowing brw)
//        {
//            if (!clientExist(brw.clientId) || !borrowingTypeExists(brw.borrowingTypeId)
//                || !tenureTypeExists(brw.tenureTypeId) 
//                || !interestTypeExists(brw.interestTypeId) || !repaymentModeExists(brw.repaymentModeId))
//            {
//                errorMessage.Append(error.BorrowingFormError);
//            }
            
//        }

//        //Validation for application date future & empty date
//        private void ValidateBorrowingInvalidDate(borrowing brw)
//        {
//            if (brw.applicationDate == DateTime.MinValue)
//            {
//                errorMessage.Append(error.BorrowingInvalidApplicationDate);
//            }

//            if (brw.applicationDate.Date > DateTime.Today)
//            {
//                errorMessage.Append(error.BorrowingFutureApplicationDate);
//            }
//        }

//        //Check for empty fields
//        private void ValidateBorrowingEmptyFields(borrowing brw)
//        {
//            if (brw.borrowingTenure < 1
//                || brw.amountRequested < 1 || brw.interestRate < 0)
//            {
//                errorMessage.Append(error.BorrowingEmptyNumericInput);
//            }
//        }

//        //Check if client Exist
//        private bool clientExist(long clientId)
//        {
//            if (le.clients.Any(p => p.clientID == clientId))
//            {
//                return true;
//            }
//            return false;
//        }

//        //Check if borrowingType Exist
//        private bool borrowingTypeExists(long brwTypeId)
//        {
//            if (le.borrowingTypes.Any(p => p.borrowingTypeId == brwTypeId))
//            {
//                return true;
//            }
//            return false;
//        }

//        //Check if tenureType Exist
//        private bool tenureTypeExists(long tenureTypeId)
//        {
//            if (le.tenureTypes.Any(p => p.tenureTypeID == tenureTypeId))
//            {
//                return true;
//            }
//            return false;
//        }

//        //Check if borrowingStatus Exist
//        private bool borrowingStatusExists(long brwStutusId)
//        {
//            if (le.loanStatus.Any(p => p.loanStatusID == brwStutusId))
//            {
//                return true;
//            }
//            return false;
//        }

//        //Check if interestType Exist
//        private bool interestTypeExists(int interestTypeId)
//        {
//            if (le.interestTypes.Any(p => p.interestTypeID == interestTypeId))
//            {
//                return true;
//            }
//            return false;
//        }

//        //Check if repaymentMode Exist
//        private bool repaymentModeExists(int repaymentModeId)
//        {
//            if (le.repaymentModes.Any(p => p.repaymentModeID == repaymentModeId))
//            {
//                return true;
//            }
//            return false;
//        }

//        //Get Borrowing Pricipal balance
//        private double GetprincipalBalance(borrowingRepaymentSchedule input, borrowing brw)
//        {
//            double principalBal = !brw.borrowingRepaymentSchedules.Any()
//                ? brw.amountDisbursed
//                : // if it's empty, start with amountDisbursed
//                brw.borrowingRepaymentSchedules.OrderByDescending(i => i.borrowingRepaymentScheduleId)
//                    // order by id descending
//                    .First() // get first one 
//                    .principalBalance - input.principalPayment;

//            return principalBal;
//        }

//        //Get Borrowing Pricipal balance
//        private double GetInterestBalance(borrowingRepaymentSchedule input, borrowing brw)
//        {
//            double interestBal = !brw.borrowingRepaymentSchedules.Any()
//                ? (brw.amountDisbursed * brw.interestRate) * brw.borrowingTenure
//                : // if it's empty, start with amountDisbursed
//                brw.borrowingRepaymentSchedules.OrderByDescending(i => i.borrowingRepaymentScheduleId)
//                    // order by id descending
//                    .First() // get first one 
//                    .interestBalance - input.interestPayment;

//            return interestBal;
//        }

//        private double GetBalanceBF(borrowing brw)
//        {
//            double getBalBF = !brw.borrowingRepaymentSchedules.Any()
//                ? (brw.amountDisbursed * brw.interestRate) * brw.borrowingTenure +brw.amountDisbursed
//                : // if it's empty, start with amountDisbursed
//                brw.borrowingRepaymentSchedules.OrderByDescending(i => i.borrowingRepaymentScheduleId)
//                    // order by id descending
//                    .First() // get first one 
//                    .balanceCD;

//            return getBalBF;
//        }

//        private double GetBalanceCD(borrowingRepaymentSchedule input, borrowing brw)
//        {
//            double getBalBF = !brw.borrowingRepaymentSchedules.Any()
//                ? ((brw.amountDisbursed * brw.interestRate) * brw.borrowingTenure + brw.amountDisbursed)
//                - (input.interestPayment + input.principalPayment)
//                : // if it's empty, start with amountDisbursed
//                brw.borrowingRepaymentSchedules.OrderByDescending(i => i.borrowingRepaymentScheduleId)
//                    // order by id descending
//                    .First() // get first one 
//                    .balanceCD - (input.interestPayment + input.principalPayment);

//            return getBalBF;
//        }

//    }
//}
