using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using coreLogic;
using coreLogic.Models.Loans;
using Microsoft.Office.Interop.Excel;
using Telerik.Web.UI;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Page = System.Web.UI.Page;

namespace coreERP.ln.loans
{
    public partial class controllerOut : Page
    {
        private List<selectedRecordsVM> selectedRecords = new List<selectedRecordsVM>();
        RepaymentsManagerLink rpmtMgr = new RepaymentsManagerLink();
        protected void Page_Load(object sender, EventArgs e)
        {
            core_dbEntities ent = new core_dbEntities();
            coreLoansEntities le = new coreLoansEntities();
            if (!Page.IsPostBack)
            {
                txtMonth.Value = int.Parse(DateTime.Now.Date.AddMonths(-1).ToString("yyyyMM"));
                txtMonth.Text = DateTime.Now.Date.AddMonths(-1).ToString("yyyyMM");
                txtMonth.NumberFormat.GroupSeparator = "";
                txtMonth.NumberFormat.DecimalDigits = 0;
            }
            loadSelectedRecords();
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
                h2Label.InnerHtml = "Please verify the details of the <u>" + remarks + "</u> below and post them";
                Session["remarks"] = remarks;

                EntityDataSource2.WhereParameters[0].DefaultValue = fileID.ToString();
                EntityDataSource2.WhereParameters[1].DefaultValue = remarks;
                multi1.SelectedIndex = 2;
                checkAndSelectAllPreviouslySelected();
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
                checkAndSelectAllPreviouslySelected();
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

                coreLoansEntities le = new coreLoansEntities();
                core_dbEntities ent = new core_dbEntities();

                coreData.ErrorLog.Logger.informationLog(txtMonth.Text);

                var date = (new DateTime(int.Parse(txtMonth.Text.Substring(0, 4)),
                    int.Parse(txtMonth.Text.Substring(4, 2)), 
                    DateTime.DaysInMonth(int.Parse(txtMonth.Text.Substring(0, 4)), int.Parse(txtMonth.Text.Substring(4, 2)))));

                coreData.ErrorLog.Logger.informationLog(date.ToString("dd-MMM-yyyy"));

                var changed = false;
                var file = le.controllerFiles.FirstOrDefault(p => p.fileID == fileID);
                if (file != null)
                {
                    var details = file.controllerFileDetails.Where(p => p.remarks == remarks && p.authorized == false).ToList();
                    jnl_batch batch = null;
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
                checkAndSelectAllPreviouslySelected();
            }
        }

