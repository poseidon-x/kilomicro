using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.hc.payroll
{
    public partial class overTimeConfig : corePage
    {

        core_dbEntities ent;
        coreLoansEntities le;
        private String[] months = new String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        
        
        public override string URL
        {
            get { return "~/hc/payroll/overTimeConfig.aspx"; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new core_dbEntities();
            le=new coreLoansEntities();
            if (!IsPostBack)
            {
              /*   cboPayCalendar.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.payCalendars.OrderByDescending(p => p.year).ThenByDescending(p => p.month))
                {
                    cboPayCalendar.Items.Add(new RadComboBoxItem(r.year.ToString()
                        + ", " + months[r.month - 1], r.payCalendarID.ToString()));
                }
              */ 
            }
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
                newVals["levelID"] = 0;
                newVals["saturdayHoursRate"] = 0;
                newVals["sundayHoursRate"] = 0;
                newVals["holidayHoursRate"] = 0;
                newVals["weekdayAfterWorkHoursRate"] = 0;
                newVals["overTime5PerTax"] = 0;
                newVals["overTime10PerTax"] = 0; 

                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_overTimeConfigs";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_overTimeConfigs";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_overTimeConfigs";
            this.RadGrid1.ExportSettings.Pdf.Title = "OverTime rates Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
   
        //protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        //{
        //    try
        //    {
        //        Hashtable newVals = new Hashtable();
        //        e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
        //        e.Canceled = true;
        //        var overTimeConfigID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["overTimeConfigID"];
        //        var b = le.overTimeConfigs.FirstOrDefault(p => p.overTimeConfigID == overTimeConfigID);
        //        b.levelID = int.Parse(newVals["levelID"].ToString());
        //        b.saturdayHoursRate = float.Parse(newVals["saturdayHoursRate"].ToString());
        //        b.sundayHoursRate = float.Parse(newVals["sundayHoursRate"].ToString());
        //        b.holidayHoursRate = float.Parse(newVals["holidayHoursRate"].ToString());
        //        b.weekdayAfterWorkHoursRate = float.Parse(newVals["weekdayAfterWorkHoursRate"].ToString());
        //        b.overTime5PerTax = float.Parse(newVals["overTime5PerTax"].ToString());
        //        b.overTime10PerTax = float.Parse(newVals["overTime10PerTax"].ToString());
                
        //        le.SaveChanges();
        //        RadGrid1.EditIndexes.Clear();
        //        RadGrid1.MasterTableView.IsItemInserted = false;
        //        RadGrid1.MasterTableView.Rebind();            
        //    }
        //    catch (Exception ex) { }
        //}

        //protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        //{
        //    try
        //    {
        //        Hashtable newVals = new Hashtable();
        //        e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
        //        e.Canceled = true;
        //        coreLogic.overTimeConfig b = new coreLogic.overTimeConfig();
        //        //b.payCalendarID = int.Parse(cboPayCalendar.SelectedValue);
        //        b.levelID = int.Parse(newVals["levelID"].ToString());
        //        b.saturdayHoursRate = float.Parse(newVals["saturdayHoursRate"].ToString());
        //        b.sundayHoursRate = float.Parse(newVals["sundayHoursRate"].ToString());
        //        b.holidayHoursRate = float.Parse(newVals["holidayHoursRate"].ToString());
        //        b.weekdayAfterWorkHoursRate = float.Parse(newVals["weekdayAfterWorkHoursRate"].ToString());
        //        b.overTime5PerTax = float.Parse(newVals["overTime5PerTax"].ToString());
        //        b.overTime10PerTax = float.Parse(newVals["overTime10PerTax"].ToString());

        //        le.overTimeConfigs.Add(b);
        //        le.SaveChanges();

        //        RadGrid1.EditIndexes.Clear();
        //        e.Item.OwnerTableView.IsItemInserted = false;
        //        e.Item.OwnerTableView.Rebind();
                    
        //    }
        //    catch (Exception ex) {  }
        //}

    }
}
