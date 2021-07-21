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
    public partial class overTime : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        private String[] months = new String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public override string URL
        {
            get { return "~/hc/payroll/overTime.aspx"; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new core_dbEntities();
            le=new coreLoansEntities();
            if (!IsPostBack)
            {
                cboPayCalendar.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.payCalendars.OrderByDescending(p => p.year).ThenByDescending(p => p.month))
                {
                    cboPayCalendar.Items.Add(new RadComboBoxItem(r.year.ToString()
                        + ", " + months[r.month - 1], r.payCalendarID.ToString()));
                }
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
                newVals["staffID"] = 0;
                newVals["payCalendarID"] = 0;
                //newVals["daysWorked"] = 0; 
                newVals["saturdayHours"] = 0;
                newVals["sundayHours"] = 0;
                newVals["holidayHours"] = 0;
                newVals["weekdayAfterWorkHours"] = 0; 

                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_overTimes";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_overTimes";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_overTimes";
            this.RadGrid1.ExportSettings.Pdf.Title = "OverTime Defined in System";
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
                var overTimeID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["overTimeID"];
                var b = le.overTimes.FirstOrDefault(p => p.overTimeID == overTimeID);
                b.staffID = int.Parse(newVals["staffID"].ToString());
                //b.daysWorked = float.Parse(newVals["daysWorked"].ToString()); 
                b.saturdayHours = float.Parse(newVals["saturdayHours"].ToString());
                b.sundayHours = float.Parse(newVals["sundayHours"].ToString());
                b.holidayHours = float.Parse(newVals["holidayHours"].ToString());
                b.weekdayAfterWorkHours = float.Parse(newVals["weekdayAfterWorkHours"].ToString()); 
                
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
                coreLogic.overTime b = new coreLogic.overTime();
                b.payCalendarID = int.Parse(cboPayCalendar.SelectedValue);
                b.staffID = int.Parse(newVals["staffID"].ToString());
                //b.daysWorked = float.Parse(newVals["daysWorked"].ToString()); 
                b.saturdayHours = float.Parse(newVals["saturdayHours"].ToString());
                b.sundayHours = float.Parse(newVals["sundayHours"].ToString());
                b.holidayHours = float.Parse(newVals["holidayHours"].ToString());
                b.weekdayAfterWorkHours = float.Parse(newVals["weekdayAfterWorkHours"].ToString()); 

                le.overTimes.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();
                    
            }
            catch (Exception ex) {  }
        }

        protected void cboPayCalendar_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboPayCalendar.SelectedValue != "")
            {
                EntityDataSource1.WhereParameters[0].DefaultValue = cboPayCalendar.SelectedValue;
            }
            else
            {
                EntityDataSource1.WhereParameters[0].DefaultValue = "-1";
            }
        }

    }
}
