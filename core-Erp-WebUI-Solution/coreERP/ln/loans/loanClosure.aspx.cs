using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core.Objects;
using coreLogic;

namespace coreERP.ln.loans
{
    public partial class loanClosure : System.Web.UI.Page
    {
        private IIDGenerator idGen;
        IJournalExtensions journalextensions = new JournalExtensions();
        IScheduleManager schMgr = new ScheduleManager();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.loan ln;
        List<coreLogic.loanGurantor> guarantors;
        List<coreLogic.loanCollateral> collaterals;
        List<coreLogic.loanFinancial> financials;
        List<coreLogic.prAllowance> allowances;
        List<coreLogic.loanCheck> checks;
        List<coreLogic.loanDocument> documents;
        coreLogic.prLoanDetail detail;

        string categoryID = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            categoryID = Request.Params["catID"];
            ent = new coreLogic.core_dbEntities();
            le = new coreLogic.coreLoansEntities();
            idGen = new IDGenerator();
            if (!IsPostBack)
            {
                Session["id"] = null;
                cboIDType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.idNoTypes)
                {
                    cboIDType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.idNoTypeName, r.idNoTypeID.ToString()));
                }
                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.banks)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_name, r.bank_id.ToString()));
                }

                cboLoanScheme.Items.Add(new RadComboBoxItem("Not Applicable", ""));

                cboCollateralType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.collateralTypes)
                {
                    cboCollateralType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.collateralTypeName, r.collateralTypeID.ToString()));
                }
                cboFinancialType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.financialTypes)
                {
                    cboFinancialType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.financialTypeName, r.financialTypeID.ToString()));
                }

                cboRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.repaymentModes)
                {
                    cboRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentModeName, r.repaymentModeID.ToString()));
                }

                cboFrequency.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.repaymentModes)
                {
                    cboFrequency.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentModeName, r.repaymentModeID.ToString()));
                }

                cboInterestType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.interestTypes.Where(p => p.interestTypeID != 5))
                {
                    cboInterestType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.interestTypeName, r.interestTypeID.ToString()));
                }

                cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanTypes.Where(p=> p.loanTypeID!=7))
                {
                    cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanTypeName, r.loanTypeID.ToString()));
                }

                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs.OrderBy(p=> p.surName).ThenBy(p=> p.otherNames))
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.staffNo + ")", r.staffID.ToString()));
                }

                cboAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.agents.OrderBy(p=>p.surName))
                {
                    cboAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.agentNo + ")", r.agentID.ToString()));
                }

                cboLoanPurpose.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanPurposes.OrderBy(p=>p.loanPurposeName))
                {
                    cboLoanPurpose.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanPurposeName, r.loanPurposeID.ToString()));
                }

                cboLoanProduct.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanProducts)
                {
                    cboLoanProduct.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanProductName, r.loanProductID.ToString()));
                }

                cboAllowanceType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.prAllowanceTypes.OrderBy(p=>p.allowanceTypeName))
                {
                    cboAllowanceType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.allowanceTypeName, r.allowanceTypeID.ToString()));
                }
                cboModeOfEntry.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.modeOfEntries)
                {
                    cboModeOfEntry.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfEntryName, r.modeOfEntryID.ToString()));
                }
                cboReason.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanClosureReasons)
                {
                    cboReason.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanClosureReasonName, r.loanClosureReasonId.ToString()));
                }

                cboLegalOwner.Items.Add(new RadComboBoxItem("Guarantor", "Guarantor"));
                cboLegalOwner.Items.Add(new RadComboBoxItem("Applicant", "Applicant"));
                var pro = ent.comp_prof.FirstOrDefault();
                if (pro != null && pro.defaultInterestTypeID > 0)
                {
                    cboInterestType.SelectedValue = pro.defaultInterestTypeID.ToString();
                }
                else
                {
                    cboInterestType.SelectedValue = "1";
                }
                txtGracePeriod.Value = 0;
                txtGracePeriod.Text = "0";
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    ln = le.loans.FirstOrDefault(p => p.loanID == id);

                    if (ln != null)
                    {
                        if (ln.loanStatusID != 4)
                        {
                            Response.Redirect("/");
                        } 
                        client = ln.client; 
                        detail = ln.prLoanDetails.FirstOrDefault(); 
                        Session["loan.cl"] = client;

                        guarantors = ln.loanGurantors.ToList();
                        collaterals = ln.loanCollaterals.ToList();
                        financials = ln.loanFinancials.ToList(); 
                        documents = ln.loanDocuments.ToList();

                        Session["loanGuarantors"] = guarantors;
                        Session["loanCollaterals"] = collaterals;
                        Session["loanFinancials"] = financials;
                        Session["loanChecks"] = checks;
                        Session["loanDocuments"] = documents;

                        gridCollateral.DataSource = collaterals;
                        gridCollateral.DataBind();
                        
                        gridGuarantor.DataSource = guarantors;
                        gridGuarantor.DataBind();

                        gridDocument.DataSource = documents;
                        gridDocument.DataBind();

                        if (ln.loanStatu != null) lblStatus.Text = ln.loanStatu.loanStatusName;
                        txtTenure.Value = ln.loanTenure;
                        txtRate.Value = ln.interestRate;
                        txtGracePeriod.Value = ln.gracePeriod;
                        txtAmountRequested.Value = ln.amountRequested; 
                        txtProcFee.Value = ln.processingFee; 
                        cboRepaymentMode.SelectedValue = ln.repaymentModeID.ToString();
                        if (ln.loanTypeID == 7)
                        {
                            cboLoanType.Items.Add(new RadComboBoxItem("Susu Scheme", "7"));
                        }
                        var sch = le.loanSchemes.Where(p => p.loanTypeId == ln.loanTypeID)
                            .OrderBy(p=> p.loanSchemeName);
                        foreach (var s in sch)
                        {
                            cboLoanScheme.Items.Add(new RadComboBoxItem(
                                s.loanSchemeName, s.loanSchemeId.ToString()));
                        }
                        if (ln.loanSchemeId != null)
                        {
                            cboLoanScheme.SelectedValue = ln.loanSchemeId.ToString();
                            txtRate.Enabled = false;
                            txtTenure.Enabled = false;
                            cboRepaymentMode.Enabled = false;
                        }
                        cboLoanType.SelectedValue = ln.loanTypeID.ToString();
                        if (ln.interestTypeID == 5)
                        {
                            cboInterestType.Items.Add(new RadComboBoxItem("Susu Scheme", "5"));
                        }
                        cboInterestType.SelectedValue = ln.interestTypeID.ToString();
                        cboClient.SelectedValue = ln.clientID.ToString();
                        cboClient.Items.Clear();
                        cboClient.Items.Add(new RadComboBoxItem(
                            (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName +
                                ", " + client.otherNames + " (" + client.accountNumber + ")", client.clientID.ToString()));
                        cboClient.SelectedIndex = 0;
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
                        dtAppDate.SelectedDate = ln.applicationDate;
                        txtDisbursed.Value = ln.amountDisbursed;
                        txtCNotes.Text = ln.creditOfficerNotes;
                        txtInsurance.Value = ln.insuranceAmount;
                        if (ln.insuranceAmount > 0)
                        {
                            trInsurance.Visible = true;
                        }
                        txtAppComments.Text = (ln.approvalComments == null) ? "" : ln.approvalComments;
                        txtCreditManagerNotes.Text = (ln.creditManagerNotes == null) ? "" : ln.creditManagerNotes;
                        if (ln.loanStatusID > 1)
                        {
                            tab1.Tabs[12].Visible = true;
                        }
                        if (ln.loanStatusID > 2)
                        {
                            tab1.Tabs[11].Visible = true;
                        }
                        RenderImages(); 
                        gridSchedule.DataSource = ln.repaymentSchedules;
                        gridSchedule.DataBind();
                        gridRepayment.DataSource = ln.loanRepayments;
                        gridRepayment.DataBind();
                        gridFinancials.DataSource = ln.loanFinancials;
                        gridFinancials.DataBind();
                        gridPenalty.DataSource = ln.loanPenalties;
                        gridPenalty.DataBind();  
                        {
                            txtTenure.Enabled = false;
                            txtRate.Enabled = false;
                            txtGracePeriod.Enabled = false;
                            txtAmountRequested.Enabled = false; 
                            if (ln.processingFee > 0) txtProcFee.Enabled = false; else txtProcFee.Enabled = true; 
                            cboRepaymentMode.Enabled = false;
                            cboLoanType.Enabled = false;
                            cboInterestType.Enabled = false; 
                            cboClient.Enabled = false; 
                            cboLoanProduct.Enabled = false; 
                            txtProcFee.Value = ln.processingFeeBalance; 
                        } 
                        if (ln.processingFee == 0) txtProcFee.Enabled = true;   
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
                            txtBasicSalary.Value = detail.basicSalary;
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

                            //detail.prAllowances.Load();
                            allowances = detail.prAllowances.ToList();
                            foreach (var r in allowances)
                            {
                                //r.prAllowanceTypeReference.Load();
                            }
                            if (detail.modeOfEntryID != null)
                            {
                                cboModeOfEntry.SelectedValue = detail.modeOfEntryID.ToString();
                            }
                            Session["allowances"] = allowances;
                            gridAllowances.DataSource = allowances;
                            gridAllowances.DataBind();
                             
                        }
                    }
                    if (ln.staffID != null)
                    {
                        cboStaff.SelectedValue = ln.staffID.Value.ToString();
                    }
                    if (ln.agentID != null)
                    {
                        cboAgent.SelectedValue = ln.agentID.Value.ToString();
                    }

                    Session["loan"] = ln;
                }  
                if (categoryID != "5")
                {
                    this.multi1.PageViews[2].Selected = true;
                }
                else
                {
                    tab1.SelectedIndex = 0;
                    this.multi1.PageViews[0].Selected = true;
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
                tab1.Tabs[6].Visible = false;
                cboLoanProduct.Visible = true;
                pnlLP.Visible = true;
                pnlLT.Visible = false;
                cboLoanType.Visible = false;
                cboRepaymentMode.SelectedValue = "30";
                cboRepaymentMode.Enabled = false;

                txtTenure.Value = 0;
                txtRate.Value = 0;
                txtProcFee.Value = 0; 

                pnlCat5.Visible = false;
                pnlCat51.Visible = true;
                pnlFees.Visible = false;
            }
            else
            {
                pnlLP.Visible = false;
                pnlLT.Visible = true;
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

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                if (client != null)
                {
                    Session["loan.cl"] = client;  
                    rotator1.Items.Clear(); 
                    if (client.clientTypeID == 6)
                    {
                        pnlJoint.Visible = true;
                        pnlRegular.Visible = false;
                        txtJointAccountName.Text = client.accountName;
                        txtAccountNo.Text = client.accountNumber;
                    }
                    else
                    {
                        pnlJoint.Visible = false;
                        pnlRegular.Visible = true;
                        txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName;
                        txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            " " : client.otherNames;
                        txtAccountNo.Text = client.accountNumber;
                    }

                    RenderImages();
                }
            }
        }

        protected void cboLoanPurpose_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboLoanPurpose.SelectedValue != "")
            {
                int lpID = int.Parse(cboLoanPurpose.SelectedValue);
                var lp = le.loanPurposes.FirstOrDefault(p => p.loanPurposeID == lpID);
                if (lp != null)
                {
                    //lp.loanPurposeDetails.Load();
                    cboLoanPurposeDetail.Items.Clear();
                    cboLoanPurposeDetail.Items.Add(new RadComboBoxItem("", ""));
                    foreach (var r in lp.loanPurposeDetails)
                    {
                        cboLoanPurposeDetail.Items.Add(new RadComboBoxItem(r.loanPurposeDetailName, r.loanPurposeDetailID.ToString()));
                    }
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

        protected void btnFind_Click(object sender, EventArgs e)
        {
            int? catID = -1;
            if (categoryID != null) catID = int.Parse(categoryID);
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else
                clients = le.clients.Where(p => p.clientTypeID == 0 || p.clientTypeID == 2|| p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID==3||item.clientTypeID==4||item.clientTypeID==5)?item.companyName:((item.clientTypeID == 6)? item.accountName : item.surName + ", " + item.otherNames) + " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        protected void btnAddGuarantor_Click(object sender, EventArgs e)
        {  
        }

        protected void gridGuarantor_ItemCommand(object sender, GridCommandEventArgs e)
        {
             
        }

        protected void btnAddCollateral_Click(object sender, EventArgs e)
        {
            if (cboCollateralType.SelectedValue != "" && txtFairValue.Text != "" && txtFairValue.Value > 0)
            {
                
            }
        }

        protected void gridCollateral_ItemCommand(object sender, GridCommandEventArgs e)
        {
             
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAmountRequested.Value != null
                && txtGracePeriod.Value != null
                && txtRate.Value != null
                && txtTenure.Value != null
                && cboInterestType.SelectedValue != ""
                && cboLoanType.SelectedValue != ""
                && cboRepaymentMode.SelectedValue != ""
                && dtAppDate.SelectedDate != null)
            {
                if (Save() == true)
                {
                    HtmlHelper.MessageBox2("Loan Account Successfully Closed!", ResolveUrl("~/ln/loans/defaultClosure.aspx?catID="
                        + ((ln.client != null) ? ln.client : client).categoryID.ToString()), "coreERP©: Successful", IconType.ok);
                }
            }
        }
         
        private bool Save()
        {
            if (cboReason.SelectedValue!="")
            {
                int reasonId = int.Parse(cboReason.SelectedValue);
                le=new coreLoansEntities();
                var l = le.loans.First(p => p.loanID == ln.loanID);
                if (l.closed == null || l.closed == false)
                {
                    l.closed = true;
                    l.loanClosures.Add(new coreLogic.loanClosure
                    {
                        loanId = l.loanID,
                        approved = true,
                        approvalComments = "",
                        approvalDate = DateTime.Now,
                        approvedBy = User.Identity.Name,
                        feesAndChargesAtClosure = 0,
                        interestBalanceAtClosure = 0,
                        principalBalanceAtClosure = 0,
                        closureComments = "",
                        closureDate = DateTime.Now,
                        loanClosureReasonId = reasonId,
                        requestedClosureDate = DateTime.Now,
                        posted=false,
                        postedBy="",
                        postingDate=null,
                        requestedBy=User.Identity.Name,
                        
                    });
                    le.SaveChanges();


                    return true;
                }
            }

            return false;
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            if (ln != null && ln.loanStatusID == 1)
            {
                Response.Redirect("/ln/loans/loanCheckList.aspx?id=" + ln.loanID.ToString() + "&catID=" + ln.client.categoryID.ToString());
            }
            else if (ln != null && ln.loanStatusID == 2)
            {
                Response.Redirect("/ln/loans/approve.aspx?id=" + ln.loanID.ToString() + "&catID=" + ln.client.categoryID.ToString());
            }
        }

        protected void btnAddFinancial_Click(object sender, EventArgs e)
        {
             
        }

        protected void btnAddAllowance_Click(object sender, EventArgs e)
        {
             
        }

        protected void gridFinancials_ItemCommand(object sender, GridCommandEventArgs e)
        {
             
        }

        protected void gridAllowances_ItemCommand(object sender, GridCommandEventArgs e)
        {
             
        }
         
        protected void btnAddCheck_Click(object sender, EventArgs e)
        {
             
        }

        protected void gridChecks_ItemCommand(object sender, GridCommandEventArgs e)
        {
             
        }

        protected void txtTenure_TextChanged(object sender, EventArgs e)
        {
            OnChange();
        }

        protected void cboLoanProduct_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboLoanProduct.SelectedValue != "" && categoryID == "5")
            {
                int id = (int.Parse(cboLoanProduct.SelectedValue));
                var lt = le.loanProducts.FirstOrDefault(p => p.loanProductID == id);
                if (lt != null)
                {
                    var amount = 0.0;
                    if (txtAmountApproved.Value != null)
                    {
                        amount = txtAmountApproved.Value.Value;
                    }
                    else if (txtAmountRequested.Value != null)
                    {
                        amount = txtAmountRequested.Value.Value;
                    } 
                    if (lt.rate != null)
                    {
                        txtRate.Value = lt.rate;
                    } 
                }
            }
        }
        
        protected void gridCollateral_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            if (dataItem.ItemIndex<ln.loanCollaterals.Count)
            { 
                e.DetailTableView.DataSource = ln.loanCollaterals.ToList()[dataItem.ItemIndex].collateralImages;
            }
        }

        protected void btnAddDcoument_Click(object sender, EventArgs e)
        {
             
        }

        protected void gridDocument_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var doc = documents[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.loanDocuments.FirstOrDefault(p => p.loanDocumentID == doc.loanDocumentID);
                        if (toBeDeleted != null)
                        {
                            lent.loanDocuments.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                Session["loanDocuments"] = documents;
                documents.RemoveAt(e.Item.ItemIndex);
            }
            gridDocument.DataSource = documents;
            gridDocument.DataBind();
        }

        protected void btnGuarantorAsApplicant_Click(object sender, EventArgs e)
        {
            try
            {
                if (guarantors.Count == 0)
                {
                    coreLogic.loanGurantor g;
                    g = new coreLogic.loanGurantor();
                    g.creation_date = DateTime.Now;
                    g.creator = User.Identity.Name;
                    g.surName = ln.client.surName;
                    g.otherNames = ln.client.otherNames;
                    g.DOB = ln.client.DOB;
                    guarantors.Add(g);
                    Session["loanGuarantors"] = guarantors;

                    gridGuarantor.DataSource = guarantors;
                    gridGuarantor.DataBind();
                }
            }
            catch (Exception) { }
        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {
            ComputeGross();
            ComputeNet();
            ComputeAMD();
        }

        private void ComputeGross()
        {
            double gross = 0;
            gross = txtBasicSalary.Value.Value;
            if (allowances.Count > 0)
            {
                foreach (var r in allowances)
                {
                    var t = le.prAllowanceTypes.FirstOrDefault(p => p.allowanceTypeID == r.allowanceTypeID);
                    if (t != null && t.isPermanent==true)
                    {
                        gross += r.amount;
                    }
                }
            }
            txtGrossSalary.Value = gross;
            txtGrossSalary.Text = gross.ToString();
        }

        private void ComputeAMD()
        {
            double amd = 0;
            amd = txtGrossSalary.Value.Value;
            if (this.txtTax.Value != null)
            {
                amd -= txtTax.Value.Value;
            }
            if (this.txtSSWelfare.Value != null)
            {
                amd -= txtSSWelfare.Value.Value;
            }
            amd /= 2.0;
            if (txtTotalDeductions.Value != null)
            {
                amd -= txtTotalDeductions.Value.Value;
            }
            if (this.txtTotalDedNotOnPR.Value != null)
            {
                amd -= txtTotalDedNotOnPR.Value.Value;
            }
            if (amd < 0) amd = 0;
            txtAMD.Value = amd;
            txtAMD.Text = amd.ToString();
        }

        private void ComputeNet()
        {
            double net = 0;
            net = txtGrossSalary.Value.Value ;
            if (txtTotalDeductions.Value != null)
            {
                net -= txtTotalDeductions.Value.Value;
            }
            if (this.txtTotalDedNotOnPR.Value != null)
            {
                net -= txtTotalDedNotOnPR.Value.Value;
            }
            if (this.txtTax.Value != null)
            {
                net -= txtTax.Value.Value;
            }
            if (this.txtSSWelfare.Value != null)
            {
                net -= txtSSWelfare.Value.Value;
            }
            txtNetSalary.Value = net;
            txtNetSalary.Text = net.ToString();
        }

        protected void cboLoanType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            if (cboLoanType.SelectedValue != "")
            {
                int loanTypeId = int.Parse(cboLoanType.SelectedValue);
                var sch = le.loanSchemes.Where(p => p.loanTypeId == loanTypeId)
                            .OrderBy(p => p.loanSchemeName);
                if (sch.Count() > 0)
                {
                    cboLoanScheme.Items.Clear();
                    cboLoanScheme.Items.Add(new RadComboBoxItem("Not Applicable", ""));
                    foreach (var s in sch)
                    {
                        cboLoanScheme.Items.Add(new RadComboBoxItem(
                            s.loanSchemeName, s.loanSchemeId.ToString()));
                    }
                }
            }
            if (cboLoanScheme.SelectedValue == "" && txtTenure.Value != null && categoryID != "5" && cboLoanType.SelectedValue!="")
            {
                int tenure = (int)txtTenure.Value.Value;
                if (tenure < 2) tenure = 1;
                else if (tenure <= 3) tenure = 3;
                else if (tenure <= 6) tenure = 6;
                else if (tenure <= 9) tenure = 9;
                else if (tenure <= 12) tenure = 12;
                else if (tenure <= 18) tenure = 18;
                else if (tenure <= 24) tenure = 24;
                else if (tenure <= 36) tenure = 36;
                else if (tenure <= 48) tenure = 48;
                else if (tenure <= 60) tenure = 60;
                int id = int.Parse(cboLoanType.SelectedValue);
                var tenure2 = (int)txtTenure.Value.Value;
                var lt = le.tenors.FirstOrDefault(p => p.tenor1 == tenure2 && p.loanTypeID==id);
                if (lt == null)
                {
                    lt = le.tenors.FirstOrDefault(p => p.tenor1 == tenure && p.loanTypeID == id);
                    if (lt == null)
                    {
                        lt = le.tenors.FirstOrDefault(p => p.tenor1 == tenure);
                        if (lt == null)
                        {
                            tenure = (int)txtTenure.Value.Value;
                            lt = le.tenors.FirstOrDefault();
                        }
                    }
                }
                if (lt != null)
                {
                    var amount = 0.0;
                    if (txtAmountApproved.Value != null)
                    {
                        amount = txtAmountApproved.Value.Value;
                    }
                    else if (txtAmountRequested.Value != null)
                    {
                        amount = txtAmountRequested.Value.Value;
                    }  
                    if (lt.defaultProcessingFeeRate != null)
                    {
                        if (ent.comp_prof.First().comp_name.Contains("Link"))
                        {
                            txtProcFee.Value = Math.Ceiling(amount * lt.defaultProcessingFeeRate.Value / 100.0);
                        }
                        else
                        {
                            txtProcFee.Value = amount * lt.defaultProcessingFeeRate.Value / 100.0;
                        }
                    }
                    if (lt.defaultInterestRate != null)
                    {
                        txtRate.Value = lt.defaultInterestRate;
                    }
                    if (lt.defaultRepaymentModeID != null)
                    {
                        cboRepaymentMode.SelectedValue = lt.defaultRepaymentModeID.ToString();
                    }
                    if (lt.defaultGracePeriod != null)
                    {
                        txtGracePeriod.Value = lt.defaultGracePeriod;
                    }
                    if (ent.comp_prof.FirstOrDefault().deductInsurance == true && cboLoanType.SelectedValue != ""
                        && amount >= 1000)
                    {
                        int ltID = int.Parse(cboLoanType.SelectedValue);
                        var li = le.insuranceSetups.FirstOrDefault(p => p.loanTypeID == ltID);
                        if (li != null)
                        {
                            txtInsurance.Value = li.insurancePercent * amount / 100.0;
                            txtInsurance.Visible = true;
                            trInsurance.Visible = true;
                        }
                    }
                }
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

        private void LoadLoan(int? id)
        {
            if (id != null)
            {
                ln = le.loans.FirstOrDefault(p => p.loanID == id);

                if (ln != null)
                { 
                    client = ln.client;
                    detail = ln.prLoanDetails.FirstOrDefault();

                    Session["loan.cl"] = client;

                    guarantors = ln.loanGurantors.ToList();
                    collaterals = ln.loanCollaterals.ToList();
                    financials = ln.loanFinancials.ToList();
                    documents = ln.loanDocuments.ToList();

                    if (Session["loanGuarantors"] != null)
                    {
                        var g = Session["loanGuarantors"] as List<coreLogic.loanGurantor>;
                        if (g != null)
                        {
                            foreach (var r in g)
                            {
                                if (r.loanGurantorID <= 0)
                                {
                                    guarantors.Add(r);
                                }
                            }
                        }
                    }
                    if (Session["loanCollaterals"] != null)
                    {
                        var g = Session["loanCollaterals"] as List<coreLogic.loanCollateral>;
                        if (g != null)
                        {
                            foreach(var r in g){
                                if( r.loanCollateralID<=0){
                                    collaterals.Add(r);
                                }
                            }
                        }
                    }
                    if (Session["loanFinancials"] != null)
                    {
                        var g = Session["loanFinancials"] as List<coreLogic.loanFinancial>;
                        if (g != null)
                        {
                            foreach(var r in g){
                                if( r.loanFinancialID<=0){
                                    financials.Add(r);
                                }
                            }
                        }
                    }
                    if (Session["loanChecks"] != null)
                    {
                        var g = Session["loanChecks"] as List<coreLogic.loanCheck>;
                        if (g != null)
                        {
                            foreach(var r in g){
                                if( r.loanCheckID<=0){
                                    checks.Add(r);
                                }
                            }
                        }
                    }
                    if (Session["loanDocuments"] != null)
                    {
                        var g = Session["loanDocuments"] as List<coreLogic.loanDocument>;
                        if (g != null)
                        {
                            foreach(var r in g){
                                if( r.loanDocumentID<=0){
                                    documents.Add(r);
                                }
                            }
                        }
                    }
                    Session["loanGuarantors"] = guarantors;
                    Session["loanCollaterals"] = collaterals;
                    Session["loanFinancials"] = financials;
                    Session["loanChecks"] = checks;
                    Session["loanDocuments"] = documents;
                      
                    if (detail != null)
                    {
                        allowances = detail.prAllowances.ToList();
                        if (Session["allowances"] != null)
                        {
                            var g = Session["allowances"] as List<coreLogic.prAllowance>;
                            if (g != null)
                            {
                                foreach(var r in g){
                                    if( r.prAllowanceID<=0){
                                        allowances.Add(r);
                                    }
                                }
                            }
                        }
                        Session["allowances"] = allowances;

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
                if (Session["loanGuarantors"] != null)
                {
                    guarantors = Session["loanGuarantors"] as List<coreLogic.loanGurantor>;
                }
                else
                {
                    guarantors = new List<coreLogic.loanGurantor>();
                    Session["loanGuarantors"] = guarantors;
                }
                if (Session["loanFinancials"] != null)
                {
                    financials = Session["loanFinancials"] as List<coreLogic.loanFinancial>;
                }
                else
                {
                    financials = new List<coreLogic.loanFinancial>();
                    Session["loanFinancials"] = financials;
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

        protected void gridSchedule_Load(object sender, EventArgs e)
        {
            if (ln != null && ln.repaymentSchedules != null)
            {
                gridSchedule.DataSource = ln.repaymentSchedules.ToList();
            }
        }

        protected void cboLoanScheme_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboLoanScheme.SelectedValue != "")
            {
                int loanSchemeId = int.Parse(cboLoanScheme.SelectedValue);
                var sch = le.loanSchemes.First(p => p.loanSchemeId == loanSchemeId);
                if (loanSchemeId != null && sch != null)
                {
                    txtRate.Value = sch.rate;
                    txtTenure.Value = sch.tenure;
                    cboRepaymentMode.SelectedValue = "30";

                    txtRate.Enabled = false;
                    txtTenure.Enabled = false;
                    cboRepaymentMode.Enabled = false;
                }
            }
        }
    }
}