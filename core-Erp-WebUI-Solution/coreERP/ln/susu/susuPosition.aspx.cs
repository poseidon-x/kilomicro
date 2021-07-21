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
    public partial class susuPosition : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/susuPosition.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "susuGradePositionID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var susuPositionID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["susuPositionID"];
                    Session["susuPositionID"] = susuPositionID;
                    newVals["susuGradeID"] = 0;
                    newVals["amountEntitled"] = 0;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["susuPositionName"] = string.Empty;
                    newVals["susuPositionNo"] = 0;
                    newVals["noOfWaitingPeriods"] = 0;
                    newVals["percentageInterest"] = 0;
                    newVals["maxDefaultDays"] = 0;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_susuPositions";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_susuPositions";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_susuPositions";
            this.RadGrid1.ExportSettings.Pdf.Title = "susuPositiones Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
   
        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "susuPositionID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var susuPositionID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["susuPositionID"];
                    var b = le.susuPositions.FirstOrDefault(p => p.susuPositionID == susuPositionID);
                    b.susuPositionName = newVals["susuPositionName"].ToString();
                    b.susuPositionNo = int.Parse(newVals["susuPositionNo"].ToString());
                    b.noOfWaitingPeriods = int.Parse(newVals["noOfWaitingPeriods"].ToString());
                    b.percentageInterest = int.Parse(newVals["percentageInterest"].ToString());
                    b.maxDefaultDays = int.Parse(newVals["maxDefaultDays"].ToString());
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "susuGradePositionID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var susuGradePositionID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["susuGradePositionID"];
                    var b = le.susuGradePositions.FirstOrDefault(p => p.susuGradePositionID == susuGradePositionID);
                    b.susuGradeID = int.Parse(newVals["susuGradeID"].ToString()); 
                    b.susuGradeID = int.Parse(newVals["susuGradeID"].ToString());
                    b.amountEntitled = double.Parse(newVals["amountEntitled"].ToString()); 
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "susuPositionID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.susuPosition b = new coreLogic.susuPosition();
                    b.susuPositionName = newVals["susuPositionName"].ToString();
                    b.susuPositionNo = int.Parse(newVals["susuPositionNo"].ToString());
                    b.noOfWaitingPeriods = int.Parse(newVals["noOfWaitingPeriods"].ToString());
                    b.percentageInterest = int.Parse(newVals["percentageInterest"].ToString());
                    b.maxDefaultDays = int.Parse(newVals["maxDefaultDays"].ToString());

                    le.susuPositions.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "susuGradePositionID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.susuGradePosition b = new coreLogic.susuGradePosition();
                    b.susuGradeID = int.Parse(newVals["susuGradeID"].ToString());
                    var susuPositionID = (int)Session["susuPositionID"];
                    b.susuGradeID = int.Parse(newVals["susuGradeID"].ToString());
                    b.amountEntitled = double.Parse(newVals["amountEntitled"].ToString()); 
                    b.susuPositionID = susuPositionID;

                    le.susuGradePositions.Add(b);
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
