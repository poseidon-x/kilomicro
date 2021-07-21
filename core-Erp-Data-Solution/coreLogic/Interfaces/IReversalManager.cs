using System;
namespace coreLogic
{
    public interface IReversalManager
    {
        void ReverseDisbursement(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.loanTranch lt, coreLogic.core_dbEntities ent, string userName);
        void ReverseFee(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.core_dbEntities ent, coreLogic.loanFee lrp, string userName);
        void ReverseInterest(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.core_dbEntities ent, coreLogic.loanPenalty lrp, string userName);
        void ReversePayment(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.core_dbEntities ent, coreLogic.loanRepayment lrp, string userName);

        void ReverseInsurance(coreLogic.coreLoansEntities le, coreLogic.loan ln, coreLogic.core_dbEntities ent, coreLogic.loanInsurance lins, string userName);
    }
}
