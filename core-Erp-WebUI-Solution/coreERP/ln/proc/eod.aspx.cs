using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;

namespace coreERP.ln.proc
{
    public partial class eod : System.Web.UI.Page
    {
        private List<LoanEOM> loans;
        private List<SavingEOM> savings;
        private coreLoansEntities le;
        private IJournalExtensions journalextensions;
        private core_dbEntities ent;
        private List<SusuEOM> susu;
        private List<ExpiredSusuEOM> expiredSusu;
        private IDisbursementsManager disbMgr;

        protected void Page_Load(object sender, EventArgs e)
        {
            disbMgr = new DisbursementsManager();
            le = new coreLoansEntities();
            journalextensions = new JournalExtensions();
            ent = new core_dbEntities();

            LoadLoans();
        }

        protected void btnProc_Click(object sender, EventArgs e)
        {
            List<int> lstProcessed = new List<int>();
            List<int> lstSusuProcessed = new List<int>();
            List<int> lstExpiredSusuProcessed = new List<int>();
            List<int> lstSavingProcessed = new List<int>();
            
            loans.Clear();
            susu.Clear();
            expiredSusu.Clear();
            savings.Clear();

            if (dtStartDate.SelectedDate != null && dtEndDate.SelectedDate != null)
            {
                for (var date = dtEndDate.SelectedDate.Value; date >= dtStartDate.SelectedDate; date = date.AddDays(-1))
                {
                    var lns = le.loans.Where(p => (p.lastPenaltyDate == null || p.lastPenaltyDate < date) && (p.loanStatusID == 4 && p.balance > 10)).ToList();
                    foreach (var l in lns)
                    {
                        var lastPenaltyDate = ((l.lastPenaltyDate == null) ? l.disbursementDate.Value : l.lastPenaltyDate.Value);
                        if (lastPenaltyDate.AddMonths(1) <= date && lstProcessed.Contains(l.loanID)==false)
                        {
                            var ld = DateTime.Now;
                            var oneMonthAfterLPD = lastPenaltyDate.AddMonths(1);
                            var date2 = date.AddMonths(-1);
                            var rs = l.repaymentSchedules.Where(p => ((p.repaymentDate>= lastPenaltyDate))
                                && p.repaymentDate <= date && p.repaymentDate>= oneMonthAfterLPD && p.principalBalance + p.interestBalance >= 1)
                                .OrderByDescending(p => p.repaymentDate).FirstOrDefault();
                            int? scheduleID = null;
                            if (rs == null && date>= l.disbursementDate.Value.AddMonths((int)l.loanTenure))
                            {
                                ld = date;
                                rs = l.repaymentSchedules.Where(p => p.repaymentDate <= ld).OrderByDescending(p => p.repaymentDate).FirstOrDefault();
                            }
                            else if(rs!=null)
                            {
                                ld = rs.repaymentDate;
                                scheduleID = rs.repaymentScheduleID;
                            }
                            else
                            {
                                continue;
                            }
                            var oneMonthAgo = ld.AddMonths(-1);

                            var addInterest = 0.0;
                            var penInterest = 0.0;
                            var princNotPaid = 0.0;
                            var pr = l.repaymentSchedules.Where(p => p.repaymentDate <= oneMonthAgo);
                            if (pr.Count() > 0) princNotPaid = pr.Sum(p => p.principalBalance);
                            var intOuts = l.repaymentSchedules.Where(p => rs == null || p.repaymentDate <= oneMonthAgo).Sum(p => p.interestBalance);
                            var addIntOuts = l.repaymentSchedules.Where(p => rs == null || p.repaymentDate <= oneMonthAgo).Sum(p => p.additionalInterestBalance);

                            var expPmt = l.repaymentSchedules.Where(p => rs == null || p.repaymentDate <= oneMonthAgo)
                                .Sum(p => p.interestBalance+p.principalBalance);

                            var lt = le.tenors.OrderByDescending(p=> p.tenor1).FirstOrDefault(p => p.loanTypeID == l.loanTypeID && p.tenor1 <= l.loanTenure);
                            if (lt == null)
                            {
                                lt = le.tenors.OrderByDescending(p => p.tenor1).FirstOrDefault(p =>  p.tenor1 <= l.loanTenure);
                                if (lt == null)
                                {
                                    lt = le.tenors.OrderByDescending(p => p.tenor1).FirstOrDefault();
                                }
                            }

                            if (addIntOuts == null) { addIntOuts = 0; }
                            if (l.interestTypeID == 1 && l.disbursementDate.Value.AddMonths((int)l.loanTenure)>date)
                            {
                                addInterest = 0;
                                penInterest = (princNotPaid + intOuts) * (lt.defaultPenaltyRate.Value / 100.0);
                            }
                            else if ((l.interestTypeID == 6 || l.interestTypeID == 7 || l.interestTypeID == 1) 
                                && l.disbursementDate.Value.AddMonths((int)l.loanTenure) > date)
                            {
                                addInterest = (princNotPaid + intOuts) * (l.interestRate / 100.0);
                                penInterest = expPmt * (lt.defaultPenaltyRate.Value / 100.0);
                            }
                            else if ((l.interestTypeID == 6 || l.interestTypeID == 7 || l.interestTypeID==1  )
                                &&  l.disbursementDate.Value.AddMonths((int)l.loanTenure) <= date)
                            {
                                addInterest = (princNotPaid + intOuts) * (l.interestRate / 100.0);
                                penInterest = (princNotPaid + intOuts) * (lt.defaultPenaltyRate.Value / 100.0);
                            }                             
                            else
                            {
                                continue;
                            }

                            loans.Add(new LoanEOM
                            {
                                AccountNumber = l.client.accountNumber,
                                additionalInterest = addInterest, 
                                clientName = (l.client.clientTypeID == 3 || l.client.clientTypeID == 4 || l.client.clientTypeID == 5) ? l.client.companyName :
                                    ((l.client.clientTypeID == 6) ? l.client.accountName : l.client.surName +
                                    ", " + l.client.otherNames),
                                interestOutstanding = intOuts,
                                LoanID = l.loanID,
                                LoanNo = l.loanNo,
                                oldInterestBalance = intOuts,
                                oldPrincipalBalance = princNotPaid,
                                PenaltyDate = oneMonthAfterLPD,
                                ScheduleID = scheduleID,
                                newPenaltyAmount=penInterest
                            });
                            lstProcessed.Add(l.loanID);
                        }
                    }

                    var svs = le.savings.Where(p => p.principalBalance > 0 && (p.lastInterestDate == null || p.lastInterestDate < date)).ToList();
                    foreach (var ln in svs)
                    {
                        if (lstSavingProcessed.Contains(ln.savingID)) continue;
                        lstSavingProcessed.Add(ln.savingID);

                        DateTime? date2 = null;
                        if (ln.lastInterestDate == null)
                            date2 = ln.firstSavingDate.AddDays(1);
                        else
                            date2 = ln.lastInterestDate.Value.AddDays(1);

                        if (date2 < date)
                        {
                            var balanceAsAt = GetBalanceAsAt(date, ln.savingID);

                            var interest = 0.0;
                            interest = ((date-date2.Value).TotalDays / 365.0) * (ln.interestRate*12.0) 
                                * balanceAsAt / 100.0;
                            var cur = ent.currencies.FirstOrDefault(p => p.currency_id == ln.currencyID);

                            if (interest > 0.0)
                            {
                                savings.Add(new SavingEOM
                                {
                                    AccountNumber = ln.client.accountNumber,
                                    clientName = ln.client.surName + ", " + ln.client.otherNames,
                                    InterestAmount = interest,
                                    Date = date,
                                    NewPrincipalBalance = ln.principalBalance,
                                    OriginalPrincipalBalance = ln.principalBalance,
                                    OriginalInterestBalance = ln.interestBalance,
                                    SavingID = ln.savingID,
                                    SavingNo = ln.savingNo
                                });
                            }                            
                        }
                    } 
                       
                    var rent=new coreReports.reportEntities();
                    var ss = rent.getSusuAccountStatus(date, null).Where(p => p.statusID==3).ToList();
                    foreach (var s in ss)
                    {
                        if (lstSusuProcessed.Contains(s.susuAccountID) == false && le.susuAccounts.FirstOrDefault(p=> p.susuAccountID==s.susuAccountID).isDormant==false)
                        {
                            susu.Add(new SusuEOM
                            {
                                AccountNumber = s.accountNumber,
                                AmountContributed = s.allContributions,
                                ContributtionAmount = s.commissionAmount,
                                SusuAccountID = s.susuAccountID,
                                SusuAccountNo = s.susuAccountNo,
                                clientName = s.clientName,
                                DaysInDefault = s.daysDelayed,
                                IsDormant = true,
                                StatusDate = date
                            });
                            lstSusuProcessed.Add(s.susuAccountID);
                        }
                    }
                    var es = le.susuAccounts.Where(p => p.disbursementDate != null && p.dueDate < date && p.convertedToLoan == false && p.isDormant == true).ToList();
                    var lt2 = le.tenors.OrderByDescending(p => p.tenor1).FirstOrDefault(p => p.loanTypeID == 7);
                    if (lt2 == null)
                    {
                        lt2 = le.tenors.OrderByDescending(p => p.tenor1).FirstOrDefault(); 
                    }
                    foreach (var s in es)
                    {
                        if (lstExpiredSusuProcessed.Contains(s.susuAccountID) == false && s.susuContributions.Sum(p=> p.amount)< s.amountEntitled)
                        {
                            expiredSusu.Add(new ExpiredSusuEOM
                            {
                                AccountNumber = s.client.accountNumber,
                                AmountContributed = s.susuContributions.Sum(p=> p.amount),
                                ContributtionAmount = s.commissionAmount,
                                SusuAccountID = s.susuAccountID,
                                SusuAccountNo = s.susuAccountNo,
                                clientName = s.client.surName + ", "+ s.client.otherNames, 
                                StatusDate = date,
                                CommissionAmount=s.commissionAmount,
                                AmountCollected=s.netAmountEntitled,
                                InterestAmount=(lt2.defaultInterestRate.Value/100)*(s.amountEntitled-s.susuContributions.Sum(p=> p.amount)),
                                ExpiryDate=s.dueDate.Value,
                                InterestRate=lt2.defaultInterestRate.Value ,
                                AmountExpected=s.amountEntitled
                            });
                            lstExpiredSusuProcessed.Add(s.susuAccountID);
                        }
                    }
                }
                Session["loansAddInterest"] = loans;
                Session["susuEOM"] = susu;
                Session["ExpiredSusuEOM"] = expiredSusu;
                Session["savingInterest"] = savings;
                if (loans.Count > 0|| susu.Count>0 || expiredSusu.Count>0)
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }

                gridInterest.DataSource = loans;
                gridInterest.DataBind();

                gridDormant.DataSource = susu;
                gridDormant.DataBind();

                gridExpired.DataSource = expiredSusu;
                gridExpired.DataBind();

                gridSaving.DataSource = savings;
                gridSaving.DataBind();
            }
        }

