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
    public partial class approve : System.Web.UI.Page
    {
        private coreLoansEntities le = new coreLoansEntities();
        private coreLogic.susuAccount sa;
        private coreLogic.client client;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtApproval.SelectedDate=DateTime.Now;
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

                        cboSusuGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Start New Group", "-100"));
                        foreach (var r in le.susuGroups.OrderBy(p => p.susuGroupNo))
                        {
                            var sa2 = le.susuAccounts
                                .FirstOrDefault(p => p.susuAccountID != sa.susuAccountID && 
                                    p.susuGroupID == r.susuGroupID
                                    && p.susuPositionID == sa.susuPositionID
                                    && p.susuGradeID == sa.susuGradeID);
                            if (sa2 == null)
                            {
                                cboSusuGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuGroupName, r.susuGroupID.ToString()));
                            }
                        } 

                        txtAmountDisbursed.Value = sa.amountTaken;
                        txtAmountEntitled.Value = sa.amountEntitled;
                        txtContribAmount.Value = sa.contributionAmount;
                        cboSusuGrade.SelectedValue = sa.susuGradeID.ToString();
                        cboSusuPosition.SelectedValue = sa.susuPositionID.ToString();
                        dtAppDate.SelectedDate = sa.applicationDate;
                        dtAppDate.Enabled = false;
                        if (sa.approvalDate != null) dtApproval.SelectedDate = sa.approvalDate;
                        else dtApproval.SelectedDate = sa.applicationDate;
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
                    cboSusuGroup.Items.Clear();
                    cboSusuGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Start New Group", "-100"));
                    foreach (var r in le.susuGroups.OrderBy(p => p.susuGroupNo))
                    {
                        var prev = (
                                    from a in le.susuAccounts
                                    from g2 in le.susuGroups
                                    where a.susuGroupID == g2.susuGroupID
                                        && a.susuGradeID == g.susuGradeID
                                    select g2
                                    ).FirstOrDefault();
                        if (prev == null)
                        {
                            cboSusuGroup.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.susuGroupName, r.susuGroupID.ToString()));
                        }
                    } 

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
                && cboSusuGroup.SelectedValue!="")
            {
                if (sa.approvalDate == null)
                {
                    int groupID = int.Parse(cboSusuGroup.SelectedValue);
                    int gradeID = int.Parse(cboSusuGrade.SelectedValue);
                    int positionID = int.Parse(cboSusuPosition.SelectedValue);
                    var sae = le.susuAccounts.FirstOrDefault(p => p.susuPositionID == positionID && p.susuGradeID == gradeID && 
                        p.susuGroupID == groupID);
                    if (sae != null)
                    {
                        HtmlHelper.MessageBox("The selected position and grade has already been assigned for this group.",
                            "coreERP©: Failed", IconType.deny);
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
                    var group = le.susuGroups.FirstOrDefault(p => p.susuGroupID == groupID);
                    if (group == null)
                    {
                        group = le.susuGroups
                            .Where(p => p.susuAccounts
                                .Where(q => q.susuPositionID == positionID && q.susuGradeID == gradeID).FirstOrDefault() == null)
                           .FirstOrDefault();
                    }
                    if (group == null)
                    {
                        var groupNo = 1;
                        if (le.susuGroups.Count() > 0)
                        {
                            groupNo = le.susuGroups.Max(p => p.susuGroupNo) + 1;
                        }
                        group = new susuGroup
                        {
                            susuGroupName = "Group " + groupNo.ToString(),
                            susuGroupNo = groupNo
                        };
                        le.susuGroups.Add(group);
                    }
                    sa.susuGroup = group;
                    sa.approvalDate = dtApproval.SelectedDate;
                    sa.approvedBy = User.Identity.Name;
                    sa.netAmountEntitled = txtNetAmount.Value.Value;
                    sa.interestAmount = txtInterestDed.Value.Value;

                    if (cboStaff.SelectedValue != "")
                    {
                        sa.staffID = int.Parse(cboStaff.SelectedValue);
                    }
                    if (cboAgent.SelectedValue != "")
                    {
                        sa.agentID = int.Parse(cboAgent.SelectedValue);
                    }
                    sa.contributionAmount = txtContribAmount.Value.Value;
                    CalculateSchedule();
                    le.SaveChanges();
                    HtmlHelper.MessageBox2("Susu Account Approved Successfully", ResolveUrl("~/ln/susu/default.aspx"), "coreERP powered by ACS", IconType.ok);
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

        private void CalculateSchedule()
        {
            DateTime? dt = null;
            var startDate = sa.approvalDate;
            for (int k = sa.susuContributionSchedules.Count - 1; k >= 0; k--)
            {
                sa.susuContributionSchedules.Remove(sa.susuContributionSchedules.ToArray()[k]);
            }
            var pos = le.susuPositions.FirstOrDefault(p => p.susuPositionID == sa.susuPositionID);
            var conf = le.susuConfigs.FirstOrDefault();
            if (pos != null && conf != null)
            {
                dt = startDate;
                for (int i = 0; i < conf.periodsInCycle; i++)
                {
                    int j = 1;
                    while (true)
                    {
                        var d = dt.Value; 
                        dt = dt.Value.AddDays(1);
                        if (d.DayOfWeek == DayOfWeek.Saturday && conf.excludeSaturdays == true) continue;
                        if (d.DayOfWeek == DayOfWeek.Sunday && conf.excludeSundays == true) continue;
                        sa.susuContributionSchedules.Add(new susuContributionSchedule
                        {
                            amount = sa.contributionAmount,
                            plannedContributionDate = d,
                            balance = sa.contributionAmount
                        });
                        j = j + 1;
                        if (j > conf.daysInPeriod) break;
                    }
                }
            }
        }
    }
}