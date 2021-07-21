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
    public partial class supplier : corePage
    {
        private core_dbEntities ent;
        private coreLoansEntities le;

        public override string URL
        {
            get { return "~/ln/setup/supplier.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "supplierContactID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var supplierId = (int) pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["supplierID"];
                    Session["supplierID"] = supplierId;
                    newVals["workPhone"] = string.Empty;
                    newVals["mobilePhone"] = string.Empty;
                    newVals["department"] = string.Empty;
                    newVals["email"] = string.Empty;
                    newVals["contactName"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["supplierName"] = string.Empty;
                    newVals["comments"] = string.Empty;
                    newVals["directions"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_suppliers";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_suppliers";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_suppliers";
            this.RadGrid1.ExportSettings.Pdf.Title = "supplieres Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item.OwnerTableView.DataKeyNames[0] == "supplierID")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem) e.Item);
                e.Canceled = true;
                var supplierId = (int) e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["supplierID"];
                var b = le.suppliers.FirstOrDefault(p => p.supplierID == supplierId);
                b.supplierName = newVals["supplierName"].ToString();
                if (newVals["comments"] != null)
                {
                    b.comments = newVals["comments"].ToString();
                }
                if (newVals["directions"] != null)
                {
                    b.directions = newVals["directions"].ToString();
                }
                b.maximumExposure = double.Parse(newVals["maximumExposure"].ToString());

                le.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
            }
            else if (e.Item.OwnerTableView.DataKeyNames[0] == "supplierContactID")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem) e.Item);
                e.Canceled = true;
                var supplierContactId = (int) e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["supplierContactID"];
                var b = le.supplierContacts.FirstOrDefault(p => p.supplierContactID == supplierContactId);
                b.contactName = newVals["contactName"].ToString();
                if (newVals["workPhone"] != null)
                {
                    b.workPhone = newVals["workPhone"].ToString();
                }
                if (newVals["mobilePhone"] != null)
                {
                    b.mobilePhone = newVals["mobilePhone"].ToString();
                }
                if (newVals["email"] != null)
                {
                    b.email = newVals["email"].ToString();
                }
                if (newVals["department"] != null)
                {
                    b.department = newVals["department"].ToString();
                }
                le.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
            }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item.OwnerTableView.DataKeyNames[0] == "supplierID")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem) e.Item);
                e.Canceled = true;
                coreLogic.supplier b = new coreLogic.supplier();
                b.supplierName = newVals["supplierName"].ToString();
                if (newVals["comments"] != null)
                {
                    b.comments = newVals["comments"].ToString();
                }
                if (newVals["directions"] != null)
                {
                    b.directions = newVals["directions"].ToString();
                }
                b.maximumExposure = double.Parse(newVals["maximumExposure"].ToString());

                le.suppliers.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
            }
            else if (e.Item.OwnerTableView.DataKeyNames[0] == "supplierContactID")
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem) e.Item);
                e.Canceled = true;
                coreLogic.supplierContact b = new coreLogic.supplierContact();
                b.contactName = newVals["contactName"].ToString();
                if (newVals["workPhone"] != null)
                {
                    b.workPhone = newVals["workPhone"].ToString();
                }
                if (newVals["mobilePhone"] != null)
                {
                    b.mobilePhone = newVals["mobilePhone"].ToString();
                }
                if (newVals["email"] != null)
                {
                    b.email = newVals["email"].ToString();
                }
                if (newVals["department"] != null)
                {
                    b.department = newVals["department"].ToString();
                }
                var supplierId = (int) Session["supplierID"];
                b.supplierID = supplierId;

                le.supplierContacts.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
            }
        }

    }
}