        protected void btnNext1_Click(object sender, EventArgs e)
        {
            coreLoansEntities le = new coreLoansEntities();
            core_dbEntities ent = new core_dbEntities();
            var loanNum = "";

            if (upload1.UploadedFiles.Count == 1)
            {
                var fileName = upload1.UploadedFiles[0].FileName;
                var f = le.controllerFiles.FirstOrDefault(p => p.fileName.ToLower().Trim() == fileName.ToLower().Trim());
                if (f != null)
                {
                    HtmlHelper.MessageBox2("This file has already been uploaded!", ResolveUrl("~/ln/loans/controllerOut.aspx"));
                    return;
                }
                int mon = (int)txtMonth.Value.Value;

                

                f = le.controllerFiles.FirstOrDefault(p => p.fileMonth == mon);
                if (f != null)
                {
                    //HtmlHelper.MessageBox("This month has already been uploaded!", "~/ln/loans/controllerOut.aspx");
                    //return;
                }
                Stream s = upload1.UploadedFiles[0].InputStream;
                StreamReader sr = new StreamReader(s);
                var line = "";
                var started = false;
                int i = 0;
                List<ControllerOutViewModel> FileDetails = new List<ControllerOutViewModel>();
                string month;
                controllerFile file = new controllerFile
                {
                    fileName = upload1.UploadedFiles[0].FileName,
                    fileMonth = mon,
                    uploadDate = DateTime.Now
                };
                month = file.fileMonth.ToString();
                while (sr.EndOfStream == false)
                {
                    line = sr.ReadLine();
                    if (!started)
                    {
                        //if (++i >= 16) 
                        started = true;
                    }

                    var list = line.Split(',');
                    if (started && list.Length >= 5 && list[2].Trim() != "" && list[5].Trim() != "")
                    {
                        var mgtUnit = list[0];
                        var staffID = list[2];
                        var oldStaffID = list[1];
                        var name = list[3];
                        var balBF = list[4];
                        var monthDed = list[5];
                        loanNum = list.Length>6 ? list[6] : "";
                         


                        if (mgtUnit != "" && staffID != "" && name != "" && balBF != "" && monthDed != "")
                        {
                            var detail = new controllerFileDetail
                            {
                                balBF = double.Parse(balBF),
                                employeeName = name,
                                managementUnit = mgtUnit,
                                loanNo = loanNum,
                                monthlyDeduction = double.Parse(monthDed),
                                oldID = oldStaffID,
                                staffID = staffID,
                                remarks = ""
                            };
                            file.controllerFileDetails.Add(detail);

                            var dt = new ControllerOutViewModel
                            {
                                mgtUnit = mgtUnit,
                                oldStaffID = oldStaffID,
                                staffID = staffID,
                                loanNum = loanNum,
                                name = name,
                                balBF = balBF,
                                monthDed = monthDed
                            };
                            FileDetails.Add(dt);
                        }
                    }
                }
                var date = (new DateTime(int.Parse(txtMonth.Text.Substring(0, 4)),
                int.Parse(txtMonth.Text.Substring(4, 2)), 1)).AddMonths(1).AddSeconds(-1);
                if (file.controllerFileDetails.Count > 0)
                {
                    

                    var staffIDs = (from d in file.controllerFileDetails
                                    select d.staffID).Distinct();
                    foreach (var staffID in staffIDs)
                    {
                        int rsID1 = 0;
                        int rsID2 = 0;
                        int rsID3 = 0;
                        int rsID4 = 0;
                        int rsID5 = 0;
                        var amts = (from d in file.controllerFileDetails
                                    where d.staffID == staffID
                                    select d.monthlyDeduction).ToList();
                        foreach (var amt in amts)
                        {

                            var details = file.controllerFileDetails.Where(p => p.staffID == staffID && p.monthlyDeduction == amt).ToList();
                            var cl = le.staffCategory1.FirstOrDefault(p => p.employeeNumber == staffID);
                            var date2 = date;

                            foreach (var detail in details.Where(p=>p.staffID==staffID).OrderByDescending(p => p.monthlyDeduction))
                            {
                                if (true /*CheckIfSelected(detail.fileDetailID)*/)
                                {
                                    if (cl != null)
                                    {
                                        var ln = le.loans.FirstOrDefault(p => p.loanNo.ToLower() == detail.loanNo.ToLower());
                                        var lowEnd = detail.monthlyDeduction * 0.9;
                                        var hiEnd = detail.monthlyDeduction * 1.1;
                                        var lnId = ln == null? -1: ln.loanID;


                                        repaymentSchedule rs;
                                        if (processWithLoanNumber(le, lnId, detail, details, 
                                            ref date2, ref rsID1, ref rsID2, ref rsID3, ref rsID4, ref rsID5)) continue;

                                        if (processExactWithoutLoanNumber(le, cl, detail, details, 
                                            ref date2, ref rsID1, ref rsID2, ref rsID3, ref rsID4, ref rsID5)) continue;

                                        if (processInExactWithoutLoanNumber(le, cl, detail, details, 
                                            ref date2, ref rsID1, ref rsID2, ref rsID3, ref rsID4, ref rsID5)) continue;

                                        if (processInExacPreviousWithoutLoanNumber(le, cl, detail, details, 
                                            ref date2, ref rsID1, ref rsID2, ref rsID3, ref rsID4, ref rsID5)) continue;
                                    }
                                    date2 = processNotFound(detail, details, date2);
                                }
                            }
                        }
                    }
                    le.controllerFiles.Add(file);

                    le.SaveChanges();
                    ent.SaveChanges();
                    EntityDataSource1.WhereParameters[0].DefaultValue = file.fileID.ToString();
                    Session["fileID"] = file.fileID;
                    multi1.SelectedIndex = 1;
                    RadGrid1.DataBind();
                }
            }
        }

