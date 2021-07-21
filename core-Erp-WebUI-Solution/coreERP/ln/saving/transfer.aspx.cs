using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.saving
{
    public partial class transfer : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        coreLogic.client client;
        coreLogic.client client2;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (cboClient.SelectedValue != "")
                {
                    var clientId = int.Parse(cboClient.SelectedValue);
                    client = le.clients.First(p => p.clientID == clientId);
                    RenderImages();
                }
                if (cboClient2.SelectedValue != "")
                {
                    var clientId2 = int.Parse(cboClient2.SelectedValue);
                    client2 = le.clients.First(p => p.clientID == clientId2);
                    RenderImages2();
                }
            }
            catch (Exception) { }
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

                    cboAccount.Items.Clear();
                    foreach (var acc in client.savings)
                    {
                        cboAccount.Items.Add(new RadComboBoxItem(acc.savingNo + " - " +
                            (acc.availableInterestBalance + acc.availablePrincipalBalance).ToString("#,##0.#0"),
                            acc.savingID.ToString()));
                    }

                    RenderImages();
                }
            }
        }

        private void RenderImages()
        {
            if (client.clientImages != null)
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

        protected void btnFind_Click(object sender, EventArgs e)
        {
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else
                clients = le.clients.Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID == 3 || item.clientTypeID == 4 || item.clientTypeID == 5) ? item.companyName : ((item.clientTypeID == 6) ? item.accountName : item.surName + ", " + item.otherNames) + " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        protected void cboClient2_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient2.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient2.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void cboClient2_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient2.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient2.SelectedValue);
                client2 = le.clients.FirstOrDefault(p => p.clientID == clientID);
                if (client2 != null)
                {
                    Session["loan.cl2"] = client2;
                    rotator1.Items.Clear();
                    if (client2.clientTypeID == 6)
                    {
                        pnlJoint2.Visible = true;
                        pnlRegular2.Visible = false;
                        txtJointAccountName2.Text = client2.accountName;
                    }
                    else
                    {
                        pnlJoint2.Visible = false;
                        pnlRegular2.Visible = true;
                        txtSurname2.Text = (client2.clientTypeID == 3 || client2.clientTypeID == 4 || client2.clientTypeID == 5) ?
                            client2.companyName : client2.surName;
                        txtOtherNames2.Text = (client2.clientTypeID == 3 || client2.clientTypeID == 4 || client2.clientTypeID == 5) ?
                            " " : client2.otherNames;
                    }

                    cboAccount2.Items.Clear();
                    foreach (var acc in client2.savings)
                    {
                        cboAccount2.Items.Add(new RadComboBoxItem(acc.savingNo + " - " +
                            (acc.availableInterestBalance + acc.availablePrincipalBalance).ToString("#,##0.#0"),
                            acc.savingID.ToString()));
                    }

                    RenderImages2();
                }
            }
        }

        private void RenderImages2()
        {
            if (client2.clientImages != null)
            {
                foreach (var item in client2.clientImages)
                {
                    //item.imageReference.Load();
                    RadBinaryImage img = new RadBinaryImage();
                    img.Width = 216;
                    img.Height = 216;
                    img.ResizeMode = BinaryImageResizeMode.Fill;
                    img.DataValue = item.image.image1;
                    RadRotatorItem it = new RadRotatorItem();
                    it.Controls.Add(img);
                    rotator2.Items.Add(it);
                }
            }
        }

        protected void btnFind2_Click(object sender, EventArgs e)
        {
            List<coreLogic.client> clients = null;
            if (txtSurname2.Text.Trim().Length > 0 && txtOtherNames2.Text.Trim().Length > 0 && txtAccountNo2.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname2.Text.Trim()) || p.accountName.Contains(txtSurname2.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames2.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo2.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2).ToList();
            else if (txtSurname2.Text.Trim().Length == 0 && txtOtherNames2.Text.Trim().Length > 0 && txtAccountNo2.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames2.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo2.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname2.Text.Trim().Length > 0 && txtOtherNames2.Text.Trim().Length == 0 && txtAccountNo2.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname2.Text.Trim()) || p.accountName.Contains(txtSurname2.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo2.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname2.Text.Trim().Length > 0 && txtOtherNames2.Text.Trim().Length > 0 && txtAccountNo2.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames2.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname2.Text.Trim()) || p.accountName.Contains(txtSurname2.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname2.Text.Trim().Length == 0 && txtOtherNames2.Text.Trim().Length == 0 && txtAccountNo2.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo2.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname2.Text.Trim().Length > 0 && txtOtherNames2.Text.Trim().Length == 0 && txtAccountNo2.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname2.Text.Trim()) || p.accountName.Contains(txtSurname2.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname2.Text.Trim().Length == 0 && txtOtherNames2.Text.Trim().Length > 0 && txtAccountNo2.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames2.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else
                clients = le.clients.Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID == 3 || item.clientTypeID == 4 || item.clientTypeID == 5) ? item.companyName : ((item.clientTypeID == 6) ? item.accountName : item.surName + ", " + item.otherNames) + " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (cboAccount2.SelectedValue != "" && cboAccount.SelectedValue != "" && txtAmount.Value != null
                && txtNarration.Text.Trim() != "" && dtAppDate.SelectedDate != null)
            {
                var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                if (ct == null)
                {
                    HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }
                var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtAppDate.SelectedDate.Value
                    && p.open == true);
                if (ctd == null)
                {
                    HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }

                int fromSavingId = int.Parse(cboAccount.SelectedValue);
                int toSavingId = int.Parse(cboAccount2.SelectedValue);
                var fromSaving = le.savings.First(p => p.savingID == fromSavingId);
                var toSaving = le.savings.First(p => p.savingID == toSavingId);
                var iamount = 0.0;
                var pamount = 0.0;

                if (Math.Round(fromSaving.availableInterestBalance, 2) >= txtAmount.Value.Value)
                {
                    iamount = txtAmount.Value.Value;
                }
                else if (Math.Round(fromSaving.availablePrincipalBalance + fromSaving.availableInterestBalance, 2) >= txtAmount.Value.Value)
                {
                    pamount = txtAmount.Value.Value - fromSaving.availableInterestBalance;
                    iamount = fromSaving.availableInterestBalance;
                }
                else
                {
                    HtmlHelper.MessageBox("There is not enough available balance to be transferred", "coreERP: Invalid",
                        IconType.deny);
                    return;
                }

                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                coreLogic.IJournalExtensions journalextensions = new coreLogic.JournalExtensions();
                try
                {


                    var dw = new coreLogic.savingWithdrawal
                    {
                        checkNo = "",
                        principalWithdrawal = pamount,
                        interestWithdrawal = iamount,
                        bankID = null,
                        interestBalance = fromSaving.interestBalance,
                        withdrawalDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = fromSaving.principalBalance,
                        modeOfPaymentID = 1,
                        fxRate = 1,
                        localAmount = (pamount + iamount)*1,
                        naration = txtNarration.Text,
                        posted = true,
                        closed = false
                    };
                    fromSaving.principalBalance -= pamount;
                    fromSaving.interestBalance -= iamount;
                    fromSaving.availablePrincipalBalance -= pamount;
                    fromSaving.availableInterestBalance = fromSaving.availableInterestBalance - iamount;
                    fromSaving.savingWithdrawals.Add(dw);

                    var da = new coreLogic.savingAdditional
                    {
                        checkNo = "",
                        savingAmount = (iamount + pamount),
                        naration = txtNarration.Text,
                        bankID = null,
                        fxRate = 1,
                        localAmount = (iamount + pamount)*1,
                        interestBalance = 0,
                        savingDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = toSaving.principalBalance + (iamount + pamount),
                        modeOfPaymentID = 1,
                        posted = true,
                        closed = false
                    };
                    toSaving.savingAdditionals.Add(da);

                    var pro = ent.comp_prof.First();
                    var jb = journalextensions.Post("LN", fromSaving.savingType.accountsPayableAccountID.Value,
                        ct.accountID, (dw.interestWithdrawal + dw.principalWithdrawal),
                        "Withdrawal from Savings Account - " +
                        (dw.principalWithdrawal + dw.interestWithdrawal).ToString("#,###.#0")
                        + " - " + fromSaving.client.accountNumber + " - " + fromSaving.client.surName + "," +
                        fromSaving.client.otherNames,
                        pro.currency_id.Value, dw.withdrawalDate, fromSaving.savingNo, ent, User.Identity.Name,
                        toSaving.client.branchID);
                    var js =
                        jb.jnl.FirstOrDefault(
                            p => p.accts.acct_id == fromSaving.savingType.accountsPayableAccountID.Value);
                    js.dbt_amt = dw.principalWithdrawal;
                    js.crdt_amt = 0;

                    var jb2 = journalextensions.Post("LN", fromSaving.savingType.interestPayableAccountID,
                        ct.accountID, (dw.interestWithdrawal),
                        "Transfer from Savings Account - "
                        + " - " + fromSaving.client.accountNumber + " - " + fromSaving.client.surName + "," +
                        fromSaving.client.otherNames,
                        pro.currency_id.Value, dw.withdrawalDate, fromSaving.savingNo, ent, User.Identity.Name,
                        fromSaving.client.branchID);
                    js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == ct.accountID);
                    ent.Entry(js).State = System.Data.Entity.EntityState.Detached;
                    js = jb2.jnl.FirstOrDefault(p => p.accts.acct_id == fromSaving.savingType.interestPayableAccountID);
                    jb.jnl.Add(js);

                    jb2 = journalextensions.Post("LN", ct.accountID,
                        toSaving.savingType.accountsPayableAccountID.Value, da.savingAmount,
                        "Transfer into Savings Account - " + da.savingAmount
                        + " - " + toSaving.client.accountNumber + " - " + toSaving.client.surName + "," +
                        toSaving.client.otherNames,
                        pro.currency_id.Value, da.savingDate, toSaving.savingNo, ent, User.Identity.Name,
                        toSaving.client.branchID);

                    var jss = jb2.jnl.ToList();
                    foreach (var j in jss)
                    {
                        jb.jnl.Add(j);
                    }

                    ent.jnl_batch.Add(jb);

                    
                    try
                    {
                        ent.SaveChanges();
                        le.SaveChanges();

                        HtmlHelper.MessageBox("Transfer processed successfully", "coreERP: Success",
                            IconType.ok);
                    }
                    catch (Exception x)
                    {
                       HtmlHelper.MessageBox("There was an error processing the transaction", "coreERP: Error",
                            IconType.deny);
                    }
                }
                finally
                {
                    //ent.Database.Connection.Close();
                    //le.Database.Connection.Close();
                }
            }
        }

    }
}