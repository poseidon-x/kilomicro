//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Threading.Tasks;
//using System.Web.Http.Cors;
//using coreData.Constants;
//using coreData.ErrorLog;
//using coreErpApi.Controllers.Models;
//using coreReports.fa;

//namespace coreErpApi.Controllers.Controllers.Loans.Setup
//{
//    //[AuthorizationFilter()]
//    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
//    public class LoanGroupController : ApiController
//    {
//        IcoreLoansEntities le;
//        ErrorMessages error = new ErrorMessages();

//        public LoanGroupController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public LoanGroupController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/
//        public IEnumerable<loanGroup> Get()
//        {
//            return le.loanGroups
//                .Include(p => p.loanGroupClients)
//                .OrderBy(p => p.loanGroupName)
//                .ToList();
//        }

//        // GET: api/
//        [HttpGet]
//        public loanGroup Get(int id)
//        {
//            loanGroup value = le.loanGroups
//                    .Include(p => p.loanGroupClients)
//                    .FirstOrDefault(p => p.loanGroupId == id);

//            if (value == null)
//            {
//                value = new loanGroup();
//            }
//            return value;
//        }

//        // GET: api/
//        [HttpGet]
//        public disbursementDateModel GetDisbursementDate()
//        {
//            return new disbursementDateModel();
//        }

//        // GET: api/
//        [HttpPost]
//        public IEnumerable<loanGroup> GetGroupsByDate(disbursementDateModel disburseDate)
//        {
//            if (disburseDate == null) return null;
//            var  grps = le.loanGroups
//                .Where(p => p.loanGroupDayId == (int)disburseDate.date.DayOfWeek)
//                .Include(p => p.loanGroupClients)
//                .ToList();
//            return grps;
//        }




//        [AuthorizationFilter()]
//        [HttpPost]
//        public  loanGroup Post(loanGroup value)
//        {
//            if (value == null) return null;
//            //Validate the input value
//            //validateLoanGroup(value);

//            if (value != null) { 
//            if (value.loanGroupId > 0)
//            {
//                var toBeSaved =  le.loanGroups
//                    .Include(p => p.loanGroupClients)
//                    .FirstOrDefault(p => p.loanGroupId == value.loanGroupId);
//                populateFields(toBeSaved, value);
//            }
//            else
//            {
//                loanGroup toBeSaved = new loanGroup();
//                populateFields(toBeSaved, value);
//                le.loanGroups.Add(toBeSaved);
//            }
//            }

//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception x)
//            {
//                throw x;
//            }
//            return value;
//        }

//        private void populateFields(loanGroup target, loanGroup source)
//        {
//            target.loanGroupName = source.loanGroupName;
//            if (source.loanGroupId < 1)
//            {
//                target.loanGroupNumber = IDGenerator.newGroupLoanNumber(le);
//                target.loanGroupDayId = source.loanGroupDayId;
//                target.relationsOfficerStaffId = source.relationsOfficerStaffId;
//                target.leaderClientId = source.leaderClientId;
//                target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                target.created = DateTime.Now;
//            }
//            else
//            {
//                target.loanGroupDayId = source.loanGroupDayId;
//                target.relationsOfficerStaffId = source.relationsOfficerStaffId;
//                target.leaderClientId = source.leaderClientId;
//                target.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                target.modified = DateTime.Now;
//            }

//            if(source.loanGroupClients.Count < 5) 
//                throw new ApplicationException(error.LoanGroupClientsBelowMin);

//            foreach (var client in source.loanGroupClients)
//            {
//                //Remove deleted clients from group
//                var sourceClientIds = source.loanGroupClients.Select(p => p.loanGroupClientId).ToList();
//                foreach (var cl in target.loanGroupClients)
//                {
//                    if (!sourceClientIds.Contains(cl.loanGroupClientId))
//                        le.loanGroupClients.Remove(cl);
//                }

//                if (client.loanGroupClientId < 1)
//                {
//                    var tobeSaved = new loanGroupClient();
//                    populateGroupClientFields(tobeSaved, client);
//                    target.loanGroupClients.Add(tobeSaved);
                
//                }

                
//            }

//        }

//        private void populateGroupClientFields(loanGroupClient target, loanGroupClient source)
//        {
//            target.clientId = source.clientId;
//            target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            target.created = DateTime.Now;
//        }

//        [HttpDelete]
//        // DELETE: api/
//        public void Delete(loanGroup input)
//        {
//            loanGroup forDelete = le.loanGroups
//                .Include(p => p.loanGroupClients)
//                .FirstOrDefault(p => p.loanGroupId == input.loanGroupId);
//            if (forDelete != null)
//            {
//                foreach (var client in forDelete.loanGroupClients)
//                {
//                    le.loanGroupClients.Remove(client);
//                }
//                le.loanGroups.Remove(forDelete);
//                try
//                {
//                    le.SaveChanges();
//                }
//                catch (Exception ex)
//                {
//                    Logger.logError(ex);
//                    throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//                }
//            }
//        }


         
//    }
//}
