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
    public partial class assetCategory : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/assetCategory.aspx"; }
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

        public string GetDescription(object dpMeth)
        {
            var desc = "";
            if (dpMeth != null)
            {
                switch (dpMeth.ToString())
                {
                    case "1":
                        desc = "Straight Line";
                        break;
                    case "2":
                        desc = "Reducing Balance";
                        break;
                }
            }

            return desc;
        }

        protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "assetSubCategoryID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var assetCategoryID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["assetCategoryID"];
                    Session["assetCategoryID"] = assetCategoryID;
                    newVals["assetSubCategoryName"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["assetCategoryName"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_assetCategories";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_assetCategories";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_assetCategories";
            this.RadGrid1.ExportSettings.Pdf.Title = "assetCategoryes Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
   
        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "assetCategoryID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var assetCategoryID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["assetCategoryID"];
                    var b = le.assetCategories.FirstOrDefault(p => p.assetCategoryID == assetCategoryID);
                    b.assetCategoryName = newVals["assetCategoryName"].ToString();
                    b.depreciationMethod = int.Parse(Session["depreciationMethod"].ToString());
                    if (Session["depreciationAccountID"] != null)
                    {
                        b.depreciationAccountID = int.Parse(Session["depreciationAccountID"].ToString());
                    }
                    if (Session["accumulatedDepreciationAccountID"] != null)
                    {
                        b.accumulatedDepreciationAccountID = int.Parse(Session["accumulatedDepreciationAccountID"].ToString());
                    }
                    if (Session["depreciationMethod"] != null)
                    {
                        b.depreciationMethod = int.Parse(Session["depreciationMethod"].ToString());
                    }
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "assetSubCategoryID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var assetSubCategoryID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["assetSubCategoryID"];
                    var b = le.assetSubCategories.FirstOrDefault(p => p.assetSubCategoryID == assetSubCategoryID);
                    b.assetSubCategoryName = newVals["assetSubCategoryName"].ToString();

                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "assetCategoryID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.assetCategory b = new coreLogic.assetCategory();
                    b.assetCategoryName = newVals["assetCategoryName"].ToString();
                    b.depreciationMethod = int.Parse(Session["depreciationMethod"].ToString());
                    if (Session["depreciationAccountID"] != null)
                    {
                        b.depreciationAccountID = int.Parse(Session["depreciationAccountID"].ToString());
                    }
                    if (Session["accumulatedDepreciationAccountID"] != null)
                    {
                        b.accumulatedDepreciationAccountID = int.Parse(Session["accumulatedDepreciationAccountID"].ToString());
                    }
                    if (Session["depreciationMethod"] != null)
                    {
                        b.depreciationMethod = int.Parse(Session["depreciationMethod"].ToString());
                    }
                    le.assetCategories.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "assetSubCategoryID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.assetSubCategory b = new coreLogic.assetSubCategory();
                    b.assetSubCategoryName = newVals["assetSubCategoryName"].ToString();
                    var assetCategoryID = (int)Session["assetCategoryID"];
                    b.assetCategoryID = assetCategoryID;

                    le.assetSubCategories.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }  
            }
            catch (Exception ex) {  }
        }

        public string AccountName(object accID)
        {
            int aID = accID==null?-1:int.Parse(accID.ToString());
            var item = (from m in ent.accts
                        where m.acct_id == aID
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
                int? id = null;
                if (Session[cboAcc.ValidationGroup] != null)
                {
                    id = int.Parse(Session[cboAcc.ValidationGroup].ToString());
                }
                var accs = (from a in ent.vw_accounts
                            from c in ent.currencies
                            where (a.currency_id == c.currency_id)
                                && (a.acct_id == id)
                            select new
                            {
                                a.acct_id,
                                a.acc_num,
                                a.acc_name,
                                major_name = c.major_name,
                                a.fullname
                            }).ToList(); ;
                cboAcc.Items.Clear();
                cboAcc.DataSource = accs;
                cboAcc.DataBind();
                if (accs.Count() > 0)
                {
                    cboAcc.SelectedValue = id.ToString();
                }
            }
            catch (Exception ex) { }
        }

        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {

                if (e.Item is GridEditableItem && e.Item.IsInEditMode == true)
                {
                    var item = e.Item as GridEditableItem;
                    int? id = null;
                    try
                    {
                        var val = item.OwnerTableView.DataKeyValues[item.ItemIndex]["assetCategoryID"];
                        if (val != null) { id = int.Parse(val.ToString()); }
                    }
                    catch (Exception) { }
                    le = new coreLoansEntities();
                    var lt = le.assetCategories.FirstOrDefault(p => p.assetCategoryID == id);
                    var rcb = item["depreciationAccountID"].Controls[1] as RadComboBox;
                    if (rcb != null)
                    {
                        if (lt != null && Session[rcb.ValidationGroup] == null)
                        {
                            Session[rcb.ValidationGroup] = lt.depreciationAccountID;
                        }
                        PopulateAccounts(rcb);
                        rcb.ValidationGroup = "depreciationAccountID";
                        rcb.SelectedIndexChanged += rcb_SelectedIndexChanged;
                    }
                    rcb = item["accumulatedDepreciationAccountID"].Controls[1] as RadComboBox;
                    if (rcb != null)
                    {
                        if (lt != null && Session[rcb.ValidationGroup] == null)
                        {
                            Session[rcb.ValidationGroup] = lt.accumulatedDepreciationAccountID;
                        }
                        PopulateAccounts(rcb);
                        rcb.ValidationGroup = "accumulatedDepreciationAccountID";
                        rcb.SelectedIndexChanged += rcb_SelectedIndexChanged;
                    }
                    rcb = item["depreciationMethod"].Controls[1] as RadComboBox;
                    if (rcb != null)
                    { 
                        rcb.ValidationGroup = "depreciationMethod";
                        rcb.SelectedIndexChanged += rcb_SelectedIndexChanged;
                    }
                }
            }
            catch (Exception) { }
        }

        protected void cboGLAcc_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                var cbo = sender as RadComboBox;
               if (e.Text.Trim().Length > 2 && cbo != null)
                {
                    using (core_dbEntities ent = new core_dbEntities())
                    {
                        var accs = (from a in ent.accts
                                    from c in ent.currencies
                                    where (a.acc_name.Contains(e.Text) || a.acct_heads.acct_cats.cat_name.Contains(e.Text) || a.acct_heads.head_name.Contains(e.Text)
                                        )
                                        && (a.currencies.currency_id == c.currency_id)
                                    select new
                                    {
                                        a.acct_id,
                                        a.acc_num,
                                        a.acc_name,
                                        major_name = c.major_name,
                                        fullname = a.acc_num + " | " + a.acc_name
                                    }).ToList();
                        cbo.DataSource = accs;
                        cbo.DataBind();
                        cbo.DataTextField = "fullname";
                        cbo.DataValueField = "acct_id";
                    }
                }
            }
            catch (Exception ex) { }
        } 

        public void rcb_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var rcb = sender as RadComboBox;
            if (rcb != null)
            {
                Session[rcb.ValidationGroup] = e.Value;
            }
        }

        protected void cboDepMeth_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var rcb = sender as RadComboBox;
            if (rcb != null)
            {
                Session[rcb.ValidationGroup] = e.Value;
            }
        }

    }
}
