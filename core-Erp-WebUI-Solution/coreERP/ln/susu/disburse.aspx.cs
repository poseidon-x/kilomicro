using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;

namespace coreERP.ln.susu
{
    public partial class disburse : System.Web.UI.Page
    {
        private coreLoansEntities le = new coreLoansEntities();
        private core_dbEntities ent = new core_dbEntities();
        private coreLogic.susuAccount sa;
        private coreLogic.client client;
        coreLogic.loan ln;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtDisbursed.SelectedDate = DateTime.Now;
                this.cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }

                foreach (var r in le.modeOfPayments.Where(p=>p.modeOfPaymentID<=3))
                {
                    cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                }
                 
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.staffNo + ")", r.staffID.ToString()));
                }

                cboAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.agents.OrderBy(p => p.surName))
                {
                    cboAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.agentNo + ")", r.agentID.ToString()));
                }

                cboSusuGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuGroups.OrderBy(p => p.susuGroupNo))
                {
                    cboSusuGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuGroupName, r.susuGroupID.ToString()));
                }

                cboSusuGrade.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuGrades.OrderBy(p=> p.susuGradeNo))
                {
                    cboSusuGrade.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuGradeName, r.susuGradeID.ToString()));
                }

                cboSusuPosition.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.susuPositions.OrderBy(p => p.susuPositionNo))
                {
                    cboSusuPosition.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuPositionName, r.susuPositionID.ToString()));
                }

                Session["id"] = null;
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    LoadAccount(id);
                    if (sa != null)
                    {
                        txtAmountDisbursed.Value = sa.netAmountEntitled;
                        txtAmountEntitled.Value = sa.amountEntitled;
                        txtContribAmount.Value = sa.contributionAmount;
                        cboSusuGrade.SelectedValue = sa.susuGradeID.ToString();
                        cboSusuPosition.SelectedValue = sa.susuPositionID.ToString();
                        dtAppDate.SelectedDate = sa.applicationDate;
                        dtAppDate.Enabled = false;
                        if (sa.approvalDate != null) dtApproval.SelectedDate = sa.approvalDate;
                        dtDisbursed.SelectedDate = sa.disbursementDate;
                        dtDue.SelectedDate = sa.dueDate;
                        dtEntitled.SelectedDate = sa.entitledDate;
                        txtNetAmount.Value = sa.netAmountEntitled;
                        txtInterestDed.Value = sa.interestAmount;
                        txtCommission.Value = sa.commissionAmount;
                        if (dtDisbursed.SelectedDate != null)
                        {
                            lblStatus.Text = "Susu Account Disbursed";
                        }
                        else if(sa.approvalDate!=null)
                        {
                            lblStatus.Text = "Susu Account Approved";
                        }
                        else
                        {
                            lblStatus.Text = "Susu Account Application Received";
                        }
                        if (sa.susuGroupID != null)
                        {
                            cboSusuGroup.SelectedValue = sa.susuGroupID.ToString();
                        }
                        cboClient.SelectedValue = sa.clientID.ToString();
                        cboClient.Items.Clear();
                        cboClient.Items.Add(new RadComboBoxItem(
                            (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName +
                                ", " + client.otherNames + " (" + client.accountNumber + ")", client.clientID.ToString()));
                        cboClient.SelectedIndex = 0;
                        txtAccountNo.Text = client.accountNumber;
                        if (sa.client.clientTypeID == 6)
                        {
                            pnlJoint.Visible = true;
                            pnlRegular.Visible = false;
                            txtJointAccountName.Text = sa.client.accountName;
                        }
                        else
                        {
                            pnlJoint.Visible = false;
                            pnlRegular.Visible = true;
                            txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                client.companyName : client.surName;
                            txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                " " : client.otherNames;
                        }
                        cboClient.Enabled = false;
                        if (sa.staffID != null)
                        {
                            cboStaff.SelectedValue = sa.staffID.ToString();
                        }
                        if (sa.agentID != null)
                        {
                            cboAgent.SelectedValue = sa.agentID.ToString();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "New Account Application";
                    }
                }
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadAccount(id);
            }
        }

        private void LoadAccount(int? id)
        {
            if (id != null)
            {
                sa = le.susuAccounts.FirstOrDefault(p => p.susuAccountID == id);
                if (sa != null)
                {
                    //sa.clientReference.Load();
                    //sa.//client.clientImages.Load();
                    client = sa.client;
                    Session["loan.cl"] = client;
                }
            }
            else
            {
                sa = new coreLogic.susuAccount();
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                }
            }
            Session["susuAccount"] = sa;
            RenderImages();
        }

        protected void cboSusuGrade_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        protected void cboSusuPosition_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            if (cboSusuGrade.SelectedValue != "" && cboSusuPosition.SelectedValue != "")
            {
                int gradeID = int.Parse(cboSusuGrade.SelectedValue);
                int positionID = int.Parse(cboSusuPosition.SelectedValue);

                var g = le.susuGrades.FirstOrDefault(p => p.susuGradeID == gradeID);
                var pos = le.susuPositions.FirstOrDefault(p => p.susuPositionID == positionID);
                var conf = le.susuConfigs.FirstOrDefault();
                if (g != null && pos != null && conf != null)
                {
                    txtAmountEntitled.Value = conf.periodsInCycle * g.contributionAmount * conf.daysInPeriod;
                    txtCommission.Value = conf.daysDeductedPerPeriod * g.contributionAmount * conf.periodsInCycle;
                    txtInterestDed.Value = (txtAmountEntitled.Value.Value - txtCommission.Value.Value) * (pos.percentageInterest / 100.0);
                    txtNetAmount.Value = txtAmountEntitled.Value.Value - txtInterestDed.Value.Value - txtCommission.Value.Value
                        ;
                    if (dtAppDate.SelectedDate != null)
                    {
                        dtEntitled.SelectedDate = ComputeEntitledDate(pos.susuPositionID, dtAppDate.SelectedDate.Value);
                        if (pos.susuPositionNo == 6)
                        {
                            dtDue.SelectedDate = dtEntitled.SelectedDate;
                        }
                        else
                        {
                            dtDue.SelectedDate = ComputeDueDate(pos.susuPositionID, dtAppDate.SelectedDate.Value);
                        }
                    }
                    txtContribAmount.Value = g.contributionAmount;
                }
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            int? catID = -1; 
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else
                clients = le.clients.Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID == 3 || item.clientTypeID == 4 || item.clientTypeID == 5) ? item.companyName : ((item.clientTypeID == 6) ? item.accountName : item.surName + ", " + item.otherNames) + " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        protected void cboClient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                if (client != null)
                {
                    Session["loan.cl"] = client; 
                    //client.clientAddresses.Load();
                    rotator1.Items.Clear();
                    //client.clientImages.Load();
                    foreach (var r in client.clientImages)
                    {
                        //r.imageReference.Load();
                    }
                    if (client.clientTypeID == 6)
                    {
                        pnlJoint.Visible = true;
                        pnlRegular.Visible = false;
                        txtJointAccountName.Text = client.accountName;
                        txtAccountNo.Text = client.accountNumber;
                    }
                    else
                    {
                        pnlJoint.Visible = false;
                        pnlRegular.Visible = true;
                        txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName;
                        txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            " " : client.otherNames;
                        txtAccountNo.Text = client.accountNumber;
                    }

                    RenderImages();
                }
            }
        }

        private void RenderImages()
        {
            if (client != null && client.clientImages != null)
            {
                foreach (var item in client.clientImages)
                {
                    //item.imageReference.Load();
                    RadBinaryImage img = new RadBinaryImage();
                    img.Width = 216;
                    img.Height = 216;
                    img.ResizeMode = BinaryImageResizeMode.Fill;
                    img.DataValue = item.image.image1;
                    RadRotatorItem it = new RadRotatorItem();
                    it.Controls.Add(img);
                    rotator1.Items.Add(it);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (client != null && cboSusuGrade.SelectedValue != "" && cboSusuPosition.SelectedValue != "" && dtAppDate.SelectedDate != null
                && cboSusuGroup.SelectedValue != ""
                && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1"))
                && sa.disbursementDate==null)
            {
                if (sa.authorized == true)
                {
                    if (txtNetAmount.Value != null
                        && txtNetAmount.Value.Value >= txtAmountDisbursed.Value.Value && dtDisbursed.SelectedDate != null
                        && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1")))
                    {
                        var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                        if (ct == null)
                        {
                            HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                            return;
                        }
                        var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtDisbursed.SelectedDate.Value
                            && p.open == true);
                        if (ctd == null)
                        {
                            HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                            return;
                        }
                        sa.applicationDate = dtAppDate.SelectedDate.Value;
                        sa.amountEntitled = txtAmountEntitled.Value.Value;
                        if (sa.susuAccountID <= 0)
                        {
                            return;
                        }
                        sa.dueDate = dtDue.SelectedDate;
                        sa.entitledDate = dtEntitled.SelectedDate;
                        sa.amountEntitled = txtAmountEntitled.Value.Value;
                        sa.susuGradeID = int.Parse(cboSusuGrade.SelectedValue);
                        sa.susuPositionID = int.Parse(cboSusuPosition.SelectedValue);
                        sa.susuGroupID = int.Parse(cboSusuGroup.SelectedValue);
                        sa.approvalDate = dtApproval.SelectedDate;
                        sa.disbursementDate = dtDisbursed.SelectedDate;
                        sa.disbursedBy = User.Identity.Name;
                        sa.netAmountEntitled = txtNetAmount.Value.Value;
                        sa.modeOfPaymentID = int.Parse(cboPaymentType.SelectedValue);
                        sa.checkNo = txtCheckNo.Text;
                        if (cboBank.SelectedValue != "")
                        {
                            sa.bankID = int.Parse(cboBank.SelectedValue);
                        }

                        if (cboStaff.SelectedValue != "")
                        {
                            sa.staffID = int.Parse(cboStaff.SelectedValue);
                        }
                        if (cboAgent.SelectedValue != "")
                        {
                            sa.agentID = int.Parse(cboAgent.SelectedValue);
                        }
                        sa.contributionAmount = txtContribAmount.Value.Value;
                        le.SaveChanges();
                        HtmlHelper.MessageBox2("Susu Account Authorized Successfully", ResolveUrl("~/ln/susu/default.aspx"), "coreERP powered by ACS", IconType.ok);

                    }
                }


               //     if(sa.exited ==false)
                        
                if (sa.disbursementDate == null)
                {
                    var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                    if (ct == null)
                    {
                        HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                        return;
                    }
                    var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtDisbursed.SelectedDate.Value
                        && p.open == true);
                    if (ctd == null)
                    {
                        HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                        return;
                    }
                    sa.applicationDate = dtAppDate.SelectedDate.Value;
                    sa.amountEntitled = txtAmountEntitled.Value.Value;
                    if (sa.susuAccountID <= 0)
                    {
                        return;
                    }
                    sa.dueDate = dtDue.SelectedDate;
                    sa.entitledDate = dtEntitled.SelectedDate;
                    sa.amountEntitled = txtAmountEntitled.Value.Value;
                    sa.susuGradeID = int.Parse(cboSusuGrade.SelectedValue);
                    sa.susuPositionID = int.Parse(cboSusuPosition.SelectedValue);
                    sa.susuGroupID = int.Parse(cboSusuGroup.SelectedValue);
                    sa.approvalDate = dtApproval.SelectedDate;
                    sa.disbursementDate = dtDisbursed.SelectedDate;
                    sa.disbursedBy = User.Identity.Name;
                    sa.netAmountEntitled = txtNetAmount.Value.Value; 
                    sa.modeOfPaymentID = int.Parse(cboPaymentType.SelectedValue);
                    sa.checkNo = txtCheckNo.Text;  
                    if (cboBank.SelectedValue != "")
                    {
                        sa.bankID = int.Parse(cboBank.SelectedValue);
                    }

                    if (cboStaff.SelectedValue != "")
                    {
                        sa.staffID = int.Parse(cboStaff.SelectedValue);
                    }
                    if (cboAgent.SelectedValue != "")
                    {
                        sa.agentID = int.Parse(cboAgent.SelectedValue);
                    }
                    sa.contributionAmount = txtContribAmount.Value.Value;
                    le.SaveChanges();
                    HtmlHelper.MessageBox2("Susu Account Disbursed Successfully", ResolveUrl("~/ln/susu/default.aspx"), "coreERP powered by ACS", IconType.ok);
                }

            }
        }

        private DateTime? ComputeDueDate(int positionID, DateTime startDate)
        {
            DateTime? dt = null;

            var pos = le.susuPositions.FirstOrDefault(p => p.susuPositionID == positionID);
            var conf = le.susuConfigs.FirstOrDefault();
            if (pos != null && conf != null)
            {
                dt = startDate;
                for (int i = 0; i < conf.periodsInCycle; i++ )
                {
                    int j = 1;
                    while (true)
                    {
                        if (dt.Value.DayOfWeek == DayOfWeek.Saturday && conf.excludeSaturdays == true) { dt = dt.Value.AddDays(1); continue; }
                        if (dt.Value.DayOfWeek == DayOfWeek.Sunday && conf.excludeSundays == true) { dt = dt.Value.AddDays(1); continue; }
                        j = j + 1;
                        if (j > conf.daysInPeriod) break;
                        dt = dt.Value.AddDays(1);
                    }
                }
            }

            return dt;
        }

        private DateTime? ComputeEntitledDate(int positionID, DateTime startDate)
        {
            DateTime? dt = null;

            var pos = le.susuPositions.FirstOrDefault(p => p.susuPositionID == positionID);
            var conf = le.susuConfigs.FirstOrDefault();
            if (pos != null && conf != null)
            {
                dt = startDate;
                int j = 1;
                while (true)
                {
                    var d = dt.Value.AddDays(1);
                    dt = d;
                    if (d.DayOfWeek == DayOfWeek.Saturday && conf.excludeSaturdays == true) continue;
                    if (d.DayOfWeek == DayOfWeek.Sunday && conf.excludeSundays == true) continue;
                    j = j + 1;
                    if (j > pos.noOfWaitingPeriods * conf.daysInPeriod) break;
                }
            }

            return dt;
        }

        private string GetRomanNumeral(int no)
        {
            var rn = "";

            var nums = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

            if (no > nums.Length) rn = no.ToString();
            else rn = nums[no - 1];

            return rn;
        }
    }
}