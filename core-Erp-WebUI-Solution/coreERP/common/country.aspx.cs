using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;
using System.Collections;

namespace coreERP.common
{
    public partial class country : corePage
    {
        public override string URL
        {
            get { return "~/common/country.aspx"; }
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
                //System.Runtime.
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "region_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var country_id = (int)Session["country_id"];
                    countries c = ent.countries.First<countries>(p => p.country_id == country_id);
                    regions reg = new regions();
                    reg.abbrev = newVals["abbrev"].ToString();
                    reg.region_name = newVals["region_name"].ToString();
                    reg.countries = c;
                    reg.creation_date = DateTime.Now;
                    reg.creator = User.Identity.Name;
                    ent.regions.Add(reg);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "district_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var region_id = (int)Session["region_id"];
                    regions r = ent.regions.First<regions>(p => p.region_id == region_id);
                    districts dist = new districts();
                    dist.district_name = newVals["district_name"].ToString();
                    dist.regions = r;
                    dist.creation_date = DateTime.Now;
                    dist.creator = User.Identity.Name;
                    ent.districts.Add(dist);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "city_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var district_id = (int)Session["district_id"];
                    districts d= ent.districts.First<districts>(p => p.district_id == district_id);
                    cities city = new cities();
                    city.city_name = newVals["city_name"].ToString();
                    city.districts = d;
                    city.creation_date = DateTime.Now;
                    city.creator = User.Identity.Name;
                    ent.cities.Add(city);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "location_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var city_id = (int)Session["city_id"];
                    cities c = ent.cities.First<cities>(p => p.city_id == city_id);
                    locations loc = new locations();
                    loc.location_name = newVals["location_name"].ToString();
                    loc.location_code = newVals["location_code"].ToString();
                    loc.cities = c;
                    loc.creation_date = DateTime.Now;
                    loc.creator = User.Identity.Name;
                    ent.locations.Add(loc);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "country_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var c = new countries();
                    c.abbrev = newVals["abbrev"].ToString();
                    c.country_name = newVals["country_name"].ToString();
                    c.nationality = newVals["nationality"].ToString();
                    c.country_code = newVals["country_code"].ToString();
                    var currency_id = int.Parse(Session["currency_id"].ToString());
                    c.currencies = ent.currencies.First<currencies>(p => p.currency_id == currency_id);
                    c.creation_date = DateTime.Now;
                    c.creator = User.Identity.Name;
                    ent.countries.Add(c);
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }

            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "country_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var country_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["country_id"];
                    var c = (from p in ent.countries where p.country_id == country_id select p).First<countries>(); 
                    c.abbrev = newVals["abbrev"].ToString();
                    c.country_name = newVals["country_name"].ToString();
                    c.nationality=newVals["nationality"].ToString();
                    c.country_code=newVals["country_code"].ToString();
                    var currency_id = int.Parse(Session["currency_id"].ToString());
                    c.currencies = ent.currencies.First<currencies>(p=>p.currency_id==currency_id);
                    c.modification_date = DateTime.Now;
                    c.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "region_id")
                {
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var countryID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["country_id"];
                    Session["country_id"] = countryID;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["region_name"] = string.Empty;
                    newVals["abbrev"] = string.Empty; 
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "district_id")
                {
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var regionID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["region_id"];
                    Session["region_id"] = regionID;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["district_name"] = string.Empty; 
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "city_id")
                {
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var districtID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["district_id"];
                    Session["district_id"] = districtID;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["city_name"] = string.Empty;
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "location_id")
                {
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var cityID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["city_id"];
                    Session["city_id"] = cityID;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["location_name"] = string.Empty;
                    newVals["location_code"] = string.Empty;
                    newVals["creator"] = User.Identity.Name;
                    newVals["creation_date"] = DateTime.Now;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    //newVals["country_id"] = 0;
                    newVals["country_name"] = string.Empty;
                    newVals["nationality"] = string.Empty;
                    newVals["abbrev"] = string.Empty;
                    newVals["country_code"] = string.Empty;
                    newVals["currency_id"] =0;
                    Session["currency_id"] = 0;
                    newVals["creator"] = User.Identity.Name;
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "country_id")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    var country_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("country_id").ToString());
                    Session["currency_id"] = (from c in ent.countries
                                                from u in ent.currencies
                                                where c.currencies.currency_id == u.currency_id
                                                    && c.country_id == country_id
                                                select new { u.currency_id }).FirstOrDefault().currency_id;
                }
            }  
        }

        protected void PopulateCurrency(Telerik.Web.UI.RadComboBox cboCur)
        {
            var selectedValue = Session["currency_id"].ToString();
            foreach (var cur in ent.currencies.ToList<currencies>())
            {
                RadComboBoxItem item = new RadComboBoxItem();

                item.Text = cur.major_name + " (" + cur.major_symbol + ")";
                item.Value = cur.currency_id.ToString();

                item.Attributes.Add("current_buy_rate", cur.current_buy_rate.ToString());
                item.Attributes.Add("major_symbol", cur.major_symbol.ToString());

                cboCur.Items.Add(item);

            }
            cboCur.SelectedValue = selectedValue;
        }

        public string CurrencyName(int country_id)
        {
            return (from c in ent.countries
                    from m in ent.currencies
                    where c.currencies.currency_id == m.currency_id
                        && c.country_id==country_id
                    select new
                    {
                        m.major_name,
                        m.major_symbol
                    }).First().major_name;
        }

        public int CurrencyID(int country_id)
        { 
            var curid= (from c in ent.countries
                    from m in ent.currencies
                    where c.currencies.currency_id == m.currency_id
                        && (country_id == 0 || c.country_id == country_id)
                    select new
                    {
                        m.currency_id
                    }).FirstOrDefault();
            if (curid != null)
            {
                return curid.currency_id;
            }
            else
            {
                return 0;
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = false;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            RadGrid1.ExportSettings.HideStructureColumns = false;
            this.RadGrid1.ExportSettings.FileName = "coreERP_countries";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = false;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.HideStructureColumns = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_countries";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            RadGrid1.ExportSettings.HideStructureColumns = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_countries";
            this.RadGrid1.ExportSettings.Pdf.Title = "Currencies Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            //RadGrid1.ExportSettings.HideStructureColumns = true;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        protected void cboCur_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                PopulateCurrency((RadComboBox)sender);
            }
            catch (Exception ex) { }
        }

        protected void cboCur_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Session["currency_id"] = ((RadComboBox)sender).SelectedValue;
            }
            catch (Exception ex) { }
        }

        protected void cboCur_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateCurrency((RadComboBox)sender);
            }
            catch (Exception ex) { }
        }

    }
}
