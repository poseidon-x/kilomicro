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
    public class LoanProductBalanceDataSource
    {

        [DataObjectMethod(DataObjectMethodType.Select)]
        public LoanBalanceByProductViewModel GetLoansByDAte(DateTime startDate, DateTime endDate)
        {
            coreLoansEntities le = new coreLoansEntities();

            var returnData = new LoanBalanceByProductViewModel
            {
                totalAmountDisbursed = 0,
                totalLoans = 0,
                totalPrincipal = 0,
                totalInterest = 0,
                totalWriteOff = 0,
                totalPayable = 0,
                totalPrincipalPayment = 0,
                totalInterestPayment = 0,
                totalAmountPaid = 0,
                totalOustanding = 0,
                company = GetCompanyProfile(),
                //details = le.
            };

            //foreach (var item in returnData.disbursedLoans)
            //{
            //    returnData.totalAmountDisbursed += item.amountDisbursed;
            //    returnData.totalAmountPayable += item.amountPayable;
            //    returnData.totalRepaymentScheduleAmount += item.repaymentScheduleAmount;
            //    returnData.totalAmountpaid += item.amountPaid;
            //    returnData.totalNumberOfPaymentsInArears += item.numberOfPaymentsInArears;
            //    returnData.totalNumberOfPayments += item.numberOfPayments;
            //    returnData.totalRepaymentAmountInArears += item.repaymentAmountInArears;
            //    returnData.totalAmountInArears += item.amountInArears;
            //}

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
        


        
        
    }
}
