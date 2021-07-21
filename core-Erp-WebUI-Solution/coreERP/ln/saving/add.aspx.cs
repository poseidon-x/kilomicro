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
    public partial class add : System.Web.UI.Page
    {
        JournalExtensions journalextensions = new JournalExtensions();
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
                
                this.cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                }

                cboCur.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.currencies)
                {
                    cboCur.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.major_name,
                        r.currency_id.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    dp = le.savings.FirstOrDefault(p => p.savingID == id);

                    if (dp != null)
                    {  
                        client = dp.client; 
                        Session["loan.cl"] = client;

                        signatories = dp.savingSignatories.ToList();

                        txtPeriod.Value = dp.period;
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
                            txtIntBalance.Value = dp.interestBalance;
                            txtPrincBal.Value = dp.principalBalance; 
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
            if (txtAmountInvested.Value != null
                && txtPeriod.Value != null
                && txtInterestRate.Value != null
                && cboSavingsType.SelectedValue != ""
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
                if (dp.savingID > 0)
                { 
                    dp.amountInvested = dp.amountInvested + txtAmountInvested.Value.Value;
                    var days=dp.maturityDate>=dtAppDate.SelectedDate.Value?
                        (dp.maturityDate.Value-dtAppDate.SelectedDate.Value).TotalDays:0;
                    dp.interestExpected = dp.interestExpected +
                        txtAmountInvested.Value.Value * (days / 365.0) * (dp.interestRate * 12 / 100.0);
                    int mopID = int.Parse(cboPaymentType.SelectedValue);
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                    var sav = le.savings.FirstOrDefault(p => p.savingID == dp.savingID);
                    var da = new coreLogic.savingAdditional
                    {
                        checkNo = txtCheckNo.Text,
                        savingAmount = txtAmountInvested.Value.Value,
                        naration = txtNaration.Text,
                        bankID = bankID,
                        fxRate = txtFxRate.Value.Value,
                        localAmount = txtAmountInvested.Value.Value * txtFxRate.Value.Value,
                        interestBalance = 0,
                        savingDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = sav.principalBalance+txtAmountInvested.Value.Value,
                        modeOfPaymentID = mop.modeOfPaymentID,
                        posted = false,
                        closed = false
                    };
                    dp.principalBalance += txtAmountInvested.Value.Value; 
                    dp.availablePrincipalBalance += txtAmountInvested.Value.Value; 
                    dp.savingAdditionals.Add(da);

                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    
                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date = dtAppDate.SelectedDate.Value;
                    int i = 1;
                    var totalInt = txtAmountInvested.Value.Value * (txtPeriod.Value.Value) * (txtInterestRate.Value.Value) / 100.0;
                    var intererst = 0.0;
                    var princ = 0.0;
                    while (date < dp.maturityDate)
                    {
                        date = date.AddMonths(1);
                        if (date >= dp.maturityDate) break;
                        if ((dp.interestRepaymentModeID == 30)
                            || (dp.interestRepaymentModeID == 90 && i % 3 == 0)
                            || (dp.interestRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listInt.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        if ((dp.principalRepaymentModeID == 30)
                            || (dp.principalRepaymentModeID == 90 && i % 3 == 0)
                            || (dp.principalRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listPrinc.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        i += 1;
                    }
                    listPrinc.Add(dp.maturityDate.Value);
                    listInt.Add(dp.maturityDate.Value);
                    listAll.Add(dp.maturityDate.Value);

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
                        var ds = le.savingSchedules.FirstOrDefault(p => p.savingID == dp.savingID && p.repaymentDate == date2);
                        if (ds == null)
                        {
                            dp.savingSchedules.Add(new coreLogic.savingSchedule
                            {
                                interestPayment = intererst,
                                principalPayment = princ,
                                repaymentDate = date2,
                                authorized = false
                            });
                        }
                        else
                        {
                            ds.interestPayment += intererst;
                            ds.principalPayment += princ;
                            ds.authorized = false;
                        }
                    }

                    try
                    {
                        var startDate = dtAppDate.SelectedDate.Value.AddDays(-15);
                        var endDate = dtAppDate.SelectedDate.Value.AddDays(15);
                        var plan = le.savingPlans.FirstOrDefault(p=> p.savingID==dp.savingID 
                            && p.plannedDate==dtAppDate.SelectedDate && p.deposited == false);
                        if(plan==null){
                            plan = le.savingPlans.Where(p => p.savingID == dp.savingID && p.plannedDate >= startDate
                                && p.plannedDate <= endDate && p.deposited==false).OrderBy(p => p.plannedDate).FirstOrDefault();
                        }
                        if (plan != null)
                        {
                            plan.deposited = true;
                            plan.amountDeposited = txtAmountInvested.Value.Value;
                        }
                    }
                    catch (Exception) { }
                    le.SaveChanges();
                    ent.SaveChanges();

                    Session["loan.cl"] = null;
                    Session["saving"] = null;
                    HtmlHelper.MessageBox2("Additional Savings Data Saved Successfully!",
                        ResolveUrl("~/ln/savingReports/addReceipt.aspx?id=" + da.savingAdditionalID.ToString()),
                        "coreERP©: Successful", IconType.ok);
                }
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", 
                    "coreERP: Incomplete", IconType.warning);
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
                    txtPeriod.Value = depType.defaultPeriod;
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
                    //dp.savingAdditionals.Load();
                    //dp.savingInterests.Load();
                    //dp.savingTypeReference.Load();
                    //dp.savingWithdrawals.Load();
                    ////dp.clientReference.Load();
                    //dp.savingSchedules.Load();
                    //dp.savingSignatories.Load();
                    //dp.savingPlans.Load();

                    foreach (var da in dp.savingAdditionals)
                    {
                        //da.modeOfPaymentReference.Load();
                    }

                    foreach (var dw in dp.savingWithdrawals)
                    {
                        //dw.modeOfPaymentReference.Load();
                    }

                    signatories = dp.savingSignatories.ToList();
                    foreach (var s in signatories)
                    {
                        //s.imageReference.Load();
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