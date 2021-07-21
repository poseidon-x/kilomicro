using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreReports
{
    public class accountBalance: vw_acc_bals
    {
        public string Year1 { get; set; }
        public string Year2 { get; set; }
        public string Year3 { get; set; }
        public string Year4 { get; set; }
        public string Year5 { get; set; }

        //Balances
        public double Year1Balance { get; set; }
        public double Year2Balance { get; set; }
        public double Year3Balance { get; set; }
        public double Year4Balance { get; set; }
        public double Year5Balance { get; set; }

        //Credits
        public double Year1Credit { get; set; }
        public double Year2Credit { get; set; }
        public double Year3Credit { get; set; }
        public double Year4Credit { get; set; }
        public double Year5Credit { get; set; }

        //Debits
        public double Year1Debit { get; set; }
        public double Year2Debit { get; set; }
        public double Year3Debit { get; set; }
        public double Year4Debit { get; set; }
        public double Year5Debit { get; set; }
    }
}
