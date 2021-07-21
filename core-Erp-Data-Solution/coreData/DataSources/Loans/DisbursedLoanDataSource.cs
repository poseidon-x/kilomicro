using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Loans;

namespace coreData.DataSources.Loans
{
    [DataObject]
    public class DisbursedLoanDataSource
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public DisbursedLoanViewModel GetRunningLoans(DateTime startDate, DateTime endDate, int repaymentInterval)
        {
            var returnData = new DisbursedLoanViewModel
            {
                date = DateTime.Now.ToString("dd-MMM-yyyy"),
                totalAmountDisbursed = 0,
                totalAmountPayable = 0,
                totalRepaymentScheduleAmount = 0,
                totalAmountpaid = 0,
                totalNumberOfPaymentsInArears = 0,
                totalAmountInArears = 0,
                totalNumberOfPayments = 0,
                totalRepaymentAmountInArears = 0,
                totalLoanTenure = 0,
                company = GetCompanyProfile()
            };

            if (repaymentInterval == 0)
            {
                returnData.disbursedLoans = GetLoans(startDate, endDate);
            }
            else
            {
                returnData.disbursedLoans = GetLoansByRepaymentType(startDate, endDate, repaymentInterval);
                if (repaymentInterval == 1)
                {
                    returnData.repaymentType = "DAILY LOANS DETAILS REPORT";
                }
                else if (repaymentInterval == 2)
                {
                    returnData.repaymentType = "WEEKLY LOANS DETAILS REPORT";
                }
            }

            foreach (var item in returnData.disbursedLoans)
            {
                returnData.totalAmountDisbursed += item.amountDisbursed;
                returnData.totalAmountPayable += item.amountPayable;
                returnData.totalRepaymentScheduleAmount += item.repaymentScheduleAmount;
                returnData.totalAmountpaid += item.amountPaid;
                returnData.totalNumberOfPaymentsInArears += item.numberOfPaymentsInArears;
                returnData.totalNumberOfPayments += item.numberOfPayments;
                returnData.totalRepaymentAmountInArears += item.repaymentAmountInArears;
                returnData.totalAmountInArears += item.amountInArears;
            }

            return returnData;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public CompanyProfileViewModel GetCompanyProfile()
        {
            using (var ctx = new core_dbEntities())
            {
                var data = ctx.comp_prof
                    .Select(p => new CompanyProfileViewModel
                    {
                        companyProfileId = p.comp_prof_id,
                        companyName = p.comp_name,
                        companyLogo = p.logo,
                        companyAddressLine = p.addr_line_1,
                        companyPhoneNumber = p.phon_num,
                        companyEmail = p.email,
                        companyCityId = p.city_id,
                        companyCountryId = p.country_id
                    }).First();

                data.companyCity = ctx.cities.FirstOrDefault(p => p.city_id == data.companyCityId).city_name;
                data.companyCountry =
                    ctx.countries.FirstOrDefault(p => p.country_id == data.companyCountryId).country_name;

                return data;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<DisbursedLoanDetailViewModel> GetLoans(DateTime startDate, DateTime endDate)
        {
            var no = 0;
            using (var le = new coreLoansEntities())
            {
                var data = le.loans
                    .Where(i => i.approvedBy != null && i.amountDisbursed > 0
                        && i.disbursementDate >= startDate && i.disbursementDate <= endDate
                        && !i.closed && i.balance > 0)
                    .Join(le.clients,
                    l => l.clientID, c => c.clientID, (l, c) => new DisbursedLoanDetailViewModel
                    {
                        amountDisbursed = l.amountDisbursed,
                        amountPayable = Math.Round((l.amountDisbursed + (l.interestRate / 100 * l.amountDisbursed)), 2),
                        loanId = l.loanID,
                        loanNumber = l.loanNo,
                        date = l.disbursementDate,
                        clientId = l.clientID,
                        clientName = c.surName + " " + c.otherNames,
                        loanTenure = l.loanTenure,
                    }).OrderBy(p => p.clientName)
                    .ToList();

                foreach (var item in data)
                {
                    var schedule = le.repaymentSchedules
                        .Where(p => p.loanID == item.loanId)
                        .OrderByDescending(p => p.repaymentDate)
                        .First();

                    item.repaymentScheduleAmount = schedule.principalPayment + schedule.interestPayment;
                    var repaymentType = le.repaymentTypes.Where(p => p.repaymentTypeName
                        .Contains("Principal and Interest"));
                    item.numberOfPayments = le.loanRepayments.Count(p => p.loanID == item.loanId
                        && repaymentType.Any(i => i.repaymentTypeID == p.repaymentTypeID));
                    item.disbursementDate = string.Format("{0:dd-MMM-yyyy}", item.date);
                    item.totalRepaymentDays = le.repaymentSchedules.Count(p => p.loanID == item.loanId);
                    item.no = ++no;
                    item.numberOfPaymentsInArears = item.totalRepaymentDays - item.numberOfPayments;
                    item.amountPaid = item.repaymentScheduleAmount * item.numberOfPayments;
                    item.repaymentAmountInArears = item.repaymentScheduleAmount * (item.totalRepaymentDays - item.numberOfPayments);
                }
                return data;
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<DisbursedLoanDetailViewModel> GetLoansByRepaymentType(DateTime startDate, DateTime endDate, int repaymentInterval)
        {
            var no = 0;
            using (var le = new coreLoansEntities())
            {
                var data = le.loans
                    .Where(i => i.approvedBy != null && i.amountDisbursed > 0 
                        && i.loanTypeID == repaymentInterval
                        && i.disbursementDate >= startDate && i.disbursementDate <= endDate)
                    .Join(le.clients,
                    l => l.clientID, c => c.clientID, (l, c) => new DisbursedLoanDetailViewModel
                    {
                        amountDisbursed = l.amountDisbursed,
                        amountPayable = Math.Round((l.amountDisbursed + (l.interestRate/100 * l.amountDisbursed)), 2),
                        loanId = l.loanID,
                        loanNumber = l.loanNo,
                        date = l.disbursementDate,
                        clientId = l.clientID,
                        clientName = c.surName + " " + c.otherNames,
                        loanTenure = l.loanTenure,
                    }).OrderBy(p => p.clientName)
                    .ToList();

                foreach (var item in data)
                {
                    var schedule = le.repaymentSchedules
                        .Where(p => p.loanID == item.loanId)
                        .OrderByDescending(p => p.repaymentDate)
                        .First();
                    var repaymentType = le.repaymentTypes.Where(p => p.repaymentTypeName
                        .Contains("Principal and Interest"));
                    item.numberOfPayments = le.loanRepayments.Count(p => p.loanID == item.loanId
                        && repaymentType.Any(i => i.repaymentTypeID == p.repaymentTypeID));
                    item.disbursementDate = string.Format("{0:dd-MMM-yyyy}", item.date);

                    item.repaymentScheduleAmount = Math.Round(schedule.principalPayment + schedule.interestPayment,2);
                    item.numberOfPayments = le.loanRepayments.Count(p => p.loanID == item.loanId);
                    item.totalRepaymentDays = le.repaymentSchedules.Count(p => p.loanID == item.loanId);
                    item.no = ++no;
                    item.numberOfPaymentsInArears = item.totalRepaymentDays - item.numberOfPayments;
                    item.amountPaid = item.repaymentScheduleAmount * item.numberOfPayments;
                    item.repaymentAmountInArears = item.repaymentScheduleAmount * (item.totalRepaymentDays - item.numberOfPayments);
                }
                return data;
            }
        }
    }
}
