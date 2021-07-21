using System;
namespace coreLogic
{
    public interface ILoansHelper
    {
        void ApplyCheck(coreLoansEntities le, int loanCheckID, string userName, int bankID, DateTime cashDate, loan ln);
        System.Collections.Generic.List<repaymentSchedule> calculateSchedule(double amount, double rate, DateTime loanDate, int? gracePeriod, double tenure, int interestTypeID, int repaymentModeID);
        System.Collections.Generic.List<repaymentSchedule> calculateSchedule(double amount, double rate, DateTime loanDate, int? gracePeriod, double tenure, int interestTypeID, int repaymentModeID, System.Collections.Generic.List<repaymentSchedule> oldSched);
        System.Collections.Generic.List<repaymentSchedule> calculateScheduleM(double amount, double rate, DateTime loanDate, double tenure);
        System.Collections.Generic.List<repaymentSchedule> calculateScheduleSusu(susuAccount account, string userName);
        void CashierCheckReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName, int? crAccNo, ref jnl_batch batch);
        void CashierDisburse(coreLoansEntities le, loan ln, core_dbEntities ent, cashierDisbursement d, string userName, susuAccount sc = null);
        void CashierReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName);
        void ClearOffInterest(coreLoansEntities le, core_dbEntities ent, loan ln, string userName);
        void CloseCashierReceipt(coreLoansEntities le, loan ln, cashierReceipt r, core_dbEntities ent, string userName, ref jnl_batch batch);
        void CreatePenalty(coreLoansEntities le, core_dbEntities ent, loan ln, string userName, double amount, DateTime date);
        void DisburseLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved, double amountDisbursed, DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent, bool addFees, string userName, string paymentMode, string crAccountNo, bool post, saving sav = null, susuAccount sc = null);
        void PostDepositAdditional(depositAdditional da, string userName, core_dbEntities ent, coreLoansEntities le, cashiersTill ct);
        void PostDepositsWithdrawal(depositWithdrawal da, string userName, core_dbEntities ent, coreLoansEntities le, cashiersTill ct);
        void PostDisbursement(coreLoansEntities le, double? amountPaid, DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent, bool addFees, string userName, string paymentMode, string crAccountNo);
        void PostInvestmentAdditional(investmentAdditional da, string userName, core_dbEntities ent, coreLoansEntities le, cashiersTill ct);
        void PostInvestmentWithdrawal(investmentWithdrawal da, string userName, core_dbEntities ent, coreLoansEntities le, cashiersTill ct);
        void PostLoan(coreLoansEntities le, loan ln, double? amountPaid, double amountApproved, DateTime? disbDate, string bank, string paymentType, string checkNo, core_dbEntities ent, bool addFees, string userName, string paymentMode, string crAccountNo, saving sav = null, susuAccount sc = null);
        void PostSavingAdditional(savingAdditional da, string userName, core_dbEntities ent, coreLoansEntities le, cashiersTill ct);
        void PostSavingsWithdrawal(savingWithdrawal da, string userName, core_dbEntities ent, coreLoansEntities le, cashiersTill ct);
        void PostSusuContribution(coreLoansEntities le, core_dbEntities ent, susuContribution sc, string userName);
        void PostSusuDisbursement(coreLoansEntities le, core_dbEntities ent, susuAccount sc, string userName);
        void PostRegularSusuContribution(coreLoansEntities le, core_dbEntities ent, regularSusuContribution sc, string userName);
        void PostRegularSusuDisbursement(coreLoansEntities le, core_dbEntities ent, regularSusuAccount sc, string userName);
        string ReceivePayment(coreLoansEntities le, loan ln, double amountPaid, DateTime paymentDate, string bID, string bankName, string checkNo, coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID, int? accountID, repaymentSchedule sch, ref jnl_batch batch);
        string ReceivePayment(coreLoansEntities le, loan ln, double amountPaid, DateTime paymentDate, string paymentTypeID, string bID, string bankName, string checkNo, coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID);
        string ReceivePayment(coreLoansEntities le, loan ln, double amountPaid, DateTime paymentDate, string paymentTypeID, string bID, string bankName, string checkNo, coreLogic.core_dbEntities ent, string userName, int modeOfPaymentID, double amt2, int? accountID, ref jnl_batch batch);
        void ReverseDisbursement(coreLoansEntities le, loan ln, loanTranch lt, core_dbEntities ent, string userName);
        void ReverseFee(coreLoansEntities le, loan ln, coreLogic.core_dbEntities ent, loanFee lrp, string userName);
        void ReverseInterest(coreLoansEntities le, loan ln, coreLogic.core_dbEntities ent, loanPenalty lrp, string userName);
        void ReversePayment(coreLoansEntities le, loan ln, coreLogic.core_dbEntities ent, loanRepayment lrp, string userName);
        void WriteOffInterest(coreLoansEntities le, core_dbEntities ent, loan ln, string userName);
        void ApplyNegativeBalanceToLoan(loan ln, coreLoansEntities le, core_dbEntities ent,
            double amountPaid, DateTime paymentDate, string userName, loan ln2);
    }
}
