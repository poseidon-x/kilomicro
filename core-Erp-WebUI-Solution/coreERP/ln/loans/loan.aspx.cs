using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using coreLogic;
using System.Data.Entity;

namespace coreERP.ln.loans
{
    public partial class loan : System.Web.UI.Page
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
                var comp = ent.comp_prof.FirstOrDefault();
                if (comp != null && !comp.comp_name.ToLower().Contains("ttl"))
                {
                    txtSecurityDeposit.Enabled = false;
                }
                if (comp.comp_name.ToLower().Contains("lendzee") || comp.comp_name.ToLower().Contains("kilo"))
                {
                    txtApplicationFee.Value = 0;
                    txtApplicationFee.Enabled = false;
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
                foreach (var r in le.loanTypes/*.Where(p=> p.loanTypeID!=7 && p.loanTypeID != 10)*/)
                {
                    cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanTypeName, r.loanTypeID.ToString()));
                }

                //TODO: Check against login user
                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                var user = User?.Identity?.Name?.Trim()?.ToLower();
                var staffs = le.staffs.OrderBy(p => p.surName).ThenBy(p => p.otherNames).ToList();
                if (!IsOwnerOrAdmin(user))
                {
                    staffs = staffs.Where(p => p.userName?.Trim()?.ToLower() == user).ToList();
                }
                foreach (var r in staffs)
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.staffNo + ")", r.staffID.ToString()));
                }
                cboAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.agents.OrderBy(p => p.surName))
                {
                    cboAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.agentNo + ")", r.agentID.ToString()));
                }

                cboLoanPurpose.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanPurposes.OrderBy(p => p.loanPurposeName))
                {
                    cboLoanPurpose.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanPurposeName, r.loanPurposeID.ToString()));
                }

                cboLoanProduct.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanProducts)
                {
                    cboLoanProduct.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanProductName, r.loanProductID.ToString()));
                }

                cboAllowanceType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.prAllowanceTypes.OrderBy(p => p.allowanceTypeName))
                {
                    cboAllowanceType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.allowanceTypeName, r.allowanceTypeID.ToString()));
                }
                cboModeOfEntry.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.modeOfEntries)
                {
                    cboModeOfEntry.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfEntryName, r.modeOfEntryID.ToString()));
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
                        if (ln.loanStatusID == 1)
                        {
                            btnApprove.Enabled = true;
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
                            .OrderBy(p => p.loanSchemeName);
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
                        if (ln.loanStatusID == 4 || ln.loanStatusID == 3)
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
                            btnFind.Enabled = false;
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
                else if (Request.Params["op"] == "new")
                {
                    cboInterestType.SelectedValue = Request.Params["i"];
                    cboRepaymentMode.SelectedValue = Request.Params["rm"];
                    txtAmountRequested.Value = double.Parse(Request.Params["a"]);
                    txtRate.Value = double.Parse(Request.Params["r"]);
                    dtAppDate.SelectedDate = DateTime.ParseExact(Request.Params["d"], "dd-MMM-yyyy", System.Globalization.CultureInfo.CurrentCulture);

                    txtTenure.Value = double.Parse(Request.Params["t"]);
                }
                else
                {
                    dtAppDate.SelectedDate = DateTime.Now;
                    Session["loan"] = ln;
                    Session["loanCollaterals"] = null;
                    Session["loanGuarantors"] = null;
                    Session["loanCollaterals"] = null;
                    Session["loanFinancials"] = null;
                    Session["loanChecks"] = null;
                    Session["loanDocuments"] = null;
                    LoadLoan(null);
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
                if (cboLoanType.SelectedValue != "7")
                { cboRepaymentMode.SelectedValue = "30"; }
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
                client = le.clients
                    .Include(r => r.loanGroupClients)
                    .Include(r => r.loanGroupClients.Select(p => p.loanGroup))
                    .FirstOrDefault(p => p.clientID == clientID);
                if (client != null)
                {

                    cboLoanType.Items.Clear();
                    cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                    foreach (var r in le.loanTypes.Where(p => p.loanTypeID != 7 && p.loanTypeID != 10))
                    {
                        cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanTypeName, r.loanTypeID.ToString()));
                    }

                    Session["loan.cl"] = client;
                    rotator1.Items.Clear();

                    var clientTypes =
                        le.clientTypes.FirstOrDefault(p => p.clientTypeName.ToLower().Contains("group client"));
                    if (client.clientTypeID == 6)
                    {
                        pnlJoint.Visible = true;
                        pnlRegular.Visible = false;
                        txtJointAccountName.Text = client.accountName;
                        txtAccountNo.Text = client.accountNumber;

                        //If user select Group loan client and later change to select non group loan client
                        txtTenure.Value = null;
                        txtTenure.ReadOnly = false;
                        cboRepaymentMode.SelectedValue = null;
                        cboRepaymentMode.Enabled = true;
                        txtRate.Value = null;
                        cboStaff.Enabled = true;
                    }
                    else if (client.clientTypeID == 7)
                    {
                        int clientGroups = client.loanGroupClients.Count;
                        var grp = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 10);
                        if (grp != null && clientGroups > 0) cboLoanType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(grp.loanTypeName, grp.loanTypeID.ToString()));

                        pnlJoint.Visible = false;
                        pnlRegular.Visible = true;
                        txtSurname.Text = client.surName;
                        txtOtherNames.Text = client.otherNames;
                        txtAccountNo.Text = client.accountNumber;
                        cboStaff.Enabled = false;
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

                        //If user select Gruop loan client and later change to select non group loan client
                        txtTenure.Value = null;
                        txtTenure.ReadOnly = false;
                        cboRepaymentMode.SelectedValue = null;
                        cboRepaymentMode.Enabled = true;
                        txtRate.Value = null;
                        cboStaff.Enabled = true;
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
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            else
                clients = le.clients.Where(p => p.clientTypeID == 0 || p.clientTypeID == 2 || p.clientTypeID == 4 || p.clientTypeID == 6).Where(
                    p => ((catID != 5 && p.categoryID != 5) || (catID == 5 && p.categoryID == 5))).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID == 3 || item.clientTypeID == 4 || item.clientTypeID == 5) ? item.companyName : ((item.clientTypeID == 6) ? item.accountName : item.surName + ", " + item.otherNames) + " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        private bool SaveGuarantor()
        {
            if (txtGSurname.Text != "" && txtGOtherNames.Text != "")
            {
                coreLogic.loanGurantor g;
                if (btnAddGuarantor.Text == "Add Guarantor")
                {
                    g = new coreLogic.loanGurantor();
                    g.creation_date = DateTime.Now;
                    g.creator = User.Identity.Name;
                }
                else
                {
                    g = Session["loanGuarantor"] as coreLogic.loanGurantor;
                }
                g.surName = txtGSurname.Text;
                g.otherNames = txtGOtherNames.Text;
                g.DOB = dtGDOB.SelectedDate;
                if (txtAddress.Text != "" && txtCity.Text != "")
                {
                    if (g.address == null) g.address = new coreLogic.address();
                    g.address.addressLine1 = txtAddress.Text;
                    g.address.cityTown = txtCity.Text;
                }
                if (cboIDType.SelectedValue != "" && txtIDNo.Text != "")
                {
                    if (g.idNo == null) g.idNo = new coreLogic.idNo();
                    g.idNo.idNoTypeID = int.Parse(cboIDType.SelectedValue);
                    g.idNo.idNo1 = txtIDNo.Text;
                }
                if (txtPhoneNo.Text != "")
                {
                    if (txtPhoneNo.Text.Length != 10)
                    {

                        HtmlHelper.MessageBox("Phone Number should be 10 digits",
                                   "coreERP©: Invalid Input", IconType.warning);
                        return false;

                    }
                    else
                    {
                        if (g.phone == null) g.phone = new coreLogic.phone();
                        g.phone.phoneNo = txtPhoneNo.Text;
                        g.phone.phoneTypeID = 1;
                    }
                }
                if (txtEmail.Text != "")
                {
                    if (g.email == null) g.email = new coreLogic.email();
                    g.email.emailAddress = txtEmail.Text;
                    g.email.emailTypeID = 1;
                }
                if (upload1.UploadedFiles.Count == 1)
                {
                    var item = upload1.UploadedFiles[0];
                    byte[] b = new byte[item.InputStream.Length];
                    item.InputStream.Read(b, 0, b.Length);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                    System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
                    i2 = i2.GetThumbnailImage(480, 480, null, IntPtr.Zero);
                    ms = null;
                    ms = new System.IO.MemoryStream();
                    i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    b = ms.ToArray();
                    i2 = null;
                    ms = null;

                    var i = new coreLogic.image
                    {
                        description = item.FileName,
                        image1 = b,
                        content_type = item.ContentType
                    };
                    g.image = i;
                }
                if (string.IsNullOrWhiteSpace(g.phone.phoneNo))
                {
                    //TODO: Alert user for phone number
                    HtmlHelper.MessageBox("Phone Number should be 10 digits",
                                   "coreERP©: Invalid Input", IconType.deny);
                    return false;
                }
                else
                {
                    if (btnAddGuarantor.Text == "Add Guarantor")
                    {
                        guarantors.Add(g);
                    }
                    Session["loanGuarantors"] = guarantors;
                    gridGuarantor.DataSource = guarantors;
                    gridGuarantor.DataBind();

                    //txtGOtherNames.Text = "";
                    //txtGSurname.Text = "";
                    //dtGDOB.SelectedDate = null;

                    //txtAddress.Text = "";
                    //txtCity.Text = "";
                    //txtPhoneNo.Text = "";
                    //txtEmail.Text = "";
                    //txtIDNo.Text = "";

                    //cboIDType.SelectedValue = "";
                    btnAddGuarantor.Text = "Add Guarantor";
                }

            }
            return true;
        }

        protected void btnAddGuarantor_Click(object sender, EventArgs e)
        {
            if (txtGSurname.Text != "" && txtGOtherNames.Text != "")
            {
                coreLogic.loanGurantor g;
                if (btnAddGuarantor.Text == "Add Guarantor")
                {
                    g = new coreLogic.loanGurantor();
                    g.creation_date = DateTime.Now;
                    g.creator = User.Identity.Name;
                }
                else
                {
                    g = Session["loanGuarantor"] as coreLogic.loanGurantor;
                }
                g.surName = txtGSurname.Text;
                g.otherNames = txtGOtherNames.Text;
                g.DOB = dtGDOB.SelectedDate;
                if (txtAddress.Text != "" && txtCity.Text != "")
                {
                    if (g.address == null) g.address = new coreLogic.address();
                    g.address.addressLine1 = txtAddress.Text;
                    g.address.cityTown = txtCity.Text;
                }
                if (cboIDType.SelectedValue != "" && txtIDNo.Text != "")
                {
                    if (g.idNo == null) g.idNo = new coreLogic.idNo();
                    g.idNo.idNoTypeID = int.Parse(cboIDType.SelectedValue);
                    g.idNo.idNo1 = txtIDNo.Text;
                }
                if (txtPhoneNo.Text != "")
                {
                    if (txtPhoneNo.Text.Length != 10)
                    {

                        HtmlHelper.MessageBox("Phone Number should be 10 digits",
                                   "coreERP©: Invalid Input", IconType.deny);
                       

                    }
                    else
                    {
                        if (g.phone == null) g.phone = new coreLogic.phone();
                        g.phone.phoneNo = txtPhoneNo.Text;
                        g.phone.phoneTypeID = 1;
                    }
                }
                if (txtEmail.Text != "")
                {
                    if (g.email == null) g.email = new coreLogic.email();
                    g.email.emailAddress = txtEmail.Text;
                    g.email.emailTypeID = 1;
                }
                if (upload1.UploadedFiles.Count == 1)
                {
                    var item = upload1.UploadedFiles[0];
                    byte[] b = new byte[item.InputStream.Length];
                    item.InputStream.Read(b, 0, b.Length);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                    System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
                    i2 = i2.GetThumbnailImage(480, 480, null, IntPtr.Zero);
                    ms = null;
                    ms = new System.IO.MemoryStream();
                    i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    b = ms.ToArray();
                    i2 = null;
                    ms = null;

                    var i = new coreLogic.image
                    {
                        description = item.FileName,
                        image1 = b,
                        content_type = item.ContentType
                    };
                    g.image = i;
                }
                //if (string.IsNullOrWhiteSpace(g.phone.phoneNo))
                //if(g.phone== null)
                //{
                //    //TODO: Alert user for phone number
                //    HtmlHelper.MessageBox("Guarantor's phone number shou");
                //    return;
                //}
                //else
                //{
                    if (btnAddGuarantor.Text == "Add Guarantor")
                    {
                        guarantors.Add(g);
                    }
                    Session["loanGuarantors"] = guarantors;
                    gridGuarantor.DataSource = guarantors;
                    gridGuarantor.DataBind();

                    //txtGOtherNames.Text = "";
                    //txtGSurname.Text = "";
                    //dtGDOB.SelectedDate = null;

                    //txtAddress.Text = "";
                    //txtCity.Text = "";
                    //txtPhoneNo.Text = "";
                    //txtEmail.Text = "";
                    //txtIDNo.Text = "";

                    //cboIDType.SelectedValue = "";
                    btnAddGuarantor.Text = "Add Guarantor";
                //}
                
            }
        }

        protected void gridGuarantor_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = guarantors[e.Item.ItemIndex];
                if (g != null)
                {
                    txtGOtherNames.Text = g.otherNames;
                    txtGSurname.Text = g.surName;
                    dtGDOB.SelectedDate = g.DOB;

                    if (g.address != null)
                    {
                        txtAddress.Text = g.address.addressLine1;
                        txtCity.Text = g.address.cityTown;
                    }
                    if (g.phone != null)
                    {
                        txtPhoneNo.Text = g.phone.phoneNo;
                    }
                    if (g.email != null)
                    {
                        txtEmail.Text = g.email.emailAddress;
                    }
                    if (g.idNo != null)
                    {
                        txtIDNo.Text = g.idNo.idNo1;
                        cboIDType.SelectedValue = g.idNo.idNoTypeID.ToString();
                    }
                    Session["loanGuarantor"] = g;
                    btnAddGuarantor.Text = "Update Guarantor";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var gua = guarantors[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.loanGurantors.FirstOrDefault(p => p.loanGurantorID == gua.loanGurantorID);
                        if (toBeDeleted != null)
                        {
                            lent.loanGurantors.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                guarantors.RemoveAt(e.Item.ItemIndex);
            }
            gridGuarantor.DataSource = guarantors;
            gridGuarantor.DataBind();
        }

        protected void btnAddCollateral_Click(object sender, EventArgs e)
        {
            if (cboCollateralType.SelectedValue != "" && txtFairValue.Text != "" && txtFairValue.Value > 0)
            {
                coreLogic.loanCollateral g;
                if (btnAddCollateral.Text == "Add Collateral")
                {
                    g = new coreLogic.loanCollateral();
                    g.creation_date = DateTime.Now;
                    g.creator = User.Identity.Name;
                }
                else
                {
                    g = Session["loanCollateral"] as coreLogic.loanCollateral;
                }
                int id = int.Parse(cboCollateralType.SelectedValue);
                g.collateralTypeID = id;//le.collateralTypes.FirstOrDefault(p => p.collateralTypeID == id) ;
                g.fairValue = txtFairValue.Value.Value;
                g.legalOwner = cboLegalOwner.Text;
                g.collateralDescription = txtCollateralDescription.Text;
                foreach (UploadedFile item in upload2.UploadedFiles)
                {
                    byte[] b = new byte[item.InputStream.Length];
                    item.InputStream.Read(b, 0, b.Length);
                    var i = new coreLogic.image
                    {
                        description = item.FileName,
                        image1 = b,
                        content_type = item.ContentType
                    };
                    coreLogic.collateralImage img = new coreLogic.collateralImage();
                    img.image = i;
                    g.collateralImages.Add(img);
                }
                if (btnAddCollateral.Text == "Add Collateral")
                {
                    collaterals.Add(g);
                }
                Session["loanCollaterals"] = collaterals;
                gridCollateral.DataSource = collaterals;
                gridCollateral.DataBind();

                txtFairValue.Value = null;
                cboCollateralType.SelectedValue = "";
                txtCollateralDescription.Text = "";

                rotator2.Items.Clear();
                btnAddCollateral.Text = "Add Collateral";
            }
        }

        protected void gridCollateral_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = collaterals[e.Item.ItemIndex];
                if (g != null)
                {
                    txtFairValue.Value = g.fairValue;
                    cboCollateralType.SelectedValue = g.collateralTypeID.ToString();
                    cboLegalOwner.SelectedValue = g.legalOwner;
                    txtCollateralDescription.Text = g.collateralDescription;

                    foreach (var item in g.collateralImages)
                    {
                        //item.imageReference.Load();
                        RadBinaryImage img = new RadBinaryImage();
                        img.Width = 320;
                        img.Height = 180;
                        img.ResizeMode = BinaryImageResizeMode.Fit;
                        img.DataValue = item.image.image1;
                        RadRotatorItem it = new RadRotatorItem();
                        it.Controls.Add(img);
                        rotator2.Items.Add(it);
                    }

                    Session["loanCollateral"] = g;
                    btnAddCollateral.Text = "Update Collateral";
                    gridCollateral.DataSource = collaterals;
                    gridCollateral.DataBind();
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var col = collaterals[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.loanCollaterals.FirstOrDefault(p => p.loanCollateralID == col.loanCollateralID);
                        if (toBeDeleted != null)
                        {
                            foreach (var img in col.collateralImages)
                            {
                                lent.collateralImages.Remove(img);
                            }
                            lent.loanCollaterals.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                collaterals.RemoveAt(e.Item.ItemIndex);
                Session["loanCollaterals"] = collaterals;
                gridCollateral.DataSource = collaterals;
                gridCollateral.DataBind();
            }
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
                var isGuarantorDetails = SaveGuarantor();
                if (!isGuarantorDetails)
                {
                   
                    HtmlHelper.MessageBox("Phone number must be at least 10 digits.",
                                   "coreERP©: Invalid Input", IconType.warning);
                }
                else
                {
                    if (Save() == true)
                    {
                        HtmlHelper.MessageBox2("Loan Data Saved Successfully!", ResolveUrl("~/ln/loans/default.aspx?catID="
                            + ((ln.client != null) ? ln.client : client).categoryID.ToString()), "coreERP©: Successful", IconType.ok);
                    }
                }
            }
        }

        protected void btnSave2_Click(object sender, EventArgs e)
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
                var isGuarantorDetails = SaveGuarantor();
                if (!isGuarantorDetails)
                {
                    Session["loanGuarantors"] = null;
                    gridGuarantor.DataSource = null;
                    HtmlHelper.MessageBox("Phone number must be at least 10 digits.",
                                   "coreERP©: Invalid Input", IconType.warning);
                }
                else
                {
                        if (Save() == true)
                    {
                        HtmlHelper.MessageBox("Loan Data Saved Successfully!");
                    }
                 }
            }
        }

        private bool Save()
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
                var existingUnpaidLoan = le.loans.Where(p => p.clientID == client.clientID && p.balance > 5 && p.loanID != ln.loanID).FirstOrDefault();
                //check if existingUnpaidLoan !=null && profile is lendzee
                //
                //then raise error and return
                var compName = ent.comp_prof.First().comp_name.ToLower();
                if (existingUnpaidLoan != null && (compName.Contains("lendzee") || compName.Contains("kilo")))
                {
                    HtmlHelper.MessageBox("A loan with the same details exist in the system. If this is indeed a different loan click save again, otherwise exit the sreen and try again.",
                            "coreERP: Duplicate Loan", IconType.warning);
                    return false;
                }

                if (client.clientID > 0 && ln.loanID <= 0 && (Session["override"] == null || (bool)Session["override"] == false))
                {
                    var l2 = le.loans.FirstOrDefault(p => p.clientID == client.clientID && p.amountRequested == txtAmountRequested.Value.Value
                        && p.loanTenure == txtTenure.Value.Value && p.applicationDate == dtAppDate.SelectedDate);
                    if (l2 != null)
                    {
                        HtmlHelper.MessageBox("A loan with the same details exist in the system. If this is indeed a different loan click save again, otherwise exit the sreen and try again.",
                            "coreERP: Duplicate Loan", IconType.warning);
                        Session["override"] = true;
                        return false;
                    }
                }
                if (txtInsurance.Value != null)
                {
                    ln.insuranceAmount = txtInsurance.Value.Value;
                }

                if (txtCNotes.Text != "")
                {
                    ln.creditOfficerNotes = txtCNotes.Text;
                }
                if (txtCreditManagerNotes.Text != "")
                {
                    ln.creditManagerNotes = txtCreditManagerNotes.Text;
                }
                if (ln.loanStatusID < 2)
                {
                    int loanTypeId = int.Parse(cboLoanType.SelectedValue);
                    var lnTyp = le.loanTypes.FirstOrDefault(p => p.loanTypeID == loanTypeId)
                        .loanTypeName;
                    double? securityValue = txtSecurityDeposit.Value;
                    if (lnTyp != null && lnTyp.ToLower().Contains("group loan"))
                    {

                        //if (securityValue != null && securityValue > 0)
                        //{
                        //    var clientSavAcc = le.savings.FirstOrDefault(p => p.clientID == client.clientID);
                        //    if (clientSavAcc == null)
                        //    {
                        //        HtmlHelper.MessageBox("Client doesn't have a savings account!, Please create one and continue Later"
                        //                              , "coreERP: Savings account Required", IconType.warning);
                        //        Session["override"] = false;
                        //        return false;

                        //    }

                        //}
                    }

                    ln.amountRequested = txtAmountRequested.Value.Value;
                    ln.applicationDate = dtAppDate.SelectedDate.Value;
                    ln.clientID = client.clientID;
                    ln.creation_date = DateTime.Now;
                    ln.creator = User.Identity.Name;
                    ln.gracePeriod = (int)txtGracePeriod.Value.Value;
                    ln.interestRate = txtRate.Value.Value;
                    ln.applicationFee = txtApplicationFee == null ? 0 : txtApplicationFee.Value.Value;
                    ln.interestTypeID = int.Parse(cboInterestType.SelectedValue);
                    ln.enteredBy = User.Identity.Name;
                    ln.applicationFeeBalance = txtApplicationFee == null ? 0 : txtApplicationFee.Value.Value;
                    ln.commission = 0;
                    ln.commissionBalance = 0;
                    ln.securityDeposit = securityValue == null ? 0 : securityValue.Value;

                    if (cboLoanScheme.SelectedValue != "")
                    {
                        ln.loanSchemeId = int.Parse(cboLoanScheme.SelectedValue);
                    }

                    var lf = ln.loanFees.FirstOrDefault();
                    if (lf == null && txtProcFee.Value.Value > 0)
                    {
                    }
                    else if (lf != null)
                    {
                        lf.last_modifier = User.Identity.Name;
                        lf.modification_date = DateTime.Now;
                        lf.feeAmount = txtProcFee.Value.Value;
                    }
                    ln.processingFee = txtProcFee.Value.Value;
                    if (ln.loanNo == null || ln.loanNo.Trim().Length == 0)
                    {
                        if (ent.comp_prof.FirstOrDefault().traditionalLoanNo == true && client != null)
                        {
                            var pastLoans = le.loans.Where(p => p.clientID == client.clientID &&
                                p.loanID != ln.loanID).Count();
                            ln.loanNo = idGen.NewLoanNumber(client.branchID.Value,
                                client.clientID, ln.loanID,
                                cboLoanType.Text.Substring(0, 1).ToUpper());
                        }
                        else
                        {
                            ln.loanNo = idGen.NewLoanNumber(client.branchID.Value,
                                client.clientID, ln.loanID,
                                cboLoanType.Text.Substring(0, 1).ToUpper());
                        }
                    }
                    if (ln.loanStatusID <= 0) ln.loanStatusID = 1;
                    ln.loanTenure = txtTenure.Value.Value;
                    ln.loanTypeID = int.Parse(cboLoanType.SelectedValue);
                    ln.repaymentModeID = int.Parse(cboRepaymentMode.SelectedValue);
                    ln.tenureTypeID = 1;
                    ln.creditOfficerNotes = txtCNotes.Text;
                    ln.approvalComments = "";

                    if (ln.loanID == 0)
                    {
                        try
                        {
                            le.loans.Add(ln);
                        }
                        catch (Exception) { }
                    }
                    if (ln.loanStatusID == 0 || ln.loanStatusID == 1)
                    {
                        var sch = le.repaymentSchedules.Where(p => p.loanID == ln.loanID).ToList();
                        for (int i = sch.Count - 1; i >= 0; i--)
                        {
                            le.repaymentSchedules.Remove(sch[i]);
                        }

                        List<coreLogic.repaymentSchedule> sched;

                        using (core_dbEntities ctx = new core_dbEntities())
                        {
                            var comp = ctx.comp_prof.FirstOrDefault();
                            if (comp.comp_name.ToLower().Contains("ttl"))
                            {
                                sched =
                                   schMgr.calculateScheduleTTL(txtAmountRequested.Value.Value, txtRate.Value.Value,
                                   dtAppDate.SelectedDate.Value, (int?)txtGracePeriod.Value, (int)txtTenure.Value.Value,
                                   int.Parse(cboInterestType.SelectedValue), int.Parse(cboRepaymentMode.SelectedValue), ln);
                            }
                            else
                            {
                                sched =
                                    schMgr.calculateSchedule(txtAmountRequested.Value.Value, txtRate.Value.Value,
                                    dtAppDate.SelectedDate.Value, (int?)txtGracePeriod.Value, (int)txtTenure.Value.Value,
                                    int.Parse(cboInterestType.SelectedValue), int.Parse(cboRepaymentMode.SelectedValue), client);
                            }
                            foreach (var rs in sched)
                            {
                                rs.creation_date = DateTime.Now;
                                rs.creator = User.Identity.Name;
                                ln.repaymentSchedules.Add(rs);
                            }
                        }
                    }
                }
                else if (ln.processingFee == 0 && txtProcFee.Value != null && txtProcFee.Value.Value > 0)
                {
                    var lf = ln.loanFees.FirstOrDefault();
                    if (lf == null && txtProcFee.Value.Value > 0)
                    {
                        lf = new coreLogic.loanFee
                        {
                            feeAmount = txtProcFee.Value.Value,
                            feeDate = dtAppDate.SelectedDate.Value,
                            feeTypeID = 1,
                            creation_date = DateTime.Now,
                            creator = User.Identity.Name
                        };
                        ln.loanFees.Add(lf);
                    }
                    else if (lf != null)
                    {
                        lf.last_modifier = User.Identity.Name;
                        lf.modification_date = DateTime.Now;
                        lf.feeAmount = txtProcFee.Value.Value;
                    }
                    ln.processingFee = txtProcFee.Value.Value;
                    ln.processingFeeBalance += txtProcFee.Value.Value;


                    var pro = ent.comp_prof.FirstOrDefault();
                    var jb2 = journalextensions.Post("LN", ln.loanType.accountsReceivableAccountID,
                        ln.loanType.unpaidCommissionAccountID, ln.processingFee,
                        "Loan Disbursement Fees- " + ln.client.surName + "," + ln.client.otherNames,
                        pro.currency_id.Value, ln.disbursementDate.Value, ln.loanNo, ent, User.Identity.Name, ln.client.branchID);
                    ent.jnl_batch.Add(jb2);
                }
                foreach (var i in collaterals)
                {
                    try
                    {
                        if (!ln.loanCollaterals.Contains(i))
                        {
                            ln.loanCollaterals.Add(i);
                        }
                    }
                    catch (Exception) { }
                }
                for (int i = ln.loanCollaterals.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        var r = ln.loanCollaterals.ToList()[i];
                        if (collaterals.Contains(r) == false)
                        {
                            ln.loanCollaterals.Remove(r);
                        }
                    }
                    catch (Exception) { }
                }
                foreach (var i in documents)
                {
                    try
                    {
                        if (!ln.loanDocuments.Contains(i))
                        {
                            ln.loanDocuments.Add(i);
                        }
                    }
                    catch (Exception) { }
                }
                for (int i = ln.loanDocuments.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        var r = ln.loanDocuments.ToList()[i];
                        if (documents.Contains(r) == false)
                        {
                            ln.loanDocuments.Remove(r);
                        }
                    }
                    catch (Exception) { }
                }
                foreach (var i in guarantors)
                {
                    try
                    {
                        if (!ln.loanGurantors.Contains(i))
                        {
                            ln.loanGurantors.Add(i);
                        }
                    }
                    catch (Exception) { }
                }
                for (int i = ln.loanGurantors.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        var r = ln.loanGurantors.ToList()[i];
                        if (guarantors.Contains(r) == false)
                        {
                            ln.loanGurantors.Remove(r);
                        }
                    }
                    catch (Exception) { }
                }
                foreach (var i in financials)
                {
                    try
                    {
                        if (!ln.loanFinancials.Contains(i))
                        {
                            ln.loanFinancials.Add(i);
                        }
                    }
                    catch (Exception) { }
                }
                for (int i = ln.loanFinancials.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        var r = ln.loanFinancials.ToList()[i];
                        if (financials.Contains(r) == false)
                        {
                            ln.loanFinancials.Remove(r);
                        }
                    }
                    catch (Exception) { }
                }

                if (cboStaff.SelectedValue != "")
                {
                    int staffID = int.Parse(cboStaff.SelectedValue);
                    ln.staffID = staffID;
                }
                if (cboAgent.SelectedValue != "")
                {
                    int agentID = int.Parse(cboAgent.SelectedValue);
                    ln.agentID = agentID;
                }
                if (categoryID == "5")
                {
                    if (ln.loanID > 0)
                    {
                        detail = ln.prLoanDetails.FirstOrDefault();
                    }
                    if (detail == null)
                    {
                        detail = new coreLogic.prLoanDetail();
                        ln.prLoanDetails.Add(detail);
                    }
                    detail.amd = txtAMD.Value.Value;
                    if (chkPreAudit.Checked == true && detail.amdPreAuditVerified == false)
                    {
                        detail.amdPreAuditVerified = chkPreAudit.Checked;
                        detail.amdPreAuditVerifiedBy = User.Identity.Name;
                    }
                    detail.authorityNoteNumber = txtAuthorityNoteNo.Text;
                    detail.grossSalary = txtGrossSalary.Value ?? 0;
                    detail.loanAdvanceNumber = txtLoanAdvanceNo.Text;
                    if (cboLoanPurposeDetail.SelectedValue != "")
                    {
                        detail.loanPurposeDetailID = int.Parse(cboLoanPurposeDetail.SelectedValue);
                    }
                    if (cboLoanPurpose.SelectedValue != "")
                    {
                        detail.loanPurposeID = int.Parse(cboLoanPurpose.SelectedValue);
                    }
                    if (cboModeOfEntry.SelectedValue != "")
                    {
                        detail.modeOfEntryID = int.Parse(cboModeOfEntry.SelectedValue);
                    }
                    detail.loanProductID = int.Parse(cboLoanProduct.SelectedValue);
                    detail.netSalary = txtNetSalary.Value.Value;
                    detail.socialSecWelfare = txtSSWelfare.Value.Value;
                    detail.tax = txtTax.Value.Value;
                    detail.totalDeductions = txtTotalDeductions.Value.Value;
                    detail.loanDeductionsNotOnPr = txtTotalDedNotOnPR.Value.Value;
                    detail.basicSalary = txtBasicSalary.Value.Value;

                    foreach (var i in allowances)
                    {
                        if (i.prAllowanceID == 0)
                        {
                            try
                            {
                                detail.prAllowances.Add(i);
                            }
                            catch (Exception) { }
                        }
                    }
                    for (int i = detail.prAllowances.Count - 1; i >= 0; i--)
                    {
                        var r = detail.prAllowances.ToList()[i];
                        if (allowances.Contains(r) == false)
                        {
                            detail.prAllowances.Remove(r);
                        }
                    }
                }
                le.SaveChanges();
                ent.SaveChanges();

                Session["loanGuarantors"] = guarantors;
                Session["loanCollaterals"] = collaterals;
                Session["loan.cl"] = ln.client;
                Session["loan"] = ln;
                Session["id"] = ln.loanID;

                return true;
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
            if (cboFinancialType.SelectedValue != "" && txtRevenue.Text != "" && txtExpenses.Text != ""
                && cboFrequency.SelectedValue != "")
            {
                coreLogic.loanFinancial g;
                if (btnAddFinancial.Text == "Add Financials")
                {
                    g = new coreLogic.loanFinancial();
                }
                else
                {
                    g = Session["loanFinancial"] as coreLogic.loanFinancial;
                }
                int id = int.Parse(cboFinancialType.SelectedValue);
                g.financialTypeID = id;// le.financialTypes.FirstOrDefault(p => p.financialTypeID == id);
                g.revenue = txtRevenue.Value.Value;
                g.expenses = txtExpenses.Value.Value;
                g.otherCosts = txtOtherCosts.Value.Value;
                g.frequencyID = int.Parse(cboFrequency.SelectedValue);

                if (btnAddFinancial.Text == "Add Financials")
                {
                    financials.Add(g);
                }
                Session["loanFinancials"] = financials;
                gridFinancials.DataSource = financials;
                gridFinancials.DataBind();

                txtRevenue.Value = null;
                txtExpenses.Value = null;
                txtOtherCosts.Value = null;
                cboFinancialType.SelectedValue = "";
                cboFrequency.SelectedValue = "";

                btnAddFinancial.Text = "Add Financials";
            }
        }

        protected void btnAddAllowance_Click(object sender, EventArgs e)
        {
            if (cboAllowanceType.SelectedValue != "" && txtAllowanceAmount.Text != "")
            {
                if (allowances == null)
                {
                    allowances = new List<coreLogic.prAllowance>();
                }
                coreLogic.prAllowance g;
                if (btnAddAllowance.Text == "Add Allowance")
                {
                    g = new coreLogic.prAllowance();
                }
                else
                {
                    g = Session["allowance"] as coreLogic.prAllowance;
                }
                int id = int.Parse(cboAllowanceType.SelectedValue);
                g.allowanceTypeID = id;
                g.amount = txtAllowanceAmount.Value.Value;

                if (btnAddAllowance.Text == "Add Allowance")
                {
                    allowances.Add(g);
                }
                Session["allowances"] = allowances;
                gridAllowances.DataSource = allowances;
                gridAllowances.DataBind();

                txtAllowanceAmount.Value = null;
                cboAllowanceType.SelectedValue = "";

                btnAddAllowance.Text = "Add Allowance";
                txt_TextChanged(this, EventArgs.Empty);
            }
        }

        protected void gridFinancials_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = financials[e.Item.ItemIndex];
                if (g != null)
                {
                    txtRevenue.Value = g.revenue;
                    txtExpenses.Value = g.expenses;
                    cboFinancialType.SelectedValue = g.financialType.financialTypeID.ToString();
                    txtOtherCosts.Value = g.otherCosts;
                    if (g.frequencyID != null) cboFrequency.SelectedValue = g.frequencyID.Value.ToString();

                    Session["loanFinancial"] = g;
                    btnAddFinancial.Text = "Update Financials";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var fin = financials[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.loanFinancials.FirstOrDefault(p => p.loanFinancialID == fin.loanFinancialID);
                        if (toBeDeleted != null)
                        {
                            lent.loanFinancials.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                financials.RemoveAt(e.Item.ItemIndex);
                Session["loanFinancials"] = financials;
                txt_TextChanged(this, EventArgs.Empty);
            }
            gridFinancials.DataSource = financials;
            gridFinancials.DataBind();
        }

        protected void gridAllowances_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = allowances[e.Item.ItemIndex];
                if (g != null)
                {
                    txtAllowanceAmount.Value = g.amount;
                    cboAllowanceType.SelectedValue = g.prAllowanceType.allowanceTypeID.ToString();

                    Session["allowance"] = g;
                    btnAddAllowance.Text = "Update Allowance";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var all = allowances[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted = lent.prAllowances.FirstOrDefault(p => p.prAllowanceID == all.prAllowanceID);
                        if (toBeDeleted != null)
                        {
                            lent.prAllowances.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }
                Session["allowances"] = allowances;
                allowances.RemoveAt(e.Item.ItemIndex);
            }
            gridAllowances.DataSource = allowances;
            gridAllowances.DataBind();
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
            if (dataItem.ItemIndex < ln.loanCollaterals.Count)
            {
                e.DetailTableView.DataSource = ln.loanCollaterals.ToList()[dataItem.ItemIndex].collateralImages;
            }
        }

        protected void btnAddDcoument_Click(object sender, EventArgs e)
        {
            if (txtDocDesc.Text != "")
            {
                if (upload3.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile item in upload3.UploadedFiles)
                    {
                        byte[] b = new byte[item.InputStream.Length];
                        item.InputStream.Read(b, 0, b.Length);


                        var i = new coreLogic.document
                        {
                            description = txtDocDesc.Text,
                            documentImage = b,
                            contentType = item.ContentType,
                            fileName = item.FileName
                        };
                        var g = new coreLogic.loanDocument
                        {
                            document = i
                        };
                        documents.Add(g);
                    }
                }
                Session["loanDocuments"] = documents;
                gridDocument.DataSource = documents;
                gridDocument.DataBind();

                txtDocDesc.Text = "";
                btnAddDcoument.Text = "Add Document";
            }
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
                    if (t != null && t.isPermanent == true)
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
            net = txtGrossSalary.Value.Value;
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
            if (cboLoanType.SelectedValue != "")
            {
                int loanTypeId = int.Parse(cboLoanType.SelectedValue);
                var sch = le.loanSchemes.Where(p => p.loanTypeId == loanTypeId)
                            .OrderBy(p => p.loanSchemeName);
                if (ent.comp_prof.First().comp_name.ToLower().Contains("ttl") || ent.comp_prof.First().comp_name.ToLower().Contains("lendzee"))
                {
                    var lnTyp = le.loanTypes.FirstOrDefault(p => p.loanTypeID == loanTypeId)
                        .loanTypeName;
                    if (lnTyp.ToLower().Contains("group loan"))
                    {
                        txtTenure.Value = 4;
                        txtTenure.ReadOnly = false;
                        cboRepaymentMode.SelectedValue = "7";
                        cboRepaymentMode.Enabled = false;
                    }
                    else
                    {
                        txtTenure.Value = null;
                        txtTenure.ReadOnly = false;
                        cboRepaymentMode.SelectedValue = null;
                        cboRepaymentMode.Enabled = true;
                        txtRate.Value = null;
                    }
                    if (txtAmountRequested.Value > 0 && client.clientID > 0)
                    {
                        loanType groupLoan = le.loanTypes.FirstOrDefault(p => p.loanTypeName.ToLower().Contains("group loan"));
                        var comp = ent.comp_prof.FirstOrDefault();
                        if (lnTyp.ToLower().Contains("group loan") && comp != null
                            && (comp.comp_name.ToLower().Contains("ttl")))
                        {
                            int clientId = client.clientID;
                            var clientLoans = le.loans.Where(p => p.clientID == clientId
                                                                  && p.loanTypeID == groupLoan.loanTypeID &&
                                                                  p.loanStatusID == 4).ToList();
                            if (clientLoans.Count == 0)
                            {
                                txtSecurityDeposit.Value = txtAmountRequested.Value * (0.10);
                            }
                            else if (clientLoans.Count == 1)
                            {
                                txtSecurityDeposit.Value = txtAmountRequested.Value * (0.15);
                            }
                            else if (clientLoans.Count >= 2)
                            {
                                txtSecurityDeposit.Value = txtAmountRequested.Value * (0.25);
                            }
                        }
                        else
                        {
                            txtSecurityDeposit.Value = 0;
                            txtSecurityDeposit.Enabled = false;
                        }


                    }
                }
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
            OnChange();
        }

        private void OnChange()
        {

            if (cboLoanScheme.SelectedValue == "" && txtTenure.Value != null && categoryID != "5" && cboLoanType.SelectedValue != "")
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
                var lt = le.tenors.FirstOrDefault(p => p.tenor1 == tenure2 && p.loanTypeID == id);
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
                    if (lt.defaultApplicationFeeRate != null)
                    {
                        //if (ent.comp_prof.First().comp_name.ToLower().Contains("lendzee"))
                        //{
                        //    txtApplicationFee.Value = amount * lt.defaultProcessingFeeRate.Value / 100.0;
                        //}
                    }
                    if (lt.defaultInterestRate != null)
                    {
                        txtRate.Value = lt.defaultInterestRate;
                    }
                    if (lt.defaultRepaymentModeID != null)
                    {
                        //int loanTypeId = int.Parse(cboLoanType.SelectedValue);
                        //var lnTyp = le.loanTypes.FirstOrDefault(p => p.loanTypeID == loanTypeId)
                        //.loanTypeName;
                        //if (!lnTyp.ToLower().Contains("group loan"))
                        //    cboRepaymentMode.SelectedValue = lt.defaultRepaymentModeID.ToString();
                    }
                    if (lt.defaultGracePeriod != null)
                    {
                        txtGracePeriod.Value = lt.defaultGracePeriod;
                    }
                    var grpLnInSt = le.insuranceSetups.FirstOrDefault(p => p.loanTypeID == 10);
                    if ((ent.comp_prof.FirstOrDefault().deductInsurance == true)
                        && cboLoanType.SelectedValue != ""
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
                    else if (grpLnInSt != null && grpLnInSt.insurancePercent > 0)
                    {
                        txtInsurance.Value = grpLnInSt.insurancePercent * amount / 100.0;
                        txtInsurance.Visible = true;
                        trInsurance.Visible = true;
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
                            foreach (var r in g)
                            {
                                if (r.loanCollateralID <= 0)
                                {
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
                            foreach (var r in g)
                            {
                                if (r.loanFinancialID <= 0)
                                {
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
                            foreach (var r in g)
                            {
                                if (r.loanCheckID <= 0)
                                {
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
                            foreach (var r in g)
                            {
                                if (r.loanDocumentID <= 0)
                                {
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
                                foreach (var r in g)
                                {
                                    if (r.prAllowanceID <= 0)
                                    {
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

                int loanTypeId = int.Parse(cboLoanType.SelectedValue);
                var lnTyp = le.loanTypes.FirstOrDefault(p => p.loanTypeID == loanTypeId)
                        .loanTypeName;
                if (loanSchemeId != null && sch != null)
                {
                    txtRate.Value = sch.rate;
                    txtTenure.Value = sch.tenure;
                    if (!lnTyp.ToLower().Contains("group loan")) cboRepaymentMode.SelectedValue = "30";

                    txtRate.Enabled = false;
                    txtTenure.Enabled = false;
                    cboRepaymentMode.Enabled = false;
                }
            }
        }

        private bool IsOwnerOrAdmin(string userName)
        {
            try
            {
                var secEnt = new coreSecurityEntities();
                var userRoles = secEnt.user_roles
                    .Include(r => r.users)
                    .Include(w => w.roles)
                    .Where(p => (p.roles.role_name.Trim().ToLower() == "owner" || p.roles.role_name.Trim().ToLower() == "admin") && p.users.user_name.Trim().ToLower() == userName.ToLower()).ToList();
                if (userRoles != null && userRoles.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}