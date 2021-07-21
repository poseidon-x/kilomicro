using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http; 
using iTextSharp.text;
using iTextSharp.text.pdf;
using coreERP;
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;
using System.Text;
using coreData.Constants;
using coreERP.Models.Cashier;

namespace coreERP.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class CashierTillDayController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public CashierTillDayController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        
        [HttpGet]
        public CashierTillRangeModel Get()
        {
            return new CashierTillRangeModel();
        }

        //Post Cashier Tiil day
        [HttpPost]
        public string OpenTillRange(CashierTillRangeModel input)
        {
            validateTillRangeInput(input);

            string creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var cashOpenTill = le.cashiersTillDays.Where(p => p.cashiersTillID == input.cashierTillId
                                           && (p.tillDay >= input.startDate && p.tillDay <= input.endDate))
                                           .ToList();

            DateTime currentDate = input.startDate;
            StringBuilder alreadyOpenTillDays = new StringBuilder();

            do
            {
                var tillDay = cashOpenTill.FirstOrDefault(p => p.tillDay.Date == currentDate.Date);
                if (tillDay == null)
                {
                    cashiersTillDay tillToBeSaved = new cashiersTillDay
                    {
                        cashiersTillID = input.cashierTillId,
                        open = true,
                        tillDay = currentDate.Date,
                        creation_date = DateTime.Now,
                        creator = creator
                    };
                    le.cashiersTillDays.Add(tillToBeSaved);
                }
                else
                {
                    if (!tillDay.open)
                    {
                        tillDay.open = true;
                        tillDay.last_modifier = creator;
                        tillDay.modification_date = DateTime.Now;
                    }
                    else
                    {
                        alreadyOpenTillDays.Append(tillDay.tillDay.ToString("dd-MMM-yyyy") + "<br />");
                    }                    
                }
                currentDate = currentDate.AddDays(1);
            } while (currentDate.Date <= input.endDate.Date);

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            return alreadyOpenTillDays.ToString();
        }

        //Post Cashier Tiil day
        [HttpPost]
        public string CloseTillRange(CashierTillRangeModel input)
        {
            validateTillRangeInput(input);

            string modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());

            var cashTillday = le.cashiersTillDays.Where(p => p.cashiersTillID == input.cashierTillId
                                           && (p.tillDay >= input.startDate && p.tillDay <= input.endDate))
                                           .ToList();

            foreach (var tillDay in cashTillday)
            {
                if (tillDay.open)
                {
                    tillDay.open = false;
                    tillDay.last_modifier = modifier;
                    tillDay.modification_date = DateTime.Now;
                }                
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            return "Till successfully close";
        }

        //Validate OpenTillRange Input
        private void validateTillRangeInput(CashierTillRangeModel input)
        {
            if (input == null || !CashierExist(input.cashierTillId) || input.startDate == DateTime.MinValue
                || input.startDate > DateTime.Today || input.endDate == DateTime.MinValue
                || input.endDate > DateTime.Today)
            {
                StringBuilder errors = new StringBuilder();
                if (input == null)
                    errors.Append(ErrorMessages.InvalidInput);
                if (!CashierExist(input.cashierTillId))
                    errors.Append(ErrorMessages.InvalidCashierSelected);
                if (input.startDate == DateTime.MinValue)
                    errors.Append(ErrorMessages.InvalidStartDate);
                if (input.startDate > DateTime.Today)
                    errors.Append(ErrorMessages.FutureStartDate);
                if (input.endDate == DateTime.MinValue)
                    errors.Append(ErrorMessages.InvalidEndDate);
                if (input.endDate > DateTime.Today)
                    errors.Append(ErrorMessages.FutureEndDate);
                throw new ApplicationException(errors.ToString());
            }
        }

        private bool CashierExist(int id)
        {
            if (le.cashiersTills.Any(p => p.cashiersTillID == id))
            {
                return true;
            }
            return false;
        }

        


    }
}