        private static DateTime processNotFound(controllerFileDetail detail, List<controllerFileDetail> details, DateTime date2)
        {
            detail.notFound = true;
            if (detail.monthlyDeduction == 0)
            {
                detail.remarks = "Zero Deduction";
            }
            else if (details.Count > 1)
            {
                detail.remarks = "Duplicate Deduction";
            }
            else
            {
                detail.remarks = "No Match";
            }
            detail.overage = detail.monthlyDeduction;
            date2 = date2.AddMonths(-1);
            return date2;
        }

        private static bool processInExacPreviousWithoutLoanNumber(coreLoansEntities le, staffCategory1 cl,
            controllerFileDetail detail, List<controllerFileDetail> details, ref DateTime date2, ref int rsID1, ref int rsID2, ref int rsID3,
            ref int rsID4, ref int rsID5)
        {
            DateTime currentDate = date2;
            int rsId1 = rsID1;
            int rsId2 = rsID2;
            int rsId3 = rsID3;
            int rsId4 = rsID4;
            int rsId5 = rsID5;
            repaymentSchedule rs;
            rs = le.repaymentSchedules.Where(p => p.loan.clientID == cl.clientID
                                                  && detail.monthlyDeduction > 0
                                                  && p.interestBalance + p.principalBalance != detail.monthlyDeduction
                                                  && detail.monthlyDeduction > 0
                                                 // && p.repaymentDate <= currentDate 
                                                  && p.loan.loanStatusID == 4
                                                  && p.interestBalance + p.principalBalance > 0
                                                  && p.repaymentScheduleID != rsId1
                                                  && p.repaymentScheduleID != rsId2
                                                  && p.repaymentScheduleID != rsId3
                                                  && p.repaymentScheduleID != rsId4
                                                  && p.repaymentScheduleID != rsId5
                                                  && detail.monthlyDeduction > 0)
                .OrderByDescending(p => p.interestPayment + p.principalPayment)
                .ThenByDescending(p => p.repaymentDate)
                .FirstOrDefault();
            if (rs != null)
            {
                detail.repaymentScheduleID = rs.repaymentScheduleID;
                detail.overage = detail.monthlyDeduction - rs.interestBalance - rs.principalBalance;
                if (detail.overage > 0)
                {
                    detail.remarks = "Over Deduction";
                }
                else
                {
                    detail.remarks = "Under Deduction";
                }
                var scat = rs.loan.client.staffCategory1.First();
                if (details.Count(p => p.staffID == scat.employeeNumber.Trim()) == 1)
                {
                    date2 = rs.repaymentDate.AddMonths(-1);
                }
                if (rsID1 == 0) rsID1 = rs.repaymentScheduleID;
                else if (rsID2 == 0) rsID2 = rs.repaymentScheduleID;
                else if (rsID3 == 0) rsID3 = rs.repaymentScheduleID;
                else if (rsID4 == 0) rsID4 = rs.repaymentScheduleID;
                else if (rsID5 == 0) rsID5 = rs.repaymentScheduleID;
                return true;
            }
            return false;
        }

