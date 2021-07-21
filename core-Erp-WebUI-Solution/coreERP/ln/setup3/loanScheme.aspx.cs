using System; 
using System.Linq; 
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.ln.setup3
{
    public partial class loanScheme : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup3/loanScheme.aspx"; }
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
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanSchemes";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanSchemes";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanSchemes";
            this.RadGrid1.ExportSettings.Pdf.Title = "Invoice Loan Configuration";
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
                var loanSchemeId = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["loanSchemeId"];
                var b = le.loanSchemes.FirstOrDefault(p => p.loanSchemeId == loanSchemeId);
                b.loanTypeId = int.Parse(newVals["loanTypeId"].ToString());
                b.employerId = int.Parse(newVals["employerId"].ToString());
                b.tenure = double.Parse(newVals["tenure"].ToString());
                b.rate = double.Parse(newVals["rate"].ToString());
                b.loanSchemeName = newVals["loanSchemeName"].ToString(); 
                
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
                coreLogic.loanScheme b = new coreLogic.loanScheme();
                b.loanTypeId = int.Parse(newVals["loanTypeId"].ToString());
                b.employerId = int.Parse(newVals["employerId"].ToString());
                b.tenure = double.Parse(newVals["tenure"].ToString());
                b.rate = double.Parse(newVals["rate"].ToString());
                b.loanSchemeName = newVals["loanSchemeName"].ToString(); 
               
                le.loanSchemes.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
                    
            }
            catch (Exception ex) {  }
        }

    }
}
