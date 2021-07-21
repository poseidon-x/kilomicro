using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; 
using System.Web.Http;
using System.Web.Http.Cors;
using coreData.Constants;
using coreLogic; 
using agencyAPI.Providers;
using agencyAPI.Models;

namespace agencyAPI.Controllers.Saving
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class SavingController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();
        ErrorMessages error = new ErrorMessages();
        IIDGenerator idGen = new IDGenerator();

        public SavingController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<saving> Get()
        {
            return le.savings
                .OrderBy(p => p.savingNo)
                .ToList();
        }

        // GET: api/Category
        public IEnumerable<saving> Get(int id)
        {
            return le.savings
                .Where(p => p.clientID == id && (p.savingAdditionals.Count > 0))
                .OrderBy(p => p.savingNo)
                .ToList();
        }

        //Get a saving account
        [HttpGet]
        public saving GetSavingAccount(int id)
        {
            var data = le.savings
                 .FirstOrDefault(p => p.clientID == id);
            if (data == null) { data = new saving(); }
            return data;
        }

        //Get a savingAdditional account
        public savingAdditional GetSavingAdditional(int id)
        {
            var data = le.savingAdditionals
                .Include(p => p.saving)
                .FirstOrDefault(p => p.savingAdditionalID == id);
            if (data == null) { data = new savingAdditional(); }
            return data;
        }

        //Get a savingAdditional account
        public savingWithdrawal GetSavingWithdrawal(int id)
        {
            var data = le.savingWithdrawals
                .Include(p => p.saving)
                .FirstOrDefault(p => p.savingWithdrawalID == id);
            if (data == null) { data = new savingWithdrawal(); }
            return data;
        }

        [HttpPost]
        // POST: api/Category
        public saving Post([FromBody]saving value)
        {
            le.savings.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPost]
        // POST: api/Category
        public saving PostSavings(saving input)
        {
            if (input == null) return null;
            if (input.savingID > 0)
            {
                var sav = le.savings.Include(p => p.savingNextOfKins).FirstOrDefault(p => p.savingID == input.savingID);
                populateSavingsFields(sav, input);
            }
            else
            {
                saving sav = new saving();
                populateSavingsFields(sav, input);
                le.savings.Add(sav);
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            le.SaveChanges();

            return input;
        }

        [HttpPost]
        // POST: api/savingAdditional
        public savingAdditional PostSavingsAddtional(savingAdditional input)
        {
            if (input == null) return null;
            var savin = le.savings.FirstOrDefault(p => p.savingID == input.saving.savingID);
            if (savin == null) throw new ApplicationException("Saving Account doesn't exist");

            savingAdditional savAddToBeSaved = new savingAdditional();
            populateSavingsAdditionalFields(savAddToBeSaved, input);
            le.savingAdditionals.Add(savAddToBeSaved);
            savin.amountInvested += savAddToBeSaved.savingAmount;
            savin.principalBalance += savAddToBeSaved.savingAmount;
            savin.availablePrincipalBalance += savAddToBeSaved.savingAmount;

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            le.SaveChanges();
            return input;
        }

        [HttpPost]
        // POST: api/Category
        public savingWithdrawal PostSavingsWithdrawal(savingWithdrawalViewModel input)
        {
            if (input == null) return null;
            var savin = le.savings.FirstOrDefault(p => p.savingID == input.saving.savingID);
            if (savin == null) throw new ApplicationException("Saving Account doesn't exist");

            savingWithdrawal sav = populateSavingsWithdrawalFields(input, savin);
            le.savingWithdrawals.Add(sav);
            savin.principalBalance -= input.principalWithdrawal;
            savin.availablePrincipalBalance -= input.principalWithdrawal;
            savin.interestBalance -= input.interestWithdrawal;
            savin.availableInterestBalance -= input.interestWithdrawal;

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            return sav;
        }

        [HttpPut]
        public savingType Put([FromBody]savingType value)
        {
            var toBeUpdated = new savingType
            {
                accountsPayableAccountID = value.accountsPayableAccountID,
                allowsInterestWithdrawal = value.allowsInterestWithdrawal,
                allowsPrincipalWithdrawal = value.allowsPrincipalWithdrawal,
                chargesIncomeAccountID = value.chargesIncomeAccountID,
                interestCalculationScheduleID = value.interestCalculationScheduleID,
                fxRealizedGainLossAccountID = value.fxRealizedGainLossAccountID,
                fxUnrealizedGainLossAccountID = value.fxUnrealizedGainLossAccountID,
                defaultPeriod = value.defaultPeriod,
                interestExpenseAccountID = value.interestExpenseAccountID,
                interestPayableAccountID = value.interestPayableAccountID,
                interestRate = value.interestRate,
                maxPlanAmount = value.maxPlanAmount,
                minPlanAmount = value.minPlanAmount,
                vaultAccountID = value.vaultAccountID,
                planID = value.planID,
                savingTypeID = value.savingTypeID,
                savingTypeName = value.savingTypeName,
                earlyWithdrawalChargeRate = value.earlyWithdrawalChargeRate,
                minDaysBeforeInterest = value.minDaysBeforeInterest
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]savingType value)
        {
            var forDelete = le.savingTypes.FirstOrDefault(p => p.savingTypeID == value.savingTypeID);
            if (forDelete != null)
            {
                le.savingTypes.Remove(forDelete);
                le.SaveChanges();
            }
        }







        private void populateSavingsFields(saving target, saving source)
        {
            target.clientID = source.clientID;
            target.savingTypeID = source.savingTypeID;
            target.amountInvested = 0.0;
            target.interestAccumulated = 0.0;
            target.interestBalance = 0.0;
            target.principalBalance = 0.0;
            target.interestRate = source.interestRate;
            target.autoRollover = false;
            target.interestRate = source.interestRate;
            if (source.savingID < 1)
            {
                var svTyp = le.savingTypes.FirstOrDefault(p => p.savingTypeID == source.savingTypeID);
                var cl = le.clients.FirstOrDefault(p => p.clientID == source.clientID);
                target.savingNo = idGen.NewSavingsNumber(cl.branchID.Value,
                            cl.clientID, source.savingID, svTyp.savingTypeName.Substring(0, 2).ToUpper());
                target.period = svTyp.defaultPeriod;
                target.agentId = source.agentId;
                target.maturityDate = source.firstSavingDate.AddMonths(svTyp.defaultPeriod);
                target.interestExpected = 0;
                target.availableInterestBalance = 0;
                target.availablePrincipalBalance = 0;
                target.clearedInterestBalance = 0;
                target.clearedPrincipalBalance = 0;
                target.currencyID = 1;
                target.firstSavingDate = source.firstSavingDate;
                target.fxRate = 1;
                target.interestExpected = 0;
                target.localAmount = 0;
                target.principalAuthorized = 0;
                target.principalAuthorized = 0;
                target.status = "A";
                target.interestMethod = true;
                target.principalRepaymentModeID = source.principalRepaymentModeID;
                target.interestRepaymentModeID = source.interestRepaymentModeID;
                target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                target.creation_date = DateTime.Now;
                target.savingPlanID = source.savingPlanID;

                var dep = source.savingPlans.Where(p => p.deposited == false).ToList();
                for (int i = dep.Count - 1; i >= 0; i--)
                {
                    var d = dep[i];
                    le.savingPlans.Remove(d);
                }
                var plan = source.savingPlans.FirstOrDefault();
                target.savingPlanID = source.savingPlanID;
                target.savingPlanAmount = source.savingPlanAmount;
                var date = target.firstSavingDate;
                var endDate = date.AddMonths(target.period);
                while (date < endDate && source.savingPlanID > 0)
                {
                    target.savingPlans.Add(new coreLogic.savingPlan
                    {
                        plannedAmount = source.savingPlanAmount,
                        plannedDate = date,
                        deposited = false,
                        creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                        creationDate = DateTime.Now,
                        amountDeposited = 0,
                    });
                    if (target.savingPlanID == 5 || target.savingPlanID == 6)
                    {
                        date = date.AddDays(1);
                    }
                    else if (target.savingPlanID == 7)
                    {
                        date = date.AddDays(7);
                    }
                    else if (target.savingPlanID == 30)
                    {
                        date = date.AddMonths(1);
                    }
                    else if (target.savingPlanID == 90)
                    {
                        date = date.AddMonths(3);
                    }
                    else if (target.savingPlanID == 180)
                    {
                        date = date.AddMonths(6);
                    }
                    else
                    {
                        break;
                    }
                    if (target.savingPlanID != 6 && date.DayOfWeek == DayOfWeek.Saturday)
                    {
                        date = date.AddDays(1);
                    }
                    if (date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        date = date.AddDays(1);
                    }

                }
            }

            foreach (var savNextofKin in source.savingNextOfKins)
            {
                if (savNextofKin.savingNextOfKinId > 0)
                {
                    var nextOfKinToBeSaved =
                        le.savingNextOfKins.FirstOrDefault(p => p.savingNextOfKinId == savNextofKin.savingNextOfKinId);
                    populateNextOfKinFields(nextOfKinToBeSaved, savNextofKin);
                }
                else
                {
                    savingNextOfKin nextOfKinToBeSaved = new savingNextOfKin();
                    populateNextOfKinFields(nextOfKinToBeSaved, savNextofKin);
                    target.savingNextOfKins.Add(nextOfKinToBeSaved);
                }
            }
        }

        private void populateNextOfKinFields(savingNextOfKin target, savingNextOfKin source)
        {
            //target.dob = source.dob;
            target.otherNames = source.otherNames;
            target.surName = source.surName;
            target.relationshipType = source.relationshipType;
            target.idTypeId = source.idTypeId;
            target.idNumber = source.idNumber;
            target.percentageAllocated = source.percentageAllocated;
            target.phoneNumber = source.phoneNumber;
        }

        private void populateSavingsAdditionalFields(savingAdditional target, savingAdditional source)
        {
            //var time = source.savingDate.Date.Add(DateTime.Now.TimeOfDay);
            var sav = source.saving;
            target.savingID = source.saving.savingID;
            target.savingDate = source.savingDate.Date.Add(DateTime.Now.TimeOfDay);
            target.savingAmount = source.savingAmount;
            target.principalBalance = source.savingAmount + sav.principalBalance;
            target.balanceBD = sav.principalBalance + sav.interestBalance;
            target.interestBalance = source.interestBalance;
            target.naration = source.naration;
            if (source.modeOfPaymentID == 2)
            {
                target.checkNo = source.checkNo;
                target.bankID = source.bankID;
            }
            target.localAmount = source.localAmount;
            target.modeOfPaymentID = source.modeOfPaymentID;
            target.posted = false;
            target.closed = false;
            target.fxRate = 0;
            target.creation_date = DateTime.Now;
            target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
        }

        private savingWithdrawal populateSavingsWithdrawalFields(savingWithdrawalViewModel source, saving sav)
        {
            coreLogic.IInvestmentManager ivMgr = new coreLogic.InvestmentManager();
            var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == source.modeOfPaymentID);
            var loggedInUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            savingWithdrawal dw = null;
            try
            {
                double pAmount = source.principalWithdrawal;
                double iAmount = source.interestWithdrawal;
                dw = ivMgr.WithdrawalOthers(ref pAmount, ref iAmount, mop, source.bankID,
                    source.withType, sav, source.withdrawalAmount, source.withdrawalDate,
                    source.checkNo, source.naration, loggedInUser);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured");
            }
            return dw;
        }
    }
}
