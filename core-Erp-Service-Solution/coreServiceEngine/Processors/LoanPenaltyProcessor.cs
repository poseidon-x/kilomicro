using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreData.ErrorLog;
using coreLogic;
using coreReports;

namespace coreService.Processors
{
    public class LoanPenaltyProcessor
    {
        private readonly IcoreLoansEntities _le;
        private readonly Icore_dbEntities _ent;
        private readonly int _loanId;
        private readonly IreportEntities _rent;
        private readonly DateTime _currentDate;
        private readonly IJournalExtensions _journalextensions;
        private readonly List<LoanEOM> _loans;
        private readonly systemDate _sysDate;

        private const int NUMBER_OF_PENALTY_GRACE_DAYS = 4;
        private const int NUMBER_OF_PENALTY_GRACE_DAYS_JIREH = 4;


        public LoanPenaltyProcessor(int loanId, IcoreLoansEntities lent, Icore_dbEntities ent,
            DateTime currentDate, IreportEntities rrent)
        {
            this._le = lent;
            this._ent = ent;
            this._loanId = loanId;
            this._currentDate = currentDate;
            _rent = rrent;
            _journalextensions = new JournalExtensions(ent, _le);
            _sysDate = _le.systemDates.FirstOrDefault();
            _loans = new List<LoanEOM>();
        }

        public void Process()
        {
            try
            {
                //Logger.serviceError("Loan penalty Processor initialized");
                //Logger.serviceError("Calculating for Loan penalty Id :" + this._loanId);

                var pro = _ent.comp_prof.FirstOrDefault();
                var cfg = _le.loanConfigs.First();
                var ln = _le.loans.First(p => p.loanID == _loanId);
                jnl_batch jb = null;

                if (pro.comp_name.ToLower().Contains("eclipse"))
                {
                    //Logger.serviceError("Calculating Ecl");

                    //process penalty for eclipse
                    ProcessPenaltyForLoanEcl(ln, _currentDate, cfg, pro, ref jb);
                }
                else if (pro.comp_name.ToLower().Contains("gfi"))
                {
                    //Logger.serviceError("GFI");

                    //process penalty for GFI
                    ProcessPenaltyForLoan33(ln, _currentDate, cfg, pro, ref jb);
                }
                else
                {
                    //Logger.serviceError("Others");
                    if (cfg.penaltyScheme == 30)
                    {
                        //process penalty for Jireh Micro Finance
                        ProcessPenaltyForLoan(ln, _currentDate, cfg, pro, ref jb);
                    }
                    else if (cfg.penaltyScheme == 31)
                    {
                        ProcessPenaltyForLoan31(ln, _currentDate, cfg, pro, ref jb);
                    }
                    else if (cfg.penaltyScheme == 1)
                    {
                        ProcessPenaltyForLoanScheme1(ln, _currentDate, cfg, pro, ref jb);
                    }
                }
            }
            catch (Exception x)
            {
                ExceptionManager.LogException(x, "LoanPenaltyProcessor.Process");
            }
        }

