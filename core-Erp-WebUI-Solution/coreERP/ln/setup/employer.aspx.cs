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
    public partial class employer : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/employer.aspx"; }
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "employerDirectorID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var employerID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["employerID"];
                    Session["employerID"] = employerID;
                    newVals["description"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "employerDepartmentID")
                {
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    e.Canceled = true;
                    var pItem = e.Item.OwnerTableView.ParentItem;
                    var employerID = (int)pItem.OwnerTableView.DataKeyValues[pItem.ItemIndex]["employerID"];
                    Session["employerID"] = employerID;
                    newVals["departmentName"] = string.Empty;
                    e.Item.OwnerTableView.InsertItem(newVals);
                }
                else
                {
                    e.Canceled = true;
                    var newVals = new System.Collections.Specialized.ListDictionary();
                    newVals["employerName"] = string.Empty;
                    newVals["officeNumber"] = "";
                    newVals["address.addressLine1"] = "";
                    newVals["address.cityTown"] = "";
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
            this.RadGrid1.ExportSettings.FileName = "coreERP_employers";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_employers";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_employers";
            this.RadGrid1.ExportSettings.Pdf.Title = "employeres Defined in System";
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
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true && e.Item.OwnerTableView.DataKeyNames.Contains("employerID"))
            {
                var item = e.Item as GridEditableItem;
                var rcb = item["employmentTypeID"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateETs(rcb);
                    rcb.ValidationGroup = "employmentTypeID";
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "employerID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var employerID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["employerID"];
                    var b = le.employers.FirstOrDefault(p => p.employerID == employerID);
                    b.employerName = newVals["employerName"].ToString(); 
                    if (Session["employmentTypeID"] != null)
                    {
                        b.employmentTypeID = int.Parse(Session["employmentTypeID"].ToString());
                    }
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
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "employerDirectorID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var employerDirectorID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["employerDirectorID"];
                    var b = le.employerDirectors.FirstOrDefault(p => p.employerDirectorID == employerDirectorID);
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
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "employerDepartmentID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    var employerDepartmentID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["employerDepartmentID"];
                    var b = le.employerDepartments.FirstOrDefault(p => p.employerDepartmentID == employerDepartmentID);
                    b.departmentName = newVals["departmentName"].ToString(); 
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
                if (e.Item.OwnerTableView.DataKeyNames[0] == "employerID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.employer b = new coreLogic.employer();
                    b.employerName = newVals["employerName"].ToString();
                    if (Session["employmentTypeID"] != null)
                    {
                        b.employmentTypeID = int.Parse(Session["employmentTypeID"].ToString());
                    }
                    if (newVals["address.addressLine1"].ToString() != "")
                    {
                        if (b.address == null)
                        {
                            b.address = new address();
                        }
                        b.address.addressLine1 = newVals["address.addressLine1"].ToString();
                        b.address.cityTown = newVals["address.cityTown"].ToString();
                    }

                    le.employers.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "employerDirectorID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.employerDirector b = new coreLogic.employerDirector();
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
                    var employerID = (int)Session["employerID"];
                    b.employerID = employerID;

                    le.employerDirectors.Add(b);
                    le.SaveChanges();

                    RadGrid1.EditIndexes.Clear();
                    e.Item.OwnerTableView.IsItemInserted = false;
                    e.Item.OwnerTableView.Rebind();
                }
                else if (e.Item.OwnerTableView.DataKeyNames[0] == "employerDepartmentID")
                {
                    Hashtable newVals = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                    e.Canceled = true;
                    coreLogic.employerDepartment b = new coreLogic.employerDepartment();
                    b.departmentName = newVals["departmentName"].ToString(); 
                    var employerID = (int)Session["employerID"];
                    b.employerID = employerID;

                    le.employerDepartments.Add(b);
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
