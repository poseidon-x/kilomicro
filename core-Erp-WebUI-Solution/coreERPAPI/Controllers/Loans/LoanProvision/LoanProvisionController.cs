using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using System.Text;
using System.Web.Http.Cors;
using coreData.Constants;
using coreErpApi.Controllers.Models;
using coreErpApi.Controllers.Models;

using coreERP;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class LoanProvisionController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        private string ErrorToReturn = "";

        public LoanProvisionController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanProvisionController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        [HttpPost]
        public provisionBatch InitializeProvision(customResquest input)
        {
            var date = new DateTime(input.provisionDate.Year, input.provisionDate.Month, 1).AddMonths(1).AddSeconds(-1);

            if (date <= DateTime.Now && le.provisionBatches.Any(p => p.provisionDate == date))//&& le.loanProvisions.Any(p => p.provisionDate == date))
            {
                coreReports.reportEntities rent = new coreReports.reportEntities();
                var proClses = le.provisionClasses.ToList();
                var vl2s = rent.GetvwLoans2(date).ToList();
                var provBatch = le.provisionBatches.Include(p => p.loanProvisions).FirstOrDefault(p => p.provisionDate == date);
                foreach (var vl2 in vl2s)
                {
                    if (vl2.principalBalance + vl2.interestBalance > 1)
                    {
                        coreLogic.provisionClass proCls = null;
                        foreach (var cls in proClses)
                        {
                            if (cls.maxDays >= vl2.daysDue && cls.minDays <= vl2.daysDue)
                            {
                                proCls = cls;
                                break;
                            }
                        }
                        if (proCls != null)
                        {
                            var provision = vl2.principalBalance * proCls.provisionPercent / 100.0;
                            var inc = provBatch.loanProvisions.FirstOrDefault(p => p.loanID == vl2.loanID);
                            if (inc == null)
                            {
                                inc = new coreLogic.loanProvision
                                {
                                    daysDue = vl2.daysDue,
                                    interestBalance = vl2.interestBalance,
                                    loanID = vl2.loanID,
                                    posted = false,
                                    principalBalance = vl2.principalBalance,
                                    proposedAmount = provision,
                                    provisionAmount = 0,
                                    provisionClassID = proCls.provisionClassID,
                                    provisionDate = date,
                                    securityValue = vl2.collateralValue,
                                    typeOfSecurity = vl2.collateralType,
                                    edited = false
                                };
                                provBatch.loanProvisions.Add(inc);
                            }
                            else if (!inc.edited)
                            {
                                inc.interestBalance = vl2.interestBalance;
                                inc.principalBalance = vl2.principalBalance;
                                inc.provisionClassID = proCls.provisionClassID;
                                inc.securityValue = vl2.collateralValue;
                                inc.typeOfSecurity = vl2.collateralType;
                                inc.proposedAmount = provision;
                                inc.daysDue = vl2.daysDue;
                            }
                        }
                    }
                }
                le.SaveChanges();

            }
            else
            {
                coreReports.reportEntities rent = new coreReports.reportEntities();
                var proClses = le.provisionClasses.ToList();
                var vl2s = rent.GetvwLoans2(date).ToList();

                provisionBatch provisionBatchToBeSaved = createNewProvisionBatch(date);
                
                foreach (var vl2 in vl2s)
                {
                    if (vl2.principalBalance + vl2.interestBalance > 1)
                    {
                        coreLogic.provisionClass proCls = null;
                        foreach (var cls in proClses)
                        {
                            if (cls.maxDays >= vl2.daysDue && cls.minDays <= vl2.daysDue)
                            {
                                proCls = cls;
                                break;
                            }
                        }
                        if (proCls != null)
                        {
                            var provision = vl2.principalBalance * proCls.provisionPercent / 100.0;
                            
                            var inc = new coreLogic.loanProvision
                            {
                                daysDue = vl2.daysDue,
                                interestBalance = vl2.interestBalance,
                                loanID = vl2.loanID,
                                posted = false,
                                principalBalance = vl2.principalBalance,
                                proposedAmount = provision,
                                provisionAmount = 0,
                                provisionClassID = proCls.provisionClassID,
                                provisionDate = date,
                                securityValue = vl2.collateralValue,
                                typeOfSecurity = vl2.collateralType,
                                edited = false
                            };
                            provisionBatchToBeSaved.loanProvisions.Add(inc);
                                
                        }
                    }
                }
                le.provisionBatches.Add(provisionBatchToBeSaved);

                try
                {
                    le.SaveChanges();
                }
                catch (Exception)
                {
                
                    throw new ApplicationException(ErrorMessages.ErrorInitializingProvision);
                }
            
            }

            var batch = le.provisionBatches
                .FirstOrDefault(p => p.provisionDate == date);

            //batch.loanProvisions = le.loanProvisions
            //        .Include(p => p.loan)
            //        .Include(p => p.loan.client)
            //        .Where(p => p.provisionBatchId == batch.provisionBatchId)
            //        .OrderBy(p => p.daysDue)
            //        //.ThenBy(p => p.loan.client.surName)
            //        //.ThenBy(p => p.loan.client.otherNames)
            //        //.ThenBy(p => p.loan.loanNo)
            //        .ToList();

            return batch;
        }

        
        [HttpPost]
        public KendoResponse GetProvision([FromBody]customResquest input)
        {
            var date = new DateTime(input.provisionDate.Year, input.provisionDate.Month, 1).AddMonths(1).AddSeconds(-1);

            if (date <= DateTime.Now && le.loanProvisions.Any(p => p.provisionDate == date))
            {
                coreReports.reportEntities rent = new coreReports.reportEntities();
                var proClses = le.provisionClasses.ToList();
                var vl2s = rent.GetvwLoans2(date).ToList();
                foreach (var vl2 in vl2s)
                {
                    if (vl2.principalBalance + vl2.interestBalance > 1)
                    {
                        coreLogic.provisionClass proCls = null;
                        foreach (var cls in proClses)
                        {
                            if (cls.maxDays >= vl2.daysDue && cls.minDays <= vl2.daysDue)
                            {
                                proCls = cls;
                                break;
                            }
                        }
                        if (proCls != null)
                        {
                            var provision = vl2.principalBalance*proCls.provisionPercent/100.0;
                            var inc = le.loanProvisions.FirstOrDefault(p => p.provisionDate == date
                                                                            && p.loanID == vl2.loanID);
                            if (inc == null)
                            {

                                inc = new coreLogic.loanProvision
                                {
                                    daysDue = vl2.daysDue,
                                    interestBalance = vl2.interestBalance,
                                    loanID = vl2.loanID,
                                    posted = false,
                                    principalBalance = vl2.principalBalance,
                                    proposedAmount = provision,
                                    provisionAmount = 0,
                                    provisionClassID = proCls.provisionClassID,
                                    provisionDate = date,
                                    securityValue = vl2.collateralValue,
                                    typeOfSecurity = vl2.collateralType,
                                    edited = false
                                };
                                le.loanProvisions.Add(inc);
                            }
                            else if (!inc.edited)
                            {
                                inc.interestBalance = vl2.interestBalance;
                                inc.principalBalance = vl2.principalBalance;
                                inc.provisionClassID = proCls.provisionClassID;
                                inc.securityValue = vl2.collateralValue;
                                inc.typeOfSecurity = vl2.collateralType;
                                inc.proposedAmount = provision;
                                inc.daysDue = vl2.daysDue;
                            }
                        }
                    }
                }
                le.SaveChanges();

            }
            else
            {
                coreReports.reportEntities rent = new coreReports.reportEntities();
                var proClses = le.provisionClasses.ToList();
                var vl2s = rent.GetvwLoans2(date).ToList();
                foreach (var vl2 in vl2s)
                {
                    if (vl2.principalBalance + vl2.interestBalance > 1)
                    {
                        coreLogic.provisionClass proCls = null;
                        foreach (var cls in proClses)
                        {
                            if (cls.maxDays >= vl2.daysDue && cls.minDays <= vl2.daysDue)
                            {
                                proCls = cls;
                                break;
                            }
                        }
                        if (proCls != null)
                        {
                            var provision = vl2.principalBalance*proCls.provisionPercent/100.0;
                            var inc = le.loanProvisions.FirstOrDefault(p => p.provisionDate == date
                                                                            && p.loanID == vl2.loanID
                                );

                            if (inc == null)
                            {
                                inc = new coreLogic.loanProvision
                                {
                                    daysDue = vl2.daysDue,
                                    interestBalance = vl2.interestBalance,
                                    loanID = vl2.loanID,
                                    posted = false,
                                    principalBalance = vl2.principalBalance,
                                    proposedAmount = provision,
                                    provisionAmount = 0,
                                    provisionClassID = proCls.provisionClassID,
                                    provisionDate = date,
                                    securityValue = vl2.collateralValue,
                                    typeOfSecurity = vl2.collateralType,
                                    edited = false
                                };
                                le.loanProvisions.Add(inc);
                            }
                            else if (!inc.edited)
                            {
                                inc.interestBalance = vl2.interestBalance;
                                inc.proposedAmount = vl2.provisionAmount;
                                inc.principalBalance = vl2.principalBalance;
                                inc.daysDue = vl2.daysDue;
                                inc.provisionClassID = proCls.provisionClassID;
                                inc.securityValue = vl2.collateralValue;
                                inc.typeOfSecurity = vl2.collateralType;
                                inc.posted = false;
                            }
                        }
                    }
                }
                le.SaveChanges();
            }
            var incs = le.loanProvisions
                    .Include(p => p.loan)
                    .Include(p => p.loan.client)
                    .Where(p => p.provisionDate == date)
                    .OrderBy(p => p.daysDue)
                    .ThenBy(p => p.loan.client.surName)
                    .ThenBy(p => p.loan.client.otherNames)
                    .ThenBy(p => p.loan.loanNo)
                    .
                    ToList();
            //return incs;


            string order = "daysDue";

            KendoHelper.getSortOrder(input, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<loanProvision>(input, parameters);

            var query = le.loanProvisions
                    .Include(p => p.loan)
                    .Include(p => p.loan.client)
                    .Where(p => p.provisionDate == date)
                    .OrderBy(p => p.daysDue)
                    .ThenBy(p => p.loan.client.surName)
                    .ThenBy(p => p.loan.client.otherNames)
                    .ThenBy(p => p.loan.loanNo).AsQueryable();
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                //query = query.Where<loanProvision>(whereClause,parameters.ToArray());
                query = query; //.Where(whereClause, parameters.ToArray());

            }

            var data = query
                //.OrderBy(order.ToString())
                .Skip(input.skip)
                .Take(input.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }





        [HttpPost]
        public KendoResponse GetBatchProvision([FromBody]ProvisionKendoRequest req)
        {
            string order = "loanNo";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            //var whereClause = KendoHelper.getWhereClause<loanProvision>(req, parameters);
            var provBatch = le.provisionBatches.Include(p => p.loanProvisions)
                .FirstOrDefault(p => p.provisionDate == req.provisionDate);

            var query = provBatch.loanProvisions.AsQueryable();
            //if (whereClause != null && whereClause.Trim().Length > 0)
            //{
            //    query = query.Where(whereClause, parameters.ToArray());
            //}

            var data = query
                .OrderBy(p=>p.loanID)
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [HttpPut]
        // PUT: api/brands/5
        public KendoResponse Put([FromBody]loanProvision value)
        {
            var toBeUpdated = le.loanProvisions.FirstOrDefault(p => p.loanProvisionID == value.loanProvisionID);

            toBeUpdated.provisionAmount = value.provisionAmount;
            toBeUpdated.edited = true;

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            

            return new KendoResponse { Data = new loanProvision[] { toBeUpdated } };
        }

        private provisionBatch createNewProvisionBatch(DateTime date)
        {
            return new provisionBatch
            {
                provisionYear = date.Year,
                provisionMonth = date.Month,
                provisionDate = date,
                edited = false,
                posted = false,
                reversed = false,
                initiatedBy = LoginHelper.getCurrentUser(new coreSecurityEntities()),
                initiationDate = DateTime.Now
            };
        }

    }
}
