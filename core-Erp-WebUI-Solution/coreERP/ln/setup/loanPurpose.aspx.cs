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
    public partial class loanPurpose : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/loanPurpose.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "loanPurposeDetailID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var loanPurposeID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["loanPurposeID"];
                    Session["loanPurposeID"] = loanPurposeID;
                    newVals["loanPurposeDetailName"] = string.Empty;
                    newVals["accepted"] = false;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["loanPurposeName"] = string.Empty;
                    newVals["accepted"] = false;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanPurposes";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanPurposes";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanPurposes";
            this.RadGrid1.ExportSettings.Pdf.Title = "Loan Purposes Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
   
        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "loanPurposeID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var loanPurposeID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["loanPurposeID"];
                    var b = le.loanPurposes.FirstOrDefault(p => p.loanPurposeID == loanPurposeID);
                    b.loanPurposeName = newVals["loanPurposeName"].ToString();
                    b.accepted = bool.Parse(newVals["accepted"].ToString());
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "loanPurposeDetailID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var loanPurposeDetailID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["loanPurposeDetailID"];
                    var b = le.loanPurposeDetails.FirstOrDefault(p => p.loanPurposeDetailID == loanPurposeDetailID);
                    b.loanPurposeDetailName = newVals["loanPurposeDetailName"].ToString();
                    b.accepted = bool.Parse(newVals["accepted"].ToString());
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "loanPurposeID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.loanPurpose b = new coreLogic.loanPurpose();
                    b.loanPurposeName = newVals["loanPurposeName"].ToString();
                    b.accepted = bool.Parse(newVals["accepted"].ToString());

                    le.loanPurposes.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "loanPurposeDetailID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.loanPurposeDetail b = new coreLogic.loanPurposeDetail();
                    b.loanPurposeDetailName = newVals["loanPurposeDetailName"].ToString();
                    var loanPurposeID = (int)Session["loanPurposeID"];
                    b.accepted = bool.Parse(newVals["accepted"].ToString());
                    b.loanPurposeID = loanPurposeID;

                    le.loanPurposeDetails.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }  
            }
            catch (Exception ex) {  }
        }

    }
}
