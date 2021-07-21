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
    public partial class applyToLoan : System.Web.UI.Page
    {
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        coreLogic.coreLoansEntities le;
        coreLogic.client client;
        coreLogic.loan ln;
        coreLogic.core_dbEntities ent;
        coreLogic.controllerFileDetail fileDetail;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            {
                this.cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Principal and Interest", "1"));
                this.cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Principal Only", "2"));
                this.cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Interest Only", "3")); 
                this.cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Processing Fee", "6"));
                this.cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Additional Interest", "7"));
                cboPaymentType.SelectedValue = "1";

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    fileDetail = le.controllerFileDetails.FirstOrDefault(p => p.fileDetailID == id && p.refunded == false);
                    if (fileDetail != null)
                    {
                        var cl = le.staffCategory1.FirstOrDefault(p => p.employeeNumber == fileDetail.staffID);
                        if (cl != null)
                        {
                            client = cl.client;

                            cboLoan.Items.Add(new RadComboBoxItem("", ""));
                            foreach (var r in client.loans)
                            {
                                cboLoan.Items.Add(new RadComboBoxItem(r.loanNo + " (" + r.amountDisbursed.ToString("#,###.#0"),
                                    r.loanID.ToString()));
                            }
                        }
                    }
                }
            }
            int? id2 = null;
            if (Session["id"] != null)
            {
                id2 = int.Parse(Session["id"].ToString());
            }
            LoadLoan(id2);
        }

        protected void btnReceive_Click(object sender, EventArgs e)
        {

            if (txtAmountPaid.Value != null
                && txtAmountPaid.Value.Value > 0
                && this.dtMntDate.SelectedDate != null && fileDetail != null
                && cboPaymentType.SelectedValue != "" && cboPaymentType.SelectedValue != null)
            {
                jnl_batch batch = null;
                rpmtMgr.ReceivePayment(le, ln, txtAmountPaid.Value.Value, 
                    dtMntDate.SelectedDate.Value, cboPaymentType.SelectedValue, null, "", "", ent,
                    User.Identity.Name, 1, txtAmountPaid.Value.Value, ln.loanType.holdingAccountID, ref batch);
                fileDetail.refunded = true;
                le.SaveChanges();
                ent.SaveChanges();

                HtmlHelper.MessageBox2("Payment Receipt Data Saved Successfully!", 
                    ResolveUrl("~/ln/loans/"), "coreERP©: Successful", IconType.ok);
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", 
                    "coreERP: Incomplete", IconType.warning);
            }
        }

        private void RenderImages()
        {             
            if (client != null && client.clientImages != null)
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

        private void LoadLoan(int? id)
        {
            if (id != null)
            {
                fileDetail = le.controllerFileDetails.FirstOrDefault(p => p.fileDetailID == id && p.refunded == false);
                if (fileDetail != null)
                {
                    var cl = le.staffCategory1.FirstOrDefault(p => p.employeeNumber == fileDetail.staffID);
                    if (cl != null)
                    { 
                        ln = cl.client.loans.FirstOrDefault(); ;

                        if (ln != null)
                        { 
                            client = ln.client;                              
                        }

                        if (cboLoan.SelectedValue != "")
                        {
                            int lID = int.Parse(cboLoan.SelectedValue);
                            ln = cl.client.loans.FirstOrDefault(p => p.loanID == lID);

                            if (ln != null)
                            {
                                client = ln.client;
                                btnReceive.Enabled = true;
                                txtAmountPaid.Value = fileDetail.overage; 
                                dtMntDate.Enabled = true;

                                Session["loan.cl"] = client;

                                if (ln.client.clientTypeID == 6)
                                {
                                    pnlJoint.Visible = true;
                                    pnlRegular.Visible = false;
                                    txtJointAccountName.Text = ln.client.accountName;
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
                                txtBalance.Value = ln.balance;
                                RenderImages();
                                gridSchedule.DataSource = ln.repaymentSchedules;
                                gridSchedule.DataBind();
                            }
                        }
                        Session["loan"] = ln;
                    }
                }

                RenderImages();
                Session["loan"] = ln;
            }
            else
            {
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                }
                if (Session["loan"] != null)
                {
                    ln = Session["loan"] as coreLogic.loan;
                }
                else
                {
                    ln = new coreLogic.loan();
                    Session["loan"] = ln;
                }
            }
        }

        protected void cboLoan_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }
    }
}