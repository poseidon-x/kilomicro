using System;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using coreErpApi.Controllers.Models;
using coreERP.Models.Client;
using System.Collections.Generic;
using coreData.Constants;
using Microsoft.Ajax.Utilities;
using System.Data.Entity;
using coreERP.Models;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    public class LoanRepaymentController : ApiController
    {
        IcoreLoansEntities le;
        HelperMethod helper;
        public LoanRepaymentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            helper = new HelperMethod();
        }

        public LoanRepaymentController(IcoreLoansEntities lent)
        {
            le = lent;
        }



        [HttpGet]
        public LoanFeePaymentMultiModel GetMulti()
        {
            return new LoanFeePaymentMultiModel
            {
                payments = new List<loanPayment>()
            };
        }
        [HttpGet]
        public GroupLoanFeePaymentMultiModel GetGroupLoanMulti()
        {
            return new GroupLoanFeePaymentMultiModel
            {
                payments = new List<GroupLoanPayment>()
            };
        }

        //NOTE:We are no longer getting the fees by Day
        [HttpPost]
        public LoanRepaymentFeePostModel GetRepaymentFeesForDay(LoanRepaymentFeePostModel model)
        {
            var ct = le.cashiersTills.SingleOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());

            var repTypeIds = le.repaymentTypes
                .Where(p => p.repaymentTypeName.ToLower().Contains("insurance")
                || p.repaymentTypeName.ToLower().Contains("processing"))
                .OrderBy(p => p.repaymentTypeID)
                .ToList();
            var dayClients = le.loanGroupClients
                .Include(p => p.loanGroup.client)
                .Include(p => p.loanGroup.loanGroupDay)
                .Where(p => p.client.clientTypeID == 7 && p.loanGroup.loanGroupDayId == model.dayId && p.loanGroup.staff.userName == ct.userName)
                .Select(p => p.clientId)
                .ToList();
            var feesRepaymt = new List<LoanRepaymentFeeModel>();
            var loanRepays = le.loans
                   .Where(p => dayClients.Contains(p.clientID) && p.loanStatusID == 3 && p.loanTypeID == 10)
                   .Join(le.clients, l => l.clientID, c => c.clientID, (l, c) => new LoanRepaymentFeeModel
                   {
                       loanId = l.loanID,
                       clientId = c.clientID,
                       clientName = c.surName + " " + c.otherNames,
                       // repaymentTypeID = repTypeId.repaymentTypeID,
                       loanNo = l.loanNo,
                       paid = false
                   })
                   .OrderBy(p => p.loanNo)
                   .ToList();
            foreach (var lp in loanRepays)
            {
                var latestOutstdFee = le.loans
                    .Where(o => o.loanID == lp.loanId)
                    .OrderBy(p => p.loanID)
                    .FirstOrDefault();
                if (latestOutstdFee != null)
                {
                    foreach (var repTypeId in repTypeIds)
                    {
                        if (repTypeId.repaymentTypeID == 6)
                        {
                            lp.processingFeeAmount = latestOutstdFee.processingFee;
                        }
                        else
                        {
                            lp.insuranceAmount = latestOutstdFee.insuranceAmount;
                        }
                    }

                }
                lp.totalFees = lp.processingFeeAmount + lp.insuranceAmount;
                feesRepaymt.Add(lp);
            }
            // feesRepayments.AddRange(loanRepays);

            var feesRepayments = feesRepaymt.OrderBy(p => p.loanNo).ThenBy(p => p.clientName).ToList();
            if (feesRepayments != null && feesRepayments.Count() > 0)
                return new LoanRepaymentFeePostModel
                {
                    repaymentFees = feesRepayments
                };

            return null;
        }

        //NOTE: This is the new one
        [HttpPost]
        public LoanFeePostModel GetTodayRepaymentFees(LoanFeePostModel model)
        {
            List<repaymentType> repTypes;
            List<LoanFeesRepayment> loanRepays;
            GetLoanRepaymentsForGroup(out repTypes, out loanRepays);
            var feesRepayments = FillInAdditionalRepaymentFields(repTypes, loanRepays);
            if (feesRepayments != null && feesRepayments.Count() > 0)
                return new LoanFeePostModel
                {
                    repaymentFees = feesRepayments
                };

            return null;
        }

        private void GetLoanRepaymentsForGroup(out List<repaymentType> repTypeIds, out List<LoanFeesRepayment> loanRepays)
        {
            var currentUserName = User?.Identity?.Name?.Trim()?.ToLower();
            if (currentUserName == null)
            {
                throw new ArgumentException("No User Identified");
            }
            var ct = le.cashiersTills.SingleOrDefault(p => p.userName.ToLower().Trim() == currentUserName);
            if (ct == null)
            {
                throw new ArgumentException("There is no till defined for the currently logged in user (" + currentUserName + ")", "coreERP©: Failed");
            }
            repTypeIds = le.repaymentTypes
                .Where(p => p.repaymentTypeName.ToLower().Contains("insurance")
                || p.repaymentTypeName.ToLower().Contains("processing"))
                .OrderBy(p => p.repaymentTypeID)
                .ToList();
            var today = DateTime.Now.Date;

            //Check for Login user
            var dayClientIds = new List<int>();
            var clients = le.loanGroupClients
                .Include(p => p.loanGroup.client)
                .Where(p => p.client.clientTypeID == 7).ToList();
            if (!helper.IsOwnerOrAdmin(currentUserName))
            {
                dayClientIds = clients.Where(p => p.loanGroup.staff.userName == currentUserName).Select(p => p.clientId).ToList();
            }
            else
            {
                dayClientIds = clients.Select(p => p.clientId).ToList();
            }
            loanRepays = le.loans
                .Include(r => r.client)
                   .Where(p => dayClientIds.
                   Contains(p.clientID) && ( p.loanStatusID == 3 || p.loanStatusID==1 || p.loanStatusID == 2) && p.loanTypeID == 10 && p.finalApprovalDate.Value.Date == today)
                   .OrderBy(p => p.loanNo)
                   .Select(l => new LoanFeesRepayment
                   {
                       loanId = l.loanID,
                       loanNo = l.loanNo,
                       clientId = l.clientID,
                       clientName = l.client.surName + " " + l.client.otherNames
                   })
                   .ToList();
        }

        private List<LoanFeesRepayment> FillInAdditionalRepaymentFields(List<repaymentType> repTypes, List<LoanFeesRepayment> loanRepays)
        {
            var feesRepaymt = new List<LoanFeesRepayment>();
            foreach (var lp in loanRepays)
            {
                var group = le.loanGroupClients
                    .Include(g => g.loanGroup)
                    .FirstOrDefault(g => g.clientId == lp.clientId);
                if (group != null)
                {
                    lp.groupName = group.loanGroup.loanGroupName;
                }
                var latestOutstdFee = le.loans
                    .Where(o => o.loanID == lp.loanId)
                    .OrderBy(p => p.loanID)
                    .FirstOrDefault();
                if (latestOutstdFee != null)
                {
                    foreach (var repType in repTypes)
                    {
                        if (repType.repaymentTypeID == 6)
                        {
                            lp.processingFeeAmount = latestOutstdFee.processingFee;
                        }
                        else
                        {
                            lp.insuranceAmount = latestOutstdFee.insuranceAmount;
                        }
                    }

                }
                lp.totalFees = lp.processingFeeAmount + lp.insuranceAmount;
                feesRepaymt.Add(lp);
                
            }
            return feesRepaymt.OrderBy(p => p.groupName).ThenBy(p => p.loanNo).ThenBy(p => p.clientName).ToList();
        }

        [HttpGet]
        public List<loan> GetLoanMulti()
        {
            var loans = le.loans
                .Where(p => p.loanTypeID == 10 && (p.loanStatusID == 3 || p.loanStatusID == 1 || p.loanStatusID == 2));
            if (loans != null)
            {
                return loans.ToList();
            }
            return null;
        }

        //Get loans for group  //TODO: delete after everything works
        [HttpGet]
        public IEnumerable<LoanViewModel> GetClientGroupLoans(int id)
        {
            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == id);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients.Select(p => p.clientId).ToList();

            return le.loans
                .Where(p => groupClients.Contains(p.clientID) && p.loanTypeID == 10 && (p.loanStatusID == 3 || p.loanStatusID == 1 || p.loanStatusID==2))
                .Join(le.clients, l => l.clientID, c => c.clientID, (l, c) => new LoanViewModel
                {
                    loanId = l.loanID,
                    clientId = c.clientID,
                    clientName = c.surName + " " + c.otherNames,
                    loanNumberWithName = l.loanNo + " - " + c.surName + " " + c.otherNames,
                    amountApproved = l.amountApproved,
                    amountDisbursed = l.amountDisbursed
                })
                .OrderBy(p => p.loanNumberWithName)
                .ToList();
        }

        // POST: api/clientServiceCharge/
        [HttpPost]
        public bool PostMulti(LoanFeePaymentMultiModel input)
        {
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ct == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" + User.Identity.Name + ")");
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == input.paymentDate.Date
                && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")");
            }

            //Validate input
            var dataToSave = new List<cashierReceipt>();
            var loan = le.loans.FirstOrDefault(p => p.loanID == input.loanId);
            generatePayment(dataToSave, input, loan, ct.cashiersTillID);
            if (dataToSave.Count > 0)
            {
                foreach (var payment in dataToSave)
                {
                    //validateInput(charge);
                }

                foreach (var payment in dataToSave)
                {
                    le.cashierReceipts.Add(payment);
                    if (payment.repaymentTypeID == 5 || payment.repaymentTypeID == 6)
                    {
                        loan.loanFees.Add(new coreLogic.loanFee
                        {
                            feeAmount = payment.amount,
                            feeDate = input.paymentDate.Date,
                            feeTypeID = payment.repaymentTypeID == 5 ? 2 : 1,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name
                        });
                    }
                    else if (payment.repaymentTypeID == 8)
                    {
                        loan.loanInsurances.Add(new loanInsurance
                        {
                            amount = payment.amount,
                            insuranceDate = input.paymentDate.Date,
                            paid = false
                        });
                    }

                    try
                    {
                        le.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //If saving fails, Log the the Exception and display message to user.
                        throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                    }
                }
                return true;
            }
            return false;

        }


        private void generatePayment(List<cashierReceipt> toBeSaved, LoanFeePaymentMultiModel input, loan ln, int cashierTillId)
        {
            foreach (var payment in input.payments)
            {
                toBeSaved.Add(new cashierReceipt
                {
                    cashierTillID = cashierTillId,
                    txDate = input.paymentDate.Date,
                    amount = payment.amount,
                    clientID = ln.clientID,
                    loanID = input.loanId,
                    paymentModeID = 1,
                    repaymentTypeID = payment.repaymentTypeID,
                    feeAmount = payment.amount,
                    posted = false,
                });
            }
        }


        // GET: api/
        public LoanRestructureViewModel Get(int id)
        {
            var loan = le.loans
                .FirstOrDefault(p => p.loanID == id);

            var principalAndInterestId = le.repaymentTypes
                .Where(p => p.repaymentTypeName.ToLower().Contains("principal and interest"))
                .FirstOrDefault().repaymentTypeID;

            var penaltyId = le.repaymentTypes
                .Where(p => p.repaymentTypeName.ToLower().Contains("penalty"))
                .FirstOrDefault().repaymentTypeID;

            var repayments = le.loanRepayments
                .Where(p => p.repaymentTypeID == principalAndInterestId && p.loanID == id)
                .OrderBy(p => p.loanRepaymentID)
                .ToList();

            var panaltyPayments = le.loanRepayments
                .Where(p => p.repaymentTypeID == penaltyId && p.loanID == id)
                .OrderBy(p => p.loanRepaymentID)
                .ToList();


            var penalties = le.loanPenalties
                .Where(p => p.loanID == id)
                .OrderByDescending(p => p.loanPenaltyID)
                .ToList();

            var dataToReturn = new LoanRestructureViewModel
            {
                loanTotalPrincipal = loan.amountDisbursed,
                loanTotalInterest = loan.amountDisbursed * (loan.interestRate / 100),
                loanBalance = loan.balance,
                totalPrinPaid = 0,
                totalIntrPaid = 0,
                totalyPenalties = 0,
                penaltyBalance = 0,
                totalPenaltyPayms = 0
            };

            double loanOverallBalanace = 0;


            if (panaltyPayments.Any())
            {
                foreach (var record in panaltyPayments)
                {
                    dataToReturn.totalPenaltyPayms += record.penaltyPaid;
                }
                var length = penalties.Count - 1;
                dataToReturn.totalyPenalties = penalties[length].penaltyBalance;
            }

            foreach (var record in repayments)
            {
                dataToReturn.totalPrinPaid += record.principalPaid;
                dataToReturn.totalIntrPaid += record.interestPaid;
            }


            dataToReturn.penaltyBalance = dataToReturn.totalyPenalties - dataToReturn.totalPenaltyPayms;

            return dataToReturn;
        }


        // POST: 
        [HttpPost]
        public bool PostGroupMulti(GroupLoanFeePaymentMultiModel input)
        {
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ct == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" + User.Identity.Name + ")");
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == input.paymentDate.Date
                && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")");
            }

            //Validate input
            var dataToSave = new List<cashierReceipt>();
            var loan = le.loans.FirstOrDefault(p => p.loanID == input.loanId);
            foreach (var payment in input.payments)
            {
                dataToSave.Add(new cashierReceipt
                {
                    cashierTillID = ct.cashiersTillID,
                    txDate = input.paymentDate.Date,
                    amount = payment.amount,
                    clientID = loan.clientID,
                    loanID = input.loanId,
                    paymentModeID = 1,
                    repaymentTypeID = payment.repaymentTypeID,
                    feeAmount = payment.amount,
                    posted = false,
                });
            }
            if (dataToSave.Count > 0)
            {
                foreach (var payment in dataToSave)
                {
                    le.cashierReceipts.Add(payment);
                    if (payment.repaymentTypeID == 5 || payment.repaymentTypeID == 6)
                    {
                        loan.loanFees.Add(new coreLogic.loanFee
                        {
                            feeAmount = payment.amount,
                            feeDate = input.paymentDate.Date,
                            feeTypeID = payment.repaymentTypeID == 5 ? 2 : 1,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name
                        });
                    }
                    else if (payment.repaymentTypeID == 8)
                    {
                        loan.loanInsurances.Add(new coreLogic.loanInsurance
                        {
                            amount = payment.amount,
                            insuranceDate = input.paymentDate.Date,
                            paid = false
                        });
                    }

                    try
                    {
                        le.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                    }
                }
                return true;
            }
            return false;

        }

        // POST: 
        [HttpPost]
        public bool PostRepaymentFeesForDay(LoanFeePostModel input)
        {
            var today = DateTime.Now.Date;
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ct == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" + User.Identity.Name + ")");
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == today
                && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")");
            }

            var dataToSave = new List<cashierReceipt>();

            foreach (var dataInput in input.repaymentFees.Where(p => p.paid==true))
            {
                if (dataInput.insuranceAmount > 0)
                {
                    dataToSave.Add(new cashierReceipt
                    {
                        cashierTillID = ct.cashiersTillID,
                        txDate = today,
                        amount = dataInput.insuranceAmount,
                        clientID = dataInput.clientId,
                        loanID = dataInput.loanId,
                        paymentModeID = 1,
                        repaymentTypeID = 8,
                        feeAmount = dataInput.insuranceAmount,
                        posted = false
                    });
                }
                if (dataInput.processingFeeAmount > 0)
                {
                    dataToSave.Add(new cashierReceipt
                    {
                        cashierTillID = ct.cashiersTillID,
                        txDate = today,
                        amount = dataInput.processingFeeAmount,
                        clientID = dataInput.clientId,
                        loanID = dataInput.loanId,
                        paymentModeID = 1,
                        repaymentTypeID = 6,
                        feeAmount = dataInput.processingFeeAmount,
                        posted = false
                    });
                }

            }


            if (dataToSave.Count > 0)
            {
                foreach (var payment in dataToSave)
                {
                    var loan = le.loans.FirstOrDefault(p => p.loanID == payment.loanID);
                    le.cashierReceipts.Add(payment);
                    if (payment.repaymentTypeID == 6)
                    {
                        loan.loanFees.Add(new coreLogic.loanFee
                        {
                            feeAmount = payment.amount,
                            feeDate = today,
                            feeTypeID = payment.repaymentTypeID == 5 ? 2 : 1,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name
                        });
                    }
                    else if (payment.repaymentTypeID == 8)
                    {
                        loan.loanInsurances.Add(new loanInsurance
                        {
                            amount = payment.amount,
                            insuranceDate = today,
                            paid = false
                        });
                    }

                    try
                    {
                        le.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                    }
                }
                return true;
            }
            return false;

        }

    }
}
