using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.cashier
{
    public partial class receipt : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.client client;
        coreLogic.loan ln;
        coreLogic.core_dbEntities ent;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            { 
                List<int> repaymentTypeIds = new List<int> {1,2,3,6,7};

                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num+")", 
                        r.bank_acct_id.ToString()));
                }
                 
                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                }
                
                cboPaymentMode.SelectedValue = "1";

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    ln = le.loans
                        .Include(p => p.repaymentSchedules)
                        .Include(p => p.loanRepayments)
                        .Include(p => p.loanInsurances)
                        .FirstOrDefault(p => p.loanID == id && (p.loanStatusID ==3|| p.loanStatusID==4));

                    if (ln.loanStatusID == 3)
                    {
                        if (ent.comp_prof.FirstOrDefault() != null && ent.comp_prof.First().comp_name.ToLower().Contains("lendzee"))
                        {
                            repaymentTypeIds = new List<int> { 5,6, 8 };
                            foreach (var r in le.repaymentTypes.Where(p => repaymentTypeIds.Contains(p.repaymentTypeID)))
                            {
                                cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentTypeName, r.repaymentTypeID.ToString()));
                            }
                        }
                        else
                        {
                            foreach (var r in le.repaymentTypes.Where(p => p.repaymentTypeID == 6))
                            {
                                cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentTypeName, r.repaymentTypeID.ToString()));
                            }
                        }

                    }
                    else if (ln.loanStatusID == 4 || ln.loanStatusID == 3)
                    {
                        if (ent.comp_prof.FirstOrDefault() != null && ent.comp_prof.First().comp_name.ToLower().Contains("lendzee"))
                        {
                            repaymentTypeIds.AddRange(new List<int> { 5,8});
                            foreach (var r in le.repaymentTypes.Where(p => repaymentTypeIds.Contains(p.repaymentTypeID)))
                            {
                                cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentTypeName, r.repaymentTypeID.ToString()));
                            }
                        }
                        else
                        {
                            foreach (var r in le.repaymentTypes.Where(p => repaymentTypeIds.Contains(p.repaymentTypeID)))
                            {
                                cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentTypeName, r.repaymentTypeID.ToString()));
                            }
                        }
                    }
                    if (ln != null)
                    {
                        //ln.loanCollaterals.Load();
                        //ln.loanGurantors.Load();
                        //ln.clientReference.Load();
                        //ln.loanStatuReference.Load();
                        //ln.repaymentSchedules.Load();
                        //ln.loanTypeReference.Load();

                        client = ln.client;
                        //client.clientAddresses.Load();
                        //client.branchReference.Load();
                        //client.clientImages.Load();
                        lblLoanID.Text = ln.loanNo;

                        foreach (var r in client.clientImages)
                        {
                            //r.imageReference.Load();
                        }

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
                            txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 ||
                                               client.clientTypeID == 5)
                                ? client.companyName
                                : client.surName;
                            txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 ||
                                                  client.clientTypeID == 5)
                                ? " "
                                : client.otherNames;
                        }
                        double totalPenalty = 0;
                        if (le.loanPenalties.Any(p => p.loanID == ln.loanID))
                        {
                            totalPenalty = le.loanPenalties.Where(p => p.loanID == ln.loanID).Sum(p => p.penaltyFee);
                        }
                        double totalExpInterest = ln.repaymentSchedules.Sum(p => p.interestPayment);
                        double totalInterestPaid = ln.loanRepayments.Sum(p => p.interestPaid);
                        double totalpenaltyPaid = ln.loanRepayments.Where(p => p.repaymentTypeID == 7).Sum(p => p.penaltyPaid);
                        txtAccountNo.Text = client.accountNumber;
                        txtBalance.Value = ln.balance;
                        txtInterestBalnace.Value = totalExpInterest - totalInterestPaid;
                        txtPenalty.Value = totalPenalty - totalpenaltyPaid;
                        
                        txtApplicationFee.Value = txtApplicationFee.Value == null ? 0 : ln.applicationFeeBalance;
                      
                        txtProcessingFee.Value = ln.processingFeeBalance;
                        if(ln.loanRepayments.Any(p => p.repaymentTypeID == 8))
                        {
                            txtInsurance.Value = ln.insuranceAmount-ln.loanRepayments.Where(p => p.repaymentTypeID == 8).Sum(p => p.amountPaid);
                        }
                        txtInsurance.Value = ln.insuranceAmount;
                        dtMntDate.SelectedDate = DateTime.Now;
                        RenderImages();
                        gridSchedule.DataSource = ln.repaymentSchedules;
                        gridSchedule.DataBind();
                    }
                    else
                    {
                        btnReceive.Enabled = false;
                    }

                    Session["loan"] = ln;
                }
                else
                {
                    ln = new coreLogic.loan();
                    Session["loan"] = ln;
                    btnReceive.Enabled = false;

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

        protected void btnReceive_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Value != null
                && txtAmountPaid.Value.Value >0
                && this.dtMntDate.SelectedDate != null
                && cboPaymentMode.SelectedValue !=""
                &&((cboPaymentMode.SelectedValue!="1" && cboBank.SelectedValue!="")||(cboPaymentMode.SelectedValue=="1")))
            {
                var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                if (ct == null)
                {
                    HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }
                var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtMntDate.SelectedDate.Value
                    && p.open==true);
                if (ctd == null)
                {
                    HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }
                if (ln.loanStatusID != 3 && (ln.amountDisbursed < 0 || ln.disbursedBy == null || ln.disbursementDate == null))
                {
                    HtmlHelper.MessageBox("The selected loan is not disbursed, please disburse first. (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }

                int repayTypeId = int.Parse(cboPaymentType.SelectedValue);

                if (repayTypeId == 1 && (txtAmountPaid.Value > (txtInterestBalnace.Value + txtBalance.Value)))
                {

                    HtmlHelper.MessageBox("The amount been paid is more than the interest and principal balance, please check", "coreERP©: Failed", IconType.deny);
                    return;
                } 
                if (repayTypeId == 2 && (txtAmountPaid.Value > (txtBalance.Value)))
                {

                    HtmlHelper.MessageBox("The amount been paid is more than the principal balance, please check", "coreERP©: Failed", IconType.deny);
                    return;
                }
                if (repayTypeId == 3 && (txtAmountPaid.Value > (txtInterestBalnace.Value)))
                {

                    HtmlHelper.MessageBox("The amount been paid is more than the interest balance, please check", "coreERP©: Failed", IconType.deny);
                    return;
                }
                if (repayTypeId == 7 && (txtAmountPaid.Value > (txtPenalty.Value)))
                {

                    HtmlHelper.MessageBox("The amount been paid is more than the Penalty balance, please check", "coreERP©: Failed", IconType.deny);
                    return;
                }
                if (repayTypeId == 6 && (txtAmountPaid.Value > ln.processingFee))
                {

                    HtmlHelper.MessageBox("The amount been paid is more than the Processing fee, please check", "coreERP©: Failed", IconType.deny);
                    return;
                }
                int? bankID = null;
                if (cboBank.SelectedValue != null && cboBank.SelectedValue != "")
                    bankID = int.Parse(cboBank.SelectedValue);
                var cd = new coreLogic.cashierReceipt
                {
                    amount = txtAmountPaid.Value.Value,
                    bankID = bankID,
                    checkNo = txtCheckNo.Text,
                    clientID = ln.client.clientID,
                    loanID = ln.loanID,
                    paymentModeID = int.Parse(cboPaymentMode.SelectedValue),
                    posted = false,
                    txDate = dtMntDate.SelectedDate.Value,
                    cashierTillID = ct.cashiersTillID,
                    repaymentTypeID = int.Parse(cboPaymentType.SelectedValue),
                    feeAmount = (int.Parse(cboPaymentType.SelectedValue) == 8 || int.Parse(cboPaymentType.SelectedValue) == 5)
                                    ? txtAmountPaid.Value.Value:0
                };
                if (ent.comp_prof.First().comp_name.ToLower().Contains("lendzee") 
                     && (cd.repaymentTypeID == 5 || cd.repaymentTypeID == 6)) 
                {
                    ln.loanFees.Add(new coreLogic.loanFee
                    {
                        feeAmount = txtAmountPaid.Value.Value,
                        feeDate = dtMntDate.SelectedDate.Value,
                        feeTypeID = cd.repaymentTypeID == 5?2:1,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name
                    });
                }
                else if (ent.comp_prof.FirstOrDefault() != null && ent.comp_prof.First().comp_name.ToLower().Contains("lendzee") 
                    && cd.repaymentTypeID == 8) 
                {
                    var lf = ln.loanInsurances.FirstOrDefault();
                    if (lf == null)
                    {
                        lf = new coreLogic.loanInsurance
                        {
                            amount = txtAmountPaid.Value.Value,
                            insuranceDate = dtMntDate.SelectedDate.Value,
                            paid = false
                        };
                        ln.loanInsurances.Add(lf);
                    }
                    else
                    {
                        lf.amount += txtAmountPaid.Value.Value;
                    }
                }
                if (cboPaymentType.SelectedValue == "2") cd.principalAmount = txtAmountPaid.Value.Value;
                else if (cboPaymentType.SelectedValue == "3") cd.interestAmount = txtAmountPaid.Value.Value;
                else if (cboPaymentType.SelectedValue == "6") cd.feeAmount = txtAmountPaid.Value.Value;
                else if (cboPaymentType.SelectedValue == "7") cd.addInterestAmount = txtAmountPaid.Value.Value;
                le.cashierReceipts.Add(cd);                
                le.SaveChanges();

                cd.loan = ln;
                cd.client = ln.client;
                cd.cashiersTill = ct;

                Session["loan.cl"] = null;
                Session["loan"] = null;
                Session["cashierReceipt"] = cd;
                HtmlHelper.MessageBox2("Payment Receipt Data Saved Successfully!", ResolveUrl("~/ln/reports/receipt.aspx"), "coreERP©: Successful", IconType.ok);
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", "coreERP: Incomplete", IconType.warning);
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

        private void LoadLoan(int? id)
        {
            if (id != null)
            {
                ln = le.loans.FirstOrDefault(p => p.loanID == id && (p.loanStatusID == 4 || p.loanStatusID == 3));

                if (ln != null)
                {
                    //ln.loanCollaterals.Load();
                    //ln.loanGurantors.Load();
                    //ln.clientReference.Load();
                    //ln.loanStatuReference.Load();
                    //ln.repaymentSchedules.Load();
                    //ln.loanRepayments.Load(); 

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

                    foreach (var r in client.clientImages)
                    {
                        //r.imageReference.Load();
                    }
                    foreach (var r in client.clientImages)
                    {
                        //r.imageReference.Load();
                    }
                    Session["loan.cl"] = client;
                    RenderImages();
                }
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