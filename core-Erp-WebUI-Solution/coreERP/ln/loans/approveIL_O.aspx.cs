using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class approveIL_O : System.Web.UI.Page
    {
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
                dtAppDate.SelectedDate = DateTime.Now;

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
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
                && ln.disbursed==false)
            {
                ln.invoiceAmount = txtAmount.Value.Value;
                ln.invoiceDate = dtAppDate.SelectedDate.Value;
                ln.clientID = client.clientID;
                ln.ceilRate = txtDisbRate.Value.Value;
                ln.proposedAmount = txtDisbAmt.Value.Value;
                ln.withHoldingTax = txtWith.Value.Value ;
                ln.invoiceDescription = txtDesc.Text;
                ln.approvedBy = "";
                ln.addFee = chkAddFees.Checked;

                var t = le.tenors.FirstOrDefault(p=>p.tenor1==6);  
                var pm = "1";  
                ln.amountApproved = txtAmount.Value.Value;
                ln.approvalDate = dtAppDate.SelectedDate.Value;
                ln.approved = true;
                le.SaveChanges();
                ent.SaveChanges();

                HtmlHelper.MessageBox2("Invoice Loan Approval Data Saved Successfully!", ResolveUrl("~/ln/loans/default2.aspx"), "coreERP©: Successful", IconType.ok);  
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