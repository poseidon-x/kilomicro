using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using coreLogic.Models;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Loans;

namespace coreLogic.HelperClasses
{
    public class DepositCertificateData
    {
        private coreLoansEntities le = new coreLoansEntities();
        private core_dbEntities ctx = new core_dbEntities();

        private deposit dep;
        private depositCertificateConfig config;

        //Constructor to intialize private variables
        public DepositCertificateData(int depositId)
        {
            dep = le.deposits
                .Include(p=>p.depositType)
                .FirstOrDefault(p => p.depositID == depositId);
            config = le.depositCertificateConfigs.FirstOrDefault();
        }

        public deposit getDeposit()
        {
            return dep;
        }

        public DepositCertificateViewModel getDepositDetails()
        {
            var comp = ctx.comp_prof.First();
            var client = le.clients.FirstOrDefault(p => p.clientID == dep.clientID);
            double intExp = 0;
            int periodIndays = 0;

            DepositCertificateViewModel dpVm;
            if (comp.comp_name.ToLower().Contains("ttl"))
            {
                //intExp = (dep.depositPeriodInDays.Value/365*dep.annualInterestRate/100)*dep.amountInvested;
                if (dep.period == 1) periodIndays = 30;
                else if (dep.period == 2) periodIndays = 60;
                else if (dep.period == 3) periodIndays = 91;
                else if (dep.period == 6) periodIndays = 182;
                else if (dep.period == 9) periodIndays = 273;
                else if (dep.period == 12) periodIndays = 365;

                //var a = Math.Round((periodIndays/365),6,6);
                var b = (dep.annualInterestRate/100);
                var n = (periodIndays/365)*(dep.annualInterestRate/100);
                intExp = ((periodIndays / 365) * (dep.annualInterestRate / 100)) * dep.amountInvested;

                dpVm = new DepositCertificateViewModel
                {
                    clientName = (client.surName + ", " + client.otherNames).ToUpper(),
                    depositAmount = dep.amountInvested,
                    amountInWords = convertAmountToWords(),
                    interestRate = Math.Round(dep.annualInterestRate, 2) + " % P.A",
                    interestExpected = intExp,
                    depositPeriod = dep.depositPeriodInDays + " DAYS",
                    depositPeriodInMonths = dep.period + " Months",
                    depositType = dep.depositType.depositTypeName,
                    depositDate = dep.firstDepositDate.ToString("d: dd-MMM-yyy"),
                    maturityDate = dep.maturityDate.Value.ToString("d: dd-MMM-yyy"),
                    maturitySum = intExp + dep.amountInvested,
                    earlyRedemptionText = config.earlyRedemptionText,
                    trustText = config.trustText,
                    authorityText = config.authorityText,
                    riskDisclosureText = config.riskDisclosureText,
                    compamy = GetCompanyProfile()
                };

            }
            else
            {
                
                if (dep.period == 3) periodIndays = 91;
                else if (dep.period == 6) periodIndays = 182;
                else if (dep.period == 12) periodIndays = 365;


                double p = dep.depositPeriodInDays/365.0;
                var r = (dep.annualInterestRate/100.0);
                var i = Math.Round(((periodIndays/365)*(dep.annualInterestRate/100)),8);
                intExp = (p * r) * dep.amountInvested;

                dpVm = new DepositCertificateViewModel
                {
                    clientName = client.clientTypeID == 5? client.companyName.ToUpper() :(client.surName + ", " + client.otherNames).ToUpper(),
                    depositAmount = dep.amountInvested,
                    amountInWords = convertAmountToWords(),
                    interestRate = Math.Round(dep.annualInterestRate,2) + " % P.A",
                    interestExpected = intExp,
                    depositPeriod = dep.depositPeriodInDays + " DAYS",
                    depositPeriodInMonths = dep.period + " Months",
                    depositType = dep.depositType.depositTypeName,
                    depositDate = dep.firstDepositDate.ToString("dd-MMM-yyy"),
                    maturityDate = dep.maturityDate.Value.ToString("dd-MMM-yyy"),
                    maturitySum = intExp + dep.amountInvested,
                    depositNumber = dep.depositNo,
                    compamy = GetCompanyProfile()
                };
            }
            
            return dpVm;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public CompanyProfileViewModel GetCompanyProfile()
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
            data.companyCountry = ctx.countries.FirstOrDefault(p => p.country_id == data.companyCountryId).country_name;

            return data;
        }

        private string convertAmountToWords()
        {
            var wholeNumberPart =
                (NumberToWordsConverter.NumberToWords((int) dep.amountInvested) + " Ghana cedis ").ToUpper();

            var fraction = (int)((dep.amountInvested - (int)dep.amountInvested) * 100);
            if (fraction > 0)
            {
                var fractionPart = (", "+NumberToWordsConverter.NumberToWords(fraction) + " Pesewas ").ToUpper();
                return wholeNumberPart + fractionPart + " - (GHS " + dep.amountInvested.ToString("N") + ")";
            }

            return wholeNumberPart + " - (GHS " + dep.amountInvested.ToString("N") + ")";


        }











    }
}
