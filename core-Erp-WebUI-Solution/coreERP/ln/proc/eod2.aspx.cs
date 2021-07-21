using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using coreReports;

namespace coreERP.ln.proc
{
    public partial class eod2 : System.Web.UI.Page
    {
        private List<LoanEOM> loans;
        private coreLoansEntities le;
        private IJournalExtensions journalextensions;
        private core_dbEntities ent;
        private IreportEntities _rent;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLoansEntities();
            journalextensions = new JournalExtensions();
            ent = new core_dbEntities();
            _rent = new reportEntities();

            LoadLoans();
        }

        protected void btnProc_Click(object sender, EventArgs e)
        {
            List<int> lstProcessed = new List<int>();
            
            loans.Clear();

            if (dtStartDate.SelectedDate != null && dtEndDate.SelectedDate != null)
            {
                var lns = le.loans.Where(p => p.amountDisbursed > 0 && p.disbursementDate != null).ToList();
                foreach (var ln in lns)
                {
                    var date = dtEndDate.SelectedDate.Value;
                    if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date.AddDays(-1)))
                        continue;
                    var lastPenaltyDate = ln.lastPenaltyDate ??
                                          ln.disbursementDate.Value.AddMonths(1).AddDays(-1);
                    if ((lastPenaltyDate.AddDays(1) <= date || lastPenaltyDate == ln.disbursementDate ||
                         ln.lastPenaltyDate == null))
                    {
                        var nextPenaltyDate = lastPenaltyDate.AddDays(1);

                        var rss = ln.repaymentSchedules
                            .Where(p => (
                                (p.repaymentDate <= nextPenaltyDate))
                                        && (p.principalBalance > 1 || p.interestBalance > 1)
                            )
                            .OrderBy(p => p.repaymentDate)
                            .ToList();
                        foreach (var rs in rss)
                        {
                            string scheduleId = rs.repaymentScheduleID.ToString();
                            var addInterest = 0.0;
                            var tenor = le.tenors.FirstOrDefault(p => p.loanTypeID == ln.loanTypeID) ??
                                        le.tenors.First();

                            DateTime? firstChargeDate = null;

                            var bouncedPen = ln.loanPenalties
                                .Where(p => p.penaltyTypeId == 2 && p.penaltyDate < nextPenaltyDate &&
                                            p.penaltyDate > rs.repaymentDate && p.penaltyBalance > 0.1
                                            && rs.repaymentDate <= nextPenaltyDate)
                                .ToList();
                            var bouncedPenAmount = 0.0;
                            if (bouncedPen.Count > 0)
                            {
                                firstChargeDate = bouncedPen.Min(p => p.penaltyDate);
                                bouncedPenAmount = bouncedPen.Sum(p => p.penaltyBalance);
                            }

                            addInterest = (rs.principalBalance + rs.interestBalance +
                                           (firstChargeDate != null && firstChargeDate <= nextPenaltyDate &&
                                            rs.repaymentDate <= firstChargeDate
                                               ? bouncedPenAmount
                                               : 0))
                                          *(tenor.defaultPenaltyRate.Value/100.0);
                            loanPenalty firstPenalty = ln.loanPenalties
                                .OrderBy(p => p.penaltyDate)
                                .FirstOrDefault(p => p.penaltyDate == rs.repaymentDate
                                                     && p.last_modifier == scheduleId
                                                     && (p.penaltyTypeId == null || p.penaltyTypeId == 1));
                            if (firstPenalty == null)
                            {
                                loans.Add(new LoanEOM
                                {
                                    additionalInterest = addInterest,
                                    AccountNumber = ln.client.accountNumber,
                                    newPenaltyAmount = addInterest,
                                    clientName = ln.client.surName + ", " + ln.client.otherNames,
                                    oldInterestBalance = rs.interestBalance,
                                    interestOutstanding = rs.interestBalance,
                                    LoanID = ln.loanID,
                                    LoanNo = ln.loanNo,
                                    ScheduleID = rs.repaymentScheduleID,
                                    penaltyInterest = 0,
                                    PenaltyDate = nextPenaltyDate,
                                    oldPrincipalBalance = rs.principalBalance,
                                });
                            }
                            else //Not First Penalty for that Schedule
                            {
                                var dailyPenaltyAmount = (rs.principalBalance + rs.interestBalance +
                                                          firstPenalty.penaltyFee
                                                          +
                                                          (firstChargeDate != null &&
                                                           firstChargeDate <= nextPenaltyDate &&
                                                           rs.repaymentDate <= firstChargeDate
                                                              ? bouncedPenAmount
                                                              : 0))
                                                         *tenor.defaultPenaltyRate.Value/(100.0*30.0);

                                loans.Add(new LoanEOM
                                {
                                    additionalInterest = 0,
                                    AccountNumber = ln.client.accountNumber,
                                    newPenaltyAmount = 0,
                                    clientName = ln.client.surName + ", " + ln.client.otherNames,
                                    oldInterestBalance = rs.interestBalance,
                                    interestOutstanding = rs.interestBalance,
                                    LoanID = ln.loanID,
                                    LoanNo = ln.loanNo,
                                    ScheduleID = rs.repaymentScheduleID,
                                    penaltyInterest = dailyPenaltyAmount,
                                    PenaltyDate = nextPenaltyDate,
                                    oldPrincipalBalance = rs.principalBalance,
                                });
                            }
                            lstProcessed.Add(ln.loanID);
                        }
                    }
                }
                Session["loansAddInterest"] = loans;
                if (loans.Count > 0)
                {
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }

