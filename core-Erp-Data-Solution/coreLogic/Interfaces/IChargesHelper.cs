using System;
namespace coreLogic
{
    public interface IChargesHelper
    {
        SavingWithdrawalCalcModel ComputeCharges(int savingId, double takeHomeAmount,DateTime withdrawalDate);
        SavingWithdrawalCalcModel EmptyAccount(int savingId);
    }
}
