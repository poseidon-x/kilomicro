using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class disburseIL : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.invoiceLoan ln;
        List<coreLogic.invoiceLoan> ils;
        coreLogic.invoiceLoanMaster ilm;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {
                ViewState["OverridenSupplier"] = null;
                ViewState["Overriden"] = null;
                Session["invoiceLoans"] = null;
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

                if (Request.Params["ilm"] != null)
                {
                    int id = int.Parse(Request.Params["ilm"]);
                    ilm = le.invoiceLoanMasters.FirstOrDefault(p => p.invoiceLoanMasterID == id);
                    if (ilm != null)
                    {
                        Session["invoiceLoanMaster"] = ilm;
                        Session["invoiceLoans"] = ilm.invoiceLoans.ToList();
                        grid.DataSource = ilm.invoiceLoans;
                        grid.DataBind();

                        if (ilm.client != null)
                        {
                            client = ilm.client;
                            RenderImages();
                            Session["loan.cl"] = client;

                            txtAccountNo.Text = client.accountNumber;
                            txtSurname.Text = client.surName;
                            txtOtherNames.Text = client.otherNames;

                            cboClient.Items.Add(new RadComboBoxItem(client.surName + ", " + client.otherNames, client.clientID.ToString()));
                            cboClient.SelectedValue = client.clientID.ToString();
                        }
                        if (ilm.disbursed == true || ilm.approved==false)
                        {
                            btnSave.Enabled = false;
                        }
                        dtAppDate.SelectedDate = ilm.invoiceDate;

                        var sup = le.suppliers.FirstOrDefault(p => p.supplierID == ilm.supplierID);
                        if (sup != null)
                        {
                            cboSupplier.Items.Add(new RadComboBoxItem(sup.supplierName, sup.supplierID.ToString()));
                            cboSupplier.SelectedValue = sup.supplierID.ToString();
                        }
                    }
                }
                else if (Request.Params["id"] != null)
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
                        
                        Session["loan.cl"] = client;
                        RenderImages();
                        txtDesc.Text = ln.invoiceDescription;
                        txtDisbAmt.Value = ln.proposedAmount;
                        txtDisbRate.Value = ln.ceilRate;
                        txtWith.Value = ln.withHoldingTax;
                        txtAmount.Value = ln.invoiceAmount;
                        var t = le.tenors.FirstOrDefault(p => p.tenor1 == 6);
                        txtRate.Value = t.defaultInterestRate;
                        if (ln.supplierID != null)
                        {
                            cboSupplier.SelectedValue = ln.supplierID.Value.ToString();
                        }
                        txtInvoiceNo.Text = ln.invoiceNo;
                        dtAppDate.SelectedDate = ln.invoiceDate;

                        if (ln.disbursed == true || ln.approved==false)
                        {
                            btnSave.Enabled = false;
                        }
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
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadLoan(id);
            }
        }
            
        private void RenderImages()
        {
            rotator1.Items.Clear();
            if (client!= null && client.clientImages != null)
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
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
            if (ct == null)
            {
                HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                return;
            }
            var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtApprDate.SelectedDate
                && p.open == true);
            if (ctd == null)
            {
                HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                return;
            }
            if (ils.Count > 0 && ilm != null)
            {
                if (ilm.invoiceLoanMasterID > 0)
                {
                    ilm = le.invoiceLoanMasters.FirstOrDefault(p => p.invoiceLoanMasterID == ilm.invoiceLoanMasterID); 
                    ilm.disbursed = true;
                } 
                foreach (var r in ils)
                {
                    if (r.invoiceLoanID > 0)
                    {
                        var ln = le.invoiceLoans.First(p => p.invoiceLoanID == r.invoiceLoanID);
                        var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                        if (user.accessLevel.disbursementLimit < ln.proposedAmount)
                        {
                            HtmlHelper.MessageBox("The amount to be disbursed is beyond your access level",
                                                        "coreERP©: Failed", IconType.deny);
                            return;
                        }
                        string ret = null;
                        //if ((ret = ValidateMaximumExposure(ln.supplierID, user)) != null)
                        //{
                        //    HtmlHelper.MessageBox(ret);
                        //    return;
                        //}
                        ln.disbursed = true;

                        var t = le.tenors.FirstOrDefault(p => p.tenor1 == 6);
                        if (t == null) t = le.tenors.FirstOrDefault();
                        var l = new coreLogic.loan
                        {
                            amountApproved = ln.proposedAmount,
                            amountRequested = ln.proposedAmount,
                            applicationDate = dtAppDate.SelectedDate.Value,
                            processingFee = ln.processingFee.Value,
                            processingFeeBalance = ln.processingFee.Value,
                            applicationFee = 0,
                            balance = 0,
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
                            loanStatusID = 3,
                            repaymentModeID = -1,
                            tenureTypeID = 1,
                            loanTenure = 0,
                            loanType = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 5),
                            loanTypeID = 5,
                            approvalComments = "",
                            invoiceNo = r.invoiceNo,
                            addFeesToPrincipal = false,
                            finalApprovalDate = ln.approvalDate
                        };
                        int? bankID = null;
                        if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                        var cd = new coreLogic.cashierDisbursement
                        {
                            addFees = false,
                            amount = ln.proposedAmount,
                            bankID = bankID,
                            paymentModeID = int.Parse(cboPaymentType.SelectedValue),
                            posted = false,
                            txDate = dtApprDate.SelectedDate.Value,
                            cashiersTill = ct,
                            checkNo = txtCheckNo.Text,
                            loan = l,
                            clientID = ln.clientID
                        };
                        le.cashierDisbursements.Add(cd);
                        le.loans.Add(l);
                        ln.amountDisbursed = ln.proposedAmount;
                        ln.disbursed = true; 
                    }
                }
                le.SaveChanges();

                HtmlHelper.MessageBox2("Invoice Loans Data Disbursed Successfully!", ResolveUrl("~/ln/loans/default3.aspx"), "coreERP©: Successful", IconType.ok);
            }
        }

        private string ValidateMaximumExposure(int? supplierId, users user)
        {
            using (var le2 = new coreLoansEntities())
            {
                var unsavedIls = le2.invoiceLoans.Where(p => p.supplierID == supplierId
                    && ilm.invoiceLoanMasterID != p.invoiceLoanMasterID)
                    .Where(p => p.disbursed == false).ToList();
                var unsavedIls2 = le2.invoiceLoans.Where(p => p.clientID == client.clientID
                    && ilm.invoiceLoanMasterID != p.invoiceLoanMasterID && p.supplierID == supplierId)
                    .Where(p => p.disbursed == false).ToList();

                var unsavedIlAmount = 0.0;
                var unsavedIlAmount2 = 0.0;
                if (unsavedIls.Any())
                {
                    unsavedIlAmount = unsavedIls.Sum(p => p.invoiceAmount);
                }
                if (unsavedIls2.Any())
                {
                    unsavedIlAmount2 = unsavedIls2.Sum(p => p.invoiceAmount);
                }

                var allOutstandingSupplierInvoices = (
                    from il in le2.invoiceLoans
                    from l in le2.loans
                    from rs in le2.repaymentSchedules  
                    where il.clientID == l.clientID && il.supplierID == supplierId
                    select new
                    {
                        balance = rs.loanID == null ? l.amountApproved : rs.interestBalance + rs.principalBalance,
                        amount = rs.loanID == null ? l.amountApproved : rs.interestPayment + rs.principalPayment
                    }
                    ).ToList();
                if (allOutstandingSupplierInvoices.Any() || unsavedIlAmount >0)
                {
                    double totalExposure = allOutstandingSupplierInvoices.Sum(p => p.balance) + unsavedIlAmount;
                    var supplier = le.suppliers.FirstOrDefault(p => p.supplierID == supplierId);
                    if (supplier != null)
                    {
                        if (totalExposure > supplier.maximumExposure)
                        {
                            if (ViewState["OverridenSupplier"] == null && user.accessLevelID >= 50)
                            {
                                ViewState["OverridenSupplier"] = "1";
                                return "Maximum Exposure for this Supplier will be Exceeded with this Disbursement.\n"
                                       +
                                       "To make an executive override, click the approve button again.";
                            }
                            else if (user.accessLevelID < 50)
                            {
                                //return "Maximum Exposure for this Client will be Exceeded with this Disbursement.\n"
                                //    + "You can get an executive to override the limits";
                            }
                        }
                    }
                }
                var allOutstandingClientInvoices = (
                    from il in le2.invoiceLoans
                    from l in le2.loans  
                    from rs in le2.repaymentSchedules 
                    where il.clientID == l.clientID && il.clientID == ilm.clientID
                    select new
                    {
                        balance = rs.loanID == null ? l.amountApproved : rs.interestBalance + rs.principalBalance,
                        amount = rs.loanID == null ? l.amountApproved : rs.interestPayment + rs.principalPayment
                    }
                    ).ToList();
                if (allOutstandingClientInvoices.Any() || unsavedIlAmount2>0)
                {
                    double totalExposure = allOutstandingClientInvoices.Sum(p => p.balance) + unsavedIlAmount2;
                    var config =
                        le.invoiceLoanConfigs.FirstOrDefault(
                            p => p.clientID == client.clientID && p.supplierID == supplierId);
                    if (config != null)
                    {
                        if (totalExposure > config.maximumExposure)
                        {
                            if (ViewState["Overriden"] == null && user.accessLevelID >= 50)
                            {
                                ViewState["Overriden"] = "1";
                                //return "Maximum Exposure for this Client will be Exceeded with this Approval.\n"
                                //       +
                                //       "To make an executive override, click the approve button again.";
                            }
                            else if (user.accessLevelID < 50)
                            {
                                //return "Maximum Exposure for this Client will be Exceeded with this Approval.\n"
                                //    + "You can get an executive to override the limits";
                            }
                        }
                    }
                }
            }
            return null;
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).ToList();
            else
                clients = le.clients.Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).ToList();
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
            OnChange();
        }

        private void OnChange()
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

                    txtAccountNo.Text = client.accountNumber;
                    txtSurname.Text = client.surName;
                    txtOtherNames.Text = client.otherNames;                     
                }
            }
            if ((txtRate.Value == null || txtRate.Value.Value == 0) && cboClient.SelectedValue != "" && cboSupplier.SelectedValue != "")
            {
                int cid = int.Parse(cboClient.SelectedValue);
                int sid = int.Parse(cboSupplier.SelectedValue);

                var conf = le.invoiceLoanConfigs.FirstOrDefault(p => p.supplierID == sid && p.clientID == cid);
                if (conf != null)
                {
                    txtRate.Value = conf.standardInterestrate;
                    txtDisbRate.Value = conf.ceilRate;
                }
            }
            if (txtAmount.Value != null)
            {
                txtWith.Value = txtAmount.Value * 5.0 / 100.0;
                if (txtDisbRate.Value != null)
                {
                    txtDisbAmt.Value = (txtAmount.Value.Value - txtWith.Value.Value) * txtDisbRate.Value.Value / 100.0;
                }
            }
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            OnChange();
        }

        protected void txtDisbRate_TextChanged(object sender, EventArgs e)
        {
            OnChange();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAmount.Value != null
                            && txtDisbRate.Value != null
                            && txtDisbAmt.Value != null
                            && txtWith.Value != null
                && dtAppDate.SelectedDate!=null)
            {
                ln.invoiceAmount = txtAmount.Value.Value;
                ln.invoiceDate = dtAppDate.SelectedDate.Value;
                ln.clientID = client.clientID;
                ln.ceilRate = txtDisbRate.Value.Value;
                ln.proposedAmount = txtDisbAmt.Value.Value;
                ln.withHoldingTax = txtWith.Value.Value;
                ln.invoiceDescription = txtDesc.Text;
                ln.approvedBy = ""; 
                if (cboSupplier.SelectedValue != "")
                {
                    ln.supplierID = int.Parse(cboSupplier.SelectedValue);
                }
                ln.invoiceNo = txtInvoiceNo.Text;
                ln.rate = txtRate.Value.Value;

                if (ln.invoiceLoanID <= 0)
                {
                    ils.Add(ln);
                }
                else
                {
                    for (int i = 0; i < ils.Count; i++)
                    {
                        if (ils[i].invoiceLoanID == ln.invoiceLoanID)
                        {
                            ils[i] = ln;
                            break;
                        }
                    }
                }

                if (ilm == null)
                {
                    ilm = new coreLogic.invoiceLoanMaster
                    {
                        approvalDate = null,
                        approved = false,
                        approvedBy = "",
                        ceilRate = txtDisbRate.Value.Value,
                        clientID = client.clientID,
                        disbursed = false,
                        supplierID = ln.supplierID,
                        invoiceDate=dtAppDate.SelectedDate.Value 
                    }; 
                }
                Session["invoiceLoanMaster"] = ilm;
                Session["invoiceLoans"] = ils;
                Session["invoiceLoan"] = null;

                txtAmount.Value = 0;  
                txtDisbAmt.Value = 0;
                txtWith.Value = 0;
                txtDesc.Text = "";
                txtInvoiceNo.Text = ""; 

                grid.DataSource = ils;
                grid.DataBind(); 
            }
        }

        private void LoadLoan(int? id)
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
            RenderImages();
            if (Session["invoiceLoans"] == null)
            {
                ils = new List<coreLogic.invoiceLoan>();
                Session["invoiceLoans"] = ils;
            }
            else
            {
                ils = Session["invoiceLoans"] as List<coreLogic.invoiceLoan>;
            }
            if (Session["invoiceLoanMaster"] == null)
            {
                ilm = new coreLogic.invoiceLoanMaster();
                Session["invoiceLoanMaster"] = ilm;
            }
            else
            {
                ilm = Session["invoiceLoanMaster"] as coreLogic.invoiceLoanMaster;
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

        protected void cboSupplier_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        protected void grid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            var ilID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["invoiceLoanID"];
            if (ilID != null)
            {
                var id = int.Parse(ilID.ToString());
                ln = le.invoiceLoans.FirstOrDefault(p => p.invoiceLoanID == id);
                if (ln != null)
                {
                    //ln.clientReference.Load();

                    client = ln.client;
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
                    //client.clientImages.Load();

                    cboClient.Items.Add(new RadComboBoxItem(client.surName + ", " + client.otherNames +
                        "  (" + client.accountNumber + ")", client.clientID.ToString()));
                    cboClient.SelectedValue = client.clientID.ToString();
                    txtSurname.Text = client.surName;
                    txtOtherNames.Text = client.otherNames;
                    txtAccountNo.Text = client.accountNumber;

                    foreach (var r in client.clientImages)
                    {
                        //r.imageReference.Load();
                    }
                    Session["loan.cl"] = client;
                    RenderImages();
                    txtDesc.Text = ln.invoiceDescription;
                    txtDisbAmt.Value = ln.proposedAmount;
                    txtDisbRate.Value = ln.ceilRate;
                    txtWith.Value = ln.withHoldingTax;
                    txtAmount.Value = ln.invoiceAmount;
                    var t = le.tenors.FirstOrDefault(p => p.tenor1 == 6);
                    txtRate.Value = t.defaultInterestRate;
                    if (ln.supplierID != null)
                    {
                        cboSupplier.SelectedValue = ln.supplierID.Value.ToString();
                    }
                    txtInvoiceNo.Text = ln.invoiceNo;
                    dtAppDate.SelectedDate = ln.invoiceDate;
                    Session["invoiceLoan"] = ln; 
                    grid.DataSource = ils;
                    grid.DataBind();

                    pnlAddEdit.Visible = true;
                }
            }
        }

    }
}