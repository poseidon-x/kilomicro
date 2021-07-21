using System;
using System.Collections.Generic;

namespace coreLogic
{
    public interface IScheduleManager
    {
        global::System.Collections.Generic.List<global::coreLogic.repaymentSchedule> calculateSchedule(double amount, double rate, DateTime loanDate, int? gracePeriod, double tenure, int interestTypeID, int repaymentModeID, client cl);
        global::System.Collections.Generic.List<global::coreLogic.repaymentSchedule> calculateSchedule(double amount, double rate, DateTime loanDate, int? gracePeriod, double tenure, int interestTypeID, int repaymentModeID, global::System.Collections.Generic.List<global::coreLogic.repaymentSchedule> oldSched);
        global::System.Collections.Generic.List<global::coreLogic.repaymentSchedule> calculateScheduleM(double amount, double rate, DateTime loanDate, double tenure);
        global::System.Collections.Generic.List<global::coreLogic.repaymentSchedule> calculateScheduleSusu(global::coreLogic.susuAccount account, string userName);

        List<repaymentSchedule> calculateScheduleDailyGFI(
            double amount, double rate, DateTime loanDate, int? gracePeriod,
            double tenure, int interestTypeID, int repaymentModeID, int loanProductId);

        List<repaymentSchedule> calculateScheduleTTL
            (double amount, double rate, DateTime loanDate, int? gracePeriod, double tenure,
            int interestTypeID, int repaymentModeID, loan ln);
    }
}
