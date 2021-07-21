using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.ln.setup2
{
    public partial class invoiceLoanConfig : corePage
    {
        private core_dbEntities ent;
        private coreLoansEntities le;

        public override string URL
        {
            get { return "~/ln/setup2/invoiceLoanConfig.aspx"; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new core_dbEntities();
            le = new coreLoansEntities();
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
                newVals["clientID"] = 0;
                newVals["supplierID"] = 0;
                newVals["ceilRate"] = 0;
                newVals["standardInterestrate"] = 0;
                newVals["standardProcessingFeerate"] = 0;
                newVals["isEnabled"] = false;
                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_invoiceLoanConfigs";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_invoiceLoanConfigs";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_invoiceLoanConfigs";
            this.RadGrid1.ExportSettings.Pdf.Title = "Invoice Loan Configuration";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            Hashtable newVals = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem) e.Item);
            e.Canceled = true;
            var invoiceLoanConfigID = (int) e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["invoiceLoanConfigID"];
            var b = le.invoiceLoanConfigs.FirstOrDefault(p => p.invoiceLoanConfigID == invoiceLoanConfigID);
            b.clientID = int.Parse(newVals["clientID"].ToString());
            b.supplierID = int.Parse(newVals["supplierID"].ToString());
            b.ceilRate = double.Parse(newVals["ceilRate"].ToString());
            b.standardInterestrate = double.Parse(newVals["standardInterestrate"].ToString());
            b.standardProcessingFeerate = double.Parse(newVals["standardProcessingFeerate"].ToString());
            b.allowPODisbursement = bool.Parse(newVals["allowPODisbursement"].ToString());
            b.maximumExposure = double.Parse(newVals["maximumExposure"].ToString());
            le.SaveChanges();
            RadGrid1.EditIndexes.Clear();
            RadGrid1.MasterTableView.IsItemInserted = false;
            RadGrid1.MasterTableView.Rebind();
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            Hashtable newVals = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem) e.Item);
            e.Canceled = true;
            coreLogic.invoiceLoanConfig b = new coreLogic.invoiceLoanConfig();
            b.clientID = int.Parse(newVals["clientID"].ToString());
            b.supplierID = int.Parse(newVals["supplierID"].ToString());
            b.ceilRate = double.Parse(newVals["ceilRate"].ToString());
            b.standardInterestrate = double.Parse(newVals["standardInterestrate"].ToString());
            b.standardProcessingFeerate = double.Parse(newVals["standardProcessingFeerate"].ToString());
            b.allowPODisbursement = bool.Parse(newVals["allowPODisbursement"].ToString());
            b.maximumExposure = double.Parse(newVals["maximumExposure"].ToString());

            le.invoiceLoanConfigs.Add(b);
            le.SaveChanges();

            RadGrid1.EditIndexes.Clear();
            e.Item.OwnerTableView.IsItemInserted = false;
            e.Item.OwnerTableView.Rebind();
        }

    }
}
