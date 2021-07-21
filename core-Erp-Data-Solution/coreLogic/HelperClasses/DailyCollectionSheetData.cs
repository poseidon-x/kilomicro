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
    public class DailyCollectionSheetData
    {
        private coreLoansEntities le = new coreLoansEntities();
        private core_dbEntities ctx = new core_dbEntities();

        private DateTime collectionDate;
        private branch branch;

        //Constructor to intialize private variables
        public DailyCollectionSheetData(int branchId, DateTime collectionDate)
        {
            branch = le.branches.FirstOrDefault(p => p.branchID == branchId);
            this.collectionDate = collectionDate;
        }



        public DailyCollectionSheetViewModel GetData()
        {
            DailyCollectionSheetViewModel data = new DailyCollectionSheetViewModel
            {
                details = new List<DailyCollectionSheetDetailViewModel>()
            };


            var groups = getGroups();
            if (groups.Any())
            {
                foreach (var grp in groups)
                {
                    var details = getGroupDetails(grp.loanGroupId, collectionDate);
                    data.details.Add(details);
                }

            }
            return data;
        }

        public IEnumerable<loanGroup> getGroups()
        {
            return le.loanGroups.Where(p => p.staff.branchID == branch.branchID).ToList();
        }

        //public IEnumerable<loanGroup> getGroupOfficersByBranch()
        //{
        //    var data = le.loanGroups.Where(p => p.staff.branchID == branch.branchID).ToList();

        //    return le.loanGroups.ToList();
        //}

        public DailyCollectionSheetDetailViewModel getGroupDetails(int groupId, DateTime collectionDate)
        {
            DailyCollectionSheetDetailViewModel group = new DailyCollectionSheetDetailViewModel();

            var grp = le.loanGroups
                .Include(p => p.loanGroupClients)
                .FirstOrDefault(p => p.loanGroupId == groupId);
            group.groupName = grp.loanGroupName;
            group.staffName = grp.staff.surName + ", " + grp.staff.otherNames;

            List<int> loanGroupClientIds = grp.loanGroupClients.Select(p => p.clientId).ToList();

            var grpLoans = le.loans.Where(p => loanGroupClientIds.Contains(p.clientID) && p.loanTypeID == 10
                                               && p.loanStatusID == 4 && !p.closed && p.balance > 0);

            var securityDepForCollDate = grpLoans
                .Where(p => p.disbursementDate.Value == collectionDate).ToList();
            if (securityDepForCollDate.Any())
            {
                group.securityDeposit = securityDepForCollDate.Sum(p => p.securityDeposit);
                group.insurance = securityDepForCollDate.Sum(p => p.insuranceAmount);
            }

            foreach (var loan in grpLoans)
            {
                var dueRepayments = le.repaymentSchedules
                    .Where(p => p.loanID == loan.loanID && p.repaymentDate <= collectionDate)
                    .ToList();

                if (!dueRepayments.Any())
                { 
                    var expectRepaymentsPerCollectionDate = dueRepayments
                        .Where(p => p.repaymentDate == collectionDate).Sum(p => p.principalPayment + p.interestBalance);
                    group.paymentExpected += expectRepaymentsPerCollectionDate;
                }

                var loanRepayments = le.loanRepayments
                    .Where(p => p.loanID == loan.loanID && p.repaymentDate == collectionDate)
                    .ToList();
                if (!loanRepayments.Any())
                {
                    var loanRepaymentsPerCollectionDate = loanRepayments
                        .Where(p => p.repaymentDate == collectionDate).Sum(p => p.amountPaid);
                    group.cashPayment += loanRepaymentsPerCollectionDate;
                }

                var loanOustandingSchedules = le.repaymentSchedules
                    .Where(p => p.loanID == loan.loanID && p.repaymentDate == collectionDate && p.interestBalance>0
                    || p.principalBalance > 0)
                    .ToList();
                if (loanOustandingSchedules.Any())
                {
                    var loanOutstanding = loanOustandingSchedules.Sum(p => p.interestBalance + p.principalBalance);
                    group.outstanding += loanOutstanding;
                }


                var cashBookCharges = le.clientServiceCharges
                    .Where(p => p.clientId == loan.clientID && p.chargeDate == collectionDate)
                    .ToList();
                if (!cashBookCharges.Any())
                {
                    //var loanChargePerCollectionDate = loanRepayments
                    //    .Where(p => p.repaymentDate == collectionDate).Sum(p => p.amountPaid);
                    group.passbookPurchased += cashBookCharges.Sum(p => p.chargeAmount);
                }
            }
            group.receipt = group.securityDeposit + group.cashPayment + group.adjustment + group.outstanding
                + group.passbookPurchased;



            return group;
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


        private IEnumerable<groupModel> getGroupsWithRunningLoans()
        {
            var grps = le.loanGroupClients
                .Include(p => p.loanGroup)
                .Join(le.staffs, lg => lg.loanGroup.relationsOfficerStaffId, s => s.staffID, (lg, s) => new {lg, s})
                .Join(le.clients, lgs => lgs.lg.clientId, c => c.clientID, (lgs, c) => new {lgs, c})
                .Join(le.loans.Where(ln => ln.loanTypeID == 10), nlgs => nlgs.lgs.lg.clientId, l => l.clientID,
                    (nlgs, l) => new groupModel
                    {
                        officer = nlgs.lgs.s.surName + " " + nlgs.lgs.s.otherNames,
                        groupId = nlgs.lgs.lg.loanGroup.loanGroupId,
                        group = nlgs.lgs.lg.loanGroup.loanGroupName,
                        loanId = l.loanID

                    }).OrderBy(p => p.loanId).ToList();

            return grps;
        }

        
    }

    public class groupModel
    {
        public string officer { get; set; }
        public int groupId { get; set; }
        public string group { get; set; }
        public int loanId { get; set; }
        //public string officer { get; set; }
        //public string officer { get; set; }
        //public string officer { get; set; }

    }
}