        private static bool processInExactWithoutLoanNumber(coreLoansEntities le, staffCategory1 cl, controllerFileDetail detail,
            List<controllerFileDetail> details, ref DateTime date2, ref int rsID1, ref int rsID2, ref int rsID3, ref int rsID4, ref int rsID5)
        {
            repaymentSchedule rs;
            DateTime currentDate = date2;
            int rsId1 = rsID1;
            int rsId2 = rsID2;
            int rsId3 = rsID3;
            int rsId4 = rsID4;
            int rsId5 = rsID5;
            rs = le.repaymentSchedules.Where(p => p.loan.clientID == cl.clientID
                                                  && detail.monthlyDeduction > 0
                                                  &&
                                                  Math.Abs(p.interestBalance + p.principalBalance - detail.monthlyDeduction) <
                                                  10
                                                  //&& p.repaymentDate <= currentDate 
                                                  && p.loan.loanStatusID == 4
                                                  && p.repaymentScheduleID != rsId1
                                                  && p.repaymentScheduleID != rsId2
                                                  && p.repaymentScheduleID != rsId3
                                                  && p.repaymentScheduleID != rsId4
                                                  && p.repaymentScheduleID != rsId5
                                                  && p.interestBalance + p.principalBalance > 0)
                .OrderByDescending(p => p.interestPayment + p.principalPayment)
                .ThenByDescending(p => p.repaymentDate)
                .FirstOrDefault();
            if (rs != null)
            {
                detail.repaymentScheduleID = rs.repaymentScheduleID;
                detail.overage = detail.monthlyDeduction - (rs.interestBalance + rs.principalBalance);




                double diff = (rs.interestBalance + rs.principalBalance) - detail.monthlyDeduction;
                if (detail.overage > 0)
                {
                    detail.remarks = "Over Deduction";
                }
                else
                {
                    detail.remarks = "Under Deduction";
                }
                var scat = rs.loan.client.staffCategory1.First();
                if (details.Count(p => p.staffID == scat.employeeNumber.Trim()) == 1)
                {
                    date2 = rs.repaymentDate.AddMonths(-1);
                }
                if (rsID1 == 0) rsID1 = rs.repaymentScheduleID;
                else if (rsID2 == 0) rsID2 = rs.repaymentScheduleID;
                else if (rsID3 == 0) rsID3 = rs.repaymentScheduleID;
                else if (rsID4 == 0) rsID4 = rs.repaymentScheduleID;
                else if (rsID5 == 0) rsID5 = rs.repaymentScheduleID;
                return true;
            }
            return false;
        }

        private static bool processExactWithoutLoanNumber(coreLoansEntities le, staffCategory1 cl, controllerFileDetail detail,
            List<controllerFileDetail> details, ref DateTime date2, ref int rsID1, ref int rsID2, ref int rsID3, ref int rsID4, ref int rsID5)
        {
            repaymentSchedule rs;
            DateTime currentDate = date2;
            int rsId1 = rsID1;
            int rsId2 = rsID2;
            int rsId3 = rsID3;
            int rsId4 = rsID4;
            int rsId5 = rsID5;
            rs = le.repaymentSchedules.Where(p => p.loan.clientID == cl.clientID
                                                  && detail.monthlyDeduction > 0
                                                  &&
                                                  Math.Abs(p.interestBalance + p.principalBalance - detail.monthlyDeduction) <=
                                                  1
                                                  //&& p.repaymentDate <= currentDate
                                                  && p.loan.loanStatusID == 4
                                                  && p.repaymentScheduleID != rsId1
                                                  && p.repaymentScheduleID != rsId2
                                                  && p.repaymentScheduleID != rsId3
                                                  && p.repaymentScheduleID != rsId4
                                                  && p.repaymentScheduleID != rsId5
                                                  && p.interestBalance + p.principalBalance > 0)
                .OrderByDescending(p => p.interestBalance + p.principalPayment)
                .ThenByDescending(p => p.repaymentDate)
                .FirstOrDefault();
            if (rs != null)
            {
                detail.repaymentScheduleID = rs.repaymentScheduleID;
                detail.overage = 0;
                detail.remarks = "Exact Match";
                var scat = rs.loan.client.staffCategory1.First();
                if (details.Count(p => p.staffID == scat.employeeNumber.Trim()) == 1)
                {
                    date2 = rs.repaymentDate.AddMonths(-1);
                }
                if (rsID1 == 0) rsID1 = rs.repaymentScheduleID;
                else if (rsID2 == 0) rsID2 = rs.repaymentScheduleID;
                else if (rsID3 == 0) rsID3 = rs.repaymentScheduleID;
                else if (rsID4 == 0) rsID4 = rs.repaymentScheduleID;
                else if (rsID5 == 0) rsID5 = rs.repaymentScheduleID;
                return true;
            }
            return false;
        }

