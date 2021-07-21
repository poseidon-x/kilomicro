using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using System.Data.Entity;
using coreLogic.Models;
using coreLogic.Models.Savings;

namespace coreLogic
{
    [DataObject]
    public class SavingsTermSheetDataSource
    {

        private readonly IcoreLoansEntities le;

        public SavingsTermSheetDataSource()
        {
            var db2 = new coreLoansEntities(); 

            le= db2;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<SavingsTermSheetData> GetData(int savingId)
        {
            var data = (
                from s in le.savings
                from c in le.clients
                from sc in le.savingPlans
                where s.clientID == c.clientID
                      && s.savingID == sc.savingID
                      && s.savingID==savingId
                select new SavingsTermSheetData
                {
                    accountNumber = s.savingNo,
                    firstAmountInvested = s.savingAdditionals.FirstOrDefault().savingAmount,
                    plannedAmount = sc.plannedAmount,
                    clientName = c.surName + ", " + c.otherNames,
                    tenure = s.period,
                    frequency =
                        s.savingPlanID == 30
                            ? "Monthly"
                            : (s.savingPlanID == 1
                                ? "Daily"
                                : (s.savingPlanID == 7
                                    ? "Weekly"
                                    : (s.savingPlanID == 14 ? "Forthnightly" : (s.savingPlanID == 91 ? "Quarterly" : "")))),
                    firstDepositDate = s.savingAdditionals.FirstOrDefault().savingDate,
                    rate = s.interestRate,
                    maturityDate = s.maturityDate.Value,
                    planAmount = s.savingPlanAmount,
                    plannedDate = sc.plannedDate
                }
                )
                .ToList()
                .OrderBy(p => p.plannedDate)
                .ToList();

            var ent = new core_dbEntities();
            var cp = ent.comp_prof.First();
            
            foreach (SavingsTermSheetData record in data)
            {
                record.companyLogo = cp.logo;
                record.companyAddress = cp.addr_line_1;
                record.companyPhone = cp.phon_num;
            }
             
            return data;
        }
    }
}