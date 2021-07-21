using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.susu
{
    public partial class receipt : System.Web.UI.Page
    {

        coreLogic.coreLoansEntities le;
        coreLogic.client client;
        coreLogic.susuAccount sa;
        coreLogic.core_dbEntities ent;
        List<coreLogic.susuContribution> receivedContributions;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {  
                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num+")", 
                        r.bank_acct_id.ToString()));
                }
                 
                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                }
                cboPaymentMode.SelectedValue = "1";

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

            }
            else
            { 
                LoadData();
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
                    //client.susuAccounts.Load();
                    var sac = client.susuAccounts
                        .Where(p => p.approvalDate != null)
                        .OrderByDescending(p=> p.approvalDate)
                        .ToList();
                    foreach (var r in sac)
                    {
                        cboAccount.Items.Add(new RadComboBoxItem(r.susuAccountNo, r.susuAccountID.ToString()));
                    }
                    if (sac.Count == 1)
                    {
                        try
                        {
                            cboAccount.SelectedValue = sac[0].susuAccountID.ToString();
                            cboAccount_SelectedIndexChanged(cboAccount, new RadComboBoxSelectedIndexChangedEventArgs("", "", "", ""));
                        }
                        catch (Exception x) { }
                    }
                    RenderImages();
                }
            }
        }

        protected void btnReceive_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Value != null
                && txtAmountPaid.Value.Value > 0
                && this.dtMntDate.SelectedDate != null
                && cboPaymentMode.SelectedValue != ""
                && ((cboPaymentMode.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentMode.SelectedValue == "1"))
                && client != null
                && sa != null)
            {
                var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                if (ct == null)
                {
                    HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }
                var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtMntDate.SelectedDate.Value
                    && p.open == true);
                if (ctd == null)
                {
                    HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }
                if (txtAmountPaid.Value.Value> sa.amountEntitled-sa.susuContributions.Sum(p=> p.amount))
                {
                    HtmlHelper.MessageBox("Contribution cannot exceed total expected contributions for the cycle.", "coreERP©: Failed", IconType.deny);
                    return;
                }
                if (txtAmountPaid.Value.Value%sa.contributionAmount != 0)
                {
                    HtmlHelper.MessageBox("Contribution must be an even multiple of the rate for the account.", "coreERP©: Failed", IconType.deny);
                    return; 
                }
                int? bankID = null;
                if (cboBank.SelectedValue != null && cboBank.SelectedValue != "")
                    bankID = int.Parse(cboBank.SelectedValue);
                int? agentID = null;
                if (cboAgent.SelectedValue != "")
                {
                    agentID = int.Parse(cboAgent.SelectedValue);
                }
                int? staffID = null;
                if (cboStaff.SelectedValue != "")
                {
                    staffID = int.Parse(cboStaff.SelectedValue);
                }
                var cd = new coreLogic.susuContribution
                {
                    amount = txtAmountPaid.Value.Value,
                    bankID = bankID,
                    checkNo = txtCheckNo.Text,
                    susuAccount = sa,
                    modeOfPaymentID = int.Parse(cboPaymentMode.SelectedValue),
                    posted = false,
                    contributionDate = dtMntDate.SelectedDate.Value,
                    receiverType = ((cboAgent.SelectedValue != "" ? 1 : (cboStaff.SelectedValue != "" ? 2 : 3))),
                    agentID = agentID,
                    staffID = staffID,
                    cashierUsername = User.Identity.Name,
                    appliedToLoan = false,
                    narration = txtNarration.Text,
                };
                receivedContributions.Add(cd);

                gridContrib.DataSource = receivedContributions;
                gridContrib.DataBind();

                Session["loan.cl"] = null;
                Session["susuAccount"] = null;
                Session["susuContributions"] = receivedContributions;

                txtContribAmount.Value = null;
                txtRemainder.Value = null;
                txtAmountContributed.Value = null;
                dtApproved.SelectedDate = null;
                dtDisbursed.SelectedDate = null;
                dtDue.SelectedDate = null;
                dtEntitled.SelectedDate = null;
                dtStarted.SelectedDate = null;
                cboBank.SelectedValue = "";
                txtCheckNo.Text = "";
                txtAmountPaid.Value = null;
                cboStaff.SelectedValue = "";
                cboAgent.SelectedValue = "";
                cboClient.Items.Clear();
                cboClient.SelectedValue = "";
                cboClient.Text = "";
                cboAccount.Items.Clear();
                cboAccount.SelectedValue = "";
                cboAccount.Text = "";
                txtAccountNo.Text = "";
                txtSurname.Text = "";
                txtOtherNames.Text = "";
                txtJointAccountName.Text = "";
                RenderImages();
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", "coreERP: Incomplete", IconType.warning);
            }
        }

        private void RenderImages()
        {
            rotator1.Items.Clear();
            if (client.clientImages != null)
            {
                foreach (var item in client.clientImages)
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

        private void LoadData()
        {
            if (Session["susuContributions"] != null)
            {
                receivedContributions = Session["susuContributions"] as List<coreLogic.susuContribution>;
            }
            else
            {
                receivedContributions = new List<coreLogic.susuContribution>();
                Session["susuContributions"] = receivedContributions;
            }
            if (Session["susuAccount"] != null)
            {
                sa = Session["susuAccount"] as coreLogic.susuAccount;
            }
            else
            {
                sa = new coreLogic.susuAccount();
                Session["susuAccount"] = sa;
            }
            if (Session["loan.cl"] != null)
            {
                client = Session["loan.cl"] as coreLogic.client;
            }
            else
            {
                client = new coreLogic.client();
                Session["loan.cl"] = client;
            }
        }

        protected void cboAccount_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboAccount.SelectedValue != "" && cboAccount.SelectedValue != "")
            {
                int id = int.Parse(cboAccount.SelectedValue);
                sa = le.susuAccounts.FirstOrDefault(p => p.susuAccountID == id);
                Session["susuAccount"] = sa;
                if (sa != null)
                {
                    txtContribAmount.Value = sa.contributionAmount;
                    var sched = sa.susuContributionSchedules.ToList();
                    var contrib = sa.susuContributions.ToList();
                    txtRemainder.Value = ((sched.Count > 0) ? sched.Sum(p => p.amount) : 0)
                        - ((contrib.Count > 0) ? contrib.Sum(p => p.amount) : 0);
                    dtApproved.SelectedDate = sa.approvalDate;
                    dtDisbursed.SelectedDate = sa.disbursementDate;
                    dtDue.SelectedDate = sa.dueDate;
                    dtEntitled.SelectedDate = sa.entitledDate;
                    dtStarted.SelectedDate = sa.startDate;
                    dtEntitled.SelectedDate = sa.entitledDate;
                    txtAmountContributed.Value = ((contrib.Count > 0) ? contrib.Sum(p => p.amount) : 0);

                    if (sa.staffID != null)
                    {
                        cboStaff.SelectedValue = sa.staffID.ToString();
                    }
                    if (sa.agentID != null)
                    {
                        cboAgent.SelectedValue = sa.agentID.ToString();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (receivedContributions.Count == 0)
            {
                HtmlHelper.MessageBox("No Contributions Added to be Saved", "coreERP: Failed", IconType.deny);
                return;
            }
            foreach (var rc in receivedContributions)
            {
                le.susuContributions.Add(new coreLogic.susuContribution
                {
                    agentID = rc.agentID,
                    amount = rc.amount,
                    appliedToLoan = rc.appliedToLoan,
                    modeOfPaymentID = rc.modeOfPaymentID,
                    posted = rc.posted,
                    susuAccountID = rc.susuAccount.susuAccountID,
                    receiverType = rc.receiverType,
                    staffID = rc.staffID,
                    cashierUsername = rc.cashierUsername,
                    bankID = rc.bankID,
                    checkNo = rc.checkNo,
                    contributionDate = rc.contributionDate,
                    narration = rc.narration,
                });
            }
            le.SaveChanges();
            Session["susuContributions"] = null;
            Session["susuAccount"] = null;
            Session["loan.cl"] = null;
            HtmlHelper.MessageBox2("Susu Contributions Received Successfully", 
                ResolveUrl("~/ln/susu/default.aspx"), "coreERP powered by ACS", IconType.ok);
        }

        protected void dtMntDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ct == null)
            {
                HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                return;
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtMntDate.SelectedDate.Value
                && p.open == true);
            if (ctd == null)
            {
                HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                return;
            }
            lblCashier.Text = ct.userName;
            lblCashierStatus.Text = (ctd.open == true) ? "Openned" : "Closed";
        }
    }
}