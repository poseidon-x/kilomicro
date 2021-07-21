using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;

namespace coreService
{
    public class LeaveModule
    {
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
                    coreLoansEntities le = new coreLoansEntities();
                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var pro = ent.comp_prof.FirstOrDefault();
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var staffs = le.staffs.Where(p => (p.lastLeaveDaysAccummulationDate== null
                        || p.lastLeaveDaysAccummulationDate < date)
                        && p.employmentStatusID==1 && p.employmentStartDate!=null).ToList();
                    foreach (var ln in staffs)
                    {
                        date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        //ln.staffLeaveBalances.Load();
                        var year=le.years.FirstOrDefault(p=>p.year1==date.Year);
                        DateTime? date2 = null;
                        if (ln.lastLeaveDaysAccummulationDate == null)
                            date2 = ln.employmentStartDate;
                        else
                            date2 = ln.employmentStartDate;

                        //ln.staffManagers1.Load();
                        var mgr = ln.staffManagers1.FirstOrDefault();
                        if (mgr != null)
                        {
                            //mgr.levelReference.Load();
                            if (mgr.level != null)
                            {
                                //mgr.level.levelLeaves.Load();
                                foreach (var ll in mgr.level.levelLeaves)
                                {
                                    var leave = ((date - date2.Value).TotalDays / 365.0) * ll.maxDaysPerAnnum;

                                    var balance = ln.staffLeaveBalances.FirstOrDefault(p => p.yearID == year.yearID);
                                    if (balance != null)
                                    {
                                        balance.leaveAccumulatedDays += leave;
                                        balance.leaveBalanceDays += leave;
                                    }
                                    else
                                    {
                                        ln.staffLeaveBalances.Add(new staffLeaveBalance
                                        {
                                            leaveAccumulatedDays = leave,
                                            leaveBalanceDays = leave,
                                            yearID = year.yearID
                                        });
                                    }
                                    ln.lastLeaveDaysAccummulationDate = date;
                                }
                            }
                        }
                    }
                    le.SaveChanges();
                    ent.SaveChanges();
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "LeaveModule.Main");
                }

                System.Threading.Thread.Sleep(1000);
                if (StopFlag == true) break;

            }
            Stopped = true;
        }
    }
}
