using System;
namespace coreLogic
{
    public interface IRepaymentsManager
    {
        void ApplyCheck(coreLogic.coreLoansEntities le, int loanCheckID, string userName, int bankID, DateTime cashDate, coreLogic.loan ln);
        void ApplyNegativeBalanceToLoan(coreLogic.loan ln, coreLogic.coreLoansEntities le, coreLogic.core_dbEntities ent, double amountPaid, DateTime paymentDate, string userName, coreLogic.loan ln2);
        void CashierCheckReceipt(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.cashierReceipt r, coreLogic.core_dbEntities ent, string userName, int? crAccNo, ref coreLogic.jnl_batch batch);
        void CashierReceipt(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.cashierReceipt r, coreLogic.core_dbEntities ent, string userName);
        void ClearOffInterest(coreLogic.coreLoansEntities le, coreLogic.core_dbEntities ent, coreLogic.loan ln, string userName);
        void CloseCashierReceipt(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.cashierReceipt r, coreLogic.core_dbEntities ent, string userName, ref coreLogic.jnl_batch batch);
        void CreatePenalty(coreLogic.coreLoansEntities le, coreLogic.core_dbEntities ent, coreLogic.loan ln, string userName, double amount, DateTime date);
        string ReceivePayment(coreLogic.coreLoansEntities le, coreLogic.loan ln, double amountPaid, DateTime paymentDate, string bID, string bankName, string checkNo, coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID, int? accountID, coreLogic.repaymentSchedule sch, ref coreLogic.jnl_batch batch);
        string ReceivePayment(coreLogic.coreLoansEntities le, coreLogic.loan ln, double amountPaid, DateTime paymentDate, string paymentTypeID, string bID, string bankName, string checkNo, coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID);
        string ReceivePayment(coreLogic.coreLoansEntities le, coreLogic.loan ln, double amountPaid, DateTime paymentDate, string paymentTypeID, string bID, string bankName, string checkNo, coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID, double amt2, int? accountID, ref coreLogic.jnl_batch batch);       
        void WriteOffInterest(coreLogic.coreLoansEntities le, coreLogic.core_dbEntities ent, coreLogic.loan ln, string userName);
        void CashierCheckReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName);
    }
}
