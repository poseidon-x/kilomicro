using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.HelperClasses
{
    public class ControllerFileProcessor
    {

        public static controllerFile processFile(controllerFile file, coreLoansEntities le, DateTime date)
        {
            if (file.controllerFileDetails.Count > 0)
            {


                var staffIDs = (from d in file.controllerFileDetails
                                select d.staffID).Distinct();
                foreach (var staffID in staffIDs)
                {
                    int rsID1 = 0;
                    int rsID2 = 0;
                    int rsID3 = 0;
                    int rsID4 = 0;
                    int rsID5 = 0;
                    var amts = (from d in file.controllerFileDetails
                                where d.staffID == staffID
                                select d.monthlyDeduction).ToList();
                    foreach (var amt in amts)
                    {

                        var details = file.controllerFileDetails.Where(p => p.staffID == staffID && p.monthlyDeduction == amt).ToList();
                        var cl = le.privateCompanyStaffs.FirstOrDefault(p => p.employeeNumber == staffID);
                        var date2 = date;

                        foreach (var detail in details.Where(p => p.staffID == staffID).OrderByDescending(p => p.monthlyDeduction))
                        {
                            if (true /*CheckIfSelected(detail.fileDetailID)*/)
                            {
                                if (cl != null)
                                {
                                    var ln = le.loans.FirstOrDefault(p => p.loanNo.ToLower() == detail.loanNo.ToLower());
                                    var lowEnd = detail.monthlyDeduction * 0.9;
                                    var hiEnd = detail.monthlyDeduction * 1.1;
                                    var lnId = ln == null ? -1 : ln.loanID;


                                    repaymentSchedule rs;
                                    if (processWithLoanNumber(le, lnId, detail, details,
                                        ref date2, ref rsID1, ref rsID2, ref rsID3, ref rsID4, ref rsID5)) continue;

                                    if (processExactWithoutLoanNumber(le, cl, detail, details,
                                        ref date2, ref rsID1, ref rsID2, ref rsID3, ref rsID4, ref rsID5)) continue;

                                    if (processInExactWithoutLoanNumber(le, cl, detail, details,
                                        ref date2, ref rsID1, ref rsID2, ref rsID3, ref rsID4, ref rsID5)) continue;

                                    if (processInExacPreviousWithoutLoanNumber(le, cl, detail, details,
                                        ref date2, ref rsID1, ref rsID2, ref rsID3, ref rsID4, ref rsID5)) continue;
                                }
                                date2 = processNotFound(detail, details, date2);
                            }
                        }
                    }
                }
                
            }
            return file;
        }

        private static DateTime processNotFound(controllerFileDetail detail, List<controllerFileDetail> details, DateTime date2)
        {
            detail.notFound = true;
            if (detail.monthlyDeduction == 0)
            {
                detail.remarks = "Zero Deduction";
            }
            else if (details.Count > 1)
            {
                detail.remarks = "Duplicate Deduction";
            }
            else
            {
                detail.remarks = "No Match";
            }
            detail.overage = detail.monthlyDeduction;
            date2 = date2.AddMonths(-1);
            return date2;
        }

        private static bool processInExacPreviousWithoutLoanNumber(coreLoansEntities le, privateCompanyStaff cl,
            controllerFileDetail detail, List<controllerFileDetail> details, ref DateTime date2, ref int rsID1, ref int rsID2, ref int rsID3,
            ref int rsID4, ref int rsID5)
        {
            DateTime currentDate = date2;
            int rsId1 = rsID1;
            int rsId2 = rsID2;
            int rsId3 = rsID3;
            int rsId4 = rsID4;
            int rsId5 = rsID5;
            repaymentSchedule rs;
            rs = le.repaymentSchedules.Where(p => p.loan.clientID == cl.clientId
                                                  && detail.monthlyDeduction > 0
                                                  && p.interestBalance + p.principalBalance != detail.monthlyDeduction
                                                  && detail.monthlyDeduction > 0
                                                  // && p.repaymentDate <= currentDate 
                                                  && p.loan.loanStatusID == 4
                                                  && p.interestBalance + p.principalBalance > 0
                                                  && p.repaymentScheduleID != rsId1
                                                  && p.repaymentScheduleID != rsId2
                                                  && p.repaymentScheduleID != rsId3
                                                  && p.repaymentScheduleID != rsId4
                                                  && p.repaymentScheduleID != rsId5
                                                  && detail.monthlyDeduction > 0)
                .OrderByDescending(p => p.interestPayment + p.principalPayment)
                .ThenByDescending(p => p.repaymentDate)
                .FirstOrDefault();
            if (rs != null)
            {
                detail.repaymentScheduleID = rs.repaymentScheduleID;
                detail.overage = detail.monthlyDeduction - rs.interestBalance - rs.principalBalance;
                if (detail.overage > 0)
                {
                    detail.remarks = "Over Deduction";
                }
                else
                {
                    detail.remarks = "Under Deduction";
                }
                var scat = rs.loan.client.staffCategory1.First();
                if (details.Count(p => p.staffID == scat.employeeNumber.Trim()) == 1)
                {
                    date2 = rs.repaymentDate.AddMonths(-1);
                }
                if (rsID1 == 0) rsID1 = rs.repaymentScheduleID;
                else if (rsID2 == 0) rsID2 = rs.repaymentScheduleID;
                else if (rsID3 == 0) rsID3 = rs.repaymentScheduleID;
                else if (rsID4 == 0) rsID4 = rs.repaymentScheduleID;
                else if (rsID5 == 0) rsID5 = rs.repaymentScheduleID;
                return true;
            }
            return false;
        }

        private static bool processInExactWithoutLoanNumber(coreLoansEntities le, privateCompanyStaff cl, controllerFileDetail detail,
            List<controllerFileDetail> details, ref DateTime date2, ref int rsID1, ref int rsID2, ref int rsID3, ref int rsID4, ref int rsID5)
        {
            repaymentSchedule rs;
            DateTime currentDate = date2;
            int rsId1 = rsID1;
            int rsId2 = rsID2;
            int rsId3 = rsID3;
            int rsId4 = rsID4;
            int rsId5 = rsID5;
            rs = le.repaymentSchedules.Where(p => p.loan.clientID == cl.clientId
                                                  && detail.monthlyDeduction > 0
                                                  &&
                                                  Math.Abs(p.interestBalance + p.principalBalance - detail.monthlyDeduction) <
                                                  10
                                                  //&& p.repaymentDate <= currentDate 
                                                  && p.loan.loanStatusID == 4
                                                  && p.repaymentScheduleID != rsId1
                                                  && p.repaymentScheduleID != rsId2
                                                  && p.repaymentScheduleID != rsId3
                                                  && p.repaymentScheduleID != rsId4
                                                  && p.repaymentScheduleID != rsId5
                                                  && p.interestBalance + p.principalBalance > 0)
                .OrderByDescending(p => p.interestPayment + p.principalPayment)
                .ThenByDescending(p => p.repaymentDate)
                .FirstOrDefault();
            if (rs != null)
            {
                detail.repaymentScheduleID = rs.repaymentScheduleID;
                detail.overage = detail.monthlyDeduction - (rs.interestBalance + rs.principalBalance);




                double diff = (rs.interestBalance + rs.principalBalance) - detail.monthlyDeduction;
                if (detail.overage > 0)
                {
                    detail.remarks = "Over Deduction";
                }
                else
                {
                    detail.remarks = "Under Deduction";
                }
                var scat = rs.loan.client.staffCategory1.First();
                if (details.Count(p => p.staffID == scat.employeeNumber.Trim()) == 1)
                {
                    date2 = rs.repaymentDate.AddMonths(-1);
                }
                if (rsID1 == 0) rsID1 = rs.repaymentScheduleID;
                else if (rsID2 == 0) rsID2 = rs.repaymentScheduleID;
                else if (rsID3 == 0) rsID3 = rs.repaymentScheduleID;
                else if (rsID4 == 0) rsID4 = rs.repaymentScheduleID;
                else if (rsID5 == 0) rsID5 = rs.repaymentScheduleID;
                return true;
            }
            return false;
        }

        private static bool processExactWithoutLoanNumber(coreLoansEntities le, privateCompanyStaff cl, controllerFileDetail detail,
            List<controllerFileDetail> details, ref DateTime date2, ref int rsID1, ref int rsID2, ref int rsID3, ref int rsID4, ref int rsID5)
        {
            repaymentSchedule rs;
            DateTime currentDate = date2;
            int rsId1 = rsID1;
            int rsId2 = rsID2;
            int rsId3 = rsID3;
            int rsId4 = rsID4;
            int rsId5 = rsID5;
            rs = le.repaymentSchedules.Where(p => p.loan.clientID == cl.clientId
                                                  && detail.monthlyDeduction > 0
                                                  &&
                                                  Math.Abs(p.interestBalance + p.principalBalance - detail.monthlyDeduction) <=
                                                  1
                                                  //&& p.repaymentDate <= currentDate
                                                  && p.loan.loanStatusID == 4
                                                  && p.repaymentScheduleID != rsId1
                                                  && p.repaymentScheduleID != rsId2
                                                  && p.repaymentScheduleID != rsId3
                                                  && p.repaymentScheduleID != rsId4
                                                  && p.repaymentScheduleID != rsId5
                                                  && p.interestBalance + p.principalBalance > 0)
                .OrderByDescending(p => p.interestBalance + p.principalPayment)
                .ThenByDescending(p => p.repaymentDate)
                .FirstOrDefault();
            if (rs != null)
            {
                detail.repaymentScheduleID = rs.repaymentScheduleID;
                detail.overage = 0;
                detail.remarks = "Exact Match";
                var scat = rs.loan.client.staffCategory1.First();
                if (details.Count(p => p.staffID == scat.employeeNumber.Trim()) == 1)
                {
                    date2 = rs.repaymentDate.AddMonths(-1);
                }
                if (rsID1 == 0) rsID1 = rs.repaymentScheduleID;
                else if (rsID2 == 0) rsID2 = rs.repaymentScheduleID;
                else if (rsID3 == 0) rsID3 = rs.repaymentScheduleID;
                else if (rsID4 == 0) rsID4 = rs.repaymentScheduleID;
                else if (rsID5 == 0) rsID5 = rs.repaymentScheduleID;
                return true;
            }
            return false;
        }

        private static bool processWithLoanNumber(coreLoansEntities le, int lnId, controllerFileDetail detail, List<controllerFileDetail> details,
            ref DateTime date2, ref int rsID1, ref int rsID2, ref int rsID3, ref int rsID4, ref int rsID5)
        {
            DateTime currentDate = date2;

            var rs = le.repaymentSchedules
                .Where(p => p.loanID == lnId && (p.principalBalance > 1 || p.interestBalance > 1)
                            //&& p.repaymentDate <= currentDate 
                            && p.loan.loanStatusID == 4)
                .OrderByDescending(p => p.repaymentDate)
                .FirstOrDefault();
            if (rs != null)
            {
                detail.repaymentScheduleID = rs.repaymentScheduleID;
                detail.overage = detail.monthlyDeduction - (rs.interestBalance + rs.principalBalance);
                if (Math.Abs(detail.overage) >= 0 && Math.Abs(detail.overage) < 1)
                {
                    detail.remarks = "Exact Match";
                }
                else if (detail.overage > 1)
                {
                    detail.remarks = "Over Deduction";
                }
                else
                {
                    detail.remarks = "Under Deduction";
                }

                var scat = rs.loan.client.staffCategory1.First();
                if (details.Count(p => p.staffID == scat.employeeNumber.Trim()) == 1)
                {
                    date2 = rs.repaymentDate.AddMonths(-1);
                }
                if (rsID1 == 0) rsID1 = rs.repaymentScheduleID;
                else if (rsID2 == 0) rsID2 = rs.repaymentScheduleID;
                else if (rsID3 == 0) rsID3 = rs.repaymentScheduleID;
                else if (rsID4 == 0) rsID4 = rs.repaymentScheduleID;
                else if (rsID5 == 0) rsID5 = rs.repaymentScheduleID;
                return true;
            }
            return false;
        }
    }
}
