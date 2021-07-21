using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreReports
{
    public class accountBalanceTab : vw_acc_bals
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int Half_Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
    }

    public enum periodType : uint
    {
        EndOfPeriod = 0,
        Daily = 1,
        Weekly = 7,
        Fortnightly = 14,
        Monthly = 30,
        BiMonthly = 60,
        Quarterly = 91,
        Half_Yearly = 182,
        Yearly = 365
    }
}
