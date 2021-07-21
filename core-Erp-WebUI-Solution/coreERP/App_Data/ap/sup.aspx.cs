using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;
using System.Collections;

namespace coreERP.ap
{
    public partial class sup : System.Web.UI.Page
    {
        private RadComboBox _cboCur, _cboType, _cboVATGL, _cboARGL, _cboAddrType, _cboCity, _cboPhonType;
        
        core_dbEntities ent;
        public sup()
        { 
        }
 
        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new core_dbEntities(); 
        }
         
        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.sups c = new coreLogic.sups();
                    var sup_type_id = int.Parse(Session["sup_type_id"].ToString());
                    sup_types t = ent.sup_types.First<sup_types>(p => p.sup_type_id == sup_type_id);
                    c.sup_types = t;
                    var currency_id = int.Parse(Session["currency_id"].ToString());
                    coreLogic.currencies u = ent.currencies.First<coreLogic.currencies>(p => p.currency_id == currency_id);
                    c.currencies = u;
                    if (Session["ap_acct_id"] != null)
                    {
                        var ap_acct_id = int.Parse(Session["ap_acct_id"].ToString());
                        coreLogic.accts acc = ent.accts.First<coreLogic.accts>(p => p.acct_id == ap_acct_id);
                        c.ap_accts = acc;
                    }
                    if (Session["vat_acct_id"] != null)
                    {
                        var vat_acct_id = int.Parse(Session["vat_acct_id"].ToString());
                        coreLogic.accts acc = ent.accts.First<coreLogic.accts>(p => p.acct_id == vat_acct_id);
                        c.vat_accts = acc;
                    }
                    c.sup_name = newVals["sup_name"].ToString();
                    c.acc_num = newVals["acc_num"].ToString();
                    if (newVals["debit_terms"].ToString() != "") c.debit_terms = newVals["debit_terms"].ToString();
                    if (newVals["contact_person"].ToString() != "") c.contact_person = newVals["contact_person"].ToString();
                    c.creation_date = DateTime.Now;
                    c.creator = User.Identity.Name;
                    ent.sups.Add(c);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                    Session["ap_acct_id"] = null;
                    Session["vat_acct_id"] = null;
                    Session["currency_id"] = null;
                    Session["sup_type_id"] = null;
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_addr_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var sup_id = (int)Session["sup_id"];
                    coreLogic.sups r = ent.sups.First<coreLogic.sups>(p => p.sup_id == sup_id);
                    sup_addr b = new sup_addr();
                    b.addr_line_1 = newVals["addr_line_1"].ToString();
                    if (newVals["addr_line_2"] != null && newVals["addr_line_2"].ToString().Length >0)
                        b.addr_line_2 = newVals["addr_line_2"].ToString();
                    b.is_default = bool.Parse(newVals["is_default"].ToString());
                    b.sup_id = sup_id;
                    var city_id = int.Parse(Session["city_id"].ToString());
                    b.city_id = city_id;
                    var addr_type = Session["addr_type"].ToString();
                    b.addr_types = ent.addr_types.First<addr_types>(p => p.addr_type == addr_type);
                    b.creation_date = DateTime.Now;
                    b.creator = User.Identity.Name;
                    ent.sup_addr.Add(b);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind(); 
                    Session["addr_type"] = null;
                    Session["city_id"] = null;
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_phon_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var sup_id = (int)Session["sup_id"];
                    coreLogic.sups r = ent.sups.First<coreLogic.sups>(p => p.sup_id == sup_id);
                    sup_phons b = new sup_phons();
                    b.phon_num = newVals["phon_num"].ToString(); 
                    b.is_default = bool.Parse(newVals["is_default"].ToString());
                    b.sup_id = sup_id; 
                    var phon_type = Session["phon_type"].ToString();
                    b.phon_types = ent.phon_types.First<phon_types>(p => p.phon_type == phon_type);
                    b.creation_date = DateTime.Now;
                    b.creator = User.Identity.Name;
                    ent.sup_phons.Add(b);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind(); 
                    Session["phon_type"] = null; 
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid2_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                var c = (from o in ent.addr_types where o.addr_type == "_" select o).Count();
                if (c == 1)
                {
                    var dum = (from o in ent.addr_types
                               where o.addr_type == "_"
                               select o).FirstOrDefault();
                    ent.addr_types.Remove(dum);
                    ent.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid3_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                var c = (from o in ent.phon_types where o.phon_type == "_" select o).Count();
                if (c == 1)
                {
                    var dum = (from o in ent.phon_types
                               where o.phon_type == "_"
                               select o).FirstOrDefault();
                    ent.phon_types.Remove(dum);
                    ent.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid4_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                var c = (from o in ent.sup_types where o.sup_type_name == "________" select o).Count();
                if (c == 1)
                {
                    var dum = (from o in ent.sup_types
                               where o.sup_type_name == "________"
                               select o).FirstOrDefault();
                    ent.sup_types.Remove(dum);
                    ent.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var sup_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["sup_id"];
                    var c = (from p in ent.sups where p.sup_id ==sup_id select p).First<sups>();

                    var sup_type_id = (int)Session["sup_type_id"];
                    sup_types t = ent.sup_types.First<sup_types>(p => p.sup_type_id == sup_type_id);
                    c.sup_types = t;
                    var currency_id = (int)Session["currency_id"];
                    coreLogic.currencies u = ent.currencies.First<coreLogic.currencies>(p => p.currency_id == currency_id);
                    c.currencies = u;
                    if (Session["ap_acct_id"] != null)
                    {
                        var ap_acct_id = (int)Session["ap_acct_id"];
                        coreLogic.accts acc = ent.accts.First<coreLogic.accts>(p => p.acct_id == ap_acct_id);
                        c.ap_accts = acc;
                    }
                    if (Session["vat_acct_id"] != null)
                    {
                        var vat_acct_id = (int)Session["vat_acct_id"];
                        coreLogic.accts acc = ent.accts.First<coreLogic.accts>(p => p.acct_id == vat_acct_id);
                        c.vat_accts = acc;
                    }
                    c.sup_name = newVals["sup_name"].ToString();
                    c.acc_num = newVals["acc_num"].ToString();
                    if (newVals["debit_terms"].ToString() != "") c.debit_terms = newVals["debit_terms"].ToString();
                    if (newVals["contact_person"].ToString() != "") c.contact_person = newVals["contact_person"].ToString();
                    c.modification_date = DateTime.Now;
                    c.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                    Session["ap_acct_id"] = null;
                    Session["vat_acct_id"] = null;
                    Session["currency_id"] = null;
                    Session["sup_type_id"] = null; 
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_addr_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var sup_addr_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["sup_addr_id"];
                    var b = (from p in ent.sup_addr where p.sup_addr_id == sup_addr_id select p).First<sup_addr>();
                    b.addr_line_1 = newVals["addr_line_1"].ToString();
                    if (newVals["addr_line_2"] != null && newVals["addr_line_2"].ToString().Length > 0)
                        b.addr_line_2 = newVals["addr_line_2"].ToString();
                    b.is_default = bool.Parse(newVals["is_default"].ToString()); 
                    var city_id = int.Parse(Session["city_id"].ToString());
                    b.city_id = city_id;
                    var addr_type = Session["addr_type"].ToString();
                    b.addr_types = ent.addr_types.First<addr_types>(p => p.addr_type == addr_type);
                    b.modification_date = DateTime.Now;
                    b.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                    Session["addr_type"] = null;
                    Session["city_id"] = null;
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_phon_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var sup_phon_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["sup_phon_id"];
                    var b = (from p in ent.sup_phons where p.sup_phon_id == sup_phon_id select p).First<sup_phons>();
                    b.phon_num = newVals["phon_num"].ToString(); 
                    b.is_default = bool.Parse(newVals["is_default"].ToString()); 
                    var phon_type = Session["phon_type"].ToString();
                    b.phon_types = ent.phon_types.First<phon_types>(p => p.phon_type == phon_type);
                    b.modification_date = DateTime.Now;
                    b.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                    Session["phon_type"] = null; 
                }
            }
            catch (Exception ex) { }
        }
 
        protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_id")
                {
                    e.Canceled = true; 
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["sup_name"] = string.Empty;
                    newVals["acc_num"] = string.Empty;
                    newVals["contact_person"] = string.Empty;
                    newVals["debit_terms"] = string.Empty; 
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_addr_id")
                {
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var sup_id = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["sup_id"];
                    Session["sup_id"] = sup_id;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["addr_line_1"] = string.Empty;
                    newVals["addr_line_2"] = string.Empty;
                    newVals["is_default"] = false;
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_phon_id")
                {
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var sup_id = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["sup_id"];
                    Session["sup_id"] = sup_id;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["phon_num"] = string.Empty; 
                    newVals["is_default"] = false;
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                } 
            }
            else if (e.CommandName == "CancelEdit")
            {
                //closes the edit form
                RadGrid1.MasterTableView.ClearEditItems();
                Session["ap_acct_id"] = null;
                Session["vat_acct_id"] = null;
                Session["currency_id"] = null;
                Session["sup_type_id"] = null;
                Session["addr_type"] = null;
                Session["phon_type"] = null;
                Session["city_id"] = null; 
            }
            else if (e.CommandName == "Edit")
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    var sup_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("sup_id").ToString());
                    try
                    {
                        Session["ap_acct_id"] = (from c in ent.sups
                                                   from u in ent.accts
                                                   where c.ap_accts.acct_id == u.acct_id
                                                       && c.sup_id == sup_id
                                                   select new { u.acct_id }).FirstOrDefault().acct_id;
                    }
                    catch (Exception) { }
                    try
                    {
                        Session["vat_acct_id"] = (from c in ent.sups
                                                   from u in ent.accts
                                                   where c.vat_accts.acct_id == u.acct_id
                                                       && c.sup_id == sup_id
                                                   select new { u.acct_id }).FirstOrDefault().acct_id;
                    }
                    catch (Exception) { }
                    try
                    {
                        Session["currency_id"] = (from c in ent.sups
                                                    from u in ent.currencies
                                                    where c.currencies.currency_id == u.currency_id
                                                        && c.sup_id == sup_id
                                                    select new { u.currency_id }).FirstOrDefault().currency_id;
                    }
                    catch (Exception) { }
                    try
                    {
                        Session["sup_type_id"] = (from c in ent.sups
                                                     from u in ent.sup_types
                                                     where c.sup_types.sup_type_id == u.sup_type_id
                                                        && c.sup_id == sup_id
                                                     select new { u.sup_type_id }).FirstOrDefault().sup_type_id;
                    }
                    catch (Exception) { }
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_addr_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    var sup_addr_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("sup_addr_id").ToString());
                    try
                    {
                        Session["addr_type"] = (from c in ent.sup_addr
                                                  from u in ent.addr_types
                                                  where c.addr_types.addr_type == u.addr_type
                                                      && c.sup_addr_id == sup_addr_id
                                                  select new { u.addr_type }).FirstOrDefault().addr_type;
                    }
                    catch (Exception) { } try
                    {
                        Session["city_id"] = (from c in ent.sup_addr
                                                from u in ent.cities
                                                where c.city_id == u.city_id
                                                      && c.sup_addr_id == sup_addr_id
                                                select new { u.city_id }).FirstOrDefault().city_id;
                    }
                    catch (Exception) { }
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "sup_phon_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    var sup_phon_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("sup_phon_id").ToString());
                    try
                    {
                        Session["phon_type"] = (from c in ent.sup_phons
                                                  from u in ent.phon_types
                                                  where c.phon_types.phon_type == u.phon_type
                                                      && c.sup_phon_id == sup_phon_id
                                                  select new { u.phon_type }).FirstOrDefault().phon_type;
                    }
                    catch (Exception) { }
                }
            }
        }

        protected void RadGrid2_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            { 
                var c = (from o in ent.addr_types select o).Count();
                if (c == 0)
                {
                    var ob = new addr_types();
                    ob.creation_date = DateTime.Now;
                    ob.creator = User.Identity.Name;
                    ob.addr_type = "_";
                    ob.addr_type_name = "________";
                    ent.addr_types.Add(ob);
                    ent.SaveChanges();
                }
            }
            else if (e.CommandName == "CancelEdit" || e.CommandName == "Cancel")
            {
                try
                {
                    var c = (from o in ent.addr_types where o.addr_type == "_" select o).Count();
                    if (c == 1)
                    {
                        var dum = (from o in ent.addr_types
                                   where o.addr_type == "_"
                                   select o).FirstOrDefault();
                        ent.addr_types.Remove(dum);
                        ent.SaveChanges();
                    }
                }
                catch (Exception ex) { }
            }
        }

        protected void RadGrid3_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            { 
                var c = (from o in ent.phon_types select o).Count();
                if (c == 0)
                {
                    var ob = new phon_types();
                    ob.creation_date = DateTime.Now;
                    ob.creator = User.Identity.Name;
                    ob.phon_type = "_";
                    ob.phon_type_name = "________";
                    ent.phon_types.Add(ob);
                    ent.SaveChanges();
                }
            }
            else if (e.CommandName == "CancelEdit" || e.CommandName == "Cancel")
            {
                try
                {
                    var c = (from o in ent.phon_types where o.phon_type == "_" select o).Count();
                    if (c == 1)
                    {
                        var dum = (from o in ent.phon_types
                                   where o.phon_type == "_"
                                   select o).FirstOrDefault();
                        ent.phon_types.Remove(dum);
                        ent.SaveChanges();
                    }
                }
                catch (Exception ex) { }
            }
        }

        protected void RadGrid4_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                var c = (from o in ent.sup_types select o).Count();
                if (c == 0)
                {
                    var ob = new sup_types();
                    ob.creation_date = DateTime.Now;
                    ob.creator = User.Identity.Name;
                    ob.sup_type_name = "________";
                    ent.sup_types.Add(ob);
                    ent.SaveChanges();
                }
            }
            else if (e.CommandName == "CancelEdit" || e.CommandName == "Cancel")
            {
                try
                {
                    var c = (from o in ent.sup_types where o.sup_type_name == "________" select o).Count();
                    if (c == 1)
                    {
                        var dum = (from o in ent.sup_types
                                   where o.sup_type_name == "________"
                                   select o).FirstOrDefault();
                        ent.sup_types.Remove(dum);
                        ent.SaveChanges();
                    }
                }
                catch (Exception ex) { }
            }
        }

        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc, byte which)
        {
            if (cboAcc.Items.Count == 0)
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
                            });
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
            }
            if (which == 1)
            {
                if (Session["ap_acct_id"] != null)
                {
                    cboAcc.Text = "";
                    cboAcc.SelectedValue = Session["ap_acct_id"].ToString();
                }
                else cboAcc.Text = " ";
            }
            else if (which == 2)
            {
                if (Session["vat_acct_id"] != null)
                {
                    cboAcc.Text = "";
                    cboAcc.SelectedValue = Session["vat_acct_id"].ToString();
                }
                else cboAcc.Text = " ";
            }
        }
         
        protected void PopulateCurrencies(Telerik.Web.UI.RadComboBox cboCur)
        {
            if (cboCur.Items.Count==0)
            {
                cboCur.Items.Clear();
                foreach (var cur in ent.currencies.ToList<currencies>())
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = cur.major_name + " (" + cur.major_symbol + ")";
                    item.Value = cur.currency_id.ToString();

                    item.Attributes.Add("current_buy_rate", cur.current_buy_rate.ToString());
                    item.Attributes.Add("major_symbol", cur.major_symbol.ToString());

                    cboCur.Items.Add(item);

                }
            }
            if (Session["currency_id"] != null)
            {
                cboCur.Text = "";
                cboCur.SelectedValue = Session["currency_id"].ToString();
            }
            else cboCur.Text = " "; 
        }

        protected void PopulateSupplierTypes(Telerik.Web.UI.RadComboBox cboType)
        {
            if (cboType.Items.Count == 0)
            {
                cboType.Items.Clear();
                foreach (var cur in ent.sup_types.ToList<sup_types>())
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = cur.sup_type_name;
                    item.Value = cur.sup_type_id.ToString();

                    cboType.Items.Add(item);

                }
            }
            if (Session["sup_type_id"] != null)
            {
                cboType.Text = "";
                cboType.SelectedValue = Session["sup_type_id"].ToString();
            }
            else cboType.Text = " ";
        }

        protected void PopulateCities(Telerik.Web.UI.RadComboBox cboCity)
        {
            if (cboCity.Items.Count == 0)
            {
                cboCity.Items.Clear();
                var accs = (from a in ent.cities
                            from c in ent.districts
                            from d in ent.regions
                            from f in ent.countries
                            where a.districts.district_id == c.district_id
                                && c.regions.region_id == d.region_id
                                && d.countries.country_id == f.country_id
                            select new
                            {
                                a.city_name,
                                f.country_name,
                                a.city_id
                            });
                foreach (var city in accs)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = city.city_name;
                    item.Value = city.city_id.ToString();

                    item.Attributes.Add("city", city.city_name);
                    item.Attributes.Add("country", city.country_name); 

                    cboCity.Items.Add(item);

                }
            }
            if (Session["city_id"] != null)
            {
                cboCity.Text = "";
                cboCity.SelectedValue = Session["city_id"].ToString();
            }
            else cboCity.Text = " ";
        }

        protected void PopulateAddressTypes(Telerik.Web.UI.RadComboBox cboType)
        {
            if (cboType.Items.Count == 0)
            {
                cboType.Items.Clear(); 
                foreach (var type in ent.addr_types)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = type.addr_type_name;
                    item.Value =type.addr_type;

                    item.Attributes.Add("code", type.addr_type);
                    item.Attributes.Add("name", type.addr_type_name);

                    cboType.Items.Add(item);

                }
            }
            if (Session["addr_type"] != null)
            {
                cboType.Text = "";
                cboType.SelectedValue = Session["addr_type"].ToString();
            }
            else cboType.Text = " ";
        }
        protected void PopulatePhoneTypes(Telerik.Web.UI.RadComboBox cboType)
        {
            if (cboType.Items.Count == 0)
            {
                cboType.Items.Clear();
                foreach (var type in ent.phon_types)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = type.phon_type_name;
                    item.Value = type.phon_type;

                    item.Attributes.Add("code", type.phon_type);
                    item.Attributes.Add("name", type.phon_type_name);

                    cboType.Items.Add(item);

                }
            }
            if (Session["phon_type"] != null)
            {
                cboType.Text = "";
                cboType.SelectedValue = Session["phon_type"].ToString();
            }
            else cboType.Text = " ";
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

        public string CityName(int sup_addr_id)
        {
            try
            {
                return (from c in ent.sup_addr
                        from m in ent.cities
                        where c.sup_addr_id == sup_addr_id
                            && c.city_id == m.city_id
                        select new
                        {
                            m.city_name
                        }).First().city_name;
            }
            catch (Exception x) { return ""; }
        }

        public string AddressTypeName(int sup_addr_id)
        {
            try
            {
                return (from c in ent.sup_addr
                        from m in ent.addr_types
                        where c.sup_addr_id == sup_addr_id
                            && c.addr_types.addr_type == m.addr_type
                        select new
                        {
                            m.addr_type_name
                        }).First().addr_type_name;
            }
            catch (Exception x) { return ""; }
        }

        public string PhoneTypeName(int sup_phon_id)
        {
            try
            {
                return (from c in ent.sup_phons
                        from m in ent.phon_types
                        where c.sup_phon_id == sup_phon_id
                            && c.phon_types.phon_type == m.phon_type
                        select new
                        {
                            m.phon_type_name
                        }).First().phon_type_name;
            }
            catch (Exception x) { return ""; }
        }
         
        public string AccountName(int sup_id, byte type)
        {
            try{
                var item = (from c in ent.sups
                    from m in ent.accts
                    where ((type==1 && c.ap_accts.acct_id == m.acct_id) ||(type==2 && c.vat_accts.acct_id == m.acct_id))
                        && c.sup_id == sup_id
                    select new
                    {
                        m.acc_num,
                        m.acc_name
                    }).First();
            return item.acc_num + " - " + item.acc_name;
            }
            catch (Exception x) { return ""; }
        }
         
        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = false;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            RadGrid1.ExportSettings.HideStructureColumns = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_suppliers";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = false;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.HideStructureColumns = true;
            RadGrid1.ExportSettings.OpenInNewWindow = false;
            this.RadGrid1.ExportSettings.FileName = "coreERP_suppliers";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            RadGrid1.ExportSettings.HideStructureColumns = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_suppliers";
            this.RadGrid1.ExportSettings.Pdf.Title = "Suppliers";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.ExportSettings.HideStructureColumns = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        protected void cboAddrType_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                _cboAddrType = (RadComboBox)sender;
                PopulateAddressTypes(_cboAddrType);
            }
            catch (Exception ex) { }
        }

        protected void cboAddrType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                _cboAddrType = (RadComboBox)sender;
                Session["addr_type"] = (_cboAddrType).SelectedValue;
                PopulateAddressTypes(_cboAddrType);
            }
            catch (Exception ex) { }
        }

        protected void cboAddrType_Load(object sender, EventArgs e)
        {
            try
            {
                _cboAddrType = (RadComboBox)sender;
                PopulateAddressTypes(_cboAddrType);
            }
            catch (Exception ex) { }
        }

        protected void cboCity_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                _cboCity = (RadComboBox)sender;
                PopulateCities(_cboCity);
            }
            catch (Exception ex) { }
        }

        protected void cboCity_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                _cboCity = (RadComboBox)sender;
                Session["city_id"] = (_cboCity).SelectedValue;
                PopulateCities(_cboCity);
            }
            catch (Exception ex) { }
        }

        protected void cboCity_Load(object sender, EventArgs e)
        {
            try
            {
                _cboCity = (RadComboBox)sender;
                PopulateCities(_cboCity);
            }
            catch (Exception ex) { }
        }

        protected void cboPhonType_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                _cboPhonType = (RadComboBox)sender;
                PopulatePhoneTypes(_cboPhonType);
            }
            catch (Exception ex) { }
        }

        protected void cboPhonType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                _cboPhonType = (RadComboBox)sender;
                Session["phon_type"] = (_cboPhonType).SelectedValue;
                PopulatePhoneTypes(_cboPhonType);
            }
            catch (Exception ex) { }
        }

        protected void cboPhonType_Load(object sender, EventArgs e)
        {
            try
            {
                _cboPhonType = (RadComboBox)sender;
                PopulatePhoneTypes(_cboPhonType);
            }
            catch (Exception ex) { }
        }

        protected void cboARAcct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                _cboARGL = (RadComboBox)sender;
                PopulateAccounts(_cboARGL, 1);
            }
            catch (Exception ex) { }
        }

        protected void cboARAcct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                _cboARGL = (RadComboBox)sender;
                Session["ap_acct_id"] = (_cboARGL).SelectedValue;
                PopulateAccounts(_cboARGL, 1);
            }
            catch (Exception ex) { }
        }

        protected void cboARAcct_Load(object sender, EventArgs e)
        {
            try
            { 
                _cboARGL = (RadComboBox)sender;
                PopulateAccounts(_cboARGL, 1);
            }
            catch (Exception ex) { }
        }

        protected void cboVATAcct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                _cboVATGL = (RadComboBox)sender;
                PopulateAccounts(_cboVATGL, 2);
            }
            catch (Exception ex) { }
        }

        protected void cboVATAcct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                _cboVATGL = (RadComboBox)sender;
                Session["vat_acct_id"] = (_cboVATGL).SelectedValue;
                PopulateAccounts(_cboVATGL, 2);
            }
            catch (Exception ex) { }
        }

        protected void cboVATAcct_Load(object sender, EventArgs e)
        {
            try
            {
                _cboVATGL = (RadComboBox)sender; 
                PopulateAccounts(_cboVATGL, 2);
            }
            catch (Exception ex) { }
        }

        protected void cboEmp_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                //PopulateLocations((RadComboBox)sender);
            }
            catch (Exception ex) { }
        }

        protected void cboEmp_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Session["rep_emp_id"] = ((RadComboBox)sender).SelectedValue;
            }
            catch (Exception ex) { }
        }

        protected void cboEmp_Load(object sender, EventArgs e)
        {
            try
            {
                //PopulateLocations((RadComboBox)sender);
            }
            catch (Exception ex) { }
        }

        protected void cboCur_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                _cboCur = (RadComboBox)sender;
                PopulateCurrencies(_cboCur);
            }
            catch (Exception ex) { }
        }

        protected void cboCur_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                _cboCur = (RadComboBox)sender;
                Session["currency_id"] = (_cboCur).SelectedValue;
                PopulateCurrencies(_cboCur);
            }
            catch (Exception ex) { }
        }

        protected void cboCur_Load(object sender, EventArgs e)
        {
            try
            {
                _cboCur = (RadComboBox)sender;
                PopulateCurrencies(_cboCur);
            }
            catch (Exception ex) { }
        }

        protected void cboType_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                _cboType = (RadComboBox)sender;
                PopulateSupplierTypes(_cboType);
            }
            catch (Exception ex) { }
        }

        protected void cboType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                _cboType = (RadComboBox)sender;
                Session["sup_type_id"] = (_cboType).SelectedValue;
                PopulateSupplierTypes(_cboType);
            }
            catch (Exception ex) { }
        }

        protected void cboType_Load(object sender, EventArgs e)
        {
            try
            {
                _cboType = (RadComboBox)sender;
                PopulateSupplierTypes(_cboType);
            }
            catch (Exception ex) { }
        }

        public string CurrencyName(int sup_id)
        {
            try
            {
                return (from c in ent.sups
                        from m in ent.currencies
                        where c.currencies.currency_id == m.currency_id
                            && c.sup_id == sup_id
                        select new
                        {
                            m.major_name,
                            m.major_symbol
                        }).First().major_name;
            }
            catch (Exception x) { return ""; }
        }

        public string SupplierTypeName(int sup_id)
        {
            try
            {
                return (from c in ent.sups
                        from m in ent.sup_types
                        where c.sup_types.sup_type_id == m.sup_type_id
                            && c.sup_id == sup_id
                        select new
                        {
                            m.sup_type_name 
                        }).First().sup_type_name;
            }
            catch (Exception x) { return ""; }
        }

    }
}
