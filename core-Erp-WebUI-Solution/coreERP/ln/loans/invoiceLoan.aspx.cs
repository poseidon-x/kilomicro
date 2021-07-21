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
    public partial class invoiceLoan : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.invoiceLoan ln;
        List<coreLogic.invoiceLoan> ils;
        coreLogic.invoiceLoanMaster ilm;
        List<coreLogic.invoiceLoan> deletedIls;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {
                ViewState["OverridenSupplier"] = null;
                ViewState["Overriden"] = null;
                Session["invoiceLoans"] = null;
                Session["invoiceLoanMaster"] = null;
                Session["invoiceLoan"] = null;
                if (Request.Params["ilm"] != null)
                {
                    int id = int.Parse(Request.Params["ilm"]);
                    ilm = le.invoiceLoanMasters.FirstOrDefault(p => p.invoiceLoanMasterID == id);
                    if (ilm != null)
                    {
                        //ilm.invoiceLoans.Load();
                        Session["invoiceLoanMaster"] = ilm;
                        Session["invoiceLoans"] = ilm.invoiceLoans.ToList();
                        grid.DataSource = ilm.invoiceLoans;
                        grid.DataBind();

                        //ilm.clientReference.Load();
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
                        var sup = le.suppliers.FirstOrDefault(p => p.supplierID == ilm.supplierID);
                        if (sup != null)
                        {
                            cboSupplier.Items.Clear();
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
            if (ils.Count > 0 && ilm != null)
            {
                int? supplierId = null;
                if (cboSupplier.SelectedValue != "")
                {
                    supplierId = int.Parse(cboSupplier.SelectedValue);
                }
                string ret = null;
                if ((ret = ValidateMaximumExposure(supplierId)) != null)
                {
                    HtmlHelper.MessageBox(ret);
                    return;
                }
                if (ilm.invoiceLoanMasterID > 0)
                {
                    ilm = le.invoiceLoanMasters.FirstOrDefault(p => p.invoiceLoanMasterID == ilm.invoiceLoanMasterID);
                }
                else
                { 
                    ilm = new coreLogic.invoiceLoanMaster
                    {
                        approvalDate = ilm.approvalDate,
                        approved = ilm.approved,
                        approvedBy = ilm.approvedBy,
                        ceilRate = ilm.ceilRate,
                        clientID = ilm.clientID,
                        disbursed = ilm.disbursed,
                        supplierID = ilm.supplierID,
                        invoiceDate = ilm.invoiceDate,
                    };
                    le.invoiceLoanMasters.Add(ilm);
                }
                foreach (var ln in ils)
                {
                    if (ln.invoiceLoanID <= 0)
                    {
                        var r = new coreLogic.invoiceLoan
                        {
                            invoiceAmount = ln.invoiceAmount,
                            invoiceDate = ln.invoiceDate,
                            clientID = ln.clientID,
                            ceilRate = ln.ceilRate,
                            proposedAmount = ln.proposedAmount,
                            withHoldingTax = ln.withHoldingTax,
                            invoiceDescription = ln.invoiceDescription,
                            approvedBy = ln.approvedBy,
                            supplierID = ln.supplierID,
                            invoiceNo = ln.invoiceNo,
                            rate = ln.rate,
                            processingFee = ln.processingFee,
                            poNumber = ln.poNumber,
                            disbursementType = ln.disbursementType,
                        };
                        ilm.invoiceLoans.Add(r);
                    }
                    else
                    {
                        var r = le.invoiceLoans.FirstOrDefault(p => p.invoiceLoanID == ln.invoiceLoanID);
                        if (r != null)
                        {
                            r.invoiceAmount = ln.invoiceAmount;
                            r.invoiceDate = ln.invoiceDate;
                            r.clientID = ln.clientID;
                            r.ceilRate = ln.ceilRate;
                            r.proposedAmount = ln.proposedAmount;
                            r.withHoldingTax = ln.withHoldingTax;
                            r.invoiceDescription = ln.invoiceDescription;
                            r.approvedBy = ln.approvedBy;
                            r.supplierID = ln.supplierID;
                            r.invoiceNo = ln.invoiceNo;
                            r.rate = ln.rate;
                            r.processingFee = ln.processingFee;
                        }
                    }
                }
                for (int i = ils.Count - 1; i >= 0; i-- )
                {
                    var l = ils[i];
                    if (ils.Find(p => p.invoiceLoanID == l.invoiceLoanID) == null)
                    {
                        ilm.invoiceLoans.Remove(l);
                    }
                }
                le.SaveChanges();

                HtmlHelper.MessageBox2("Invoice Loans Data Saved Successfully!", 
                    ResolveUrl("~/ln/loans/default3.aspx"), "coreERP©: Successful", IconType.ok);
            }
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
            coreLogic.invoiceLoanConfig conf = null;
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                if (client != null)
                {
                    Session["loan.cl"] = client;
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

                conf = le.invoiceLoanConfigs.FirstOrDefault(p => p.supplierID == sid && p.clientID == cid);
                if (conf != null)
                {
                    txtRate.Value = conf.standardInterestrate;
                    txtDisbRate.Value = conf.ceilRate;
                }
            }
            if (txtAmount.Value != null)
            {
                txtWith.Value = txtAmount.Value * 3.0 / 100.0;
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
                int? supplierId = null;
                if (cboSupplier.SelectedValue != "")
                {
                    supplierId = int.Parse(cboSupplier.SelectedValue);
                }
                string ret = null;
                if (( ret = ValidateMaximumExposure(supplierId)) != null)
                {
                    HtmlHelper.MessageBox(ret);
                    return;
                }
                if (ln == null) ln = new coreLogic.invoiceLoan();
                if (chkPO.Checked == false)
                {
                    ln.invoiceNo = txtInvoiceNo.Text;
                    ln.invoiceDescription = txtDesc.Text;
                    ln.disbursementType = "INV";
                    ln.poNumber = "";
                }
                else
                {
                    ln.poNumber = txtInvoiceNo.Text.Trim();
                    ln.disbursementType = "PO";
                    ln.invoiceNo = "";
                    ln.invoiceDescription = txtDesc.Text;
                }
                ln.invoiceAmount = txtAmount.Value.Value;
                ln.invoiceDate = dtAppDate.SelectedDate.Value;
                ln.clientID = client.clientID;
                ln.ceilRate = txtDisbRate.Value.Value;
                ln.proposedAmount = txtDisbAmt.Value.Value;
                ln.withHoldingTax = txtWith.Value.Value;
                ln.invoiceDescription = txtDesc.Text;
                ln.approvedBy = ""; 
                if (supplierId!=null)
                {
                    ln.supplierID = supplierId.Value;
                }
                ln.rate = txtRate.Value.Value;
                var conf = le.invoiceLoanConfigs.FirstOrDefault(p => p.supplierID == ln.supplierID && p.clientID == ln.clientID);
                if (conf != null)
                {
                    ln.processingFee = Math.Ceiling(conf.standardProcessingFeerate * ln.proposedAmount / 100.0);
                } 
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

                if (ilm == null || ilm.invoiceLoanMasterID<=0)
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
                btnAdd.Text = "Add Invoice Loan Item";
                btnAdd.Enabled = false;
                btnNew.Enabled = true;
                btnSave.Enabled = true;
                pnlAddEdit.Visible = false;
                ViewState["OverridenSupplier"] = null;
                ViewState["Overriden"] = null;
            }
        }

        private string ValidateMaximumExposure(int? supplierId)
        {
            //using (var le2 = new coreLoansEntities())
            //{
            //    var unsavedIlAmount = 0.0;
            //    var unsavedIlAmount2 = 0.0;
                
            //    var allOutstandingSupplierInvoices = (
            //        from il in le2.invoiceLoans
            //        from l in le2.loans
            //        from rs in le2.repaymentSchedules 
            //        where il.clientID == l.clientID && il.supplierID == supplierId
            //        select new
            //        {
            //            balance = rs.loanID == null ? l.amountApproved : rs.interestBalance + rs.principalBalance,
            //            amount = rs.loanID == null ? l.amountApproved : rs.interestPayment + rs.principalPayment
            //        }
            //        ).ToList();
            //    if (allOutstandingSupplierInvoices.Any()|| unsavedIlAmount>0)
            //    {
            //        double totalExposure = allOutstandingSupplierInvoices.Sum(p => p.balance) + unsavedIlAmount;
            //        var supplier = le.suppliers.FirstOrDefault(p => p.supplierID == supplierId);
            //        if (supplier != null)
            //        {
            //            if (totalExposure > supplier.maximumExposure)
            //            {
            //                if (ViewState["OverridenSupplier"] == null)
            //                {
            //                    ViewState["OverridenSupplier"] = "1";
            //                    return "Maximum Exposure for this Supplier will be Exceeded with this Addition.\n"
            //                           +
            //                           "To submit for an executive override, click the approve button again.";
            //                }
            //            }
            //        }
            //    }
            //    var allOutstandingClientInvoices = (
            //        from il in le2.invoiceLoans
            //        from l in le2.loans
            //        from rs in le2.repaymentSchedules 
            //        where il.clientID == l.clientID && il.clientID == client.clientID
            //        select new
            //        {
            //            balance = rs==null || rs.loanID == null ? l.amountApproved : rs.interestBalance + rs.principalBalance,
            //            amount = rs == null || rs.loanID == null ? l.amountApproved : rs.interestPayment + rs.principalPayment
            //        }
            //        ).ToList();
            //    if (allOutstandingClientInvoices.Any()|| unsavedIlAmount2>0)
            //    {
            //        double totalExposure = allOutstandingClientInvoices.Sum(p => p.balance) + unsavedIlAmount2;
            //        var config =
            //            le.invoiceLoanConfigs.FirstOrDefault(
            //                p => p.clientID == client.clientID && p.supplierID == supplierId);
            //        if (config != null)
            //        {
            //            if (totalExposure > config.maximumExposure)
            //            {
            //                if (ViewState["Overriden"] == null)
            //                {
            //                    ViewState["Overriden"] = "1";
            //                    return "Maximum Exposure for this Client will be Exceeded with this Addition.\n"
            //                                          +
            //                                          "To submit for an executive override, click the add button again.";
            //                }
            //            }
            //        }
            //    }
            //}
            return null;
        }

        private void LoadLoan(int? id)
        {
            if (id != null)
            {
                ln = le.invoiceLoans.FirstOrDefault(p => p.invoiceLoanID == id);

                if (ln != null)
                { 
                    client = ln.client;  
                    Session["loan.cl"] = client;
                }
                Session["invoiceLoan"] = ln;
            }
            else
            {
                if (Session["invoiceLoan"] != null)
                {
                    ln = Session["invoiceLoan"] as coreLogic.invoiceLoan;
                }
                else
                {
                    ln = new coreLogic.invoiceLoan();
                    Session["invoiceLoan"] = ln;
                }
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

            if (Session["deletedInvoiceLoans"] == null)
            {
                deletedIls = new List<coreLogic.invoiceLoan>();
                Session["deletedInvoiceLoans"] = deletedIls;
            }
            else
            {
                deletedIls = Session["deletedInvoiceLoans"] as List<coreLogic.invoiceLoan>;
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
            if (Session["loan.cl"] != null)
            {
                client = Session["loan.cl"] as coreLogic.client;
            }
        }

        protected void cboClient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                var res = (
                        from p in le.invoiceLoanConfigs
                        from c in le.clients
                        where p.clientID == c.clientID
                            && (c.surName.Contains(e.Text) || c.otherNames.Contains(e.Text) || c.companyName.Contains(e.Text)
                            || c.accountName.Contains(e.Text))
                        select new
                        {
                            c.clientID,
                            c.surName,
                            c.otherNames,
                            c.companyName,
                            c.accountName,
                            c.accountNumber,
                            c.clientTypeID
                        }
                    ).Distinct().OrderBy(p => p.surName).ToList();
                foreach (var cl in res)
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

                if (e.CommandName == "MyEdit")
                {
                ln = le.invoiceLoans.FirstOrDefault(p => p.invoiceLoanID == id);
                client = ln.client;
                    if (ln != null)
                    {
                        cboClient.Items.Add(new RadComboBoxItem(client.surName + ", " + client.otherNames +
                                                                "  (" + client.accountNumber + ")",
                            client.clientID.ToString()));
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
                        Session["invoiceLoan"] = ln;
                        btnAdd.Text = "Update Invoice Loan";
                        pnlAddEdit.Visible = true;
                        btnAdd.Enabled = true;
                        chkPO.Checked = ln.disbursementType == "PO";
                        btnNew.Enabled = false;
                        if (chkPO.Checked == true)
                        {
                            divInvoiceAmount.InnerText = "P/O Amount";
                            divInvoiceDate.InnerText = "P/O Date";
                            divInvoiceDescription.InnerText = "P/O Description";
                            divInvoiceNo.InnerText = "P/O Number";
                        }
                        else
                        {
                            divInvoiceAmount.InnerText = "Invoice Amount";
                            divInvoiceDate.InnerText = "Invoice Date";
                            divInvoiceDescription.InnerText = "Invoice Description";
                            divInvoiceNo.InnerText = "Invoice Number";
                        }
                    }
                }
                else if (e.CommandName == "MyDelete")
                {
                    var toDel = ils.First(p => p.invoiceLoanID == id);
                    ils.Remove(toDel);
                    Session["invoiceLoans"] = ils;

                    deletedIls.Add(toDel);
                    Session["deletedInvoiceLoans"] = deletedIls;
                }
                Session["invoiceLoans"] = ils;

                grid.DataSource = ils;
                grid.DataBind();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (pnlAddEdit.Visible == false)
            {
                pnlAddEdit.Visible = true;
                btnAdd.Enabled = true;
                btnNew.Enabled = false;
            }
        }

        protected void cboSupplier_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            int? clientID = null;
            if (cboClient.SelectedValue != "")
            {
                clientID = int.Parse(cboClient.SelectedValue);
            }
            cboSupplier.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
            var res = (
                    from p in le.invoiceLoanConfigs
                    from c in le.suppliers
                    where p.supplierID == c.supplierID
                        && (clientID==null || p.clientID==clientID)
                    select new
                    {
                        c.supplierID,
                        c.supplierName
                    }
                ).Distinct().OrderBy(p => p.supplierName).ToList();
            foreach (var cl in res)
            {
                cboSupplier.Items.Add(new Telerik.Web.UI.RadComboBoxItem(cl.supplierName, cl.supplierID.ToString()));
            }
        }

        protected void chkPO_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPO.Checked == true)
            {
                divInvoiceAmount.InnerText = "P/O Amount";
                divInvoiceDate.InnerText = "P/O Date";
                divInvoiceDescription.InnerText = "P/O Description";
                divInvoiceNo.InnerText = "P/O Number";
            }
            else
            {
                divInvoiceAmount.InnerText = "Invoice Amount";
                divInvoiceDate.InnerText = "Invoice Date";
                divInvoiceDescription.InnerText = "Invoice Description";
                divInvoiceNo.InnerText = "Invoice Number";
            }
        }

    }
}