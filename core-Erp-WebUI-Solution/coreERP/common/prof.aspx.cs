using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.common
{
    public partial class prof : corePage
    {
        core_dbEntities ent;
        comp_prof profile;
        public override string URL
        {
            get { return "~/common/prof.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                divError.Style["visibility"] = "hidden";
                ent = new core_dbEntities();
                profile = ent.comp_prof.FirstOrDefault();
                if (profile == null)
                {
                    profile = new comp_prof
                    {
                        comp_name="Enter Company Name",
                        addr_line_1 = "Enter Address",
                        vat_flat_rate = 3,
                        vat_rate = 12.5,
                        fmoy = 1,
                        comp_prof_id = 1,
                        employee_rate = 5.5,
                        employer_rate = 12.5,
                        creation_date = DateTime.Now,
                        creator = Context.User.Identity.Name,
                        enf_ou_usg = false,
                        enf_ou_sec = false,
                        nhil_rate = 2.5,
                        petty_cash_ceil = 1000,
                        num_b4_name = true,
                        edit_posted_jnl=true
                    };
                    ent.comp_prof.Add(profile);
                    ent.SaveChanges();
                }

                if (!IsPostBack)
                {
                    PopulateCountries(cboCountry);
                    PopulateCities(cboCity);
                    PopulateCurrencies(cboCurrency);
                    Display();
                }
            }
            catch (Exception x)
            {
                ManageException(x);
            }
        }
         
        protected void PopulateCountries(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                string val = cboAcc.SelectedValue;
                cboAcc.Items.Clear(); 
                cboAcc.DataSource = ent.countries.ToArray<countries>();
                cboAcc.DataBind();
                if (val != null && val.Length > 0) cboAcc.SelectedValue = val;
                else cboAcc.Text = "Select a Country";
            }
            catch (Exception ex) { }
        }

        protected void PopulateCities(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                string val = cboAcc.SelectedValue;
                cboAcc.Items.Clear(); 
                cboAcc.DataSource = ent.vw_cities.OrderBy(p=>p.city_name).ToArray<vw_cities>();
                cboAcc.DataBind();
                if (val != null && val.Length > 0) cboAcc.SelectedValue = val;
                else cboAcc.Text = "Select a City";
            }
            catch (Exception ex) { }
        }

        protected void PopulateCurrencies(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                string val = cboAcc.SelectedValue;
                cboAcc.Items.Clear();
                cboAcc.DataSource = ent.currencies.OrderBy(p => p.major_name).ToArray<currencies>();
                cboAcc.DataBind();
                if (val != null && val.Length > 0) cboAcc.SelectedValue = val;
                else cboAcc.Text = "Select a Currency";
            }
            catch (Exception ex) { }
        }

        private void Display()
        {
            if(profile.addr_line_1!="Enter Address")txtAddressLine1.Text=profile.addr_line_1;
            if (profile.addr_line_2 != null) txtAddressLine2.Text = profile.addr_line_2;
            if (profile.city_id != null) cboCity.SelectedValue = profile.city_id.ToString();
            if (profile.comp_name != "Enter Company Name") txtCompName.Text = profile.comp_name;
            if (profile.country_id != null) cboCountry.SelectedValue = profile.country_id.ToString();
            if (profile.currency_id != null) cboCurrency.SelectedValue = profile.currency_id.ToString();
            if (profile.email != null) txtEmail.Text = profile.email;
            neEmployeeSSFRate.Value = profile.employee_rate;
            neEmployerSSFRate.Value = profile.employer_rate;
            neNHILRate.Value = profile.nhil_rate;
            neWithTaxRate.Value = profile.withh_rate;
            neVATRate.Value = profile.vat_rate;
            neVATFlatRate.Value = profile.vat_flat_rate;
            nePettyCashCeiling.Value = profile.petty_cash_ceil;
            chkGLNoPrecedesName.Checked = profile.num_b4_name;
            if (profile.fax != null) txtFax.Text = profile.fax;
            cboFMOY.SelectedValue = profile.fmoy.ToString();
            if (profile.phon_num != null) txtPhoneNumber.Text = profile.phon_num;
            if (profile.web != null) txtWebsite.Text = profile.web;
            if (profile.vat_num != null) txtVATNo.Text = profile.vat_num;
            chkAllowEditPostedJnl.Checked = profile.edit_posted_jnl;
            chkEnforceCCSecurity.Checked = profile.enf_ou_sec;
            chkEnforceCCUsage.Checked = profile.enf_ou_usg;
            txtPPB.Value = profile.price_per_bag;
        }

        private void Save()
        {
            profile.addr_line_1 = txtAddressLine1.Text;
            if (txtAddressLine2.Text != "") profile.addr_line_2 = txtAddressLine2.Text;
            if (cboCity.SelectedValue != "") profile.city_id = int.Parse(cboCity.SelectedValue);
            if (txtCompName.Text != "")  profile.comp_name = txtCompName.Text;
            if (cboCountry.SelectedValue != "") profile.country_id = int.Parse(cboCountry.SelectedValue);
            if (cboCurrency.SelectedValue != "") profile.currency_id = int.Parse(cboCurrency.SelectedValue);
            if (txtEmail.Text != "") profile.email = txtEmail.Text;
            profile.employee_rate = neEmployeeSSFRate.Value.Value;
            profile.employer_rate = neEmployerSSFRate.Value.Value;
            profile.nhil_rate = neNHILRate.Value.Value;
            profile.petty_cash_ceil = nePettyCashCeiling.Value.Value;
            profile.num_b4_name = chkGLNoPrecedesName.Checked;
            if (txtFax.Text != "") profile.fax = txtFax.Text;
            profile.fmoy = byte.Parse(cboFMOY.SelectedValue);
            if (txtPhoneNumber.Text != "") profile.phon_num = txtPhoneNumber.Text;
            if (txtWebsite.Text != "") profile.web = txtWebsite.Text;
            if (txtVATNo.Text != "") profile.vat_num = txtVATNo.Text;
            profile.edit_posted_jnl = chkAllowEditPostedJnl.Checked;
            profile.enf_ou_usg = chkEnforceCCUsage.Checked;
            profile.enf_ou_sec = chkEnforceCCSecurity.Checked;
            profile.vat_flat_rate = (neVATFlatRate.Value != null) ? neVATFlatRate.Value.Value : 0;
            profile.vat_rate = (neVATRate.Value != null) ? neVATRate.Value.Value : 0;
            profile.withh_rate = (neWithTaxRate.Value != null) ? neWithTaxRate.Value.Value : 0;
            profile.price_per_bag = txtPPB.Value.Value;
            if (upload3.UploadedFiles.Count > 0)
            {
                var item=upload3.UploadedFiles[0];
                byte[] b = new byte[item.InputStream.Length];
                item.InputStream.Read(b, 0, b.Length);

                profile.logo = b;
            }
            ent.SaveChanges();
        }

        private void ManageException(Exception ex)
        {
            string errorMsg = "There was an error processing your request:";
            if (ex is System.Data.Entity.Core.UpdateException)
            {
                if (ex.InnerException.Message.Contains("uk_acct_cat_name") ||
                    ex.InnerException.Message.Contains("uk_acct_cat_code"))
                {
                    errorMsg += "<br />The Main Account Head you are trying to create already exist.";
                }
                if (ex.InnerException.Message.Contains("uk_acct_cat_max_acct_num") ||
                    ex.InnerException.Message.Contains("uk_acct_cat_min_acct_num"))
                {
                    errorMsg += "<br />The Account number range specified overlaps another account head.";
                }
            }
            errorMsg += "Please correct and continue or cancel.";
            divError.Style["visibility"] = "visible";
            spanError.InnerHtml = errorMsg;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception x)
            {
                ManageException(x);
            }
        }

        public string CostCenterName(int user_gl_ou_id)
        {
            var item = (from c in ent.user_gl_ou_gl_ou
                        from m in ent.vw_gl_ou
                        where c.gl_ou.ou_id == m.ou_id
                            && c.user_gl_ou_id == user_gl_ou_id
                        select new
                        {
                            m.ou_name1
                        }).FirstOrDefault();
            return item == null ? "" : item.ou_name1;
        }

        protected void PopulateCostCenters(Telerik.Web.UI.RadComboBox cboCC)
        {
            try
            {
                var cc = (from a in ent.vw_gl_ou
                          select a);
                cboCC.Items.Clear();
                foreach (var cur in cc)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = cur.ou_name1;
                    item.Value = cur.ou_id.ToString();

                    item.Attributes.Add("ou_name", cur.ou_name1);

                    cboCC.Items.Add(item);

                }
                if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != "0")
                    cboCC.SelectedValue = Session["gl_ou_id"].ToString();
                else
                {
                    cboCC.Text = " ";
                }
            }
            catch (Exception ex) { }
        }

        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true)
            {
                var item = e.Item as GridEditableItem;
                var rcb = item["gl_ou_id"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateCostCenters(rcb);
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboCC_SelectedIndexChanged);
                }
            }
        }

        void cboCC_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["gl_ou_id"] = e.Value;
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                user_gl_ou_gl_ou glOU = new user_gl_ou_gl_ou();
                glOU.user_name = newVals["user_name"].ToString();
                glOU.allow = bool.Parse(newVals["allow"].ToString());
                if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != ""
                     && Session["gl_ou_id"].ToString() != "0")
                {
                    var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                    gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                    glOU.gl_ou = ou;
                }

                glOU.creation_date = DateTime.Now;
                glOU.creator = User.Identity.Name;
                ent.user_gl_ou_gl_ou.Add(glOU);
                ent.SaveChanges();
                Save();
                Session["gl_ou_id"] = 0;
                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
            }
            catch (Exception ex) { ManageException(ex); }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                var user_gl_ou_id = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["user_gl_ou_id"];
                var glOU = (from p in ent.user_gl_ou_gl_ou where p.user_gl_ou_id == user_gl_ou_id select p).First<user_gl_ou_gl_ou>();
                 
                glOU.user_name = newVals["user_name"].ToString();
                glOU.allow = bool.Parse(newVals["allow"].ToString());
                if (Session["gl_ou_id"] != null && Session["gl_ou_id"].ToString() != ""
                     && Session["gl_ou_id"].ToString() != "0")
                {
                    var gl_ou_id = int.Parse(Session["gl_ou_id"].ToString());
                    gl_ou ou = ent.gl_ou.First<gl_ou>(p => p.ou_id == gl_ou_id);
                    glOU.gl_ou = ou;
                }
                glOU.modification_date = DateTime.Now;
                glOU.last_modifier = User.Identity.Name;
                ent.SaveChanges();
                Save();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
                Session["gl_ou_id"] = 0;
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
                e.Canceled = true;
                var newVals = new System.Collections.Specialized.ListDictionary();
                newVals["user_gl_ou_id"] = 0;
                newVals["gl_ou_id"] = 0;
                newVals["user_name"] = string.Empty;
                newVals["allow"] = false;  
                newVals["creator"] = User.Identity.Name;
                newVals["creation_date"] = DateTime.Now;
                e.Item.OwnerTableView.InsertItem(newVals);
            }
            else if (e.CommandName == "Edit")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                var user_gl_ou_id = int.Parse(((GridEditableItem)e.Item).GetDataKeyValue("user_gl_ou_id").ToString());
                var ou = (from c in ent.user_gl_ou_gl_ou
                          from u in ent.vw_gl_ou
                          where c.gl_ou.ou_id == u.ou_id
                                  && c.user_gl_ou_id == user_gl_ou_id
                          select new { u.ou_id }).FirstOrDefault();
                if (ou != null) Session["gl_ou_id"] = ou.ou_id;
            }
        }
          
    }
}
