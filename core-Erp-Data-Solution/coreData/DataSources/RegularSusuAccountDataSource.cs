using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using System.Data.Entity;
using coreLogic.Models;

namespace coreData.DataSources
{
    [DataObject]
    public class RegularSusuAccountDataSource
    {

        private readonly coreLoansEntities le;

        public RegularSusuAccountDataSource()
        {
            var db2 = new coreLoansEntities();

            le = db2;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<RegularSusuAccountViewModel> GetGroupStatement(
            int? clientID, int? susuAccountID, int? susuGroupID, DateTime? startDate, DateTime? endDate)
        {
            var data = (
                from s in le.regularSusuAccounts
                from c in le.clients 
                from t in le.regularSusuContributions
                join cis in le.clientImages on c.clientID equals cis.clientID into cirs
                from ci in cirs.DefaultIfEmpty()
                join i in le.images on ci.imageID equals i.imageID into irs
                from ir in irs.DefaultIfEmpty()
                join st in le.staffs on s.staffID equals st.staffID into srs
                from sts in srs.DefaultIfEmpty()
                where s.clientID == c.clientID &&
                    s.regularSusuAccountID == t.regularSusuAccountID
                    && s.startDate != null
                    && (clientID == null || clientID == 0 || c.clientID == clientID)
                select new RegularSusuAccountViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    susuAccountID = s.regularSusuAccountID,
                    susuContributionID = t.regularSusuContributionID,
                    susuGroupID = 1,
                    susuGroupName = "Normal Susu",
                    susuPosition = "Normal Susu",
                    contributionRate = s.contributionAmount,
                    startDate = s.startDate.Value,
                    contributionDate = t.contributionDate,
                    contributionAmount = t.amount,
                    staffName = (sts.staffNo == null) ? "" : sts.surName + ", " + sts.otherNames,
                    susuAccountNO = s.regularSusuAccountNo,
                    narration = t.narration,
                    amountDisbursed = s.principalPaid,
                    interestAmount = s.interestAmount,
                    commisionAmount = 0,
                    clientPicture = ir.image1,
                    expectedtotalContribution = s.amountEntitled
                }
                ).ToList().OrderBy(p => p.contributionDate).ToList();

            var data2 = (
                from s in le.regularSusuAccounts
                from c in le.clients
                from t in le.regularSusuWithdrawals
                join cis in le.clientImages on c.clientID equals cis.clientID into cirs
                from ci in cirs.DefaultIfEmpty()
                join i in le.images on ci.imageID equals i.imageID into irs
                from ir in irs.DefaultIfEmpty()
                join st in le.staffs on s.staffID equals st.staffID into srs
                from sts in srs.DefaultIfEmpty()
                where s.clientID == c.clientID &&
                    s.regularSusuAccountID == t.regularSusuAccountId
                    && s.startDate != null
                    && (clientID == null || clientID == 0 || c.clientID == clientID)
                select new RegularSusuAccountViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    susuAccountID = s.regularSusuAccountID,
                    susuContributionID = t.regularSusuWithdrawalId,
                    susuGroupID = 1,
                    susuGroupName = "Normal Susu",
                    susuPosition = "Normal Susu",
                    contributionRate = s.contributionAmount,
                    startDate = s.startDate.Value,
                    contributionDate = t.withdrawalDate,
                    contributionAmount = -t.amount,
                    staffName = (sts.staffNo == null) ? "" : sts.surName + ", " + sts.otherNames,
                    susuAccountNO = s.regularSusuAccountNo,
                    narration = t.narration,
                    amountDisbursed = s.principalPaid,
                    interestAmount = s.interestAmount,
                    commisionAmount = 0,
                    clientPicture = ir.image1,
                    expectedtotalContribution = s.amountEntitled
                }
                ).ToList().OrderBy(p => p.contributionDate).ToList();
            data.AddRange(data2);
            data = data
                .OrderBy(p => p.contributionDate)
                .ToList();
            var ent = new core_dbEntities();
            var cp = ent.comp_prof.First();

            foreach (RegularSusuAccountViewModel record in data)
            {
                record.companyLogo = cp.logo;
                record.companyAddress = cp.addr_line_1;
                record.companyPhone = cp.phon_num;
            }

            foreach (var acc in data.Select(p => p.susuAccountID).Distinct())
            {
                double runningTotal = 0;

                foreach (var record in data.Where(p => p.susuAccountID == acc))
                {
                    runningTotal = runningTotal + record.contributionAmount;
                    record.cummulativePaid = runningTotal;
                }
            }

            return data;
        }
    }
}