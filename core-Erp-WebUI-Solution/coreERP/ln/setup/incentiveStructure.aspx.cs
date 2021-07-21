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
    public partial class incentiveStructure : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/allowanceType.aspx"; }
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
                e.Canceled = true;
                var newVals = new System.Collections.Specialized.ListDictionary();
                newVals["lowerLimit"] = 0.0;
                newVals["upperLimit"] = 0.0;
                newVals["incentiveAmount"] = 0.0;
                newVals["commissionRate"] = 0.0;
                newVals["withHoldingRate"] = 0.0; 
                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_incentiveStructures";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_incentiveStructures";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_incentiveStructures";
            this.RadGrid1.ExportSettings.Pdf.Title = "Incentive Structures Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
   
        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true; 
                var incentiveStructureID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["incentiveStructureID"];
                var b = le.incentiveStructures.FirstOrDefault(p=> p.incentiveStructureID==incentiveStructureID);
                b.lowerLimit = double.Parse(newVals["lowerLimit"].ToString());
                b.upperLimit = double.Parse(newVals["upperLimit"].ToString());
                b.incentiveAmount = double.Parse(newVals["incentiveAmount"].ToString());
                b.commissionRate = double.Parse(newVals["commissionRate"].ToString());
                b.withHoldingRate = double.Parse(newVals["withHoldingRate"].ToString());
                le.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();            
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                coreLogic.incentiveStructure b = new coreLogic.incentiveStructure();
                b.lowerLimit = double.Parse(newVals["lowerLimit"].ToString());
                b.upperLimit = double.Parse(newVals["upperLimit"].ToString());
                b.incentiveAmount = double.Parse(newVals["incentiveAmount"].ToString());
                b.commissionRate = double.Parse(newVals["commissionRate"].ToString());
                b.withHoldingRate = double.Parse(newVals["withHoldingRate"].ToString());
                
                le.incentiveStructures.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
                    
            }
            catch (Exception ex) {  }
        }

    }
}
