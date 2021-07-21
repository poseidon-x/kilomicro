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
    public partial class loanProduct : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/loanProduct.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "loanProductHistoryID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var loanProductID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["loanProductID"];
                    Session["loanProductID"] = loanProductID; 
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["loanProductName"] = string.Empty;
                    newVals["loanTenure"] = 0;
                    newVals["minAge"] = 0;
                    newVals["maxAge"] = 0;
                    newVals["rate"] = 0;
                    newVals["procFeeRate"] = 0; 
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanProducts";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanProducts";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanProducts";
            this.RadGrid1.ExportSettings.Pdf.Title = "Loan Products Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
   
        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "loanProductID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var loanProductID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["loanProductID"];
                    var b = le.loanProducts.FirstOrDefault(p => p.loanProductID == loanProductID);
                    b.loanProductName = newVals["loanProductName"].ToString();
                    b.rate = double.Parse(newVals["rate"].ToString());
                    b.loanTenure = double.Parse(newVals["loanTenure"].ToString());
                    b.minAge = double.Parse(newVals["minAge"].ToString());
                    b.maxAge = double.Parse(newVals["maxAge"].ToString());
                    b.procFeeRate = double.Parse(newVals["procFeeRate"].ToString());

                    coreLogic.loanProductHistory hist = new loanProductHistory
                    {
                        loanProductID = b.loanProductID,
                        loanProductName = b.loanProductName,
                        loanTenure = b.loanTenure,
                        rate = b.rate,
                        minAge = b.minAge,
                        maxAge = b.maxAge,
                        archiveDate = DateTime.Now
                    };
                    b.loanProductHistories.Add(hist);

                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "loanProductHistoryID")
                { 
                }
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "loanProductID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.loanProduct b = new coreLogic.loanProduct();
                    b.loanProductName = newVals["loanProductName"].ToString();
                    b.rate = double.Parse(newVals["rate"].ToString());
                    b.loanTenure = double.Parse(newVals["loanTenure"].ToString());
                    b.minAge = double.Parse(newVals["minAge"].ToString());
                    b.maxAge = double.Parse(newVals["maxAge"].ToString());
                    b.procFeeRate = double.Parse(newVals["procFeeRate"].ToString());

                    le.loanProducts.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "loanProductHistoryID")
                { 
                }  
            }
            catch (Exception ex) {  }
        }

    }
}
