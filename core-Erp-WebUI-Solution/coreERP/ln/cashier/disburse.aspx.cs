using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.cashier
{
    public partial class disburse : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.client client;
        coreLogic.loan ln;
        List<coreLogic.loanGurantor> guarantors;
        List<coreLogic.loanCollateral> collaterals;
        coreLogic.core_dbEntities ent;
        string categoryID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "";
            if (categoryID == "5")
            {
                Response.Redirect("/ln/cashier/disbursePayroll.aspx?id=" + Request.Params["id"] + "&catID=" + categoryID);
            }
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            { 
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
                    Session["id"] = id;
                    ln = le.loans.FirstOrDefault(p => p.loanID == id);

                    if (ln != null)
                    { 
                        client = ln.client;  

                        Session["loan.cl"] = client;
                        lblLoanID.Text = ln.loanNo;

                        guarantors = ln.loanGurantors.ToList();
                        collaterals = ln.loanCollaterals.ToList();
 
                        gridCollateral.DataSource = collaterals;
                        gridCollateral.DataBind();

                        gridGuarantor.DataSource = guarantors;
                        gridGuarantor.DataBind();
                          
                        txtAmountApproved.Value = ln.amountApproved;
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
                        txtAmountApproved.Value = ln.amountApproved;
                        txtDisbursed.Value = ln.amountDisbursed;
                        dtDisbDate.SelectedDate = DateTime.Now;

                        RenderImages();
                        gridSchedule.DataSource = ln.repaymentSchedules;
                        gridSchedule.DataBind();
                        gridRepayment.DataSource = ln.loanRepayments;
                        gridRepayment.DataBind();
                    }

                    Session["loan"] = ln;

                    var prof = ent.comp_prof.FirstOrDefault();
                }
                else
                {
                    ln = new coreLogic.loan();
                    Session["loan"] = ln;

                    guarantors = new List<coreLogic.loanGurantor>();
                    Session["loanGuarantors"] = guarantors;

                    collaterals = new List<coreLogic.loanCollateral>();
                    Session["loanCollaterals"] = collaterals;
                }
                chkPostToSavings.Checked = false;
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
         
        protected void gridGuarantor_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = guarantors[e.Item.ItemIndex];
                if (g != null)
                {
                   
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
            }
            gridGuarantor.DataSource = guarantors;
            gridGuarantor.DataBind();
        }
         
        protected void gridCollateral_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
               
            }
            else if (e.CommandName == "DeleteItem")
            {
            }
            gridCollateral.DataSource = collaterals;
            gridCollateral.DataBind();
        }

        private void RenderImages()
        {
            if (client.clientImages != null)
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

        protected void btnDisburse_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Value != null
                && txtAmountPaid.Value.Value <= txtAmountApproved.Value.Value - txtDisbursed.Value.Value
                && dtDisbDate.SelectedDate != null
                && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1")))
            {
                var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                if (user.accessLevel.disbursementLimit < txtAmountPaid.Value)
                {
                    HtmlHelper.MessageBox("The amount to be disbursed is beyond your access level",
                                                "coreERP©: Failed", IconType.deny);
                    return;
                }
                if (chkPostToSavings.Checked == true)
                {
                    var sav = le.savings.FirstOrDefault(p => p.clientID == ln.clientID);
                    if (sav == null)
                    {
                        HtmlHelper.MessageBox("The client does not have a savings account created. Create one before disbursing", 
                            "coreERP©: Failed", IconType.deny);
                        return;
                    }
                }
                var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                if (ct == null)
                {
                    HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }
                var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtDisbDate.SelectedDate.Value
                    && p.open == true);
                if (ctd == null)
                {
                    HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }
                //ln.cashierDisbursements.Load();
                var soFar = ln.cashierDisbursements.Sum(p => p.amount);
                if (soFar == null) soFar = 0;

                if (ln.amountApproved < (txtAmountPaid.Value.Value + soFar))
                {
                    HtmlHelper.MessageBox("Amount to disburse is greater than approved amount!", "coreERP©: Failed", IconType.deny);
                    return;
                }
                int? bankID = null;
                if (cboBank.SelectedValue != null && cboBank.SelectedValue != "")
                    bankID = int.Parse(cboBank.SelectedValue);
                var cd = new coreLogic.cashierDisbursement
                {
                    amount = txtAmountPaid.Value.Value,
                    bankID = bankID,
                    checkNo = txtCheckNo.Text,
                    clientID = ln.client.clientID,
                    loanID = ln.loanID,
                    paymentModeID = int.Parse(cboPaymentType.SelectedValue),
                    posted = false,
                    txDate = dtDisbDate.SelectedDate.Value,
                    cashierTillID = ct.cashiersTillID,
                    postToSavingsAccount = chkPostToSavings.Checked
                };
                le.cashierDisbursements.Add(cd);
                le.SaveChanges(); 

                Session["loanGuarantors"] = null;
                Session["loanCollaterals"] = null;
                Session["loan.cl"] = null;
                Session["loan"] = null;
               HtmlHelper.MessageBox2("The disbursement has been received successfully",
                   ResolveUrl("~/ln/cashier/default3.aspx"), "coreERP©: Successful", IconType.ok);
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", "coreERP: Incomplete", IconType.warning);
            }
        }

        private void LoadLoan(int? id)
        {
            if (id != null)
            {
                ln = le.loans.FirstOrDefault(p => p.loanID == id);

                if (ln != null)
                { 
                    client = ln.client; 
                    Session["loan.cl"] = client;

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
    }
}