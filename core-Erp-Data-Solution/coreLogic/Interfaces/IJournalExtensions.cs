using System;
using System.Collections.Generic;
using coreLogic.Models;

namespace coreLogic
{
    public interface IJournalExtensions
    {
        jnl_batch Post(string source, int debitAccountID, int creditAccountID, double amount, string description, int currencyID, DateTime txDate, string refNo, Icore_dbEntities ent, string userName, int? branchID);
        jnl_batch Post(string source, int debitAccountID, int creditAccountID, int accountID2, double amount, double amount2, double amount3, string description, int currencyID, DateTime txDate, string refNo, Icore_dbEntities ent, string userName);
        jnl Post(string source, string txType, int accountID, double amount, string description, int currencyID, DateTime txDate, string refNo, Icore_dbEntities ent, string userName, int? branchID);

        jnl_batch PostFullBatch(string source, IEnumerable<JournalTransactionLine> lines,
            int currencyID, DateTime txDate, Icore_dbEntities ent, string userName,
            int? branchID);

        jnl_batch PostFullBatch(string source, IEnumerable<JournalTransactionLine> lines,
            int currencyID, DateTime txDate, Icore_dbEntities ent, string userName);

    }
}
