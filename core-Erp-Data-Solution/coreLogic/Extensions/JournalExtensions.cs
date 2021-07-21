using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using coreLogic;
using coreLogic.Models;

namespace coreLogic
{
    public class JournalExtensions : coreLogic.IJournalExtensions
    {
        private Icore_dbEntities ent;
        private IcoreLoansEntities le;

        public JournalExtensions()
        {
           ent = new core_dbEntities(); 
            le=new coreLoansEntities();
        }

        public JournalExtensions(Icore_dbEntities sent, IcoreLoansEntities lent)
        {
            ent = sent;
            le = lent;
        }

        public  jnl_batch Post(string source, int debitAccountID, int creditAccountID, double amount,
            string description, int currencyID, DateTime txDate, string refNo, Icore_dbEntities ent, string userName,
            int? branchID)
        {
            if (txDate.CanClose(ent))
            {
                int? glOuID = null;
                if (branchID != null)
                {
                    var brnch = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    if (brnch != null)
                    {
                        glOuID = brnch.gl_ou_id;
                    }
                }
                var jb = new jnl_batch_stg
                {
                    batch_no = coreExtensions.NextGLBatchNumber(),
                    creation_date = DateTime.Now,
                    creator = userName,
                    is_adj = false,
                    multi_currency = false,
                    posted = false,
                    source = source
                };

                var item = new jnl_stg
                {
                    acct_id = debitAccountID,
                    dbt_amt = amount,
                    description = description,
                    creation_date = DateTime.Now,
                    creator = userName,
                    currency_id = currencyID,
                    frgn_dbt_amt = amount,
                    cost_center_id = glOuID,
                    rate = 1,
                    ref_no = refNo,
                    tx_date = txDate
                };
                jb.jnl_stg.Add(item);
                var item2 = new jnl_stg
                 {
                     acct_id = creditAccountID,
                     crdt_amt = amount,
                     description = description,
                     creation_date = DateTime.Now,
                     creator = userName,
                     currency_id = currencyID,
                     frgn_crdt_amt = amount,
                     cost_center_id = glOuID,
                     rate = 1,
                     ref_no = refNo,
                     tx_date = txDate,
                 };
                jb.jnl_stg.Add(item2);
                var jb2 = jb.ToJournal();
                jb2.jnl.Add(item.ToJournal(ent));
                jb2.jnl.Add(item2.ToJournal(ent));

                return jb2;
            }
            else
            {
                throw new ApplicationException("The selected date belongs to a closed period. Please change.");
            }
        }

        public  jnl_batch Post(string source, int debitAccountID, int creditAccountID, 
            int accountID2, double amount, double amount2, double amount3,
           string description, int currencyID, DateTime txDate, string refNo, Icore_dbEntities ent, string userName)
        {
            if (txDate.CanClose(ent))
            {
                var jb = new jnl_batch_stg
                {
                    batch_no = coreExtensions.NextGLBatchNumber(),
                    creation_date = DateTime.Now,
                    creator = userName,
                    is_adj = false,
                    multi_currency = false,
                    posted = false,
                    source = source
                };

                var item = new jnl_stg
                {
                    acct_id = debitAccountID,
                    dbt_amt = amount,
                    description = description,
                    creation_date = DateTime.Now,
                    creator = userName,
                    currency_id = currencyID,
                    frgn_dbt_amt = amount,
                    cost_center_id = 0,
                    rate = 1,
                    ref_no = refNo,
                    tx_date = txDate,
                };
                jb.jnl_stg.Add(item);
                var item2 = new jnl_stg
                {
                    acct_id = creditAccountID,
                    crdt_amt = amount2,
                    description = description,
                    creation_date = DateTime.Now,
                    creator = userName,
                    currency_id = currencyID,
                    frgn_crdt_amt = amount,
                    cost_center_id = 0,
                    rate = 1,
                    ref_no = refNo,
                    tx_date = txDate,
                };
                jb.jnl_stg.Add(item2);
                var item3 = new jnl_stg
                {
                    acct_id = accountID2,
                    crdt_amt = (amount3 > 0) ? amount3 : 0,
                    dbt_amt = (amount3 > 0) ? 0 : amount3,
                    description = description,
                    creation_date = DateTime.Now,
                    creator = userName,
                    currency_id = currencyID,
                    frgn_crdt_amt = amount,
                    cost_center_id = 0,
                    rate = 1,
                    ref_no = refNo,
                    tx_date = txDate,
                };
                jb.jnl_stg.Add(item3);
                var jb2 = jb.ToJournal();
                jb2.jnl.Add(item.ToJournal(ent));
                jb2.jnl.Add(item2.ToJournal(ent));
                jb2.jnl.Add(item3.ToJournal(ent));

                return jb2;
            }
            else
            {
                throw new ApplicationException("The selected date belongs to a closed period. Please change.");
            }
        }

