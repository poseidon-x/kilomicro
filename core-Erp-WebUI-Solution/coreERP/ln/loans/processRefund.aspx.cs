using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreERP.code;

namespace coreERP.ln.loans
{
    public partial class processRefund : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.IJournalExtensions je;

        protected void Page_Load(object sender, EventArgs e)
        {
            le=new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            je = new coreLogic.JournalExtensions();

            if (!Page.IsPostBack)
            {
                dtDate.SelectedDate = DateTime.Now;
                var data = le.multiPaymentClients.Where(p => p.approved == true && p.balance>0
                    && p.refunded==false).ToList();
                foreach(var dat in data)
                {
                    foreach(var mp in dat.multiPayments)
                    {
                        var ln = le.loans.FirstOrDefault(p => p.loanID == mp.loanID);
                        if(ln!= null)
                        {
                            var cl = ln.client;
                            if (cl != null)
                            {
                                dat.clientName = (cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5)
                                        ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")";
                                break;
                            }
                        }
                    }
                }
                gridSchedule.DataSource = data;
                gridSchedule.DataBind();

                cboBank.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new RadComboBoxItem(r.bank_acct_desc, r.bank_acct_id.ToString()));
                }

                cboModeOfPayment.Items.Add(new RadComboBoxItem("Cash", "1"));
                cboModeOfPayment.Items.Add(new RadComboBoxItem("Cheque", "2"));
                cboModeOfPayment.Items.Add(new RadComboBoxItem("Bank Transfer", "3"));
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            bool chnged = false;
            foreach (var item in gridSchedule.Items)
            {
                if (item is GridDataItem)
                {
                    var item2 = item as GridDataItem;
                    if (item2.Selected)
                    {
                        var key = item2.GetDataKeyValue("multiPaymentClientID").ToString();
                        var rpID = int.Parse(key);
                        var rp = le.multiPaymentClients.FirstOrDefault(p => p.multiPaymentClientID == rpID);
                        if (rp != null)
                        {
                            List<coreReports.vwLoanBalance> balances = new List<coreReports.vwLoanBalance>();
                            foreach (var it in rp.multiPayments)
                            {
                                balances.Add(new coreReports.vwLoanBalance
                                {
                                    amountPaid = (decimal)it.amountPaid,
                                    description = "Payment of loan " + it.loanNo,
                                    disbursementDate = it.disbursementDate,
                                    interestOutstanding = it.interestOutstanding,
                                    principalOutstanding = it.principalOutstanding,
                                    processingFee = it.processingFee,
                                    invoiceNo = it.invoiceNo,
                                    loanID = it.loanID,
                                    loanNo = it.loanNo
                                });
                            }
                            coreReports.ln.rptMultiPmtRem rpt = new coreReports.ln.rptMultiPmtRem();
                            rpt.SetDataSource(balances);
                            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());

                            rpt.SetParameterValue("companyName", Settings.companyName);
                            rpt.SetParameterValue("invoiceDate", rp.invoiceDate);
                            rpt.SetParameterValue("clientName", rp.clientName);
                            rpt.SetParameterValue("amount", rp.balance);

                            this.rvw.ReportSource = rpt;
                            rvw.DataBind();
                        }
                    }
                }
            }

            if (chnged == true)
            {
                le.SaveChanges();
                HtmlHelper.MessageBox2("Loan refunds successfully apporved",
                    ResolveUrl("/ln/loans/approveRefund.aspx"), "coreERP©: Failed", IconType.deny);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            bool chnged = false;
            coreLogic.JournalExtensions je = new coreLogic.JournalExtensions();

            using (var le = new coreLogic.coreLoansEntities())
            {
                using (var ent = new coreLogic.core_dbEntities())
                {
                    foreach (var item in gridSchedule.Items)
                    {
                        if (item is GridDataItem)
                        {
                            var item2 = item as GridDataItem;
                            if (item2.Selected)
                            {
                                var key = item2.GetDataKeyValue("multiPaymentClientID").ToString();
                                var rpID = int.Parse(key);
                                var rp = le.multiPaymentClients.FirstOrDefault(p => p.multiPaymentClientID == rpID && p.refunded == false);
                                if (rp != null)
                                {
                                    var acc = ent.def_accts.FirstOrDefault(p => p.code == "RF");
                                    if (acc != null)
                                    {
                                        var acctID = rp.cashierReceipt.loan.loanType.vaultAccountID;

                                        int modeOfPaymentID = int.Parse(cboModeOfPayment.SelectedValue);
                                        if (modeOfPaymentID == 2 || modeOfPaymentID == 3)
                                        {
                                            int bankID = int.Parse(cboBank.SelectedValue);
                                            var bank = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                                            acctID = bank.accts.acct_id;
                                        }

                                        var jb = je.Post("LN", acc.accts.acct_id, acctID, rp.balance, "Client Refund " + rp.cashierReceipt.client.surName
                                            + ", " + rp.cashierReceipt.client.otherNames, ent.comp_prof.FirstOrDefault().currency_id.Value,
                                            dtDate.SelectedDate.Value, rp.cashierReceipt.loan.loanNo, ent, User.Identity.Name, rp.cashierReceipt.client.branchID);
                                        ent.jnl_batch.Add(jb);

                                        rp.refunded = true;
                                        rp.balance = 0;
                                        chnged = true;
                                    }
                                }
                            }
                        }
                    }

                    if (chnged == true)
                    {
                        le.SaveChanges();
                        ent.SaveChanges();
                        HtmlHelper.MessageBox2("Loan refunds successfully posted",
                            ResolveUrl("/ln/loans/processRefund.aspx"), "coreERP©: Failed", IconType.deny);
                    }
                }
            }
        }
    }
}