using System;
using System.Collections.Generic;

namespace coreLogic
{
    public interface IDisbursementsManager
    {
        void CashierDisburse(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.core_dbEntities ent, coreLogic.cashierDisbursement d, string userName, coreLogic.susuAccount sc = null);
        void PostDisbursement(coreLogic.coreLoansEntities le, double? amountPaid, DateTime? disbDate, string bank, string paymentType, string checkNo, coreLogic.core_dbEntities ent, bool addFees, string userName, string paymentMode, string crAccountNo);
        void PostLoan(coreLogic.coreLoansEntities le, coreLogic.loan ln, double? amountPaid, double amountApproved, DateTime? disbDate, string bank, string paymentType, string checkNo, coreLogic.core_dbEntities ent, bool addFees, string userName, string paymentMode, string crAccountNo, coreLogic.saving sav = null, coreLogic.susuAccount sc = null);

    }
}
