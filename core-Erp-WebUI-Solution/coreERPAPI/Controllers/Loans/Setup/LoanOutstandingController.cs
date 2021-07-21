using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreData.Constants;
using coreERP;
using coreERP.Models;
using coreERP.Providers;
using coreERP.Models.Client;
using coreERP.Models.Loan;
using coreReports;
using coreErpApi.Controllers.Models;
using System.Globalization;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [AuthorizationFilter()]
    public class LoanOutstandingController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();
        HelperMethod helper;

        public LoanOutstandingController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            helper = new HelperMethod();
        }

        public LoanOutstandingController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/
        [HttpPost]
        public IEnumerable<vwOutstandingLoan> Get()
        {
            var outstanding = le.vwOutstandingLoans
                .OrderBy(p => p.loanGroupName)
                .ToList();            
            return outstanding;
        }

        //Get the Arrears for Logged-in user
        [HttpPost]
        public KendoResponse GetOutstanding([FromBody]KendoRequest req, DateTime date)
        {// get list of entries per pages and with filters

            //TODO: Sort and group all by Branch
            var currentUserName = User.Identity.Name.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(currentUserName))
            {
                throw new ArgumentException("No User Identified");
            }
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUserName);

            var groupIds = le.loanGroups
                .Where(p => p.staff.userName.ToLower().Trim() == currentUserName)
                .Select(p => p.loanGroupId).ToList();

            string order = "clientName";
            KendoHelper.getSortOrder(req, ref order);
            var endOfDayDate = date;
            endOfDayDate = endOfDayDate.Date.AddDays(1).AddSeconds(-1);
            reportEntities repEnt = new reportEntities();
            var arrears = new List<vwOutstandingLoanNew>();
            var arrearsResult = repEnt.spGetLoanArrearsWithDays(endOfDayDate)
                .Where(p => (p.Payable.Value - ((p.Paid ?? 0) + (p.WriteOffAmount ?? 0))) > 0).ToList();

            arrears= arrearsResult.Select(p => new vwOutstandingLoanNew
                {
                    outstanding = (p.Payable ?? 0) - ((p.WriteOffAmount ?? 0) + (p.Paid ?? 0)),
                    clientName = p.clientName,
                    loanNo = p.loanNo,
                    loanGroupName = p.loanGroupName,
                    LastRepaymentDate = p.LastRepaymentDate,
                    LastDueDate = p.LastDueDate,
                    disbursementDate = p.disbursementDate,
                    amountDisbursed = p.amountDisbursed.Value,
                    Payable = p.Payable.Value,
                    Paid = p.Paid.Value,
                    WriteOffDate = p.WriteOffDate,
                    WriteOffAmount = p.WriteOffAmount.Value,
                    loanGroupId = p.loanGroupId ?? 0,
                    clientID = p.clientID,
                    DaysDefault=p.daysDue
                })
                .Where(p => p.outstanding >= 1 )
                .OrderBy(p => p.loanGroupName)
                .ToList();

            foreach (var outStand in arrears)
            {
                outStand.BranchName = helper.GetBranchNameForClient(outStand.clientID);
                outStand.BranchId = helper.GetBranchIdForClient(outStand.clientID);

                if (outStand.loanGroupId != null && outStand.loanGroupId != 0)
                    outStand.Officer = helper.GetGroupOfficerName(outStand.loanGroupId.Value);
                else
                    outStand.Officer = "No Officer Assigned";

                if (string.IsNullOrWhiteSpace(outStand.loanGroupName))
                    outStand.loanGroupName = "No Group";

                outStand.ClientPhone = helper.GetClientPhoneNumber(outStand.clientID);
            }
            //Check for User Role
            if (!helper.IsOwner(currentUserName))
            {

                var userBranchId = helper.GetBranchIdForUser(currentUserName);
                if(isBranchAdmin)
                    arrears = arrears.Where(p => p.BranchId == userBranchId).ToList();
                else
                    arrears = arrears.Where(p => p.BranchId == userBranchId && groupIds.Contains(p.loanGroupId.Value)).ToList();
            }
            var query = arrears.OrderBy(p => p.BranchName).ThenBy(p => p.clientName).AsQueryable();

            string direction = "A";
            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                order = sort.field;
                if (sort.dir != "asc")
                {
                    direction = "D";
                }
            }
            

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();


            return new KendoResponse(data, query.Count());
        }



        [HttpPost]
        public KendoResponse GetOverPayments([FromBody]KendoRequest req, DateTime date)
        {// get list of entries per pages and with filters

            //TODO: Sort and group all by Branch
            var currentUserName = User?.Identity?.Name?.Trim()?.ToLower();

            string order = "clientName";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<vwOutstandingLoan>(req, parameters);
            var endOfDayDate = date;
            endOfDayDate = endOfDayDate.Date.AddDays(1).AddSeconds(-1);
            var result = le.vwOutstandingLoans
                .Where(p => p.LastDueDate <= endOfDayDate)
                .Select(p => new vwOutstandingLoanNew
                {
                    outstanding = (p.Paid ?? 0) - (p.Payable ?? 0.0),
                    clientName = p.clientName,
                    loanNo = p.loanNo,
                    loanGroupName = p.loanGroupName,
                    LastRepaymentDate = p.LastRepaymentDate,
                    LastDueDate = p.LastDueDate,
                    disbursementDate = p.disbursementDate,
                    amountDisbursed = p.amountDisbursed,
                    Payable = p.Payable,
                    Paid = p.Paid,
                    clientID = p.clientID
                })
                .Where(p => (p.Payable - p.Paid) < 0)
                .ToList();
            foreach (var outStand in result)
            {
                outStand.BranchName = helper.GetBranchNameForClient(outStand.clientID);
            }

            //Check for User Role
            if (!helper.IsOwner(currentUserName))
            {
                var userBranch = helper.GetBranchNameForLoggedInUser(currentUserName);
                result = result.Where(p => p.BranchName == userBranch).ToList();
            }
            var query = result.OrderBy(p => p.BranchName).ThenBy(p => p.clientName).AsQueryable();

            string direction = "A";
            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                order = sort.field;
                if (sort.dir != "asc")
                {
                    direction = "D";
                }
            }
            if (req != null && req.filter != null && req.filter.filters != null && req.filter.filters.Any())
            {
                //query = fttx_int_portal.Providers.Utility.Instance.getFilterExpression(req.filter.filters.First(), query);
            }

            if (req.sort != null && req.sort.Any())
            {
                //query = fttx_int_portal.Providers.Utility.Instance.getSortExpression(order, query, direction);
            }

            var data = query
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();


            return new KendoResponse(data, query.Count());
        }

        //GET ALL ARREARS for Admin/systems owner
        [HttpPost]
        public KendoResponse GetFullArrears([FromBody]KendoRequest req, DateTime date)
        {// get list of entries per pages and with filters

            var currentUserName = User?.Identity?.Name?.Trim()?.ToLower();
            //TODO: Sort and group all by Branch
            string order = "clientName";
            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<vwOutstandingLoan>(req, parameters);
            var isSysOwner =helper.IsOwner(currentUserName);
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUserName);

            if (!isSysOwner && !isBranchAdmin)
            {
                return new KendoResponse(new vwOutstandingLoanNew[0], 0);
            }
            var endOfDayDate = date;
            endOfDayDate = endOfDayDate.Date.AddDays(1).AddSeconds(-1);
            reportEntities repEnt = new reportEntities();
            var arrears = repEnt.spGetLoanArrearsWithDays(endOfDayDate)
            .Where(p =>
               ((p.Payable ?? 0) - ((p.Paid ?? 0) + (p.WriteOffAmount ?? 0))) > 0).ToList();

            var results=arrears.Select(p => new vwOutstandingLoanNew
                {
                    outstanding = (p.Payable ?? 0) - ((p.WriteOffAmount ?? 0) + (p.Paid ?? 0)),
                    clientName = p.clientName,
                    loanNo = p.loanNo,
                    loanGroupName = p.loanGroupName,
                    LastRepaymentDate = p.LastRepaymentDate,
                    LastDueDate = p.LastDueDate,
                    disbursementDate = p.disbursementDate,
                    amountDisbursed = p.amountDisbursed.Value,
                    Payable = p.Payable.Value,
                    Paid = p.Paid.Value,
                    WriteOffDate = p.WriteOffDate,
                    WriteOffAmount = p.WriteOffAmount.Value,
                    loanGroupId = p.loanGroupId,
                    clientID = p.clientID,
                    DaysDefault=p.daysDue
                })
                .Where(p => p.outstanding >= 1)
                .OrderBy(p => p.loanGroupName)
                .ToList();

            foreach (var loanOut in results)
            {
                if (loanOut.loanGroupId == null)
                {
                    if (string.IsNullOrWhiteSpace(loanOut.Officer))
                        loanOut.Officer = "No Officer Assigned";
                    if (string.IsNullOrWhiteSpace(loanOut.loanGroupName))
                        loanOut.loanGroupName = "No Group";
                }
                else
                {
                    //loanOut.Officer =helper.GetGroupOfficerName(loanOut.loanGroupId.Value);
                    loanOut.Officer = helper.GetGroupOfficerFullName(loanOut.loanGroupId.Value);
                    loanOut.OfficerUserName = helper.GetGroupOfficerUserName(loanOut.loanGroupId.Value);
                }
                loanOut.BranchName =helper.GetBranchNameForClient(loanOut.clientID);
                loanOut.BranchId = helper.GetBranchIdForClient(loanOut.clientID);
                loanOut.ClientPhone = helper.GetClientPhoneNumber(loanOut.clientID);
            }
            //Check for Branch Admin, to display only his branch's arrears
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUserName);
                results = results.Where(p => p.BranchId == userBranchId).ToList();
            }
            var query = results.OrderBy(p => p.BranchName).ThenBy(p => p.clientName).AsQueryable();

            string direction = "A";
            if (req != null && req.sort != null && req.sort.Any())
            {
                var sort = req.sort.First();
                order = sort.field;
                if (sort.dir != "asc")
                {
                    direction = "D";
                }
            }
            var data = query.Skip(req.skip).Take(req.take).ToArray();
            return new KendoResponse(data, query.Count());
        }


        [HttpGet]
        public IEnumerable<branch> GetBranches()
        {
            var branches = le.branches.OrderBy(p => p.branchName).ToList();
            var currentUserName = User?.Identity?.Name?.Trim()?.ToLower();

            //Check for User Role
            if (!helper.IsOwner(currentUserName))
            {
                var staffBranchId = le?.staffs?.FirstOrDefault(e => e.userName.ToLower() == currentUserName)?.branchID;
                if (staffBranchId != null)
                    branches = branches.Where(p => p.branchID == staffBranchId).ToList();
            }
            return branches;
        }

        [HttpGet]
        public List<ClientModel> GetClientsForOutstanding()
        {
            var currentUserName = User?.Identity?.Name?.Trim()?.ToLower();
            var isOwner = helper.IsOwner(currentUserName);
            var clients = le.clients.Select(p => new ClientModel
            {
                clientId = p.clientID,
                surName = (p.clientTypeID == 3 || p.clientTypeID == 4 || p.clientTypeID == 5) ?
                p.companyName : ((p.clientTypeID == 6) ? p.accountName : p.surName +
                    ", " + p.otherNames) + "(" + p.accountNumber + ")"
            }).OrderBy(p => p.surName).ToList();

            //Check for User Role
            if (!isOwner)
            {
                var staffBranchId = helper.GetBranchIdForUser(currentUserName); //le?.staffs?.FirstOrDefault(e => e.userName.ToLower() == currentUserName)?.branchID;
                if (staffBranchId != null)
                    clients = clients.Where(p => p.branchId == staffBranchId).OrderBy(p => p.surName).ToList();
            }
            return clients;
        }


        [HttpPost]
        public KendoResponse GetOutstandingLoans(LoanOutstandingInputModel model)
        {
            var currentUserName = User?.Identity?.Name?.Trim()?.ToLower();
            var isOwner = helper.IsOwner(currentUserName);
            var isBranchAdmin = helper.IsBranchAdminOwner(currentUserName);
            var endDate=model.EndDate.Date.AddDays(1).AddSeconds(-1);
            if (!isOwner && !isBranchAdmin)
            {
                return new KendoResponse(new LoanOutstandingModel[0], 0);
            }
            var loanOfficers = le.staffs
                   .Select(p => new StaffViewModel
                   {
                       staffId = p.staffID,
                       staffName = p.surName + " " + p.otherNames
                   })
                   .OrderBy(i => i.staffName)
                   .ToList();
            var oficerIds = loanOfficers.Select(r => r.staffId).ToList();
            var repEnt = new reportEntities();
            var outstandings = new List<LoanOutstandingModel>();
            var cl = le.clients.Where(p => (model.BranchID == null || p.branchID == model.BranchID)).ToList();
            if (model.ClientId == null)
            {
                var res = repEnt.spOutstanding(endDate).Where(p =>
                     (model.ExpiredFlag == 0 || (model.ExpiredFlag == 1 && p.expiryDate < endDate)
                     || (model.ExpiredFlag == 2 && p.expiryDate > endDate)) || oficerIds.Contains(p.StaffID)
                     && ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)) > 0).ToList();
                if (res.Count == 0)
                {
                    return new KendoResponse("There are no outstanding loans for the criteria specified.");
                }
                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                outstandings = res.Select(p => new LoanOutstandingModel
                {
                    ClientId = p.clientID,
                    ClientName = p.clientName,
                    LoanAmount = p.amountDisbursed,
                    LoanGroupName = string.IsNullOrWhiteSpace(p.LoanGroupName) ? "No Group" : p.LoanGroupName,
                    LoanId = p.loanID,
                    LoanNo = p.loanNo,
                    OutstandingAmount = ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)),
                    DaysDefault = p.daysDue,
                    ExpiryDate = p.expiryDate,
                    WriteOffAmount = p.writtenOff,
                    Collateral = p.fairValue,
                    DisbursementDate = p.disbursementDate
                    //Officer = string.IsNullOrWhiteSpace(loanOfficers?.FirstOrDefault(t => t.staffId == p.StaffID)?.staffName) ? "No Officer Assigned" : loanOfficers?.FirstOrDefault(t => t.staffId == p.StaffID)?.staffName

                })
                .Where(p => p.OutstandingAmount >= 1)
                .OrderBy(p => p.LoanGroupName).ThenBy(p => p.ClientName).ToList();
            }
            else
            {
                var res = repEnt.spOutstanding(endDate).Where(p => p.clientID == model.ClientId
                    && (model.ExpiredFlag == 0 || (model.ExpiredFlag == 1 && p.expiryDate < endDate)
                    || (model.ExpiredFlag == 2 && p.expiryDate > endDate)) || oficerIds.Contains(p.StaffID)
                    && ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)) > 0).ToList();
                if (res.Count == 0)
                {
                    return new KendoResponse("There are no outstanding loans for the criteria specified.");
                }
                for (int i = res.Count - 1; i >= 0; i--)
                {
                    if (cl.FirstOrDefault(p => p.clientID == res[i].clientID) == null)
                    {
                        res.Remove(res[i]);
                    }
                }
                outstandings = res.Select(p => new LoanOutstandingModel
                {
                    ClientId = p.clientID,
                    ClientName = p.clientName,
                    LoanAmount = p.amountDisbursed,
                    LoanGroupName = string.IsNullOrWhiteSpace(p.LoanGroupName) ? "No Group" : p.LoanGroupName,
                    LoanId = p.loanID,
                    LoanNo = p.loanNo,
                    OutstandingAmount = ((p.amountDisbursed + p.interest + p.fee) - (p.totalPaid + p.writtenOff)),
                    DaysDefault = p.daysDue,
                    ExpiryDate = p.expiryDate,
                    WriteOffAmount = p.writtenOff,
                    Collateral = p.fairValue,
                    DisbursementDate = p.disbursementDate
                   // Officer = string.IsNullOrWhiteSpace(loanOfficers.FirstOrDefault(t => t.staffId == p.StaffID)?.staffName) ? "No Officer Assigned" : loanOfficers.FirstOrDefault(t => t.staffId == p.StaffID)?.staffName

                })
                .Where(p => p.OutstandingAmount >= 1)
                .OrderBy(p => p.LoanGroupName).ThenBy(p => p.ClientName).ToList();
            }
            foreach (var outStanding in outstandings)
            {
                outStanding.BranchName =helper.GetBranchNameForClient(outStanding.ClientId);
                outStanding.BranchId = helper.GetBranchIdForClient(outStanding.ClientId);
                //Get the group officer name
                var clientGrpID = helper.GetGroupIdForClient(outStanding.ClientId);
                if (clientGrpID != null)
                    outStanding.Officer = helper.GetGroupOfficerFullName(clientGrpID.Value);
                else
                    outStanding.Officer = "No Officer Assigned";
            }
            //Check for User Role
            if (isBranchAdmin)
            {
                var userBranchId = helper.GetBranchIdForUser(currentUserName);
                outstandings = outstandings.Where(p => p.BranchId == userBranchId).ToList();
            }
            var query = outstandings.OrderBy(p => p.BranchName).ThenBy(p => p.ClientName).AsQueryable();

            
            var data = query.ToArray();
            return new KendoResponse(data, query.Count());
        }

    }
}
