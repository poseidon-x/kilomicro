﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.hc.setup
{
    public partial class oneTimeDeductionType : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/hc/setup/oneTimeDeductionType.aspx"; }
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
                newVals["oneTimeDeductionTypeName"] = string.Empty;
                newVals["isPercent"] = false;
                newVals["isBeforeTax"] = false;
                newVals["isBeforePension"] = false;
                newVals["amount"] = 0; 
                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_oneTimeDeductionTypes";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_oneTimeDeductionTypes";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_oneTimeDeductionTypes";
            this.RadGrid1.ExportSettings.Pdf.Title = "One Time Deduction Types Defined in System";
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
                var oneTimeDeductionTypeID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["oneTimeDeductionTypeID"];
                var b = le.oneTimeDeductionTypes.FirstOrDefault(p => p.oneTimeDeductionTypeID == oneTimeDeductionTypeID);
                b.oneTimeDeductionTypeName = newVals["oneTimeDeductionTypeName"].ToString();
                b.isPercent = bool.Parse(newVals["isPercent"].ToString());
                b.isBeforeTax = bool.Parse(newVals["isBeforeTax"].ToString());
                b.isBeforePension = bool.Parse(newVals["isBeforePension"].ToString());
                b.amount = double.Parse(newVals["amount"].ToString()); 
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
                coreLogic.oneTimeDeductionType b = new coreLogic.oneTimeDeductionType();
                b.oneTimeDeductionTypeName = newVals["oneTimeDeductionTypeName"].ToString();
                b.isPercent = bool.Parse(newVals["isPercent"].ToString());
                b.isBeforeTax = bool.Parse(newVals["isBeforeTax"].ToString());
                b.amount = double.Parse(newVals["amount"].ToString());
                b.isBeforePension = bool.Parse(newVals["isBeforePension"].ToString());

                le.oneTimeDeductionTypes.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
                    
            }
            catch (Exception ex) {  }
        }

    }
}
