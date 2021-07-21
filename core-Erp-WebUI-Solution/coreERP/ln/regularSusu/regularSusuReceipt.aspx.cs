using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.ln.susu
{
    public partial class regularSusuReceipt : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.client client;
        coreLogic.regularSusuAccount sa;
        coreLogic.core_dbEntities ent;
        List<coreLogic.regularSusuContribution> receivedContributions;

        protected void Page_Load(object sender, EventArgs e)
        {
           le = new coreLoansEntities();
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
                    rotator1.Items.Clear();

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

                    var sac = client.regularSusuAccounts
                        .OrderByDescending(p=> p.approvalDate)
                        .ToList();
                    foreach (var r in sac)
                    {
                        cboAccount.Items.Add(new RadComboBoxItem(r.regularSusuAccountNo, r.regularSusuAccountID.ToString()));
                    }
                    if (sac.Count == 1)
                    {
                        cboAccount.SelectedValue = sac[0].regularSusuAccountID.ToString();
                        cboAccount_SelectedIndexChanged(cboAccount, new RadComboBoxSelectedIndexChangedEventArgs("", "", "", ""));
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
                if (txtAmountPaid.Value.Value % sa.contributionAmount != 0)
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
                var cd = new coreLogic.regularSusuContribution
                {
                    amount = txtAmountPaid.Value.Value,
                    bankID = bankID,
                    checkNo = txtCheckNo.Text,
                    regularSusuAccount = sa,
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
                Session["regularSusuAccount"] = null;
                Session["regularSusuContributions"] = receivedContributions;

                txtContribAmount.Value = null;
                txtAmountContributed.Value = null;
                dtApproved.SelectedDate = null;
                dtApproved.SelectedDate = null;  
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
            if (Session["regularSusuContributions"] != null)
            {
                receivedContributions = Session["regularSusuContributions"] as List<coreLogic.regularSusuContribution>;
            }
            else
            {
                receivedContributions = new List<coreLogic.regularSusuContribution>();
                Session["regularSusuContributions"] = receivedContributions;
            }
            if (Session["regularSusuAccount"] != null)
            {
                sa = Session["regularSusuAccount"] as coreLogic.regularSusuAccount;
            }
            else
            {
                sa = new coreLogic.regularSusuAccount();
                Session["regularSusuAccount"] = sa;
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
                sa = le.regularSusuAccounts.FirstOrDefault(p => p.regularSusuAccountID == id);
                Session["regularSusuAccount"] = sa;
                if (sa != null)
                {
                    var contrib = 0.0;
                    var disb = 0.0;
                    var comm = 0.0;
                    if (sa.regularSusuContributions.Count > 0)
                    {
                        contrib = sa.regularSusuContributions.Sum(p => p.amount);
                        comm = Math.Ceiling(sa.regularSusuContributions.Count / 31.0) * sa.contributionRate - sa.commissionPaid;
                    }
                    if (sa.regularSusuWithdrawals.Count > 0)
                    {
                        disb = sa.regularSusuWithdrawals.Sum(p => p.amount);
                    }
                    sa.amountEntitled = contrib;
                    sa.netAmountEntitled = contrib - disb - comm;
                    sa.regularSusCommissionAmount = comm;
                    txtContribAmount.Value = sa.contributionAmount;
                    txtRemainder.Value = sa.netAmountEntitled;
                    dtApproved.SelectedDate = sa.approvalDate;  
                    dtStarted.SelectedDate = sa.startDate;
                    txtAmountContributed.Value = contrib;

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
                le.regularSusuContributions.Add(new coreLogic.regularSusuContribution
                {
                    agentID = rc.agentID,
                    amount = rc.amount,
                    appliedToLoan = rc.appliedToLoan,
                    modeOfPaymentID = rc.modeOfPaymentID,
                    posted = rc.posted,
                    regularSusuAccountID = rc.regularSusuAccount.regularSusuAccountID,
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
            Session["regularSusuContributions"] = null;
            Session["regularSusuAccount"] = null;
            Session["loan.cl"] = null;
            HtmlHelper.MessageBox2("Normal Susu Contributions Received Successfully",
                ResolveUrl("~/ln/regularSusu/regularSusuDefault.aspx"), "coreERP powered by ACS", IconType.ok);
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