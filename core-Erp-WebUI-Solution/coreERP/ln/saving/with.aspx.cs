using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.saving
{
    public partial class with : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.saving dp;
        List<coreLogic.savingSignatory> signatories;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            { 
                cboSavingsType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.savingTypes)
                {
                    cboSavingsType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.savingTypeName, r.savingTypeID.ToString()));
                }

                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }

                cboCur.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.currencies)
                {
                    cboCur.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.major_name,
                        r.currency_id.ToString()));
                }
                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"]=id;
                    dp = le.savings.FirstOrDefault(p => p.savingID == id);

                    if (dp != null)
                    { 

                        client = dp.client;  
                        Session["loan.cl"] = client;

                        signatories = dp.savingSignatories.ToList(); 
                         
                        txtInterestRate.Value = dp.interestRate;

                        var ssa = le.staffSavings.FirstOrDefault(p => p.savingID == id);
                        if (ssa != null || User.IsInRole("admin"))
                        {
                            txtAvailIntBal.Visible = false;
                            txtAvailPrincBal.Visible = false;
                            txtIntBalance.Visible = false;
                            txtPrincBal.Visible = false;
                        }
                        else
                        {
                            txtAvailIntBal.Value = dp.availableInterestBalance;
                            txtAvailPrincBal.Value = dp.availablePrincipalBalance;
                            txtIntBalance.Value = dp.clearedInterestBalance;
                            txtPrincBal.Value = dp.clearedPrincipalBalance;
                        }


                        cboSavingsType.SelectedValue=dp.savingType.savingTypeID.ToString();
                        cboClient.Items.Clear();
                        cboClient.Items.Add(new RadComboBoxItem(
                            (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName +
                                ", " + client.otherNames + " (" + client.accountNumber + ")", client.clientID.ToString()));
                        cboClient.SelectedIndex = 0;
                        if (dp.client.clientTypeID == 6)
                        {
                            pnlJoint.Visible = true;
                            pnlRegular.Visible = false;
                            txtJointAccountName.Text = dp.client.accountName;
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
                        txtAccountNo.Text = client.accountNumber;
                        if (dp.currencyID > 0)
                        {
                            cboCur.SelectedValue = dp.currencyID.ToString();
                            var cur = ent.currencies.FirstOrDefault(p => p.currency_id == dp.currencyID);
                            if (cur != null)
                            {
                                txtFxRate.Value = cur.current_buy_rate;
                            }
                        }

                        if (ssa != null || User.IsInRole("admin"))
                        {
                            gridDep.DataSource = dp.savingAdditionals;
                            gridDep.DataBind();
                            gridInt.DataSource = dp.savingInterests;
                            gridInt.DataBind();
                            gridWith.DataSource = dp.savingWithdrawals;
                            gridWith.DataBind();
                        }

                        gridDocument.DataSource = dp.savingSignatories;
                        gridDocument.DataBind();
                        RenderImages();
                    }

                    Session["saving"] = dp;
                }
                else
                {
                    dp = new coreLogic.saving();
                    Session["saving"] = dp;
                }
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadSaving(id);
            }

            try
            {
                if (cboClient.SelectedValue != "")
                {
                    var clientId = int.Parse(cboClient.SelectedValue);
                    client = le.clients.First(p => p.clientID == clientId);
                    RenderImages();
                }
            }
            catch (Exception) { }
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

                    RenderImages();
                }
            }
        }

        private void RenderImages()
        {

            if (client.clientImages != null)
            {
                var i = client.clientImages.FirstOrDefault();
                if (i != null)
                {
                    RadBinaryImage1.DataValue = i.image.image1;
                }
            }
        }
 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAmount.Value != null 
                && txtInterestRate.Value != null
                && cboSavingsType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && cboPaymentType.SelectedValue!=""
                && chlWTyp.SelectedItem != null
                && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1"))
                && txtNaration.Text.Trim() != "")
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
                if (dp.savingID > 0)
                {
                    var pamount = 0.0;
                    var iamount = 0.0;

                    if (chlWTyp.SelectedItem.Value == "I")
                    {
                        if (Math.Round(dp.interestBalance, 2) >= txtAmount.Value.Value)
                        {
                            iamount = txtAmount.Value.Value;
                        }
                    }
                    else if (chlWTyp.SelectedItem.Value == "P")
                    {
                        if (Math.Round(dp.principalBalance, 2) >= txtAmount.Value.Value)
                        {
                            pamount = txtAmount.Value.Value;
                        }
                    }
                    else if (chlWTyp.SelectedItem.Value == "B")
                    {
                        if (Math.Round(dp.interestBalance, 2) >= txtAmount.Value.Value)
                        {
                            iamount = txtAmount.Value.Value;
                        }
                        else if (Math.Round(dp.principalBalance + dp.interestBalance, 2) >= txtAmount.Value.Value)
                        {
                            pamount = txtAmount.Value.Value - dp.interestBalance;
                            iamount = dp.interestBalance;
                        }
                    }

                    var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                    if (user.accessLevel.withdrawalLimit < txtAmount.Value)
                    {
                        HtmlHelper.MessageBox("The amount to be withdrawn is beyond your access level",
                                                    "coreERP©: Failed", IconType.deny);
                        return;
                    }

                    SavingWithdrawalCalcModel calc = null;
                    var tier = le.chargeTypeTiers
                        .Where(p => p.chargeType.chargeTypeCode == "EWC") 
                        .FirstOrDefault();
                    if (tier != null)
                    {
                        IChargesHelper chMgr = new ChargesHelper();
                        try
                        {
                            calc = chMgr.ComputeCharges(dp.savingID, txtAmount.Value.Value, dtAppDate.SelectedDate.Value);

                            if (calc.totalCharges > 0 && ViewState["confirmed"] == null)
                            {
                                HtmlHelper.MessageBox("This withdrawal will attract a charge of " + calc.totalCharges.ToString("#, ##0.#0")
                                    + " \nIf client has agreed to it, then click save again",
                                                    "coreERP©: Failed", IconType.deny);
                                ViewState["confirmed"] = "Yes";
                                return;
                            }
                        }
                        catch (ApplicationException x2)
                        {
                            HtmlHelper.MessageBox(x2.Message, "coreERP©: Failed", IconType.deny);
                            return;
                        }
                    }

                    int mopID = int.Parse(cboPaymentType.SelectedValue);
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var acctID = dp.savingType.vaultAccountID.Value;
                    if (cboPaymentType.SelectedValue != "1" && bankID != null)
                    {
                        var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                        if (ba != null)
                        { 
                            if (ba.accts != null)
                            {
                                acctID = ba.accts.acct_id;
                            }
                        }
                    }
                    var ln = dp;
                    var pro = ent.comp_prof.FirstOrDefault();
                    var cur = ent.currencies.FirstOrDefault(p => p.currency_id == ln.currencyID);
                    var diff = txtFxRate.Value.Value - ln.fxRate;
                    coreLogic.IInvestmentManager ivMgr = new coreLogic.InvestmentManager();

                    savingWithdrawal dw = null;
                    try
                    {
                        if (calc != null)
                        {
                            coreLogic.IChargesHelper chMgr = new coreLogic.ChargesHelper(); 

                            dw = ivMgr.WithdrawalEWC(ref pamount, ref iamount, mop, bankID,
                            chlWTyp.SelectedValue, dp, txtAmount.Value.Value, dtAppDate.SelectedDate.Value.Date,
                            txtCheckNo.Text, txtNaration.Text, User.Identity.Name, calc);
                        }
                        else
                        {
                            dw = ivMgr.WithdrawalOthers(ref pamount, ref iamount, mop, bankID,
                            chlWTyp.SelectedValue, dp, txtAmount.Value.Value, dtAppDate.SelectedDate.Value.Date,
                            txtCheckNo.Text, txtNaration.Text, User.Identity.Name);
                        }
                    }
                    catch (ApplicationException x) //Validation exception
                    {
                        HtmlHelper.MessageBox(x.Message, "coreERP©: Failed", IconType.deny);
                        return;
                    }

                    le.SaveChanges();
                    ent.SaveChanges();

                    Session["loan.cl"] = null;
                    Session["saving"] = null;
                    HtmlHelper.MessageBox2("Withdrawal from Savings Data Saved Successfully!", 
                        ResolveUrl("~/ln/savingReports/withReceipt.aspx?id="+dw.savingWithdrawalID.ToString()));

                }
                else
                {
                    HtmlHelper.MessageBox("Amount is not authorized. Kindly seek authorization first!");
                }
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", "coreERP: Incomplete", IconType.warning);
            }
        }

        protected void cboSavingsType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboSavingsType.SelectedValue != "")
            {
                int id = int.Parse(cboSavingsType.SelectedValue);
                var depType = le.savingTypes.FirstOrDefault(p => p.savingTypeID == id);
                if (depType != null)
                {
                    txtInterestRate.Value = depType.interestRate; 
                }
            }
        }

        public void CalculateInterest2(DateTime date)
        {
            try
            {

                List<DateTime> listInt = new List<DateTime>();
                List<DateTime> listPrinc = new List<DateTime>();
                List<DateTime> listAll = new List<DateTime>();

                DateTime date2 = (dp.lastInterestDate == null) ? dp.firstSavingDate : dp.lastInterestDate.Value;
                //dp.savingSchedules.Load();
                DateTime lastDate = date2;
                foreach (var sched in dp.savingSchedules.ToList())
                {
                    if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.authorized == false) && (sched.principalPayment == 0 && sched.interestPayment == 0))
                    {
                        le.savingSchedules.Remove(sched);
                    }
                    else if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.principalPayment > 0) && (sched.expensed == false))
                    {
                        sched.interestPayment = 0;
                    }
                    else if ((sched.repaymentDate >= date2 && sched.repaymentDate <= date) && (sched.authorized == false) && (sched.expensed == false))
                    {
                        le.savingSchedules.Remove(sched);
                    }
                }
                var totalPrinc = dp.amountInvested - dp.savingSchedules.Sum(p => p.principalPayment);
                int i = 1;
                var totalInt = dp.principalBalance * (date - date2).TotalDays / 30.0 * (dp.interestRate) / 100.0;
                var intererst = totalInt;
                var princ = 0.0;

                var pro = ent.comp_prof.FirstOrDefault();
                coreLogic.savingSchedule pen = null;
                if (dp.savingSchedules.FirstOrDefault(p => p.repaymentDate == date) != null)
                {
                    pen = dp.savingSchedules.FirstOrDefault(p => p.repaymentDate == date);
                    pen.interestPayment += intererst;
                }
                else
                {
                    pen = new coreLogic.savingSchedule
                    {
                        interestPayment = totalInt,
                        principalPayment = 0,
                        repaymentDate = date,
                        authorized = false,
                        temp = true
                    };
                    dp.savingSchedules.Add(pen);
                }
                pen.interestPayment = intererst;
                pen.expensed = true;
                pen.temp = false;
                //pen.savingReference.Load();
                pen.saving.interestAccumulated += intererst;
                pen.saving.interestBalance += intererst;
                //pen.saving.savingTypeReference.Load();
                //pen.saving.clientReference.Load();
                pen.repaymentDate = date;
                pen.saving.lastInterestDate = date;

                var jb = journalextensions.Post("LN", pen.saving.savingType.interestExpenseAccountID.Value,
                    pen.saving.savingType.accountsPayableAccountID.Value, intererst,
                    "Interest Calculated on Savings - " + pen.saving.client.surName + "," + pen.saving.client.otherNames,
                    pro.currency_id.Value, pen.repaymentDate, pen.saving.savingNo, ent, User.Identity.Name, pen.saving.client.branchID);

                ent.jnl_batch.Add(jb);
            }
            catch (Exception x)
            {
            }
        }

        private void LoadSaving(int? id)
        {
            if (id != null)
            {
                dp = le.savings.FirstOrDefault(p => p.savingID == id);

                if (dp != null)
                { 

                    signatories = dp.savingSignatories.ToList();
                     
                    Session["loan.cl"] = client;
                     
                    signatories = dp.savingSignatories.ToList();
                    if (Session["signatories"] != null)
                    {
                        var sch = Session["signatories"] as List<coreLogic.savingSignatory>;
                        if (sch != null)
                        {
                            for (int i = sch.Count - 1; i >= 0; i--)
                            {
                                if (sch[i].savingSignatoryID <= 0)
                                {
                                    signatories.Add(sch[i]);
                                }
                            }
                        }
                    }
                    Session["signatories"] = signatories;

                }

                Session["saving"] = dp;
            }
            else
            {
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                }
                if (Session["saving"] != null)
                {
                    dp = Session["saving"] as coreLogic.saving;
                }
                else
                {
                    dp = new coreLogic.saving();
                    Session["saving"] = dp;
                } 
                if (Session["signatories"] != null)
                {
                    signatories = Session["signatories"] as List<coreLogic.savingSignatory>;

                }
                else
                {
                    signatories = new List<coreLogic.savingSignatory>();
                    Session["signatories"] = signatories;
                }
            }
        }
    }
}