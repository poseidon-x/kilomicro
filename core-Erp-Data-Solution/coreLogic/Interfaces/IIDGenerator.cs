using System;
namespace coreLogic
{
    public interface IIDGenerator
    {
        string NewClientAccountNumber(int branchID, int categoryID);
        string NewDepositNumber(int branchID, int clientID, int depositID = 0, string productPrefix = "");
        string NewInvestmentNumber(int branchID, int clientID, int investmentID = 0, string productPrefix = "");
        string NewGroupSusuNumber(int branchID, int clientID, int susuAccountID = 0, string productPrefix = "");
        string NewLoanNumber(int branchID, int clientID, int loanID = 0, string productPrefix = "");
        string NewNormalSusuNumber(int branchID, int clientID, int susuAccountID = 0, string productPrefix = "");
        string NewSavingsNumber(int branchID, int clientID, int savingID = 0, string productPrefix = "");
        string NewStaffNumber(int branchID, DateTime startDate);

        string NewInventoryItemNumber(long inventoryItemId, ICommerceEntities ce,
            Icore_dbEntities ent, string productPrefix);
    }
}
