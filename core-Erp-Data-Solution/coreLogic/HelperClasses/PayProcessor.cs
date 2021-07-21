using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace coreLogic
{
    public class PayProcessor
    {
        IJournalExtensions journalextensions = new JournalExtensions(); 
        private static String[] months = new String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public static payMaster ProcessPay(int staffID, payCalendar calendar, coreLoansEntities le)
        {            
            payMaster mast = null;
            payMasterOverTime mastovertime = null;

            var staff = le.staffs.FirstOrDefault(p => p.staffID == staffID);
            if (staff != null)
            {
                CleanPay(staffID, calendar, le); 
                var benefits = staff.staffBenefits.FirstOrDefault();
                if (benefits != null && staff.employmentStatusID == 1)
                {
                    var basic = benefits.basicSalary;
                    var fullBasic = basic;
                    var remAmount = benefits.basicSalary;
                    var totalAllowance = 0.0;
                    var totalDeductionBeforeTax = 0.0;
                    var totalDeductionAfterTax = 0.0;
                    var totalPensionEmployer=0.0;
                    var totalPensionEmployee=0.0;
                    var grossSalary = 0.0;
                    var netSalary = 0.0;
                    var pc = staff.staffDaysWorkeds.FirstOrDefault(p => p.payCalendarID == calendar.payCalendarID);
                    if (pc != null && pc.daysWorked>0)
                    {
                        var ratio = (pc.daysWorked / ((double)calendar.daysInMonth));
                        if (ratio > 1) ratio = 1;
                        basic = basic * ratio;
                    }
                    else
                    {
                        basic = 0;
                    }

                    var taxableSalary = basic;
                    var totalTax = 0.0;
                    var totalTaxRelief = 0.0;
                    var totalLoanDeductions = 0.0;
                    var totalBIK = 0.0;
                    
                    var totalOverTimeHours = 0.0;
                    var totalOverTimeAmount = 0.0;
                    var overTimeTaxAmount = 0.0;

                    var saturdayAmount = 0.0;
                    var sundayAmount = 0.0;
                    var holidayAmount = 0.0;
                    var weekdayAfterWorkAmount  = 0.0;

                    mast = new payMaster { 
                        staffID=staffID,
                        payCalendarID=calendar.payCalendarID,
                        basicSalary=basic,
                        netSalary=0.0
                    };
                                      
                                        
                    foreach (var r in staff.staffAllowances.Where(p => p.isEnabled == true))
                    { 
                        var amt=r.amount;
                        if (r.allowanceType.isPercent == true)
                        {
                            amt = basic * r.amount / 100.0;
                        }
                        totalAllowance = totalAllowance + amt;
                        if (amt > 0)
                        {
                            mast.payMasterAllowances.Add(new payMasterAllowance
                            {
                                allowanceTypeID = r.allowanceTypeID,
                                amount = amt,
                                description = r.allowanceType.alllowanceTypeName,
                                isPercent = r.allowanceType.isPercent,
                                percentValue = 0
                            });
                        }
                        if (r.allowanceType.isTaxable == true)
                        {
                            if (r.allowanceType.addToBasicAndTax == false)
                            {
                                var t = amt * r.allowanceType.taxPercent / 100.0;
                                if (t > 0)
                                {
                                    mast.payMasterTaxes.Add(new payMasterTax
                                    {
                                        amount = t,
                                        description = r.allowanceType.alllowanceTypeName + " Tax"
                                    });
                                    totalTax += t;
                                }
                            }
                            else
                            {
                                taxableSalary += amt;
                            }
                        }
                    }
                    foreach (var r in staff.staffBenefitsInKinds.Where(p => p.isEnabled == true))
                    { 
                        var amt = r.amount;
                        if (r.benefitsInKind.isPercent == true)
                        {
                            amt = basic * r.amount / 100.0;
                        }
                        totalBIK = totalBIK + amt;
                        if (amt > 0)
                        {
                            mast.payMasterBenefitsInKinds.Add(new payMasterBenefitsInKind
                            {
                                benefitsInKindID = r.benefitsInKindID,
                                amount = amt,
                                description = r.benefitsInKind.benefitsInKindName,
                                isPercent = r.benefitsInKind.isPercent,
                                percentValue = 0
                            });
                        }
                    } 
                    foreach (var r in staff.staffDeductions.Where(p => p.isEnabled == true))
                    { 
                        var amt=0.0;
                        if (r.percentValue > 0)
                        {
                            amt=(r.percentValue * basic / 100.0);
                            totalDeductionAfterTax = totalDeductionAfterTax + amt;
                        }
                        else
                        {
                            amt=r.amount;
                            totalDeductionAfterTax = totalDeductionAfterTax + amt;
                        }
                        if (amt > 0)
                        {
                            mast.payMasterDeductions.Add(new payMasterDeduction
                            {
                                amount = amt,
                                deductionTypeID = r.deductionTypeID,
                                description = r.deductionType.deductionTypeName,
                                isPercent = r.percentValue > 0,
                                percentValue = r.percentValue
                            });
                        }
                    }
                    foreach (var r in staff.staffPensions.Where(p => p.isEnabled == true
                        && p.pensionType.isBeforeTax==true))
                    { 
                        var amt=0.0;
                        var amt2 = 0.0;
                        if (r.isPercent == true)
                        {
                            amt=(r.employeeAmount * fullBasic / 100.0);
                            totalPensionEmployee = totalPensionEmployee + amt;
                            amt2 = (r.employerAmount * fullBasic / 100.0);
                            totalPensionEmployer = totalPensionEmployer + amt2;
                        }
                        else
                        {
                            amt=r.employeeAmount;
                            totalPensionEmployee = totalPensionEmployee + amt;
                            amt2 = r.employerAmount;
                            totalPensionEmployer = totalPensionEmployer + amt2;
                        }
                        taxableSalary -= amt;
                        if (amt > 0 || amt2 > 0)
                        {
                            mast.payMasterPensions.Add(new payMasterPension
                            {
                                employeeAmount = amt,
                                employerAmount = amt2,
                                description = r.pensionType.pensionTypeName,
                                isPercent = r.isPercent,
                                isBeforeTax = r.pensionType.isBeforeTax,
                                pensionTypeID = r.pensionTypeID
                            });
                        }
                    }
                    var startDate = new DateTime(calendar.year, calendar.month, 1);
                    var endDate = (new DateTime(calendar.year, calendar.month, 1)).AddMonths(1).AddSeconds(-1);
                    foreach (var r in staff.staffLoans.Where(p => p.principalBalance>1 || p.interestBalance> 1))
                    {
                        var sched = r.staffLoanSchedules.FirstOrDefault(p => p.deductionDate >= startDate &&
                            p.deductionDate <= endDate);
                        if (sched != null)
                        {
                            var amt = sched.principalDeduction + sched.interestDeduction;  
                            totalLoanDeductions = totalLoanDeductions + amt;
                            mast.payMasterLoans.Add(new payMasterLoan
                            {
                                amountDeducted = amt,
                                principalDeducted = sched.principalDeduction,
                                interestDeducted = sched.interestDeduction,
                                description = "Loan: " + r.staffLoanType.loanTypeName +
                                " (Bal: " + (r.principalBalance + r.interestBalance - sched.principalDeduction
                                - sched.interestDeduction).ToString("#,##0.#0") + ")",
                                staffLoanID = r.staffLoanID
                            });
                        }
                    }
                    totalTax = CalculateTax(staff, taxableSalary, le);
                    mast.payMasterTaxes.Add(new payMasterTax
                    {
                        amount = totalTax,
                        description = "IRS Tax"
                    });
                    grossSalary = basic + totalAllowance + totalBIK;
                    foreach (var r in staff.staffPensions.Where(p => p.isEnabled == true
                       && p.pensionType.isBeforeTax == false))
                    {
                        //r.pensionTypeReference.Load();
                        var amt = 0.0;
                        var amt2 = 0.0;
                        if (r.isPercent == true)
                        {
                            amt = (r.employeeAmount * fullBasic / 100.0);
                            totalPensionEmployee = totalPensionEmployee + amt;
                            amt2 = (r.employerAmount * fullBasic / 100.0);
                            totalPensionEmployer = totalPensionEmployer + amt2;
                        }
                        else
                        {
                            amt = r.employeeAmount;
                            totalPensionEmployee = totalPensionEmployee + amt;
                            amt2 = r.employerAmount;
                            totalPensionEmployer = totalPensionEmployer + amt2;
                        }
                        if (amt > 0 || amt2 > 0)
                        {
                            mast.payMasterPensions.Add(new payMasterPension
                            {
                                employeeAmount = amt,
                                employerAmount = amt2,
                                description = r.pensionType.pensionTypeName,
                                isPercent = r.isPercent,
                                isBeforeTax = r.pensionType.isBeforeTax,
                                pensionTypeID = r.pensionTypeID
                            });
                        }
                    }
                    foreach (var r in staff.staffTaxReliefs.Where(p => p.isEnabled == true))
                    { 
                        var amt = r.amount;
                        if (totalTaxRelief + amt > totalTax)
                        {
                            amt = totalTax - totalTaxRelief;
                        }
                        totalTaxRelief = totalTaxRelief + amt;
                        if (amt > 0)
                        {
                            mast.payMasterTaxReliefs.Add(new payMasterTaxRelief
                                {
                                    taxReliefTypeID = r.taxReliefTypeID,
                                    amount = amt,
                                    description = r.taxReliefType.taxReliefTypeName
                                });
                        }
                    }

                    var staffManager = le.staffManagers.FirstOrDefault(p => p.staffID == staffID);
                    if (staffManager != null)
                    {

                        //var config = le.overTimeConfigs.FirstOrDefault(p => p.levelID == staffManager.levelID);
                        //foreach (var r in staff.overTimes.Where(p => p.payCalendarID == calendar.payCalendarID))
                        //{
                        //    totalOverTimeHours = r.saturdayHours + r.sundayHours + r.holidayHours + r.weekdayAfterWorkHours;

                        //    saturdayAmount = r.saturdayHours * config.saturdayHoursRate;
                        //    sundayAmount = r.sundayHours * config.sundayHoursRate;
                        //    holidayAmount = r.holidayHours * config.holidayHoursRate;
                        //    weekdayAfterWorkAmount = r.weekdayAfterWorkHours * config.weekdayAfterWorkHoursRate;

                        //    totalOverTimeAmount = totalOverTimeAmount + saturdayAmount + sundayAmount + holidayAmount + weekdayAfterWorkAmount;

                        //}

                        //if (config != null)
                        //{
                        //    var taxOverTimeAmount = (totalOverTimeAmount / fullBasic) * 100.0;
                        //    var taxPer = 0.0;
                        //    if (taxOverTimeAmount > 50)
                        //    {
                        //        overTimeTaxAmount = (config.overTime10PerTax * totalOverTimeAmount) / 100.0;
                        //        taxPer = config.overTime10PerTax;
                        //    }
                        //    else
                        //    {
                        //        overTimeTaxAmount = (config.overTime5PerTax * totalOverTimeAmount) / 100.0;
                        //        taxPer = config.overTime5PerTax;
                        //    }

                        //    if (totalOverTimeAmount > 0)
                        //    {
                        //        mast.payMasterOverTimes.Add(new payMasterOverTime
                        //        {
                        //            saturdayHoursAmount = saturdayAmount,
                        //            sundayHoursAmount = sundayAmount,
                        //            holidayHoursAmount = holidayAmount,
                        //            weekdayAfterWorkHoursAmount = weekdayAfterWorkAmount,
                        //            overTimeTaxAmount = overTimeTaxAmount
                        //        });

                        //        mast.payMasterTaxes.Add(new payMasterTax
                        //        {
                        //            amount = overTimeTaxAmount,
                        //            description = "Overtime Tax (" + taxPer.ToString() + "% Withholding)"
                        //        });
                        //    }
                        //}
                    }

                    netSalary = grossSalary - totalDeductionBeforeTax - totalDeductionAfterTax - totalPensionEmployee
                        - totalTax + totalTaxRelief - totalLoanDeductions
                        + totalOverTimeAmount - overTimeTaxAmount;
                    mast.netSalary = netSalary;
                    le.payMasters.Add(mast);
                }
            }

            return mast;
        }

        public  void PostPay(int staffID, payCalendar calendar, coreLoansEntities le, core_dbEntities ent,
            string userName)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            var acc = le.payrollPostingAccounts.FirstOrDefault();
            payMaster mast = le.payMasters.FirstOrDefault(p => p.payCalendarID == calendar.payCalendarID
                && p.staffID == staffID);
            var staff = le.staffs.FirstOrDefault(p=> p.staffID==staffID);
            var date = new DateTime(calendar.year, calendar.month, 1).AddMonths(1).AddSeconds(-1);
            if (mast != null && staff != null)
            {
                var payDate = (new DateTime(mast.payCalendar.year, mast.payCalendar.month, 1)).AddMonths(1).AddSeconds(-1);

                var totalDeductions = mast.payMasterDeductions.Sum(p => p.amount);
                var totalEmployeePension = mast.payMasterPensions.Sum(p => p.employeeAmount);
                var totalEmployerPension = mast.payMasterPensions.Sum(p => p.employerAmount);
                var totalLoanDeductions = mast.payMasterLoans.Sum(p => p.amountDeducted);
                var totalTax = mast.payMasterTaxes.Sum(p => p.amount);
                var staffSavingsAccount = le.staffSavings.FirstOrDefault(p => p.staffID == staffID);

                var noSavings = false;
                jnl_batch jb = null;
                if (staffSavingsAccount == null)
                {
                    noSavings = true;
                }
                else
                {
                    var savingsAccount = le.savings.FirstOrDefault(p=> p.savingID==staffSavingsAccount.savingID);
                    if (savingsAccount != null)
                    {
                        jb = journalextensions.Post("PR",
                                                    acc.payrollExpenseAccountID, savingsAccount.savingType.accountsPayableAccountID.Value, 
                                                    mast.netSalary, "Net Salary: " + calendar.year.ToString() + ", " + months[calendar.month - 1] +
                                                       ":" + staff.surName + ", " + staff.otherNames, pro.currency_id.Value,
                                                   date, staff.staffNo, ent, userName, null);
                        ent.jnl_batch.Add(jb);
                        var sa = new savingAdditional
                        {
                            creation_date = DateTime.Now,
                            creator = userName,
                            fxRate = 1.0,
                            interestBalance = 0,
                            lastPrincipalFxGainLoss = 0,
                            localAmount = mast.netSalary,
                            modeOfPaymentID = 1,
                            naration = "Net Salary: " + calendar.year.ToString() + ", " + months[calendar.month - 1],
                            posted = true,
                            principalBalance = savingsAccount.principalBalance,
                            savingAmount = mast.netSalary,
                            savingDate = date
                        };
                        savingsAccount.savingAdditionals.Add(sa);
                        savingsAccount.availablePrincipalBalance += mast.netSalary;
                        savingsAccount.principalBalance += mast.netSalary;
                        savingsAccount.amountInvested += mast.netSalary;
                    }
                    else
                    {
                        noSavings = true;
                    }
                }
                if (noSavings == true)
                {
                    jb = journalextensions.Post("PR",
                            acc.payrollExpenseAccountID, acc.netSalaryAccountID, 
                            mast.netSalary, "Net Salary: " + calendar.year.ToString() + ", " + months[calendar.month - 1] +
                               ":" + staff.surName + ", " + staff.otherNames, pro.currency_id.Value,
                           date, staff.staffNo, ent, userName, null);
                    ent.jnl_batch.Add(jb);
                }
                if (totalDeductions > 0)
                {
                    var jb2 = journalextensions.Post("PR",
                           acc.payrollExpenseAccountID, acc.voluntaryDeductionsAccountID, 
                           totalDeductions, "Voluntary Deductions: " + calendar.year.ToString() + ", " + months[calendar.month - 1] +
                              ":" + staff.surName + ", " + staff.otherNames, pro.currency_id.Value,
                          date, staff.staffNo, ent, userName, null);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);
                }

                if (totalEmployeePension > 0)
                {
                    var jb2 = journalextensions.Post("PR",
                           acc.payrollExpenseAccountID, acc.pensionsPayableAccountID, 
                           totalEmployeePension, "Employee Pension Contribution : " + calendar.year.ToString() + ", " + months[calendar.month - 1] +
                              ":" + staff.surName + ", " + staff.otherNames, pro.currency_id.Value,
                          date, staff.staffNo, ent, userName, null);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);
                }

                if (totalEmployerPension > 0)
                {
                    var jb2 = journalextensions.Post("PR",
                           acc.payrollExpenseAccountID, acc.pensionsPayableAccountID, 
                           totalEmployerPension, "Employer Pension Contribution : " + calendar.year.ToString() + ", " + months[calendar.month - 1] +
                              ":" + staff.surName + ", " + staff.otherNames, pro.currency_id.Value,
                          date, staff.staffNo, ent, userName, null);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);
                }

                if (totalLoanDeductions > 0)
                {
                    var jb2 = journalextensions.Post("PR",
                           acc.loansReceivableAccountID, acc.loansRepaymentsAccountID, 
                           totalLoanDeductions, "Loan Deduction : " + calendar.year.ToString() + ", " + months[calendar.month - 1] +
                              ":" + staff.surName + ", " + staff.otherNames, pro.currency_id.Value,
                          date, staff.staffNo, ent, userName, null);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);
                }

                if (totalTax > 0)
                {
                    var jb2 = journalextensions.Post("PR",
                           acc.payrollExpenseAccountID, acc.taxPayableAccountID, 
                           totalTax, "Tax Deduction : " + calendar.year.ToString() + ", " + months[calendar.month - 1] +
                              ":" + staff.surName + ", " + staff.otherNames, pro.currency_id.Value,
                          date, staff.staffNo, ent, userName, null);
                    var list = jb2.jnl.ToList();
                    jb.jnl.Add(list[0]);
                    jb.jnl.Add(list[1]);
                }

                var startDate = new DateTime(calendar.year, calendar.month, 1);
                var endDate = (new DateTime(calendar.year, calendar.month, 1)).AddMonths(1).AddSeconds(-1);
                foreach (var r in mast.payMasterLoans)
                {
                    //r.staffLoanReference.Load();
                    if (r.staffLoan != null)
                    {
                        r.staffLoan.principalBalance = r.staffLoan.principalBalance - r.principalDeducted;
                        r.staffLoan.interestBalance = r.staffLoan.interestBalance - r.interestDeducted;

                        r.staffLoan.staffLoanRepayments.Add(
                            new staffLoanRepayment
                            {
                                balanceAfter = r.staffLoan.principalBalance,
                                checkedBy = userName,
                                creationDate = DateTime.Now,
                                enteredBy = userName,
                                interestPaid = r.interestDeducted,
                                principalPaid = r.principalDeducted,
                                repaymentDate = payDate,
                                repaymentType = "D" 
                            });
                    }
                }
            }
        }

        public static double CalculateTax(staff staff, double taxableAmount, coreLoansEntities le)
        {
            var tax = 0.0;
            var running = taxableAmount;

            var taxTable = le.taxTables.OrderBy(p => p.sortOrder).ToList();
            foreach (var r in taxTable)
            {
                var ta = (r.amount > running) ? running : r.amount;
                var t = ta * r.taxPercent / 100.0;
                running -= ta;
                tax += t;
                if (running <= 0) break;
            }

            return tax;
        }

        public static void CleanPay(int staffID, payCalendar calendar, coreLoansEntities le)
        {
          
            var mast = le.payMasters.FirstOrDefault(p => p.staffID == staffID && p.payCalendarID == calendar.payCalendarID);
            if (mast != null)
            {
                //mast.payMasterOverTimes.Load();
                for (int i = mast.payMasterOverTimes.Count - 1; i >= 0; i--)
                {
                    var r = mast.payMasterOverTimes.ToList()[i];
                    le.payMasterOverTimes.Remove(r);
                }

                //mast.payMasterAllowances.Load();
                for (int i = mast.payMasterAllowances.Count - 1; i >= 0;i--)
                {
                    var r = mast.payMasterAllowances.ToList()[i];
                    le.payMasterAllowances.Remove(r);
                }
                //mast.payMasterBenefitsInKinds.Load();
                for (int i = mast.payMasterBenefitsInKinds.Count - 1; i >= 0; i--)
                {
                    var r = mast.payMasterBenefitsInKinds.ToList()[i];
                    le.payMasterBenefitsInKinds.Remove(r);
                }
                //mast.payMasterDeductions.Load();
                for (int i = mast.payMasterDeductions.Count - 1; i >= 0; i--)
                {
                    var r = mast.payMasterDeductions.ToList()[i];
                    le.payMasterDeductions.Remove(r);
                }
                //mast.payMasterLoans.Load();
                for (int i = mast.payMasterLoans.Count - 1; i >= 0; i--)
                {
                    var r = mast.payMasterLoans.ToList()[i];
                    le.payMasterLoans.Remove(r);
                }
                //mast.payMasterOneTimeDeductions.Load();
                for (int i = mast.payMasterOneTimeDeductions.Count - 1; i >= 0; i--)
                {
                    var r = mast.payMasterOneTimeDeductions.ToList()[i];
                    le.payMasterOneTimeDeductions.Remove(r);
                }
                //mast.payMasterPensions.Load();
                for (int i = mast.payMasterPensions.Count - 1; i >= 0; i--)
                {
                    var r = mast.payMasterPensions.ToList()[i];
                    le.payMasterPensions.Remove(r);
                }
                //mast.payMasterTaxes.Load();
                for (int i = mast.payMasterTaxes.Count - 1; i >= 0; i--)
                {
                    var r = mast.payMasterTaxes.ToList()[i];
                    le.payMasterTaxes.Remove(r);
                }
                //mast.payMasterTaxReliefs.Load();
                for (int i = mast.payMasterTaxReliefs.Count - 1; i >= 0; i--)
                {
                    var r = mast.payMasterTaxReliefs.ToList()[i];
                    le.payMasterTaxReliefs.Remove(r);
                }

                le.payMasters.Remove(mast);
            }
        }
    }
}
