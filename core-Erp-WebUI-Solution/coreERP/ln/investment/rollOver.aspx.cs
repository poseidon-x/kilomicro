using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.investment
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
                foreach (var cl in le.clients
                    .Where(p => p.clientTypeID == 3)
                    .OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
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
            }
        }
  
        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                cboInvestment.Items.Clear();
                cboInvestment.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.investments.Where(p => p.client.clientID == clientID
                    && (p.interestBalance>1 || p.principalBalance>1)))
                {
                    cboInvestment.Items.Add(new RadComboBoxItem(cl.amountInvested.ToString("#,###.#0")
                        + " ("+cl.firstinvestmentDate.ToString("dd-MMM-yyyy") +")", cl.investmentID.ToString()));
                }
            }
        }

        protected void cboInvestment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboInvestment.SelectedValue != "")
            {
                int investmentID = int.Parse(cboInvestment.SelectedValue);
                var dp = le.investments.FirstOrDefault(p => p.investmentID == investmentID);
                if (dp != null)
                {
                    lblAmount.Text = dp.amountInvested.ToString("#,###.#0");
                    lblDepDate.Text = dp.firstinvestmentDate.ToString("dd-MMM-yyyy");
                    lblInte.Text = dp.interestBalance.ToString("#,###.#0");
                    lblMaturityDate.Text = dp.maturityDate.Value.ToString("dd-MMM-yyyy");
                    lblPrinc.Text = dp.principalBalance.ToString("#,###.#0");
                    dtAppDate.SelectedDate = dp.maturityDate.Value.AddDays(1);
                    cboInterestRepaymentMode.SelectedValue = dp.interestRepaymentModeID.ToString();

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
            if (cboInvestment.SelectedValue != "")
            {
                int investmentID = int.Parse(cboInvestment.SelectedValue);
                var dp = le.investments.FirstOrDefault(p => p.investmentID == investmentID);
                if (dp != null)
                {
                    var iamount = dp.interestBalance;
                    var pamount = dp.principalBalance;

                    int mopID = 1;
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    var dw = new coreLogic.investmentWithdrawal
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
                        naration="Rollover of Investment",
                        localAmount=dp.localAmount,
                        posted=true
                    };
                    dp.investmentWithdrawals.Add(dw);

                    dp.principalBalance = 0;
                    dp.interestBalance = 0;
                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    //dp.clientReference.Load();
                    //dp.investmentTypeReference.Load();
                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var acctID = dp.investmentType.vaultAccountID.Value ;
                    var pro = ent.comp_prof.FirstOrDefault();
                    var jb = journalextensions.Post("LN", 
                        acctID, 
                        dp.investmentType.accountsPayableAccountID.Value,(iamount + pamount),
                        "RollOver from investment - " + (iamount + pamount).ToString("#,###.#0")
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.investmentNo, ent, User.Identity.Name,
                                dp.client.branchID);

                    ent.jnl_batch.Add(jb);
                    var maturityDate=DateTime.Now;
                    
                    var days = 0;
                    switch ((int)txtPeriod.Value.Value)
                    {
                        case 1:
                            days = 30;
                            break;
                        case 2:
                            days = 60;
                            break;
                        case 3:
                            days = 91;
                            break;
                        case 6:
                            days = 182;
                            break;
                        case 9:
                            days = 273;
                            break;
                        case 12:
                            days = 365;
                            break;
                        default:
                            break;
                    }
                    if (days == 0) maturityDate = dtAppDate.SelectedDate.Value.AddMonths((int)txtPeriod.Value.Value);
                    else
                    {
                        maturityDate = dtAppDate.SelectedDate.Value.AddDays(days);
                    }
                    var cnt = le.investments.Where(p => p.client.clientID == dp.client.clientID).Count();
                    char c = (char)(((int)'A') + cnt);
                    var vinvestmentNo = dp.client.accountNumber + "/" + c; 
                    if (pro.traditionalLoanNo == false)
                    {
                        vinvestmentNo = dp.investmentType.investmentTypeName.Substring(0, 2).ToUpper() + "" +
                            coreLogic.coreExtensions.NextSystemNumber(
                            "investment_" + dp.investmentType.investmentTypeName.Substring(0, 2).ToUpper());
                    }
                    var dp2 = new coreLogic.investment
                    {
                        amountInvested = iamount + pamount,
                        interestExpected = (iamount + pamount)
                        * ((maturityDate - dtAppDate.SelectedDate.Value).TotalDays / 365.0)
                        * ((txtRate.Value.Value/12.0) * 12/100.0),
                        autoRollover = false,
                        client = dp.client,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        investmentNo = vinvestmentNo,
                        investmentType = dp.investmentType,
                        firstinvestmentDate = dtAppDate.SelectedDate.Value,
                        interestAccumulated = 0,
                        interestAuthorized = 0,
                        interestBalance = 0,
                        interestMethod = dp.interestMethod,
                        interestRate = txtRate.Value.Value/12.0,
                        period = (int)txtPeriod.Value.Value,
                        maturityDate = maturityDate,
                        principalAuthorized = 0,
                        principalBalance = iamount + pamount,
                        principalRepaymentModeID = int.Parse(cboPrincRepaymentMode.SelectedValue),
                        interestRepaymentModeID = int.Parse(cboInterestRepaymentMode.SelectedValue),
                        lastInterestDate = null,
                        currencyID=dp.currencyID,
                        fxRate=dp.fxRate,
                        interestCalculationScheduleID=dp.interestCalculationScheduleID,
                        interestPayableAccountID=dp.interestPayableAccountID,
                        lastInterestFxGainLoss=0,
                        localAmount=iamount+pamount,
                        lastPrincipalFxGainLoss=0
                    };
                    le.investments.Add(dp2);

                    var da = new coreLogic.investmentAdditional
                    {
                        checkNo = "",
                        investmentAmount = iamount + pamount,
                        bankID = bankID,
                        interestBalance = 0,
                        investmentDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = pamount + iamount,
                        modeOfPayment = mop,
                        naration = "Rollover of Investment",
                        fxRate = dp.fxRate,
                        lastPrincipalFxGainLoss = 0,
                        localAmount = iamount + pamount,
                        posted = true
                    };
                    dp2.investmentAdditionals.Add(da);
                    var jb2 = journalextensions.Post("LN", acctID,
                        dp.investmentType.accountsPayableAccountID.Value, iamount + pamount,
                        "investment Re-investment - "
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.investmentNo, ent, User.Identity.Name,
                        dp.client.branchID);
                    ent.jnl_batch.Add(jb2);

                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date = dtAppDate.SelectedDate.Value;
                    int i = 1;
                    var totalInt = (iamount + pamount) * (txtPeriod.Value.Value) * (txtRate.Value.Value/12.0) / 100.0;
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
                        dp2.investmentSchedules.Add(new coreLogic.investmentSchedule
                        {
                            interestPayment = intererst,
                            principalPayment = princ,
                            repaymentDate = date2,
                            authorized = false
                        });
                    }

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Depoit Rolled Over Successfully", ResolveUrl("~/ln/investment/rollOver.aspx"), "coreERP©: Successful", IconType.ok);
                }
            }
        }

        protected void btnRollOverPrinc_Click(object sender, EventArgs e)
        {
            if (cboInvestment.SelectedValue != "")
            {
                int investmentID = int.Parse(cboInvestment.SelectedValue);
                var dp = le.investments.FirstOrDefault(p => p.investmentID == investmentID);
                if (dp != null)
                {
                    var iamount = 0;
                    var pamount = dp.principalBalance;

                    int mopID = 1;
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    var dw = new coreLogic.investmentWithdrawal
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
                        naration = "Rollover of Investment",
                        localAmount = dp.localAmount,
                        posted = true
                    };
                    dp.investmentWithdrawals.Add(dw);

                    dp.principalBalance = 0; 
                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var acctID = dp.investmentType.vaultAccountID.Value;// ent.accts.FirstOrDefault(p => p.acc_num == "1000").acct_id;
                    var pro = ent.comp_prof.FirstOrDefault();
                    var jb = journalextensions.Post("LN", dp.investmentType.accountsPayableAccountID.Value,
                        acctID, (iamount + pamount),
                        "Roll Over of investment - " + (iamount + pamount).ToString("#,###.#0")
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.investmentNo, ent, User.Identity.Name,
                        dp.client.branchID);

                    ent.jnl_batch.Add(jb);

                    var maturityDate = DateTime.Now;
                    var days = 0;
                    switch ((int)txtPeriod.Value.Value)
                    {
                        case 1:
                            days = 30;
                            break;
                        case 2:
                            days = 60;
                            break;
                        case 3:
                            days = 91;
                            break;
                        case 6:
                            days = 182;
                            break;
                        case 9:
                            days = 273;
                            break;
                        case 12:
                            days = 365;
                            break;
                        default:
                            break;
                    }
                    if (days == 0) maturityDate = dtAppDate.SelectedDate.Value.AddMonths((int)txtPeriod.Value.Value);
                    else
                    {
                        maturityDate = dtAppDate.SelectedDate.Value.AddDays(days);
                    }
                    var cnt = le.investments.Where(p => p.client.clientID == dp.client.clientID).Count();
                    char c = (char)(((int)'A') + cnt);
                    var vinvestmentNo = dp.client.accountNumber + "/" + c;
                    if (pro.traditionalLoanNo == false)
                    {
                        vinvestmentNo = dp.investmentType.investmentTypeName.Substring(0, 2).ToUpper() + "" +
                            coreLogic.coreExtensions.NextSystemNumber(
                            "investment_" + dp.investmentType.investmentTypeName.Substring(0, 2).ToUpper());
                    }
                    var dp2 = new coreLogic.investment
                    {
                        amountInvested = pamount,
                        interestExpected = (pamount)
                        * ((maturityDate - dtAppDate.SelectedDate.Value).TotalDays / 365.0)
                        * ((txtRate.Value.Value / 12.0) * 12 / 100.0),
                        autoRollover = false,
                        client = dp.client,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        investmentNo = vinvestmentNo,
                        investmentType = dp.investmentType,
                        firstinvestmentDate = dtAppDate.SelectedDate.Value,
                        interestAccumulated = 0,
                        interestAuthorized = 0,
                        interestBalance = 0,
                        interestMethod = dp.interestMethod,
                        interestRate = txtRate.Value.Value/12.0,
                        period = (int)txtPeriod.Value.Value,
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
                        lastPrincipalFxGainLoss = 0
                    };
                    le.investments.Add(dp2);

                    var da = new coreLogic.investmentAdditional
                    {
                        checkNo = "",
                        investmentAmount = iamount + pamount,
                        bankID = bankID,
                        interestBalance = 0,
                        investmentDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = pamount + iamount,
                        modeOfPayment = mop,
                        naration="Rollover of Investment",
                        fxRate=dp.fxRate,
                        lastPrincipalFxGainLoss=0,
                        localAmount=iamount+pamount,
                        posted=true
                    };
                    dp2.investmentAdditionals.Add(da);
                    var jb2 = journalextensions.Post("LN", acctID,
                        dp.investmentType.accountsPayableAccountID.Value, iamount + pamount,
                        "investment Re-investment - "
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.investmentNo, ent, User.Identity.Name,
                        dp.client.branchID);
                    ent.jnl_batch.Add(jb2);

                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date = dtAppDate.SelectedDate.Value;
                    int i = 1;
                    var totalInt = (iamount + pamount) * (txtPeriod.Value.Value) * (txtRate.Value.Value/12.0) / 100.0;
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
                        dp2.investmentSchedules.Add(new coreLogic.investmentSchedule
                        {
                            interestPayment = intererst,
                            principalPayment = princ,
                            repaymentDate = date2,
                            authorized = false
                        });
                    }

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Depoit Rolled Over Successfully", ResolveUrl("~/ln/investment/rollOver.aspx"), "coreERP©: Successful", IconType.ok);

                }
            }
        }

        protected void btnRollOverInt_Click(object sender, EventArgs e)
        {
            if (cboInvestment.SelectedValue != "")
            {
                int investmentID = int.Parse(cboInvestment.SelectedValue);
                var dp = le.investments.FirstOrDefault(p => p.investmentID == investmentID);
                if (dp != null)
                {
                    var iamount = dp.interestBalance;
                    var pamount = 0;

                    int mopID = 1;
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    var dw = new coreLogic.investmentWithdrawal
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
                        naration = "Rollover of Investment",
                        localAmount = dp.localAmount,
                        posted = true
                    };
                    dp.investmentWithdrawals.Add(dw);

                    dp.principalBalance = 0;
                    dp.interestBalance = 0;
                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    //dp.clientReference.Load();
                    //dp.investmentTypeReference.Load();
                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var acctID = ent.accts.FirstOrDefault(p => p.acc_num == "1000").acct_id;
                    var pro = ent.comp_prof.FirstOrDefault();
                    var jb = journalextensions.Post("LN", dp.investmentType.accountsPayableAccountID.Value,
                        acctID, (iamount + pamount),
                        "RollOver from investment - " + (iamount + pamount).ToString("#,###.#0")
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.investmentNo, ent, User.Identity.Name,
                        dp.client.branchID);

                    ent.jnl_batch.Add(jb);

                    var maturityDate = DateTime.Now;
                    var days = 0;
                    switch ((int)txtPeriod.Value.Value)
                    {
                        case 1:
                            days = 30;
                            break;
                        case 2:
                            days = 60;
                            break;
                        case 3:
                            days = 91;
                            break;
                        case 6:
                            days = 182;
                            break;
                        case 9:
                            days = 273;
                            break;
                        case 12:
                            days = 365;
                            break;
                        default:
                            break;
                    }
                    if (days == 0) maturityDate = dtAppDate.SelectedDate.Value.AddMonths((int)txtPeriod.Value.Value);
                    else
                    {
                        maturityDate = dtAppDate.SelectedDate.Value.AddDays(days);
                    }
                    var cnt = le.investments.Where(p => p.client.clientID == dp.client.clientID).Count();
                    char c = (char)(((int)'A') + cnt);
                    var vinvestmentNo = dp.client.accountNumber + "/" + c;
                    if (pro.traditionalLoanNo == false)
                    {
                        vinvestmentNo = dp.investmentType.investmentTypeName.Substring(0, 2).ToUpper() + "" +
                            coreLogic.coreExtensions.NextSystemNumber(
                            "investment_" + dp.investmentType.investmentTypeName.Substring(0, 2).ToUpper());
                    }
                    var dp2 = new coreLogic.investment
                    {
                        amountInvested = iamount,
                        interestExpected = (iamount )
                        * ((maturityDate - dtAppDate.SelectedDate.Value).TotalDays / 365.0)
                        * ((txtRate.Value.Value / 12.0) * 12 / 100.0),
                        autoRollover = false,
                        client = dp.client,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        investmentNo = vinvestmentNo,
                        investmentType = dp.investmentType,
                        firstinvestmentDate = dtAppDate.SelectedDate.Value,
                        interestAccumulated = 0,
                        interestAuthorized = 0,
                        interestBalance = 0,
                        interestMethod = dp.interestMethod,
                        interestRate = txtRate.Value.Value/12.0,
                        period = (int)txtPeriod.Value.Value,
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
                        lastPrincipalFxGainLoss = 0
                    };
                    le.investments.Add(dp2);

                    var da = new coreLogic.investmentAdditional
                    {
                        checkNo = "",
                        investmentAmount = iamount + pamount,
                        bankID = bankID,
                        interestBalance = 0,
                        investmentDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = pamount + iamount,
                        modeOfPayment = mop,
                        naration = "Rollover of Investment",
                        fxRate = dp.fxRate,
                        lastPrincipalFxGainLoss = 0,
                        localAmount = iamount + pamount,
                        posted = true
                    };
                    dp2.investmentAdditionals.Add(da);
                    var jb2 = journalextensions.Post("LN", acctID,
                        dp.investmentType.accountsPayableAccountID.Value, iamount + pamount,
                        "investment Re-investment - "
                        + " - " + dp.client.accountNumber + " - " + dp.client.surName + "," + dp.client.otherNames,
                        pro.currency_id.Value, dtAppDate.SelectedDate.Value, dp.investmentNo, ent, User.Identity.Name,
                        dp.client.branchID);
                    ent.jnl_batch.Add(jb2);

                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date = dtAppDate.SelectedDate.Value;
                    int i = 1;
                    var totalInt = (iamount + pamount) * (txtPeriod.Value.Value) * (txtRate.Value.Value/12.0) / 100.0;
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
                        dp2.investmentSchedules.Add(new coreLogic.investmentSchedule
                        {
                            interestPayment = intererst,
                            principalPayment = princ,
                            repaymentDate = date2,
                            authorized = false
                        });
                    }

                    le.SaveChanges();
                    ent.SaveChanges();
                    HtmlHelper.MessageBox2("Depoit Rolled Over Successfully", ResolveUrl("~/ln/investment/rollOver.aspx"), "coreERP©: Successful", IconType.ok);

                }
            }
        }
    }
}