//*******************************************
//***   CLIENT SERVICE CHARGES CONTROLLER                
//***   CREATOR: EMMANUEL OWUSU(MAN)  AND MODIFIED BY SAMUEL WENDOLIN KETECHIE   
//***   WEEK: OCT 20TH, 2015  	
//***   MODIFICATION DATE: JAN 13, 2020
//*******************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using coreData.Constants;
using coreData.ErrorLog;
using coreERP.Models.Client;

namespace coreErpApi.Controllers.Controllers.Clients
{
    [AuthorizationFilter()]
    public class ClientServiceChargeController : ApiController
    {
        IcoreLoansEntities le;
        private readonly ErrorMessages error = new ErrorMessages();

        //Default constructor
        public ClientServiceChargeController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        //Constructor for unit testing
        public ClientServiceChargeController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/clientCheck
        public IEnumerable<clientServiceCharge> GetAllClientServiceCharges(int id)
        {
            var data = le.clientServiceCharges
                .Where(p => p.clientId == id)
                .OrderBy(p => p.chargeDate)
                .ToList();
            return data;
        }

        // GET: api/clientServiceCharge/
        [HttpGet]
        public clientServiceCharge Get(int id)
        {
            var check =  le.clientServiceCharges
                .FirstOrDefault(p => p.clientServiceChargeId == id);

            if (check == null)
            { check = new clientServiceCharge(); }

            return check;
        }

        // GET: api/clientServiceCharge/
        [HttpGet]
        public ClientServiceChargeMultiModel Get()
        {
            return new ClientServiceChargeMultiModel 
            {
                payments = new List<clientPayment>() 
            };
        }

        // GET: api/clientServiceCharge/
        [HttpGet]
        public GroupClientServiceChargeModel GetGroupService()
        {
            return new GroupClientServiceChargeModel();
            
        }

        // POST: api/clientServiceCharge/
        [HttpPost]
        public clientServiceCharge Post(clientServiceCharge input)
        {
            //Validate input
            validateInput(input);

            if (input.clientServiceChargeId > 0)
            {
                var toBeUpdated = le.clientServiceCharges
                    .FirstOrDefault(p => p.clientServiceChargeId == input.clientServiceChargeId);
                populateServiceChargeFields(toBeUpdated, input);
            }
            else
            {
                clientServiceCharge toBeSaved = new clientServiceCharge();
                populateServiceChargeFields(toBeSaved, input);
                le.clientServiceCharges.Add(toBeSaved);
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                //If saving fails, Log the the Exception and display message to user.
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            return input;
        }


        // POST: api/clientServiceCharge/
        [HttpPost]
        public bool PostMulti(ClientServiceChargeMultiModel input)
        {
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ct == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" + User.Identity.Name + ")");
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == input.chargeDate.Date
                && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")");
            }

            //Validate input
            var data = generateCharges(input);
            foreach (var charge in data)
            {
                validateInput(charge);
            }

            foreach (var charge in data)
            {
                if (charge.clientServiceChargeId > 0)
                {
                    var toBeUpdated = le.clientServiceCharges
                        .FirstOrDefault(p => p.clientServiceChargeId == charge.clientServiceChargeId);
                    populateServiceChargeFields(toBeUpdated, charge);
                }
                else
                {
                    clientServiceCharge toBeSaved = new clientServiceCharge();
                    populateServiceChargeFields(toBeSaved, charge);
                    le.clientServiceCharges.Add(toBeSaved);
                }

                try
                {
                    le.SaveChanges();
                }
                catch (Exception ex)
                {
                    //If saving fails, Log the the Exception and display message to user.
                    Logger.logError(ex);
                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                } 
            }
            
            return true;
        }


        private List<clientServiceCharge> generateCharges(ClientServiceChargeMultiModel input) 
        {
            var dataToReturn = new List<clientServiceCharge>();
            foreach (var payment in input.payments)
            {
                dataToReturn.Add(new clientServiceCharge 
                {
                    clientId = input.clientId,
                    chargeDate = input.chargeDate.Date,
                    chargeTypeId = payment.chargeTypeId,
                    chargeAmount = payment.amount
                });
            }
            return dataToReturn;
        }






        //populate clientCheck the fields to save
        private void populateServiceChargeFields(clientServiceCharge toBeSaved, clientServiceCharge input)
        {
            toBeSaved.clientId = input.clientId;
            toBeSaved.chargeDate = input.chargeDate;
            toBeSaved.chargeTypeId = input.chargeTypeId;
            toBeSaved.chargeAmount = input.chargeAmount;
            toBeSaved.posted = false;
            if (toBeSaved.clientServiceChargeId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            else
            {
                toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.modified = DateTime.Now;
            }

        }

        //populate clientCheck the fields to save
        private void validateInput(clientServiceCharge input)
        {

            if (input == null||input.posted)
            {
                StringBuilder errorToReturn = new StringBuilder();
                if (input == null)
                    errorToReturn.Append(error.InputDataEmpty);
                if (input.posted)
                    errorToReturn.Append(error.ChargePostedAlready);

                if (!String.IsNullOrEmpty(errorToReturn.ToString()))
                {
                    Logger.logError(errorToReturn.ToString());
                    throw new ApplicationException(errorToReturn.ToString());
                }
                
            }
        }


        [HttpPost]
        public bool PostGroupMulti(GroupClientServiceChargeModel input)
        {
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ct == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" + User.Identity.Name + ")");
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == input.chargeDate.Date
                && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")");
            }

            //Validate input
            var data = generateGroupCharges(input);

            foreach (var charge in data)
            {
                validateInput(charge);
            }

            foreach (var charge in data)
            {
                if (charge.clientServiceChargeId > 0)
                {
                    var toBeUpdated = le.clientServiceCharges
                        .FirstOrDefault(p => p.clientServiceChargeId == charge.clientServiceChargeId);
                    populateServiceChargeFields(toBeUpdated, charge);
                }
                else
                {
                    clientServiceCharge toBeSaved = new clientServiceCharge();
                    populateServiceChargeFields(toBeSaved, charge);
                    le.clientServiceCharges.Add(toBeSaved);
                }

                try
                {
                    le.SaveChanges();
                }
                catch (Exception ex)
                {
                    //If saving fails, Log the the Exception and display message to user.
                    Logger.logError(ex);
                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
                }
            }

            return true;
        }

        private List<clientServiceCharge> generateGroupCharges(GroupClientServiceChargeModel input)
        {
            var dataToReturn = new List<clientServiceCharge>();
            foreach (var payment in input.payments)
            {
                dataToReturn.Add(new clientServiceCharge
                {
                    clientId = payment.clientId,
                    chargeDate = input.chargeDate.Date,
                    chargeTypeId = payment.chargeTypeId,
                    chargeAmount = payment.amount
                });
            }
            return dataToReturn;
        }



    }
}
