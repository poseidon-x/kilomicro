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
    public partial class group : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/group.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "groupExecID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var groupID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["groupID"];
                    Session["groupID"] = groupID;
                    newVals["description"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["groupName"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_groups";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_groups";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_groups";
            this.RadGrid1.ExportSettings.Pdf.Title = "groupes Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        public string GetGroupType(object id)
        {
            if (id != null)
            {
                if (id.ToString() == "1") return "Formal";
                if (id.ToString() == "2") return "Informal";
            }

            return "";
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item.OwnerTableView.DataKeyNames[0] == "groupID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var groupID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["groupID"];
                    var b = le.groups.FirstOrDefault(p => p.groupID == groupID);
                    b.groupName = newVals["groupName"].ToString();
                    if (newVals["groupSize"] != null) b.groupSize = int.Parse(newVals["groupSize"].ToString());
                    if (newVals["groupTypeID"] != null) b.groupTypeID = int.Parse(newVals["groupTypeID"].ToString());
                    //b.addressReference.Load();
                    if (newVals["address.addressLine1"].ToString() != "")
                    {
                        if (b.address == null)
                        {
                            b.address = new address();
                        }
                        b.address.addressLine1 = newVals["address.addressLine1"].ToString();
                        b.address.cityTown = newVals["address.cityTown"].ToString();
                    }
                    le.SaveChanges();
                    RadGrid1.EditIndexes.Clear();
                    RadGrid1.MasterTableView.IsItemInserted = false;
                    RadGrid1.MasterTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "groupExecID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var groupExecID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["groupExecID"];
                    var b = le.groupExecs.FirstOrDefault(p => p.groupExecID == groupExecID);
                    b.surName = newVals["surName"].ToString();
                    b.otherNames = newVals["otherNames"].ToString();
                    //b.phoneReference.Load();
                    if (newVals["phone.phoneNo"].ToString() != "")
                    {
                        if (b.phone == null)
                        {
                            b.phone = new phone();
                        }
                        b.phone.phoneNo = newVals["phone.phoneNo"].ToString(); 
                    }
                    //b.emailReference.Load();
                    if (newVals["email.emailAddress"].ToString() != "")
                    {
                        if (b.email == null)
                        {
                            b.email = new email();
                        }
                        b.email.emailAddress = newVals["email.emailAddress"].ToString();
                    }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "groupID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.group b = new coreLogic.group();
                    b.groupName = newVals["groupName"].ToString();
                    if (newVals["groupSize"] != null) b.groupSize = int.Parse(newVals["groupSize"].ToString());
                    if (newVals["groupTypeID"] != null) b.groupTypeID = int.Parse(newVals["groupTypeID"].ToString());
                    if (newVals["address.addressLine1"].ToString() != "")
                    {
                        if (b.address == null)
                        {
                            b.address = new address();
                        }
                        b.address.addressLine1 = newVals["address.addressLine1"].ToString();
                        b.address.cityTown = newVals["address.cityTown"].ToString();
                    }

                    le.groups.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "groupExecID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.groupExec b = new coreLogic.groupExec();
                    b.surName = newVals["surName"].ToString();
                    b.otherNames = newVals["otherNames"].ToString();
                    if (newVals["phone.phoneNo"].ToString() != "")
                    {
                        if (b.phone == null)
                        {
                            b.phone = new phone();
                        }
                        b.phone.phoneNo = newVals["phone.phoneNo"].ToString();
                    }
                    if (newVals["email.emailAddress"].ToString() != "")
                    {
                        if (b.email == null)
                        {
                            b.email = new email();
                        }
                        b.email.emailAddress = newVals["email.emailAddress"].ToString();
                    }
                    var groupID = (int)Session["groupID"];
                    b.groupID = groupID;

                    le.groupExecs.Add(b);
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
