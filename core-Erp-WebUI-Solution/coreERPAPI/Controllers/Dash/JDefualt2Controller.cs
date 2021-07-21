using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using coreERP.Models;
using coreERP.Models.DashAppModel;
using System.Linq.Dynamic;
using System.Data.Entity;
using coreReports;

namespace coreERP.Controllers.Dash
{
    public class JDefault2Controller : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public JDefault2Controller()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/relationType
        [HttpGet]
        [HttpPost]
        public KendoResponse GetDue([FromBody]KendoRequest req, long? id)
        {
            string order = "dateDue";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<DashViewModel>(req, parameters);

            var query = (from ls in le.repaymentSchedules
                       join l in le.loans on ls.loanID equals l.loanID
                       join c in le.clients on l.clientID equals c.clientID
                       join s in le.staffs on l.staffID equals s.staffID into stf
                       from st in stf.DefaultIfEmpty()
                       where ((ls.interestBalance > 1 || ls.principalBalance > 1)
                                && (ls.repaymentDate <= DateTime.Now))
                                && (l.loanStatusID != 7)
                                && (l.loanStatusID > 3)
                                && (
                                    (st != null && st.userName.Trim().ToLower() == User.Identity.Name.Trim().ToLower())
                                    || (true == true)
                                  )
                            && (id == null || id == -1 || c.branchID == id)
                       orderby ls.repaymentDate descending
                       select new DashViewModel
                       {
                           clientID = c.accountNumber,
                           clientName = c.surName + ", " + c.otherNames,
                           amountDue = ls.principalBalance + ls.interestBalance,
                           dateDue = ls.repaymentDate,
                           loanID=l.loanID,
                           staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                           loanNo = l.loanNo,
                       }).ToList();
            
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray()).ToList();
            }

            var data = query 
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take) 
                .ToArray();

            return new KendoResponse(data.ToArray(), query.Count()); 
        }

        // GET: api/DashAppModel
        [HttpGet]
        [HttpPost]
        public KendoResponse GetApp([FromBody]KendoRequest req, long? id)
        {
            string order = "applicationDate";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<DashAppModel>(req, parameters);

            var query = (
                        from l in le.loans
                        join c in le.clients on l.clientID equals c.clientID
                        join s in le.staffs on l.staffID equals s.staffID into stf
                        from st in stf.DefaultIfEmpty()
                        where l.loanStatusID <= 2
                            && (id == null || id == -1 || c.branchID == id)
                        select new DashAppModel
                        {
                            clientID = c.accountNumber,
                            clientName = c.surName + ", " + c.otherNames,
                            amountRequested = l.amountRequested,
                            applicationDate = l.applicationDate,
                            loanID = l.loanID,
                            categoryID = c.categoryID.Value,
                            staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                            loanNo = l.loanNo,
                        }).ToList();

            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray()).ToList();
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data.ToArray(), query.Count()); 
        }

        // GET: api/DashUndModel
        [HttpGet]
        [HttpPost]
        public KendoResponse GetUnd([FromBody]KendoRequest req, long? id)
        {
            string order = "finalApprovalDate";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<DashUndModel>(req, parameters);

            var query = (
                         from l in le.loans
                         join c in le.clients on l.clientID equals c.clientID
                         join s in le.staffs on l.staffID equals s.staffID into stf
                         from st in stf.DefaultIfEmpty()
                         where l.loanStatusID == 3 && l.amountApproved > 0
                            && (id == null || id == -1 || c.branchID == id)
                         select new DashUndModel
                         {
                             clientID = c.accountNumber,
                             clientName = c.surName + ", " + c.otherNames,
                             amountRequested = l.amountRequested,
                             applicationDate = l.applicationDate,
                             amountApproved = l.amountApproved,
                             finalApprovalDate = l.finalApprovalDate.Value,
                             categoryID = c.categoryID.Value,
                             loanID = l.loanID,
                             staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                             loanNo = l.loanNo,
                         }).ToList();

            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray()).ToList();
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data.ToArray(), query.Count()); 
        }

        // GET: api/DashTopTenModel
        [HttpGet]
        [HttpPost]
        public KendoResponse GetTopTen([FromBody]KendoRequest req, long? id)
        {
            string order = "totalDue DESC";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<DashTopTenModel>(req, parameters);

            var query =
                (from l in le.loans
                    from c in le.clients
                    join r in le.loanRepayments on l.loanID equals r.loanID into lrs
                    from lr in lrs.DefaultIfEmpty()
                    join s in le.staffs on l.staffID equals s.staffID into stf
                    from st in stf.DefaultIfEmpty()
                    where c.clientID == l.clientID && l.balance > 20
                          && l.loanStatusID == 4
                          && l.disbursementDate != null
                          && l.finalApprovalDate != null
                          && (id == null || id == -1 || c.branchID == id)
                    group lr by new
                    {
                        lr.loanID,
                        c.accountNumber,
                        c.surName,
                        c.otherNames,
                        l.applicationDate,
                        l.finalApprovalDate,
                        l.disbursementDate,
                        l.amountApproved,
                        l.amountDisbursed,
                        c.categoryID,
                        l.amountRequested,
                        l.loanNo,
                        staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                    }
                    into lrg
                    select new DashTopTenModel
                    {
                        clientID = lrg.Key.accountNumber,
                        clientName = lrg.Key.surName + ", " + lrg.Key.otherNames,
                        amountRequested = lrg.Key.amountRequested,
                        applicationDate = lrg.Key.applicationDate,
                        amountApproved = lrg.Key.amountApproved,
                        finalApprovalDate = lrg.Key.finalApprovalDate.Value,
                        categoryID = (int) lrg.Key.categoryID,
                        disbursementDate = lrg.Key.disbursementDate.Value,
                        amountDisbursed = lrg.Key.amountDisbursed,
                        amountPaid = (lrg.Any() ? lrg.Sum(p => p.principalPaid + p.interestPaid) : 0),
                        lastPaymentDate = (lrg.Any() ? lrg.Max(p => p.repaymentDate) : lrg.Key.disbursementDate.Value),
                        totalDue =
                            lrg.Key.amountDisbursed - (lrg.Any() ? lrg.Sum(p => p.principalPaid + p.interestPaid) : 0),
                        loanID = lrg.Key.loanNo,
                        staffName = lrg.Key.staffName,
                        loanNo = lrg.Key.loanNo,
                    }).OrderByDescending(p => p.totalDue).ToList();

            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray()).ToList();
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            foreach(var record in data){
                record.totalDue = GetBalanceAsAt(record.loanID);

                var cp = le.clientPhones.Include(p => p.phone).FirstOrDefault(p => p.clientID == record.clientPKID);

                var ce = le.clientEmails.Include(p => p.email).FirstOrDefault(p => p.clientID == record.clientPKID);

                if (cp != null)
                {
                    record.phoneNumber = cp.phone.phoneNo;

                }

                if (ce != null)
                {
                    record.email = ce.email.emailAddress;
                }
            }
            return new KendoResponse(data.ToArray(), query.Count()); 
        }


        // GET: api/DashOverMaturedInvestment
        [HttpGet]
        [HttpPost]
        public KendoResponse GetOverMatureInvestment([FromBody]KendoRequest req, long? id)
        {
            string order = "maturityDate";
            var today = DateTime.Today;

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<DashOverMaturedInvestmentModel>(req, parameters);

            var query =
               (from i in le.deposits
                from c in le.clients 
                join s in le.staffs on i.staffID equals s.staffID into stf
                from st in stf.DefaultIfEmpty()
                where (
                    c.clientID == i.client.clientID && (i.maturityDate < today && i.principalBalance > 0)
                    && (id == null || id == -1 || c.branchID == id)
                    && (i.principalBalance>1)
                )
                select new DashInvestmentModel
                {
                    clientID = c.accountNumber,
                    clientName = c.surName + ", " + c.otherNames,
                    investmentId = i.depositID,
                    investmentNo = i.depositNo,
                    amountDeposited = i.amountInvested,
                    interestAccrued = i.interestAccumulated,
                    firstDepositDate = i.firstDepositDate,
                    maturityDate = i.maturityDate.Value,
                    interestRate = i.interestRate,
                    staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                }).OrderByDescending(p => p.maturityDate).ToList();

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray()).ToList();
            }

            return new KendoResponse(data.ToArray(), query.Count());
        }

        // GET: api/DashTopTenModel
        [HttpGet]
        [HttpPost]
        public KendoResponse GetMatureInvestment([FromBody]KendoRequest req, long? id)
        {
            string order = "maturityDate";
            var startOfThisWeek = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek));
            var endOfThisWeek = startOfThisWeek.AddDays(6);

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<DashInvestmentModel>(req, parameters);

            var query =
               (from i in le.deposits
                from c in le.clients
                join s in le.staffs on i.staffID equals s.staffID into stf
                from st in stf.DefaultIfEmpty()
                where (
                    c.clientID == i.client.clientID && (i.maturityDate >= startOfThisWeek && i.maturityDate <= endOfThisWeek)
                    && (id == null || id == -1 || c.branchID == id)
                    && (i.principalBalance > 1)
                )
                select new DashInvestmentModel
                {
                    clientID = c.accountNumber,
                    clientName = c.surName + ", " + c.otherNames,
                    investmentId = i.depositID,
                    investmentNo = i.depositNo,
                    amountDeposited = i.amountInvested,
                    interestAccrued = i.interestAccumulated,
                    firstDepositDate = i.firstDepositDate,
                    maturityDate = i.maturityDate.Value,
                    interestRate = i.interestRate,
                    staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                }).OrderByDescending(p => p.maturityDate).ToList();

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray()).ToList();
            }

            return new KendoResponse(data.ToArray(), query.Count());
        }

        // GET: api/DashOverMaturedInvestment
        [HttpGet]
        [HttpPost]
        public KendoResponse GetTopTenInvestors([FromBody]KendoRequest req, long? id)
        {
            string order = "currentBalance DESC";
            var today = DateTime.Today;

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<TopTenInvestorsModel>(req, parameters);

            var query =
               (from i in le.deposits
                from c in le.clients
                join s in le.staffs on i.staffID equals s.staffID into stf
                from st in stf.DefaultIfEmpty()
                where (
                    c.clientID == i.client.clientID && (i.maturityDate < today && i.principalBalance > 0)
                    && (id == null || id == -1 || c.branchID == id)
                    && (i.principalBalance > 1)
                )
                select new TopTenInvestorsModel
                {
                    clientID = c.accountNumber,
                    clientName = c.surName + ", " + c.otherNames,
                    investmentNo = i.depositNo,
                    amountInvested = i.amountInvested,
                    interestAccrued = i.interestAccumulated,
                    firstDepositDate = i.firstDepositDate,
                    principalBalance=i.principalBalance,
                    interestBalance =i.interestBalance ,
                    amountWithdrawn = i.amountInvested + i.interestAccumulated - i.principalBalance - i.interestBalance,
                    currentBalance = i.principalBalance + i.interestBalance,

                    staffName = ((st == null) ? "" : st.surName + ", " + st.otherNames),
                    clientPKID=c.clientID,
                }).OrderByDescending(p => p.firstDepositDate).ToList();

            var data = query
                .ToArray();

            foreach (TopTenInvestorsModel record in data)
            {
                var cp = le.clientPhones.Include(p=>p.phone).FirstOrDefault(p=>p.clientID==record.clientPKID);

                var ce = le.clientEmails.Include(p=>p.email).FirstOrDefault(p => p.clientID == record.clientPKID);

                if (cp != null)
                {
                    record.phoneNumber = cp.phone.phoneNo;
                  
                }

                if(ce != null){
                    record.email = ce.email.emailAddress;
                } 
            }  

            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = data.Where(whereClause, parameters.ToArray()).ToList();
            }

            data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data.ToArray(), query.Count());
        }

        private double GetBalanceAsAt(string loanNo)
        {
            var bal = 0.0;
            DateTime date = DateTime.Now;
            using (var rent = new reportEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var rs = rent.vwLoanActualSchedules.Where(p => p.loanNo == loanNo && p.date < date
                        ).ToList();
                    var rs2 = le.loanRepayments.Where(p => p.loan.loanNo == loanNo && p.repaymentDate <= date
                        && (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 || p.repaymentTypeID == 3)).ToList();
                    bal = rent.vwLoanActualSchedules.Where(p => p.loanNo == loanNo)
                        .Max(p => p.amountDisbursed)-((rs2.Count > 0) ?rs2.Sum(p => p.principalPaid) : 0.0);
                    if (bal < 0)
                    {
                        bal = 0;
                    }
                }
            }

            return bal;
        }
    }
}