        private static bool processWithLoanNumber(coreLoansEntities le, int lnId, controllerFileDetail detail, List<controllerFileDetail> details,
            ref DateTime date2, ref int rsID1, ref int rsID2, ref int rsID3, ref int rsID4, ref int rsID5)
        {
            DateTime currentDate = date2;

            var rs = le.repaymentSchedules
                .Where(p => p.loanID == lnId && (p.principalBalance > 1 || p.interestBalance > 1)
                            //&& p.repaymentDate <= currentDate 
                            && p.loan.loanStatusID == 4)
                .OrderByDescending(p => p.repaymentDate)
                .FirstOrDefault();
            if (rs != null)
            {
                detail.repaymentScheduleID = rs.repaymentScheduleID;
                detail.overage = detail.monthlyDeduction - (rs.interestBalance + rs.principalBalance);
                if (Math.Abs(detail.overage) >= 0 && Math.Abs(detail.overage) < 1)
                {
                    detail.remarks = "Exact Match";
                }
                else if (detail.overage > 1)
                {
                    detail.remarks = "Over Deduction";
                }
                else
                {
                    detail.remarks = "Under Deduction";
                }

                var scat = rs.loan.client.staffCategory1.First();
                if (details.Count(p => p.staffID == scat.employeeNumber.Trim()) == 1)
                {
                    date2 = rs.repaymentDate.AddMonths(-1);
                }
                if (rsID1 == 0) rsID1 = rs.repaymentScheduleID;
                else if (rsID2 == 0) rsID2 = rs.repaymentScheduleID;
                else if (rsID3 == 0) rsID3 = rs.repaymentScheduleID;
                else if (rsID4 == 0) rsID4 = rs.repaymentScheduleID;
                else if (rsID5 == 0) rsID5 = rs.repaymentScheduleID;
                return true;
            }
            return false;
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
                    var dataItem2 = (ck.NamingContainer as GridDataItem);
                    dataItem2.Selected = ck.Checked;
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

                    var fileDetailId = (int)dataItem2.GetDataKeyValue("fileDetailID");
                    var fileDetail = selectedRecords.FirstOrDefault(p => p.fileDetailId == fileDetailId);
                    if (fileDetail == null)
                    {
                        fileDetail = new selectedRecordsVM
                        {
                            fileDetailId = fileDetailId,
                            selected = ck.Checked
                        };
                        selectedRecords.Add(fileDetail);
                    }
                    else
                    {
                        fileDetail.selected = ck.Checked;
                    }
                    Session["selectedRecords"] = selectedRecords;
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
                            var fileDetailId = (int) dataItem.GetDataKeyValue("fileDetailID");
                            var fileDetail = selectedRecords.FirstOrDefault(p => p.fileDetailId == fileDetailId);
                            if (fileDetail == null)
                            {
                                fileDetail = new selectedRecordsVM
                                {
                                    fileDetailId = fileDetailId,
                                    selected = chk.Checked
                                };
                                selectedRecords.Add(fileDetail);
                            }
                            else
                            {
                                fileDetail.selected = chk.Checked;
                            }
                        }
                    }
                    Session["selectedRecords"] = selectedRecords;
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

