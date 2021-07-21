using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Loans;
using coreLibrary;

namespace coreData.DataSources.Loans
{
    [DataObject]
    public class ClientServiceChargeDataSource
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public ClientChargeSummary GetClientServiceCharges(DateTime startDate, DateTime endDate, string cashier, int chargeTypeId)
        {
            coreLoansEntities le = new coreLoansEntities();
            var returnData = new ClientChargeSummary
            {
                charges = getData(le, startDate, endDate, cashier, chargeTypeId)
            };
            returnData.totalCharges = returnData.charges.Sum(p => p.amount);

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

        private List<ClientServiceChargeViewModel> getData(coreLoansEntities le, DateTime start, DateTime end, string cashier, int chargeTypeId) 
        {
            if (!isStringEmpty(cashier) && chargeTypeId > 0)
            {
                return le.clientServiceCharges
                    .Where(p => p.posted && p.chargeDate >= start && p.chargeDate <= end 
                        && p.creator.ToLower() == cashier && p.chargeTypeId == chargeTypeId)
                    .ToList()
                    .Select(p => new ClientServiceChargeViewModel
                    {
                        date = p.chargeDate.ToString("dd-MMM-yyyy"),
                        accountNO = p.client.accountNumber,
                        clientName = string.Format("{0}, {1}", p.client.surName, p.client.otherNames),
                        chargeType = p.chargeType.chargeTypeName,
                        amount = p.chargeAmount
                    }).ToList();
            }
            else if (isStringEmpty(cashier) && chargeTypeId > 0)
            {
                return le.clientServiceCharges
                    .Where(p => p.posted && p.chargeDate >= start && p.chargeDate <= end && p.chargeTypeId == chargeTypeId)
                    .ToList()
                    .Select(p => new ClientServiceChargeViewModel
                    {
                        date = p.chargeDate.ToString("dd-MMM-yyyy"),
                        accountNO = p.client.accountNumber,
                        clientName = string.Format("{0}, {1}", p.client.surName, p.client.otherNames),
                        chargeType = p.chargeType.chargeTypeName,
                        amount = p.chargeAmount
                    }).ToList();
            }
            else if (!isStringEmpty(cashier) && chargeTypeId < 1)
            {
                return le.clientServiceCharges
                    .Where(p => p.posted && p.chargeDate >= start && p.chargeDate <= end
                        && p.creator.ToLower() == p.creator.ToLower())
                    .ToList()
                    .Select(p => new ClientServiceChargeViewModel
                    {
                        date = p.chargeDate.ToString("dd-MMM-yyyy"),
                        accountNO = p.client.accountNumber,
                        clientName = p.client.surName + ", " + p.client.otherNames,
                        chargeType = p.chargeType.chargeTypeName,
                        amount = p.chargeAmount
                    }).ToList();
            }
            else 
            {
                return le.clientServiceCharges
                    .Where(p => p.posted && p.chargeDate >= start && p.chargeDate <= end)
                    .ToList()
                    .Select(p => new ClientServiceChargeViewModel
                    {
                        date = p.chargeDate.ToString("dd-MMM-yyyy"),
                        accountNO = p.client.accountNumber,
                        clientName = p.client.surName+", "+p.client.otherNames,
                        chargeType = p.chargeType.chargeTypeName,
                        amount = p.chargeAmount
                    }).ToList();
            }
            
        }

        private List<ClientServiceChargeViewModel> getDataWithCashier(coreLoansEntities le, DateTime start, DateTime end, string cashier)
        {
            return le.clientServiceCharges
                    .Where(p => p.posted && p.creator.ToLower() == cashier.ToLower() && p.chargeDate >= start 
                        && p.chargeDate <= end)
                        .ToList()
                    .Select(p => new ClientServiceChargeViewModel
                    {
                        date = p.chargeDate.ToString("dd-MMM-yyyy"),
                        accountNO = p.client.accountNumber,
                        clientName = p.client.surName + ", " + p.client.otherNames,
                        chargeType = p.chargeType.chargeTypeName,
                        amount = p.chargeAmount
                    }).ToList();
        }

        private bool isStringEmpty(string input) 
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) 
            {
                return true;
            }
            return false;
        }


        


        
        
    }
}
