using coreLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.deposit
{
    public partial class rollOver : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        core_dbEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new core_dbEntities();
            if(!IsPostBack)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.OrderBy(p=> p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }

                cboPeriod.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.depositPeriodInDays)
                {
                    cboPeriod.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.period, r.periodInDays.ToString()));
                }

                this.cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Monthly", "30"));
                cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Quarterly", "90"));
                cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Half-Yearly", "180"));
                cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("At Maturity", "-1"));
                cboPrincRepaymentMode.SelectedValue = "-1";

                this.cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Monthly", "30"));
                cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Quarterly", "90"));
                cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Half-Yearly", "180"));
                cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("At Maturity", "-1"));
                cboInterestRepaymentMode.SelectedValue = "-1";
            }
        }
  
        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                cboDeposit.Items.Clear();
                cboDeposit.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.deposits.Where(p => p.client.clientID == clientID
                    && (p.interestBalance>1 || p.principalBalance>1)))
                {
                    cboDeposit.Items.Add(new RadComboBoxItem(cl.amountInvested.ToString("#,###.#0")
                        + " ("+cl.firstDepositDate.ToString("dd-MMM-yyyy") +")", cl.depositID.ToString()));
                }
            }
        }

        protected void cboDeposit_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboDeposit.SelectedValue != "")
            {
                int depositID = int.Parse(cboDeposit.SelectedValue);
                var dp = le.deposits
                    .Include(p => p.depositAdditionals)
                    .Include(p => p.depositWithdrawals)
                    .Include(p => p.depositInterests)
                    .FirstOrDefault(p => p.depositID == depositID);
                if (dp != null)
                {
                    lblAmount.Text = dp.amountInvested.ToString("#,###.#0");
                    lblDepDate.Text = dp.firstDepositDate.ToString("dd-MMM-yyyy");
                    lblInte.Text = (dp.depositInterests.Sum(p => p.interestAmount) - dp.depositWithdrawals.Sum(p => p.interestWithdrawal)).ToString("#,###.#0");
                    lblMaturityDate.Text = dp.maturityDate.Value.ToString("dd-MMM-yyyy");
                    lblPrinc.Text = (dp.depositAdditionals.Sum(p => p.depositAmount) - dp.depositWithdrawals.Sum(p => p.principalWithdrawal)).ToString("#,###.#0");
                    dtAppDate.SelectedDate = dp.maturityDate.Value.AddDays(1);
                    //cboInterestRepaymentMode.SelectedValue = dp.interestRepaymentModeID.ToString();

                    if (ent.comp_prof.First().comp_name.ToLower().Contains("link") || dp.maturityDate.Value < DateTime.Now)
                    {
                        btnRollOverAll.Enabled = true;
                        btnRollOverInt.Enabled = true;
                        btnRollOverPrinc.Enabled = true;
                    }
                    else
                    {
                        btnRollOverAll.Enabled = false;
                        btnRollOverInt.Enabled = false;
                        btnRollOverPrinc.Enabled = false;
                    }
                }
            }
        }

        protected void btnRollOverAll_Click(object sender, EventArgs e)
        {
            if (cboDeposit.SelectedValue != "")
            {
                int depositID = int.Parse(cboDeposit.SelectedValue);
                var dp = le.deposits.FirstOrDefault(p => p.depositID == depositID);
                if (dp != null)
                {
                    var iamount = dp.interestBalance;
                    var pamount = dp.principalBalance;

                    int mopID = 1;
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    var dw = new coreLogic.depositWithdrawal
                    {
                        checkNo = "",
                        principalWithdrawal = dp.principalBalance,
                        interestWithdrawal = dp.interestBalance,
                        bankID = bankID,
                        interestBalance = 0,
                        withdrawalDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = 0,
                        modeOfPayment = mop,
                        fxRate=dp.fxRate,
                        naration="Rollover of Deposit",
                        localAmount=dp.localAmount,
                        posted=true
                    };
                    dp.depositWithdrawals.Add(dw);

                    dp.principalBalance = 0;
                    dp.interestBalance = 0;
                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    //dp.clientReference.Load();
                    //dp.depositTypeReference.Load();
                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var acctID = dp.depositType.vaultAccountID.Value ;
                    var pro = ent.comp_prof.FirstOrDefault();
                    var jb = journalextensions.Post("LN", dp.depositType.accountsPayableAccountID.Value,
                        acctID, (iamount + pamount),
                        "RollOver from deposit - " + (iamount + pamount).ToString("#,###.#0")
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.depositNo, ent, User.Identity.Name,
                                dp.client.branchID);

                    ent.jnl_batch.Add(jb);
                    var maturityDate=DateTime.Now;
                                     
                    maturityDate = dtAppDate.SelectedDate.Value.AddDays(int.Parse(cboPeriod.SelectedValue));


                    var vdepositNo = (new IDGenerator()).NewDepositNumber(dp.client.branchID.Value,
                        dp.client.clientID, -1,
                        dp.depositType.depositTypeName.Substring(0, 2).ToUpper());
                    var dp2 = new coreLogic.deposit
                    {
                        amountInvested = iamount + pamount,
                        interestExpected = (iamount + pamount)
                        * ((maturityDate - dtAppDate.SelectedDate.Value).TotalDays / 365.0)
                        * ((txtRate.Value.Value / 12.0) * 12 / 100.0),
                        autoRollover = false,
                        client = dp.client,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        depositNo = vdepositNo,
                        depositType = dp.depositType,
                        firstDepositDate = dtAppDate.SelectedDate.Value,
                        interestAccumulated = 0,
                        interestAuthorized = 0,
                        interestBalance = 0,
                        interestMethod = dp.interestMethod,
                        interestRate = txtRate.Value.Value / 12.0,
                        period = int.Parse(cboPeriod.SelectedValue) / 30,
                        maturityDate = maturityDate,
                        principalAuthorized = 0,
                        principalBalance = iamount + pamount,
                        principalRepaymentModeID = int.Parse(cboPrincRepaymentMode.SelectedValue),
                        interestRepaymentModeID = int.Parse(cboInterestRepaymentMode.SelectedValue),
                        lastInterestDate = null,
                        currencyID = dp.currencyID,
                        fxRate = dp.fxRate,
                        interestCalculationScheduleID = dp.interestCalculationScheduleID,
                        interestPayableAccountID = dp.interestPayableAccountID,
                        lastInterestFxGainLoss = 0,
                        localAmount = iamount + pamount,
                        lastPrincipalFxGainLoss = 0,
                        depositPeriodInDays = int.Parse(cboPeriod.SelectedValue),
                        annualInterestRate = txtRate.Value ?? 0
                    };
                    le.deposits.Add(dp2);

                    var da = new coreLogic.depositAdditional
                    {
                        checkNo = "",
                        depositAmount = iamount + pamount,
                        bankID = bankID,
                        interestBalance = 0,
                        depositDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = pamount + iamount,
                        modeOfPayment = mop,
                        naration = "Rollover of Deposit",
                        fxRate = dp.fxRate,
                        lastPrincipalFxGainLoss = 0,
                        localAmount = iamount + pamount,
                        posted = true
                    };
                    dp2.depositAdditionals.Add(da);
                    var jb2 = journalextensions.Post("LN", acctID,
                        dp.depositType.accountsPayableAccountID.Value, iamount + pamount,
                        "deposit Re-deposit - "
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.depositNo, ent, User.Identity.Name,
                        dp.client.branchID);
                    ent.jnl_batch.Add(jb2);

                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date = dtAppDate.SelectedDate.Value;
                    int i = 1;
                    var totalInt = (iamount + pamount) * (int.Parse(cboPeriod.SelectedValue) / 30) * (txtRate.Value.Value/12.0) / 100.0;
                    var intererst = 0.0;
                    var princ = 0.0;
                    while (date < dp2.maturityDate.Value)
                    {
                        date = date.AddMonths(1);
                        if (date >= dp2.maturityDate.Value) break;
                        if ((dp2.interestRepaymentModeID == 30)
                            || (dp2.interestRepaymentModeID == 90 && i % 3 == 0)
                            || (dp2.interestRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listInt.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        if ((dp2.principalRepaymentModeID == 30)
                            || (dp2.principalRepaymentModeID == 90 && i % 3 == 0)
                            || (dp2.principalRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listPrinc.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        i += 1;
                    }
                    listPrinc.Add(dp2.maturityDate.Value);
                    listInt.Add(dp2.maturityDate.Value);
                    listAll.Add(dp2.maturityDate.Value);

                    foreach (DateTime date2 in listAll)
                    {
                        if (listPrinc.Contains(date2))
                        {
                            princ = (iamount + pamount) / listPrinc.Count;
                        }
                        if (listInt.Contains(date2))
                        {
                            intererst = totalInt / listInt.Count;
                        }
                        dp2.depositSchedules.Add(new coreLogic.depositSchedule
                        {
                            interestPayment = intererst,
                            principalPayment = princ,
                            repaymentDate = date2,
                            authorized = false
                        });
                    }

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Deposit Rolled Over Successfully", ResolveUrl("~/ln/deposit/rollOver.aspx"), "coreERP©: Successful", IconType.ok);
                }
            }
        }

        protected void btnRollOverPrinc_Click(object sender, EventArgs e)
        {
            if (cboDeposit.SelectedValue != "")
            {
                int depositID = int.Parse(cboDeposit.SelectedValue);
                var dp = le.deposits.FirstOrDefault(p => p.depositID == depositID);
                if (dp != null)
                {
                    var iamount = 0;
                    var pamount = dp.principalBalance;

                    int mopID = 1;
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    var dw = new coreLogic.depositWithdrawal
                    {
                        checkNo = "",
                        principalWithdrawal = dp.principalBalance,
                        interestWithdrawal = 0,
                        bankID = bankID,
                        interestBalance = dp.interestBalance,
                        withdrawalDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = 0,
                        modeOfPayment = mop,
                        fxRate = dp.fxRate,
                        naration = "Rollover of Deposit",
                        localAmount = dp.localAmount,
                        posted = true
                    };
                    dp.depositWithdrawals.Add(dw);

                    dp.principalBalance = 0; 
                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var acctID = dp.depositType.vaultAccountID.Value;// ent.accts.FirstOrDefault(p => p.acc_num == "1000").acct_id;
                    var pro = ent.comp_prof.FirstOrDefault();
                    var jb = journalextensions.Post("LN", dp.depositType.accountsPayableAccountID.Value,
                        acctID, (iamount + pamount),
                        "Roll Over of deposit - " + (iamount + pamount).ToString("#,###.#0")
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.depositNo, ent, User.Identity.Name,
                        dp.client.branchID);

                    ent.jnl_batch.Add(jb);

                    var maturityDate = DateTime.Now;
                    
                    maturityDate = dtAppDate.SelectedDate.Value.AddDays(int.Parse(cboPeriod.SelectedValue));
                    
                    var vdepositNo= (new IDGenerator()).NewDepositNumber(dp.client.branchID.Value,
                        dp.client.clientID, -1,
                        dp.depositType.depositTypeName.Substring(0, 2).ToUpper());
                    var dp2 = new coreLogic.deposit
                    {
                        amountInvested = pamount,
                        interestExpected = (pamount)
                        * ((maturityDate - dtAppDate.SelectedDate.Value).TotalDays / 365.0)
                        * ((txtRate.Value.Value / 12.0) * 12 / 100.0),
                        autoRollover = false,
                        client = dp.client,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        depositNo = vdepositNo,
                        depositType = dp.depositType,
                        firstDepositDate = dtAppDate.SelectedDate.Value,
                        interestAccumulated = 0,
                        interestAuthorized = 0,
                        interestBalance = 0,
                        interestMethod = dp.interestMethod,
                        interestRate = txtRate.Value.Value/12.0,
                        period = int.Parse(cboPeriod.SelectedValue) / 30,
                        maturityDate = maturityDate,
                        principalAuthorized = 0,
                        principalBalance = iamount + pamount,
                        principalRepaymentModeID = int.Parse(cboPrincRepaymentMode.SelectedValue),
                        interestRepaymentModeID = int.Parse(cboInterestRepaymentMode.SelectedValue),
                        lastInterestDate=null,
                        fxRate = dp.fxRate,
                        interestCalculationScheduleID = dp.interestCalculationScheduleID,
                        interestPayableAccountID = dp.interestPayableAccountID,
                        lastInterestFxGainLoss = 0,
                        localAmount = iamount + pamount,
                        lastPrincipalFxGainLoss = 0,
                        depositPeriodInDays = int.Parse(cboPeriod.SelectedValue),
                        annualInterestRate = txtRate.Value ?? 0
                    };
                    le.deposits.Add(dp2);

                    var da = new coreLogic.depositAdditional
                    {
                        checkNo = "",
                        depositAmount = iamount + pamount,
                        bankID = bankID,
                        interestBalance = 0,
                        depositDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = pamount + iamount,
                        modeOfPayment = mop,
                        naration="Rollover of Deposit",
                        fxRate=dp.fxRate,
                        lastPrincipalFxGainLoss=0,
                        localAmount=iamount+pamount,
                        posted=true
                    };
                    dp2.depositAdditionals.Add(da);
                    var jb2 = journalextensions.Post("LN", acctID,
                        dp.depositType.accountsPayableAccountID.Value, iamount + pamount,
                        "deposit Re-deposit - "
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.depositNo, ent, User.Identity.Name,
                        dp.client.branchID);
                    ent.jnl_batch.Add(jb2);

                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date = dtAppDate.SelectedDate.Value;
                    int i = 1;
                    var totalInt = (iamount + pamount) * (int.Parse(cboPeriod.SelectedValue) / 365.0) * (txtRate.Value.Value / 100.0);
                    var intererst = 0.0;
                    var princ = 0.0;
                    while (date < dp2.maturityDate.Value)
                    {
                        date = date.AddMonths(1);
                        if (date >= dp2.maturityDate.Value) break;
                        if ((dp2.interestRepaymentModeID == 30)
                            || (dp2.interestRepaymentModeID == 90 && i % 3 == 0)
                            || (dp2.interestRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listInt.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        if ((dp2.principalRepaymentModeID == 30)
                            || (dp2.principalRepaymentModeID == 90 && i % 3 == 0)
                            || (dp2.principalRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listPrinc.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        i += 1;
                    }
                    listPrinc.Add(dp2.maturityDate.Value);
                    listInt.Add(dp2.maturityDate.Value);
                    listAll.Add(dp2.maturityDate.Value);

                    foreach (DateTime date2 in listAll)
                    {
                        if (listPrinc.Contains(date2))
                        {
                            princ = (iamount + pamount) / listPrinc.Count;
                        }
                        if (listInt.Contains(date2))
                        {
                            intererst = totalInt / listInt.Count;
                        }
                        dp2.depositSchedules.Add(new coreLogic.depositSchedule
                        {
                            interestPayment = intererst,
                            principalPayment = princ,
                            repaymentDate = date2,
                            authorized = false
                        });
                    }

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Depoit Rolled Over Successfully", ResolveUrl("~/ln/deposit/rollOver.aspx"), "coreERP©: Successful", IconType.ok);

                }
            }
        }

        protected void btnRollOverInt_Click(object sender, EventArgs e)
        {
            if (cboDeposit.SelectedValue != "")
            {
                int depositID = int.Parse(cboDeposit.SelectedValue);
                var dp = le.deposits.FirstOrDefault(p => p.depositID == depositID);
                if (dp != null)
                {
                    var iamount = dp.interestBalance;
                    var pamount = 0;

                    int mopID = 1;
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    var dw = new coreLogic.depositWithdrawal
                    {
                        checkNo = "",
                        principalWithdrawal = 0,
                        interestWithdrawal = dp.interestBalance,
                        bankID = bankID,
                        interestBalance = 0,
                        withdrawalDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = dp.principalBalance,
                        modeOfPayment = mop,
                        fxRate = dp.fxRate,
                        naration = "Rollover of Deposit",
                        localAmount = dp.localAmount,
                        posted = true
                    };
                    dp.depositWithdrawals.Add(dw);

                    dp.principalBalance = 0;
                    dp.interestBalance = 0;
                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    //dp.clientReference.Load();
                    //dp.depositTypeReference.Load();
                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var acctID = dp.depositType.vaultAccountID.Value;
                    var pro = ent.comp_prof.FirstOrDefault();
                    var jb = journalextensions.Post("LN", dp.depositType.accountsPayableAccountID.Value,
                        acctID, (iamount + pamount),
                        "RollOver from deposit - " + (iamount + pamount).ToString("#,###.#0")
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.depositNo, ent, User.Identity.Name,
                        dp.client.branchID);

                    ent.jnl_batch.Add(jb);

                    

                    var maturityDate = DateTime.Now;
                    maturityDate = dtAppDate.SelectedDate.Value.AddDays(int.Parse(cboPeriod.SelectedValue));
                    
                    var vdepositNo = (new IDGenerator()).NewDepositNumber(dp.client.branchID.Value,
                        dp.client.clientID, -1,
                        dp.depositType.depositTypeName.Substring(0, 2).ToUpper());
                    var dp2 = new coreLogic.deposit
                    {
                        amountInvested = iamount,
                        interestExpected = (iamount )
                        * ((maturityDate - dtAppDate.SelectedDate.Value).TotalDays / 365.0)
                        * ((txtRate.Value.Value / 12.0) * 12 / 100.0),
                        autoRollover = false,
                        client = dp.client,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        depositNo = vdepositNo,
                        depositType = dp.depositType,
                        firstDepositDate = dtAppDate.SelectedDate.Value,
                        interestAccumulated = 0,
                        interestAuthorized = 0,
                        interestBalance = 0,
                        interestMethod = dp.interestMethod,
                        interestRate = txtRate.Value.Value/12.0,
                        period = int.Parse(cboPeriod.SelectedValue)/30,
                        maturityDate = maturityDate,
                        principalAuthorized = 0,
                        principalBalance = iamount + pamount,
                        principalRepaymentModeID = int.Parse(cboPrincRepaymentMode.SelectedValue),
                        interestRepaymentModeID = int.Parse(cboInterestRepaymentMode.SelectedValue),
                        fxRate = dp.fxRate,
                        interestCalculationScheduleID = dp.interestCalculationScheduleID,
                        interestPayableAccountID = dp.interestPayableAccountID,
                        lastInterestFxGainLoss = 0,
                        localAmount = iamount + pamount,
                        lastPrincipalFxGainLoss = 0,
                        depositPeriodInDays = int.Parse(cboPeriod.SelectedValue),
                        annualInterestRate = txtRate.Value ?? 0
                    };
                    le.deposits.Add(dp2);

                    var da = new coreLogic.depositAdditional
                    {
                        checkNo = "",
                        depositAmount = iamount + pamount,
                        bankID = bankID,
                        interestBalance = 0,
                        depositDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = pamount + iamount,
                        modeOfPayment = mop,
                        naration = "Rollover of Deposit",
                        fxRate = dp.fxRate,
                        lastPrincipalFxGainLoss = 0,
                        localAmount = iamount + pamount,
                        posted = true
                    };
                    dp2.depositAdditionals.Add(da);
                    var jb2 = journalextensions.Post("LN", acctID,
                        dp.depositType.accountsPayableAccountID.Value, iamount + pamount,
                        "deposit Re-deposit - "
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.depositNo, ent, User.Identity.Name,
                        dp.client.branchID);
                    ent.jnl_batch.Add(jb2);

                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date = dtAppDate.SelectedDate.Value;
                    int i = 1;
                    var totalInt = (iamount + pamount) * (int.Parse(cboPeriod.SelectedValue) / 365.0) * (txtRate.Value.Value / 100.0); 
                    var intererst = 0.0;
                    var princ = 0.0;
                    while (date < dp2.maturityDate.Value)
                    {
                        date = date.AddMonths(1);
                        if (date >= dp2.maturityDate.Value) break;
                        if ((dp2.interestRepaymentModeID == 30)
                            || (dp2.interestRepaymentModeID == 90 && i % 3 == 0)
                            || (dp2.interestRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listInt.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        if ((dp2.principalRepaymentModeID == 30)
                            || (dp2.principalRepaymentModeID == 90 && i % 3 == 0)
                            || (dp2.principalRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listPrinc.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        i += 1;
                    }
                    listPrinc.Add(dp2.maturityDate.Value);
                    listInt.Add(dp2.maturityDate.Value);
                    listAll.Add(dp2.maturityDate.Value);

                    foreach (DateTime date2 in listAll)
                    {
                        if (listPrinc.Contains(date2))
                        {
                            princ = (iamount + pamount) / listPrinc.Count;
                        }
                        if (listInt.Contains(date2))
                        {
                            intererst = totalInt / listInt.Count;
                        }
                        dp2.depositSchedules.Add(new coreLogic.depositSchedule
                        {
                            interestPayment = intererst,
                            principalPayment = princ,
                            repaymentDate = date2,
                            authorized = false
                        });
                    }

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Depoit Rolled Over Successfully", ResolveUrl("~/ln/deposit/rollOver.aspx"), "coreERP©: Successful", IconType.ok);

                }
            }
        }
    }
}