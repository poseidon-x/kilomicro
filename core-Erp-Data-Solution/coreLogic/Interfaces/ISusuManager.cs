using System;
namespace coreLogic
{
    public interface ISusuManager
    {
        void PostRegularSusuContribution(coreLogic.coreLoansEntities le, coreLogic.core_dbEntities ent, coreLogic.regularSusuContribution sc, string userName);
        void PostRegularSusuDisbursement(coreLogic.coreLoansEntities le, coreLogic.core_dbEntities ent, coreLogic.regularSusuAccount sc, string userName);
        void PostSusuContribution(coreLogic.coreLoansEntities le, coreLogic.core_dbEntities ent, coreLogic.susuContribution sc, string userName);
        void PostSusuDisbursement(coreLogic.coreLoansEntities le, coreLogic.core_dbEntities ent, coreLogic.susuAccount sc, string userName);
    }
}
