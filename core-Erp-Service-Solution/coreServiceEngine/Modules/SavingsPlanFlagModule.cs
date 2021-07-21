using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;

namespace coreService
{
    public class SavingsPlanFlagModule
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        public bool StopFlag;
        public bool Stopped;

        public void Main()
        {
            Stopped = false;
            StopFlag = false;

            while (!StopFlag)
            {
                try
                {
                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    DateTime endOfMonth = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
                    //flags is called only at end of month
                    //

                    if (date == endOfMonth)
                    {
                        coreLoansEntities le = new coreLoansEntities();
                        coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                        var lns =
                            le.savings.Where(
                                p => ((p.principalBalance > 0 || p.interestBalance > 0) && (p.maturityDate >= date)
                                      && (p.savingType.planID != null))).ToList();
                        var oneWeekAgo = date.AddDays(-7);
                        var oneMonthAgo = date.AddDays(-30);

                        
                        foreach (var ln in lns)
                        {
                            if (ln.savingType.planID == null) continue;
                            var una = ln.savingPlanFlags.FirstOrDefault(p => p.approved == null);
                            if (una != null) continue; //Skip if there is a pending re-assignment
                            una = ln.savingPlanFlags.FirstOrDefault(p => p.flagDate > oneMonthAgo);
                            if (una != null) continue; //Skip if there was aflagging less than a month ago
                            
                            var totalDeposits = 0.0;
                            var da = ln.savingAdditionals.ToList();
                            if (da.Count > 0)
                            {
                                totalDeposits = da.Sum(p => p.savingAmount);
                            }
                            var totalDays = (date - ln.firstSavingDate).TotalDays;
                            var totalDepositPeriodByPlanInDays = totalDays /ln.savingType.planID.Value;
                            var periodElapsed = Math.Ceiling(totalDepositPeriodByPlanInDays);

                            var perExp = (totalDeposits/ periodElapsed);
                            var newType = le.savingTypes.Where(p => p.maxPlanAmount > perExp && p.minPlanAmount < perExp
                                                                    && p.defaultPeriod == ln.savingType.defaultPeriod &&
                                                                    ln.savingTypeID != p.savingTypeID)
                                .OrderByDescending(p => p.minPlanAmount).FirstOrDefault();
                            if (newType != null && newType.savingTypeID != ln.savingTypeID)
                            {
                                ln.savingPlanFlags.Add(new savingPlanFlag
                                {
                                    approved = null,
                                    applied = null,
                                    appliedDate = null,
                                    approvedBy = null,
                                    approvedDate = null,
                                    currentPlanId = (byte) ln.savingTypeID,
                                    proposedPlanId = (byte) newType.savingTypeID,
                                    flagDate = date
                                });
                            }

                        }

                        //foreach (var ln in lns)
                        //{
                        //    var una = ln.savingPlanFlags.FirstOrDefault(p => p.approved == null);
                        //    if (una != null) continue; //Skip if there is a pending re-assignment
                        //    una = ln.savingPlanFlags.FirstOrDefault(p => p.flagDate > oneWeekAgo);
                        //    if (una != null) continue; //Skip if there was aflagging less than aweek ago

                        //    var totalDeposits = 0.0;
                        //    var da = ln.savingAdditionals.ToList();
                        //    if (da.Count > 0)
                        //    {
                        //        totalDeposits = da.Sum(p => p.savingAmount);
                        //    }
                        //    var totalDays = (date - ln.firstSavingDate).TotalDays; 

                        //    var perExp = (totalDeposits / totalDays) * ln.savingType.planID;
                        //    var newType = le.savingTypes.Where(p =>  p.maxPlanAmount > perExp && p.minPlanAmount < perExp
                        //         && p.defaultPeriod == ln.savingType.defaultPeriod && ln.savingTypeID != p.savingTypeID)
                        //        .OrderByDescending(p => p.minPlanAmount).FirstOrDefault(); 
                        //    if (newType!= null && newType.savingTypeID != ln.savingTypeID)
                        //    {
                        //        ln.savingPlanFlags.Add(new savingPlanFlag
                        //        {
                        //            approved = null,
                        //            applied = null,
                        //            appliedDate = null,
                        //            approvedBy = null,
                        //            approvedDate = null,
                        //            currentPlanId = (byte)ln.savingTypeID,
                        //            proposedPlanId = (byte)newType.savingTypeID,
                        //            flagDate = date
                        //        });
                        //    }

                        //}
                        le.SaveChanges();
                        ent.SaveChanges();

                        lns = null;
                        le.Dispose();
                        ent.Dispose();
                        le = null;
                        ent = null;
                        GC.Collect();
                    }
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "SavingsPlanFlagModule.Main");
                }

                System.Threading.Thread.Sleep(30000);
                if (StopFlag == true) break;
            }
            Stopped = true;
        } 
    }
}
