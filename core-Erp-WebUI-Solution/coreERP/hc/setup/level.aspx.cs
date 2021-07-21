using System;
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
    public partial class level : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/level.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "levelNotchID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var levelID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["levelID"];
                    Session["levelID"] = levelID;
                    newVals["notchName"] = string.Empty;
                    newVals["sortOrder"] = 0;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelAllowanceID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var levelID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["levelID"];
                    Session["levelID"] = levelID;
                    newVals["allowanceTypeID"] = 0;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelDeductionID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var levelID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["levelID"];
                    Session["levelID"] = levelID;
                    newVals["deductionTypeID"] = 0;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelBenefitsInKindID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var levelID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["levelID"];
                    Session["levelID"] = levelID;
                    newVals["benefitsInKindID"] = 0;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelLeaveID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var levelID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["levelID"];
                    Session["levelID"] = levelID;
                    newVals["leaveTypeID"] = 0;
                    newVals["maxDaysPerAnnum"] = 0;
                    e.Item.OwnerTableView.InsertItem(newVals);
                } 
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["levelName"] = string.Empty; 
                    newVals["sortOrder"] = 0; 
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_levels";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_levels";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_levels";
            this.RadGrid1.ExportSettings.Pdf.Title = "Levels Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        public string EmploymentTypeName(object empID)
        {
            int? id = null;
            if (empID != null) id = (int)empID;
            var item = le.employmentTypes.FirstOrDefault(p => p.employmentTypeID == id);
            return item == null ? "" : item.employmentTypeName;
        }

        protected void PopulateETs(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                cboAcc.DataValueField = "employmentTypeID";
                cboAcc.DataTextField = "employmentTypeName";
                cboAcc.DataSource = le.employmentTypes.ToList();
                cboAcc.DataBind();
                cboAcc.Items.Insert(0, new RadComboBoxItem("", "0"));
            }
            catch (Exception ex) { }
        }

        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        { 
        }

        void cboAcc_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var rcb = o as RadComboBox;
            if (rcb != null)
            {
                Session[rcb.ValidationGroup] = e.Value;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "levelID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var levelID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["levelID"];
                    var b = le.levels.FirstOrDefault(p => p.levelID == levelID);
                    b.levelName = newVals["levelName"].ToString(); 
                    b.sortOrder=int.Parse(newVals["sortOrder"].ToString()); 
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelAllowanceID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var levelAllowanceID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["levelAllowanceID"];
                    var b = le.levelAllowances.FirstOrDefault(p => p.levelAllowanceID == levelAllowanceID);
                    b.allowanceTypeID = int.Parse(newVals["allowanceTypeID"].ToString()); 
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelDeductionID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var levelDeductionID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["levelDeductionID"];
                    var b = le.levelDeductions.FirstOrDefault(p => p.levelDeductionID == levelDeductionID);
                    b.deductionTypeID = int.Parse(newVals["deductionTypeID"].ToString());
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelLeaveID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var levelLeaveID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["levelLeaveID"];
                    var b = le.levelLeaves.FirstOrDefault(p => p.levelLeaveID == levelLeaveID);
                    b.leaveTypeID = int.Parse(newVals["leaveTypeID"].ToString());
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelBenefitsInKindID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var levelBenefitsInKindID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["levelBenefitsInKindID"];
                    var b = le.levelBenefitsInKinds.FirstOrDefault(p => p.levelBenefitsInKindID == levelBenefitsInKindID);
                    b.benefitsInKindID = int.Parse(newVals["benefitsInKindID"].ToString());
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelNotchID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var levelNotchID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["levelNotchID"];
                    var b = le.levelNotches.FirstOrDefault(p => p.levelNotchID == levelNotchID);
                    b.sortOrder = int.Parse(newVals["sortOrder"].ToString());
                    b.notchName = newVals["notchName"].ToString();
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "levelID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true; 
                    var b = new coreLogic.level();
                    b.levelName = newVals["levelName"].ToString();
                    b.sortOrder = int.Parse(newVals["sortOrder"].ToString());
                    le.levels.Add(b);
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelAllowanceID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true; 
                    var b = new coreLogic.levelAllowance();
                    b.allowanceTypeID = int.Parse(newVals["allowanceTypeID"].ToString());
                    b.levelID = int.Parse(Session["levelID"].ToString());
                    le.levelAllowances.Add(b);
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelDeductionID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true; 
                    var b = new coreLogic.levelDeduction();
                    b.deductionTypeID = int.Parse(newVals["deductionTypeID"].ToString());
                    b.levelID = int.Parse(Session["levelID"].ToString());
                    le.levelDeductions.Add(b);
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelLeaveID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true; 
                    var b = new coreLogic.levelLeave();
                    b.leaveTypeID = int.Parse(newVals["leaveTypeID"].ToString());
                    b.levelID = int.Parse(Session["levelID"].ToString());
                    le.levelLeaves.Add(b);
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelBenefitsInKindID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true; 
                    var b = new coreLogic.levelBenefitsInKind();
                    b.benefitsInKindID = int.Parse(newVals["benefitsInKindID"].ToString());
                    b.levelID = int.Parse(Session["levelID"].ToString());
                    le.levelBenefitsInKinds.Add(b);
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "levelNotchID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var b = new coreLogic.levelNotch();
                    b.sortOrder = int.Parse(newVals["sortOrder"].ToString());
                    b.notchName = newVals["notchName"].ToString();
                    b.levelID = int.Parse(Session["levelID"].ToString());
                    le.levelNotches.Add(b);
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                } 
            }
            catch (Exception ex) {  }
        }

    }
}
