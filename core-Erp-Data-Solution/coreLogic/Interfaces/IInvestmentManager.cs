using System;
namespace coreLogic
{
    public interface IInvestmentManager
    {
        savingWithdrawal WithdrawalEWC(ref double pamount, ref double iamount, modeOfPayment mop, int? bankID,
                string withdrawalType, coreLogic.saving dep, double takeHomeAmount, DateTime dateWithdrawn,
                string checkNo, string narration, string userName, SavingWithdrawalCalcModel calc);
        savingWithdrawal WithdrawalOthers(ref double pamount, ref double iamount, modeOfPayment mop, int? bankID,
            string withdrawalType, coreLogic.saving dep, double takeHomeAmount, DateTime dateWithdrawn,
            string checkNo, string narration, string userName);

        void PostDepositAdditional(coreLogic.depositAdditional da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct);
        void PostDepositsWithdrawal(coreLogic.depositWithdrawal da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct);
        void PostDepositsWithdrawalCharges(coreLogic.depositWithdrawal da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct);
        void PostInvestmentAdditional(coreLogic.investmentAdditional da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct);
        void PostClientServiceCharge(coreLogic.clientServiceCharge csc, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct);
        void PostInvestmentWithdrawal(coreLogic.investmentWithdrawal da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct);
        void PostSavingAdditional(coreLogic.savingAdditional da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct);
        void PostSavingsWithdrawal(coreLogic.savingWithdrawal da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct);
        void CloseSavingAdditional(coreLogic.savingAdditional da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct, ref jnl_batch batch);
        void CloseSavingsWithdrawal(coreLogic.savingWithdrawal da, string userName, coreLogic.core_dbEntities ent, coreLogic.coreLoansEntities le, coreLogic.cashiersTill ct, ref jnl_batch batch);
    }
}
