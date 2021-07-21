using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic
{
    public class ReportHelper
    {
        public List<coreReports.accountBalance> getFiveYearBalanceSheet(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx)
        {
            List<coreReports.accountBalance> balances = new List<coreReports.accountBalance>();

            using (var rent = new coreReports.reportEntities())
            {
                int i = 0;
                var date2 = startDate;
                for (var date = startDate; date <= endDate; date = date.AddYears(1))
                {
                    i = i + 1;
                    date2 = date.AddYears(1).AddSeconds(-1);
                    var yearBals = rent.get_bal_sht(date, date2, noTx, costCenterID).ToList();
                    foreach (var bal in yearBals)
                    {
                        var bal2 = balances.FirstOrDefault(p => p.acct_id == bal.acct_id);
                        if (bal2 == null)
                        {
                            bal2 = new coreReports.accountBalance
                            {
                                acct_id = bal.acct_id,
                                acc_name = bal.acc_name,
                                acc_num = bal.acc_num,
                                cat_code = bal.cat_code,
                                cat_name = bal.cat_name,
                                head_name1 = bal.head_name1,
                                head_name2 = bal.head_name2,
                                head_name3 = bal.head_name3,
                                head_name4 = bal.head_name4,
                                head_name5 = bal.head_name5,
                                head_name6 = bal.head_name6,
                                head_name7 = bal.head_name7,
                                loc_beg_bal = bal.loc_beg_bal,
                                loc_end_bal = bal.loc_end_bal,
                                currency_id = bal.currency_id
                            };
                            balances.Add(bal2);
                        }
                        if (i == 1)
                        {
                            bal2.Year1 = date.Year.ToString();
                            bal2.Year1Balance = bal.loc_end_bal;
                        }
                        else if (i == 2)
                        {
                            bal2.Year2 = date.Year.ToString();
                            bal2.Year2Balance = bal.loc_end_bal;
                        }
                        else if (i == 3)
                        {
                            bal2.Year3 = date.Year.ToString();
                            bal2.Year3Balance = bal.loc_end_bal;
                        }
                        else if (i == 4)
                        {
                            bal2.Year4 = date.Year.ToString();
                            bal2.Year4Balance = bal.loc_end_bal;
                        }
                        else if (i == 5)
                        {
                            bal2.Year5 = date.Year.ToString();
                            bal2.Year5Balance = bal.loc_end_bal;
                        }
                    }
                }
            }

            return balances;
        }

        public List<coreReports.accountBalance> getFiveYearBalanceSheetSummary(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx)
        {
            List<coreReports.accountBalance> balances = new List<coreReports.accountBalance>();

            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                int i = 0;
                var date2 = startDate;
                for (var date = startDate; date <= endDate; date = date.AddYears(1))
                {
                    i = i + 1;
                    date2 = date.AddYears(1).AddSeconds(-1);
                    var yearBals = rent.get_bal_sht_sum(date, date2, noTx,true, costCenterID).ToList();
                    foreach (var bal in yearBals)
                    {
                        var bal2 = balances.FirstOrDefault(p => p.acct_id == bal.acct_id);
                        if (bal2 == null)
                        {
                            bal2 = new coreReports.accountBalance
                            {
                                acct_id = bal.acct_id,
                                acc_name = bal.acc_name,
                                acc_num = bal.acc_num,
                                cat_code = bal.cat_code,
                                cat_name = bal.cat_name,
                                head_name1 = bal.head_name1,
                                head_name2 = bal.head_name2,
                                head_name3 = bal.head_name3,
                                head_name4 = bal.head_name4,
                                head_name5 = bal.head_name5,
                                head_name6 = bal.head_name6,
                                head_name7 = bal.head_name7,
                                loc_beg_bal = bal.loc_beg_bal,
                                loc_end_bal = bal.loc_end_bal,
                                currency_id = bal.currency_id,
                                Year1 = endDate.Year.ToString(),
                                Year2 = endDate.AddYears(-1).Year.ToString(),
                                Year3 = endDate.AddYears(-2).Year.ToString(),
                                Year4 = endDate.AddYears(-3).Year.ToString(),
                                Year5 = endDate.AddYears(-4).Year.ToString()
                            };
                            balances.Add(bal2);
                        }
                        if (i == 1)
                        {
                            bal2.Year1 = date.Year.ToString();
                            bal2.Year1Balance = bal.loc_end_bal;
                        }
                        else if (i == 2)
                        {
                            bal2.Year2 = date.Year.ToString();
                            bal2.Year2Balance = bal.loc_end_bal;
                        }
                        else if (i == 3)
                        {
                            bal2.Year3 = date.Year.ToString();
                            bal2.Year3Balance = bal.loc_end_bal;
                        }
                        else if (i == 4)
                        {
                            bal2.Year4 = date.Year.ToString();
                            bal2.Year4Balance = bal.loc_end_bal;
                        }
                        else if (i == 5)
                        {
                            bal2.Year5 = date.Year.ToString();
                            bal2.Year5Balance = bal.loc_end_bal;
                        }
                    }
                }
            }

            return balances;
        }

        public List<coreReports.accountBalanceTab> getBalanceSheet(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, coreReports.periodType periodType)
        {
            var reportData = new List<coreReports.accountBalanceTab>();

            switch (periodType)
            {
                case coreReports.periodType.Quarterly:
                    getBalanceSheetQuarterly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Half_Yearly:
                    getBalanceSheetHalfYearly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Yearly:
                    getBalanceSheetYearly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Monthly:
                    getBalanceSheetMonthly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Weekly:
                    getBalanceSheetWeekly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Daily:
                    getBalanceSheetDaily(startDate, endDate, costCenterID, noTx, reportData);
                    break;
            }
            return reportData;
        }

        private void getBalanceSheetQuarterly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountBalanceTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    int quarterNumber = (sd.Month - 1) / 3 + 1;
                    DateTime qstart = new DateTime(sd.Year, (quarterNumber - 1) * 3 + 1, 1);
                    DateTime qend = qstart.Date.AddMonths(3).AddSeconds(-1);

                    var data = rent.get_bal_sht(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountBalanceTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_bal = r.bud_bal,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            credit = r.credit,
                            cur_rate = r.cur_rate,
                            currency_id = r.currency_id,
                            debit = r.debit,
                            frgn_beg_bal = r.frgn_beg_bal,
                            frgn_end_bal = r.frgn_end_bal,
                            Half_Year = ((qend.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7,
                            loc_beg_bal = r.loc_beg_bal,
                            loc_end_bal = r.loc_beg_bal,
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Quarter = ((qend.Month <= 3) ? 1 : ((qend.Month <= 6) ? 2 : ((qend.Month <= 9) ? 3 : 4))),
                            Year = (Int16)qend.Year
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }


        private void getBalanceSheetHalfYearly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountBalanceTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {  
                    DateTime qend = (sd.Month<7)? new DateTime(sd.Year, 6, 30, 23, 59,59)
                        : new DateTime(sd.Year, 12, 31, 23, 59, 59);

                    var data = rent.get_bal_sht(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountBalanceTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_bal = r.bud_bal,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            credit = r.credit,
                            cur_rate = r.cur_rate,
                            currency_id = r.currency_id,
                            debit = r.debit,
                            frgn_beg_bal = r.frgn_beg_bal,
                            frgn_end_bal = r.frgn_end_bal,
                            Half_Year = ((qend.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7,
                            loc_beg_bal = r.loc_beg_bal,
                            loc_end_bal = r.loc_beg_bal,
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol, 
                            Year = (Int16)qend.Year
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }


        private void getBalanceSheetYearly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountBalanceTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    DateTime qend =  new DateTime(sd.Year, 12, 31, 23, 59, 59);

                    var data = rent.get_bal_sht(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountBalanceTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_bal = r.bud_bal,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            credit = r.credit,
                            cur_rate = r.cur_rate,
                            currency_id = r.currency_id,
                            debit = r.debit,
                            frgn_beg_bal = r.frgn_beg_bal,
                            frgn_end_bal = r.frgn_end_bal, 
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7,
                            loc_beg_bal = r.loc_beg_bal,
                            loc_end_bal = r.loc_beg_bal,
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Year = (Int16)qend.Year
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }

        private void getBalanceSheetMonthly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountBalanceTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                { 
                    DateTime qend = (new DateTime(sd.Year, sd.Month, 1)).AddMonths(1).AddSeconds(-1);

                    var data = rent.get_bal_sht(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountBalanceTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_bal = r.bud_bal,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            credit = r.credit,
                            cur_rate = r.cur_rate,
                            currency_id = r.currency_id,
                            debit = r.debit,
                            frgn_beg_bal = r.frgn_beg_bal,
                            frgn_end_bal = r.frgn_end_bal,
                            Half_Year = ((sd.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7,
                            loc_beg_bal = r.loc_beg_bal,
                            loc_end_bal = r.loc_beg_bal,
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Quarter = ((qend.Month <= 3) ? 1 : ((qend.Month <= 6) ? 2 : ((qend.Month <= 9) ? 3 : 4))),
                            Year = (Int16)qend.Year,
                            Month = sd.Month
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }

        private void getBalanceSheetWeekly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountBalanceTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    DateTime startOfWeek = sd.Date.AddDays(-(int)sd.Date.DayOfWeek);
                    DateTime qend = startOfWeek.Date.AddDays(7).AddSeconds(-1);

                    var data = rent.get_bal_sht(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountBalanceTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_bal = r.bud_bal,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            credit = r.credit,
                            cur_rate = r.cur_rate,
                            currency_id = r.currency_id,
                            debit = r.debit,
                            frgn_beg_bal = r.frgn_beg_bal,
                            frgn_end_bal = r.frgn_end_bal,
                            Half_Year = ((sd.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7,
                            loc_beg_bal = r.loc_beg_bal,
                            loc_end_bal = r.loc_beg_bal,
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Quarter = ((qend.Month == 3) ? 1 : ((qend.Month == 6) ? 2 : ((qend.Month == 9) ? 3 : 4))),
                            Year = (Int16)qend.Year,
                            Month = sd.Month,
                            Week = ((qend.Day < 8) ? 1 : ((qend.Day < 15) ? 2 : ((qend.Day < 22) ? 3 : ((qend.Day < 29) ? 4 : 5))))
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }

        private void getBalanceSheetDaily(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountBalanceTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    DateTime qend = sd.Date.AddDays(1).AddSeconds(-1);

                    var data = rent.get_bal_sht(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountBalanceTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_bal = r.bud_bal,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            credit = r.credit,
                            cur_rate = r.cur_rate,
                            currency_id = r.currency_id,
                            debit = r.debit,
                            frgn_beg_bal = r.frgn_beg_bal,
                            frgn_end_bal = r.frgn_end_bal,
                            Half_Year = ((sd.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7,
                            loc_beg_bal = r.loc_beg_bal,
                            loc_end_bal = r.loc_beg_bal,
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Quarter = ((qend.Month == 3) ? 1 : ((qend.Month == 6) ? 2 : ((qend.Month == 9) ? 3 : 4))),
                            Year = (Int16)qend.Year,
                            Month = sd.Month,
                            Week = ((qend.Day < 8) ? 1 : ((qend.Day < 15) ? 2 : ((qend.Day < 22) ? 3 : ((qend.Day < 29) ? 4 : 5)))),
                            Day = qend.Day
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }

        public List<coreReports.accountAmountTab> getOperatingStatement(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, coreReports.periodType periodType)
        {
            var reportData = new List<coreReports.accountAmountTab>();

            switch (periodType)
            {
                case coreReports.periodType.Quarterly:
                    getOperatingStatementQuarterly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Half_Yearly:
                    getOperatingStatementHalfYearly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Yearly:
                    getOperatingStatementYearly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Monthly:
                    getOperatingStatementMonthly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Weekly:
                    getOperatingStatementWeekly(startDate, endDate, costCenterID, noTx, reportData);
                    break;
                case coreReports.periodType.Daily:
                    getOperatingStatementDaily(startDate, endDate, costCenterID, noTx, reportData);
                    break;
            }
            return reportData;
        }

        private void getOperatingStatementQuarterly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountAmountTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    int quarterNumber = (sd.Month - 1) / 3 + 1;
                    DateTime qstart = new DateTime(sd.Year, (quarterNumber - 1) * 3 + 1, 1);
                    DateTime qend = qstart.Date.AddMonths(3).AddSeconds(-1);

                    var data = rent.get_op_stmt(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountAmountTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_amt = r.bud_amt,
                            bud_ytd_amt = r.bud_ytd_amt,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            currency_id = r.currency_id,
                            acc_amt_2 = r.acc_amt_2, 
                            Half_Year = ((qend.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7,
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Quarter = ((qend.Month <= 3) ? 1 : ((qend.Month <= 6) ? 2 : ((qend.Month <= 9) ? 3 : 4))),
                            Year = (Int16)qend.Year
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }


        private void getOperatingStatementHalfYearly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountAmountTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    DateTime qend = (sd.Month < 7) ? new DateTime(sd.Year, 6, 30, 23, 59, 59)
                        : new DateTime(sd.Year, 12, 31, 23, 59, 59);

                    var data = rent.get_op_stmt(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountAmountTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_amt = r.bud_amt,
                            bud_ytd_amt = r.bud_ytd_amt,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            currency_id = r.currency_id,
                            acc_amt_2 = r.acc_amt_2, 
                            Half_Year = ((qend.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7, 
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Year = (Int16)qend.Year
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }


        private void getOperatingStatementYearly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountAmountTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    DateTime qend = new DateTime(sd.Year, 12, 31, 23, 59, 59);

                    var data = rent.get_op_stmt(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountAmountTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_amt = r.bud_amt,
                            bud_ytd_amt = r.bud_ytd_amt,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            currency_id = r.currency_id,
                            acc_amt_2 = r.acc_amt_2, 
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7, 
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Year = (Int16)qend.Year
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }

        private void getOperatingStatementMonthly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountAmountTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    DateTime qend = (new DateTime(sd.Year, sd.Month, 1)).AddMonths(1).AddSeconds(-1);

                    var data = rent.get_op_stmt(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountAmountTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_amt = r.bud_amt,
                            bud_ytd_amt = r.bud_ytd_amt,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            currency_id = r.currency_id,
                            acc_amt_2 = r.acc_amt_2, 
                            Half_Year = ((sd.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7, 
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Quarter = ((qend.Month <= 3) ? 1 : ((qend.Month <= 6) ? 2 : ((qend.Month <= 9) ? 3 : 4))),
                            Year = (Int16)qend.Year,
                            Month = sd.Month
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }

        private void getOperatingStatementWeekly(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountAmountTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    DateTime startOfWeek = sd.Date.AddDays(-(int)sd.Date.DayOfWeek);
                    DateTime qend = startOfWeek.Date.AddDays(7).AddSeconds(-1);

                    var data = rent.get_op_stmt(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountAmountTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_amt = r.bud_amt,
                            bud_ytd_amt = r.bud_ytd_amt,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name,
                            currency_id = r.currency_id,
                            acc_amt_2 = r.acc_amt_2, 
                            Half_Year = ((sd.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7, 
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Quarter = ((qend.Month == 3) ? 1 : ((qend.Month == 6) ? 2 : ((qend.Month == 9) ? 3 : 4))),
                            Year = (Int16)qend.Year,
                            Month = sd.Month,
                            Week = ((qend.Day < 8) ? 1 : ((qend.Day < 15) ? 2 : ((qend.Day < 22) ? 3 : ((qend.Day < 29) ? 4 : 5))))
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }

        private void getOperatingStatementDaily(DateTime startDate, DateTime endDate,
            int? costCenterID, bool noTx, List<coreReports.accountAmountTab> reportData)
        {
            using (var rent = new coreReports.reportEntities())
            {
                rent.Database.CommandTimeout = 6000;
                var sd = startDate;
                while (sd <= endDate)
                {
                    DateTime qend = sd.Date.AddDays(1).AddSeconds(-1);

                    var data = rent.get_op_stmt(sd, qend, noTx, costCenterID).ToList();
                    foreach (var r in data)
                    {
                        reportData.Add(new coreReports.accountAmountTab
                        {
                            acc_name = r.acc_name,
                            acc_num = r.acc_num,
                            acct_id = r.acct_id,
                            bud_amt = r.bud_amt,
                            bud_ytd_amt = r.bud_ytd_amt,
                            cat_code = r.cat_code,
                            cat_name = r.cat_name, 
                            currency_id = r.currency_id,
                            acc_amt_2 = r.acc_amt_2, 
                            Half_Year = ((sd.Month < 7) ? 1 : 2),
                            head_name1 = r.head_name1,
                            head_name2 = r.head_name2,
                            head_name3 = r.head_name3,
                            head_name4 = r.head_name4,
                            head_name5 = r.head_name5,
                            head_name6 = r.head_name6,
                            head_name7 = r.head_name7, 
                            major_name = r.major_name,
                            major_symbol = r.major_symbol,
                            minor_name = r.minor_name,
                            minor_symbol = r.minor_symbol,
                            Quarter = ((qend.Month == 3) ? 1 : ((qend.Month == 6) ? 2 : ((qend.Month == 9) ? 3 : 4))),
                            Year = (Int16)qend.Year,
                            Month = sd.Month,
                            Week = ((qend.Day < 8) ? 1 : ((qend.Day < 15) ? 2 : ((qend.Day < 22) ? 3 : ((qend.Day < 29) ? 4 : 5)))),
                            Day = qend.Day
                        });
                    }

                    sd = qend.AddDays(1).Date;
                }
            }
        }

    }
}