                gridInterest.DataSource = loans;
                gridInterest.DataBind();
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
             
        }

        protected void gridInterest_Load(object sender, EventArgs e)
        {
            gridInterest.DataSource = loans;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (loans.Count > 0)
            {
                jnl_batch jb = null;
                var pro = ent.comp_prof.FirstOrDefault();
                foreach (var r in loans)
                {
                    var ln = le.loans.FirstOrDefault(p => p.loanID == r.LoanID);
                    var ni = r.penaltyInterest;
                    if (r.penaltyInterest > 0)
                    {
                        ln.loanPenalties.Add(new loanPenalty
                        {
                            proposedAmount = 0,
                            penaltyBalance = r.penaltyInterest,
                            penaltyDate = r.PenaltyDate,
                            penaltyFee = r.penaltyInterest,
                            penaltyTypeId = null,
                            loanID = ln.loanID,
                            creator = User.Identity.Name,
                            creation_date = DateTime.Now
                        });
                        jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                            ln.loanType.unearnedInterestAccountID, r.penaltyInterest,
                            "Daily Penalty & Additional Interest for Loan - " + ln.client.surName + ", " +
                            ln.client.otherNames,
                            pro.currency_id.Value, r.PenaltyDate, ln.loanNo, ent, "SYSTEM",
                            ln.client.branchID);
                        ent.jnl_batch.Add(jb);
                    }
                    if (r.additionalInterest > 0)
                    {
                        ln.loanPenalties.Add(new loanPenalty
                        {
                            proposedAmount = 0,
                            penaltyDate = r.PenaltyDate,
                            penaltyBalance = r.additionalInterest,
                            penaltyFee = r.additionalInterest,
                            creator = "SYSTEM",
                            creation_date = DateTime.Now
                        });
                        jb = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                            ln.loanType.unearnedInterestAccountID, r.additionalInterest,
                            "Penalty & Additional Interest for Loan - " + ln.client.surName + ", " +
                            ln.client.otherNames,
                            pro.currency_id.Value, r.PenaltyDate, ln.loanNo, ent, "SYSTEM",
                            ln.client.branchID);
                        ent.jnl_batch.Add(jb);
                    } 
                    ln.lastPenaltyDate = r.PenaltyDate;
                }

                if (jb != null)
                {
                    ent.jnl_batch.Add(jb);
                }

            }

            le.SaveChanges();


            Session["loansAddInterest"] = null;
            HtmlHelper.MessageBox2("End of Day Processed successfully!", ResolveUrl("/ln/proc/eod.aspx"),
                "coreERP©: Successful", IconType.ok);
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

        private double GetPrincipalBalanceAsAt(DateTime date, int loanId)
        {
            double bal = 0.0;
            date = date.Date;
            var rs = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
                && (
                     (p.date <= date)
                   )
                ).ToList();
            var rs2 = le.loanRepayments.Where(p => p.loanID == loanId && p.repaymentDate <= date
                                                   &&
                                                   (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 ||
                                                    p.repaymentTypeID == 3)).ToList();
            if (rs.Count > 0)
            {
                bal = rs.Max(p => p.amountDisbursed)
                      - rs.Sum(p => p.principalPaid);
            }
            else
            {
                bal = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId)
                    .Max(p => p.amountDisbursed)
                      -
                      ((rs2.Count > 0)
                          ? rs2.Sum(p => p.principalPaid)
                          : 0.0);
            }
            if (bal < 0)
            {
                bal = 0;
            }

            return bal;
        }

        private void GetBalanceAsAt(DateTime date, int loanId, ref double bal)
        {
            date = date.Date;
            var rs = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
                && (
                     (p.amountPaid > 0 || p.date <= date)
                     || (Math.Abs(p.amountPaid) < 1 || p.date < date)
                   )
                ).ToList();
            var rs2 = le.loanRepayments.Where(p => p.loanID == loanId && p.repaymentDate <= date
                                                   &&
                                                   (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 ||
                                                    p.repaymentTypeID == 3)).ToList();
            if (rs.Count > 0)
            {
                bal = rs.Max(p => p.amountDisbursed)
                      + rs.Sum(p => p.interest)
                      + rs.Sum(p => p.penaltyAmount)
                      - rs.Sum(p => p.amountPaid);
            }
            else
            {
                bal = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId)
                    .Max(p => p.amountDisbursed)
                      -
                      ((rs2.Count > 0)
                          ? rs2.Sum(p => p.principalPaid)
                          : 0.0);
            }
            if (bal < 0)
            {
                bal = 0;
            }
        }

    }
     
}