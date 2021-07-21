using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class disburseIL_O : System.Web.UI.Page
    {
        IDisbursementsManager disbMgr = new DisbursementsManager();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.invoiceLoan ln;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {
                le = new coreLogic.coreLoansEntities();
                Session["le"] = le;

                cboSupplier.Items.Add(new RadComboBoxItem("", ""));
                foreach (var r in le.suppliers)
                {
                    cboSupplier.Items.Add(new RadComboBoxItem(r.supplierName, r.supplierID.ToString()));
                }

                this.cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }

                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    ln = le.invoiceLoans.FirstOrDefault(p => p.invoiceLoanID == id);

                    if (ln != null)
                    {
                        client = ln.client; 
                        cboClient.Items.Add(new RadComboBoxItem(client.surName + ", " + client.otherNames +
                            "  (" + client.accountNumber + ")", client.clientID.ToString()));
                        cboClient.SelectedValue = client.clientID.ToString();
                        txtSurname.Text = client.surName;
                        txtOtherNames.Text = client.otherNames;
                        txtAccountNo.Text = client.accountNumber;
                        chkAddFees.Checked = ln.addFee;
                         
                        Session["loan.cl"] = client;
                        RenderImages();
                        txtDesc.Text = ln.invoiceDescription;
                        txtDisbAmt.Value = ln.proposedAmount;
                        txtDisbRate.Value = ln.ceilRate;
                        txtWith.Value = ln.withHoldingTax;
                        txtAmount.Value = ln.invoiceAmount;
                        if (ln.supplierID != null)
                        {
                            cboSupplier.SelectedValue = ln.supplierID.Value.ToString();
                        }
                        txtInvoiceNo.Text = ln.invoiceNo;
                    }

                    Session["invoiceLoan"] = ln;
                }
                else
                {
                    ln = new coreLogic.invoiceLoan();
                    Session["invoiceLoan"] = ln;
                }

            }
            else
            {
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                }
                if (Session["invoiceLoan"] != null)
                {
                    ln = Session["invoiceLoan"] as coreLogic.invoiceLoan;
                }
                else
                {
                    ln = new coreLogic.invoiceLoan();
                    Session["invoiceLoan"] = ln;
                }
                if (Session["le"] != null)
                {
                    le = Session["le"] as coreLogic.coreLoansEntities;
                }
                else
                {
                    le = new coreLogic.coreLoansEntities();
                    Session["le"] = le;
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
                    img.Width = 209;
                    img.Height = 113;
                    img.ResizeMode = BinaryImageResizeMode.Fit;
                    img.DataValue = item.image.image1;
                    RadRotatorItem it = new RadRotatorItem();
                    it.Controls.Add(img);
                    rotator1.Items.Add(it);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAmount.Value != null
                && txtDisbRate.Value != null
                && txtDisbAmt.Value != null
                && txtWith.Value != null
                && dtAppDate.SelectedDate != null
                && ln.disbursed == false
                && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1")))
            {
                ln.invoiceAmount = txtAmount.Value.Value;
                ln.invoiceDate = dtAppDate.SelectedDate.Value;
                ln.clientID = client.clientID;
                ln.ceilRate = txtDisbRate.Value.Value;
                ln.proposedAmount = txtDisbAmt.Value.Value;
                ln.withHoldingTax = txtWith.Value.Value ;
                ln.invoiceDescription = txtDesc.Text;
                ln.approvedBy = ""; 
                var t = le.tenors.FirstOrDefault(p=>p.tenor1==6);
                var l = new coreLogic.loan
                { 
                    amountApproved = ln.proposedAmount,
                    amountRequested = ln.proposedAmount,
                    applicationDate = dtAppDate.SelectedDate.Value,
                    processingFee = Math.Ceiling(ln.proposedAmount * t.defaultProcessingFeeRate.Value / 100.0),
                    applicationFee = 0,
                    applicationFeeBalance = 0, 
                    clientID = client.clientID,
                    commission = 0,
                    commissionBalance = 0,
                    creation_date = DateTime.Now,
                    creator = User.Identity.Name,
                    creditOfficerNotes = "",
                    gracePeriod = t.defaultGracePeriod.Value,
                    interestRate = ln.rate,
                    interestTypeID = 1,
                    loanNo = "IL" + coreLogic.coreExtensions.NextSystemNumber(
                        "LOAN_IL"),
                    loanStatusID = 4,
                    repaymentModeID = -1,
                    tenureTypeID = 1,
                    loanTenure = 0,
                    loanType = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 5),
                    loanTypeID = 5,
                    client = client,
                    approvalComments = "",
                    invoiceNo = txtInvoiceNo.Text,
                    addFeesToPrincipal=chkAddFees.Checked
                };
                int? bankID=null;
                if(cboBank.SelectedValue!="")bankID=int.Parse(cboBank.SelectedValue);
                var cd = new coreLogic.cashierDisbursement
                {
                    addFees = chkAddFees.Checked,
                    amount = ln.proposedAmount,
                    bankID = bankID,
                    paymentModeID = int.Parse(cboPaymentType.SelectedValue),
                    posted = true,
                    txDate = dtAppDate.SelectedDate.Value,
                    cashiersTill = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.ToLower().Trim()),
                    checkNo = txtCheckNo.Text,
                    loan = l,
                    clientID = ln.clientID
                };
                le.cashierDisbursements.Add(cd);
                le.loans.Add(l);
                var pm = "1";
                if (txtCheckNo.Text != "") pm = "2";
              
                disbMgr.PostLoan(le, l, ln.proposedAmount, ln.proposedAmount,
                    dtAppDate.SelectedDate.Value, cboBank.SelectedValue, cboPaymentType.SelectedValue, txtCheckNo.Text, ent, l.addFeesToPrincipal,
                    User.Identity.Name, cboPaymentType.SelectedValue, null, null); 
                ln.amountDisbursed = ln.proposedAmount;
                ln.disbursed = true;
                ln.approved = true;
                le.SaveChanges();
                ent.SaveChanges();

                HtmlHelper.MessageBox2("Invoice Loan Disbursement Data Saved Successfully!", ResolveUrl("~/ln/loans/default2.aspx"), "coreERP©: Successful", IconType.ok);  
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).ToList();
            else
                clients = le.clients.ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID==3||item.clientTypeID==4||item.clientTypeID==5)?item.companyName:item.surName +
                ", " + item.otherNames + " (" + item.accountNumber + ")", item.clientID.ToString()));
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
                    rotator1.Items.Clear();

                    RenderImages();
                }
            }
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            txtWith.Value = txtAmount.Value * 5.0 / 100.0;
        }

        protected void txtDisbRate_TextChanged(object sender, EventArgs e)
        { 
            txtDisbAmt.Value=(txtAmount.Value.Value-txtWith.Value.Value)*txtDisbRate.Value.Value/100.0;
        }
    }
}