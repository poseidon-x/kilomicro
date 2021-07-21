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
    public partial class banks : corePage
    {
        public override string URL
        {
            get { return "~/gl/accounts/banks.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "bank_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.banks b = new coreLogic.banks();
                    b.bank_name = newVals["bank_name"].ToString();
                    b.commission_rate = double.Parse(newVals["commission_rate"].ToString());
                    b.full_name = newVals["full_name"].ToString();
                    b.creation_date = DateTime.Now;
                    b.creator = User.Identity.Name;
                    if (newVals["institution_type"] != null)
                    {
                        b.institution_type = newVals["institution_type"].ToString();
                    }
                    ent.banks.Add(b);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "branch_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var bank_id = (int)Session["bank_id"];
                    coreLogic.banks r = ent.banks.First<coreLogic.banks>(p => p.bank_id == bank_id);
                    bank_branches b = new bank_branches();
                    b.branch_name = newVals["branch_name"].ToString();
                    b.branch_code = newVals["branch_name"].ToString();
                    b.banks = r;
                    var location_id = int.Parse(Session["location_id"].ToString());
                    b.locations = ent.locations.First<locations>(p => p.location_id == location_id);
                    b.creation_date = DateTime.Now;
                    b.creator = User.Identity.Name;
                    ent.bank_branches.Add(b);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                    Session["location_id"] = null;
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "bank_acct_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var branch_id = (int)Session["branch_id"];
                    bank_branches d = ent.bank_branches.First<bank_branches>(p => p.branch_id == branch_id);
                    bank_accts ba = new bank_accts();
                    ba.bank_acct_num = newVals["bank_acct_num"].ToString();
                    ba.bank_acct_desc = newVals["bank_acct_desc"].ToString();
                    ba.bank_branches = d;
                    var gl_acct_id = int.Parse(Session["gl_acct_id"].ToString());
                    ba.accts = ent.accts.First<accts>(p => p.acct_id == gl_acct_id);
                    ba.creation_date = DateTime.Now;
                    ba.creator = User.Identity.Name;
                    ent.bank_accts.Add(ba);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                    Session["gl_acct_id"] = null;
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "bank_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var bank_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["bank_id"];
                    var c = (from p in ent.banks where p.bank_id == bank_id select p).First<coreLogic.banks>();
                    c.bank_name = newVals["bank_name"].ToString();
                    c.full_name = newVals["full_name"].ToString();
                    c.commission_rate = double.Parse(newVals["commission_rate"].ToString());
                    if (newVals["institution_type"] != null)
                    {
                        c.institution_type = newVals["institution_type"].ToString();
                    }
                    c.modification_date = DateTime.Now;
                    c.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "branch_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var branch_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["branch_id"];
                    var c = (from p in ent.bank_branches where p.branch_id == branch_id select p).First<bank_branches>();
                    c.branch_name = newVals["branch_name"].ToString();
                    var location_id = int.Parse(Session["location_id"].ToString());
                    c.locations = ent.locations.First<locations>(p => p.location_id == location_id);
                    c.modification_date = DateTime.Now;
                    c.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                    Session["location_id"] = null;
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "bank_acct_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var bank_acct_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["bank_acct_id"];
                    var c = (from p in ent.bank_accts where p.bank_acct_id == bank_acct_id select p).First<bank_accts>();
                    c.bank_acct_desc = newVals["bank_acct_desc"].ToString();
                    c.bank_acct_num = newVals["bank_acct_num"].ToString();
                    var gl_acct_id = int.Parse(Session["gl_acct_id"].ToString());
                    c.accts = ent.accts.First<accts>(p => p.acct_id == gl_acct_id);
                    c.modification_date = DateTime.Now;
                    c.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                    Session["gl_acct_id"] = null;
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "bank_id")
                {
                    e.Canceled = true; 
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["bank_name"] = string.Empty;
                    newVals["commission_rate"] = 0.0000; 
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    newVals["institution_type"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "branch_id")
                {
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var bank_id = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["bank_id"];
                    Session["bank_id"] = bank_id;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["branch_name"] = string.Empty;
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "bank_acct_id")
                {
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var branch_id = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["branch_id"];
                    Session["branch_id"] = branch_id;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["bank_acct_num"] = string.Empty;
                    newVals["bank_acct_desc"] = string.Empty;  
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
            else if (e.CommandName == "CancelEdit")
            {
                //closes the edit form
                RadGrid1.MasterTableView.ClearEditItems();
                Session["gl_acct_id"] = null;
                Session["location_id"] = null;
            }
            else if (e.CommandName == "Edit")
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "bank_acct_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    var bank_acct_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("bank_acct_id").ToString());
                    Session["gl_acct_id"] = (from c in ent.bank_accts
                                                from u in ent.accts
                                                where c.accts.acct_id == u.acct_id
                                                    && c.bank_acct_id == bank_acct_id
                                                select new { u.acct_id }).FirstOrDefault().acct_id;
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "branch_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    var branch_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("branch_id").ToString());
                    Session["location_id"] = (from c in ent.bank_branches
                                                 from u in ent.locations
                                                where c.locations.location_id == u.location_id
                                                     && c.branch_id == branch_id
                                                select new { u.location_id }).FirstOrDefault().location_id;
                }
            }   
        }


        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true && e.Item.OwnerTableView.DataKeyNames[0] == "branch_id")
            {
                var item = e.Item as GridEditableItem;
                var rcb = item.Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateLocations(rcb); 
                    rcb.DataBind();
                }
            }
            else if (e.Item is GridEditableItem && e.Item.IsInEditMode == true && e.Item.OwnerTableView.DataKeyNames[0] == "bank_acct_id")
            {
                var item = e.Item as GridEditableItem;
                var rcb = item.FindControl("cboGLAcc") as RadComboBox;
                if (rcb != null)
                {
                    PopulateAccounts(rcb);
                    rcb.DataBind();
                }
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
            cboAcc.Items.Clear(); 
            foreach (var cur in accs)
            {
                RadComboBoxItem item = new RadComboBoxItem();

                item.Text = cur.acc_num + " - " + cur.acc_name;
                item.Value = cur.acct_id.ToString();

                item.Attributes.Add("acc_num", cur.acc_num);
                item.Attributes.Add("acc_name", cur.acc_name);
                item.Attributes.Add("acc_cur", cur.major_name);

                cboAcc.Items.Add(item);

            }
            if (Session["gl_acct_id"] != null)
            { 
                cboAcc.SelectedValue = Session["gl_acct_id"].ToString();
            }
            else cboAcc.Text = "";
        }

        protected void PopulateLocations(Telerik.Web.UI.RadComboBox cboLoc)
        { 
            var accs = (from a in ent.locations
                        from c in ent.cities
                        where a.cities.city_id == c.city_id
                        select new
                        {
                            a.location_id,
                            a.location_name, 
                            c.city_name
                        });
            cboLoc.Items.Clear();
            foreach (var cur in accs)
            {
                RadComboBoxItem item = new RadComboBoxItem();

                item.Text = cur.location_name;
                item.Value = cur.location_id.ToString();

                item.Attributes.Add("city_name", cur.city_name); 

                cboLoc.Items.Add(item);

            }
            if (Session["location_id"] != null)
            {
                cboLoc.Text = " ";
                cboLoc.SelectedValue = Session["location_id"].ToString();
            }
            else cboLoc.Text = " "; 
        }

        public string LocationName(int branch_id)
        {
            return (from c in ent.bank_branches
                    from m in ent.locations
                    where c.locations.location_id == m.location_id
                        && c.branch_id == branch_id
                    select new
                    {
                        m.location_name 
                    }).First().location_name;
        }


        public string AccountName(int bank_acct_id)
        {
            var item= (from c in ent.bank_accts
                    from m in ent.accts
                    where c.accts.acct_id == m.acct_id
                        && c.bank_acct_id == bank_acct_id
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
                var cbo=((RadComboBox)sender);
                if(cbo.SelectedValue!="")
                Session["gl_acct_id"] = cbo.SelectedValue;
            }
            catch (Exception ex) { }
        }

        protected void cboGLAcc_Load(object sender, EventArgs e)
        {
            try
            {
                var cbo = ((RadComboBox)sender);
                Session["gl_acct_id"] = cbo.SelectedValue;
                    PopulateAccounts(cbo); 
            }
            catch (Exception ex) { }
        }

        protected void cboLoc_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                PopulateLocations((RadComboBox)sender);
            }
            catch (Exception ex) { }
        }

        protected void cboLoc_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Session["location_id"] = ((RadComboBox)sender).SelectedValue;
                PopulateLocations((RadComboBox)sender); 
            }
            catch (Exception ex) { }
        }

        protected void cboLoc_Load(object sender, EventArgs e)
        {
            try
            {
                var cboLoc = ((RadComboBox)sender);
                if (!string.IsNullOrEmpty(cboLoc.SelectedValue))
                {
                    Session["location_id"] = cboLoc.SelectedValue;
                }
                PopulateLocations((RadComboBox)sender);
            }
            catch (Exception ex) { }
        }

    }
}
