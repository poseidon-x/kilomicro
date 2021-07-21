using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            
            core_dbEntities ent = new core_dbEntities();
            coreLoansEntities le = new coreLoansEntities();
        //    MigrationDataset ds = new MigrationDataset();
        //    MigrationDatasetTableAdapters.unearnedinterestTableAdapter da = new MigrationDatasetTableAdapters.unearnedinterestTableAdapter();
        //    da.FillUE(ds.unearnedinterest);
        //    var pro = ent.comp_prof.FirstOrDefault();
        //    for (int i = 0; i < ds.unearnedinterest.Count-2; i += 2)
        //    {
        //        var sr = ds.unearnedinterest[i];
        //        var sr1 = ds.unearnedinterest[i+1];

        //        var jb2 = new jnl_batch
        //        {
        //            batch_no = coreExtensions.NextGLBatchNumber(),
        //            creation_date = DateTime.Now,
        //            creator = "SYSTEM",
        //            is_adj = false,
        //            multi_currency = false,
        //            posted = false,
        //            source = "PR"
        //        };
        //        if (sr.IsAccountNull() == false && sr.IsAmountNull() == false && sr1.IsAccountNull() == false && sr1.IsAmountNull() == false)
        //        {
        //            var accNum = sr.Account;  
        //            var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
        //            var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == sr1.Account);
        //            if (sr.Journal_Type.ToUpper().Trim()=="DR")
        //            {
        //                var j = JournalExtensions.Post("PR", "dr",
        //                    acc.acct_id, double.Parse(sr.Amount),sr.Description, pro.currency_id.Value,
        //                    new DateTime(2013,10,31), sr.Loan_ID, ent, "SYSTEM");
        //                jb2.jnl.Add(j);
        //                j = JournalExtensions.Post("PR", "cr",
        //                    acc2.acct_id, double.Parse(sr1.Amount), sr1.Description, pro.currency_id.Value,
        //                    new DateTime(2013, 10, 31), sr1.Loan_ID, ent, "SYSTEM");
        //                jb2.jnl.Add(j); 
        //            }
        //            else
        //            {
        //                var j = JournalExtensions.Post("PR", "cr",
        //                    acc.acct_id, double.Parse(sr.Amount), sr.Description, pro.currency_id.Value,
        //                    new DateTime(2013, 10, 31), sr.Loan_ID, ent, "SYSTEM");
        //                jb2.jnl.Add(j);
        //                j = JournalExtensions.Post("PR", "dr",
        //                    acc2.acct_id, double.Parse(sr1.Amount), sr1.Description, pro.currency_id.Value,
        //                    new DateTime(2013, 10, 31), sr1.Loan_ID, ent, "SYSTEM");
        //                jb2.jnl.Add(j);
        //            }
        //            ent.jnl_batch.Add(jb2);
        //        }
        //    }
        //    ent.SaveChanges();
        //
        }
        
      /*  static void Main(string[] args)
        {
            coreLoansEntities le = new coreLoansEntities();
            core_dbEntities ent = new core_dbEntities();
            MigrationDataset ds = new MigrationDataset();
            ds.EnforceConstraints = true;
            MigrationDatasetTableAdapters.ClientsTableAdapter ca = new MigrationDatasetTableAdapters.ClientsTableAdapter();
            MigrationDatasetTableAdapters.LoansTableAdapter la = new MigrationDatasetTableAdapters.LoansTableAdapter();
            MigrationDatasetTableAdapters.DisbursementsTableAdapter da = new MigrationDatasetTableAdapters.DisbursementsTableAdapter();
            MigrationDatasetTableAdapters.ReceiptsTableAdapter ra = new MigrationDatasetTableAdapters.ReceiptsTableAdapter();
            MigrationDatasetTableAdapters.CompanyTableAdapter compa = new MigrationDatasetTableAdapters.CompanyTableAdapter();
            MigrationDatasetTableAdapters.EmployerTableAdapter ea = new MigrationDatasetTableAdapters.EmployerTableAdapter();
            MigrationDatasetTableAdapters.Prudential_SMETableAdapter prA = new MigrationDatasetTableAdapters.Prudential_SMETableAdapter();
            MigrationDatasetTableAdapters.SalaryTableAdapter sa = new MigrationDatasetTableAdapters.SalaryTableAdapter();
            MigrationDatasetTableAdapters.Ops_ac_costsTableAdapter oa = new MigrationDatasetTableAdapters.Ops_ac_costsTableAdapter();
            MigrationDatasetTableAdapters.Petty_Cash_ExpensesTableAdapter pca = new MigrationDatasetTableAdapters.Petty_Cash_ExpensesTableAdapter();
            MigrationDatasetTableAdapters.Directors_to_Petty_CashTableAdapter dpa = new MigrationDatasetTableAdapters.Directors_to_Petty_CashTableAdapter();
            MigrationDatasetTableAdapters.Bank_to_Petty_cashTableAdapter bpa = new MigrationDatasetTableAdapters.Bank_to_Petty_cashTableAdapter();
            MigrationDatasetTableAdapters.Vault_to_bankTableAdapter vba = new MigrationDatasetTableAdapters.Vault_to_bankTableAdapter();
            MigrationDatasetTableAdapters.Vault_to_Petty_CashTableAdapter vpa = new MigrationDatasetTableAdapters.Vault_to_Petty_CashTableAdapter();
            MigrationDatasetTableAdapters.Fidelity_BankTableAdapter vda = new MigrationDatasetTableAdapters.Fidelity_BankTableAdapter();
            MigrationDatasetTableAdapters.SA_FEESTableAdapter safa = new MigrationDatasetTableAdapters.SA_FEESTableAdapter();
            MigrationDatasetTableAdapters.SA_INTEERESTTableAdapter saia = new MigrationDatasetTableAdapters.SA_INTEERESTTableAdapter();
            MigrationDatasetTableAdapters.SA_LOANSTableAdapter sala = new MigrationDatasetTableAdapters.SA_LOANSTableAdapter();
            MigrationDatasetTableAdapters.SA_REPAYMENTTableAdapter sara = new MigrationDatasetTableAdapters.SA_REPAYMENTTableAdapter();
            MigrationDatasetTableAdapters.DepositorsTableAdapter dsa = new MigrationDatasetTableAdapters.DepositorsTableAdapter();
            MigrationDatasetTableAdapters.Other_one_off_transactionTableAdapter ooa = new MigrationDatasetTableAdapters.Other_one_off_transactionTableAdapter();
            MigrationDatasetTableAdapters.Prudential_SavingsTableAdapter psa = new MigrationDatasetTableAdapters.Prudential_SavingsTableAdapter();
            MigrationDatasetTableAdapters.Rtn_of_fundsTableAdapter rofa = new MigrationDatasetTableAdapters.Rtn_of_fundsTableAdapter();

            MigrationDatasetTableAdapters._2012_AdditionsTableAdapter addDa = new MigrationDatasetTableAdapters._2012_AdditionsTableAdapter();
            MigrationDatasetTableAdapters._2012_ReclassTableAdapter recDa = new MigrationDatasetTableAdapters._2012_ReclassTableAdapter();
            MigrationDatasetTableAdapters._2013_uploadTableAdapter upDa = new MigrationDatasetTableAdapters._2013_uploadTableAdapter();

            addDa.Fill(ds._2012_Additions);
            recDa.Fill(ds._2012_Reclass);
            upDa.Fill(ds._2013_upload);
            /*
            ca.Fill(ds.Clients);
            la.Fill(ds.Loans);
            da.Fill(ds.Disbursements);
            ra.Fill(ds.Receipts);
            compa.Fill(ds.Company);
            ea.Fill(ds.Employer);
            prA.Fill(ds.Prudential_SME);
            sa.Fill(ds.Salary);
            oa.Fill(ds.Ops_ac_costs);
            pca.Fill(ds.Petty_Cash_Expenses);
            dpa.Fill(ds.Directors_to_Petty_Cash);
            bpa.Fill(ds.Bank_to_Petty_cash);
            vba.Fill(ds.Vault_to_bank);
            vpa.Fill(ds.Vault_to_Petty_Cash);
            sala.Fill(ds.SA_LOANS);
            sara.Fill(ds.SA_REPAYMENT);
            saia.Fill(ds.SA_INTEEREST);
            safa.Fill(ds.SA_FEES);
            dsa.Fill(ds.Depositors);
            ooa.Fill(ds._Other_one_off_transaction);
            vda.Fill(ds.Fidelity_Bank);
            psa.Fill(ds.Prudential_Savings);
            rofa.Fill(ds.Rtn_of_funds);
            

            var pro = ent.comp_prof.FirstOrDefault();
            var jb2 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PR"
            };
            var total = 0.0;
            foreach (MigrationDataset._2012_AdditionsRow sr in ds._2012_Additions)
            {
                if (sr.IsF4Null() == false && sr.IsF4Null() == false)
                {
                    var accNum = "";
                    if (sr.F4 < 6229 && sr.F4!=6225)
                    {
                        accNum = sr.F4.ToString();
                    }
                    if (sr.F4 > 6229 && sr.F4 < 6232)
                    {
                        accNum = sr.F4.ToString();
                    }
                    else if (sr.F4 > 6231)
                    {
                        accNum = "23" + sr.F4.ToString().Substring(2);
                    }
                    else if (sr.F4 == 6225)
                    {
                        accNum = "23" + sr.F4.ToString().Substring(2);
                    }
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "2900");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PR", "dr",
                                acc.acct_id, sr.F3, sr.F2, pro.currency_id.Value,
                                sr.Post_CR_journal_to_Directors_Account, "", ent, "SYSTEM");
                            jb2.jnl.Add(j);
                            j = JournalExtensions.Post("PR", "cr",
                                acc2.acct_id, sr.F3, sr.F2, pro.currency_id.Value,
                                sr.Post_CR_journal_to_Directors_Account, "", ent, "SYSTEM");
                            jb2.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb2.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb2);
            }


            var jb = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PR"
            };
            total = 0.0;
            foreach (MigrationDataset._2012_ReclassRow sr in ds._2012_Reclass)
            {
                if (sr.IsF4Null() == false && sr.IsF4Null() == false)
                {
                    var accNum = "";
                    if (sr.F4 < 6229 && sr.F4 != 6225)
                    {
                        accNum = sr.F4.ToString();
                    }
                    if (sr.F4 > 6229 && sr.F4 < 6232)
                    {
                        accNum = sr.F4.ToString();
                    }
                    else if (sr.F4 > 6231)
                    {
                        accNum = "23" + sr.F4.ToString().Substring(2);
                    }
                    else if (sr.F4 == 6225)
                    {
                        accNum = "23" + sr.F4.ToString().Substring(2);
                    }
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1002");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PR", "dr",
                                acc.acct_id, sr.F3, sr.F2, pro.currency_id.Value,
                                sr.F1, "", ent, "SYSTEM");
                            jb.jnl.Add(j);
                            j = JournalExtensions.Post("PR", "cr",
                                acc2.acct_id, sr.F3, sr.F2, pro.currency_id.Value,
                                sr.F1, "", ent, "SYSTEM");
                            jb.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb);
            }

            jb = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PR"
            };
            total = 0.0;
            foreach (MigrationDataset._2013_uploadRow sr in ds._2013_upload)
            {
                if (sr.IsF4Null() == false && sr.IsF4Null() == false)
                {
                    var accNum = "";
                    if (sr.F4 < 6229 && sr.F4 != 6225)
                    {
                        accNum = sr.F4.ToString();
                    }
                    if (sr.F4 > 6229 && sr.F4 < 6232)
                    {
                        accNum = sr.F4.ToString();
                    }
                    else if (sr.F4 > 6231)
                    {
                        accNum = "23" + sr.F4.ToString().Substring(2);
                    }
                    else if (sr.F4 == 6225)
                    {
                        accNum = "23" + sr.F4.ToString().Substring(2);
                    }
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1002");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PR", "dr",
                                acc.acct_id, sr.F3, sr.F2, pro.currency_id.Value,
                                sr.Reclassification, "", ent, "SYSTEM");
                            jb.jnl.Add(j);
                            j = JournalExtensions.Post("PR", "cr",
                                acc2.acct_id, sr.F3, sr.F2, pro.currency_id.Value,
                                sr.Reclassification, "", ent, "SYSTEM");
                            jb.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb);
            }

            /*
            var jb3 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PR"
            };
            foreach (MigrationDataset.Prudential_SMERow sr in ds.Prudential_SME)
            {
                if (sr.IsAmountNull() == false && sr.IsAccount_codeNull() == false)
                {
                    var accNum = "";
                    if (int.Parse(sr.Account_code) < 25)
                    {
                        accNum = "62" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) == 30)
                    {
                        accNum = "40" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) > 31)
                    {
                        accNum = "23" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) == 25)
                    {
                        accNum = "23" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1046");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PR", sr.Transaction_type,
                                acc.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb3.jnl.Add(j);
                            j = JournalExtensions.Post("PR", (sr.Transaction_type.Trim().ToLower() == "dr") ? "cr" : "dr",
                                acc2.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb3.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb3.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb3);
            }

            var jb13 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PR"
            };
            foreach (MigrationDataset.Prudential_SavingsRow sr in ds.Prudential_Savings)
            {
                if (sr.IsAmountNull() == false && sr.IsAccount_codeNull() == false)
                {
                    var accNum = "";
                    if (int.Parse(sr.Account_code) < 25)
                    {
                        accNum = "62" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) == 30)
                    {
                        accNum = "40" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) > 31)
                    {
                        accNum = "23" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) == 25)
                    {
                        accNum = "23" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1048");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PR", sr.Transaction_type,
                                acc.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb13.jnl.Add(j);
                            j = JournalExtensions.Post("PR", (sr.Transaction_type.Trim().ToLower() == "dr") ? "cr" : "dr",
                                acc2.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb13.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb13.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb13);
            }

            var jb23 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PR"
            };
            foreach (MigrationDataset.Fidelity_BankRow sr in ds.Fidelity_Bank)
            {
                if (sr.IsAmountNull() == false && sr.IsAccount_codeNull() == false)
                {
                    var accNum = "";
                    if (int.Parse(sr.Account_code) < 25)
                    {
                        accNum = "62" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) == 30)
                    {
                        accNum = "40" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) > 31)
                    {
                        accNum = "23" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    else if (int.Parse(sr.Account_code) == 25)
                    {
                        accNum = "23" + sr.Account_code.Trim().PadLeft(2, '0');
                    }
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1049");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PR", sr.Transaction_type,
                                acc.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                DateTime.ParseExact(sr.Date, "d-MMM-yy",System.Globalization.CultureInfo.CurrentCulture), "", ent, "SYSTEM");
                            jb23.jnl.Add(j);
                            j = JournalExtensions.Post("PR", (sr.Transaction_type.Trim().ToLower() == "dr") ? "cr" : "dr",
                                acc2.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                DateTime.ParseExact(sr.Date, "d-MMM-yy", System.Globalization.CultureInfo.CurrentCulture), "", ent, "SYSTEM");
                            jb23.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb23.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb23);
            }

            var jb4 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PC"
            };
            foreach (MigrationDataset.Petty_Cash_ExpensesRow sr in ds.Petty_Cash_Expenses)
            {
                if (sr.IsExpense_CodeNull() == false)
                {
                    var accNum = "";
                    if (sr.Expense_Code < 25)
                    {
                        accNum = "62" + sr.Expense_Code.ToString().PadLeft(2, '0');
                    }
                    else if (sr.Expense_Code == 30)
                    {
                        accNum = "40" + sr.Expense_Code.ToString().PadLeft(2, '0');
                    }
                    else if (sr.Expense_Code > 31)
                    {
                        accNum = "23" + sr.Expense_Code.ToString().PadLeft(2, '0');
                    }
                    else if (sr.Expense_Code == 25)
                    {
                        accNum = "23" + sr.Expense_Code.ToString().PadLeft(2, '0');
                    }
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1002");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PC", "DR",
                                acc.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb4.jnl.Add(j);
                            j = JournalExtensions.Post("PC", "CR",
                                acc2.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb4.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb4.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb4);
            }

            var jb5 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PC"
            };
            foreach (MigrationDataset.Directors_to_Petty_CashRow sr in ds.Directors_to_Petty_Cash)
            {
                if (sr.IsTransaction_typeNull() == false)
                {
                    var accNum = "2900";
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1002");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PC", "CR",
                                acc.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb5.jnl.Add(j);
                            j = JournalExtensions.Post("PC", "DR",
                                acc2.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb5.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb5.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb5);
            }

            var jb6 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PC"
            };
            foreach (MigrationDataset.Bank_to_Petty_cashRow sr in ds.Bank_to_Petty_cash)
            {
                if (sr.IsTransaction_typeNull() == false)
                {
                    var accNum = "2900";
                    if (sr.Account == "Prudential Ops") accNum = "1047";
                    if (sr.Account == "Prudential SME") accNum = "1046";
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1002");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PC", "CR",
                                acc.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb6.jnl.Add(j);
                            j = JournalExtensions.Post("PC", "DR",
                                acc2.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb6.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb6.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb6);
            }

            var jb61 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "PC"
            };
            foreach (MigrationDataset.Rtn_of_fundsRow sr in ds.Rtn_of_funds)
            {
                if (sr.IsPurposeNull() == false)
                {
                    var accNum = "1046";
                    var accNum2 = "1000";
                    if (sr.IsPayment_modeNull() == false && sr.Payment_mode == "Investor cash") accNum2 = "3201";
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == accNum2);
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("PC", "DR",
                                acc.acct_id, sr.Amount, sr.Purpose  + " - " + sr.Account_Name, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb61.jnl.Add(j);
                            j = JournalExtensions.Post("PC", "CR",
                                acc2.acct_id, sr.Amount, sr.Purpose + " - " + sr.Account_Name, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb61.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb61.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb61);
            }

            var jb7 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "VM"
            };
            foreach (MigrationDataset.Vault_to_bankRow sr in ds.Vault_to_bank)
            {
                if (sr.IsDescriptionNull() == false)
                {
                    var accNum = "2900";
                    if (sr.Bank == "Prudential Ops") accNum = "1047";
                    if (sr.Bank == "Prudential SME") accNum = "1046";
                    if (sr.Bank == "Prudential Savings") accNum = "1048";
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1000");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("VM", "DR",
                                acc.acct_id, sr.Amount_banked, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb7.jnl.Add(j);
                            j = JournalExtensions.Post("PC", "CR",
                                acc2.acct_id, sr.Amount_banked, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb7.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb7.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb7);
            }

            var jb8 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "VM"
            };
            foreach (MigrationDataset.Vault_to_Petty_CashRow sr in ds.Vault_to_Petty_Cash)
            {
                if (sr.IsDescriptionNull() == false)
                {
                    var accNum = "1002";
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == "1000");
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("VM", "DR",
                                acc.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb8.jnl.Add(j);
                            j = JournalExtensions.Post("PC", "CR",
                                acc2.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb8.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb8.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb8);
            }

            var jb9 = new jnl_batch
            {
                batch_no = coreExtensions.NextGLBatchNumber(),
                creation_date = DateTime.Now,
                creator = "SYSTEM",
                is_adj = false,
                multi_currency = false,
                posted = false,
                source = "VM"
            };
            foreach (MigrationDataset._Other_one_off_transactionRow sr in ds._Other_one_off_transaction)
            {
                if (sr.IsDescriptionNull() == false)
                {
                    var accNum = "";
                    var accNum2 = "";
                    if (sr.Account_CR == "Prudential SME")
                    {
                        accNum = "1046";
                    }
                    else if (sr.Account_CR.Trim() == "Investor")
                    {
                        accNum = "3201";
                    }
                    else if (sr.Account_CR.Trim() == "Prudential Ops")
                    {
                        accNum = "1047";
                    }
                    else if (sr.Account_CR.Trim() == "Prudential Savings")
                    {
                        accNum = "1048";
                    }
                    else if (sr.Account_CR.Trim() == "Investor ")
                    {
                        accNum = "3201";
                    }
                    else if (sr.Account_CR.Trim() == "Fidelity Bank")
                    {
                        accNum = "1049";
                    }
                    else if (sr.Account_CR.Trim() == "Disbursement" )
                    {
                        accNum = "1001";
                    }
                    else if (sr.Account_CR.Trim() == "Vault")
                    {
                        accNum = "1000";
                    }
                    else if (sr.Account_CR.Trim() == "Shareholders fund")
                    {
                        accNum = "3200";
                    }                        
                    else if (sr.Account_CR.Trim() == "Director")
                    {
                        accNum = "2900";
                    }
                    else if (sr.Account_CR.Trim() == "Interest Income SME")
                    {
                        accNum = "4000";
                    }
                    else if (sr.Account_CR.Trim() == "Petty cash")
                    {
                        accNum = "1002";
                    }
                    if (sr.Account_DR.Trim() == "Prudential SME")
                    {
                        accNum2 = "1046";
                    }
                    else if (sr.Account_DR.Trim() == "Petty cash")
                    {
                        accNum2 = "1002";
                    }
                    else if (sr.Account_DR.Trim() == "Fidelity Bank")
                    {
                        accNum2 = "1049";
                    }
                    else if (sr.Account_DR.Trim() == "Prudential Ops")
                    {
                        accNum2 = "1047";
                    }
                    else if (sr.Account_DR.Trim() == "Interest Income SME")
                    {
                        accNum2 = "4000";
                    }
                    else if (sr.Account_DR.Trim() == "Shareholders fund")
                    {
                        accNum2 = "3200";
                    } 
                    else if (sr.Account_DR.Trim() == "Director")
                    {
                        accNum2 = "2900";
                    }
                    else if (sr.Account_DR.Trim() == "Prudential Savings")
                    {
                        accNum2 = "1048";
                    }
                    else if (sr.Account_DR.Trim() == "Vault")
                    {
                        accNum2 = "1000";
                    }
                    else if (sr.Account_DR.Trim() == "Disbursement" )
                    {
                        accNum2 = "1001";
                    }
                    else if (sr.Account_DR.Trim() == "Petty cash")
                    {
                        accNum2 = "1002";
                    }
                    else if (sr.Account_DR.Trim() == "Prudential Ops")
                    {
                        accNum2 = "1047";
                    }
                    else if (sr.Account_DR.Trim() == "Employee Allowance (expense code - 5)")
                    {
                        accNum2 = "6205";
                    }
                    else if (sr.Account_DR.Trim() == "Net salary payment")
                    {
                        accNum2 = "2334";
                    }
                    else if (sr.Account_DR.Trim() == "Investor ")
                    {
                        accNum2 = "3201";
                    }
                    if (accNum != "")
                    {
                        var acc = ent.accts.FirstOrDefault(p => p.acc_num == accNum);
                        var acc2 = ent.accts.FirstOrDefault(p => p.acc_num == accNum2);
                        if (acc != null)
                        {
                            var j = JournalExtensions.Post("VM", "CR",
                                acc.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb9.jnl.Add(j);
                            j = JournalExtensions.Post("PC", "DR",
                                acc2.acct_id, sr.Amount, sr.Description, pro.currency_id.Value,
                                sr.Date, "", ent, "SYSTEM");
                            jb9.jnl.Add(j);
                        }
                    }
                }
            }
            if (jb9.jnl.Count > 0)
            {
                ent.jnl_batch.Add(jb9);
            }

            ent.SaveChanges();
            foreach (MigrationDataset.EmployerRow er in ds.Employer)
            {
                var e = le.employers.FirstOrDefault(p => p.employerName == er.EMPLOYER);
                if (e == null)
                {
                    e = new employer
                    {
                        employerName = er.EMPLOYER,
                        address = new address { addressLine1 = er.IsMY_ADDRESSNull() ? "" : er.MY_ADDRESS }
                    };
                    e.employerDirectors.Add(new employerDirector
                    {
                        email = new email { emailAddress = er.IsEMAIL_ADDRESSNull() ? "" : er.EMAIL_ADDRESS, emailTypeID = 1 },
                        phone = new phone { phoneNo = er.IsMOBILE_NUMBERNull() ? "" : er.MOBILE_NUMBER, phoneTypeID = 2 },
                        idNo = new idNo
                        {
                            idNo1 = er.IsID_NUMBERNull() ? "" : er.ID_NUMBER,
                            idNoTypeID = er.IsID_TYPENull() ? 1 : (er.ID_TYPE == "Passport" ?
                                   1 : (er.ID_TYPE == "Voters ID" ? 2 : (er.ID_TYPE == "Drivers license" ?
                                   4 : (er.ID_TYPE == "NHIS" ? 3 : er.ID_TYPE == "Staff ID" ? 5 : 1))))
                        }
                    });
                    le.employers.Add(e);
                }
            }

            Dictionary<string, PostData> listPost = new Dictionary<string, PostData>();
            List<group> groups = new List<group>();
            foreach (MigrationDataset.ClientsRow cr in ds.Clients)
            {
                if (cr.IsRefNull() == false)
                {
                    client c = new client();
                    c.accountNumber = (cr.IsBranchNull() ? "KM" : cr.Branch) + cr.Ref.ToString().PadLeft(5, '0');
                    c.surName = cr.Surname;
                    c.otherNames = cr.Other_names;
                    if (cr.IsDOBNull() == false) c.DOB = cr.DOB;
                    if (cr.IsID_TypeNull() == false)
                    {
                        idNo id = new idNo();
                        switch (cr.ID_Type)
                        {
                            case "Passport":
                                id.idNoTypeID = 1;
                                break;
                            case "Voters ID":
                                id.idNoTypeID = 2;
                                break;
                            case "Drivers license":
                                id.idNoTypeID = 4;
                                break;
                            case "NHIS":
                                id.idNoTypeID = 3;
                                break;
                            case "Staff ID":
                                id.idNoTypeID = 5;
                                break;
                        }
                        id.idNo1 = cr.ID_Number;
                        c.idNo = id;
                    }
                    if (cr.IsWorkPhoneNull() == false)
                    {
                        clientPhone cp = new clientPhone();
                        cp.phoneTypeID = 1;
                        cp.phone = new phone
                        {
                            phoneNo = cr.WorkPhone,
                            phoneTypeID = 1
                        };
                        c.clientPhones.Add(cp);
                    }
                    if (cr.IsMobilePhoneNull() == false)
                    {
                        clientPhone cp = new clientPhone();
                        cp.phoneTypeID = 2;
                        cp.phone = new phone
                        {
                            phoneNo = cr.MobilePhone,
                            phoneTypeID = 2
                        };
                        c.clientPhones.Add(cp);
                    }
                    if (cr.IsHomePhoneNull() == false)
                    {
                        clientPhone cp = new clientPhone();
                        cp.phoneTypeID = 3;
                        cp.phone = new phone
                        {
                            phoneNo = cr.HomePhone,
                            phoneTypeID = 3
                        };
                        c.clientPhones.Add(cp);
                    }
                    if (cr.IsPostal_AddressNull() == false)
                    {
                        clientAddress a = new clientAddress();
                        a.addressTypeID = 2;
                        a.address = new address
                        {
                            addressLine1 = cr.Postal_Address,
                        };
                        c.clientAddresses.Add(a);
                    }
                    if (cr.IsDirectionsNull() == false)
                    {
                        clientAddress a = new clientAddress();
                        a.addressTypeID = 1;
                        a.address = new address
                        {
                            addressLine1 = cr.Directions,
                        };
                        c.clientAddresses.Add(a);
                    }
                    if (cr.IsClient_CategoryNull() == false)
                    {
                        switch (cr.Client_Category)
                        {
                            case "SME":
                                c.categoryID = 1;
                                var crs = ds.Company.Select("[COMPANY ID]='" + (cr.IsInstitutionNull() ? "" : cr.Institution) + "'");
                                if (crs.Count() == 1)
                                {
                                    MigrationDataset.CompanyRow er = crs[0] as MigrationDataset.CompanyRow;
                                    if (er != null)
                                    {
                                        c.smeCategories.Add(new smeCategory
                                        {
                                            address = new address { addressLine1 = er.IsAddressNull() ? "" : er.Address },
                                            address1 = new address { addressLine1 = er.IsAddressNull() ? "" : er.Address },
                                            companyName = er.COMPANY_NAME,
                                            incDate = er.IsDATE_OF_INCORPORATIONNull() ? DateTime.Now : er.DATE_OF_INCORPORATION,
                                            regNo = er.IsREGISTRATION_NONull() ? "" : er.REGISTRATION_NO,
                                            regDate = er.IsCOMMENCEMENT_DATENull() ? DateTime.Now : er.COMMENCEMENT_DATE
                                        });
                                    }
                                }
                                break;
                            case "Micro":
                                c.categoryID = 4;
                                break;
                            case "Employee":
                                c.categoryID = 2;
                                var ers = ds.Employer.Select("[EMPLOYER ID]='" + (cr.IsInstitutionNull() ? "" : cr.Institution) + "'");
                                if (ers.Count() > 1)
                                {
                                    MigrationDataset.EmployerRow er = ers[0] as MigrationDataset.EmployerRow;
                                    var e = le.employers.FirstOrDefault(p => p.employerName == er.EMPLOYER);
                                    if (e != null)
                                    {
                                        c.employeeCategories.Add(new employeeCategory
                                        {
                                            employerID = e.employerID,
                                            employer = e
                                        });
                                    }
                                }
                                break;
                            case "Group":
                                c.categoryID = 3;
                                if (cr.IsInstitutionNull() == false)
                                {
                                    var inst = cr.Institution.Trim();
                                    var grp = groups.FirstOrDefault(p => p.groupName == inst);
                                    if (grp == null)
                                    {
                                        grp = new group { groupName=inst, groupTypeID=2};
                                        le.groups.Add(grp);
                                        groups.Add(grp);
                                    }
                                    c.groupCategories.Add(new groupCategory
                                    {
                                        group = grp
                                    }
                                    );
                                }
                                break;
                        }
                    }
                    c.sectorID = 9;
                    c.maritalStatusID = 0;
                    c.creation_date = DateTime.Now;
                    c.creator = "SYSTEM";

                    var drs = ds.Depositors.Select("[Client ID]=" + cr.Ref.ToString());
                    deposit dp = null;
                    if (drs.Length > 0)
                    {
                        dp = new deposit();
                        le.deposits.Add(dp);
                        dp.principalBalance = 0;
                        dp.amountInvested = 0;
                        dp.firstDepositDate = ((MigrationDataset.DepositorsRow)drs[0]).Date;
                    }
                    foreach (MigrationDataset.DepositorsRow drr in drs)
                    {
                        if (drr.IsAmountNull() == false)
                        {
                            dp.principalBalance += drr.Amount;
                            dp.maturityDate = drr.Date.AddDays(90);
                            int mopID = 1;
                            var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                            int? bankID = null;
                            var daa = new coreLogic.depositAdditional
                            {
                                checkNo = "",
                                depositAmount = drr.Amount,
                                bankID = bankID,
                                interestBalance = 0,
                                depositDate = drr.Date,
                                creation_date = DateTime.Now,
                                creator = "SYSTEM",
                                principalBalance = drr.Amount,
                                modeOfPayment = mop
                            };
                            dp.depositAdditionals.Add(daa);
                            dp.amountInvested += drr.Amount;
                            dp.client = c;
                            dp.creation_date = DateTime.Now;
                            dp.creator = "SYSTEM";
                            dp.period = 90;
                            dp.interestRate = 0;
                            int depTypeID = 4;
                            dp.depositType = le.depositTypes.FirstOrDefault(p => p.depositTypeID == depTypeID);

                            if (dp.depositNo == null || dp.depositNo.Trim().Length == 0)
                            {
                                dp.depositNo = "CA" + "" +
                                coreLogic.coreExtensions.NextSystemNumber(
                                "DEPOSIT_CA");
                            }


                            jb = coreLogic.JournalExtensions.Post("LN", dp.depositType.vaultAccountID.Value,
                                dp.depositType.accountsPayableAccountID.Value, drr.Amount,
                                "Deposit - " + dp.client.surName + "," + dp.client.otherNames,
                                pro.currency_id.Value, drr.Date, dp.depositNo, ent, "SYSTEM");

                            ent.jnl_batch.Add(jb);
                        }
                    }
                    if (cr.Ref != 161)
                    {
                        var lrs = ds.Loans.Select("[Client ID]=" + cr.Ref.ToString());
                        foreach (MigrationDataset.LoansRow lr in lrs)
                        {
                            var comments = "";
                            if (lr.IsCommentsNull() == false) comments = lr.Comments;
                            var balance = lr.Amount_borrowed;

                            var ps = "";
                            if (lr.IsPayment_statusNull() == false) ps = lr.Payment_status;

                            loan l = new loan();
                            l.loanStatusID = 4;
                            l.amountApproved = lr.Amount_borrowed;
                            l.amountRequested = lr.Amount_borrowed;
                            l.amountDisbursed = 0;
                            l.applicationDate = lr.Loan_start_date;
                            l.applicationFee = 0;
                            l.processingFee = 0;
                            l.balance = 0;
                            l.commission = 0;
                            l.creation_date = DateTime.Now;
                            l.creator = "SYSTEM";
                            var sub = "";
                            if (lr.IsCategoryNull() == false)
                            {
                                switch (lr.Category)
                                {
                                    case "SME":
                                        l.loanTypeID = 1;
                                        sub = "S";
                                        break;
                                    case "ME":
                                        l.loanTypeID = 2;
                                        sub = "M";
                                        break;
                                    case "EMP":
                                        l.loanTypeID = 3;
                                        sub = "E";
                                        break;
                                    case "GROUP":
                                        l.loanTypeID = 4;
                                        sub = "G";
                                        break;
                                }
                            }
                            l.loanType = le.loanTypes.FirstOrDefault(p => p.loanTypeID == l.loanTypeID);
                            l.loanNo = sub + coreExtensions.NextSystemNumber("LOAN_" + sub);
                            if (lr.IsPayment_statusNull() == false)
                            {
                                switch (lr.Payment_status)
                                {
                                    case "Completed":
                                        l.repaymentModeID = 30;
                                        break;
                                    case "Monthly":
                                        l.repaymentModeID = 30;
                                        break;
                                    case "Daily":
                                        l.repaymentModeID = 5;
                                        break;
                                    case "Weekly":
                                        l.repaymentModeID = 7;
                                        break;
                                    case "Fortnightly":
                                        l.repaymentModeID = 14;
                                        break;
                                }
                            }
                            l.gracePeriod = 0;
                            l.interestRate = lr.Interest_rate * 100;
                            l.interestTypeID = 1;
                            l.creditOfficerNotes = "";
                            l.approvalComments = "";
                            l.tenureTypeID = 1;
                            l.loanTenure = lr._Period_of_loan__month_;
                            if (l.loanTenure == 0) l.loanTenure = 1;
                            if (cr.Ref == 16)
                            {
                                l.loanTenure = 0;
                                l.repaymentModeID = -1;
                                l.interestTypeID = 2;
                            }

                            c.loans.Add(l);
                            LoansHelper.PostLoan(le, l, lr.Amount_borrowed, l.amountApproved, lr.Loan_start_date,
                                "", "1", "", ent, false, "SYSTEM", "1", "");

                            var endDate = lr.Loan_start_date.AddDays(30);
                            var endDate2 = lr.Loan_start_date.AddDays(15);
                            var drrs = ds.Disbursements.Select("([Client ID] = '" + cr.Ref.ToString() +
                                    "' AND [Account Name] = '" + lr.Loan_ID
                                    + "' AND Amount <= " + lr.Amount_borrowed.ToString() +
                                    " AND ([Date] >= #" + lr.Loan_start_date.AddDays(0).ToString("yyyy/MM/dd") + "#" +
                                    " AND [Date] <= #" + endDate.ToString("yyyy/MM/dd") + "#)" + ")");
                            if (drrs.Length == 0)
                                drrs = ds.Disbursements.Select("([Client ID] = '" + cr.Ref.ToString() +
                                    "' AND ([Date] >= #" + lr.Loan_start_date.AddDays(0).ToString("yyyy/MM/dd") + "#" +
                                    " AND [Date] <= #" + endDate.ToString("yyyy/MM/dd") + "#)"
                                    + " AND Amount <= " + lr.Amount_borrowed.ToString() + ") ");
                            if (l.loanTenure > 0)
                            {
                                foreach (MigrationDataset.DisbursementsRow dr in drrs)
                                {
                                    if (dr.Amount >= 0)
                                    {
                                        var disbDate = lr.Loan_start_date;
                                        if (dr.IsDateNull() == false)
                                        {
                                            disbDate = dr.Date;
                                        }
                                        var pm = "1";
                                        var crAccountNo = "";
                                        if (dr.IsPayment_modeNull() == false)
                                        {
                                            if (dr.Payment_mode.ToLower().Trim() == "cheque")
                                            {
                                                pm = "2";
                                                if (dr.Bank == "Prudential SME") crAccountNo = "1046";
                                                if (dr.Bank == "Fidelity Bank") crAccountNo = "1049";
                                            }
                                            if (dr.Payment_mode.ToLower().Trim() == "bank")
                                            {
                                                pm = "3";
                                                if (dr.Bank == "Prudential SME") crAccountNo = "1046";
                                                if (dr.Bank == "Fidelity Bank") crAccountNo = "1049";
                                            }
                                            if (dr.Payment_mode.Trim() == "Shareholder cash")
                                            {
                                                pm = "1";
                                                crAccountNo = "3200";
                                            }
                                            if (dr.Payment_mode.Trim() == "Investor cash")
                                            {
                                                pm = "1";
                                                crAccountNo = "3201";
                                            }
                                            if (dr.Payment_mode.Trim() == "Direct cheque")
                                            {
                                                pm = "2";
                                                if (dr.Bank == "Prudential SME") crAccountNo = "1046";
                                                if (dr.Bank == "Fidelity Bank") crAccountNo = "1049";
                                            }
                                            if (dr.Payment_mode.Trim() == "Multi cheque payment")
                                            {
                                                pm = "2";
                                                if (dr.Bank == "Prudential SME") crAccountNo = "1046";
                                                if (dr.Bank == "Fidelity Bank") crAccountNo = "1049";
                                            }
                                            if (dr.Payment_mode.Trim() == "Vault cash")
                                            {
                                                pm = "1";
                                                crAccountNo = "1000";
                                            }
                                        }
                                        LoansHelper.DisburseLoan(le, l, dr.Amount, l.amountApproved, l.amountDisbursed, disbDate,
                                            "", pm, "", ent, false, "SYSTEM", pm, crAccountNo, false);
                                    }
                                }
                            }

                            var rrs = ds.Receipts.Select("[Loan ID] = '" + lr.Loan_ID + "'");
                            if (rrs.Length == 0)
                            {
                            }
                            var totalInterest = l.repaymentSchedules.Sum(p => p.interestPayment);
                            var totalPayments = 0.0;
                            var paymentDate = DateTime.MinValue;
                            foreach (MigrationDataset.ReceiptsRow rr in rrs)
                            {
                                if ((rr.Purpose != "Processing & Application fee" && rr.Purpose != "processing & Application fee")
                                    && (rr.Purpose != "Penalty charge" && rr.Purpose != "Extra interest on loan"))
                                {
                                    totalPayments += rr.Amount;
                                    paymentDate = (paymentDate < rr.Date) ? rr.Date : paymentDate;
                                }
                            }
                            if (cr.Ref != 16 && totalInterest + balance < totalPayments)
                            {
                                var amount = totalPayments - (totalInterest + balance);
                                l.repaymentSchedules.Add(new repaymentSchedule
                                {
                                    loan = l,
                                    loanID = l.loanID,
                                    interestBalance = amount,
                                    interestPayment = amount,
                                    interestWritenOff = 0,
                                    principalBalance = 0,
                                    principalPayment = 0,
                                    proposedInterestWriteOff = 0,
                                    creation_date = DateTime.Now,
                                    creator = "SYSTEM",
                                    repaymentDate = paymentDate
                                }
                        );
                                var sched = l.repaymentSchedules.ToList();

                                var jba = coreLogic.JournalExtensions.Post("LN", l.loanType.accountsReceivableAccountID,
                                    l.loanType.unearnedInterestAccountID, amount,
                                    "Loan Additional Interest- " + l.client.surName + "," + l.client.otherNames,
                                    pro.currency_id.Value, l.disbursementDate.Value, l.loanNo, ent, "SYSTEM");
                                ent.jnl_batch.Add(jba);
                            } 
                            foreach (MigrationDataset.ReceiptsRow rr in rrs)
                            {
                                if (rr.Amount >= 0)
                                {
                                    var pm = "1";
                                    if (rr.IsMode_of_paymentNull() == false)
                                    {
                                        if (rr.Mode_of_payment == "Cash payment") pm = "1";
                                        if (rr.Mode_of_payment == "Cheque payment") pm = "2";
                                        if (rr.Mode_of_payment == "Bank payment") pm = "3";
                                    }
                                    var amt2 = 0.0;
                                    var amt = rr.Amount;
                                    var checkNo = "";
                                    if (rr.Is_Cheque______E_zwich_NoNull() == false) checkNo = rr._Cheque______E_zwich_No;
                                    var rm = "1";
                                    if (rr.IsPurposeNull() == false)
                                    {
                                        if (rr.Purpose == "Loan repayment")
                                        {
                                            if ((comments == "Early repayment" ) && balance > 0)
                                            {
                                                rm = "2";
                                                amt = (balance <= rr.Amount) ? balance : rr.Amount;
                                                amt2 = rr.Amount - balance;
                                                balance -= rr.Amount;
                                            }
                                            else if ((comments == "Early repayment" ) )
                                            {
                                                rm = "3";
                                            }
                                            else
                                            {
                                            rm = "1";
                                            }
                                        }
                                        if (rr.Purpose == "Processing & Application fee" || rr.Purpose == "processing & Application fee")
                                        {
                                            rm = "6";
                                            l.processingFee += rr.Amount;
                                        }
                                        if (rr.Purpose == "Interest")
                                        {
                                            rm = "3";
                                        }
                                        if (rr.Purpose == "Penalty charge" || rr.Purpose == "Extra interest on loan")
                                        {
                                            rm = "7";
                                            var lp = new loanPenalty
                                            {
                                                proposedAmount = 0,
                                                penaltyFee = rr.Amount,
                                                penaltyBalance = rr.Amount,
                                                penaltyDate = rr.Date,
                                                creation_date = DateTime.Now,
                                                creator = "SYSTEM"
                                            };
                                            l.loanPenalties.Add(lp);
                                        }
                                    } 
                                    LoansHelper.ReceivePayment(le, l, amt, rr.Date, rm, "", "", checkNo, ent, "SYSTEM",
                                        int.Parse(pm), (amt2 > 0 && rr.Amount > amt && rm == "2")?rr.Amount:0, null);
                                    if (amt2 > 0 && rr.Amount>amt && rm=="2")
                                    {
                                        LoansHelper.ReceivePayment(le, l, amt2, rr.Date, "3", "", "", checkNo, ent, "SYSTEM",
                                            int.Parse(pm), 1000, null);
                                    }
                                }
                            }
                            if ((comments == "Early repayment") || ps=="Completed")
                            {
                                LoansHelper.WriteOffInterest(le, ent, l, "SYSTEM");
                            }
                            if (comments != "Early repayment")
                            {
                                LoansHelper.ClearOffInterest(le, ent, l, "SYSTEM");
                            }
                        }
                    }
                    else
                    {
                        foreach (MigrationDataset.SA_LOANSRow lr in ds.SA_LOANS)
                        {
                            if (lr.IsDescriptionNull() == false)
                            {
                                loan l = new loan();
                                l.loanStatusID = 4;
                                l.amountApproved = lr.Loan_amount;
                                l.amountRequested = lr.Loan_amount;
                                l.amountDisbursed = 0;
                                l.applicationDate = lr.Loan_Date;
                                l.applicationFee = 0;
                                l.processingFee = 0;
                                l.balance = 0;
                                l.commission = 0;
                                l.creation_date = DateTime.Now;
                                l.creator = "SYSTEM";
                                var sub = "";
                                if (lr.Loan_type == "Invoiced loans")
                                {
                                    l.loanTypeID = 5;
                                    l.loanTenure = 0;
                                    sub = "I";
                                }
                                else
                                {
                                    l.loanTypeID = 2;
                                    l.loanTenure = 0;
                                    sub = "M";
                                }
                                var balance = lr.Loan_amount;

                                l.loanType = le.loanTypes.FirstOrDefault(p => p.loanTypeID == l.loanTypeID);
                                l.loanNo = sub + coreExtensions.NextSystemNumber("LOAN_" + sub);
                                l.repaymentModeID = -1;
                                l.gracePeriod = 0;
                                l.interestRate = 6.5;
                                l.interestTypeID = 1;
                                l.creditOfficerNotes = "";
                                l.approvalComments = "";
                                l.tenureTypeID = 1;
                                //if (l.loanTenure == 0) l.loanTenure = 1;

                                c.loans.Add(l);
                                LoansHelper.PostLoan(le, l, lr.Loan_amount, l.amountApproved, lr.Loan_Date,
                                    "", "2", "", ent, false, "SYSTEM", "2", "");
                                bool paid=false;

                                var rrs = ds.SA_REPAYMENT.Select("[Track number]=" + lr.Track_number.ToString());
                                foreach (MigrationDataset.SA_REPAYMENTRow rr in rrs)
                                {
                                    if (rr.Repayment >= 0)
                                    {
                                        paid = true;
                                        var pm = "1";
                                        if (rr.IsModeNull() == false)
                                        {
                                            if (rr.Mode == "Cash") pm = "1";
                                            if (rr.Mode == "Cheque") pm = "2";
                                            if (rr.Mode == "Bank") pm = "3";
                                        }
                                        var checkNo = "";
                                        if (rr.IsCheque_detailsNull() == false) checkNo = rr.Cheque_details;
                                        var rm = "2"; 
                                        LoansHelper.ReceivePayment(le, l, rr.Repayment, rr.Payment_date, rm, "", "",
                                            checkNo, ent, "SYSTEM",
                                            int.Parse(pm));
                                    }
                                }
                                var frs = ds.SA_FEES.Select("[Track number]=" + lr.Track_number.ToString());
                                foreach (MigrationDataset.SA_FEESRow rr in frs)
                                {
                                    if (rr.Processing_fee >= 0)
                                    {
                                        var pm = "1";
                                        if (rr.IsModeNull() == false)
                                        {
                                            if (rr.Mode == "Cash") pm = "1";
                                            if (rr.Mode == "Cheque") pm = "2";
                                            if (rr.Mode == "Bank") pm = "3";
                                        }
                                        var checkNo = "";
                                        if (rr.IsCheque_detailsNull() == false) checkNo = rr.Cheque_details;
                                        var rm = "6";
                                        l.processingFee += rr.Processing_fee;
                                        LoansHelper.ReceivePayment(le, l, rr.Processing_fee, rr.Payment_date, rm, "", "",
                                            checkNo, ent, "SYSTEM",
                                            int.Parse(pm));
                                    }
                                }
                                var irs = ds.SA_INTEEREST.Select("[Track number]=" + lr.Track_number.ToString());
                                var totalInterest = l.repaymentSchedules.Sum(p => p.interestBalance);
                                foreach (MigrationDataset.SA_INTEERESTRow rr in irs)
                                {
                                    if (rr.Interest >= 0)
                                    {
                                        if (rr.Interest > totalInterest)
                                        {
                                            var pm = "1";
                                            if (rr.IsModeNull() == false)
                                            {
                                                if (rr.Mode == "Cash") pm = "1";
                                                if (rr.Mode == "Cheque") pm = "2";
                                                if (rr.Mode == "Bank") pm = "3";
                                            }
                                            var checkNo = "";
                                            if (rr.IsCheque_detailsNull() == false) checkNo = rr.Cheque_details;
                                            var rm = "3";
                                            var sched = l.repaymentSchedules.ToList(); 
                                            LoansHelper.ReceivePayment(le, l, rr.Interest, rr.Payment_date, rm, "", "",
                                                checkNo, ent, "SYSTEM",
                                                int.Parse(pm));
                                            LoansHelper.WriteOffInterest(le, ent, l, "SYSTEM");
                                        }
                                        else
                                        {
                                            var pm = "1";
                                            if (rr.IsModeNull() == false)
                                            {
                                                if (rr.Mode == "Cash") pm = "1";
                                                if (rr.Mode == "Cheque") pm = "2";
                                                if (rr.Mode == "Bank") pm = "3";
                                            }
                                            var checkNo = "";
                                            if (rr.IsCheque_detailsNull() == false) checkNo = rr.Cheque_details;
                                            var rm = "3";
                                            LoansHelper.ReceivePayment(le, l, rr.Interest, rr.Payment_date, rm, "", "",
                                                checkNo, ent, "SYSTEM",
                                                int.Parse(pm));
                                            LoansHelper.WriteOffInterest(le, ent, l, "SYSTEM");
                                        }
                                    }
                                }

                                if (paid == false)
                                {
                                    l.processingFee = Math.Ceiling(l.amountRequested * 0.02);
                                    l.processingFeeBalance = Math.Ceiling(l.amountRequested * 0.02);
                                }
                            }
                        }
                    }

                    le.clients.Add(c);
                }

            }

            foreach (MigrationDataset.DisbursementsRow dr in ds.Disbursements)
            {
                if (dr.IsAmountNull()==false && dr.Amount >= 0)
                {
                    var disbDate = dr.Date;
                    var pm = "1";
                    var crAccountNo = "";
                    if (dr.IsPayment_modeNull() == false)
                    {
                        if (dr.Payment_mode.ToLower().Trim() == "cheque")
                        {
                            pm = "2";
                            if (dr.Bank == "Prudential SME") crAccountNo = "1046";
                            if (dr.Bank == "Fidelity Bank") crAccountNo = "1049";
                        }
                        if (dr.Payment_mode.ToLower().Trim() == "bank")
                        {
                            pm = "3";
                            if (dr.Bank == "Prudential SME") crAccountNo = "1046";
                            if (dr.Bank == "Fidelity Bank") crAccountNo = "1049";
                        }
                        if (dr.Payment_mode.Trim() == "Shareholder cash")
                        {
                            pm = "1";
                            crAccountNo = "3200";
                        }
                        if (dr.Payment_mode.Trim() == "Investor cash")
                        {
                            pm = "1";
                            crAccountNo = "3201";
                        }
                        if (dr.Payment_mode.Trim() == "Direct cheque")
                        {
                            pm = "2";
                            if (dr.Bank == "Prudential SME") crAccountNo = "1046";
                            if (dr.Bank == "Fidelity Bank") crAccountNo = "1049";
                        }
                        if (dr.Payment_mode.Trim() == "Multi cheque payment")
                        {
                            pm = "2";
                            if (dr.Bank == "Prudential SME") crAccountNo = "1046";
                            if (dr.Bank == "Fidelity Bank") crAccountNo = "1049";
                        }
                        if (dr.Payment_mode.Trim() == "Vault cash")
                        {
                            pm = "1";
                            crAccountNo = "1000";
                        }
                    }
                    if (dr.IsCheque_numberNull() == false)
                    {
                        string key = dr.Cheque_number;
                        if (listPost.ContainsKey(key))
                        {
                            PostData data = listPost[key];
                            data.amount += dr.Amount;
                            data.Details.Add(new PostData
                            {
                                description = "Loan Disbursement " + dr.Account_Name,
                                date = dr.Date,
                                checkNo = dr.Cheque_number,
                                account = "1001",
                                amount = dr.Amount
                            });
                            listPost[key] = data;
                        }
                        else
                        {
                            PostData data = new PostData();
                            data.description = "Loan Disbursement";
                            data.date = dr.Date;
                            data.account = crAccountNo;
                            data.amount = dr.Amount;
                            data.checkNo = dr.Cheque_number;
                            if (data.Details == null) data.Details = new List<PostData>();
                            data.Details.Add(new PostData
                            {
                                description = "Loan Disbursement " + dr.Account_Name,
                                date = dr.Date,
                                checkNo = dr.Cheque_number,
                                account = "1001",
                                amount = dr.Amount
                            });
                            listPost.Add(key, data);
                        }
                    }
                    else
                    {
                        PostData data = new PostData();
                        data.description = "Loan Disbursement" + dr.Account_Name;
                        data.date = dr.Date;
                        data.account = crAccountNo;
                        data.amount = dr.Amount;
                        data.checkNo = "";
                        listPost.Add(System.Guid.NewGuid().ToString(), data);
                        if (data.Details == null) data.Details = new List<PostData>();
                        data.Details.Add(new PostData
                        {
                            description = "Loan Disbursement " + dr.Account_Name,
                            date = dr.Date,
                            checkNo = "",
                            account = "1001",
                            amount = dr.Amount
                        });
                    }

                }
            }

            foreach (KeyValuePair<string, PostData> data in listPost)
            {
                var jb0 = new jnl_batch
                {
                    batch_no = coreExtensions.NextGLBatchNumber(),
                    creation_date = DateTime.Now,
                    creator = "SYSTEM",
                    is_adj = false,
                    multi_currency = false,
                    posted = false,
                    source = "LD"
                };

                var acc = ent.accts.FirstOrDefault(p => p.acc_num == data.Value.account);
                var j = JournalExtensions.Post("LD", "CR",
                    acc.acct_id, data.Value.amount, data.Value.description, pro.currency_id.Value,
                    data.Value.date, "", ent, "SYSTEM");
                jb0.jnl.Add(j);
                foreach (PostData data2 in data.Value.Details)
                {
                    acc = ent.accts.FirstOrDefault(p => p.acc_num == data2.account);
                    j = JournalExtensions.Post("LD", "DR",
                       acc.acct_id, data2.amount, data2.description, pro.currency_id.Value,
                       data2.date, "", ent, "SYSTEM");
                    jb0.jnl.Add(j);
                }

                if (jb0.jnl.Count > 0)
                {
                    ent.jnl_batch.Add(jb0);
                }
            }
            
            le.SaveChanges();
            ent.SaveChanges();
        }*/
    }

    public class PostData
    {
        public string description;
        public double amount;
        public string account;
        public string checkNo;
        public DateTime date;
        public List<PostData> Details;
    }
}
