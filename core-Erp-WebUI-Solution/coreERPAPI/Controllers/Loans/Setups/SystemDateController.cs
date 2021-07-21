using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using coreData.Constants;
using coreERP.Providers;

namespace coreERP.Controllers.Loans.Setups
{
    [AuthorizationFilter()]
    public class SystemDateController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();
        core_dbEntities ent = new core_dbEntities();

        public SystemDateController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        [HttpGet]
        public IEnumerable<systemDate> Get()
        {
            var bas = le.systemDates.FirstOrDefault();
            bas = bas ?? new systemDate();

            return new List<systemDate> { bas };
        }

        [HttpPost]
        public KendoResponse GetKendoResponse([FromBody]KendoRequest req)
        {
            string order = "systemDateID";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<systemDate>(req, parameters);


            var bas = le.systemDates.FirstOrDefault();
            bas = bas ?? new systemDate();
            var dates = new List<systemDate> { bas };

            var query = dates.AsQueryable();

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [HttpPost]
        public systemDate Post(systemDate bas)
        {
            var sbas = le.systemDates.FirstOrDefault();

            if (sbas == null)
            {
                sbas = new systemDate();
                le.systemDates.Add(sbas);
            }
            sbas.loanSystemDate = bas.loanSystemDate;
            sbas.savingSystemDate = bas.savingSystemDate;
            sbas.depositSystemDate = bas.depositSystemDate;
            sbas.investmentSystemDate = bas.investmentSystemDate;
            sbas.susuSystemDate = bas.susuSystemDate;
            sbas.creditUnionSystemDate = bas.creditUnionSystemDate;
            sbas.accountsSystemDate = bas.accountsSystemDate;

            if (le.systemDates.Count() <= 0)
            {
                le.Entry(sbas).State = System.Data.Entity.EntityState.Modified;
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                throw new ApplicationException(x.Message);
            }
            return bas;
        }

        [HttpPost]
        public systemDate PostSystemDate(systemDate bas)
        {
            var sbas = le.systemDates.FirstOrDefault();

            if (sbas != null)
            {
                popuplateDetails(sbas, bas);
            }
            else
            {
                sbas = new systemDate();
                le.systemDates.Add(sbas);
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                throw new ApplicationException(x.Message);
            }
            return bas;
        }

        [HttpPut]
        public systemDate Put(systemDate bas)
        {
            var sbas = le.systemDates.FirstOrDefault();
            if (sbas == null)
            {
                sbas = new systemDate();
                le.systemDates.Add(sbas);
            }
            sbas.investmentSystemDate = bas.investmentSystemDate;
            sbas.susuSystemDate = bas.susuSystemDate;
            sbas.creditUnionSystemDate = bas.creditUnionSystemDate;
            sbas.accountsSystemDate = bas.accountsSystemDate;

            if (le.systemDates.Count() <= 0)
            {
                le.Entry(sbas).State = System.Data.Entity.EntityState.Modified;
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                throw new ApplicationException(x.Message);
            }
            return bas;
        }


        private void popuplateDetails(systemDate toBeSaved, systemDate input)
        {
            //Validate Input
            validateInput(toBeSaved, input);

            //Tarck loan date Changes
            if (input.loanSystemDate != null && toBeSaved.loanSystemDate != null)
                if (toBeSaved.loanSystemDate.Value != input.loanSystemDate.Value)
                {
                    loanSystemDateChange loanDateChange = new loanSystemDateChange
                    {
                        changeFrom = toBeSaved.loanSystemDate.Value,
                        changeTo = input.loanSystemDate.Value,
                        creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                        created = DateTime.Now
                    };
                    toBeSaved.loanSystemDateChanges.Add(loanDateChange);
                }
            toBeSaved.loanSystemDate = input.loanSystemDate;

            //Tarck Bank account date Changes
            if (input.savingSystemDate != null && toBeSaved.savingSystemDate != null)
                if (toBeSaved.savingSystemDate.Value != input.savingSystemDate.Value)
                {
                    savingSystemDateChange bankAccountDateChange = new savingSystemDateChange
                    {
                        changeFrom = toBeSaved.savingSystemDate.Value,
                        changeTo = input.savingSystemDate.Value,
                        creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                        created = DateTime.Now
                    };
                    toBeSaved.savingSystemDateChanges.Add(bankAccountDateChange);
                }
            toBeSaved.savingSystemDate = input.savingSystemDate;

            //Tarck Investment date Changes
            if (input.depositSystemDate != null && toBeSaved.depositSystemDate != null)
                if (toBeSaved.depositSystemDate.Value != input.depositSystemDate.Value)
                {
                    depositSystemDateChange depositDateChange = new depositSystemDateChange
                    {
                        changeFrom = toBeSaved.depositSystemDate.Value,
                        changeTo = input.depositSystemDate.Value,
                        creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                        created = DateTime.Now
                    };
                    toBeSaved.depositSystemDateChanges.Add(depositDateChange);
                }
            toBeSaved.depositSystemDate = input.depositSystemDate;

            //Tarck Company Investment date Changes
            if (input.investmentSystemDate != null && toBeSaved.investmentSystemDate != null)
                if (toBeSaved.investmentSystemDate.Value != input.investmentSystemDate.Value)
                {
                    investmentSystemDateChange investmentDateChange = new investmentSystemDateChange
                    {
                        changeFrom = toBeSaved.investmentSystemDate.Value,
                        changeTo = input.investmentSystemDate.Value,
                        creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                        created = DateTime.Now
                    };
                    toBeSaved.investmentSystemDateChanges.Add(investmentDateChange);
                }
            toBeSaved.investmentSystemDate = input.investmentSystemDate;
        }

        private void validateInput(systemDate toBeSaved, systemDate input)
        {
            if ((input.loanSystemDate == null && toBeSaved.loanSystemDate != null)
                || (input.depositSystemDate == null && toBeSaved.depositSystemDate != null)
                || (input.savingSystemDate == null && toBeSaved.savingSystemDate != null)
                || (input.investmentSystemDate == null && toBeSaved.investmentSystemDate != null)
                || (input.loanSystemDate != null && input.loanSystemDate.Value.Date > DateTime.Today)
                || (input.depositSystemDate != null && input.depositSystemDate.Value.Date > DateTime.Today)
                || (input.savingSystemDate != null && input.savingSystemDate.Value.Date > DateTime.Today)
                || (input.investmentSystemDate != null && input.investmentSystemDate.Value.Date > DateTime.Today)
                || (input.loanSystemDate != null && input.loanSystemDate.Value.Date <= DateTime.MinValue)
                || (input.depositSystemDate != null && input.depositSystemDate.Value.Date <= DateTime.MinValue)
                || (input.savingSystemDate != null && input.savingSystemDate.Value.Date <= DateTime.MinValue)
                || (input.investmentSystemDate != null && input.investmentSystemDate.Value.Date <= DateTime.MinValue))
            {
                StringBuilder errorMessage = new StringBuilder();

                if (input.loanSystemDate == null && toBeSaved.loanSystemDate != null)
                    errorMessage.Append(ErrorMessages.EmptyLoanDate);
                if (input.depositSystemDate == null && toBeSaved.depositSystemDate != null)
                    errorMessage.Append(ErrorMessages.EmptyInvestmentDate);
                if (input.savingSystemDate == null && toBeSaved.savingSystemDate != null)
                    errorMessage.Append(ErrorMessages.EmptySavingsDate);
                if (input.investmentSystemDate == null && toBeSaved.investmentSystemDate != null)
                    errorMessage.Append(ErrorMessages.EmptyCompanyInvestmentDate);
                if (input.loanSystemDate != null && input.loanSystemDate.Value.Date > DateTime.Today)
                    errorMessage.Append(ErrorMessages.FutureLoanDate);
                if (input.depositSystemDate != null && input.depositSystemDate.Value.Date > DateTime.Today)
                    errorMessage.Append(ErrorMessages.FutureInvestmentDate);
                if (input.savingSystemDate != null && input.savingSystemDate.Value.Date > DateTime.Today)
                    errorMessage.Append(ErrorMessages.FutureSavingsDate);
                if (input.investmentSystemDate != null && input.investmentSystemDate.Value.Date > DateTime.Today)
                    errorMessage.Append(ErrorMessages.FutureCompanyInvestmentDate);
                if (input.loanSystemDate != null && input.loanSystemDate.Value.Date <= DateTime.MinValue)
                    errorMessage.Append(ErrorMessages.InvalidLoanDate);
                if (input.depositSystemDate != null && input.depositSystemDate.Value.Date <= DateTime.MinValue)
                    errorMessage.Append(ErrorMessages.InvalidInvestmentDate);
                if (input.savingSystemDate != null && input.savingSystemDate.Value.Date <= DateTime.MinValue)
                    errorMessage.Append(ErrorMessages.InvalidSavingsDate);
                if (input.investmentSystemDate != null && input.investmentSystemDate.Value.Date <= DateTime.MinValue)
                    errorMessage.Append(ErrorMessages.InvalidCompanyInvestmentDate);
                throw new ApplicationException(errorMessage.ToString());
            }
        }
    }
}
