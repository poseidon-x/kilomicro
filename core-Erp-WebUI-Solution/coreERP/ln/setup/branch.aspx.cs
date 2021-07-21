using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.ln.setup
{
    public partial class branch : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/branch.aspx"; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new core_dbEntities();
            le=new coreLoansEntities();
        }
         
        protected void RadGrid1_ItemInserted(object source, Telerik.Web.UI.GridInsertedEventArgs e)
        {
            //throw e.Exception;
        }

        protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                e.Canceled = true;
                var newVals = new System.Collections.Specialized.ListDictionary();
                newVals["branchName"] = string.Empty;
                newVals["branchCode"] = string.Empty;
                newVals["gl_ou_id"] = 0; 
                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_branches";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_branches";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_branches";
            this.RadGrid1.ExportSettings.Pdf.Title = "Branches Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        public string AccountName(int accID)
        {
           var item = (from m in ent.accts
                        where m.acct_id == accID
                        select new
                        {
                            m.acc_num,
                            m.acc_name
                        }).FirstOrDefault();
            return item == null ? "" : item.acc_num + " - " + item.acc_name;
        }

        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                var accs = (from a in ent.accts
                            from c in ent.currencies
                            from h in ent.acct_heads
                            where a.currencies.currency_id == c.currency_id
                                && a.acct_heads.acct_head_id == h.acct_head_id
                            select new
                            {
                                a.acct_id,
                                a.acc_num,
                                a.acc_name,
                                c.major_name,
                                c.major_symbol,
                                h.head_name
                            });
                cboAcc.Items.Clear();
                var profile = ent.comp_prof.FirstOrDefault();
                var numb4 = profile != null ? profile.num_b4_name : false;
                foreach (var cur in accs)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = numb4 ? cur.acc_num + " - " + cur.acc_name
                         : cur.acc_name + " - " + cur.acc_num; ;
                    item.Value = cur.acct_id.ToString();

                    item.Attributes.Add("acc_num", cur.acc_num);
                    item.Attributes.Add("acc_name", cur.acc_name);
                    item.Attributes.Add("acc_cur", cur.major_name);
                    item.Attributes.Add("head_name", cur.head_name);

                    cboAcc.Items.Add(item);

                }
                cboAcc.Items.Insert(0, new RadComboBoxItem("", "0"));
            }
            catch (Exception ex) { }
        }

        void cboAcc_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var rcb = o as RadComboBox;
            if (rcb != null)
            {
                Session[rcb.ValidationGroup] = e.Value;
            }
        }

        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true)
            {
                var item = e.Item as GridEditableItem;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true; 
                var branchID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["branchID"];
                var b = le.branches.FirstOrDefault(p=> p.branchID==branchID);
                if(Session["vaultAccountID"] != null)
                {
                    b.vaultAccountID=int.Parse(Session["vaultAccountID"].ToString());
                }
                if (Session["bankAccountID"] != null)
                {
                    b.bankAccountID = int.Parse(Session["bankAccountID"].ToString());
                }
                if (Session["cashierTillAccountID"] != null)
                {
                    b.cashierTillAccountID = int.Parse(Session["cashierTillAccountID"].ToString());
                }
                if (Session["unearnedInterestAccountID"] != null)
                {
                    b.unearnedInterestAccountID = int.Parse(Session["unearnedInterestAccountID"].ToString());
                }
                if (Session["interestIncomeAccountID"] != null)
                {
                    b.interestIncomeAccountID = int.Parse(Session["interestIncomeAccountID"].ToString());
                }
                if (Session["unpaidCommissionAccountID"] != null)
                {
                    b.unpaidCommissionAccountID = int.Parse(Session["unpaidCommissionAccountID"].ToString());
                }
                if (Session["commissionAndFeesAccountID"] != null)
                {
                    b.commissionAndFeesAccountID = int.Parse(Session["commissionAndFeesAccountID"].ToString());
                }
                if (Session["accountsReceivableAccountID"] != null)
                {
                    b.accountsReceivableAccountID = int.Parse(Session["accountsReceivableAccountID"].ToString());
                }
                if (Session["unearnedExtraChargesAccountID"] != null)
                {
                    b.unearnedExtraChargesAccountID = int.Parse(Session["unearnedExtraChargesAccountID"].ToString());
                }
                b.branchName = newVals["branchName"].ToString();
                b.branchCode = newVals["branchCode"].ToString();
                if (newVals["gl_ou_id"] != null)
                {
                    b.gl_ou_id = int.Parse(newVals["gl_ou_id"].ToString());
                }
                le.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
                Session["vaultAccountID"] = null;
                Session["bankAccountID"] = null;
                Session["cashierTillAccountID"] = null;
                Session["unearnedInterestAccountID"] = null;
                Session["interestIncomeAccountID"] = null;
                Session["unpaidCommissionAccountID"] = null;
                Session["commissionAndFeesAccountID"] = null;
                Session["accountsReceivableAccountID"] = null;
                Session["unearnedExtraChargesAccountID"] = null;                  
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true; 
                coreLogic.branch b = new coreLogic.branch();
                 if(Session["vaultAccountID"] != null)
                {
                    b.vaultAccountID=int.Parse(Session["vaultAccountID"].ToString());
                }
                if (Session["bankAccountID"] != null)
                {
                    b.bankAccountID = int.Parse(Session["bankAccountID"].ToString());
                }
                if (Session["cashierTillAccountID"] != null)
                {
                    b.cashierTillAccountID = int.Parse(Session["cashierTillAccountID"].ToString());
                }
                if (Session["unearnedInterestAccountID"] != null)
                {
                    b.unearnedInterestAccountID = int.Parse(Session["unearnedInterestAccountID"].ToString());
                }
                if (Session["interestIncomeAccountID"] != null)
                {
                    b.interestIncomeAccountID = int.Parse(Session["interestIncomeAccountID"].ToString());
                }
                if (Session["unpaidCommissionAccountID"] != null)
                {
                    b.unpaidCommissionAccountID = int.Parse(Session["unpaidCommissionAccountID"].ToString());
                }
                if (Session["commissionAndFeesAccountID"] != null)
                {
                    b.commissionAndFeesAccountID = int.Parse(Session["commissionAndFeesAccountID"].ToString());
                }
                if (Session["accountsReceivableAccountID"] != null)
                {
                    b.accountsReceivableAccountID = int.Parse(Session["accountsReceivableAccountID"].ToString());
                }
                if (Session["unearnedExtraChargesAccountID"] != null)
                {
                    b.unearnedExtraChargesAccountID = int.Parse(Session["unearnedExtraChargesAccountID"].ToString());
                }
                b.branchName = newVals["branchName"].ToString();
                b.branchCode = newVals["branchCode"].ToString();
                if (newVals["gl_ou_id"] != null)
                {
                    b.gl_ou_id = int.Parse(newVals["gl_ou_id"].ToString());
                }


                //Check if branch existed already
                var branchOld = le.branches.SingleOrDefault(r => r.branchName.Trim().ToLower() == b.branchName.Trim().ToLower());
                if(branchOld == null){
                    var prefix = b.branchName.Substring(0, 2).ToUpper();
                    //Insert the branch details into dbo.sys_no table
                    var branchSysNo=new sys_no
                    {
                        creation_date = DateTime.Now.Date,
                        creator = "SYSTEM",
                        name = "loan.cl.accountNumber." + prefix,
                        prefix = prefix,
                        value = 0,
                        step = 1,
                        suffix=""
                    };
                    ent = new core_dbEntities();
                    ent.sys_no.Add(branchSysNo);
                }
                
                le.branches.Add(b);
                le.SaveChanges();
                ent.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
                    
            }
            catch (Exception ex)
            {
            }
        }

    }
}
