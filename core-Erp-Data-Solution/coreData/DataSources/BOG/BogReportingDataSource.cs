using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreReports;

namespace coreData.DataSources.BOG
{
    [DataObject]
   public class BogReportingDataSource
    {
        private readonly Icore_dbEntities ctx;

        //call a constructor to instialize a the  context 
        public BogReportingDataSource()
        {
            ctx = new core_dbEntities();
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }
        public IEnumerable<getLoanByDemographic_Result> GetLoanStats(int demographicType, DateTime monthEndDate)
        {
            using (var rent = new reportEntities())
            {
                return rent.getLoanByDemographic(demographicType, monthEndDate).ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<getPublicDepositStats_Result> GetDepositStats(DateTime monthEndDate)
        {
            using (var rent = new reportEntities())
            {
                return rent.getPublicDepositStats(monthEndDate).ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public MF7B GetMf7B(DateTime endMonthDate, DateTime monthEndDate)
        {
            var startDate = new DateTime(monthEndDate.Year, monthEndDate.Month, 01);
            var endDate = new DateTime(monthEndDate.Year, monthEndDate.Month, DateTime.DaysInMonth(monthEndDate.Year, monthEndDate.Month));

            var comp = ctx.comp_prof.First();
            MF7B data = new MF7B
            {
                startDate = startDate,
                endDate = endDate,
                companyName = comp.comp_name,
                companyLogo = comp.logo,
                details = new List<spLoanReportBogMF7B_Result>()
            };

            using (var rent = new reportEntities())
            {
                data.details = rent.spLoanReportBogMF7B(startDate, endDate)
                    .OrderBy(p => p.disbursed)
                    .ToList();
            }
            if (data.details.Any())
            {
                data.totalDisbursed = data.details.Sum(p => p.amountDisbursed);
                data.totalOutsatnding = data.details.Sum(p => p.outstanding);
                data.totalSecurityValue = data.details.Sum(p => p.valueOfSecurity);
            }
            return data;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public ScheduleH GetDepositFromPublic(DateTime monthEndDate)
        {
            var startDate = new DateTime(monthEndDate.Year, monthEndDate.Month, 01);
            var endDate = new DateTime(monthEndDate.Year, monthEndDate.Month, DateTime.DaysInMonth(monthEndDate.Year, monthEndDate.Month));

            var comp = ctx.comp_prof.First();
            ScheduleH data = new ScheduleH
            {
                startDate = startDate.ToString("dd-MMM-yyyy"),
                endDate = endDate.ToString("dd-MMM-yyyy"),
                companyName = comp.comp_name,
                companyLogo = comp.logo,
                details = new List<ScheduleHDeatils>()
            };

            using (var rent = new reportEntities())
            {
                var dataToProcess = rent.spDepositFromPublicScheduleH(startDate, endDate).ToList();
                var savin = dataToProcess.Where(p => p.accountType == "Savings").ToList();
                var dep = dataToProcess.Where(p => p.accountType == "Fixed (Time) Deposit").ToList();

                if (savin.Any())
                {
                    ScheduleHDeatils sav = new ScheduleHDeatils
                    {
                        accountType = savin.First().accountType,
                        maleCount = savin.Where(p => p.gender == "M").Sum(p => p.numberOfAccountHolders),
                        femaleCount = savin.Where(p => p.gender == "F").Sum(p => p.numberOfAccountHolders),
                        noOfAccountExceedingFivePercent = savin.Where(p => p.criteria == "Exceeding 5% of Paid Capital").Sum(p => p.numberOfAccountHolders),
                        amountExceedingFivePercent = Math.Round(savin.Where(p => p.criteria == "Exceeding 5% of Paid Capital").Sum(p => p.amountInvested),2),
                        noOfAccountNotExceedingFivePercent = savin.Where(p => p.criteria == "Not Exceeding 5% of Paid Capital").Sum(p => p.numberOfAccountHolders),
                        amountNotExceedingFivePercent = Math.Round(savin.Where(p => p.criteria == "Not Exceeding 5% of Paid Capital").Sum(p => p.amountInvested),2)
                    };
                    data.details.Add(sav);
                }

                if (dep.Any())
                {
                    ScheduleHDeatils deposit = new ScheduleHDeatils
                    {
                        accountType = dep.First().accountType,
                        maleCount = dep.Where(p => p.gender == "M").Sum(p => p.numberOfAccountHolders),
                        femaleCount = dep.Where(p => p.gender == "F").Sum(p => p.numberOfAccountHolders),
                        noOfAccountExceedingFivePercent = dep.Where(p => p.criteria == "Exceeding 5% of Paid Capital").Sum(p => p.numberOfAccountHolders),
                        amountExceedingFivePercent = Math.Round(dep.Where(p => p.criteria == "Exceeding 5% of Paid Capital").Sum(p => p.amountInvested), 2),
                        noOfAccountNotExceedingFivePercent = dep.Where(p => p.criteria == "Not Exceeding 5% of Paid Capital").Sum(p => p.numberOfAccountHolders),
                        amountNotExceedingFivePercent = Math.Round(dep.Where(p => p.criteria == "Not Exceeding 5% of Paid Capital").Sum(p => p.amountInvested), 2)
                    };
                    data.details.Add(deposit);
                }                
            }
            if (data.details.Any())
            {
                data.totalMaleCount = data.details.Sum(p => p.maleCount);
                data.totalFemaleCount = data.details.Sum(p => p.femaleCount);
                data.totalNoOfAccountExceedingFivePercent = data.details.Sum(p => p.noOfAccountExceedingFivePercent);
                data.totalAmountExceedingFivePercent = data.details.Sum(p => p.amountExceedingFivePercent);
                data.totalNoOfAccountNotExceedingFivePercent = data.details.Sum(p => p.noOfAccountNotExceedingFivePercent);
                data.totalAmountNotExceedingFivePercent = data.details.Sum(p => p.amountNotExceedingFivePercent);

            }
            return data;
        }



    }

    public class MF7B
    {
        public string companyName { get; set; }
        public byte[] companyLogo { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public double? totalDisbursed { get; set; }
        public double? totalOutsatnding { get; set; }
        public double? totalSecurityValue { get; set; }
        public IEnumerable<spLoanReportBogMF7B_Result> details { get; set; }
    }

    public class ScheduleH
    {
        public string companyName { get; set; }
        public byte[] companyLogo { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int totalMaleCount { get; set; }
        public int totalFemaleCount { get; set; }
        public int totalNoOfAccountExceedingFivePercent { get; set; }
        public double totalAmountExceedingFivePercent { get; set; }
        public int totalNoOfAccountNotExceedingFivePercent { get; set; }
        public double totalAmountNotExceedingFivePercent { get; set; }
        public List<ScheduleHDeatils> details { get; set; }
    }

    public class ScheduleHDeatils
    {
        public string accountType { get; set; }
        public int maleCount { get; set; }
        public int femaleCount { get; set; }
        public double amountExceedingFivePercent { get; set; }
        public int noOfAccountExceedingFivePercent { get; set; }
        public double amountNotExceedingFivePercent { get; set; }
        public int noOfAccountNotExceedingFivePercent { get; set; }
    }
}
