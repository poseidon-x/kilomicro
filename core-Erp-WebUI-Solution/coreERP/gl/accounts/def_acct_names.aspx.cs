using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;
using System.Collections;

namespace coreERP.gl.accounts
{
    public partial class def_acct_names : corePage
    {
        public override string URL
        {
            get { return "~/gl/accounts/def_acct_names.aspx"; }
        }


        core_dbEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new core_dbEntities();
        }

        void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "code")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.def_acct_names b = new coreLogic.def_acct_names();
                    b.code = newVals["code"].ToString();
                    b.description = newVals["description"].ToString();
                    b.creation_date = DateTime.Now;
                    b.creator = User.Identity.Name;
                    ent.def_acct_names.Add(b);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "code")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var vcode = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["code"].ToString();
                    var b = (from p in ent.def_acct_names where p.code == vcode select p).First<coreLogic.def_acct_names>();
                    b.code = newVals["code"].ToString();
                    b.description = newVals["description"].ToString();
                    b.modification_date = DateTime.Now;
                    b.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind(); 
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_ItemInserted(object source, Telerik.Web.UI.GridInsertedEventArgs e)
        { 
        }

        protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "def_acct_id")
                {
                    e.Canceled = true; 
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["code"] = string.Empty;
                    newVals["description"] = string.Empty; 
                    newVals["creator"] = string.Empty;
                    newVals["creation_date"] = DateTime.Now; 
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
            else if (e.CommandName == "CancelEdit")
            {
                //closes the edit form
                RadGrid1.MasterTableView.ClearEditItems(); 
            }
            else if (e.CommandName == "Edit")
            { 
            }   
        }

        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc)
        { 
            var accs = (from a in ent.accts
                        from c in ent.currencies
                        where a.currencies.currency_id == c.currency_id
                        select new
                        {
                            a.acct_id,
                            a.acc_num,
                            a.acc_name,
                            c.major_name,
                            c.major_symbol
                        }).ToList();
            cboAcc.DataSource = accs;
            if (Session["gl_acct_id"] == null || Session["gl_acct_id"].ToString()=="0")
            {
                cboAcc.Items.Insert(0, new RadComboBoxItem("Select a GL Account", null));
            }
            else cboAcc.SelectedValue = Session["gl_acct_id"].ToString();

        }

        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditFormItem && e.Item.IsInEditMode == true)
            {
                try
                {
                    var item = e.Item as GridEditFormItem;
                    var rcb = item.FindControl("cboGLAcc") as RadComboBox;
                    if (rcb != null)
                    {
                        PopulateAccounts(rcb);
                        rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboGLAcc_SelectedIndexChanged);
                        rcb.DataBind();
                    }
                }
                catch (Exception) { }
            }
        }

        public string AccountName(int def_acct_id)
        {
            var item= (from c in ent.def_accts
                    from m in ent.accts
                    where c.accts.acct_id == m.acct_id
                        && c.def_acct_id == def_acct_id
                    select new
                    {
                        m.acc_num,
                        m.acc_name
                    }).First();
            return item.acc_num + " - " + item.acc_name;
        }

        public string AccountCurrency(int bank_acct_id)
        {
            var item = (from c in ent.bank_accts
                        from m in ent.accts
                        from u in ent.currencies
                        where c.accts.acct_id == m.acct_id
                            && m.currencies.currency_id==u.currency_id
                            && c.bank_acct_id == bank_acct_id
                        select new
                        {
                            m.acc_num,
                            m.acc_name,
                            u.major_name,
                            u.major_symbol
                        }).First();
            return item.major_name +  " (" + item.major_symbol + ")";
        }
        
        public int LocationID(int country_id)
        { 
            return (from c in ent.countries
                    from m in ent.currencies
                    where c.currencies.currency_id == m.currency_id
                        && (country_id == 0 || c.country_id == country_id)
                    select new
                    {
                        m.currency_id
                    }).First().currency_id;
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = false;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            RadGrid1.ExportSettings.HideStructureColumns = false;
            this.RadGrid1.ExportSettings.FileName = "coreERP_banks";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = false;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.HideStructureColumns = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_banks";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            RadGrid1.ExportSettings.HideStructureColumns = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_banks";
            this.RadGrid1.ExportSettings.Pdf.Title = "Currencies Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            //RadGrid1.ExportSettings.HideStructureColumns = true;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        protected void cboGLAcc_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                PopulateAccounts((RadComboBox)sender);
            }
            catch (Exception ex) { }
        }

        protected void cboGLAcc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Session["gl_acct_id"] = ((RadComboBox)sender).SelectedValue;
            }
            catch (Exception ex) { }
        }

        protected void cboGLAcc_Load(object sender, EventArgs e)
        {
            try
            {
                //PopulateAccounts((RadComboBox)sender);
            }
            catch (Exception ex) { }
        }

    }
}