        public  jnl Post(string source, string txType, int accountID, double amount,
            string description, int currencyID, DateTime txDate, string refNo, Icore_dbEntities ent, string userName,
            int? branchID)
        {
            if (txDate.CanClose(ent))
            {
                int? glOuID = null;
                if (branchID != null)
                {
                    var brnch = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    if (brnch != null)
                    {
                        glOuID = brnch.gl_ou_id;
                    }
                }
                var crd = (txType.ToLower().Trim() == "cr") ? amount : 0;
                var dbt = (txType.ToLower().Trim() == "dr") ? amount : 0;

                var item = new jnl_stg
                {
                    acct_id = accountID,
                    dbt_amt = dbt,
                    crdt_amt = crd,
                    description = description,
                    creation_date = DateTime.Now,
                    creator = userName,
                    currency_id = currencyID,
                    frgn_dbt_amt = dbt,
                    frgn_crdt_amt = crd,
                    cost_center_id = glOuID,
                    rate = 1,
                    ref_no = refNo,
                    tx_date = txDate,
                };

                return item.ToJournal(ent);
            }
            else
            {
                throw new ApplicationException("The selected date belongs to a closed period. Please change.");
            }
        }

        public jnl_batch PostFullBatch(string source, IEnumerable<JournalTransactionLine> lines,
            int currencyID, DateTime txDate, Icore_dbEntities ent, string userName)
        {
            return PostFullBatch(source, lines, currencyID, txDate, ent, userName, null); 
        }

        public jnl_batch PostFullBatch(string source, IEnumerable<JournalTransactionLine> lines,
            int currencyID, DateTime txDate, Icore_dbEntities ent, string userName,
            int? branchID)
        {
            if (txDate.CanClose(ent))
            {
                if (Math.Abs(lines.Sum(p => p.debit) - lines.Sum(p => p.credit)) > 0.01)
                {
                    throw new ApplicationException("The batch does not balance");
                }
                int? glOuID = null;
                if (branchID != null)
                {
                    var brnch = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    if (brnch != null)
                    {
                        glOuID = brnch.gl_ou_id;
                    }
                }
                var jb = new jnl_batch
                {
                    batch_no = coreExtensions.NextGLBatchNumber(),
                    creation_date = DateTime.Now,
                    creator = userName,
                    is_adj = false,
                    multi_currency = false,
                    posted = false,
                    source = source
                };

                foreach (var tx in lines)
                {
                    var item = new jnl
                    {
                        accts = ent.accts.First(p=> p.acct_id==tx.accountId),
                        dbt_amt = tx.debit,
                        crdt_amt = tx.credit,
                        description = tx.description,
                        creation_date = DateTime.Now,
                        creator = userName,
                        currencies = ent.currencies.First(p=> p.currency_id == currencyID),
                        frgn_dbt_amt = tx.debit,
                        frgn_crdt_amt = tx.credit,
                        gl_ou = ent.gl_ou.FirstOrDefault(p=> p.ou_id == glOuID),
                        rate = 1,
                        ref_no = tx.refNo,
                        tx_date = txDate
                    };
                    jb.jnl.Add(item);
                } 

                return jb;
            }
            else
            {
                throw new ApplicationException("The selected date belongs to a closed period. Please change.");
            }
        }

    }
}
