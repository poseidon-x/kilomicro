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
    public partial class susuGroup : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/susuGroup.aspx"; }
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
                newVals["susuGroupNo"] = 0; 
                newVals["susuGroupName"] = "";
                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_susuGroups";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_susuGroups";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_susuGroups";
            this.RadGrid1.ExportSettings.Pdf.Title = "susuGroups Defined in System";
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

        protected void PopulateRepaymentModes(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
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
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true; 
                var susuGroupID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["susuGroupID"];
                var b = le.susuGroups.FirstOrDefault(p => p.susuGroupID == susuGroupID);

                b.susuGroupNo = int.Parse(newVals["susuGroupNo"].ToString()); 
                b.susuGroupName = newVals["susuGroupName"].ToString(); 
                le.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
                Session["defaultRepaymentModeID"] = null;                 
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
                coreLogic.susuGroup b = new coreLogic.susuGroup();

                b.susuGroupNo = int.Parse(newVals["susuGroupNo"].ToString()); 
                b.susuGroupName = newVals["susuGroupName"].ToString(); 
                le.susuGroups.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
                     
            }
            catch (Exception ex) {  }
        }

    }
}
