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
    public partial class exit : System.Web.UI.Page
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
                    txtAmountEntitled.Value = conf.periodsInCycle * g.contributionAmount * conf.daysInPeriod;
                    txtCommission.Value = conf.daysDeductedPerPeriod * g.contributionAmount * conf.periodsInCycle;
                    txtInterestDed.Value = (txtAmountEntitled.Value.Value - txtCommission.Value.Value) * (pos.percentageInterest / 100.0);
                    txtNetAmount.Value = txtAmountEntitled.Value.Value - txtInterestDed.Value.Value - txtCommission.Value.Value;
                    if (dtAppDate.SelectedDate != null)
                    {
                        if (dtEntitled.SelectedDate == null)
                        {
                            dtEntitled.SelectedDate = ComputeEntitledDate(positionID, dtAppDate.SelectedDate.Value);
                        }
                        if (dtDue.SelectedDate == null)
                        {
                            dtDue.SelectedDate = ComputeDueDate(positionID, dtAppDate.SelectedDate.Value);
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
            if (client != null && cboSusuGrade.SelectedValue != "" && cboSusuPosition.SelectedValue != "" && dtApproval.SelectedDate != null
                && cboSusuGroup.SelectedValue!="")
            {
                if (sa.approvalDate != null)
                {
                    var totalContrib = 0.0;
                    if (sa.susuContributions.Count > 0)
                    {
                        totalContrib = sa.susuContributions.Sum(p => p.amount);
                    }
                    if (sa.amountEntitled > totalContrib && ViewState["confirmed"]==null)
                    {
                        HtmlHelper.MessageBox("The selected client has not finished contributig.\n Exiting will attarct a 10% addditional interest.\nClick save again if you confirm the interest.");
                        ViewState["confirmed"] = "1";
                        return;
                    }
                    sa.exitApprovedBy = User.Identity.Name;
                    sa.exited = true;
                    var addInterest = (sa.amountEntitled - totalContrib) * 0.1;
                    sa.exitDate = dtApproval.SelectedDate;
                    sa.interestAmount += addInterest;
                    sa.amountEntitled += addInterest;
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

                    var ent = new core_dbEntities();
                    var journalextensions = new JournalExtensions();

                    var conf = le.susuConfigs.FirstOrDefault();
                    var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 7);
                    if (lt == null) lt = le.loanTypes.FirstOrDefault();
                    var pro = ent.comp_prof.FirstOrDefault();

                    coreLogic.jnl_batch jb = journalextensions.Post("LN", lt.interestIncomeAccountID,
                        lt.unearnedInterestAccountID, addInterest,
                        "Group Susu Contribution for " + sa.client.surName + "," + sa.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, sa.susuAccountNo, ent, User.Identity.Name,
                        sa.client.branchID);
                    ent.jnl_batch.Add(jb);

                    le.SaveChanges();
                    HtmlHelper.MessageBox2("Susu Account Exit Approved Successfully", ResolveUrl("~/ln/susu/default.aspx"), "coreERP powered by ACS", IconType.ok);
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