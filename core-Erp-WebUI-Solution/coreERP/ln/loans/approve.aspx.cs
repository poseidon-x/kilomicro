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
  
    public partial class approve : System.Web.UI.Page
    {
        IScheduleManager schMgr = new ScheduleManager();
        coreLogic.coreLoansEntities le;
        coreLogic.client client;
        coreLogic.loan ln;
        List<coreLogic.loanGurantor> guarantors;
        List<coreLogic.loanCollateral> collaterals;
        List<coreLogic.loanCheck> checks;
        List<coreLogic.loanDocument> documents;
        coreLogic.prLoanDetail detail;
        List<coreLogic.prAllowance> allowances;
        

        string categoryID = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblInvalidAmt.Visible = false;
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "1";
            le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            { 
                cboRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.repaymentModes)
                {
                    cboRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentModeName, r.repaymentModeID.ToString()));
                }

                cboInterestType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.interestTypes)
                {
                    cboInterestType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.interestTypeName, r.interestTypeID.ToString()));
                }
                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.banks)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_name, r.bank_id.ToString()));
                }

                cboLoanScheme.Items.Add(new RadComboBoxItem("Not Applicable", ""));

                cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanTypes)
                {
                    cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanTypeName, r.loanTypeID.ToString()));
                }

                cboLoanPurpose.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanPurposes)
                {
                    cboLoanPurpose.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanPurposeName, r.loanPurposeID.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    ln = le.loans.FirstOrDefault(p => p.loanID == id);

                    if (ln != null)
                    {
                        detail = ln.prLoanDetails.FirstOrDefault();
                        client = ln.client;

                        Session["loan.cl"] = client;

                        guarantors = ln.loanGurantors.ToList();
                        collaterals = ln.loanCollaterals.ToList(); 
                        documents = ln.loanDocuments.ToList();

                        Session["loanGuarantors"] = guarantors;
                        Session["loanCollaterals"] = collaterals;
                        Session["loanChecks"] = checks;
                        Session["loanDocuments"] = documents;

                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();

                        gridCollateral.DataSource = collaterals;
                        gridCollateral.DataBind();

                        gridGuarantor.DataSource = guarantors;
                        gridGuarantor.DataBind();
                          
                        txtTenure.Value = ln.loanTenure;
                        lblLoanID.Text = ln.loanNo;
                        txtRate.Value = ln.interestRate; 
                        txtAmountRequested.Value = ln.amountRequested; 
                        txtProcFee.Value = ln.processingFee; 
                        cboRepaymentMode.SelectedValue = ln.repaymentModeID.ToString();
                        cboLoanType.SelectedValue = ln.loanTypeID.ToString();
                        cboInterestType.SelectedValue = ln.interestTypeID.ToString();
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
                        txtAmountApproved.Value = ln.amountApproved;
                        dtAppDate.SelectedDate = ln.applicationDate;
                        chkAddFees.Checked = ln.addFeesToPrincipal;
                        txtAppComments.Text = (ln.approvalComments == null) ? "" : ln.approvalComments;
                        RenderImages();
                        gridSchedule.DataSource = ln.repaymentSchedules;
                        gridSchedule.DataBind();
                        txtAppComments.Text = ln.approvalComments;
                        txtCreditManagerNotes.Text = (ln.creditManagerNotes == null) ? "" : ln.creditManagerNotes;

                        txtInsurance.Value = ln.insuranceAmount;
                        if (ln.insuranceAmount > 0)
                        {
                            trInsurance.Visible = true;
                        }

                        if (ln.loanSchemeId != null)
                        {
                            cboLoanScheme.SelectedValue = ln.loanSchemeId.ToString();
                            txtRate.Enabled = false;
                            txtTenure.Enabled = false;
                            cboRepaymentMode.Enabled = false;
                        }

                        rpCheckList.DataSource = ln.loanCheckLists;
                        rpCheckList.DataBind();
                        if (detail != null)
                        {
                            txtAMD.Value = detail.amd;
                            chkPreAudit.Checked = detail.amdPreAuditVerified;
                            txtAuthorityNoteNo.Text = detail.authorityNoteNumber;
                            txtGrossSalary.Value = detail.grossSalary;
                            txtLoanAdvanceNo.Text = detail.loanAdvanceNumber;
                            txtNetSalary.Value = detail.netSalary;
                            txtSSWelfare.Value = detail.socialSecWelfare;
                            txtTax.Value = detail.tax;
                            txtTotalDedNotOnPR.Value = detail.loanDeductionsNotOnPr;
                            txtTotalDeductions.Value = detail.totalDeductions;
                            cboLoanPurpose.SelectedValue = detail.loanPurposeID.ToString();
                            cboLoanProduct.SelectedValue = detail.loanProductID.ToString();

                            //detail.loanPurposeReference.Load();
                            if (detail.loanPurpose != null)
                            {
                                //detail.loanPurpose.loanPurposeDetails.Load();
                                cboLoanPurposeDetail.Items.Clear();
                                cboLoanPurposeDetail.Items.Add(new RadComboBoxItem("", ""));
                                foreach (var r in detail.loanPurpose.loanPurposeDetails)
                                {
                                    cboLoanPurposeDetail.Items.Add(new RadComboBoxItem(r.loanPurposeDetailName, r.loanPurposeDetailID.ToString()));
                                }
                                cboLoanPurposeDetail.SelectedValue = detail.loanPurposeDetailID.ToString();
                            }
                             
                            allowances = detail.prAllowances.ToList(); 
                            Session["allowances"] = allowances;
                            gridAllowances.DataSource = allowances;
                            gridAllowances.DataBind();
                        }
                    }

                    Session["loan"] = ln;
                }
                else
                {
                    ln = new coreLogic.loan();
                    Session["loan"] = ln;

                    guarantors = new List<coreLogic.loanGurantor>();
                    Session["loanGuarantors"] = guarantors;

                    collaterals = new List<coreLogic.loanCollateral>();
                    Session["loanCollaterals"] = collaterals;

                    checks = new List<coreLogic.loanCheck>();
                    Session["loanChecks"] = checks;
                    documents = new List<coreLogic.loanDocument>();
                    Session["loanDocuments"] = documents;
                }
                if (categoryID == "5")
                {
                    cboLoanProduct.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                    foreach (var r in le.loanProducts)
                    {
                        cboLoanProduct.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanProductName
                                + " - (" + ComputeApproved(r.loanProductID).ToString("#,##0.#0") + ")",
                                r.loanProductID.ToString()));
                    }
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
            if (categoryID == "5")
            {
                cboLoanType.SelectedValue = "6";
                cboInterestType.Enabled = false;

                cboLoanType.Enabled = false;
                tab1.Tabs[0].Visible = true;
                tab1.Tabs[1].Visible = true;
                tab1.Tabs[5].Visible = false;
                cboLoanProduct.Visible = true;
                cboLoanType.Visible = false;
                cboRepaymentMode.SelectedValue = "30";
                cboRepaymentMode.Enabled = false;
                txtTenure.Enabled = false;
                pnlMD.Visible = true;

                pnlFees.Visible = false;
            }
            if (ln.loanStatusID == 4 || ln.loanStatusID == 7)
            {
                btnApprove.Enabled = false;
                btnDeny.Enabled = false;
            }
            if (ent.comp_prof.FirstOrDefault().deductInsurance == true)
            {
                trInsurance.Attributes["visibility"] = "visible";
                trInsurance.Visible = true;
            }
            else
            {
                trInsurance.Attributes["visibility"] = "hidden";
                trInsurance.Visible = false;
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            if (categoryID=="5" && cboLoanProduct.SelectedValue != "")
            {
                var pid = int.Parse(cboLoanProduct.SelectedValue);
                var maxAmt = ComputeApproved(pid);
                if (txtAmountApproved.Value.Value > maxAmt)
                {
                    HtmlHelper.MessageBox("Amount to be approved is greater than maximum amount client can borrow for this product!");
                    return;
                }
            }
            if (ln.loanStatusID != 4 && ln.loanStatusID != 7 && ln.loanStatusID!=3)
            {
                var unapp = ln.loanCheckLists.FirstOrDefault(p => p.passed == false);
                coreLogic.loanCheckList unapp2 = null;
                if (categoryID == "5")
                {
                    unapp2 = ln.loanCheckLists.FirstOrDefault(p => categoryID == "5" && p.categoryCheckList.isMandatory == true && p.passed == false);
                }
                var passed = true;
                foreach (RepeaterItem item in rpCheckList.Items)
                {
                    try
                    {
                        var lblDesc = item.FindControl("lblDesc") as Label;
                        var chkPassed = item.FindControl("chkPassed") as CheckBox;
                        var chkMandatory = item.FindControl("chkMandatory") as CheckBox;
                        var txtComments = item.FindControl("txtComments") as TextBox;
                        if (lblDesc != null && chkPassed != null && txtComments != null)
                        {
                            var cl = ln.loanCheckLists.FirstOrDefault(p => p.description == lblDesc.Text);
                            if (cl != null)
                            {
                                cl.passed = chkPassed.Checked;
                                cl.comments = txtComments.Text;
                                cl.creationDate = DateTime.Now;
                            }
                            if (categoryID == "5")
                            {
                                if (chkMandatory.Checked == true && chkPassed.Checked == false)
                                {
                                    passed = false;
                                }
                            }
                            else
                            {
                                if (chkPassed.Checked == false)
                                {
                                    passed = false;
                                }
                            }
                        }
                    }
                    catch (Exception x) { }
                }
                if (txtAmountApproved.Value != null && txtAmountApproved.Value.Value > 0
                    && dtAppDate.SelectedDate != null && ((categoryID != "5" && unapp == null) || (categoryID == "5" && unapp2 == null))
                    && passed == true)
                {
                    var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                    if (user.accessLevel.approvalLimit < txtAmountApproved.Value)
                    {
                        HtmlHelper.MessageBox("The amount to be approved is beyond your access level",
                                                    "coreERP©: Failed", IconType.deny);
                        return;
                    }
                    int? id = null;
                    if (cboLoanProduct.SelectedValue != "") id = int.Parse(cboLoanProduct.SelectedValue);
                    var lp = le.loanProducts.FirstOrDefault(p => p.loanProductID == id);
                    if (categoryID == "5")
                    {
                        if ((DateTime.Now - ln.client.DOB.Value).TotalDays / 365.25 >= 59.7)
                        {
                            HtmlHelper.MessageBox("Client Age Greater than 59. Application Should be Declined!");
                            return;
                        }
                        else if ((DateTime.Now - ln.client.DOB.Value).TotalDays / 365.25 >= 59.7)
                        {
                            if (lp != null && lp.loanTenure > 12)
                            {
                                HtmlHelper.MessageBox("Client Age Greater than 59. Application Should be Limited to 12 Months!");
                                return;
                            }
                        }
                        else if (lp.minAge > (DateTime.Now - ln.client.DOB.Value).TotalDays / 365.25)
                        {
                            HtmlHelper.MessageBox("Client Age Less than Minimum Age for the selected product!");
                            return;
                        }
                        else if (lp.maxAge < (DateTime.Now - ln.client.DOB.Value).TotalDays / 365.25)
                        {
                            HtmlHelper.MessageBox("Client Age Greater than Maximum Age for the selected product!");
                            return;
                        }
                        if (ln.client.staffCategory1.Count() > 0 && 
                            (ln.client.staffCategory1.FirstOrDefault().lengthOfService < 6
                            && (DateTime.Now-ln.client.staffCategory1.FirstOrDefault().employmentStartDate.Value).TotalDays/30.0<6))
                        {
                            HtmlHelper.MessageBox("Client Length of Service Less than 6 months. Applications should be Declined!");
                            return;
                        }
                    }
                    ln.amountApproved = txtAmountApproved.Value.Value;
                    this.ln.finalApprovalDate = dtAppDate.SelectedDate.Value;
                    ln.modification_date = DateTime.Now;
                    ln.last_modifier = User.Identity.Name; 
                    ln.interestRate = txtRate.Value.Value;
                    ln.interestTypeID = int.Parse(cboInterestType.SelectedValue); 
                    ln.processingFee = txtProcFee.Value.Value;
                    ln.commission = 0; 
                    ln.processingFeeBalance = txtProcFee.Value.Value;
                    ln.commissionBalance = 0;
                    ln.approvalComments = txtAppComments.Text;
                    ln.addFeesToPrincipal = chkAddFees.Checked;
                    ln.approvedBy = User.Identity.Name;

                    ln.loanStatusID = 3;
                    ln.loanTenure = txtTenure.Value.Value;
                    ln.loanTypeID = int.Parse(cboLoanType.SelectedValue);
                    ln.repaymentModeID = int.Parse(cboRepaymentMode.SelectedValue);
                    ln.tenureTypeID = 1;
                    if (txtInsurance.Value != null && txtInsurance.Value.Value > 0 && trInsurance.Visible == true)
                    {
                        ln.insuranceAmount = txtInsurance.Value.Value;
                    }
                    if (ln.edited == false)
                    {
                        foreach (var rs in ln.repaymentSchedules.ToList())
                        {
                            le.repaymentSchedules.Remove(rs);
                        }
                        List<coreLogic.repaymentSchedule> sched ;
                        if (categoryID == "5")
                        {
                            sched =
                                schMgr.calculateScheduleM((chkAddFees.Checked ? ln.processingFee : 0) +
                                                          txtAmountApproved.Value.Value, txtRate.Value.Value,
                                    dtAppDate.SelectedDate.Value, (int) txtTenure.Value.Value);
                        }
                        else
                        {
                            using (core_dbEntities ctx = new core_dbEntities())
                            {
                                var comp = ctx.comp_prof.FirstOrDefault();
                              
                                if (comp.comp_name.ToLower().Contains("ttl"))
                                {
                                    sched =
                                       schMgr.calculateScheduleTTL((chkAddFees.Checked ? ln.processingFee : 0) +
                                        txtAmountApproved.Value.Value, txtRate.Value.Value,
                                       dtAppDate.SelectedDate.Value, (int?)ln.gracePeriod, (int)txtTenure.Value.Value,
                                       int.Parse(cboInterestType.SelectedValue), int.Parse(cboRepaymentMode.SelectedValue), ln);
                                }
                                else
                                {
                                    sched =
                                        schMgr.calculateSchedule((chkAddFees.Checked ? ln.processingFee : 0) +
                                                                 txtAmountApproved.Value.Value, txtRate.Value.Value,
                                        dtAppDate.SelectedDate.Value, (int?)ln.gracePeriod, (int)txtTenure.Value.Value,
                                        int.Parse(cboInterestType.SelectedValue), int.Parse(cboRepaymentMode.SelectedValue), ln.client);
                                }
                            }
                            foreach (var rs in sched)
                            {
                                rs.creation_date = DateTime.Now;
                                rs.creator = User.Identity.Name;
                                ln.repaymentSchedules.Add(rs);
                            }

                        }
                        
                    }
                    le.SaveChanges();

                    Session["loanGuarantors"] = null;
                    Session["loanCollaterals"] = null;
                    Session["loan.cl"] = null;
                    Session["loan"] = null;

                    HtmlHelper.MessageBox2("Loan Approval Data Saved Successfully!", ResolveUrl("~/ln/loans/default.aspx?catID=" +
                        ln.client.categoryID.ToString()), "coreERP©: Successful", IconType.ok);
                }
                else
                {
                    HtmlHelper.MessageBox("Kindly check to make sure this loan has been verified(checklist) and amount approved>0.", "coreERP: Incomplete", IconType.warning);
                }
            }
            else
            {
                HtmlHelper.MessageBox("Sorry, you cannot approve or deny an already approved or denied loan!");
            }
        } 

        protected void gridGuarantor_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                
            }
            else if (e.CommandName == "DeleteItem")
            {
            }
            gridGuarantor.DataSource = guarantors;
            gridGuarantor.DataBind();
        }
         
        protected void gridCollateral_ItemCommand(object sender, GridCommandEventArgs e)
        {
         
        }

        private void RenderImages()
        {
            if (client.clientImages != null)
            {
                foreach (var item in client.clientImages)
                {
                    //item.imageReference.Load();
                    RadBinaryImage img = new RadBinaryImage();
                    img.Width = 216;
                    img.Height = 216;
                    img.ResizeMode = BinaryImageResizeMode.Fill;
                    img.DataValue = item.image.image1;
                    RadRotatorItem it = new RadRotatorItem();
                    it.Controls.Add(img);
                    rotator1.Items.Add(it);
                }
            }
        }

        protected void btnAddCheck_Click(object sender, EventArgs e)
        {
            if (cboBank.SelectedValue != "" && this.txtCheckNo.Text != "" && txtCheckAmount.Text != "" && dtCheckDate.SelectedDate != null)
            {
                coreLogic.loanCheck g;
                if (btnAddCheck.Text == "Add Check")
                {
                    g = new coreLogic.loanCheck();
                }
                else
                {
                    g = Session["loanCheck"] as coreLogic.loanCheck;
                }
                int id = int.Parse(cboBank.SelectedValue);
                g.bankID = id;
                g.checkNumber = txtCheckNo.Text;
                g.checkAmount = txtCheckAmount.Value.Value;
                g.checkDate = dtCheckDate.SelectedDate.Value;
                if (btnAddCheck.Text == "Add Check")
                {
                    checks.Add(g);
                }
                Session["loanChecks"] = checks;
                gridChecks.DataSource = checks;
                gridChecks.DataBind();

                txtCheckNo.Text = "";
                txtCheckAmount.Value = null;
                cboBank.SelectedValue = "";
                dtCheckDate.SelectedDate = null;

                btnAddCheck.Text = "Add Check";
            }
        }

        protected void gridChecks_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = checks[e.Item.ItemIndex];
                if (g != null)
                {
                    txtCheckAmount.Value = g.checkAmount;
                    txtCheckNo.Text = g.checkNumber;
                    cboBank.SelectedValue = g.bankID.ToString();
                    dtCheckDate.SelectedDate = g.checkDate;

                    Session["loanCheck"] = g;
                    btnAddCheck.Text = "Update Check";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                checks.RemoveAt(e.Item.ItemIndex);
            }
            gridChecks.DataSource = checks;
            gridChecks.DataBind();
        }

        protected void btnDeny_Click(object sender, EventArgs e)
        {
            if (ln.loanStatusID!= 7 && ln.loanStatusID!= 3&& ln.loanStatusID!=4){
                ln.loanStatusID = 7;
                le.SaveChanges();
                Response.Redirect("/ln/loans/default.aspx");
            }
            else
            {
                HtmlHelper.MessageBox("Sorry, you cannot approve or deny an already approved or denied loan!");
            }
        }

        protected void gridCollateral_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            if (dataItem.ItemIndex < ln.loanCollaterals.Count)
            {
                e.DetailTableView.DataSource = ln.loanCollaterals.ToList()[dataItem.ItemIndex].collateralImages;
            }
        }

        private double ComputeApproved(int loanProductID)
        {
            double approved = 0.0;

            var lp = le.loanProducts.FirstOrDefault(p => p.loanProductID == loanProductID);
            if (lp != null)
            {
                detail = ln.prLoanDetails.FirstOrDefault();
                if (detail != null)
                {
                    var cntMandPassed = ln.loanCheckLists.Count(p => p.passed == true && p.categoryCheckList.isMandatory==true);
                    var cntMandNotPassed = ln.loanCheckLists.Count(p => p.passed == false && p.categoryCheckList.isMandatory == true);
                    var cntNonMandPassed = ln.loanCheckLists.Count(p => p.passed == true && p.categoryCheckList.isMandatory == false);

                    if (cntMandPassed > 0 && cntMandNotPassed == 0)
                    {
                        var basePerc = 90.0;
                        basePerc = basePerc + cntNonMandPassed * 2;
                        if (basePerc > 95) basePerc = 95;
                        if (chkAddFees.Checked == true && basePerc > 93)
                        {
                            basePerc = 93;
                        }
                        approved = detail.amd * basePerc / 100.0;
                        approved = Math.Ceiling((approved * lp.loanTenure) / (1 + ((lp.rate /100.0)* lp.loanTenure)));
                    }
                }
            }

            return approved;
        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {
            if (cboLoanProduct.SelectedValue != "")
            {
                int id = int.Parse(cboLoanProduct.SelectedValue);
                var lp = le.loanProducts.FirstOrDefault(p => p.loanProductID == id);
                txtProcFee.Value = Math.Ceiling(lp.procFeeRate * txtAmountApproved.Value.Value / 100.0);
                List<coreLogic.repaymentSchedule> sched =
                            schMgr.calculateScheduleM((chkAddFees.Checked ? txtProcFee.Value.Value : 0) +
                                txtAmountApproved.Value.Value, txtRate.Value.Value,
                            dtAppDate.SelectedDate.Value, txtTenure.Value.Value
                            );
                lblMD.Text = sched.Max(p => p.principalPayment + p.interestPayment).ToString("#,##0.#0");
            }
        }

        protected void cboLoanProduct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboLoanProduct.SelectedValue != "")
            {
                int id = int.Parse(cboLoanProduct.SelectedValue);
                var approved = ComputeApproved(id);
                txtAmountApproved.Value = approved;
                var lp = le.loanProducts.FirstOrDefault(p => p.loanProductID == id);
                txtTenure.Value = lp.loanTenure;
                txtRate.Value = lp.rate;
                txtProcFee.Value = Math.Ceiling(lp.procFeeRate * approved / 100.0);

                List<coreLogic.repaymentSchedule> sched =
                            schMgr.calculateScheduleM((chkAddFees.Checked ? txtProcFee.Value.Value : 0) +
                                approved, lp.rate,
                            dtAppDate.SelectedDate.Value, lp.loanTenure
                            );
                lblMD.Text = sched.Max(p => p.principalPayment + p.interestPayment).ToString("#,##0.#0");

                detail = ln.prLoanDetails.FirstOrDefault();
                detail.loanProductID=id;
            }
        }

        protected void chkAddFees_CheckedChanged(object sender, EventArgs e)
        { 
        }

        protected bool GetMandatory(object id)
        {
            var mand = false;
            var loanCheckListID = -1;
            if (id != null)
            {
                mand = (id as coreLogic.categoryCheckList).isMandatory;
            }
            return mand;
        }

        private void LoadLoan(int? id)
        {
            if (id != null)
            {
                ln = le.loans.FirstOrDefault(p => p.loanID == id);

                if (ln != null)
                {
                    //ln.loanCollaterals.Load();
                    //ln.loanGurantors.Load();
                    //ln.clientReference.Load();
                    //ln.loanStatuReference.Load();
                    //ln.repaymentSchedules.Load();
                    //ln.loanCheckLists.Load();
                    //ln.loanDocuments.Load();
                    //ln.prLoanDetails.Load();

                    detail = ln.prLoanDetails.FirstOrDefault();
                    client = ln.client;
                    //client.clientAddresses.Load();
                    //client.branchReference.Load();
                    ////client.categoryReference.Load();
                    //client.category.categoryCheckLists.Load();
                    //client.clientImages.Load();

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

                    guarantors = ln.loanGurantors.ToList();
                    collaterals = ln.loanCollaterals.ToList();
                    documents = ln.loanDocuments.ToList();

                    foreach (var i in guarantors)
                    {
                        //i.addressReference.Load();
                        //i.emailReference.Load();
                        //i.idNoReference.Load();
                        //i.imageReference.Load();
                        //i.phoneReference.Load();
                    }
                    foreach (var i in collaterals)
                    {
                        //i.collateralImages.Load();
                        foreach (var j in i.collateralImages)
                        {
                            //j.imageReference.Load();
                        }
                        //i.collateralTypeReference.Load();
                    } 
                    Session["loanGuarantors"] = guarantors;
                    Session["loanCollaterals"] = collaterals;
                    Session["loanChecks"] = checks;
                    Session["loanDocuments"] = documents;

                    RenderImages(); 
                    if (detail != null)
                    {
                        //detail.loanPurposeReference.Load();
                        if (detail.loanPurpose != null)
                        {
                            //detail.loanPurpose.loanPurposeDetails.Load(); 
                        }

                        //detail.prAllowances.Load();
                        allowances = detail.prAllowances.ToList();
                        foreach (var r in allowances)
                        {
                            //r.prAllowanceTypeReference.Load();
                        }
                        Session["allowances"] = allowances; 
                    }
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
                if (Session["loanGuarantors"] != null)
                {
                    guarantors = Session["loanGuarantors"] as List<coreLogic.loanGurantor>;
                }
                else
                {
                    guarantors = new List<coreLogic.loanGurantor>();
                    Session["loanGuarantors"] = guarantors;
                } 
                if (Session["allowances"] != null)
                {
                    allowances = Session["allowances"] as List<coreLogic.prAllowance>;
                }
                else
                {
                    allowances = new List<coreLogic.prAllowance>();
                    Session["allowances"] = allowances;
                }
                if (Session["loanChecks"] != null)
                {
                    checks = Session["loanChecks"] as List<coreLogic.loanCheck>;
                }
                else
                {
                    checks = new List<coreLogic.loanCheck>();
                    Session["loanChecks"] = checks;
                }
                if (Session["loanDocuments"] != null)
                {
                    documents = Session["loanDocuments"] as List<coreLogic.loanDocument>;
                }
                else
                {
                    documents = new List<coreLogic.loanDocument>();
                    Session["loanDocuments"] = documents;
                }
                if (Session["loanCollaterals"] != null)
                {
                    collaterals = Session["loanCollaterals"] as List<coreLogic.loanCollateral>;
                }
                else
                {
                    collaterals = new List<coreLogic.loanCollateral>();
                    Session["loanCollaterals"] = collaterals;
                }
            }
        }
    }
}