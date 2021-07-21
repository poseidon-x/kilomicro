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
    public partial class notification : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/notification.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "notificationScheduleID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var notificationID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["notificationID"];
                    Session["notificationID"] = notificationID;
                    newVals["startTime"] = string.Empty;
                    newVals["endTime"] = string.Empty;
                    newVals["frequencyID"] = -1;
                    newVals["dayOfWeekID"] = -1;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "notificationRecipientID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var notificationID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["notificationID"];
                    Session["notificationID"] = notificationID;
                    newVals["email"] = "";
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["notificationCode"] = string.Empty;
                    newVals["notificationName"] = "";
                    newVals["description"] = "";
                    newVals["isActive"] = true;
                    //newVals["employmentTypeID"] = 0;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_notification";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_notification";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_notification";
            this.RadGrid1.ExportSettings.Pdf.Title = "employeres Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
         
        protected void PopulateStaffIDs(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                cboAcc.DataValueField = "staffID";
                cboAcc.DataTextField = "staffName";
                cboAcc.DataSource = (from s in le.staffs
                                     select new {staffName=s.surName+", " + s.otherNames, s.staffID }).ToList();
                cboAcc.DataBind();
                cboAcc.Items.Insert(0, new RadComboBoxItem("", "-1"));
            }
            catch (Exception ex) { }
        }
        
         protected void PopulateDOWs(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                Dictionary<int, string> dict = new Dictionary<int, string>();
                dict.Add(-1, "");
                dict.Add(0, "Sunday");
                dict.Add(1, "Monday");
                dict.Add(2, "Tuesday");
                dict.Add(3, "Wednesday");
                dict.Add(4, "Thursday");
                dict.Add(5, "Friday");
                dict.Add(6, "Saturday");

                cboAcc.DataValueField = "Key";
                cboAcc.DataTextField = "Value";
                cboAcc.DataSource = dict;
                cboAcc.DataBind();
                cboAcc.Items.Insert(0, new RadComboBoxItem("", "-1"));
            }
            catch (Exception ex) { }
        }
        
         protected void PopulateFrequencies(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                Dictionary<int, string> dict = new Dictionary<int, string>();
                dict.Add(-1, "");
                dict.Add(0, "On-Demand");
                dict.Add(1, "Hourly");
                dict.Add(2, "Daily");
                dict.Add(3, "Weekly");
                dict.Add(4, "Monthly"); 

                cboAcc.DataValueField = "Key";
                cboAcc.DataTextField = "Value";
                cboAcc.DataSource = dict;
                cboAcc.DataBind(); 
            }
            catch (Exception ex) { }
        }

        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true && e.Item.OwnerTableView.DataKeyNames.Contains("notificationRecipientID"))
            {
                var item = e.Item as GridEditableItem;
                var rcb = item["staffID"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateStaffIDs(rcb);
                    rcb.ValidationGroup = "staffID";
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAcc_SelectedIndexChanged);
                    //rcb.DataBind();
                }
            }
            else if (e.Item is GridEditableItem && e.Item.IsInEditMode == true && e.Item.OwnerTableView.DataKeyNames.Contains("notificationScheduleID"))
            {
                var item = e.Item as GridEditableItem;
                var rcb = item["dayOfWeekID"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateDOWs(rcb);
                    rcb.ValidationGroup = "dayOfWeekID";
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAcc_SelectedIndexChanged);
                    //rcb.DataBind();
                }

                item = e.Item as GridEditableItem;
                rcb = item["frequencyID"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateFrequencies(rcb);
                    rcb.ValidationGroup = "frequencyID";
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAcc_SelectedIndexChanged);
                    //rcb.DataBind();
                }
            }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "notificationID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var notificationID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["notificationID"];
                    var b = le.notifications.FirstOrDefault(p => p.notificationID == notificationID);
                    b.notificationName = newVals["notificationName"].ToString();
                    b.notificationCode = newVals["notificationCode"].ToString();
                    b.description = newVals["description"].ToString();
                    b.isActive = bool.Parse(newVals["isActive"].ToString()); 
                    
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "notificationScheduleID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var notificationScheduleID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["notificationScheduleID"];
                    var b = le.notificationSchedules.FirstOrDefault(p => p.notificationScheduleID == notificationScheduleID);
                    if (Session["frequencyID"] != null)
                    {
                        b.frequencyID = int.Parse(Session["frequencyID"].ToString());
                    }
                    if (Session["dayOfWeekID"] != null)
                    {
                        b.frequencyID = int.Parse(Session["dayOfWeekID"].ToString());
                    } 
                    b.startTime = TimeSpan.ParseExact(newVals["startTime"].ToString(), "hh:mm", System.Globalization.CultureInfo.CurrentCulture);
                    b.endTime = TimeSpan.ParseExact(newVals["endTime"].ToString(), "hh:mm", System.Globalization.CultureInfo.CurrentCulture);
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "notificationRecipientID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var notificationRecipientID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["notificationRecipientID"];
                    var b = le.notificationRecipients.FirstOrDefault(p => p.notificationRecipientID == notificationRecipientID);
                    if (Session["staffID"] != null)
                    {
                        b.staffID = int.Parse(Session["staffID"].ToString());
                    }
                    b.email = newVals["email"].ToString();
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "notificationID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.notification b = new coreLogic.notification(); 
                    b.notificationName = newVals["notificationName"].ToString();
                    b.notificationCode = newVals["notificationCode"].ToString();
                    b.description = newVals["description"].ToString();
                    b.isActive = bool.Parse(newVals["isActive"].ToString());


                    le.notifications.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "notificationScheduleID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.notificationSchedule b = new coreLogic.notificationSchedule();
                    if (Session["frequencyID"] != null)
                    {
                        b.frequencyID = int.Parse(Session["frequencyID"].ToString());
                    }
                    if (Session["dayOfWeekID"] != null)
                    {
                        b.frequencyID = int.Parse(Session["dayOfWeekID"].ToString());
                    } 
                    b.startTime = TimeSpan.ParseExact(newVals["startTime"].ToString(), "hh\\:mm", System.Globalization.CultureInfo.CurrentCulture);
                    b.endTime = TimeSpan.ParseExact(newVals["endTime"].ToString(), "hh\\:mm", System.Globalization.CultureInfo.CurrentCulture);
                    var notificationID = (int)Session["notificationID"];
                    b.notificationID = notificationID;

                    le.notificationSchedules.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "notificationRecipientID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.notificationRecipient b = new coreLogic.notificationRecipient();
                    if (Session["staffID"] != null)
                    {
                        b.staffID = int.Parse(Session["staffID"].ToString());
                    }
                    b.email = newVals["email"].ToString();
                    var notificationID = (int)Session["notificationID"];
                    b.notificationID = notificationID;

                    le.notificationRecipients.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }  
            }
            catch (Exception ex) {  }
        }

        public string GetStaffName(object staffID)
        {
            if (staffID != null)
            {
                int id = (int)staffID;
                var staff = le.staffs.FirstOrDefault(p => p.staffID == id);
                if (staff != null)
                {
                    return staff.surName + ", " + staff.otherNames;
                }
            }

            return "";
        }

        public string GetDayOfWeek(object dayOfWeekID)
        {
            if (dayOfWeekID != null)
            {
                int id = (int)dayOfWeekID;
                Dictionary<int, string> dict = new Dictionary<int, string>();
                dict.Add(0, "Sunday");
                dict.Add(1, "Monday");
                dict.Add(2, "Tuesday");
                dict.Add(3, "Wednesday");
                dict.Add(4, "Thursday");
                dict.Add(5, "Friday");
                dict.Add(6, "Saturday");

                var dow = dict[id];
                return dow;
            }

            return "";
        }


        public string GetFrequency(object frequencyID)
        {
            if (frequencyID != null)
            {
                int id = (int)frequencyID;
                Dictionary<int, string> dict = new Dictionary<int, string>();
                dict.Add(0, "On-Demand");
                dict.Add(1, "Hourly");
                dict.Add(2, "Daily");
                dict.Add(3, "Weekly");
                dict.Add(4, "Monthly"); 

                var dow = dict[id];
                return dow;
            }

            return "";
        }
    }
}
