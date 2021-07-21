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
    public partial class approveDepreciation : System.Web.UI.Page
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
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            var pro = ent.comp_prof.FirstOrDefault();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtProposedAmount = item.FindControl("txtProposedAmount") as Telerik.Web.UI.RadNumericTextBox;
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                if (lblID != null && txtProposedAmount != null && chkSelected != null && chkSelected.Checked==true)
                {
                    int id = int.Parse(lblID.Text);
                    var pen = le.assetDepreciations.FirstOrDefault(p => p.assetDepreciationID == id);
                    if (pen != null)
                    {
                        pen.asset.assetCurrentValue -= txtProposedAmount.Value.Value;
                        pen.depreciationAmount = txtProposedAmount.Value.Value;
                        pen.proposedAmount = 0;

                        var jb = journalextensions.Post("LN", pen.asset.depreciationAccountID.Value,
                            pen.asset.accumulatedDepreciationAccountID.Value, (txtProposedAmount.Value.Value),
                            "Depreciation - " + pen.asset.assetDescription+ " - " ,
                            pro.currency_id.Value, pen.drepciationDate.Value, pen.asset.assetTag, ent, User.Identity.Name,
                            null);

                        ent.jnl_batch.Add(jb); 
                    }
                }
            }
            le.SaveChanges();
            ent.SaveChanges();
            HtmlHelper.MessageBox2("Asset depreciated successfully!", ResolveUrl("/fa/assets/approveDepreciation.aspx"), "coreERP©: Successful", IconType.ok);
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
            cboSCat.Items.Clear();
            cboSCat.Items.Add(new RadComboBoxItem("", ""));
            int? catID = null;
            if (cboCat.SelectedValue != "")
            {
                catID = int.Parse(cboCat.SelectedValue);
            }
            foreach (var s in le.assetSubCategories.Where(p => catID == null || p.assetCategoryID == catID))
            {
                cboSCat.Items.Add(new RadComboBoxItem(s.assetSubCategoryName, s.assetSubCategoryID.ToString()));
            }
            OnChange();
        }

        protected void cboSCat_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
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
            CalculateDepreciation(assetID, catID, ouID,sCatID);
            var pens = le.assetDepreciations.Where(p => p.proposedAmount > 0 && (assetID == null || p.asset.assetID == assetID)
                && (catID == null || p.asset.assetSubCategory.assetCategoryID == catID)
                && (sCatID == null || p.asset.assetSubCategory.assetSubCategoryID == sCatID) 
                && (ouID == null || p.asset.ouID == ouID)).ToList(); 
            rpPenalty.DataSource = pens;
            rpPenalty.DataBind();
        }
    }
}