        private void LoadLoans()
        {
            if (Session["loansAddInterest"]!=null)
            {
                loans = Session["loansAddInterest"] as List<LoanEOM>;
            }
            else
            {
                loans = new List<LoanEOM>();
                Session["loansAddInterest"] = loans;
            }
            if (Session["savingInterest"] != null)
            {
                savings = Session["savingInterest"] as List<SavingEOM>;
            }
            else
            {
                savings = new List<SavingEOM>();
                Session["savingInterest"] = savings;
            }
            if (Session["susuEOM"] != null)
            {
                susu = Session["susuEOM"] as List<SusuEOM>;
            }
            else
            {
                susu = new List<SusuEOM>();
                Session["susuEOM"] = susu;
            }
            if (Session["ExpiredSusuEOM"] != null)
            {
                expiredSusu = Session["ExpiredSusuEOM"] as List<ExpiredSusuEOM>;
            }
            else
            {
                expiredSusu = new List<ExpiredSusuEOM>();
                Session["ExpiredSusuEOM"] = expiredSusu;
            }
        }

        protected void gridInterest_Load(object sender, EventArgs e)
        {
            gridInterest.DataSource = loans;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (loans.Count > 0 || susu.Count > 0 || expiredSusu.Count > 0)
            {
                jnl_batch jb = null;
                var pro = ent.comp_prof.FirstOrDefault();
                foreach (var r in loans)
                {
                    var l = le.loans.FirstOrDefault(p => p.loanID == r.LoanID);
                    var ni = r.penaltyInterest;
                    if (r.penaltyInterest + r.additionalInterest > 0)
                    {
                        le.loanPenalties.Add(new loanPenalty
                        {
                            proposedAmount = 0,
                            penaltyBalance = r.penaltyInterest,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name,
                            loanID = r.LoanID,
                            penaltyDate = r.PenaltyDate,
                            penaltyFee = r.penaltyInterest
                        });
                        if (jb == null)
                        {
                            jb = journalextensions.Post("LN", l.loanType.accountsReceivableAccountID,
                                l.loanType.unearnedInterestAccountID, r.penaltyInterest + r.additionalInterest,
                                "Penalty & Additional Interest for Loan - " + l.client.surName + "," + l.client.otherNames,
                                pro.currency_id.Value, r.PenaltyDate, l.loanNo, ent, User.Identity.Name,
                                l.client.branchID);
                        }
                        else
                        {
                            var jb2 = journalextensions.Post("LN", l.loanType.accountsReceivableAccountID,
                                l.loanType.unearnedInterestAccountID, r.penaltyInterest + r.additionalInterest,
                                "Penalty & Additional Interest for Loan - " + l.client.surName + "," + l.client.otherNames,
                                pro.currency_id.Value, r.PenaltyDate, l.loanNo, ent, User.Identity.Name,
                                l.client.branchID);
                            var list = jb2.jnl.ToList();
                            jb.jnl.Add(list[0]);
                            jb.jnl.Add(list[1]);
                        }
                    }
                    var rs = l.repaymentSchedules.FirstOrDefault(p => p.repaymentScheduleID == r.ScheduleID);
                    if (rs == null)
                    {
                        rs = new repaymentSchedule
                        {
                            repaymentDate = r.PenaltyDate,
                            origInterestPayment = 0,
                            origPrincipalPayment = 0,
                            balanceBF = 0,
                            balanceCD = 0,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name,
                            proposedInterestWriteOff = 0,
                            origPrincipalBF = 0,
                            principalBalance = 0,
                            principalPayment = 0,
                            origPrincipalCD = 0,
                            interestBalance = 0,
                            interestWritenOff = 0,
                            interestPayment = 0,
                            edited = false,
                            penaltyAmount = r.penaltyInterest,
                            additionalInterestBalance = 0,
                            additionalInterest = 0
                        };
                        l.repaymentSchedules.Add(rs);
                    }
                    rs.penaltyAmount = rs.penaltyAmount + r.penaltyInterest;
                    rs.interestPayment = rs.interestPayment - rs.interestBalance + r.additionalInterest;
                    rs.interestBalance = r.additionalInterest;
                    if (rs.additionalInterestBalance == null) rs.additionalInterestBalance = 0;
                    rs.additionalInterest = r.additionalInterest;
                    rs.additionalInterestBalance = r.additionalInterest;
                    l.lastPenaltyDate = r.PenaltyDate;
                }
                foreach (var s in susu)
                {
                    var ss = le.susuAccounts.FirstOrDefault(p => p.susuAccountID == s.SusuAccountID);
                    if (ss != null)
                    {
                        ss.isDormant = s.IsDormant;
                    }
                }
                foreach (var s in expiredSusu)
                {
                    var ss = le.susuAccounts.FirstOrDefault(p => p.susuAccountID == s.SusuAccountID);
                    if (ss != null)
                    {
                        ss.convertedToLoan = true;
                        var t = le.tenors.FirstOrDefault(p => p.tenor1 == 7);
                        if (t == null)
                        {
                            t = le.tenors.FirstOrDefault();
                        }
                        var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 7);
                        var l = new coreLogic.loan
                        {
                            amountApproved = s.Balance,
                            amountRequested = s.Balance,
                            applicationDate = s.ExpiryDate,
                            processingFee = 0,
                            processingFeeBalance = 0,
                            applicationFee = 0,
                            balance = 0,
                            applicationFeeBalance = 0,
                            clientID = ss.client.clientID,
                            commission = 0,
                            commissionBalance = 0,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name,
                            creditOfficerNotes = "",
                            gracePeriod = t.defaultGracePeriod.Value,
                            interestRate = t.defaultInterestRate.Value,
                            interestTypeID = pro.defaultInterestTypeID,
                            loanNo = (new IDGenerator()).NewLoanNumber(ss.client.branchID.Value, ss.client.clientID),
                            loanStatusID = 3,
                            repaymentModeID = -1,
                            tenureTypeID = 1,
                            loanTenure = 0,
                            loanType = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 5),
                            loanTypeID = 5,
                            approvalComments = "",
                            invoiceNo = "",
                            addFeesToPrincipal = false,
                            finalApprovalDate = s.ExpiryDate,
                            amountDisbursed = 0,
                            approvedBy = User.Identity.Name,
                            checkedBy = User.Identity.Name,
                            disbursedBy = User.Identity.Name,
                            enteredBy = User.Identity.Name
                        };
                        int? bankID = null;
                        var cd = new coreLogic.cashierDisbursement
                        {
                            addFees = false,
                            amount = s.Balance,
                            bankID = bankID,
                            paymentModeID = 1,
                            posted = false,
                            txDate = s.ExpiryDate,
                            cashiersTill = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.ToLower().Trim()),
                            checkNo = "",
                            loan = l,
                            clientID = ss.client.clientID
                        };
                        le.cashierDisbursements.Add(cd);
                        le.loans.Add(l);
                        cd.client = ss.client;
                        cd.loan = l;
                        l.client = ss.client;
                        l.loanType = lt;
                        ss.loan = l;
                        le.Entry(l).State = System.Data.Entity.EntityState.Added;
                        le.Entry(cd).State = System.Data.Entity.EntityState.Added;
                        ss.interestAmount = s.InterestAmount;

                        disbMgr.CashierDisburse(le, l, ent, cd, User.Identity.Name, ss);
                    }
                }

                if (jb != null)
                {
                    ent.jnl_batch.Add(jb);
                }

            }

            foreach (var ln in savings)
            {
                var sv = le.savings.FirstOrDefault(p => p.savingID == ln.SavingID);
                if (ln.InterestAmount > 0)
                {
                    sv.savingDailyInterests.Add(new savingDailyInterest
                    {
                        interestDate = ln.Date,
                        applied = false,
                        balanceAsAt = ln.OriginalPrincipalBalance,
                        interestAmount = ln.InterestAmount,
                    });
                    sv.lastInterestDate = ln.Date;
                }
            }

            le.SaveChanges();

            var lst = le.savingDailyInterests.Where(p => p.applied == false).ToList();
            if (lst.Count > 0)
            {
                var firsttUnAccumDate =
                    lst.Min(p => p.interestDate);
                if (firsttUnAccumDate != null)
                {
                    var eomDate = (new DateTime(firsttUnAccumDate.Year, firsttUnAccumDate.Month, 1)).AddMonths(1).AddSeconds(-1).Date;
                    if (firsttUnAccumDate <= eomDate)
                    {
                        try
                        {
                            var unapp = le.savingDailyInterests.Where(p => p.applied == false && p.interestDate <= eomDate)
                                .Select(p => p.savingID)
                                .Distinct()
                                .ToList();
                            foreach (var una in unapp)
                            {
                                var totalUnApplied = le.savingDailyInterests.Where(p => p.savingID == una && p.interestDate <= eomDate)
                                    .Sum(p => p.interestAmount);

                                if (totalUnApplied > 0)
                                {
                                    var sv = le.savings.FirstOrDefault(p => p.savingID == una);
                                    var inte = new coreLogic.savingInterest
                                    {
                                        interestAmount = totalUnApplied,
                                        fromDate = firsttUnAccumDate,
                                        toDate = eomDate,
                                        creation_date = DateTime.Now,
                                        creator = "SYSTEM",
                                        fxRate = 1.0,
                                        interestDate = eomDate,
                                        principal = 0,
                                        proposedAmount = 0,
                                        localAmount = totalUnApplied,
                                        savingID = una,
                                        interestBalance = totalUnApplied
                                    };
                                    sv.savingInterests.Add(inte);

                                    sv.interestAccumulated = sv.interestAccumulated + totalUnApplied;
                                    sv.interestBalance = sv.interestBalance + totalUnApplied;
                                    sv.interestAuthorized = sv.interestAuthorized + totalUnApplied;

                                    foreach (var r in sv.savingDailyInterests.Where(p => p.savingID == una && p.applied == false && p.interestDate <= eomDate).ToList())
                                    {
                                        r.applied = true;
                                    }

                                    var pro = ent.comp_prof.FirstOrDefault();
                                    var jb2 = journalextensions.Post("LN", sv.savingType.interestExpenseAccountID.Value,
                                        sv.savingType.interestPayableAccountID, totalUnApplied,
                                        "Interest Accummulated on Deposits & Savings - " + sv.client.surName + "," + sv.client.otherNames,
                                        pro.currency_id.Value, eomDate, sv.savingNo, ent, "SYSTEM", sv.client.branchID);
                                    ent.jnl_batch.Add(jb2);
                                }
                            }
                            le.SaveChanges();
                            ent.SaveChanges();
                        }
                        catch (Exception xi)
                        {
                            //ExceptionManager.LogException(xi, "InterestModule.Main2");
                        }
                    }
                }

                Session["loansAddInterest"] = null;
                Session["susuEOM"] = null;
                Session["ExpiredSusuEOM"] = null;
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("End of Day Processed successfully!", ResolveUrl("/ln/proc/eod.aspx"), "coreERP©: Successful", IconType.ok);
            }
        }

        protected void gridDormant_Load(object sender, EventArgs e)
        {
            gridDormant.DataSource = susu;
        }

        protected void gridExpired_Load(object sender, EventArgs e)
        {
            gridExpired.DataSource = expiredSusu;
        }

        private double GetBalanceAsAt(DateTime date, int savingID)
        {
            var bal = 0.0;

            using (var rent = new coreReports.reportEntities())
            {
                bal = rent.vwSavingStatements.Where(p => p.loanID == savingID && p.date <= date)
                    .Sum(p => p.Dr - p.Cr);
                if (bal < 0) bal = 0;
            }

            return bal;
        }

        protected void gridSaving_Load(object sender, EventArgs e)
        {
            gridSaving.DataSource = savings;
        }
    }

    public class LoanEOM
    {
        public int LoanID { get; set; }
        public string  LoanNo { get; set; }
        public string AccountNumber { get; set; }
        public string clientName { get; set; }
        public double oldPrincipalBalance { get; set; }
        public double oldBalance { get { return oldPrincipalBalance + oldInterestBalance; } }
        public double interestOutstanding { get; set; }
        public double oldInterestBalance { get; set; }
        public double additionalInterest { get; set; }
        public double penaltyInterest { get; set; }
        public double newInterestBalance
        {
            get { return oldInterestBalance + additionalInterest; }
        }
        public double remainingBalance
        {
            get { return oldPrincipalBalance + newInterestBalance; }
        }
        public double newPenaltyAmount { get; set; }
        public int? ScheduleID { get; set; }
        public DateTime PenaltyDate { get; set; }
    }

    public class SusuEOM
    {
        public int SusuAccountID { get; set; }
        public string SusuAccountNo { get; set; }
        public string AccountNumber { get; set; }
        public string clientName { get; set; }
        public bool IsDormant { get; set; }
        public double ContributtionAmount { get; set; }
        public double AmountContributed { get; set; }
        public int DaysInDefault { get; set; }
        public DateTime StatusDate { get; set; }
    }
    public class ExpiredSusuEOM
    {
        public int SusuAccountID { get; set; }
        public string SusuAccountNo { get; set; }
        public string AccountNumber { get; set; }
        public string clientName { get; set; }
        public bool IsDormant { get; set; }
        public double ContributtionAmount { get; set; }
        public double AmountContributed { get; set; }
        public int DaysInDefault { get; set; }
        public DateTime StatusDate { get; set; }
        public double CommissionAmount { get; set; }
        public double InterestRate { get; set; }
        public double InterestAmount { get; set; }
        public double AmountCollected { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double AmountExpected { get; set; }
        public double Balance { get { return AmountExpected - AmountContributed; } }
    }
    public class SavingEOM
    {
        public int SavingID { get; set; }
        public string SavingNo { get; set; }
        public string AccountNumber { get; set; }
        public string clientName { get; set; }
        public double OriginalPrincipalBalance { get; set; }
        public double NewPrincipalBalance { get; set; }
        public DateTime Date { get; set; }
        public double OriginalInterestBalance { get; set; }
        public double InterestAmount { get; set; }       
    }
}