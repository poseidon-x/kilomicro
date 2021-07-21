using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreReports
{
    public class accountAmountTab : vw_op_stmt
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int Half_Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; } 
    }
}
