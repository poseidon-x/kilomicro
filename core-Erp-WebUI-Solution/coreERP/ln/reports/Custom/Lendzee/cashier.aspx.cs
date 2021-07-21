using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using coreReports;
using coreLogic;
using coreReports.Models;

namespace coreERP.ln.reports
{
    public partial class cashierLendzee : Page
    {
        coreSecurityEntities ctx = new coreSecurityEntities();
        ReportDocument rpt;
        private DateTime start;
        private DateTime end;
        IcoreLoansEntities le;

        public string URL
        {
            get { return "~/ln/reports/cashierLendzee.aspx"; }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            
            if (rpt != null)
            {
                try
                {
                    rpt.Dispose();
                    rpt.Close();
                    rpt = null;
                }
                catch (Exception) { }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["endDate"] = null;
                dtpEndDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 23, 59, 59)).AddMonths(1).AddDays(-1);
                dtpStartDate.SelectedDate = (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    1, 0, 0, 0));
                
                coreLoansEntities le = new coreLoansEntities();
                coreSecurityEntities sec = new coreSecurityEntities();
                cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in sec.users.OrderBy(p => p.full_name))
                {
                    if (le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == r.user_name.ToLower()) != null)
                    {
                        cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.full_name + " (" + r.user_name + ")", r.user_name));
                    }
                }

                cboFieldAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.agents.OrderBy(p => p.surName).ThenBy(p=> p.otherNames))
                {
                    cboFieldAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " (" + r.agentNo + ")", 
                        r.surName + ", " + r.otherNames));
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            Session["resCashier"] = null;
            Session["userName"] = cboUserName.SelectedValue;
            Session["all"] = "N";
            Bind(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, cboUserName.SelectedValue);   
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                DateTime endDate = dtpEndDate.SelectedDate.Value;
                DateTime startDate = dtpStartDate.SelectedDate.Value;
                string userName = cboUserName.SelectedValue;
                if (Session["all"] != "N")
                {
                    BindAll(startDate, endDate, userName);
                }
                else
                {
                    Bind(startDate, endDate, userName);
                }
            }
        }

        private void Bind(DateTime startDate, DateTime endDate, string userName)
        {
           // var loginOfficer = User.Identity.Name.Trim().ToLower();

            startDate = dtpStartDate.SelectedDate.Value.Date;
            if((endDate - startDate).TotalDays > 31)
            {
                HtmlHelper.MessageBox("Report cannot be viewed for more than a month");
            }
            //dtpEndDate.SelectedDate=dtp//
            rpt = new coreReports.Custom.Lendzzee.rptCashierLendzee();  
            var rent=(new reportEntities());
            rent.Database.CommandTimeout = 300000;
            var resRep = rent.vwCashierRepaymentsGroupeds.Where(p =>
                     (p.repaymentDate >= dtpStartDate.SelectedDate && p.repaymentDate <= dtpEndDate.SelectedDate)
                     && (userName.Trim() =="" ||p.userName == userName) &&(cboFieldAgent.SelectedValue == "" || p.agentName == cboFieldAgent.SelectedValue)
                     && ((p.repaymentTypeName == "Interest Only" || p.repaymentTypeName == "Principal and Interest"
                         || p.repaymentTypeName == "Principal Only" || p.repaymentTypeName == "Penalty"))
                 ).Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            
            var clientServiceRep = rent.vwCashierRepaymentsGroupeds.Where(p =>
                     (p.repaymentDate >= dtpStartDate.SelectedDate && p.repaymentDate <= dtpEndDate.SelectedDate)
                     && ((p.repaymentTypeName == "Client Service Charge"))
                 ).Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            var resSusu = rent.vwCashierRepayments.Where(p =>
                    (p.repaymentDate >= dtpStartDate.SelectedDate && p.repaymentDate <= dtpEndDate.SelectedDate)
                    && (cboFieldAgent.SelectedValue == "" || p.agentName == cboFieldAgent.SelectedValue)
                    && ((p.repaymentTypeName == "Group Susu Contribution" || p.repaymentTypeName == "Normal Susu Contribution"))
                )
                .Where(p => p.posted)
                .OrderBy(p => p.repaymentDate)
                .ToList();
            var resFees = rent.vwCashierRepayments.Where(p =>
                    (p.repaymentDate >= dtpStartDate.SelectedDate && p.repaymentDate <= dtpEndDate.SelectedDate)
                    && ((p.repaymentTypeName == "Processing Fee" || p.repaymentTypeName == "Application Fee"
                    || p.repaymentTypeName == "Commission") || (p.feePaid > 0))
                ).Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                .OrderBy(p => p.repaymentDate)
                .ToList();
            var resDisb = rent.vwCashierDisbs.Where(p => p.disbursementDate >= dtpStartDate.SelectedDate
                && p.disbursementDate <= dtpEndDate.SelectedDate)
                .Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                .ToList();
            var resDA = rent.vwDepositAdditionals.Where(p => p.depositDate >= dtpStartDate.SelectedDate
                && p.depositDate <= dtpEndDate.SelectedDate)
                .Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p=> p.posted)
                .ToList();
            var resDW = rent.vwDepositWithdrawals.Where(p => p.withdrawalDate >= dtpStartDate.SelectedDate
                && p.withdrawalDate <= dtpEndDate.SelectedDate)
                .Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                .ToList();
            var resSA = rent.vwSavingAdditionals.Where(p => p.savingDate >= dtpStartDate.SelectedDate
                && p.savingDate <= dtpEndDate.SelectedDate)
                .Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                .ToList();
            var resSW = rent.vwSavingWithdrawals.Where(p => p.withdrawalDate >= dtpStartDate.SelectedDate
                && p.withdrawalDate <= dtpEndDate.SelectedDate)
                .Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                .ToList();
            var le = new coreLoansEntities();
            le.Database.CommandTimeout = 3000;
            int? selectedCashierStaffId = null;
            var selectedStaff = le.staffs.FirstOrDefault(s => s.userName.ToLower().Trim() == userName.ToLower().Trim());
            selectedCashierStaffId = selectedStaff?.staffID;
            var selectedStaffsLoanGroup = le.loanGroupClients
                .Where(lc => lc.loanGroup.relationsOfficerStaffId == selectedCashierStaffId)
                .Select(c => c.loanGroupId)
                .ToList();
            var endOfDayDate = dtpEndDate.SelectedDate.Value;
            endOfDayDate = endOfDayDate.Date.AddDays(1).AddSeconds(-1);
            var resOR = rent.spGetLoanArrears(endOfDayDate).Where(p => (p.Payable.Value) - ((p.Paid ?? 0) + (p.WriteOffAmount ?? 0)) > 0
            ).ToList()
            .Where(p => selectedCashierStaffId == null || selectedStaffsLoanGroup.Any(lc => lc == p.loanGroupId))
                .Select(p => new OutstandingRepaymentModel
                {
                    outstanding = (p.Payable ?? 0.0) - ((p.Paid ?? 0) + (p.WriteOffAmount ?? 0)),
                    clientName = p.clientName,
                    loanNo = p.loanNo,
                    loanGroupName = p.loanGroupName,
                    LastRepaymentDate = p.LastRepaymentDate == null ? DateTime.Now : p.LastRepaymentDate.Value,
                    LastDueDate = p.LastDueDate == null ? DateTime.Now : p.LastDueDate.Value,
                    disbursementDate = p.disbursementDate == null ? DateTime.Now : p.disbursementDate.Value,
                    amountDisbursed = p.amountDisbursed == null ? 0 : p.amountDisbursed.Value,
                    Payable = p.Payable == null ? 0 : p.Payable.Value,
                    Paid = p.Paid == null ? 0 : p.Paid.Value
                })
                .Where(p => p.outstanding >= 1)
                .OrderBy(p => p.loanGroupName)
                .ToList();

            rpt.SetDataSource(resRep);
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierSusuContrib.rpt")
                {
                    sr.SetDataSource(resSusu);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptOutstandingRepayment.rpt")
                {
                    sr.SetDataSource(resOR);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierClientServiceCharge.rpt")
                {
                    sr.SetDataSource(clientServiceRep);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierFees.rpt")
                {
                    sr.SetDataSource(resFees); 
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierDisb.rpt")
                {
                    sr.SetDataSource(resDisb); 
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierDepositAdd.rpt")
                {
                    sr.SetDataSource(resDA); 
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierDepositWith.rpt")
                {
                    sr.SetDataSource(resDW); 
                    break;
                }
            }

            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierSavingAdd.rpt")
                {
                    sr.SetDataSource(resSA); 
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierSavingWith.rpt")
                {
                    sr.SetDataSource(resSW); 
                    break;
                }
            }
            foreach (Section section in rpt.ReportDefinition.Sections)
            {
                foreach (object item in section.ReportObjects)
                {
                    SubreportObject subReport = item as SubreportObject;
                    if (subReport != null)
                    {
                        if (subReport.Name == "fees" && resFees.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "disb" && resDisb.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "da" && resDA.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "dw" && resDW.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "sa" && resSA.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "sw" && resSW.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "susu" && resSusu.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                    }
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptProfile.rpt")
                {
                    sr.SetDataSource(rent.vwCompProfs.ToList()); 
                    break;
                }
            }  
            //rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Detailed Cashier Report between "+startDate.ToString("dd-MMM-yyyy")
                +" and " +endDate.ToString("dd-MMM-yyyy"));
            rpt.SetParameterValue("cashierName", cboUserName.Text);
            this.rvw.ReportSource = rpt;
        }

        private void BindAll(DateTime startDate, DateTime endDate, string userName)
        {
            DateTime startDat = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
            DateTime endDat = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            if ((endDate - startDate).TotalDays > 31)
            {
                HtmlHelper.MessageBox("Report cannot be viewed for more than a month");
            }
            rpt = new coreReports.Custom.Lendzzee.rptCashierLendzee();
            var rent = (new reportEntities());
            rent.Database.CommandTimeout = 3000;
            var resRep = rent.vwCashierRepaymentsGroupeds
                .Where(p =>
                     (p.repaymentDate >= startDat && p.repaymentDate <= endDat) 
                     && (cboFieldAgent.SelectedValue == "" || p.agentName == cboFieldAgent.SelectedValue)
                     && ((p.repaymentTypeName == "Interest Only" || p.repaymentTypeName == "Principal and Interest"
                     || p.repaymentTypeName == "Principal Only" || p.repaymentTypeName == "Penalty"))
                 )
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            var user = ctx.users.Include(p => p.user_roles).FirstOrDefault(p => p.user_name == User.Identity.Name.ToLower().Trim());

            var authRoles = new List<string> { "admin", "itAdmin", "Admin. Officer", "manager", "loanAdmin", "DeptManager", "Accountant" };
            if (user.user_roles.Any(q => authRoles.Contains(q.roles.role_name)))
            {
                resRep = resRep
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            }
            else
            {
                resRep = resRep
                .Where(p => p.userName.ToLower().Trim() == userName.ToLower().Trim() || user.user_roles.Any(q => authRoles.Contains(q.roles.role_name)))
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            }
            rent.Database.Log = r => System.Diagnostics.Debug.WriteLine(r);
            var clientServiceRep1 = rent.vwCashierRepaymentsGroupeds//.Where(p =>
                     //(p.repaymentDate >= dtpStartDate.SelectedDate && p.repaymentDate <= dtpEndDate.SelectedDate)
                     //&& 
                     //((p.repaymentTypeName == "Client Service Charge"))
                 //)
                 //.Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList(); 

            var clientServiceRep = rent.vwCashierRepaymentsGroupeds.Where(p =>
                     (p.repaymentDate >= dtpStartDate.SelectedDate && p.repaymentDate <= dtpEndDate.SelectedDate)
                     && ((p.repaymentTypeName == "Client Service Charge"))
                 ).Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim())
                .Where(p => p.posted)
                 .OrderBy(p => p.repaymentDate)
                 .ToList();
            
            var resSusu = rent.vwCashierRepayments.Where(p =>
                    (p.repaymentDate >= dtpStartDate.SelectedDate && p.repaymentDate <= dtpEndDate.SelectedDate) 
                    && (cboFieldAgent.SelectedValue == "" || p.agentName == cboFieldAgent.SelectedValue)
                    && ((p.repaymentTypeName == "Group Susu Contribution" || p.repaymentTypeName == "Normal Susu Contribution"))
                )
                .OrderBy(p => p.repaymentDate)
                .ToList();
            var resFees = rent.vwCashierRepayments.Where(p =>
                    (p.repaymentDate >= startDat && p.repaymentDate <= endDat)
                    && ((p.repaymentTypeName == "Processing Fee" || p.repaymentTypeName == "Application Fee"
                    || p.repaymentTypeName == "Commission") || (p.feePaid > 0))
                ).Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim()).OrderBy(p => p.repaymentDate).ToList();
            var resDisb = rent.vwCashierDisbs.Where(p => p.disbursementDate >= startDat
                && p.disbursementDate <= endDat)
                .Where(p => userName.Trim() == "" || p.userName.ToLower().Trim() == userName.ToLower().Trim()).ToList();
            var resDA = rent.vwDepositAdditionals
                .Where(p => p.depositDate >= startDat
                && p.depositDate <= endDat).Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim()).ToList();
            var resDW = rent.vwDepositWithdrawals.Where(p => p.withdrawalDate >= startDat
                && p.withdrawalDate <= endDat).Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim()).ToList();
            var resSA = rent.vwSavingAdditionals.Where(p => p.savingDate >= startDat
                && p.savingDate <= endDat).Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim()).ToList();
            var resSW = rent.vwSavingWithdrawals.Where(p => p.withdrawalDate >= startDat
                && p.withdrawalDate <= endDat).Where(p => userName.Trim() == "" || p.creator.ToLower().Trim() == userName.ToLower().Trim()).ToList();
            //if (res.Count == 0 && res2.Count==0)
            //{
            //    //HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
            //    //return;
            //}
            var le = new coreLoansEntities();
            le.Database.CommandTimeout = 3000;
            int? selectedCashierStaffId = null;
            var selectedStaff = le.staffs.FirstOrDefault(s => s.userName.ToLower().Trim() == userName.ToLower().Trim());
            selectedCashierStaffId = selectedStaff?.staffID;
            var selectedStaffsLoanGroup = le.loanGroupClients
                .Where(lc => lc.loanGroup.relationsOfficerStaffId == selectedCashierStaffId)
                .Select(c => c.loanGroupId)
                .ToList();
            var endOfDayDate = dtpEndDate.SelectedDate.Value;
            endOfDayDate = endOfDayDate.Date.AddDays(1).AddSeconds(-1);
            var resOR = rent.spGetLoanArrears(endOfDayDate).Where(p => (p.Payable.Value) - ((p.Paid ?? 0) + (p.WriteOffAmount ?? 0)) > 0
            ).ToList()
            .Where(p => selectedCashierStaffId == null || selectedStaffsLoanGroup.Any(lc => lc == p.loanGroupId))
                .Select(p => new OutstandingRepaymentModel
                {
                    outstanding = (p.Payable ?? 0.0) - ((p.Paid ?? 0) + (p.WriteOffAmount ?? 0)),
                    clientName = p.clientName,
                    loanNo = p.loanNo,
                    loanGroupName = p.loanGroupName,
                    LastRepaymentDate = p.LastRepaymentDate == null ? DateTime.Now : p.LastRepaymentDate.Value,
                    LastDueDate = p.LastDueDate == null ? DateTime.Now : p.LastDueDate.Value,
                    disbursementDate = p.disbursementDate == null ? DateTime.Now : p.disbursementDate.Value,
                    amountDisbursed = p.amountDisbursed == null ? 0 : p.amountDisbursed.Value,
                    Payable = p.Payable == null ? 0 : p.Payable.Value,
                    Paid = p.Paid == null ? 0 : p.Paid.Value
                })
                .Where(p => p.outstanding >= 1)
                .OrderBy(p => p.loanGroupName)
                .ToList();

            
            rpt.SetDataSource(resRep);
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierSusuContrib.rpt")
                {
                    sr.SetDataSource(resSusu);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptOutstandingRepayment.rpt")
                {
                    sr.SetDataSource(resOR);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierClientServiceCharge.rpt")
                {
                    sr.SetDataSource(clientServiceRep);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierFees.rpt")
                {
                    sr.SetDataSource(resFees);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierDisb.rpt")
                {
                    sr.SetDataSource(resDisb);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierDepositAdd.rpt")
                {
                    sr.SetDataSource(resDA);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierDepositWith.rpt")
                {
                    sr.SetDataSource(resDW);
                    break;
                }
            }

            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierSavingAdd.rpt")
                {
                    sr.SetDataSource(resSA);
                    break;
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptCashierSavingWith.rpt")
                {
                    sr.SetDataSource(resSW);
                    break;
                }
            }
            foreach (Section section in rpt.ReportDefinition.Sections)
            {
                foreach (object item in section.ReportObjects)
                {
                    SubreportObject subReport = item as SubreportObject;
                    if (subReport != null)
                    {
                        if (subReport.Name == "fees" && resFees.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "disb" && resDisb.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "da" && resDA.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "dw" && resDW.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "sa" && resSA.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "sw" && resSW.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                        if (subReport.Name == "susu" && resSusu.Count == 0)
                        {
                            subReport.ObjectFormat.EnableSuppress = true;
                        }
                    }
                }
            }
            foreach (ReportDocument sr in rpt.Subreports)
            {
                if (sr.Name == "rptProfile.rpt")
                {
                    sr.SetDataSource(rent.vwCompProfs.ToList());
                    break;
                }
            }
            //rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", "Detailed Cashier Report between " + dtpStartDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                + " and " + dtpEndDate.SelectedDate.Value.ToString("dd-MMM-yyyy"));
            rpt.SetParameterValue("cashierName", cboUserName.Text);
            this.rvw.ReportSource = rpt;
        }
        
        protected void dtTransactionDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        { 
        }

        protected void btnRunAll_Click(object sender, EventArgs e)
        {
            Session["endDate"] = dtpEndDate.SelectedDate;
            Session["startDate"] = dtpStartDate.SelectedDate;
            Session["resCashier"] = null;
            Session["userName"] = cboUserName.SelectedValue;
            Session["all"] = "Y";
            BindAll(dtpStartDate.SelectedDate.Value, dtpEndDate.SelectedDate.Value, cboUserName.SelectedValue);  
        }
    }
}
