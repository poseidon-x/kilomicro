using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;

namespace coreERP.ln.deposit
{
    public partial class with : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.deposit dp;
        coreLogic.IInvestmentManager invMgr = new coreLogic.InvestmentManager();
        private double principalBalance = 0;
        private double interestBalance = 0;

        bool postuseSavAcc;
        private double disInvestmentTotalCharge = 0.0;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            txtWithdrawalCharge.Visible = false;
            if (!IsPostBack)
            { 
                cboDepositType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.depositTypes)
                {
                    cboDepositType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.depositTypeName, r.depositTypeID.ToString()));
                }

                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }

                postuseSavAcc = le.depositConfigs.Any() && le.depositConfigs.FirstOrDefault() != null && le.depositConfigs.First().interestWithDrawBySav; 
                if (postuseSavAcc)
                {
                    var modes = le.modeOfPayments.Where(p => p.modeOfPaymentName.ToLower().Contains("savings")).ToList();
                    foreach (var r in modes)
                    {
                        cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                    }
                    txtCheckNo.Enabled = false;
                    cboBank.Enabled = false;
                }
                else
                {
                    foreach (var r in le.modeOfPayments)
                    {
                        cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                    }
                }
                
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    dp = le.deposits
                        .Include(p => p.depositAdditionals)
                        .Include(p => p.depositInterests)
                        .Include(p => p.depositWithdrawals)
                        .FirstOrDefault(p => p.depositID == id);

                    if (dp != null)
                    { 
                        client = dp.client; 
                        Session["loan.cl"] = client;

                        gridDocument.DataSource = dp.depositSignatories;
                        gridDocument.DataBind();

                        if (dp.maturityDate > DateTime.Today)
                        {
                            var dpDisInvstConf = le.depositDisInvestmentConfigs.ToList();
                            if (dpDisInvstConf.Count > 0)
                            {
                                panelWithdrwalCharge.Visible = true;
                                txtWithdrawalCharge.Visible = true;
                            }
                        }
                        principalBalance = (dp.depositAdditionals.Sum(p => p.depositAmount) - (dp.depositWithdrawals.Sum(p => p.principalWithdrawal)));
                        interestBalance = (dp.depositInterests.Sum(p => p.interestAmount) - (dp.depositWithdrawals.Sum(p => p.interestWithdrawal)));

                        txtPeriod.Value = dp.period;
                        txtInterestRate.Value = dp.interestRate;
                        txtIntBalance.Value = interestBalance;
                        txtPrincBal.Value = principalBalance; 
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
                            pnlJoint.Visible = false;
                            pnlRegular.Visible = true;
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
                        RenderImages();
                    }

                    Session["deposit"] = dp;
                }
                else
                {
                    dp = new coreLogic.deposit();
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
                    //client.clientAddresses.Load();
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
                && txtPeriod.Value != null
                && txtInterestRate.Value != null
                && cboDepositType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && cboPaymentType.SelectedValue!=""
                && chlWType.SelectedItem != null
                && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1") || (cboPaymentType.SelectedValue == "7"))
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

                var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                if (user.accessLevel.withdrawalLimit < txtAmount.Value)
                {
                    HtmlHelper.MessageBox("The amount to be withdrawn is beyond your access level",
                                                "coreERP©: Failed", IconType.deny);
                    return;
                }

                if (postuseSavAcc)
                {
                    var sav = le.savings.FirstOrDefault(p => p.clientID == dp.client.clientID); 
                    if (sav == null || sav.savingID < 0)
                    {
                        HtmlHelper.MessageBox("The Investment client doesn't have a savings account",
                                                    "coreERP©: Failed", IconType.deny);
                        return;
                    }
                }

            
                if (chlWType.SelectedItem.Value == "I")
                {
                    var bal = txtIntBalance.Value - txtAmount.Value.Value;
                    if (bal < 0)
                    {
                        HtmlHelper.MessageBox("Sorry interest amount been withdraw is  more than available, Please correct amount. Diff: "+ txtAmount.Value.Value,
                                                    "coreERP©: Failed", IconType.deny);
                        return;
                    }
                }
                else if (chlWType.SelectedItem.Value == "P")
                {
                    if (txtAmount.Value.Value > Math.Round(principalBalance, 2))
                    {
                        HtmlHelper.MessageBox("Sorry principal amount been withdraw is more avialable, Please correct amount",
                                                    "coreERP©: Failed", IconType.deny);
                        return;
                    }
                }
                else if (chlWType.SelectedItem.Value == "B")
                {
                    if (txtAmount.Value.Value > Math.Round(principalBalance+interestBalance, 2))
                    {
                        HtmlHelper.MessageBox("Sorry interest and principal amount been withdraw more avialable, Please correct amount",
                                                    "coreERP©: Failed", IconType.deny);
                        return;
                    }
                }

                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                if (dp.depositID > 0 &&
                    ((ent.comp_prof.FirstOrDefault().comp_name.Contains("Link Exchange") &&
                      dp.principalAuthorized + dp.interestAuthorized >= txtAmount.Value.Value)
                     || ent.comp_prof.FirstOrDefault().comp_name.Contains("Link Exchange") == false)
                    )
                {
                    var pamount = 0.0;
                    var iamount = 0.0;
                    if (chlWType.SelectedItem.Value == "I")
                    {
                        if (Math.Round(txtIntBalance.Value.Value, 2) >= txtAmount.Value.Value)
                        {
                            iamount = txtAmount.Value.Value;
                        }
                    }
                    else if (chlWType.SelectedItem.Value == "P")
                    {
                        if (Math.Round(principalBalance, 2) >= txtAmount.Value.Value)
                        {
                            pamount = txtAmount.Value.Value;
                        }
                    }
                    else if (chlWType.SelectedItem.Value == "B")
                    {
                        if (Math.Round(interestBalance, 2) >= txtAmount.Value.Value)
                        {
                            iamount = txtAmount.Value.Value;
                        }
                        else if (Math.Round(principalBalance + interestBalance, 2) >= txtAmount.Value.Value)
                        {
                            pamount = txtAmount.Value.Value - interestBalance;
                            iamount = interestBalance;
                        }
                    }
                    bool isDisInvt = false;
                    double? withCharge = 0.0;

                    if (dp.maturityDate > dtAppDate.SelectedDate.Value)
                    {
                        var dpConf = le.depositConfigs.FirstOrDefault();
                        if (dpConf != null && dpConf.disInvestmentRate > 0)
                        {
                            withCharge = txtWithdrawalCharge.Value;
                            if (withCharge != null)
                            {
                                isDisInvt = true;
                            }
                        }
                    }

                    
                    //if (pamount == 0 && iamount == 0) return;
                    int mopID = int.Parse(cboPaymentType.SelectedValue);
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                    var dw = new coreLogic.depositWithdrawal
                    {
                        checkNo = txtCheckNo.Text,
                        principalWithdrawal = pamount,
                        interestWithdrawal = iamount,
                        bankID = bankID,
                        interestBalance = interestBalance,
                        withdrawalDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = principalBalance,
                        naration = txtNaration.Text,
                        modeOfPayment = mop,
                        isDisInvestment = isDisInvt,
                        disInvestmentCharge = isDisInvt ? withCharge.Value : 0
                    };

                    List<savingAdditional> sa = new List<savingAdditional>();

                    var useSavAcc = le.depositConfigs.FirstOrDefault();
                    postuseSavAcc = le.depositConfigs.Any() && useSavAcc != null && useSavAcc.interestWithDrawBySav;
                    if (postuseSavAcc)
                    {
                        var sav = le.savings.FirstOrDefault(p => p.clientID == dp.client.clientID);
                        if (sav != null && sav.savingID > 0)
                        {
                            if (pamount > 0.0)
                            {
                                sav.amountInvested += pamount;
                                var da = new coreLogic.savingAdditional
                                {
                                    checkNo = null,
                                    savingAmount = pamount,
                                    naration = "Principal withdrawal from investment",
                                    bankID = null,
                                    fxRate = 0,
                                    localAmount = pamount,
                                    interestBalance = 0,
                                    savingDate = dtAppDate.SelectedDate.Value,
                                    creation_date = DateTime.Now,
                                    creator = User.Identity.Name,
                                    principalBalance = pamount,
                                    modeOfPaymentID = mop.modeOfPaymentID,
                                    posted = false,
                                    closed = false
                                };
                                sav.principalBalance += pamount;
                                sav.availablePrincipalBalance += pamount;
                                sav.savingAdditionals.Add(da);

                                sav.modification_date = DateTime.Now;
                                sav.last_modifier = User.Identity.Name;

                                sa.Add(da);
                            }
                            if (iamount > 0.0)
                            {
                                sav.amountInvested += iamount;
                                var da = new coreLogic.savingAdditional
                                {
                                    checkNo = null,
                                    savingAmount = iamount,
                                    naration = "Interest withdrawal from investment",
                                    bankID = null,
                                    fxRate = 0,
                                    localAmount = iamount,
                                    interestBalance = 0,
                                    savingDate = dtAppDate.SelectedDate.Value,
                                    creation_date = DateTime.Now,
                                    creator = User.Identity.Name,
                                    principalBalance = iamount,
                                    modeOfPaymentID = mop.modeOfPaymentID,
                                    posted = false,
                                    closed = false
                                };
                                sav.principalBalance += iamount;
                                sav.availablePrincipalBalance += iamount;
                                sav.savingAdditionals.Add(da);

                                sav.modification_date = DateTime.Now;
                                sav.last_modifier = User.Identity.Name;

                                sa.Add(da);
                            }


                        }
                    }
                    


                    dp.principalBalance -= pamount;
                    dp.interestBalance -= iamount;
                    dp.principalAuthorized -= pamount;
                    dp.interestAuthorized -= iamount;
                    dp.depositWithdrawals.Add(dw);

                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    le.SaveChanges();
                    ent.SaveChanges();

                    var usercashTill = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == User.Identity.Name.ToLower());
                    foreach (var savingAdd in sa)
                    {
                        invMgr.PostSavingAdditional(savingAdd, savingAdd.creator, ent, le, usercashTill);
                    }
                    


                    Session["loan.cl"] = null;
                    Session["deposit"] = null;
                    HtmlHelper.MessageBox2("Withdrawal from Deposit Data Saved Successfully!",
                        ResolveUrl("~/ln/depositReports/withReceipt.aspx?id=" + dw.depositWithdrawalID.ToString()));

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

        protected void cboDepositType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboDepositType.SelectedValue != "")
            {
                int id = int.Parse(cboDepositType.SelectedValue);
                var depType = le.depositTypes.FirstOrDefault(p => p.depositTypeID == id);
                if (depType != null)
                {
                    txtInterestRate.Value = depType.interestRate;
                    txtPeriod.Value = depType.defaultPeriod;
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
                    //dp.depositAdditionals.Load();
                    //dp.depositInterests.Load();
                    //dp.depositTypeReference.Load();
                    //dp.depositWithdrawals.Load();
                    //dp.clientReference.Load();
                    //dp.depositSchedules.Load();
                    //dp.depositSignatories.Load();

                    foreach (var sig in dp.depositSignatories)
                    {
                        //sig.imageReference.Load();
                    }
                    foreach (var da in dp.depositAdditionals)
                    {
                        //da.modeOfPaymentReference.Load();
                    }

                    foreach (var dw in dp.depositWithdrawals)
                    {
                        //dw.modeOfPaymentReference.Load();
                    }

                    client = dp.client;
                    //client.clientAddresses.Load();
                    foreach (var i in client.clientAddresses)
                    {
                        //i.addressReference.Load();
                        if (i.address != null)
                        {
                            //i.address.addressImages.Load();
                            foreach (var j in i.address.addressImages)
                            {
                                //j.imageReference.Load();
                            }
                        }
                    }
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

        protected void txtAmount_OnTextChanged(object sender, EventArgs e)
        {
            if (dp.maturityDate > DateTime.Today)
            {
                var dpDisInvstConf = le.depositDisInvestmentConfigs.ToList();
                if (dpDisInvstConf.Count > 0)
                {
                    double disInvstRate = getDisInvestmentRate(dp);

                    panelWithdrwalCharge.Visible = true;
                    txtWithdrawalCharge.Visible = true;
                    double interestBalanace = dp.interestBalance;

                    if (chlWType.SelectedItem.Value == "I")
                    {
                        double maxWithDrawalAMount = interestBalanace;
                        txtAmount.MaxValue = maxWithDrawalAMount;
                    }
                    else if (chlWType.SelectedItem.Value == "B")
                    {
                        double maxWithDrawalAMount = (interestBalanace + dp.principalBalance);
                        txtAmount.MaxValue = maxWithDrawalAMount;
                    }

                    if (txtAmount != null && txtAmount.Value > 0 && chlWType.SelectedItem.Value == "I")
                    {
                        if (txtAmount.Value.Value >= interestBalanace)
                        {
                            disInvestmentTotalCharge = interestBalanace * (disInvstRate);
                            txtWithdrawalCharge.Value = disInvestmentTotalCharge;
                        }
                        else if (txtAmount.Value.Value > 0 && txtAmount.Value.Value < interestBalanace)
                        {
                            disInvestmentTotalCharge = txtAmount.Value.Value * (disInvstRate);
                            txtWithdrawalCharge.Value = disInvestmentTotalCharge;
                        }

                    }//If with type is Both Interest and principal
                    else if (txtAmount != null && txtAmount.Value > 0 && chlWType.SelectedItem.Value == "B")
                    {
                        if (txtAmount.Value.Value >= interestBalanace)
                        {
                            disInvestmentTotalCharge = interestBalanace * (disInvstRate);
                            txtWithdrawalCharge.Value = disInvestmentTotalCharge;
                        }
                        else if (txtAmount.Value.Value > 0 && txtAmount.Value.Value < interestBalanace)
                        {
                            disInvestmentTotalCharge = txtAmount.Value.Value * (disInvstRate);
                            txtWithdrawalCharge.Value = disInvestmentTotalCharge;
                        }

                    }
                    else
                    {
                        txtWithdrawalCharge.Value = 0;
                    }
                }
            }
        }

        public double getDisInvestmentRate(coreLogic.deposit dep)
        {
            var dpDate = dep.depositAdditionals.LastOrDefault();
            double investedPeroid = (DateTime.Now - dpDate.depositDate).Days;
            int depositPeriodInDays = 0;

            if (dep.period == 2)
            {
                depositPeriodInDays = 60;
            }
            else if (dep.period == 3)
            {
                depositPeriodInDays = 91;
            }
            else if (dep.period == 6)
            {
                depositPeriodInDays = 182;
            }
            else if (dep.period == 12)
            {
                depositPeriodInDays = 365;
            }

            double investedPeriodInRate = (investedPeroid / depositPeriodInDays) * 100;

            var dpDisInvstConf = le.depositDisInvestmentConfigs.ToList();
            var disInvstPenalty =
                dpDisInvstConf.FirstOrDefault(
                    p => p.minTenure <= investedPeriodInRate && p.maxTenure >= investedPeriodInRate);

            double disInvstRate = disInvstPenalty == null ? 0.0 : disInvstPenalty.penaltyRate / 100;
            return disInvstRate;
        }
    }
}