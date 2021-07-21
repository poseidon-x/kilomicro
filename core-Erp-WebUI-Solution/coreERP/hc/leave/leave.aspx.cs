using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.hc.leave
{
    public partial class leave : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.staff staff;
        coreLogic.staffLeave ln; 

        protected void Page_Load(object sender, EventArgs e)
        { 
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {
                le = new coreLogic.coreLoansEntities();
                Session["le"] = le;

                cboLeaveType.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.leaveTypes.OrderBy(p => p.leaveTypeName))
                {
                    cboLeaveType.Items.Add(new RadComboBoxItem(r.leaveTypeName, r.leaveTypeID.ToString()));
                }
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    ln = le.staffLeaves.FirstOrDefault(p => p.staffLeaveID == id);

                    if (ln != null)
                    { 
                        //ln.staffReference.Load();
                        
                        staff = ln.staff;
                        //staff.staffAddresses.Load();
                        //staff.staffImages.Load();
                        foreach (var r in staff.staffImages)
                        {
                            //r.imageReference.Load();
                        }
                        Session["staffLeave.cl"] = staff;

                        txtLeaveDays.Value = ln.daysApplied;
                        cboLeaveType.SelectedValue = ln.leaveTypeID.ToString();                         
                        cboStaff.SelectedValue = ln.staffID.ToString();
                        dtAppDate.SelectedDate = ln.applicationDate;
                        dtpLeaveStartsDate.SelectedDate = ln.startDate;
                        cboStaff.Items.Clear();
                        cboStaff.Items.Add(new RadComboBoxItem(staff.surName + ", " + staff.otherNames + " ("
                            + staff.staffNo + ")", staff.staffID.ToString()));
                        cboStaff.SelectedValue = ln.staffID.ToString();
                        txtSurname.Text = staff.surName;
                        txtOtherNames.Text = staff.otherNames;
                        txtAccountNo.Text = staff.staffNo; 
                         
                        RenderImages();
                    } 

                    Session["staffLeave"] = ln;
                } 
                else
                {
                    ln = new coreLogic.staffLeave();
                    dtAppDate.SelectedDate = DateTime.Now;
                    Session["staffLeave"] = ln; 
                } 
            }
            else
            {
                if (Session["staffLeave.cl"] != null)
                {
                    staff = Session["staffLeave.cl"] as coreLogic.staff;
                }
                if (Session["staffLeave"] != null)
                {
                    ln = Session["staffLeave"] as coreLogic.staffLeave;
                }
                else
                {
                    ln = new coreLogic.staffLeave();
                    Session["staffLeave"] = ln;
                }
                le = Session["le"] as coreLogic.coreLoansEntities;
            }  
        }

        protected void cboStaff_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboStaff.SelectedValue != "")
            {
                int staffID = int.Parse(cboStaff.SelectedValue);
                staff = le.staffs.FirstOrDefault(p => p.staffID == staffID);
                if (staff != null)
                {
                    Session["staffLeave.cl"] = staff; 
                    //staff.staffAddresses.Load();
                    rotator1.Items.Clear();

                    RenderImages();
                }
            }
        }
         
        private void RenderImages()
        {
            if (staff.staffImages != null)
            {
                foreach (var item in staff.staffImages)
                {
                    //item.imageReference.Load();
                    RadBinaryImage img = new RadBinaryImage();
                    img.Width = 209;
                    img.Height = 113;
                    img.ResizeMode = BinaryImageResizeMode.Fit;
                    img.DataValue = item.image.image1;
                    RadRotatorItem it = new RadRotatorItem();
                    it.Controls.Add(img);
                    rotator1.Items.Add(it);
                }
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        { 
            List<coreLogic.staff> staffs = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.staffNo.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                staffs = le.staffs.Where(p => p.staffNo.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.surName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                staffs = le.staffs.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).ToList();
            else
                staffs = le.staffs.ToList();
            cboStaff.Items.Clear();
            cboStaff.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in staffs)
            {
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(item.surName + ", " + item.otherNames + " (" + item.staffNo + ")", item.staffID.ToString()));
            }
        }
         
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtLeaveDays.Value != null   
                && cboLeaveType.SelectedValue != "" 
                && dtAppDate.SelectedDate != null
                && dtpLeaveStartsDate.SelectedDate!= null)
            {
                if (Save() == true)
                {
                    HtmlHelper.MessageBox2("Leave of Absence Data Saved Successfully!", ResolveUrl("~/hc/leave/default.aspx"), "coreERP©: Successful", IconType.ok);
                }
            }
        }
         
        private bool Save()
        {
            if (txtLeaveDays.Value != null
                && cboLeaveType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && dtpLeaveStartsDate.SelectedDate != null)
            {
                if (ln.managerApproved==false)
                {
                    ln.daysApplied = (int)txtLeaveDays.Value.Value;
                    ln.applicationDate = dtAppDate.SelectedDate.Value;
                    if (ln.staffID <= 0)
                    {
                        ln.staffID = staff.staffID;
                    } 
                    ln.leaveTypeID = int.Parse(cboLeaveType.SelectedValue );
                    ln.startDate = dtpLeaveStartsDate.SelectedDate;
                    var i = ln.daysApplied;
                    var date = ln.startDate.Value;
                    while (i > 0)
                    {
                        date = date.AddDays(1);
                        if (date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday)
                        {
                            i = i - 1;
                        }
                    }
                    ln.endDate = date;

                    if (ln.staffLeaveID <= 0)
                    {
                        le.staffLeaves.Add(ln);
                    } 
                }  
                le.SaveChanges();
                ent.SaveChanges();
                 
                Session["staffLeave"] = ln;
                return true;
            }

            return false;
        }   
    }
}