        //Jireh
        private void ProcessPenaltyForLoan(loan ln, DateTime date, loanConfig cfg,
            comp_prof pro, ref jnl_batch jb)
        {
            if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            var lastPenaltyDate = ((ln.lastPenaltyDate == null)
                    ? ln.disbursementDate.Value
                    : ln.lastPenaltyDate.Value);

            //If company is JIREH MF, add specified grace period.
            //if (pro.comp_name.ToLower().Contains("jireh"))
            //{
            //    lastPenaltyDate = lastPenaltyDate.AddDays(NUMBER_OF_PENALTY_GRACE_DAYS_JIREH);
            //}     


            if ((lastPenaltyDate.AddMonths(1).AddDays(NUMBER_OF_PENALTY_GRACE_DAYS_JIREH) <= date || lastPenaltyDate == ln.disbursementDate || ln.lastPenaltyDate == null))
            {
                var oneMonthAfterLpd = ((ln.lastPenaltyDate == null)
                    ? ln.disbursementDate.Value
                    : lastPenaltyDate.AddMonths(1));

                if (ln.lastPenaltyDate != null && oneMonthAfterLpd > date.AddDays(-1))
                {
                    ExceptionManager.LogInformation("Second Skip //if (ln.lastPenaltyDate != null && oneMonthAfterLPD > date)");
                    return;
                }
                if (_sysDate != null && _sysDate.loanSystemDate != null && _sysDate.loanSystemDate < oneMonthAfterLpd)
                {
                    ExceptionManager.LogInformation("Third Skip //if (sysDate != null && sysDate.loanSystemDate != null && sysDate.loanSystemDate > oneMonthAfterLPD)");
                    return;
                }
                var rs = ln.repaymentSchedules
                    .ToList()
                    .Where(p => (
                        ((ln.lastPenaltyDate == null) ||
                         p.repaymentDate > lastPenaltyDate)
                        && (p.repaymentDate <= oneMonthAfterLpd.AddDays(4))
                        ))
                    .OrderByDescending(p => p.repaymentDate)
                    .FirstOrDefault();
                int? scheduleID = null;
                if (rs == null && date >= ln.disbursementDate.Value.AddMonths((int)ln.loanTenure)
                        && oneMonthAfterLpd <= ln.disbursementDate.Value.AddMonths((int)ln.loanTenure + 3))
                {
                    rs = new repaymentSchedule
                    {
                        additionalInterest = 0,
                        additionalInterestBalance = 0,
                        penaltyAmount = 0,
                        principalBalance = 0,
                        principalPayment = 0,
                        proposedInterestWriteOff = 0,
                        origPrincipalBF = 0,
                        origPrincipalCD = 0,
                        origPrincipalPayment = 0,
                        interestBalance = 0,
                        interestPayment = 0,
                        interestWritenOff = 0,
                        loanID = ln.loanID,
                        creator = "SYSTEM",
                        creation_date = DateTime.Now,
                        balanceCD = 0,
                        origInterestPayment = 0,
                        repaymentScheduleID = 0,
                        repaymentDate = oneMonthAfterLpd
                    };
                }
                else if (rs != null)
                {
                    scheduleID = rs.repaymentScheduleID;
                    oneMonthAfterLpd = rs.repaymentDate;
                }
                else
                {
                    return;
                }

                var addInterest = 0.0;
                var balanceAsAt = 0.0;
                var penaltyAmount = 0.0;

                var lt =
                    _le.tenors.OrderByDescending(p => p.tenor1)
                        .FirstOrDefault(p => p.loanTypeID == ln.loanTypeID && p.tenor1 <= ln.loanTenure);
                if (lt == null)
                {
                    lt =
                        _le.tenors.OrderByDescending(p => p.tenor1)
                            .FirstOrDefault(p => p.loanTypeID == ln.loanTypeID && p.tenor1 <= ln.loanTenure);
                    if (lt == null)
                    {
                        lt =
                            _le.tenors.OrderByDescending(p => p.tenor1)
                                .FirstOrDefault(p => p.tenor1 <= ln.loanTenure);
                        if (lt == null)
                        {
                            lt = _le.tenors.OrderByDescending(p => p.tenor1).FirstOrDefault();
                        }
                    }
                }
                double principalBalanceAsAt = 0.0;
                principalBalanceAsAt = GetPrincipalBalanceAsAt(oneMonthAfterLpd, ln.loanID);
                if (principalBalanceAsAt < 20)
                {
                    return;
                }
                GetBalanceAsAt(oneMonthAfterLpd, ln.loanID, ref balanceAsAt);

                if (balanceAsAt > 1)
                {
                    if (ln.interestTypeID == 1)
                    {
                        addInterest = ln.amountDisbursed * (ln.interestRate / 100.0);
                        if (ln.lastPenaltyDate != null && ln.repaymentSchedules.Any(p => p.principalBalance + p.interestBalance > 30 &&
                                p.repaymentDate <= ln.lastPenaltyDate))
                        {
                            var overdueBalance = 0.0;
                            GetBalanceAsAt(ln.lastPenaltyDate.Value, ln.loanID, ref overdueBalance);
                            if (overdueBalance > 0)
                            {
                                penaltyAmount = overdueBalance * (lt.defaultPenaltyRate.Value / 100.0);
                            }
                        }
                    }
                    else
                    {
                        addInterest = balanceAsAt * (ln.interestRate / 100.0);
                    }
                }
                if (addInterest > 0)
                {
                    if (jb == null)
                    {
                        string startNormalDate = System.Configuration.ConfigurationManager.AppSettings["startCashAccountingLoans"];
                        if (startNormalDate == null) startNormalDate = "";
                        DateTime startCashAccountingLoans = DateTime.MinValue;
                        if ((_le.loanConfigs.Any() && (DateTime.TryParseExact(startNormalDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.AllowInnerWhite,
                            out startCashAccountingLoans) && startCashAccountingLoans > DateTime.MinValue
                            && startCashAccountingLoans <= ln.disbursementDate.Value.Date)))
                        {
                            jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, addInterest,
                                "Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);
                        }
                        else if (_le.loanConfigs.Any())
                        {
                            jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.interestIncomeAccountID, addInterest,
                                "Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);
                        }
                        else
                        {
                            jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, addInterest,
                                "Penalty & Additional Interest for Loan - " + ln.client.surName + ", " +
                                ln.client.otherNames,
                                pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);
                        }
                    }
                    else
                    {
                        jnl_batch jb2;
                        string startNormalDate = System.Configuration.ConfigurationManager.AppSettings["startCashAccountingLoans"];
                        if (startNormalDate == null) startNormalDate = "";
                        DateTime startCashAccountingLoans = DateTime.MinValue;
                        if ((_le.loanConfigs.Any() && (DateTime.TryParseExact(startNormalDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.AllowInnerWhite,
                            out startCashAccountingLoans) && startCashAccountingLoans > DateTime.MinValue
                            && startCashAccountingLoans <= ln.disbursementDate.Value.Date)))
                        {
                            jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, addInterest,
                                "Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);
                        }
                        else if (_le.loanConfigs.Any())
                        {
                            jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.interestIncomeAccountID, addInterest,
                                "Interest for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);
                        }
                        else
                        {
                            jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, addInterest,
                                "Penalty & Additional Interest for Loan - " + ln.client.surName + ", " +
                                ln.client.otherNames,
                                pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);
                        }
                        var list = jb2.jnl.ToList();
                        jb.jnl.Add(list[0]);
                        jb.jnl.Add(list[1]);
                    }

                    if (penaltyAmount > 0)
                    {
                        if (jb == null)
                        {
                            string startNormalDate = System.Configuration.ConfigurationManager.AppSettings["startCashAccountingLoans"];
                            if (startNormalDate == null) startNormalDate = "";
                            DateTime startCashAccountingLoans = DateTime.MinValue;
                            if ((_le.loanConfigs.Any() && (DateTime.TryParseExact(startNormalDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.AllowInnerWhite,
                                out startCashAccountingLoans) && startCashAccountingLoans > DateTime.MinValue
                                && startCashAccountingLoans <= ln.disbursementDate.Value.Date)))
                            {
                                jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                    ln.loanType.unearnedInterestAccountID, penaltyAmount,
                                    "Penalty for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                    pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                    ln.client.branchID);
                            }
                            else if (_le.loanConfigs.Any())
                            {
                                jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                    ln.loanType.interestIncomeAccountID, penaltyAmount,
                                    "Penalty for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                    pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                    ln.client.branchID);
                            }
                            else
                            {
                                jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                    ln.loanType.unearnedInterestAccountID, penaltyAmount,
                                    "Penalty Interest for Loan - " + ln.client.surName + ", " +
                                    ln.client.otherNames,
                                    pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                    ln.client.branchID);
                            }
                        }
                        else
                        {
                            jnl_batch jb2;
                            string startNormalDate = System.Configuration.ConfigurationManager.AppSettings["startCashAccountingLoans"];
                            if (startNormalDate == null) startNormalDate = "";
                            DateTime startCashAccountingLoans = DateTime.MinValue;
                            if ((_le.loanConfigs.Any() && (DateTime.TryParseExact(startNormalDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.AllowInnerWhite,
                                out startCashAccountingLoans) && startCashAccountingLoans > DateTime.MinValue
                                && startCashAccountingLoans <= ln.disbursementDate.Value.Date)))
                            {
                                jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                    ln.loanType.unearnedInterestAccountID, penaltyAmount,
                                    "Penalty for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                    pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                    ln.client.branchID);
                            }
                            else if (_le.loanConfigs.Any())
                            {
                                jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                    ln.loanType.interestIncomeAccountID, penaltyAmount,
                                    "Penalty for Loan - " + ln.client.surName + "," + ln.client.otherNames,
                                    pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                    ln.client.branchID);
                            }
                            else
                            {
                                jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                    ln.loanType.unearnedInterestAccountID, penaltyAmount,
                                    "Penalty Interest for Loan - " + ln.client.surName + ", " +
                                    ln.client.otherNames,
                                    pro.currency_id.Value, oneMonthAfterLpd, ln.loanNo, _ent, "SYSTEM",
                                    ln.client.branchID);
                            }
                            var list = jb2.jnl.ToList();
                            jb.jnl.Add(list[0]);
                            jb.jnl.Add(list[1]);
                        }
                    }

                    rs = ln.repaymentSchedules.FirstOrDefault(p => p.repaymentScheduleID == scheduleID);
                    if (rs == null)
                    {
                        rs = new repaymentSchedule
                        {
                            repaymentDate = oneMonthAfterLpd,
                            origInterestPayment = 0,
                            origPrincipalPayment = 0,
                            balanceBF = 0,
                            balanceCD = 0,
                            creation_date = DateTime.Now,
                            creator = "SYSTEM",
                            proposedInterestWriteOff = 0,
                            origPrincipalBF = 0,
                            principalBalance = 0,
                            principalPayment = 0,
                            origPrincipalCD = 0,
                            interestBalance = 0,
                            interestWritenOff = 0,
                            interestPayment = 0,
                            edited = false,
                            penaltyAmount = 0,
                            additionalInterestBalance = 0,
                            additionalInterest = 0
                        };
                        ln.repaymentSchedules.Add(rs);
                    }
                    rs.penaltyAmount = penaltyAmount;
                    rs.interestPayment = addInterest;
                    rs.interestBalance = addInterest - (rs.interestPayment - rs.interestBalance);
                    rs.additionalInterest = addInterest;
                    rs.additionalInterestBalance = addInterest;
                }
                ln.lastPenaltyDate = oneMonthAfterLpd;

                if (jb != null)
                {
                    lock (this)
                    {
                        _ent.jnl_batch.Add(jb);
                    }
                    jb = null;
                }
                _le.SaveChanges();
                _ent.SaveChanges();
            }
        }

        //Eclipse
        private void ProcessPenaltyForLoan32(loan ln, DateTime date, loanConfig cfg,
            comp_prof pro, ref jnl_batch jb)
        {
            var loanTenure = (int)ln.loanTenure;

            //don't apply if last penalty date is 4 days ago or later
            if ((ln.disbursementDate.Value.AddMonths(loanTenure).AddDays(NUMBER_OF_PENALTY_GRACE_DAYS) > date))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            //don't apply if last penalty date is 4 days ago or later
            if ((ln.lastPenaltyDate != null && ln.lastPenaltyDate > date))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            //don't apply if balance is less than 20
            if ((ln.balance < 20))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            DateTime dateToStartAuntoPenalty = new DateTime(2015, 6, 1);
            string autoPenaltyConfigDate =
                System.Configuration.ConfigurationManager.AppSettings["AUTO_PENALTY_START_DATE"];
            if (!String.IsNullOrEmpty(autoPenaltyConfigDate))
            {
                DateTime.TryParseExact(autoPenaltyConfigDate, "yyyy-MM-dd", CultureInfo.CurrentCulture,
                    DateTimeStyles.AdjustToUniversal, out dateToStartAuntoPenalty);
            }
            //don't apply if disbursementDate is before 01-06-2015
            if ((ln.disbursementDate < dateToStartAuntoPenalty))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            //retrieve last penalty date
            var lastPenaltyDate = ((ln.lastPenaltyDate) ?? ln.disbursementDate.Value.AddMonths(loanTenure).AddDays(NUMBER_OF_PENALTY_GRACE_DAYS));
                //: ln.lastPenaltyDate.Value);
            //apply penalty after one month of not paying
            if ((lastPenaltyDate <= date || lastPenaltyDate == ln.disbursementDate))
            {
                //move to next penalty date
                var oneDayAfterLastPenaltyDate = ((ln.lastPenaltyDate == null)
                    ? lastPenaltyDate
                    : lastPenaltyDate.AddDays(1));

                //don't procees if customer doesn't owe for more than 4 days
                if (ln.lastPenaltyDate != null && oneDayAfterLastPenaltyDate > date)
                {
                    ExceptionManager.LogInformation("Second Skip //if (ln.lastPenaltyDate != null && oneMonthAfterLPD > date)");
                    return;
                }

                var balanceAsAt = 0.0;
                var penaltyAmount = 0.0;
                var additionalInterest = 0.0;
                GetTotalBalanceAsAt(oneDayAfterLastPenaltyDate, ln.loanID, ln, ref balanceAsAt);

                //get penalty rate
                double penaltyRate = getPenaltyRate(ln);

                if (balanceAsAt > 1)
                {
                    penaltyAmount = (penaltyRate / 100.0) * 12 * (30 / 365.0) * balanceAsAt;
                    additionalInterest = (ln.interestRate / 100.0) * 12 * (1 / 365.0) * balanceAsAt;
                }

                DateTime startDate = new DateTime(oneDayAfterLastPenaltyDate.Year, oneDayAfterLastPenaltyDate.Month, 1);
                loanPenalty penalty = ln.loanPenalties.FirstOrDefault(p => p.penaltyDate >= startDate
                                                                           &&
                                                                           p.penaltyDate <=
                                                                           oneDayAfterLastPenaltyDate);
                if (penalty == null)
                {
                    if (jb == null)
                    {
                        jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                            ln.loanType.unearnedInterestAccountID, penaltyAmount + additionalInterest,
                            "Penalty for Loan - " + ln.client.surName + ", " +
                            ln.client.otherNames,
                            pro.currency_id.Value, oneDayAfterLastPenaltyDate, ln.loanNo, _ent, "SYSTEM",
                            ln.client.branchID);
                    }
                    
                    ln.loanPenalties.Add(new loanPenalty
                        {
                            penaltyFee = penaltyAmount + additionalInterest,
                            penaltyDate = oneDayAfterLastPenaltyDate,
                            penaltyBalance = penaltyAmount + additionalInterest,
                            penaltyTypeId = 1,
                            creator = "System",
                            creation_date = DateTime.Now,
                        });
                }
                else
                {
                    if(jb != null)
                    {
                        jnl_batch jb2;
                        jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, additionalInterest,
                                "Additional Interest for Loan - " + ln.client.surName + ", " +
                                ln.client.otherNames,
                                pro.currency_id.Value, oneDayAfterLastPenaltyDate, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);

                        var list = jb2.jnl.ToList();
                        jb.jnl.Add(list[0]);
                        jb.jnl.Add(list[1]);
                    }

                    foreach (var record in _ent.jnl.Where(p => p.ref_no == ln.loanNo 
                    && p.tx_date.Month == penalty.penaltyDate.Month).ToList())
                    {
                        if (record.crdt_amt > 0)
                        {
                            record.crdt_amt += additionalInterest;
                        }else if (record.dbt_amt > 0)
                        {
                            record.dbt_amt += additionalInterest;
                        }
                    }
                    penalty.penaltyFee = penalty.penaltyFee + additionalInterest;
                    penalty.penaltyBalance = penalty.penaltyBalance + additionalInterest;
                    penalty.penaltyDate = oneDayAfterLastPenaltyDate;
                }
                //Increment  last penalty date
                ln.lastPenaltyDate = oneDayAfterLastPenaltyDate;

                if (jb != null)
                {
                    lock (this)
                    {
                        _ent.jnl_batch.Add(jb);
                    }
                    jb = null;
                }
                _le.SaveChanges();
                _ent.SaveChanges();
            }
        }



        //Eclipse
        private void ProcessPenaltyForLoanEcl(loan ln, DateTime date, loanConfig cfg,
            comp_prof pro, ref jnl_batch jb)
        {
            var sysDate = _le.systemDates.FirstOrDefault();
            if (ln.disbursementDate == null) return;

            double gracePeriod = 0;
            var loanConfig = _le.configs.FirstOrDefault();
            if (loanConfig != null)
            {
                gracePeriod = loanConfig.loanPenaltyGracePeriodInDays;
            }
            var loanTenure = (int)ln.loanTenure;

            //don't apply if last penalty date is 4 days ago or later
            if ((ln.disbursementDate.Value.Date.AddMonths(loanTenure).AddDays(gracePeriod) > date))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            ////don't apply if last penalty date is 4 days ago or later
            //if ((ln.lastPenaltyDate != null && ln.lastPenaltyDate > date))
            //{
            //    ExceptionManager.LogInformation(
            //        "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
            //    return;
            //}

            ////don't apply if balance is less than 20
            //if ((ln.balance < 20))
            //{
            //    ExceptionManager.LogInformation(
            //        "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
            //    return;
            //}

            DateTime dateToStartAuntoPenalty = new DateTime(2015, 6, 1);
            string autoPenaltyConfigDate = System.Configuration.ConfigurationManager.AppSettings["AUTO_PENALTY_START_DATE"];
            if (!String.IsNullOrEmpty(autoPenaltyConfigDate))
            {
                DateTime.TryParseExact(autoPenaltyConfigDate, "yyyy-MM-dd", CultureInfo.CurrentCulture,
                    DateTimeStyles.AdjustToUniversal, out dateToStartAuntoPenalty);
            }
            //don't apply if disbursementDate is before 01-06-2015
            if ((ln.disbursementDate.Value.Date < dateToStartAuntoPenalty))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            //retrieve last penalty date
            var lastPenaltyDate = ((ln.lastPenaltyDate) ?? ln.disbursementDate.Value.Date.AddMonths(loanTenure).AddDays(gracePeriod));
            
            //apply penalty after one month of not paying
            if (lastPenaltyDate <= date )
            {
                //move to next penalty date
                var oneDayAfterLastPenaltyDate = ((ln.lastPenaltyDate == null)
                    ? lastPenaltyDate
                    : lastPenaltyDate.AddDays(1));

                //don't procees if customer doesn't owe for more than 4 days
                if (ln.lastPenaltyDate != null && oneDayAfterLastPenaltyDate > date)
                {
                    ExceptionManager.LogInformation("Second Skip //if (ln.lastPenaltyDate != null && oneMonthAfterLPD > date)");
                    return;
                }

                var balanceAsAt = 0.0;
                var penaltyAmount = 0.0;
                var additionalInterest = 0.0;
                GetLoanBalanceAsAt(oneDayAfterLastPenaltyDate, ln.loanID, ln, ref balanceAsAt);

                //get penalty rate
                double penaltyRate = getPenaltyRate(ln);

                if (balanceAsAt > 1)
                {
                    //penaltyAmount = (penaltyRate / 100.0) * 12 * (30 / 365.0) * balanceAsAt;
                    additionalInterest = (ln.interestRate / 100.0) * 12 * (1 / 365.0) * balanceAsAt;
                }

                DateTime startDate = new DateTime(oneDayAfterLastPenaltyDate.Year, oneDayAfterLastPenaltyDate.Month, 1);
                loanPenalty penalty = ln.loanPenalties.FirstOrDefault(p => p.penaltyDate >= startDate
                                                                           &&
                                                                           p.penaltyDate <=
                                                                           oneDayAfterLastPenaltyDate);
                //if (penalty == null)
                //{
                //    if (jb == null)
                //    {
                //        jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                //            ln.loanType.unearnedInterestAccountID, penaltyAmount + additionalInterest,
                //            "Penalty for Loan - " + ln.client.surName + ", " +
                //            ln.client.otherNames,
                //            pro.currency_id.Value, oneDayAfterLastPenaltyDate, ln.loanNo, _ent, "SYSTEM",
                //            ln.client.branchID);
                //    }

                //    ln.loanPenalties.Add(new loanPenalty
                //    {
                //        penaltyFee = penaltyAmount + additionalInterest,
                //        penaltyDate = oneDayAfterLastPenaltyDate,
                //        penaltyBalance = penaltyAmount + additionalInterest,
                //        penaltyTypeId = 1,
                //        creator = "System",
                //        creation_date = DateTime.Now,
                //    });
                //}
                if(additionalInterest > 0)
                {
                    if (jb != null)
                    {
                        jnl_batch jb2;
                        jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, additionalInterest,
                                "Additional Interest for Loan - " + ln.client.surName + ", " +
                                ln.client.otherNames,
                                pro.currency_id.Value, oneDayAfterLastPenaltyDate, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);

                        var list = jb2.jnl.ToList();
                        jb.jnl.Add(list[0]);
                        jb.jnl.Add(list[1]);
                    }

                    foreach (var record in _ent.jnl.Where(p => p.ref_no == ln.loanNo
                    && p.tx_date.Month == penalty.penaltyDate.Month).ToList())
                    {
                        if (record.crdt_amt > 0)
                        {
                            record.crdt_amt += additionalInterest;
                        }
                        else if (record.dbt_amt > 0)
                        {
                            record.dbt_amt += additionalInterest;
                        }
                    }
                    penalty.penaltyFee = penalty.penaltyFee + additionalInterest;
                    penalty.penaltyBalance = penalty.penaltyBalance + additionalInterest;
                    penalty.penaltyDate = oneDayAfterLastPenaltyDate;

                    //Increment  last penalty date
                    ln.lastPenaltyDate = oneDayAfterLastPenaltyDate;

                    if (jb != null)
                    {
                        lock (this)
                        {
                            _ent.jnl_batch.Add(jb);
                        }
                        jb = null;
                    }
                    _le.SaveChanges();
                    _ent.SaveChanges();
                }
                
            }
        }


        private void ProcessPenaltyForLoan31(loan ln, DateTime date, loanConfig cfg,
            comp_prof pro, ref jnl_batch jb)
        {
            if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))
                return;
            if (ln.disbursementDate != null)
            {
                var lastPenaltyDate = ln.lastPenaltyDate ?? ln.disbursementDate.Value;
                if ((lastPenaltyDate.AddMonths(1) <= date || lastPenaltyDate == ln.disbursementDate || ln.lastPenaltyDate == null))
                {
                    var oneMonthAfterLpd = ((ln.lastPenaltyDate == null)
                        ? ln.disbursementDate.Value.AddMonths(1)
                        : lastPenaltyDate.AddMonths(1));
                    if (oneMonthAfterLpd > date)
                    {
                        ExceptionManager.LogInformation("Second Skip //if (ln.lastPenaltyDate != null && oneMonthAfterLPD > date)");
                        return;
                    }
                    if (_sysDate != null && _sysDate.loanSystemDate != null && _sysDate.loanSystemDate > oneMonthAfterLpd)
                    {
                        ExceptionManager.LogInformation("Third Skip //if (sysDate != null && sysDate.loanSystemDate != null && sysDate.loanSystemDate < oneMonthAfterLPD)");
                        return;
                    }

                    Process31(ln, cfg, pro, ref jb);
                    Save31(ln, cfg, pro, ref jb);

                    ln = _le.loans.First(p => p.loanID == ln.loanID);
                    ln.lastPenaltyDate = oneMonthAfterLpd;

                    _le.SaveChanges();
                    _ent.SaveChanges();
                }
            }
        }

        //GFI
        private void ProcessPenaltyForLoan33(loan ln, DateTime date, loanConfig cfg,
            comp_prof pro, ref jnl_batch jb)
        {
            var loanTenure = (int)ln.loanTenure;
            var lastDuePmtDate = ln.repaymentSchedules //get last expected paymeny date
                .OrderByDescending(p => p.repaymentDate)
                .First()
                .repaymentDate;

            //don't apply if last penalty date is 4 days ago or later
            if (lastDuePmtDate > date)
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            //don't apply if last penalty date is 4 days ago or later
            if ((ln.lastPenaltyDate != null && ln.lastPenaltyDate > date))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            //don't apply if balance is less than 20
            if ((ln.balance < 20))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            DateTime dateToStartAuntoPenalty = new DateTime(2015, 6, 1);
            string autoPenaltyConfigDate =
                System.Configuration.ConfigurationManager.AppSettings["AUTO_PENALTY_START_DATE"];
            if (autoPenaltyConfigDate != null && autoPenaltyConfigDate != "")
            {
                DateTime.TryParseExact(autoPenaltyConfigDate, "yyyy-MM-dd", CultureInfo.CurrentCulture,
                    DateTimeStyles.AdjustToUniversal, out dateToStartAuntoPenalty);
            }
            //don't apply if disbursementDate is before 01-06-2015
            if ((ln.disbursementDate < dateToStartAuntoPenalty))
            {
                ExceptionManager.LogInformation(
                    "Second Skip // if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))");
                return;
            }

            //retrieve last penalty date
            var lastPenaltyDate = ((ln.lastPenaltyDate == null) ? ln.disbursementDate.Value.AddMonths(loanTenure)
                : ln.lastPenaltyDate.Value);
            //apply penalty after one month of not paying
            if ((lastPenaltyDate <= date || lastPenaltyDate == ln.disbursementDate || ln.lastPenaltyDate == null))
            {
                //move to next penalty date
                var oneDayAfterLastPenaltyDate = ((ln.lastPenaltyDate == null)
                    ? lastPenaltyDate
                    : lastPenaltyDate.AddDays(1));

                //don't procees if customer doesn't owe for more than 4 days
                if (ln.lastPenaltyDate != null && oneDayAfterLastPenaltyDate > date)
                {
                    ExceptionManager.LogInformation("Second Skip //if (ln.lastPenaltyDate != null && oneMonthAfterLPD > date)");
                    return;
                }

                var balanceAsAt = 0.0;
                var penaltyAmount = 0.0;
                GetTotalBalanceAsAt(oneDayAfterLastPenaltyDate, ln.loanID, ln, ref balanceAsAt);

                //get penalty rate
                double penaltyRate = getPenaltyRate(ln);
                if (balanceAsAt > 1 && lastPenaltyDate < lastDuePmtDate.AddMonths(3))
                {
                    penaltyAmount = (penaltyRate / 100.0) * 12 * (1 / 91.0) * balanceAsAt;
                }
                else if (balanceAsAt > 1 && lastPenaltyDate >= lastDuePmtDate.AddMonths(3))
                {
                    penaltyAmount = (penaltyRate / 100.0) * 12 * (1 / 30.0) * balanceAsAt;
                }

                if (penaltyAmount > 0)
                {
                    if (jb == null)
                    {
                        jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                            ln.loanType.unearnedInterestAccountID, penaltyAmount,
                            "Penalty for Loan - " + ln.client.surName + ", " +
                            ln.client.otherNames,
                            pro.currency_id.Value, oneDayAfterLastPenaltyDate, ln.loanNo, _ent, "SYSTEM",
                            ln.client.branchID);
                    }
                    else
                    {
                        jnl_batch jb2;
                        jb2 = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, penaltyAmount,
                                "Penalty & Additional Interest for Loan - " + ln.client.surName + ", " +
                                ln.client.otherNames,
                                pro.currency_id.Value, oneDayAfterLastPenaltyDate, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);

                        var list = jb2.jnl.ToList();
                        jb.jnl.Add(list[0]);
                        jb.jnl.Add(list[1]);
                    }
                    DateTime startDate = new DateTime(oneDayAfterLastPenaltyDate.Year, oneDayAfterLastPenaltyDate.Month, 1);
                    loanPenalty penalty = ln.loanPenalties.FirstOrDefault(p => p.penaltyDate >= startDate
                                                                               &&
                                                                               p.penaltyDate <=
                                                                               oneDayAfterLastPenaltyDate);

                    if (penalty == null)
                    {
                        ln.loanPenalties.Add(new loanPenalty
                        {
                            penaltyFee = penaltyAmount,
                            penaltyDate = oneDayAfterLastPenaltyDate,
                            penaltyBalance = penaltyAmount,
                            penaltyTypeId = 1,
                            creator = "System",
                            creation_date = DateTime.Now,
                        });
                    }
                    else
                    {
                        penalty.penaltyFee = penalty.penaltyFee + penaltyAmount;
                        penalty.penaltyBalance = penalty.penaltyBalance + penaltyAmount;
                        penalty.penaltyDate = oneDayAfterLastPenaltyDate;
                    }

                }
                //Increment  last penalty date
                ln.lastPenaltyDate = oneDayAfterLastPenaltyDate;

                if (jb != null)
                {
                    lock (this)
                    {
                        _ent.jnl_batch.Add(jb);
                    }
                    jb = null;
                }
                _le.SaveChanges();
                _ent.SaveChanges();
            }
        }


        private void ProcessPenaltyForLoanScheme1(loan ln, DateTime date, loanConfig cfg,
            comp_prof pro, ref jnl_batch jb)
        {
            try
            {
                if (!(ln.lastPenaltyDate == null || ln.lastPenaltyDate < date))
                    return;
                var lastPenaltyDate = ((ln.lastPenaltyDate == null)
                    ? ln.disbursementDate.Value.AddMonths(1).AddDays(-1)
                    : ln.lastPenaltyDate.Value);
                if ((lastPenaltyDate.AddDays(1) <= date || lastPenaltyDate == ln.disbursementDate ||
                     ln.lastPenaltyDate == null))
                {
                    var nextPenaltyDate = lastPenaltyDate.AddDays(1);
                    if (ln.lastPenaltyDate != null && nextPenaltyDate > date)
                    {
                        ExceptionManager.LogInformation("Second Skip //if (ln.lastPenaltyDate != null && nextPenaltyDate > date)");
                        return;
                    }
                    if (_sysDate != null && _sysDate.loanSystemDate != null && _sysDate.loanSystemDate > nextPenaltyDate)
                    {
                        ExceptionManager.LogInformation("Third Skip //if (sysDate != null && sysDate.loanSystemDate != null && sysDate.loanSystemDate < nextPenaltyDate)");
                        return;
                    }

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
                        var tenor = _le.tenors.FirstOrDefault(p => p.loanTypeID == ln.loanTypeID) ?? _le.tenors.First();

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
                                      * (tenor.defaultPenaltyRate.Value / 100.0);
                        loanPenalty firstPenalty = ln.loanPenalties
                            .OrderBy(p => p.penaltyDate)
                            .FirstOrDefault(p => p.penaltyDate == rs.repaymentDate
                                                 && p.last_modifier == scheduleId
                                                 && (p.penaltyTypeId == null || p.penaltyTypeId == 1));
                        if (firstPenalty == null)
                        {
                            firstPenalty = new loanPenalty
                            {
                                proposedAmount = 0,
                                penaltyDate = nextPenaltyDate,
                                penaltyBalance = addInterest,
                                penaltyFee = addInterest,
                                creator = "SYSTEM",
                                creation_date = DateTime.Now,
                                last_modifier = scheduleId,
                            };
                            ln.loanPenalties.Add(firstPenalty);
                            jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, addInterest,
                                "Penalty & Additional Interest for Loan - " + ln.client.surName + ", " +
                                ln.client.otherNames,
                                pro.currency_id.Value, nextPenaltyDate, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);
                            _ent.jnl_batch.Add(jb);
                        }
                        else //Not First Penalty for that Schedule
                        {
                            var dailyPenaltyAmount = (rs.principalBalance + rs.interestBalance + firstPenalty.penaltyFee
                                                      +
                                                      (firstChargeDate != null && firstChargeDate <= nextPenaltyDate &&
                                                       rs.repaymentDate <= firstChargeDate
                                                          ? bouncedPenAmount
                                                          : 0))
                                                     * tenor.defaultPenaltyRate.Value / (100.0 * 30.0);
                            firstPenalty = new loanPenalty
                            {
                                proposedAmount = 0,
                                penaltyDate = nextPenaltyDate,
                                penaltyBalance = dailyPenaltyAmount,
                                penaltyFee = dailyPenaltyAmount,
                                creator = "SYSTEM",
                                creation_date = DateTime.Now
                            };
                            ln.loanPenalties.Add(firstPenalty);
                            jb = _journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                                ln.loanType.unearnedInterestAccountID, dailyPenaltyAmount,
                                "Daily Penalty & Additional Interest for Loan - " + ln.client.surName + ", " +
                                ln.client.otherNames,
                                pro.currency_id.Value, nextPenaltyDate, ln.loanNo, _ent, "SYSTEM",
                                ln.client.branchID);
                            _ent.jnl_batch.Add(jb);
                        }
                    }
                    ln.lastPenaltyDate = nextPenaltyDate;
                    _le.SaveChanges();
                    _ent.SaveChanges();
                }
            }
            catch (Exception x)
            {
                ExceptionManager.LogException(x, "PenaltyModule.Main");
            }
        }

        private void GetBalanceAsAt(DateTime date, int loanId, ref double bal)
        {
            date = date.Date;
            var rsPrinc = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
                                                            && (  p.date <= date)  
                                                            && p.amountDisbursed > 0 
                ).Take(1).ToList();
            var rsInt = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
                                                               && (
                                                                   (p.amountPaid > 0
                                                                    || (p.date < date && p.interest > 0)
                                                                   ||( p.amountPaid < -1)
                                                                   )) 
                ).ToList();
            var rsPmt = _le.loanRepayments.Where(p => p.loanID == loanId && p.repaymentDate <= date
                                                    &&
                                                    (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 ||
                                                     p.repaymentTypeID == 3)).ToList();
            if (rsPrinc.Count>0)
            {
                bal = rsPrinc.Max(p => p.amountDisbursed);
            }
            if (rsInt.Count > 0)
            {
                bal = bal + rsInt.Sum(p => p.penaltyAmount)
                      - rsInt.Sum(p => p.amountPaid);
            }
            else
            {
                bal = bal - ((rsPmt.Count > 0)
                          ? rsPmt.Sum(p => p.principalPaid)
                          : 0.0);
            }
            if (rsInt.Any())
            {
                bal += rsInt.Sum(p => p.interest);
            }

        if (bal < 0)
            {
                bal = 0;
            }
        }


        private void GetTotalBalanceAsAt(DateTime date, int loanId, loan ln, ref double bal)
        {
            date = date.Date;
            DateTime startOfMonth = new DateTime(date.Year, date.Month, 1);

            var rs = _rent.vwLoanActualSchedules
                .Where(p => p.loanID == loanId
                && ((p.amountPaid > 0 || p.date <= date) || (Math.Abs(p.amountPaid) < 1 || p.date < date)))
                .ToList();

            var rsPenalty = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
                && ((p.penaltyAmount > 0 && p.date < startOfMonth)
                     && p.interest == 0))
                     .ToList();

            var rs2 = _le.loanRepayments.Where(p => p.loanID == loanId && p.repaymentDate <= date &&
                                                   (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 ||
                                                    p.repaymentTypeID == 3 || p.repaymentTypeID == 7)).ToList();

            if (rs.Count > 0)
            {
                bal = rs.Max(p => p.amountDisbursed)
                      + rs.Sum(p => p.interest)
                      + (rsPenalty.Any() ? rsPenalty.Sum(p => p.penaltyAmount) : 0)
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

        private void GetLoanBalanceAsAt(DateTime date, int loanId, loan ln, ref double bal)
        {
            date = date.Date;
            DateTime startOfMonth = new DateTime(date.Year, date.Month, 1);

            List<int> repayIds = new List<int> { 1, 2, 3};

            
            double principalBal = ln.amountDisbursed -
                                  (ln.loanRepayments.Where(p => p.repaymentDate.Date <= date.Date && (p.repaymentTypeID == 1 || p.repaymentTypeID == 2))
                                      .Sum(p => p.principalPaid));

            double interestBal = ln.repaymentSchedules.Sum(p => p.interestPayment) -
                                  (ln.loanRepayments.Where(p => p.repaymentDate.Date <= date.Date && (p.repaymentTypeID == 1 || p.repaymentTypeID ==3))
                                      .Sum(p => p.interestPaid));

            double penaltyBal = ln.loanPenalties.Where(p => p.penaltyDate.Date <= date.Date).Sum(p => p.penaltyBalance) -
                                  (ln.loanRepayments.Where(p => p.repaymentDate.Date <= date.Date && (p.repaymentTypeID == 7 ))
                                      .Sum(p => p.penaltyPaid));


            //var rs = _rent.vwLoanActualSchedules
            //    .Where(p => p.loanID == loanId
            //    && ((p.amountPaid > 0 || p.date <= date) || (Math.Abs(p.amountPaid) < 1 || p.date < date)))
            //    .ToList();

            //var rsPenalty = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId
            //    && ((p.penaltyAmount > 0 && p.date < startOfMonth)
            //         && p.interest == 0))
            //         .ToList();

            //var rs2 = _le.loanRepayments.Where(p => p.loanID == loanId && p.repaymentDate <= date &&
            //                                       (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 ||
            //                                        p.repaymentTypeID == 3 || p.repaymentTypeID == 7)).ToList();

            //if (rs.Count > 0)
            //{
            bal = principalBal+ interestBal+ penaltyBal;
            //}
            //else
            //{
            //    bal = _rent.vwLoanActualSchedules.Where(p => p.loanID == loanId)
            //        .Max(p => p.amountDisbursed)
            //          -
            //          ((rs2.Count > 0)
            //              ? rs2.Sum(p => p.principalPaid)
            //              : 0.0);
            //}
            if (bal < 0)
            {
                bal = 0;
            }
        }


        private void Process31(loan ln, loanConfig cfg, comp_prof pro, ref jnl_batch jb)
        {
            List<int> lstProcessed = new List<int>();

            var lastPenaltyDate = ln.lastPenaltyDate ?? ln.disbursementDate.Value;
            if (lastPenaltyDate.AddMonths(1) <= _currentDate && lstProcessed.Contains(ln.loanID) == false)
            {
                var oneMonthAfterLpd = lastPenaltyDate.AddMonths(1);
                var rs = ln.repaymentSchedules.Where(p => p.repaymentDate <= oneMonthAfterLpd)
                    .OrderByDescending(p => p.repaymentDate).FirstOrDefault();
                int? scheduleId = null;

                if (rs != null)
                {
                    scheduleId = rs.repaymentScheduleID;
                }
                else if (rs == null && oneMonthAfterLpd < ln.disbursementDate.Value.AddMonths((int)ln.loanTenure))
                {
                    return;
                }
                var addInterest = 0.0;
                var penInterest = 0.0;
                var princNotPaid = 0.0;
                var penBalance = 0.0;
                var pr = ln.repaymentSchedules
                    .Where(p => p.repaymentDate <= oneMonthAfterLpd)
                    .ToList();
                if (pr.Any())
                {
                    princNotPaid = GetPrincipalBalanceAsAt(oneMonthAfterLpd, ln.loanID);
                }
                if (ln.loanPenalties.Any())
                {
                    penBalance = ln.loanPenalties.Sum(p => p.penaltyBalance);
                }
                var intOuts =
                        ln.repaymentSchedules
                        .Where(p => rs == null || p.repaymentDate <= oneMonthAfterLpd)
                        .Sum(p => p.interestBalance);

                var lt =
                    _le.tenors.OrderByDescending(p => p.tenor1)
                        .FirstOrDefault(p => p.loanTypeID == ln.loanTypeID && p.tenor1 <= ln.loanTenure);
                if (lt == null)
                {
                    lt =
                        _le.tenors.OrderByDescending(p => p.tenor1)
                            .FirstOrDefault(p => p.tenor1 <= ln.loanTenure);
                    if (lt == null)
                    {
                        lt = _le.tenors.OrderByDescending(p => p.tenor1).FirstOrDefault();
                    }
                }
                var totalMonths = 0.0;
                for (DateTime date = ln.disbursementDate.Value.AddMonths(1); date <= oneMonthAfterLpd; date = date.AddMonths(1))
                {
                    totalMonths += 1;
                }
                addInterest = (princNotPaid + intOuts + penBalance) *
                    (ln.interestRate / 100.0);
                penInterest = (princNotPaid + intOuts + penBalance) *
                    (lt.defaultPenaltyRate.Value / 100.0);


                if (addInterest + penInterest < 0)
                {
                    return;
                }

                _loans.Add(new LoanEOM
                {
                    AccountNumber = ln.client.accountNumber,
                    additionalInterest = 0,
                    clientName =
                        (ln.client.clientTypeID == 3 || ln.client.clientTypeID == 4 ||
                         ln.client.clientTypeID == 5)
                            ? ln.client.companyName
                            : ((ln.client.clientTypeID == 6)
                                ? ln.client.accountName
                                : ln.client.surName +
                                  ", " + ln.client.otherNames),
                    interestOutstanding = intOuts,
                    LoanID = ln.loanID,
                    LoanNo = ln.loanNo,
                    oldInterestBalance = intOuts,
                    oldPrincipalBalance = princNotPaid,
                    PenaltyDate = oneMonthAfterLpd,
                    ScheduleID = scheduleId,
                    newPenaltyAmount = penInterest,
                    penaltyInterest = penInterest
                });

            }
        }

        private void Save31(loan ln, loanConfig cfg, comp_prof pro, ref jnl_batch jb)
        {
            if (_loans.Count > 0)
            {
                foreach (var r in _loans)
                {
                    var l = _le.loans.FirstOrDefault(p => p.loanID == r.LoanID);
                    var ni = r.penaltyInterest;
                    if (r.penaltyInterest + r.additionalInterest > 0.05)
                    {
                        _le.loanPenalties.Add(new loanPenalty
                        {
                            proposedAmount = 0,
                            penaltyBalance = r.penaltyInterest,
                            creation_date = DateTime.Now,
                            creator = "SYSTEM",
                            loanID = r.LoanID,
                            penaltyDate = r.PenaltyDate,
                            penaltyFee = r.penaltyInterest
                        });
                        if (jb == null)
                        {
                            jb = _journalextensions.Post("LN", l.loanType.accountsReceivableAccountID,
                                l.loanType.unearnedInterestAccountID, r.penaltyInterest + r.additionalInterest,
                                "Penalty & Additional Interest for Loan - " + l.client.surName + "," +
                                l.client.otherNames,
                                pro.currency_id.Value, r.PenaltyDate, l.loanNo, _ent, "SYSTEM",
                                l.client.branchID);
                        }
                        else
                        {
                            var jb2 = _journalextensions.Post("LN", l.loanType.accountsReceivableAccountID,
                                l.loanType.unearnedInterestAccountID, r.penaltyInterest + r.additionalInterest,
                                "Penalty & Additional Interest for Loan - " + l.client.surName + "," +
                                l.client.otherNames,
                                pro.currency_id.Value, r.PenaltyDate, l.loanNo, _ent, "SYSTEM",
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
                            creator = "SYSTEM",
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

                if (jb != null)
                {
                    _ent.jnl_batch.Add(jb);
                }

            }
        }

        private double GetPrincipalBalanceAsAt(DateTime date, int loanId)
        {
            double bal = 0.0;
            date = date.Date;
            var rs = _rent.vwLoanActualSchedules
                .Where(p => p.loanID == loanId && p.date <= date)
                .ToList();
            var rs2 = _le.loanRepayments.Where(p => p.loanID == loanId && p.repaymentDate <= date
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

        private double getPenaltyRate(loan ln)
        {
            double penaltyRate = 0;
            tenor tn = _le.tenors.FirstOrDefault(p => p.loanTypeID == ln.loanTypeID && p.tenor1 == ln.loanTenure);
            if (tn == null)
            {
                tn = _le.tenors.FirstOrDefault(p => p.loanTypeID == ln.loanTypeID);
                if (tn == null)
                {
                    tn = _le.tenors.FirstOrDefault(p => p.tenor1 == ln.loanTenure);
                    if (tn == null)
                    {
                        tn = _le.tenors.FirstOrDefault();
                    }
                }
            }
            if (tn != null)
            {
                penaltyRate = tn.defaultPenaltyRate.Value;
            }
            return penaltyRate;
        }

    }

    public class LoanEOM
    {
        public int LoanID { get; set; }
        public string LoanNo { get; set; }
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

}
