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
    public partial class tenor : corePage
    {
        core_dbEntities ent = new core_dbEntities();
        coreLoansEntities le = new coreLoansEntities();
        public override string URL
        {
            get { return "~/ln/setup/tenor.aspx"; }
        }


        protected void Page_Load(object sender, EventArgs e)
        { 
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
                newVals["tenror1"] = 0;
                newVals["defaultRepaymentModeID"] = 0;
                newVals["loanTypeID"] = 0;
                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_tenors";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_tenors";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_tenors";
            this.RadGrid1.ExportSettings.Pdf.Title = "tenors Defined in System";
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

        public string RepyamentModeName(object accID)
        {
            var rpID = 0;
            if (accID != null) rpID = int.Parse(accID.ToString());
            var item = (from m in le.repaymentModes
                        where m.repaymentModeID == rpID
                        select new
                        {
                            m.repaymentModeName
                        }).FirstOrDefault();
            return item == null ? "" : item.repaymentModeName;
        }

        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc)
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

        protected void PopulateRepaymentModes(Telerik.Web.UI.RadComboBox cboAcc)
        { 
                var accs = (from a in le.repaymentModes 
                            select new
                            {
                                a.repaymentModeName,
                                a.repaymentModeID 
                            });
                cboAcc.Items.Clear(); 
                foreach (var cur in accs)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = cur.repaymentModeName ;
                    item.Value = cur.repaymentModeID.ToString();
                     
                    cboAcc.Items.Add(item);

                }
                cboAcc.Items.Insert(0, new RadComboBoxItem("", "0")); 
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
                var rcb =  item["defaultRepaymentModeID"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateRepaymentModes(rcb);
                    rcb.ValidationGroup = "defaultRepaymentModeID";
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAcc_SelectedIndexChanged);
                    //rcb.DataBind();
                }
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        { 
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true; 
                var tenorID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["tenorID"];
                var b = le.tenors.FirstOrDefault(p=> p.tenorid==tenorID);
                
                b.tenor1 = int.Parse(newVals["tenor1"].ToString());
                if (newVals["defaultInterestRate"] != null)
                {
                    b.defaultInterestRate = double.Parse(newVals["defaultInterestRate"].ToString());
                }
                if (newVals["defaultPenaltyRate"] != null)
                {
                    b.defaultPenaltyRate = double.Parse(newVals["defaultPenaltyRate"].ToString());
                }
                if (newVals["defaultGracePeriod"] != null)
                {
                    b.defaultGracePeriod = int.Parse(newVals["defaultGracePeriod"].ToString());
                }
                if (newVals["defaultApplicationFeeRate"] != null)
                {
                    b.defaultApplicationFeeRate = double.Parse(newVals["defaultApplicationFeeRate"].ToString());
                }
                if (newVals["defaultProcessingFeeRate"] != null)
                {
                    b.defaultProcessingFeeRate = double.Parse(newVals["defaultProcessingFeeRate"].ToString());
                }
                if (newVals["defaultCommissionRate"] != null)
                {
                    b.defaultCommissionRate = double.Parse(newVals["defaultCommissionRate"].ToString());
                }
                if (newVals["loanTypeID"] != null)
                {
                    b.loanTypeID = int.Parse(newVals["loanTypeID"].ToString());
                }
                le.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
                Session["defaultRepaymentModeID"] = null;  
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        { 
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true; 
                coreLogic.tenor b = new coreLogic.tenor();
                 
                if (Session["defaultRepaymentModeID"] != null)
                {
                    b.defaultRepaymentModeID = int.Parse(Session["defaultRepaymentModeID"].ToString());
                }
                b.tenor1 = int.Parse(newVals["tenor1"].ToString());
                if (newVals["defaultInterestRate"] != null)
                {
                    b.defaultInterestRate = double.Parse(newVals["defaultInterestRate"].ToString());
                }
                if (newVals["defaultPenaltyRate"] != null)
                {
                    b.defaultPenaltyRate = double.Parse(newVals["defaultPenaltyRate"].ToString());
                }
                if (newVals["defaultGracePeriod"] != null)
                {
                    b.defaultGracePeriod = int.Parse(newVals["defaultGracePeriod"].ToString());
                }
                if (newVals["defaultApplicationFeeRate"] != null)
                {
                    b.defaultApplicationFeeRate = double.Parse(newVals["defaultApplicationFeeRate"].ToString());
                }
                if (newVals["defaultProcessingFeeRate"] != null)
                {
                    b.defaultProcessingFeeRate = double.Parse(newVals["defaultProcessingFeeRate"].ToString());
                }
                if (newVals["defaultCommissionRate"] != null)
                {
                    b.defaultCommissionRate = double.Parse(newVals["defaultCommissionRate"].ToString());
                }
                if (newVals["loanTypeID"] != null)
                {
                    b.loanTypeID = int.Parse(newVals["loanTypeID"].ToString());
                }

                le.tenors.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();

                Session["defaultRepaymentModeID"] = null;      
                     
        }

    }
}
