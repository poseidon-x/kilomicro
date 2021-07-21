using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.ln.deposit
{
    public partial class deposit : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.deposit dp;
        List<coreLogic.depositSchedule> sched = new List<coreLogic.depositSchedule>();
        List<coreLogic.depositSignatory> signatories = new List<coreLogic.depositSignatory>();
        private IIDGenerator idGen;

        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new coreLogic.core_dbEntities();
            le = new coreLogic.coreLoansEntities();
            idGen = new IDGenerator();
            if (!IsPostBack)
            {
                Session["id"] = null;
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

                cboPeriod.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.depositPeriodInDays)
                {
                    cboPeriod.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.period,r.periodInDays.ToString()));
                }

                

                var dpconf = le.depositConfigs.FirstOrDefault();
                if (dpconf != null && dpconf.clientDepositBySav)
                {
                    foreach (var r in le.modeOfPayments)
                    {
                        cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName,
                            r.modeOfPaymentID.ToString()));
                    }
                }
                else
                {
                    foreach (var r in le.modeOfPayments.Where(p => !p.modeOfPaymentName.ToLower().Contains("saving")))
                    {
                        cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                    }
                }
                

                foreach (var r in le.depositRepaymentModes)
                {
                    cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentModeName, r.depositRepaymentModeId.ToString()));
                }

                foreach (var r in le.depositRepaymentModes)
                {
                    cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentModeName, r.depositRepaymentModeId.ToString()));
                }
                
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs.OrderBy(p => p.surName).ThenBy(p => p.otherNames))
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

                if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("link"))
                {
                    //chkAutoHeader.Visible = false;
                    chkAuto.Visible = false;
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    dp = le.deposits.FirstOrDefault(p => p.depositID == id);

                    if (dp != null)
                    { 
                        client = dp.client; 
                        Session["loan.cl"] = client;

                        sched = dp.depositSchedules.Where(p => p.temp == false).ToList();
                        Session["depositSchedules"] = sched;
                        signatories = dp.depositSignatories.ToList();

                        Session["signatories"] = dp.depositSignatories.ToList();
                        gridDocument.DataSource = dp.depositSignatories;
                        gridDocument.DataBind();

                        //txtPeriod.Value = dp.period;
                        chkAuto.Checked = dp.interestMethod;
                        txtInterestRate.Value = dp.interestRate;
                        txtIntBalance.Value = dp.interestBalance;
                        txtPrincBal.Value = dp.principalBalance;
                        txtAmountInvested.Value = dp.amountInvested;
                        cboDepositType.SelectedValue=dp.depositType.depositTypeID.ToString();
                        txtAmountInvested.ReadOnly = true;
                        dtAppDate.SelectedDate = dp.firstDepositDate;
                        if (dp.staffID != null)
                        {
                            cboStaff.SelectedValue = dp.staffID.ToString();
                        }
                        if (dp.agentId != null)
                        {
                            cboAgent.SelectedValue = dp.agentId.ToString();
                        }
                        dtAppDate.Enabled = false;
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
                        cboPrincRepaymentMode.SelectedValue = dp.principalRepaymentModeID.ToString();
                        cboInterestRepaymentMode.SelectedValue = dp.interestRepaymentModeID.ToString();
                        dtMaturyityDate.SelectedDate = dp.maturityDate;
                        gridDep.DataSource = dp.depositAdditionals;
                        gridDep.DataBind();
                        gridInt.DataSource = dp.depositInterests;
                        gridInt.DataBind();
                        gridWith.DataSource = dp.depositWithdrawals;
                        gridWith.DataBind();
                        txtIntExpected.Value = dp.interestExpected;

                        gridSchedule.DataSource = dp.depositSchedules.Where(p=>p.temp==false).OrderBy(p=>p.repaymentDate);
                        gridSchedule.DataBind();

                        gridDocument.DataSource = signatories;
                        gridDocument.DataBind();
                    }

                    Session["deposit"] = dp;
                }
                else
                {
                    dp = new coreLogic.deposit();
                    Session["deposit"] = dp;

                    sched = new List<coreLogic.depositSchedule>();
                    Session["depositSchedules"] = sched;

                    signatories = new List<coreLogic.depositSignatory>();
                    Session["signatories"] = signatories;

                    cboInterestRepaymentMode.SelectedValue = "-1";
                    cboPrincRepaymentMode.SelectedValue = "-1";
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

        protected void cboPaymentType_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboPaymentType.SelectedValue != "")
            {
                int paymentTypeId = int.Parse(cboPaymentType.SelectedValue);
                
            }
        }

        protected void dtAppDate_Changed(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (dtAppDate.SelectedDate.Value != null && cboPeriod.SelectedValue != "")
            {
                int periodInMonths = int.Parse(cboPeriod.SelectedValue) / 30;
                if (dtAppDate.SelectedDate != null && !(periodInMonths < 0))
                {
                    if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("link exchange"))
                    {
                        dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddMonths(periodInMonths);
                    }
                    else
                    {
                        int periodInDays = int.Parse(cboPeriod.SelectedValue);

                        if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("eclipse"))
                        {
                            dtMaturyityDate.SelectedDate =
                                dtAppDate.SelectedDate.Value.AddDays(periodInDays);
                        }
                        else
                        {
                            dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddDays(periodInDays);
                        }

                    }
                }
                onChange();
            }
        }

        protected void cboPeriod_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboPeriod.SelectedValue != "")
            {
                int periodInMonths = int.Parse(cboPeriod.SelectedValue) / 30;
                if (dtAppDate.SelectedDate != null && periodInMonths != null)
                {
                    if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("link exchange"))
                    {
                        dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddMonths(periodInMonths);
                    }
                    else
                    {
                        int periodInDays = int.Parse(cboPeriod.SelectedValue);
                        
                        if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("eclipse"))
                        {
                            if (periodInDays == 91 || periodInDays == 182)
                            {
                                dtMaturyityDate.SelectedDate =
                                    dtAppDate.SelectedDate.Value.AddDays(periodInDays - 1);
                            }
                            else
                            {
                                dtMaturyityDate.SelectedDate =
                                dtAppDate.SelectedDate.Value.AddDays(periodInDays);
                            }
                            
                        }
                        else
                        {
                            dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddDays(periodInDays);
                        }
                        
                    }
                }

                onChange();
            }
        }

        protected void txtAmountInvested_Changed(object sender, EventArgs e)
        {
            if (txtAmountInvested.Value > 0)
            {
                onChange();
            }
        }

        

        protected void onChange()
        {
            if (dtAppDate.SelectedDate != null && cboPeriod.SelectedValue != "" && txtAmountInvested.Value > 0
                && (txtInterestRate.Value > 0 || txtRateA.Value > 0))
            {
                double rate = 0;
                rate = txtRateA.Value > 0 ? txtRateA.Value.Value : txtInterestRate.Value.Value;
                double intExpec = (rate/100.0)*(int.Parse(cboPeriod.SelectedValue)/365.0)*txtAmountInvested.Value.Value;
                txtIntExpected.Value = intExpec;
                txtPrincBal.Value = 0;
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
                    //client.clientImages.Load();
                    
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

                    RenderImages();
                }
            }
        }

        private void RenderImages()
        {
            if (client.clientImages != null)
            {
                var i = client.clientImages.FirstOrDefault();
                //if (i != null && i.image != null)
                //{
                //    RadBinaryImage1.DataValue = i.image.image1;
                //}
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p=>p.clientTypeID==1 || p.clientTypeID==2).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else
                clients = le.clients.Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID == 3 || item.clientTypeID == 4 || item.clientTypeID == 5) ? item.companyName : ((item.clientTypeID == 6) ? item.accountName : item.surName + ", " + item.otherNames) + " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAmountInvested.Value != null
                && cboPeriod.SelectedValue != ""
                && txtInterestRate.Value != null
                && cboDepositType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && dtMaturyityDate.SelectedDate!=null
                && cboPrincRepaymentMode.SelectedValue!=""
                && cboInterestRepaymentMode.SelectedValue != ""
                && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1") 
                || cboPaymentType.Text.ToLower().Contains("savings"))
                && (txtNaration.Text.Trim()!=""||dp.depositID>0))
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
                int periodinDays = int.Parse(cboPeriod.SelectedValue);
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                dp.amountInvested = txtAmountInvested.Value.Value;
                dp.firstDepositDate = dtAppDate.SelectedDate.Value;
                int clientId = int.Parse(cboClient.SelectedValue);
                client = le.clients.FirstOrDefault(p => p.clientID == clientId);
                if (dp.depositID <= 0) dp.client = client;
                dp.creation_date = DateTime.Now;
                dp.creator = User.Identity.Name;
                dp.period = (periodinDays/30);
                dp.interestRate = txtInterestRate.Value.Value;
                dp.annualInterestRate = txtRateA.Value.Value;
                dp.depositPeriodInDays = getPeriodInDays((int)dp.period);
                int depTypeId = int.Parse(cboDepositType.SelectedValue);
                dp.depositType = le.depositTypes.FirstOrDefault(p => p.depositTypeID == depTypeId);
                dp.interestMethod = chkAuto.Checked;
                dp.interestExpected = txtIntExpected.Value??0;
                dp.principalRepaymentModeID = int.Parse(cboPrincRepaymentMode.SelectedValue);
                dp.interestRepaymentModeID = int.Parse(cboInterestRepaymentMode.SelectedValue);
                if (cboStaff.SelectedValue != "")
                {
                    dp.staffID = int.Parse(cboStaff.SelectedValue);
                }
                if (cboAgent.SelectedValue != "")
                {
                    dp.agentId = int.Parse(cboAgent.SelectedValue);
                } 
                if (dp.depositNo == null || dp.depositNo.Trim().Length == 0)
                {
                    dp.depositNo = idGen.NewDepositNumber(client.branchID.Value,
                        client.clientID, dp.depositID,
                        cboDepositType.Text.Substring(0, 2).ToUpper());                    
                }

                if (dp.depositID <= 0) le.deposits.Add(dp);

                var ss = dp.depositSchedules.ToList();
                for (int i=ss.Count-1; i>=0; i--)
                {
                    var s = ss[i];
                    if (!sched.Contains(s) && s.expensed==false)
                    {
                        le.depositSchedules.Remove(s);
                    }
                }
                foreach (var i in sched)
                {
                    if (!dp.depositSchedules.Contains(i))
                    {
                        dp.depositSchedules.Add(i);
                    }
                }
                
                foreach (var r in signatories)
                {
                    if (r.depositSignatoryID < 1)//!dp.depositSignatories.Contains(r))
                    {
                        dp.depositSignatories.Add(r);
                    }
                }
                List<int> curSig = signatories.Select(p => p.depositSignatoryID).ToList();
                List<depositSignatory> curSigInDb = dp.depositSignatories.ToList();
                foreach (var r in curSigInDb)
                {
                    if (curSig.Contains(r.depositSignatoryID))
                    {
                        dp.depositSignatories.Remove(r);
                    }
                }
                //for (int i = dp.depositSignatories.Count - 1; i >= 0; i--)
                //{
                //    var r = dp.depositSignatories.ToList()[i];
                //    if (!signatories.Contains(r))
                //    {
                //        dp.depositSignatories.Remove(r);
                //    }
                //}
                coreLogic.depositAdditional da=null;
                if (dp.depositID <= 0)
                {
                    dp.principalBalance = txtAmountInvested.Value.Value;
                    dp.maturityDate = dtMaturyityDate.SelectedDate;
                    var periodDays = 0;

                    //if (dp.period == 3)
                    //{
                    //    if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("eclipse"))
                    //    {
                    //        dp.maturityDate = dp.firstDepositDate.AddMonths(3);
                    //    }
                    //    else
                    //    {
                    //        dp.maturityDate = dp.firstDepositDate.AddDays(91);
                    //    }
                    //    periodDays = 91;
                    //}
                    //else if (dp.period == 6)
                    //{
                    //    if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("eclipse"))
                    //    {
                    //        dp.maturityDate = dp.firstDepositDate.AddMonths(6);
                    //    }
                    //    else
                    //    {
                    //        dp.maturityDate = dp.firstDepositDate.AddDays(182);
                    //    }
                    //    periodDays = 182;
                    //}
                    //else if (dp.period == 12)
                    //{
                    //    if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("eclipse"))
                    //    {
                    //        dp.maturityDate = dp.firstDepositDate.AddMonths(12);
                    //    }
                    //    else
                    //    {
                    //        dp.maturityDate = dp.firstDepositDate.AddDays(365);
                    //    }
                    //    periodDays = 365;
                    //}

                    //dp.interestExpected = txtAmountInvested.Value.Value
                    //                      *(periodDays/365.0)
                    //                      *(txtInterestRate.Value.Value*12/100.0);

                    int mopID = 0;
                    if (cboPaymentType.SelectedValue != "")
                        mopID = int.Parse(cboPaymentType.SelectedValue);
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                    da = new coreLogic.depositAdditional
                    {
                        checkNo = txtCheckNo.Text,
                        depositAmount = txtAmountInvested.Value.Value,
                        bankID = bankID,
                        interestBalance = 0,
                        depositDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = txtAmountInvested.Value.Value,
                        modeOfPayment = mop,
                        posted = false,
                        naration = txtNaration.Text
                    };

                    if(mopID ==7) { 
                    coreLogic.saving sav = le.savings.FirstOrDefault(p => p.clientID == clientId);
                    if (sav != null && sav.savingID > 0)
                    {
                        var pamount = 0.0;
                        var iamount = 0.0;



                        if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                        ////coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                        //var acctID = sav.savingType.vaultAccountID.Value;
                        //if (cboPaymentType.SelectedValue != "1" && bankID != null)
                        //{
                        //    var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                        //    if (ba != null)
                        //    {
                        //        if (ba.accts != null)
                        //        {
                        //            acctID = ba.accts.acct_id;
                        //        }
                        //    }
                        //}
                        var ln = sav;
                        coreLogic.IInvestmentManager ivMgr = new coreLogic.InvestmentManager();

                        savingWithdrawal dw = null;
                        try
                        {
                            dw = ivMgr.WithdrawalOthers(ref pamount, ref iamount, mop, bankID,
                                "P", sav, txtAmountInvested.Value.Value, dtAppDate.SelectedDate.Value,
                                txtCheckNo.Text, txtNaration.Text, User.Identity.Name);
                        }
                        catch (ApplicationException x) //Validation exception
                        {
                            HtmlHelper.MessageBox(x.Message, "coreERP©: Failed", IconType.deny);
                            return;
                        }

                    }

                }

                dp.depositAdditionals.Add(da);
                    CalculateSchedule();
                }
                
                le.SaveChanges();
                ent.SaveChanges();

                Session["loan.cl"] = null;
                Session["deposit"] = null;
                if (da != null)
                {
                    HtmlHelper.MessageBox2("Deposit Data Saved Successfully!",
                        ResolveUrl("~/ln/depositReports/addReceipt.aspx?id=" + da.depositAdditionalID.ToString()),
                        "coreERP©: Successful", IconType.ok);
                }
                else
                {
                    HtmlHelper.MessageBox2("Deposit Data Saved Successfully!",
                           ResolveUrl("~/ln/deposit/default.aspx?id=" + dp.depositID.ToString()),
                           "coreERP©: Successful", IconType.ok);
                }
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", 
                    "coreERP: Incomplete", IconType.warning);
            }
        }

        private void CalculateSchedule()
        {
            if (sched.Count == 0 && chkAuto.Checked == true)
            {
                List<DateTime> listInt = new List<DateTime>();
                List<DateTime> listPrinc = new List<DateTime>();
                List<DateTime> listAll = new List<DateTime>();

                DateTime date = dtAppDate.SelectedDate.Value;
                int i = 1;
                var totalInt = txtAmountInvested.Value.Value * int.Parse(cboPeriod.SelectedValue) * (txtInterestRate.Value.Value) / 100.0;
                var intererst = 0.0;
                var princ = 0.0;
                while (date < dtMaturyityDate.SelectedDate.Value)
                {
                    date = date.AddMonths(1);
                    if (date >= dtMaturyityDate.SelectedDate.Value) break;
                    if ((cboInterestRepaymentMode.SelectedValue == "30")
                        || (cboInterestRepaymentMode.SelectedValue == "90" && i % 3 == 0)
                        || (cboInterestRepaymentMode.SelectedValue == "180" && i % 6 == 0)
                        )
                    {
                        listInt.Add(date);
                        if (listAll.Contains(date) == false) listAll.Add(date);
                    }
                    if ((cboPrincRepaymentMode.SelectedValue == "30")
                        || (cboPrincRepaymentMode.SelectedValue == "90" && i % 3 == 0)
                        || (cboPrincRepaymentMode.SelectedValue == "180" && i % 6 == 0)
                        )
                    {
                        listPrinc.Add(date);
                        if (listAll.Contains(date) == false) listAll.Add(date);
                    }
                    i += 1;
                }
                listPrinc.Add(dtMaturyityDate.SelectedDate.Value);
                listInt.Add(dtMaturyityDate.SelectedDate.Value);
                listAll.Add(dtMaturyityDate.SelectedDate.Value);

                dp.modern = true;
                foreach (DateTime date2 in listAll)
                {
                    if (listPrinc.Contains(date2))
                    {
                        princ = txtAmountInvested.Value.Value / listPrinc.Count;
                    }
                    if (listInt.Contains(date2))
                    {
                        intererst = totalInt / listInt.Count;
                    }
                    dp.depositSchedules.Add(new coreLogic.depositSchedule
                    {
                        interestPayment = intererst,
                        principalPayment = princ,
                        repaymentDate = date2,
                        authorized = false,
                        expensed = false,
                        temp = false
                    });
                }
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.",
                    "coreERP: Incomplete", IconType.warning);
            }
        }

        protected void cboDepositType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboDepositType.SelectedValue != "")
            {
                int id = int.Parse(cboDepositType.SelectedValue);
                
                var depType = le.depositTypes.Include(p => p.depositTypeAllowedTenures).FirstOrDefault(p => p.depositTypeID == id);
                if (depType != null)
                {
                    if (ent.comp_prof.FirstOrDefault().comp_name.ToLower().Contains("eclipse"))
                    {
                        int clientId = int.Parse(cboClient.SelectedValue);
                        client = le.clients.FirstOrDefault(p => p.clientID == clientId);
                        if (depType.fixedRate && client != null && client.categoryID == 4)
                        {
                            int rate = le.depositConfigs.FirstOrDefault().staffDepositInterestRate;
                            txtInterestRate.Value = Math.Round(rate/12.0, 2);
                            txtRateA.Value = rate;
                            txtInterestRate.ReadOnly = true;
                            txtRateA.ReadOnly = true;
                        }
                        else
                        {
                            if (depType.fixedRate)
                            {
                                txtInterestRate.Value = depType.interestRate;
                                txtRateA.Value = Math.Round(depType.interestRate*12.0, 2);
                                txtInterestRate.ReadOnly = true;
                                txtRateA.ReadOnly = true;
                            }
                            else
                            {
                                txtInterestRate.ReadOnly = true;
                                txtRateA.ReadOnly = false;
                            }
                        }
                    }
                    else
                    {
                        txtInterestRate.Value = depType.interestRate;
                        txtRateA.Value = Math.Round(depType.interestRate * 12.0, 2);
                    }


                if (dtAppDate.SelectedDate != null && (!String.IsNullOrEmpty(cboPeriod.SelectedValue)))
                {
                    int periodInMonths = int.Parse(cboPeriod.SelectedValue)/30;
                        if (ent.comp_prof.FirstOrDefault().comp_name.Contains("Link Exchange"))
                        {
                            dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddMonths(periodInMonths);
                        }
                        else
                        {
                            dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddDays(int.Parse(cboPeriod.SelectedValue));
                        }
                    }
                }
            }
        }

        protected void btnAddSchedule_Click(object sender, EventArgs e)
        {
            if (dtRepaymentDate.SelectedDate!=null && txtPrincipal.Value != null &&
                txtInterest.Value!= null)
            {
                coreLogic.depositSchedule g;
                if (btnAddSchedule.Text == "Add Schedule")
                {
                    g = new coreLogic.depositSchedule();
                }
                else
                {
                    g = Session["depositSchedule"] as coreLogic.depositSchedule;
                } 
                g.principalPayment = txtPrincipal.Value.Value;
                g.interestPayment = txtInterest.Value.Value;
                g.repaymentDate = dtRepaymentDate.SelectedDate.Value;
                g.temp = false;
                g.expensed = false;
                g.authorized = false;

                if (btnAddSchedule.Text == "Add Schedule")
                {
                    sched.Add(g);
                }
                else
                {
                    var s = sched.FirstOrDefault(p => p.depositScheduleID == g.depositScheduleID);
                    s.principalPayment = g.principalPayment;
                    s.interestPayment = g.interestPayment;
                    s.repaymentDate = g.repaymentDate;
                }
                Session["depositSchedules"] = sched;
                gridSchedule.DataSource = sched;
                gridSchedule.DataBind();

                txtPrincipal.Value = null;
                txtInterest.Value = null;
                dtRepaymentDate.SelectedDate = null;

                btnAddSchedule.Text = "Add Schedule";
            }
        }

        protected void gridSchedule_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = sched[e.Item.ItemIndex];
                if (g != null)
                {
                    txtPrincipal.Value = g.principalPayment;
                    txtInterest.Value = g.interestPayment;
                    dtRepaymentDate.SelectedDate = g.repaymentDate;

                    Session["depositSchedule"] = g; 
                    btnAddSchedule.Text = "Update Schedule";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                sched.RemoveAt(e.Item.ItemIndex);
            }
            gridSchedule.DataSource = sched;
            gridSchedule.DataBind();
        }

        

        protected void txtRateA_TextChanged(object sender, EventArgs e)
        {
            if (txtRateA.Value != null)
            {
                txtInterestRate.Value = Math.Round(txtRateA.Value.Value / 12.0,7);
                onChange();
            }
        }

        protected void txtInterestRate_TextChanged(object sender, EventArgs e)
        {
            if (txtInterestRate.Value != null)
            {
                txtRateA.Value = Math.Round(txtInterestRate.Value.Value * 12.0, 7);
                onChange();
            }
        }

        protected void gridDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = signatories[e.Item.ItemIndex];
                if (g != null)
                {
                    txtDocDesc.Text = g.fullName;
                     
                    Session["signatory"] = g;
                    btnAddDcoument.Text = "Update Signatory";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                signatories.RemoveAt(e.Item.ItemIndex);
            }
            gridDocument.DataSource = signatories;
            gridDocument.DataBind();
        }

        protected void btnAddDcoument_Click(object sender, EventArgs e)
        {
            if (txtDocDesc.Text != "")
            {
                if (upload4.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile item in upload4.UploadedFiles)
                    {
                        byte[] b = new byte[item.InputStream.Length];
                        item.InputStream.Read(b, 0, b.Length);


                        if (btnAddDcoument.Text == "Update Signatory")
                        {
                            var g = Session["signatory"] as coreLogic.depositSignatory;
                            g.fullName = txtDocDesc.Text;
                            g.image.description = txtDocDesc.Text;
                            g.image.image1 = b;
                            g.image.content_type = item.ContentType;
                        }
                        else
                        {
                            var i = new coreLogic.image
                            {
                                description = txtDocDesc.Text,
                                image1 = b,
                                content_type = item.ContentType 
                            };

                            var g = new coreLogic.depositSignatory
                            {
                                deposit = dp,
                                image = i,
                                fullName=txtDocDesc.Text
                            };
                            signatories.Add(g);
                        }
                    }
                }
                else if (btnAddDcoument.Text == "Update Signatory")
                {
                    var g = Session["signatory"] as coreLogic.depositSignatory;
                    g.fullName = txtDocDesc.Text;
                }
                Session["signatories"] = signatories;
                gridDocument.DataSource = signatories;
                gridDocument.DataBind();

                txtDocDesc.Text = "";
                btnAddDcoument.Text = "Add Signatory";
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

        private void LoadDeposit(int? id)
        {
            if (id != null)
            {
                dp = le.deposits.FirstOrDefault(p => p.depositID == id);

                if (dp != null)
                { 
                    client = dp.client; 
                    Session["loan.cl"] = client;

                    sched = dp.depositSchedules.Where(p => p.temp == false).ToList();
                    if (Session["depositSchedules"] != null)
                    {
                        var sch = Session["depositSchedules"] as List<coreLogic.depositSchedule>;
                        if (sch != null)
                        {
                            for (int i = sch.Count - 1; i >= 0; i--)
                            {
                                if (sch[i].depositScheduleID <= 0)
                                {
                                    sched.Add(sch[i]);
                                }
                            }
                        }
                    }
                    Session["depositSchedules"] = sched;

                    signatories=dp.depositSignatories.ToList();
                    if (Session["signatories"] != null)
                    {
                        var sch = Session["signatories"] as List<coreLogic.depositSignatory>;
                        if (sch != null)
                        {
                            for (int i = sch.Count - 1; i >= 0; i--)
                            {
                                if (sch[i].depositSignatoryID <= 0)
                                {
                                    signatories.Add(sch[i]);
                                }
                            }
                        }
                    }
                    Session["signatories"] = signatories;
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
                dp = new coreLogic.deposit();
                Session["deposit"] = dp; 
                if (Session["depositSchedules"] != null)
                {
                    sched = Session["depositSchedules"] as List<coreLogic.depositSchedule>;
                }
                else
                {
                    sched = new List<coreLogic.depositSchedule>();
                    Session["depositSchedules"] = sched;
                }
                if (Session["signatories"] != null)
                {
                    signatories = Session["signatories"] as List<coreLogic.depositSignatory>;

                }
                else
                {
                    signatories = new List<coreLogic.depositSignatory>();
                    Session["signatories"] = signatories;
                } 
            }
        }

        protected void tab1_TabClick(object sender, RadTabStripEventArgs e)
        {

        }

        protected void gridMultiPayment_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void gridMultiPayment_OnInsertCommand(object sender, GridCommandEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void gridMultiPayment_OnItemInserted(object sender, GridInsertedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void gridMultiPayment_OnItemCreated(object sender, GridItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void gridMultiPayment_OnUpdateCommand(object sender, GridCommandEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void gridMultiPayment_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void gridMultiPayment_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private int getPeriodInDays(int periodInMonths)
        {
            int depositPeriodInDays = 0; 
            switch (periodInMonths)
            {
                case 1:
                    depositPeriodInDays = 30;
                    break;
                case 2:
                    depositPeriodInDays = 60;
                    break;
                case 3:
                    depositPeriodInDays = 91;
                    break;
                case 4:
                    depositPeriodInDays = 121;
                    break;
                case 5:
                    depositPeriodInDays = 151;
                    break;
                case 6:
                    depositPeriodInDays = 182;
                    break;
                case 7:
                    depositPeriodInDays = 212;
                    break;
                case 8:
                    depositPeriodInDays = 242;
                    break;
                case 9:
                    depositPeriodInDays = 274;
                    break;
                case 10:
                    depositPeriodInDays = 304;
                    break;
                case 11:
                    depositPeriodInDays = 334;
                    break;
                case 12:
                    depositPeriodInDays = 365;
                    break;
            }
            return depositPeriodInDays;
        }
    }
}