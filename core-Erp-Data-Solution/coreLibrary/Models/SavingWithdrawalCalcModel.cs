using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic
{
    public class SavingWithdrawalCalcModel
    {
        public double takeHomeAmount { get; set; }
        public double principalWithdrawal { get; set; }
        public double interestWithdrawal { get; set; }
        public double principalCharges { get; set; }
        public double interestCharges { get; set; }
        public chargeTypeTier chargeTypeTier { get; set; }
        public double totalCharges
        {
            get
            {
                return principalCharges + interestCharges;
            }
        }
        public double netWithdrawalAmount
        {
            get
            {
                return principalWithdrawal + interestWithdrawal ;
            }
        }
        public double grossWithdrawalWamount
        {
            get
            {
                return totalCharges + netWithdrawalAmount;
            }
        }
    }
}
