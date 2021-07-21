using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.fa.assets
{
    public partial class postAsset : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.assets.OrderBy(p => p.assetDescription))
                {
                    cboClient.Items.Add(new RadComboBoxItem(cl.assetDescription, cl.assetID.ToString()));
                }
                cboCat.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.assetCategories.OrderBy(p => p.assetCategoryName))
                {
                    cboCat.Items.Add(new RadComboBoxItem(cl.assetCategoryName, cl.assetCategoryID.ToString()));
                }
                cboOU.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in ent.vw_ou.OrderBy(p => p.ou_name))
                {
                    cboOU.Items.Add(new RadComboBoxItem(cl.ou_name, cl.ou_id.ToString()));
                }
                cboSCat.Items.Clear();
                cboSCat.Items.Add(new RadComboBoxItem("", ""));
                foreach (var s in le.assetSubCategories)
                {
                    cboSCat.Items.Add(new RadComboBoxItem(s.assetSubCategoryName, s.assetSubCategoryID.ToString()));
                }
                PopulateAccounts(cboCrAcc);
                PopulateAccounts(cboDebAcc);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (cboCrAcc.SelectedValue == "" || cboDebAcc.SelectedValue == "")
            {
                HtmlHelper.MessageBox("Please select posting accounts!");
            }
            else
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                var pro = ent.comp_prof.FirstOrDefault();
                foreach (RepeaterItem item in rpPenalty.Items)
                {
                    var lblID = item.FindControl("lblID") as Label;
                    var chkSelected = item.FindControl("chkSelected") as CheckBox;
                    if (lblID != null && chkSelected != null && chkSelected.Checked == true
                        && cboDebAcc.SelectedValue != "" && cboCrAcc.SelectedValue != "")
                    {
                        int crAccID = int.Parse(cboCrAcc.SelectedValue);
                        int debAccID = int.Parse(cboDebAcc.SelectedValue);

                        int id = int.Parse(lblID.Text);
                        var pen = le.assets.FirstOrDefault(p => p.assetID == id);
                        if (pen != null)
                        {
                            //pen.assetSubCategoryReference.Load();
                            //pen.assetSubCategory.assetCategoryReference.Load();

                            var jb = journalextensions.Post("LN", debAccID,
                                crAccID, pen.assetPrice,
                                "Asset Purchase - " + pen.assetDescription,
                                pro.currency_id.Value, pen.assetPurchaseDate, pen.assetTag, ent, User.Identity.Name,null);
                            pen.posted = true;

                            ent.jnl_batch.Add(jb);
                        }
                    }
                }
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Asset depreciated successfully!", ResolveUrl("/fa/assets/postAsset.aspx"), "coreERP©: Successful", IconType.ok);
            }
        }

        protected void PopulateAccounts(RadComboBox cboAcc)
        {
            try
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                var accs = (from a in ent.accts
                            from c in ent.currencies
                            where a.currencies.currency_id == c.currency_id
                            orderby a.acc_num
                            select new
                            {
                                a.acct_id,
                                a.acc_num,
                                a.acc_name,
                                c.major_name,
                                c.major_symbol
                            }).ToList();
                cboAcc.DataSource = accs;
                cboAcc.DataBind();
                cboAcc.Items.Insert(0, new RadComboBoxItem("Transaction Account", null));
            }
            catch (Exception ex) { }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            var pro = ent.comp_prof.FirstOrDefault();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtProposedAmount = item.FindControl("txtProposedAmount") as Telerik.Web.UI.RadNumericTextBox; 
                if (lblID != null && txtProposedAmount != null )
                {
                    int id = int.Parse(lblID.Text);
                    var pen = le.assetDepreciations.FirstOrDefault(p => p.assetDepreciationID == id);
                    if (pen != null)
                    {
                        le.assetDepreciations.Remove(pen);
                    }
                }
            }
            le.SaveChanges(); 
            Response.Redirect("/fa/assets/approveDepreciation.aspx");
        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        public void CalculateDepreciation(int? assetID, int? catID, int? ouID, int? sCatID)
        {
            try
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var lns = le.depreciationSchedules.Where(p => p.drepciationDate <= date &&
                   (assetID == null || p.asset.assetID == assetID)
                && (catID == null || p.asset.assetSubCategory.assetCategoryID == catID)
                && (sCatID == null || p.asset.assetSubCategory.assetSubCategoryID == sCatID)
                && (ouID == null || p.asset.ouID == ouID)
                && (p.asset.lastDepreciationDate == null || p.drepciationDate > p.asset.lastDepreciationDate)).ToList();
                foreach (var cl in lns)
                {
                    //cl.assetReference.Load();
                    ////cl.asset.assetSubCategoryReference.Load();
                    //cl.asset.assetSubCategory.assetCategoryReference.Load();
                    //cl.asset.assetDepreciations.Load();
                    var sch = cl.asset.assetDepreciations.FirstOrDefault(p => p.drepciationDate == cl.drepciationDate);
                    if (sch == null)
                    {
                        var inte = new coreLogic.assetDepreciation
                        {
                            assetID = cl.assetID,
                            assetValue = cl.assetValue,
                            depreciationAmount = 0,
                            proposedAmount = cl.depreciationAmount,
                            drepciationDate = cl.drepciationDate,
                            period = 1,
                            startDate = cl.startDate
                        };
                        cl.asset.assetDepreciations.Add(inte);
                    }
                    cl.asset.lastDepreciationDate = (cl.asset.lastDepreciationDate == null || cl.asset.lastDepreciationDate <= cl.drepciationDate) ? cl.drepciationDate : cl.asset.lastDepreciationDate;

                    le.SaveChanges();
                }
            }
            catch (Exception x)
            {
            }
        }

        protected void cboSCat_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        protected void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rpPenalty.Items)
            { 
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                if (chkSelected != null)
                {
                    chkSelected.Checked = true;
                }
            }
        }

        protected void btnUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                if (chkSelected != null)
                {
                    chkSelected.Checked = false;
                }
            }
        }

        protected void cboCat_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        protected void cboOU_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            int? assetID = null;
            int? ouID = null;
            int? catID = null;
            int? sCatID = null;
            if (cboClient.SelectedValue != "")
            {
                assetID = int.Parse(cboClient.SelectedValue);
            }
            if (cboCat.SelectedValue != "")
            {
                catID = int.Parse(cboCat.SelectedValue);
            }
            if (cboOU.SelectedValue != "")
            {
                ouID = int.Parse(cboOU.SelectedValue);
            }
            if (cboSCat.SelectedValue != "")
            {
                sCatID = int.Parse(cboSCat.SelectedValue);
            }
            var pens = le.assets.Where(p => p.posted == false && (assetID == null || p.assetID == assetID)
                && (catID == null || p.assetSubCategory.assetCategoryID == catID)
                && (sCatID == null || p.assetSubCategory.assetSubCategoryID == sCatID)
                && (ouID == null || p.ouID == ouID)).ToList(); 
            rpPenalty.DataSource = pens;
            rpPenalty.DataBind();
        }
    }
}