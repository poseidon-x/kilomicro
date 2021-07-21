using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.ln.deposit
{
    public partial class add : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.deposit dp;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            { 
                cboDepositType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.depositTypes)
                {
                    cboDepositType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.depositTypeName, r.depositTypeID.ToString()));
                }

                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts.ToList())
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }

                cboPeriod.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.depositPeriodInDays)
                {
                    cboPeriod.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.period, r.periodInDays.ToString()));
                }

                cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                }
                cboPaymentType.Enabled = true;

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    dp = le.deposits.Include(p => p.depositType).FirstOrDefault(p => p.depositID == id);

                    if (dp != null)
                    {
                        if (!dp.depositType.allowAdditionalDeposit)
                        {
                            HtmlHelper.MessageBox2("Deposit of type " + dp.depositType.depositTypeName + " does not allow for addition",
                            ResolveUrl("~/dash/home.aspx"),
                            "coreERP©: Note", IconType.deny);
                        }
                        client = dp.client;
                        Session["loan.cl"] = client;

                        cboPeriod.SelectedValue = dp.depositPeriodInDays.ToString();
                        txtRateA.Value = dp.annualInterestRate;
                        txtIntBalance.Value = dp.interestBalance;
                        txtPrincBal.Value = dp.principalBalance; 
                        cboDepositType.SelectedValue=dp.depositType.depositTypeID.ToString();
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
                            txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                client.companyName : client.surName;
                            txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                " " : client.otherNames;
                        }
                        txtAccountNo.Text = client.accountNumber;

                       
                        gridDep.DataSource = dp.depositAdditionals;
                        gridDep.DataBind();
                        gridInt.DataSource = dp.depositInterests;
                        gridInt.DataBind();
                        gridWith.DataSource = dp.depositWithdrawals;
                        gridWith.DataBind();

                        gridDocument.DataSource = dp.depositSignatories;
                        gridDocument.DataBind();
                        RenderImages();
                    }

                    Session["deposit"] = dp;
                } 
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadDeposit(id);
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
                    //RadBinaryImage1.im.Clear();

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
            if (txtAmountInvested.Value != null
                && dtAppDate.SelectedDate != null
                && cboPaymentType.SelectedValue != ""
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
                if (dp.depositID > 0)
                {
                    if (!dp.depositAdditionals.Any())
                    {
                        dp.firstDepositDate = dtAppDate.SelectedDate.Value;
                        dp.maturityDate = dtAppDate.SelectedDate.Value.Date.AddDays(dp.depositPeriodInDays);
                        //dp.interestExpected = (dp.depositPeriodInDays/365.0)*(dp.annualInterestRate/100)* txtAmountInvested.Value.Value;
                    }
                    dp.amountInvested += txtAmountInvested.Value.Value;
                    var days = dp.maturityDate >= dtAppDate.SelectedDate.Value ?
                        (dp.maturityDate.Value - dtAppDate.SelectedDate.Value).TotalDays : 0; 
                    int mopID = int.Parse(cboPaymentType.SelectedValue);
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                    var da = new coreLogic.depositAdditional
                    {
                        checkNo = txtCheckNo.Text,
                        depositAmount = txtAmountInvested.Value.Value,
                        bankID = bankID,
                        interestBalance = dp.interestBalance,
                        depositDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = dp.principalBalance+txtAmountInvested.Value.Value,
                        modeOfPayment = mop,
                        naration = txtNaration.Text,
                        posted = false
                    };
                    double additionalInterestExpected = (dp.maturityDate.Value - da.depositDate).TotalDays/365.0*
                                                        (dp.annualInterestRate/100)*(da.depositAmount);

                    dp.interestExpected += additionalInterestExpected;
                    dp.principalBalance += txtAmountInvested.Value.Value; 
                    dp.depositAdditionals.Add(da);

                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    le.SaveChanges();
                    
                    Session["loan.cl"] = null;
                    Session["deposit"] = null;
                    HtmlHelper.MessageBox2("Additional Deposit Data Saved Successfully!",
                        ResolveUrl("~/ln/depositReports/addReceipt.aspx?id=" + da.depositAdditionalID.ToString()),
                        "coreERP©: Successful", IconType.ok);
                }
                else
                {
                    HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", "coreERP: Unsaved Deposit", IconType.warning);
                }
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", "coreERP: Incomplete", IconType.warning);
            }
        }

        //protected void cboDepositType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    if (cboDepositType.SelectedValue != "")
        //    {
        //        int id = int.Parse(cboDepositType.SelectedValue);
        //        var depType = le.depositTypes.FirstOrDefault(p => p.depositTypeID == id);
        //        if (depType != null)
        //        {
        //            txtInterestRate.Value = depType.interestRate;
        //            txtPeriod.Value = depType.defaultPeriod;
        //        }
        //    }
        //}

        protected void txtAmountInvested_Changed(object sender, EventArgs e)
        {
            
            if (txtAmountInvested.Value > 0 && (dtAppDate.SelectedDate != null) && dtAppDate.SelectedDate.Value != null)
            {
                onChange();
            }
        }

        protected void dtAppDate_changed(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (dtAppDate.SelectedDate != null && dtAppDate.SelectedDate.Value != null && txtAmountInvested.Value > 0)
            {
                onChange();
            }
        }



        protected void onChange()
        {
            if (IsPostBack && (dtAppDate.SelectedDate != null))
            {

                if (dp.maturityDate.Value <= dtAppDate.SelectedDate.Value)
                {
                    HtmlHelper.MessageBox2("Sorry, Additional deposit is not allowed for Matured Investment",
                        ResolveUrl("~/dash/home.aspx"),
                        "coreERP©: Note", IconType.deny);
                }
                else
                {
                    if (dtAppDate.SelectedDate != null && cboPeriod.SelectedValue != "" && txtAmountInvested.Value > 0
                   && txtRateA.Value > 0)
                    {

                        double additionalInterestExpected =
                            (dp.maturityDate.Value - dtAppDate.SelectedDate.Value).TotalDays / 365 *
                            (dp.annualInterestRate / 100) * (txtAmountInvested.Value.Value);

                        addIntExpec.Value = additionalInterestExpected;
                    }
                }

               
            }
        }

        private void LoadDeposit(int? id)
        {
            if (id != null)
            {
                dp = le.deposits.FirstOrDefault(p => p.depositID == id);

                if (dp != null)
                {  
                    client = dp.client;
                    Session["loan.cl"] = client;
                }
                RenderImages();
                Session["deposit"] = dp;
            }
            else
            {
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                }
                if (Session["deposit"] != null)
                {
                    dp = Session["deposit"] as coreLogic.deposit;
                }
                else
                {
                    dp = new coreLogic.deposit();
                    Session["deposit"] = dp;
                } 
            }
        }
    }
}