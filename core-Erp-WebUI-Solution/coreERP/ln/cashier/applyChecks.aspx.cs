using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreERP.code;
using coreData.ErrorLog;
using coreERP.Models.LoanRepaymentUpload;
using System.Data.Entity;

namespace coreERP.ln.cashier
{
    public partial class applyChecks : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        string catID = "";
        coreLogic.loanCheck check;
        coreLogic.multiPaymentClient mpc;
        List<LoanModel> lns = new List<LoanModel>();

        protected void Page_Load(object sender, EventArgs e)
        {
            catID = Request.Params["catID"];
            if (catID == null) catID = "1";
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                dtDate.SelectedDate = DateTime.Now.Date;

                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }
            }

            if (Request.Params["checkID"] != null && Request.Params["checkID"].Trim() != "")
            {
                int checkID = int.Parse(Request.Params["checkID"]);
                check = le.loanChecks.FirstOrDefault(p => p.loanCheckID == checkID);
                if (check != null)
                {
                    if (!IsPostBack)
                    {
                        dtDate.SelectedDate = check.checkDate;
                        txtValue.Value = check.balance;
                        if (check.bankID != null) cboBank.SelectedValue = check.bankID.ToString();
                        txtCheckNo.Text = check.checkNumber;
                        if (check.clientID != null)
                        {
                            cboClient.SelectedValue = check.clientID.ToString();
                            cboClient.Enabled = false;
                        }
                        if (check.bankID != null)
                        {
                            cboBank.SelectedValue = check.bankID.ToString();
                        }
                        cboPaymentMode.SelectedValue = "2";
                        cboPaymentMode.Enabled = false;
                        OnChange();
                        btnAllocate_Click(btnAllocate, EventArgs.Empty);
                    }
                }
            }

            if (Request.Params["mpcID"] != null && Request.Params["mpcID"].Trim() != "")
            {
                int mpcID = int.Parse(Request.Params["mpcID"]);
                mpc = le.multiPaymentClients.FirstOrDefault(p => p.multiPaymentClientID == mpcID);
                if (mpcID != null)
                {
                    if (!IsPostBack)
                    {
                        dtDate.SelectedDate = mpc.invoiceDate;
                        txtValue.Value = mpc.balance;
                        if (mpc.cashierReceipt != null)
                        {
                            if (mpc.cashierReceipt.bankID != null) cboBank.SelectedValue = mpc.cashierReceipt.bankID.ToString();
                            cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((mpc.cashierReceipt.client.clientTypeID == 3
                                || mpc.cashierReceipt.client.clientTypeID == 4 || mpc.cashierReceipt.client.clientTypeID == 5) ?
                                 mpc.cashierReceipt.client.companyName : ((mpc.cashierReceipt.client.clientTypeID == 6) ? mpc.cashierReceipt.client.accountName : mpc.cashierReceipt.client.surName +
                                ", " + mpc.cashierReceipt.client.otherNames) + " (" + mpc.cashierReceipt.client.accountNumber
                                + ")", mpc.cashierReceipt.client.clientID.ToString()));
                            cboClient.SelectedValue = mpc.cashierReceipt.clientID.ToString();
                            cboClient.Enabled = false;
                        }
                        OnChange();
                        btnAllocate_Click(btnAllocate, EventArgs.Empty);
                    }
                }
            }
        }

        protected void cboClient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p =>
                     (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                        ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (txtValue.Value != null
                && ((cboPaymentMode.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentMode.SelectedValue == "1")))
            {
                List<coreReports.getLoanBalances_Result> loans = Session["loans"] as List<coreReports.getLoanBalances_Result>;
                if (loans != null)
                {
                    var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                    if (ct == null)
                    {
                        HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                        return;
                    }
                    var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtDate.SelectedDate.Value);
                    if (ctd == null)
                    {
                        HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                        return;
                    }
                    int? bankID = null;
                    if (cboBank.SelectedValue != null && cboBank.SelectedValue != "")
                        bankID = int.Parse(cboBank.SelectedValue);
                    int i = 0;
                    var amount = txtValue.Value.Value;
                    if (check != null && check.balance < amount)
                    {
                        HtmlHelper.MessageBox("The check balance amount is less than the amount being paid", "coreERP©: Failed", IconType.deny);
                        return;
                    }
                    List<coreReports.vwLoanBalance> balances = new List<coreReports.vwLoanBalance>();
                    coreLogic.cashierReceipt cd = null;
                    List<coreLogic.cashierReceipt> cds = new List<coreLogic.cashierReceipt>();
                    while (i < loans.Count)
                    {
                        GridItem item = gridLoans.Items[i];
                        var txt = item.FindControl("txtAmount") as RadNumericTextBox;
                        var payType = item.FindControl("cboPaymentType") as RadComboBox;
                        int payTypeId = int.Parse(payType.SelectedValue);

                        if (txt != null && txt.Value != null)
                        {
                            var loanID = loans[i].loanID;
                            var ln = le.loans.FirstOrDefault(p => p.loanID == loanID);
                            var addInterest = 0.0;
                            var princ = 0.0;
                            var procFee = 0.0;
                            var interest = 0.0;

                            if (payTypeId == 1)
                            {
                                princ = loans[i].principalOutstanding <= txt.Value.Value ? loans[i].principalOutstanding : txt.Value.Value;
                                interest = loans[i].principalOutstanding + loans[i].interestOutstanding <= txt.Value.Value ?
                                     loans[i].interestOutstanding :
                                     ((loans[i].principalOutstanding < txt.Value.Value) ?
                                     txt.Value.Value - loans[i].principalOutstanding : 0);
                            }else if (payTypeId == 2)
                            {
                                princ = loans[i].principalOutstanding <= txt.Value.Value ? loans[i].principalOutstanding : txt.Value.Value;
                            }else if (payTypeId == 3)
                            {
                                interest = loans[i].interestOutstanding <= txt.Value.Value ? loans[i].interestOutstanding : txt.Value.Value;
                            }
                            else if (payTypeId == 6)
                            {
                                procFee = loans[i].processingFee <= txt.Value.Value ? loans[i].processingFee : txt.Value.Value;
                            }
                            else if (payTypeId == 8)
                            {
                                princ = loans[i].principalOutstanding <= txt.Value.Value ?
                                     loans[i].principalOutstanding : txt.Value.Value;
                                procFee = loans[i].principalOutstanding + loans[i].processingFee <= txt.Value.Value ?
                                     loans[i].processingFee :
                                     ((loans[i].principalOutstanding < txt.Value.Value) ?
                                     txt.Value.Value - loans[i].principalOutstanding : 0);
                                interest = txt.Value.Value - princ - procFee - addInterest;
                            }


                            if (interest > loans[i].interestOutstanding)
                            {
                                addInterest = interest - loans[i].interestOutstanding;
                                interest = loans[i].interestOutstanding;
                            }
                            
                            if (addInterest > 0.05)
                            {
                                cd = new coreLogic.cashierReceipt
                                {
                                    amount = addInterest,
                                    bankID = bankID,
                                    checkNo = txtCheckNo.Text,
                                    clientID = ln.client.clientID,
                                    loanID = ln.loanID,
                                    paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                                    posted = false,
                                    txDate = dtDate.SelectedDate.Value,
                                    cashierTillID = ct.cashiersTillID,
                                    repaymentTypeID = 7,
                                    principalAmount = 0,
                                    interestAmount = 0,
                                    addInterestAmount = addInterest,
                                    feeAmount = 0
                                };
                                cds.Add(cd);
                                le.cashierReceipts.Add(cd);
                            }
                            if (payTypeId == 6)
                            {
                                cd = new coreLogic.cashierReceipt
                                {
                                    amount = procFee,
                                    bankID = bankID,
                                    checkNo = txtCheckNo.Text,
                                    clientID = ln.client.clientID,
                                    loanID = ln.loanID,
                                    paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                                    posted = false,
                                    txDate = dtDate.SelectedDate.Value,
                                    cashierTillID = ct.cashiersTillID,
                                    repaymentTypeID = 6,
                                    principalAmount = 0,
                                    interestAmount = 0,
                                    addInterestAmount = 0,
                                    feeAmount = procFee
                                };
                                cds.Add(cd);
                                le.cashierReceipts.Add(cd);
                            }
                            if (payTypeId == 1)
                            {
                                cd = new coreLogic.cashierReceipt
                                {
                                    amount = princ + interest,
                                    bankID = bankID,
                                    checkNo = txtCheckNo.Text,
                                    clientID = ln.client.clientID,
                                    loanID = ln.loanID,
                                    paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                                    posted = false,
                                    txDate = dtDate.SelectedDate.Value,
                                    cashierTillID = ct.cashiersTillID,
                                    repaymentTypeID = 1,
                                    principalAmount = princ,
                                    interestAmount = interest,
                                    addInterestAmount = 0,
                                    feeAmount = 0
                                };
                                cds.Add(cd);
                                if (interest >= loans[i].interestOutstanding)
                                {
                                    cd.writeOff = true;
                                }
                                le.cashierReceipts.Add(cd);
                            }
                            if (payTypeId == 2)
                            {
                                cd = new coreLogic.cashierReceipt
                                {
                                    amount = princ,
                                    bankID = bankID,
                                    checkNo = txtCheckNo.Text,
                                    clientID = ln.client.clientID,
                                    loanID = ln.loanID,
                                    paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                                    posted = false,
                                    txDate = dtDate.SelectedDate.Value,
                                    cashierTillID = ct.cashiersTillID,
                                    repaymentTypeID = 2,
                                    principalAmount = princ,
                                    interestAmount = interest,
                                    addInterestAmount = 0,
                                    feeAmount = 0
                                };
                                cds.Add(cd);
                                le.cashierReceipts.Add(cd);
                            }
                            if (payTypeId == 3)
                            {
                                cd = new coreLogic.cashierReceipt
                                {
                                    amount = interest,
                                    bankID = bankID,
                                    checkNo = txtCheckNo.Text,
                                    clientID = ln.client.clientID,
                                    loanID = ln.loanID,
                                    paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                                    posted = false,
                                    txDate = dtDate.SelectedDate.Value,
                                    cashierTillID = ct.cashiersTillID,
                                    repaymentTypeID = 3,
                                    principalAmount = princ,
                                    interestAmount = interest,
                                    addInterestAmount = 0,
                                    feeAmount = 0
                                };
                                cds.Add(cd);
                                if (interest >= loans[i].interestOutstanding)
                                {
                                    cd.writeOff = true;
                                }
                                le.cashierReceipts.Add(cd);
                            }
                            if (payTypeId == 8)
                            {
                                cd = new coreLogic.cashierReceipt
                                {
                                    amount = princ + interest,
                                    bankID = bankID,
                                    checkNo = txtCheckNo.Text,
                                    clientID = ln.client.clientID,
                                    loanID = ln.loanID,
                                    paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                                    posted = false,
                                    txDate = dtDate.SelectedDate.Value,
                                    cashierTillID = ct.cashiersTillID,
                                    repaymentTypeID = 1,
                                    principalAmount = princ,
                                    interestAmount = interest,
                                    addInterestAmount = 0,
                                    feeAmount = 0
                                };
                                cds.Add(cd);
                                le.cashierReceipts.Add(cd);

                                cd = new coreLogic.cashierReceipt
                                {
                                    amount = procFee,
                                    bankID = bankID,
                                    checkNo = txtCheckNo.Text,
                                    clientID = ln.client.clientID,
                                    loanID = ln.loanID,
                                    paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                                    posted = false,
                                    txDate = dtDate.SelectedDate.Value,
                                    cashierTillID = ct.cashiersTillID,
                                    repaymentTypeID = 6,
                                    principalAmount = 0,
                                    interestAmount = 0,
                                    addInterestAmount = 0,
                                    feeAmount = procFee
                                };
                                cds.Add(cd);
                                le.cashierReceipts.Add(cd);
                            }

                            txt.Value = princ + interest + procFee + addInterest;
                            amount -= txt.Value.Value;
                            if (check != null)
                            {
                                check.balance = 0;
                            }
                            if (mpc != null)
                            {
                                mpc.balance -= txt.Value.Value;
                            }

                            balances.Add(new coreReports.vwLoanBalance
                            {
                                amountPaid = (decimal)Math.Ceiling(txt.Value.Value),
                                description = "Payment of loan " + ln.loanNo,
                                disbursementDate = ln.disbursementDate.Value,
                                interestOutstanding = interest,
                                principalOutstanding = princ,
                                processingFee = procFee,
                                invoiceNo = ln.invoiceNo,
                                loanID = ln.loanID,
                                loanNo = ln.loanNo
                            });
                            cd.multiPayments.Add(new coreLogic.multiPayment
                            {
                                amountPaid = Math.Ceiling(txt.Value.Value),
                                description = "Payment of loan " + ln.loanNo,
                                disbursementDate = ln.disbursementDate.Value,
                                interestOutstanding = interest,
                                principalOutstanding = princ,
                                processingFee = procFee,
                                invoiceNo = ln.invoiceNo,
                                loanID = ln.loanID,
                                loanNo = ln.loanNo
                            });
                            if (addInterest > 0.05)
                            {
                                balances.Add(new coreReports.vwLoanBalance
                                {
                                    amountPaid = (decimal)addInterest,
                                    description = "Additional Interest on loan " + ln.loanNo,
                                    disbursementDate = ln.disbursementDate.Value,
                                    interestOutstanding = addInterest,
                                    principalOutstanding = 0,
                                    processingFee = 0,
                                    invoiceNo = ln.invoiceNo,
                                    loanID = ln.loanID,
                                    loanNo = ln.loanNo
                                });
                                cd.multiPayments.Add(new coreLogic.multiPayment
                                {
                                    amountPaid = addInterest,
                                    description = "Additional Interest on loan " + ln.loanNo,
                                    disbursementDate = ln.disbursementDate.Value,
                                    interestOutstanding = addInterest,
                                    principalOutstanding = 0,
                                    processingFee = 0,
                                    invoiceNo = ln.invoiceNo,
                                    loanID = ln.loanID,
                                    loanNo = ln.loanNo
                                });
                            }
                        }
                        i += 1;
                    }

                    txtRemAmt.Value = amount;
                    
                    if (cd != null)
                    {
                        var mrc = new coreLogic.multiPaymentClient
                        {
                            amount = amount,
                            invoiceDate = dtDate.SelectedDate.Value,
                            clientName = cboClient.Text,
                            balance = amount,
                            refunded = false,
                            approved = false,
                            checkAmount = txtValue.Value.Value,
                            posted = false
                        };
                        cd.multiPaymentClients.Add(mrc);

                        foreach (var c in cds)
                        {
                            foreach (var mr in c.multiPayments)
                            {
                                mr.multiPaymentClient = mrc;
                            }
                        }
                    }
                    le.SaveChanges();
                    ent.SaveChanges();

                    ////Logger.informationLog(txtRemAmt.Value.ToString());
                    ////if (txtRemAmt == null)
                    ////{
                    ////    Logger.informationLog("Remaining amount is null");
                    ////}
                    ////else
                    ////{
                    ////    Logger.informationLog("Remaining amount is not null");
                    ////} 


                    if (txtRemAmt != null)
                    {
                        coreReports.ln.rptMultiPmtRem rpt = new coreReports.ln.rptMultiPmtRem();
                        rpt.SetDataSource(balances);
                        rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());

                        rpt.SetParameterValue("companyName", Settings.companyName);
                        rpt.SetParameterValue("invoiceDate", dtDate.SelectedDate.Value);
                        rpt.SetParameterValue("clientName", cboClient.Text);
                        rpt.SetParameterValue("amount", amount);

                        this.rvw.ReportSource = rpt;
                        rvw.DataBind();
                        OnChange();
                    }
                    else
                    {
                        Response.Redirect("~/");
                    }
                    HtmlHelper.MessageBox2("Data Saved Successfully!"
                        , ResolveUrl("~/"), "coreERP©: Success", IconType.ok);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            if (dtDate.SelectedDate != null)
            {
                List<coreReports.getLoanBalances_Result> loans = new List<coreReports.getLoanBalances_Result>();
                if (cboClient.SelectedValue != "")
                {
                    int id = int.Parse(cboClient.SelectedValue);
                    var cl = le.clients.FirstOrDefault(p => p.clientID == id);
                    if (cl != null)
                    {
                        var rent = new coreReports.reportEntities();
                        rent.Database.CommandTimeout = 5000;
                        loans = rent.getLoanBalances(id, dtDate.SelectedDate.Value.Date).ToList();

                        foreach (var loan in loans)
                        {
                            double interestBal = 0;
                            var ln = le.loans.FirstOrDefault(p => p.loanID == loan.loanID);
                            if (ln.loanTypeID == 5)
                            {
                                var invoiceLn = le.invoiceLoans.FirstOrDefault(p => p.invoiceNo == ln.invoiceNo);
                                var daysDiff = (DateTime.Today - loan.disbursementDate).Days;
                                var rate = invoiceLn == null ? ln.interestRate : invoiceLn.rate;
                                var expectedInterest = ((rate / 100.0) * ln.amountDisbursed) * daysDiff / 30.0;
                                interestBal = expectedInterest - ln.loanRepayments.Sum(p => p.interestPaid);
                            }
                            else 
                            {
                                var payment = le.loanRepayments.Where(p => p.loanID == loan.loanID).ToList();
                                var daysDiff = (DateTime.Today - loan.disbursementDate).Days;
                                var rate = ln.interestRate;
                                var expectedInterest = ((rate / 100.0) * ln.amountDisbursed) * daysDiff / 30.0;
                                interestBal = expectedInterest - ln.loanRepayments.Sum(p => p.interestPaid);
                            }
                            

                            loan.interestOutstanding = interestBal;
                            loan.totalOutstanding = Math.Round(interestBal + loan.principalOutstanding + loan.processingFee,2);
                        }
                    }
                }
                Session["loans"] = loans;
                gridLoans.DataSource = loans;
                gridLoans.DataBind();
            }
        }

        protected void btnAllocate_Click(object sender, EventArgs e)
        {
            if (txtValue.Value != null)
            {
                List<coreReports.getLoanBalances_Result> loans = Session["loans"] as List<coreReports.getLoanBalances_Result>;
                if (loans != null)
                {
                    var amount = txtValue.Value.Value;
                    int i = 0;
                    while (amount > 0 && i < loans.Count)
                    {
                        var amt = 0.0;
                        if (loans[i].totalOutstanding <= amount)
                        {
                            amt = loans[i].totalOutstanding;
                        }
                        else
                        {
                            amt = amount;
                        }
                        GridItem item = gridLoans.Items[i];
                        var txt = item.FindControl("txtAmount") as RadNumericTextBox;
                        if (txt != null)
                        {
                            txt.Value = amt;
                        }
                        amount -= amt;
                        i++;
                    }
                    txtRemAmt.Value = amount;
                }
            }
        }

        protected void dtDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            OnChange();
        }

        protected void gridLoans_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            OnChange();
        }

        protected void btnAllocate2_Click(object sender, EventArgs e)
        {
            List<coreReports.getLoanBalances_Result> loans = Session["loans"] as List<coreReports.getLoanBalances_Result>;
            if (loans != null)
            {
                int i = 0;
                var amount = txtValue.Value.Value;
                List<coreReports.vwLoanBalance> balances = new List<coreReports.vwLoanBalance>();
                while (i < loans.Count)
                {
                    GridItem item = gridLoans.Items[i];
                    var txt = item.FindControl("txtAmount") as RadNumericTextBox;
                    var payType = item.FindControl("cboPaymentType") as RadComboBox;
                    int payTypeId = int.Parse(payType.SelectedValue);

                    if (txt != null && txt.Value != null)
                    {
                        var loanID = loans[i].loanID;
                        var ln = le.loans.FirstOrDefault(p => p.loanID == loanID);
                        var addInterest = 0.0;
                        var princ = 0.0;
                        var procFee = 0.0;
                        var interest = 0.0;

                        if (payTypeId == 1)
                        {
                            princ = loans[i].principalOutstanding <= txt.Value.Value ? loans[i].principalOutstanding : txt.Value.Value;
                            interest = loans[i].principalOutstanding + loans[i].interestOutstanding <= txt.Value.Value ?
                                 loans[i].interestOutstanding :
                                 ((loans[i].principalOutstanding < txt.Value.Value) ?
                                 txt.Value.Value - loans[i].principalOutstanding : 0);
                        }
                        else if (payTypeId == 2)
                        {
                            princ = loans[i].principalOutstanding <= txt.Value.Value ? loans[i].principalOutstanding : txt.Value.Value;
                        }
                        else if (payTypeId == 3)
                        {
                            interest = loans[i].interestOutstanding <= txt.Value.Value ? loans[i].interestOutstanding : txt.Value.Value;
                        }
                        else if (payTypeId == 6)
                        {
                            procFee = loans[i].processingFee <= txt.Value.Value ? loans[i].processingFee : txt.Value.Value;
                        }
                        else if (payTypeId == 8)
                        {
                            princ = loans[i].principalOutstanding <= txt.Value.Value ?
                                 loans[i].principalOutstanding : txt.Value.Value;
                            procFee = loans[i].principalOutstanding + loans[i].processingFee <= txt.Value.Value ?
                                 loans[i].processingFee :
                                 ((loans[i].principalOutstanding < txt.Value.Value) ?
                                 txt.Value.Value - loans[i].principalOutstanding : 0);
                            interest = txt.Value.Value - princ - procFee - addInterest;
                        }


                        if (interest > loans[i].interestOutstanding)
                        {
                            addInterest = interest - loans[i].interestOutstanding;
                            interest = loans[i].interestOutstanding;
                        }

                        amount -= txt.Value.Value;
                        balances.Add(new coreReports.vwLoanBalance
                        {
                            amountPaid = (int)Math.Ceiling(txt.Value.Value),
                            description = "Payment of loan " + ln.loanNo,
                            disbursementDate = ln.disbursementDate.Value,
                            interestOutstanding = interest,
                            principalOutstanding = princ,
                            processingFee = procFee,
                            invoiceNo = ln.invoiceNo,
                            loanID = ln.loanID,
                            loanNo = ln.loanNo
                        });
                        if (addInterest > 0.01)
                        {
                            balances.Add(new coreReports.vwLoanBalance
                            {
                                amountPaid = (int)Math.Floor(addInterest),
                                description = "Additional Interest on loan " + ln.loanNo,
                                disbursementDate = ln.disbursementDate.Value,
                                interestOutstanding = addInterest,
                                principalOutstanding = 0,
                                processingFee = 0,
                                invoiceNo = ln.invoiceNo,
                                loanID = ln.loanID,
                                loanNo = ln.loanNo
                            });
                        }
                    }
                    i += 1;
                }
                txtRemAmt.Value = amount;
                if (txtRemAmt.Value != null)
                {
                    coreReports.ln.rptMultiPmtRem rpt = new coreReports.ln.rptMultiPmtRem();
                    rpt.SetDataSource(balances);
                    rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());

                    rpt.SetParameterValue("companyName", Settings.companyName);
                    rpt.SetParameterValue("invoiceDate", dtDate.SelectedDate.Value);
                    rpt.SetParameterValue("clientName", cboClient.Text);
                    rpt.SetParameterValue("amount", amount);

                    this.rvw.ReportSource = rpt;
                    rvw.DataBind();
                }
            }
        }
    }
}