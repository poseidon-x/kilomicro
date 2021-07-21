using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using coreData.ErrorLog;
using coreLogic.HelperClasses;

namespace coreLogic
{
    public class CashiersTillCheck
    {
        private coreLoansEntities le;
        private string userName;
        public CashiersTillCheck(string userName)
        {
            le = new coreLoansEntities();
            this.userName = userName;
        }

        public void CheckForOpenedTill(DateTime date)
        {
            var user =
                (new coreLogic.coreSecurityEntities()).users.First(
                    p => p.user_name.ToLower().Trim() == userName.ToLower().Trim());
            var ctl = le.cashiersTills
                .FirstOrDefault(p => p.userName.ToLower().Trim() == user.user_name.ToLower().Trim());
            var ctd =
                le.cashiersTillDays.FirstOrDefault(
                    p => p.cashiersTillID == ctl.cashiersTillID && p.tillDay == date.Date
                         && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException(user.full_name +"'s  till for " + date.Date.ToString("dd-MMM-yyyy")+" is not open");
            }          
        }

        public void CheckForDefinedTill()
        {
            bool tillDefined = false;
            var user =
                (new coreLogic.coreSecurityEntities()).users.First(
                    p => p.user_name.ToLower().Trim() == userName.ToLower().Trim());
            var ctl =
                le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == user.user_name.ToLower().Trim());
            if (ctl == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" +
                                               user.full_name + ")");
            }
        }


        public void CheckForcashierFunding(DateTime date)
        {
            bool tillDefined = false;
            var user =
                (new coreLogic.coreSecurityEntities()).users.First(
                    p => p.user_name.ToLower().Trim() == userName.ToLower().Trim());
            var ctl = le.cashiersTills
                .Include(p => p.cashierFunds)
                .FirstOrDefault(p => p.userName.ToLower().Trim() == user.user_name.ToLower().Trim());
            if (!ctl.cashierFunds.Any(p => p.fundDate.Date == date.Date))
            {
                throw new ApplicationException("There is no funds allocated to the currently logged in user (" +
                                               user.full_name + ")");
            }
        }

        public void CheckForInterTransferReceivingCashier(string userName)
        {
            var user =
                (new coreLogic.coreSecurityEntities()).users.First(
                    p => p.user_name.ToLower().Trim() == userName.ToLower().Trim());
            var ctl = le.cashiersTills
                .Include(p => p.cashierFunds)
                .FirstOrDefault(p => p.userName.ToLower().Trim() == userName.ToLower().Trim());


            var ctd =
                le.cashiersTillDays.FirstOrDefault(
                    p => p.cashiersTillID == ctl.cashiersTillID && p.tillDay == DateTime.Today
                         && p.open == true);
            if (ctd == null)
            {
                throw new ApplicationException(user.full_name + "'s  till for " + DateTime.Today.Date.ToString("dd-MMM-yyyy") + " is not open");
            }



            if (!ctl.cashierFunds.Any(p => p.fundDate.Date == DateTime.Today))
            {
                throw new ApplicationException("There is no funds allocated to Receiving Cashier (" +
                                               user.full_name + ")");
            }
        }
    }
}
