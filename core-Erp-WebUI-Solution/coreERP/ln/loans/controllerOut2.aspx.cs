using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.ln.loans
{
    public partial class controllerOut2 : System.Web.UI.Page
    {
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            {   
                cboFile.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.controllerFiles.OrderByDescending(p=>p.fileMonth))
                {
                    cboFile.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.fileMonth.ToString() + " | "
                        + r.fileName ,
                        r.fileID.ToString()));
                }

                txtMonth.Value = int.Parse(DateTime.Now.Date.AddMonths(-1).ToString("yyyyMM"));
                txtMonth.Text = DateTime.Now.Date.AddMonths(-1).ToString("yyyyMM");
                txtMonth.NumberFormat.GroupSeparator = "";
                txtMonth.NumberFormat.DecimalDigits = 0;
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (Session["remarks"] != null)
            {
                var fileID = (int)Session["fileID"];
                var remarks = Session["remarks"].ToString();
                if (remarks == "Over Deduction")
                {
                    remarks = "Exact Match";
                    btnPrev.Enabled = false;
                }
                else if (remarks == "Under Deduction")
                {
                    remarks = "Over Deduction";
                }
                else if (remarks == "Duplicate Deduction")
                {
                    remarks = "Under Deduction";
                }
                else if (remarks == "No Match")
                {
                    remarks = "Duplicate Deduction";
                    btnNext3.Enabled = true;
                    btnPost.Enabled = true;
                }
                else if (remarks == "Zero Deduction")
                {
                    remarks = "No Match";
                    btnNext3.Enabled = true;
                }
                h2Label.InnerHtml = "Please verify the details of the <u>"+remarks+"</u> below and post them";
                Session["remarks"] = remarks;
                EntityDataSource2.WhereParameters[0].DefaultValue = fileID.ToString();
                EntityDataSource2.WhereParameters[1].DefaultValue = remarks;
                multi1.SelectedIndex = 2;
                RadGrid2.DataBind();
            }
        }

        protected void btnNext3_Click(object sender, EventArgs e)
        {
            if (Session["remarks"] != null)
            {
                var fileID = (int)Session["fileID"];
                var remarks = Session["remarks"].ToString();
                if (remarks == "Exact Match")
                {
                    remarks = "Over Deduction";
                    btnPrev.Enabled = true;
                }
                else if (remarks == "Over Deduction")
                {
                    remarks = "Under Deduction";
                }
                else if (remarks == "Under Deduction")
                {
                    remarks = "Duplicate Deduction";
                    btnPost.Enabled = false;
                }
                else if (remarks == "Duplicate Deduction")
                {
                    remarks = "No Match"; 
                }
                else if (remarks == "No Match")
                {
                    remarks = "Zero Deduction";
                    btnNext3.Enabled = false;
                }
                h2Label.InnerHtml = "Please verify the details of the <u>" + remarks + "</u> below and post them";
                Session["remarks"] = remarks;
                EntityDataSource2.WhereParameters[0].DefaultValue = fileID.ToString();
                EntityDataSource2.WhereParameters[1].DefaultValue = remarks;
                multi1.SelectedIndex = 2;
                RadGrid2.DataBind();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var remarks = Session["remarks"];
            if (remarks != null)
            {
                var fileID = (int)Session["fileID"];
                RadGrid2.ExportSettings.ExportOnlyData = true;
                RadGrid2.ExportSettings.IgnorePaging = true;
                RadGrid2.ExportSettings.OpenInNewWindow = true;
                this.RadGrid2.ExportSettings.FileName = fileID.ToString() + " - " + remarks.ToString();
                RadGrid2.MasterTableView.ExportToExcel();
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (Session["remarks"] != null)
            {
                var fileID = (int)Session["fileID"];
                var remarks = Session["remarks"].ToString();

                coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();

                coreData.ErrorLog.Logger.informationLog(txtMonth.Text);

                var date = (new DateTime(int.Parse(txtMonth.Text.Substring(0, 4)),
                    int.Parse(txtMonth.Text.Substring(4, 2)),
                    DateTime.DaysInMonth(int.Parse(txtMonth.Text.Substring(0, 4)), int.Parse(txtMonth.Text.Substring(4, 2)))));

                coreData.ErrorLog.Logger.informationLog(date.ToString("dd-MMM-yyyy"));

                var changed = false;
                var file = le.controllerFiles.FirstOrDefault(p => p.fileID == fileID); 
                var details = file.controllerFileDetails.Where(p => p.remarks == remarks && p.authorized == false).ToList();
                coreLogic.jnl_batch batch = null;
                foreach (var detail in details)
                { 
                    if (detail.repaymentSchedule != null)
                    { 
                        if (!CheckIfSelected(detail.fileDetailID)) continue;

                        var amount = detail.monthlyDeduction;
                        if (detail.overage > 0) amount -= detail.overage;

                        rpmtMgr.ReceivePayment(le, detail.repaymentSchedule.loan,
                            amount, date, null, "", "", ent, User.Identity.Name,
                            1, detail.repaymentSchedule.loan.loanType.holdingAccountID,
                            detail.repaymentSchedule, ref batch);
                        if (batch.jnl.Count == 4 || batch.jnl.Count == 2)
                        {
                            var j = batch.jnl.FirstOrDefault(p => p.accts.acct_id == detail.repaymentSchedule.loan.loanType.holdingAccountID);
                            if (j != null)
                            {
                                j.description = "Payroll Loan Deductions - " + detail.controllerFile.fileMonth.ToString()
                                    + " (" + detail.remarks + ")";
                            }
                        }
                        detail.authorized = true;
                        changed = true;
                    }
                }

                if (changed == true)
                {
                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox(remarks + " Posted Successfuly!");
                }

                EntityDataSource2.WhereParameters[0].DefaultValue = fileID.ToString();
                EntityDataSource2.WhereParameters[1].DefaultValue = remarks;
                multi1.SelectedIndex = 2;
                RadGrid2.DataBind();
            }
        }

        protected void btnNext2_Click(object sender, EventArgs e)
        {
            if (Session["fileID"] != null)
            {
                var fileID = (int)Session["fileID"];
                var remarks = "Exact Match";
                btnExport.Enabled = true;
                h2Label.InnerHtml = "Please verify the details of the <b>" + remarks + "</b> below and post them";
                Session["remarks"] = remarks;
                EntityDataSource2.WhereParameters[0].DefaultValue = fileID.ToString();
                EntityDataSource2.WhereParameters[1].DefaultValue = remarks;
                multi1.SelectedIndex = 2;
                RadGrid2.DataBind();
            }
        }

        protected void btnNext1_Click(object sender, EventArgs e)
        {
            
        }

        protected void cboFile_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (cboFile.SelectedValue != "")
            {
                int fileID = int.Parse(cboFile.SelectedValue);
                var file = le.controllerFiles.FirstOrDefault(p => p.fileID == fileID);
                if (file == null)
                {
                    HtmlHelper.MessageBox2("File Not Found", "~/ln/loans/controllerOut.aspx", "coreERP: Failed", IconType.deny);
                    return;
                }

                coreData.ErrorLog.Logger.informationLog(file.fileMonth.ToString());
                txtMonth.Value = int.Parse(file.fileMonth.ToString());
                txtMonth.Text = file.fileMonth.ToString();
                //f//ile.controllerFileDetails.Load();
                var totalDeductions = file.controllerFileDetails.Sum(p => p.monthlyDeduction);

                le.SaveChanges();
                ent.SaveChanges();
                EntityDataSource1.WhereParameters[0].DefaultValue = file.fileID.ToString();
                Session["fileID"] = file.fileID;
                multi1.SelectedIndex = 1;
                RadGrid1.DataBind();
            }
        }

        private bool CheckIfSelected(int detailID)
        {
            bool selected = false;
            try
            {
                foreach (GridDataItem dataItem in RadGrid2.MasterTableView.Items)
                {
                    if (dataItem.GetDataKeyValue("fileDetailID").ToString() == detailID.ToString())
                    {
                        var ck = dataItem.FindControl("CheckBox1") as CheckBox;
                        if (ck != null)
                        {
                            if (ck.Checked == true)
                            {
                                selected = true;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception) { }

            return selected;
        }

        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            try
            {
                var ck = (sender as CheckBox);
                if (ck != null)
                {
                    (ck.NamingContainer as GridItem).Selected = ck.Checked;
                    bool checkHeader = true;
                    foreach (GridDataItem dataItem in RadGrid2.MasterTableView.Items)
                    {
                        var chk = (dataItem.FindControl("CheckBox1") as CheckBox);
                        if (chk != null && !chk.Checked)
                        {
                            checkHeader = false;
                            break;
                        }
                    }
                    GridHeaderItem headerItem = RadGrid2.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                    (headerItem.FindControl("headerChkbox") as CheckBox).Checked = checkHeader;
                }
            }
            catch (Exception) { }
        }

        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            try
            {
                GridHeaderItem headerItem = RadGrid2.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                var headerCheckBox = (headerItem.FindControl("headerChkbox") as CheckBox);
                if (headerCheckBox == null) headerCheckBox = (sender as CheckBox);
                if (headerCheckBox != null)
                {
                    foreach (GridDataItem dataItem in RadGrid2.MasterTableView.Items)
                    {
                        var chk = (dataItem.FindControl("CheckBox1") as CheckBox);
                        if (chk != null)
                        {
                            chk.Checked = headerCheckBox.Checked;
                            dataItem.Selected = headerCheckBox.Checked;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        protected void SwapLoans(int key)
        {
            try
            {
                using (var _le = new coreLoansEntities())
                {
                    var detail = _le.controllerFileDetails.First(p => p.fileDetailID == key);
                    var allClientDetails = _le.controllerFileDetails
                        .Where(p => p.fileID == detail.fileID
                                    && p.staffID == detail.staffID)
                        .ToList()
                        .OrderBy(p => new Random().Next())
                        .ToList();

                    controllerFileDetail workingDetail = null;

                    for (int i = 0; i < allClientDetails.Count; i++)
                    {
                        controllerFileDetail thisDetail = allClientDetails[i];
                        if (workingDetail == null)
                        {
                            workingDetail = thisDetail;
                            continue;
                        }
                        var rsId = thisDetail.repaymentScheduleID;
                        var remarks = thisDetail.remarks;
                        var overage = thisDetail.overage;
                        var amount = thisDetail.repaymentSchedule.interestBalance +
                                     thisDetail.repaymentSchedule.principalBalance;
                        var amount2 = workingDetail.repaymentSchedule.interestBalance +
                                     workingDetail.repaymentSchedule.principalBalance;

                        thisDetail.remarks = workingDetail.remarks;
                        thisDetail.repaymentScheduleID = workingDetail.repaymentScheduleID;
                        thisDetail.overage = thisDetail.monthlyDeduction - amount2;

                        workingDetail.repaymentScheduleID = rsId;
                        workingDetail.remarks = remarks;
                        workingDetail.overage = workingDetail.monthlyDeduction - amount;

                        if (thisDetail.overage > 1)
                        {
                            thisDetail.remarks = "Over Deduction";
                        }
                        else if (thisDetail.overage < -1)
                        {
                            thisDetail.remarks = "Under Deduction";
                        }

                        if (workingDetail.overage > 1)
                        {
                            workingDetail.remarks = "Over Deduction";
                        }
                        else if (workingDetail.overage < -1)
                        {
                            workingDetail.remarks = "Under Deduction";
                        }
 
                        workingDetail = thisDetail;
                    }

                    _le.SaveChanges();
                    RadGrid1.DataBind();
                    RadGrid2.DataBind();
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        protected void RadGrid1_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Swap")
            {
                var item = e.Item as GridDataItem;
                var key = int.Parse(item.GetDataKeyValue("fileDetailID").ToString());
                SwapLoans(key);
            }
        }

        protected void RadGrid2_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Swap")
            {
                var item = e.Item as GridDataItem;
                var key = int.Parse(item.GetDataKeyValue("fileDetailID").ToString());
                SwapLoans(key);
            }
        }
    }
}