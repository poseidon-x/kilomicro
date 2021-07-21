using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using System.Data.Entity;
using coreLogic.Models;

namespace coreLogic
{
    [DataObject]
    public class GroupSusuDataSource
    {

        private readonly coreLoansEntities le;

        public GroupSusuDataSource()
        {
            var db2 = new coreLoansEntities(); 

            le= db2;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<SusuContributionViewModel> GetGroupStatement(
            int? clientID,int? susuAccountID,int? susuGroupID,DateTime? startDate,DateTime? endDate)
        {
            var data = (
                from s in le.susuAccounts
                from c in le.clients
                from g in le.susuGroups
                from p in le.susuPositions
                from t in le.susuContributions
                join cis in le.clientImages on c.clientID equals cis.clientID into cirs
                from ci in cirs.DefaultIfEmpty()
                join i in le.images on ci.imageID equals i.imageID into irs
                from ir in irs.DefaultIfEmpty()
                join st in le.staffs on s.staffID equals st.staffID into srs
                from sts in srs.DefaultIfEmpty()
                where s.clientID == c.clientID && s.susuGroupID == g.susuGroupID
                    && s.susuPositionID == p.susuPositionID
                    & s.susuAccountID == t.susuAccountID
                    && s.startDate !=null
                    && (clientID==null || clientID == 0 || c.clientID==clientID)
                select new SusuContributionViewModel
                {
                    clientID = s.clientID,
                    clientName=c.surName+", "+c.otherNames,
                    susuAccountID = s.susuAccountID,
                    susuContributionID = t.susuContributionID,
                    susuGroupID = s.susuGroupID.Value,
                    susuGroupName = g.susuGroupName,
                    susuPosition = p.susuPositionName,
                    contributionRate = s.contributionAmount,
                    startDate = s.startDate.Value,
                    contributionDate= t.contributionDate,
                    contributionAmount = t.amount,
                    staffName = (sts.staffNo==null)?"": sts.surName+", "+ sts.otherNames,
                    susuAccountNO = s.susuAccountNo,
                    narration = t.narration,
                    amountDisbursed = s.principalPaid,
                    interestAmount = s.interestAmount,
                    commisionAmount = s.commissionAmount,
                    clientPicture = ir.image1,
                    expectedtotalContribution=s.amountEntitled
                }
                ).ToList().OrderBy(p=>p.contributionDate).ToList();

            var ent = new core_dbEntities();
            var cp = ent.comp_prof.First();
            

            foreach(SusuContributionViewModel record in data){
                record.companyLogo = cp.logo;
                record.companyAddress = cp.addr_line_1;
                record.companyPhone = cp.phon_num;
            }

            foreach(var acc in data.Select(p=>p.susuAccountID).Distinct()){
                double runningTotal = 0;

                foreach(var record in data.Where(p=>p.susuAccountID==acc)){
                    runningTotal = runningTotal + record.contributionAmount;
                    record.cummulativePaid = runningTotal;
                }
            }

            return data;
        }
    }
}