        protected void RadGrid2_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Swap")
            {
                var item = e.Item as GridDataItem;
                var key = int.Parse(item.GetDataKeyValue("fileDetailID").ToString());
                SwapLoans(key);
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

        public static void CreateExcelWithConflictRecords(List<ControllerOutViewModel> dataToReturn, string month)
        {
            GC.Collect();

            var oExcel = new Application();
            oExcel.Visible = false;
            var oWorkBook = (_Workbook)(oExcel.Workbooks.Add(Missing.Value));
            var oSheet = (_Worksheet)oWorkBook.ActiveSheet;
            
            string[,] tmp = new string[dataToReturn.Count,7];

            for (int i=0; i<dataToReturn.Count; i++)
            {
                var record = dataToReturn[i];
                string[] tmp2 = new[]
                {
                    record.mgtUnit, record.oldStaffID, record.staffID, record.name, record.balBF,
                    record.monthDed, record.loanNum,
                };
                tmp[i, 0] = record.mgtUnit;
                tmp[i, 1] = record.oldStaffID;
                tmp[i, 2] = record.staffID;
                tmp[i, 3] = record.name;
                tmp[i, 4] = record.balBF;
                tmp[i, 5] = record.monthDed;
                tmp[i, 6] = record.loanNum;
            }


            string[,] Data = tmp;

            for (int j = 0; j < Data.GetLength(0); j++)
            {
                for (int k = 0; k < Data.GetLength(1); k++)
                {
                    oSheet.Cells[j + 1, k + 1] = Data.GetValue(j, k);
                }
            }
            string strFile = month + "Duplicates_" + DateTime.Now.Ticks.ToString() + ".csv";
            oWorkBook.SaveAs(@"C:\Users\Public\Documents\" + strFile,
                XlFileFormat.xlWorkbookNormal,
                null, null, false, false, XlSaveAsAccessMode.xlShared,
                false, false, null, null, null);

            oWorkBook.Close(null, null, null);
            oExcel.Workbooks.Close();
            oExcel.Quit();
            Marshal.ReleaseComObject(oExcel);
            Marshal.ReleaseComObject(oSheet);
            Marshal.ReleaseComObject(oWorkBook);
            oSheet = null;
            oWorkBook = null;
            oExcel = null;

            GC.Collect();

        }

        private void loadSelectedRecords()
        {
            if (Session["selectedRecords"] != null)
            {
                selectedRecords = Session["selectedRecords"] as List<selectedRecordsVM>;
            }
            else
            {
                selectedRecords = new List<selectedRecordsVM>();
            }
        }

        protected void RadGrid1_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            var dataItem = e.Item as GridDataItem;
            if (dataItem != null)
            {
                var fileDetailId = (int)dataItem.GetDataKeyValue("fileDetailID");
                var fileDetail = selectedRecords.FirstOrDefault(p => p.fileDetailId == fileDetailId);
                if (fileDetail != null)
                {
                    dataItem.Selected = fileDetail.selected;
                }
            }
        }

        private void checkAndSelectAllPreviouslySelected()
        {
            foreach (var item in RadGrid2.Items)
            {
                var dataItem = item as GridDataItem;
                if (dataItem != null)
                {
                    var fileDetailId = (int)dataItem.GetDataKeyValue("fileDetailID");
                    var fileDetail = selectedRecords.FirstOrDefault(p => p.fileDetailId == fileDetailId);
                    if (fileDetail != null)
                    {
                        dataItem.Selected = fileDetail.selected;
                        var chk = (dataItem.FindControl("CheckBox1") as CheckBox);
                        if (chk != null)
                        {
                            chk.Checked = fileDetail.selected;
                        }
                    }
                }
            }
        }


        protected void RadGrid1_OnDataBound(object sender, EventArgs e)
        {
        }

        protected void RadGrid1_OnPreRender(object sender, EventArgs e)
        {
            checkAndSelectAllPreviouslySelected();
        }
    }

    public class selectedRecordsVM
    {
        public int fileDetailId { get; set; }
        public bool selected { get; set; }

    }
}