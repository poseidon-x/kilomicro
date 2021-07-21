using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;
using System.Data.SqlClient;

namespace coreERP.ln.saving
{
    public partial class saving : System.Web.UI.Page
    {
        private coreLogic.coreLoansEntities le;
        private coreLogic.core_dbEntities ent;
        private coreLogic.client client;
        private coreLogic.saving dp;
        private List<coreLogic.savingSchedule> sched = new List<coreLogic.savingSchedule>();
        private List<coreLogic.savingSignatory> signatories = new List<coreLogic.savingSignatory>();
        private IIDGenerator idGen;
        private   List<savingNextOfKin> nextOfKins=new List<savingNextOfKin>();

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            idGen = new IDGenerator();
            var prof = ent.comp_prof.First();

            if (!IsPostBack)
            {
                Session["id"] = null;
                Session["signatories"] = null;
                Session["savingSchedules"] = null;
                Session["nextOfKins"] = null;
                cboSavingsType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.savingTypes)
                {
                    cboSavingsType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.savingTypeName,
                        r.savingTypeID.ToString()));
                }

                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs.OrderBy(p => p.surName).ThenBy(p => p.otherNames))
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

                this.cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Monthly", "30"));
                cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Quarterly", "90"));
                cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Half-Yearly", "180"));
                cboPrincRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("At Maturity", "-1"));

                this.cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Monthly", "30"));
                cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Quarterly", "90"));
                cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Half-Yearly", "180"));
                cboInterestRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("At Maturity", "-1"));

                cboSavingPlan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("At Random", "0"));
                cboSavingPlan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Daily - 5 Days a Week", "5"));
                cboSavingPlan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Daily - 6 Days a Week", "6"));
                cboSavingPlan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Weekly", "7"));
                cboSavingPlan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Monthly", "30"));
                cboSavingPlan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Quarterly", "91"));
                cboSavingPlan.Items.Add(new Telerik.Web.UI.RadComboBoxItem("Half-Yearly", "182"));

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    dp = le.savings.FirstOrDefault(p => p.savingID == id);

                    if (dp != null)
                    {
                        signatories = dp.savingSignatories.ToList();
                        client = dp.client;
                        Session["loan.cl"] = client;

                        sched = dp.savingSchedules.Where(p => p.temp == false).ToList();
                        Session["savingSchedules"] = sched;
                        signatories = dp.savingSignatories.ToList();
                        Session["signatories"] = signatories;
                        nextOfKins = dp.savingNextOfKins.ToList();
                        Session["nextOfKins"] = nextOfKins;

                        txtInterestRate.Value = dp.interestRate;
                        txtRateA.Value = dp.interestRate*12;

                        var ssa = le.staffSavings.FirstOrDefault(p => p.savingID == id);
                        if (ssa != null)
                        {
                            txtIntBalance.Visible = false;
                            txtPrincBal.Visible = false;
                        }
                        else
                        {
                            txtIntBalance.Value = dp.interestBalance;
                            txtPrincBal.Value = dp.principalBalance;
                        }

                        cboSavingsType.SelectedValue = dp.savingType.savingTypeID.ToString();
                        dtAppDate.SelectedDate = dp.firstSavingDate;
                        if (
                            (new coreSecurityEntities()).users.First(p => p.user_name == User.Identity.Name)
                                .accessLevelID < 39)
                        {
                            dtAppDate.Enabled = false;
                        }
                        if (dp.staffID != null)
                        {
                            cboStaff.SelectedValue = dp.staffID.ToString();
                        }
                        if (dp.agentId != null)
                        {
                            cboAgent.SelectedValue = dp.agentId.ToString();
                        }
                        cboClient.Items.Clear();
                        cboClient.Items.Add(new RadComboBoxItem(
                            (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5)
                                ? client.companyName
                                : client.surName +
                                  ", " + client.otherNames + " (" + client.accountNumber + ")",
                            client.clientID.ToString()));
                        cboClient.SelectedIndex = 0;
                        if (dp.client.clientTypeID == 6)
                        {
                            pnlJoint.Visible = true;
                            pnlRegular.Visible = false;
                            txtJointAccountName.Text = dp.client.accountName;
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
                        txtAccountNo.Text = client.accountNumber;
                        cboPrincRepaymentMode.SelectedValue = dp.principalRepaymentModeID.ToString();
                        cboInterestRepaymentMode.SelectedValue = dp.interestRepaymentModeID.ToString();
                        cboSavingPlan.SelectedValue = dp.savingPlanID.ToString();
                        txtPlanAmount.Value = dp.savingPlanAmount;

                        if (ssa == null || User.IsInRole("admin"))
                        {
                            gridDep.DataSource = dp.savingAdditionals;
                            gridDep.DataBind();
                            using (var rent = new coreReports.reportEntities())
                            {
                                var res = rent.vwSavingStatements
                                    .Where(p => p.loanID == dp.savingID)
                                    .ToList();
                                var sis = dp.savingInterests
                                    .OrderBy(p => p.interestDate)
                                    .ToList()
                                    .Select(p => new
                                    {
                                        p.creator,
                                        p.fxRate,
                                        p.interestAmount,
                                        p.interestBalance,
                                        p.interestDate,
                                        p.interestRate,
                                        p.last_modifier,
                                        p.lastInterestFxGainLoss,
                                        p.localAmount,
                                        p.modification_date,
                                        p.principal,
                                        p.proposedAmount,
                                        p.saving,
                                        p.savingID,
                                        p.savingInterestID,
                                        p.toDate,
                                        deposited = GetCumDep(p, res),
                                        principalBalance = GetPrincBal(p, res),
                                    });

                                gridInt.DataSource = sis;
                                gridInt.DataBind();
                            }
                            gridWith.DataSource = dp.savingWithdrawals;
                            gridWith.DataBind();
                            gridSchedule.DataSource =
                                dp.savingSchedules.Where(p => p.temp == false).OrderBy(p => p.repaymentDate);
                            gridSchedule.DataBind();
                        }

                        gridGuarantor.DataSource = signatories;
                        gridGuarantor.DataBind();

                        gridPlan.DataSource = dp.savingPlans;
                        gridPlan.DataBind();
                    }

                    Session["saving"] = dp;
                }
                else
                {
                    dp = new coreLogic.saving();
                    Session["saving"] = dp;

                    sched = new List<coreLogic.savingSchedule>();
                    Session["savingSchedules"] = sched;

                    cboInterestRepaymentMode.SelectedValue = "-1";
                    cboPrincRepaymentMode.SelectedValue = "-1";
                    cboSavingPlan.SelectedValue = "0";
                    txtPlanAmount.Value = 0;
                }

                if (ent.comp_prof.First().comp_name.ToLower().Contains("link") == true)
                {
                    txtInterestRate.ReadOnly = true;
                    txtInterestRate.Enabled = false;
                    txtRateA.ReadOnly = true;
                    txtRateA.Enabled = false;
                }
                gridNextOfKin.DataSource = dp.savingNextOfKins;
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadSaving(id);
            }
        }


        protected void cboClient_SelectedIndexChanged(object sender,
            Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
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
                    //client.clientImages.Load();
                    foreach (var r in client.clientImages)
                    {
                        //r.imageReference.Load();
                    }
                    if (client.clientTypeID == 6)
                    {
                        pnlJoint.Visible = true;
                        pnlRegular.Visible = false;
                        txtJointAccountName.Text = client.accountName;
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

                    RenderImages();
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
            List<coreLogic.client> clients = null;
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 &&
                txtAccountNo.Text.Trim().Length > 0)
                clients =
                    le.clients.Where(
                        p =>
                            p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim()))
                        .Where(
                            p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                                p => p.accountNumber.Contains(txtAccountNo.Text.Trim()))
                        .Where(p => p.clientTypeID == 1 || p.clientTypeID == 2)
                        .ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 &&
                     txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim()))
                    .Where(
                        p =>
                            p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 || p.clientTypeID == 6)
                    .ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 &&
                     txtAccountNo.Text.Trim().Length > 0)
                clients =
                    le.clients.Where(
                        p =>
                            p.surName.Contains(txtSurname.Text.Trim()) ||
                            p.accountName.Contains(txtSurname.Text.Trim())).Where(
                                p => p.accountNumber.Contains(txtAccountNo.Text.Trim()))
                        .Where(
                            p =>
                                p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 ||
                                p.clientTypeID == 6)
                        .ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 &&
                     txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p =>
                        p.surName.Contains(txtSurname.Text.Trim()) ||
                        p.accountName.Contains(txtSurname.Text.Trim()))
                    .Where(
                        p =>
                            p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 ||
                            p.clientTypeID == 6)
                    .ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 &&
                     txtAccountNo.Text.Trim().Length > 0)
                clients =
                    le.clients.Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim()))
                        .Where(
                            p =>
                                p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 ||
                                p.clientTypeID == 6)
                        .ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 &&
                     txtAccountNo.Text.Trim().Length == 0)
                clients =
                    le.clients.Where(
                        p =>
                            p.surName.Contains(txtSurname.Text.Trim()) ||
                            p.accountName.Contains(txtSurname.Text.Trim()))
                        .Where(
                            p =>
                                p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 ||
                                p.clientTypeID == 6)
                        .ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 &&
                     txtAccountNo.Text.Trim().Length == 0)
                clients =
                    le.clients.Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim()))
                        .Where(
                            p =>
                                p.clientTypeID == 1 || p.clientTypeID == 2 ||
                                p.clientTypeID == 5 || p.clientTypeID == 6)
                        .ToList();
            else
                clients =
                    le.clients.Where(
                        p =>
                            p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 5 ||
                            p.clientTypeID == 6).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(
                    new Telerik.Web.UI.RadComboBoxItem(
                        (item.clientTypeID == 3 || item.clientTypeID == 4 || item.clientTypeID == 5)
                            ? item.companyName
                            : ((item.clientTypeID == 6) ? item.accountName : item.surName + ", " + item.otherNames) +
                              " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (
                txtInterestRate.Value != null
                && cboSavingsType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && txtPlanAmount.Value != null
                && cboPrincRepaymentMode.SelectedValue != ""
                && cboInterestRepaymentMode.SelectedValue != ""
                )
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                dp.amountInvested = 0.0;
                dp.firstSavingDate = dtAppDate.SelectedDate.Value;
                int depTypeID = int.Parse(cboSavingsType.SelectedValue);
                var prof = ent.comp_prof.First();
                var dt = le.savingTypes.First(p => p.savingTypeID == depTypeID);
                if (prof.comp_name.ToLower().Contains("link"))
                {
                    if (dt.maxPlanAmount < txtPlanAmount.Value.Value || dt.minPlanAmount > txtPlanAmount.Value.Value)
                    {
                        HtmlHelper.MessageBox(
                            "Planned Deposit Amount is outside the allowed range for the selected product:" +
                            dt.savingTypeName + "!",
                            "coreERP©: Invalid Input", IconType.deny);
                        return;
                    }
                    if (cboStaff.SelectedValue == null || cboStaff.SelectedValue == "")
                    {
                        HtmlHelper.MessageBox("You must select a relationship officer before proceeding!",
                            "coreERP©: Invalid Input", IconType.deny);
                        return;
                    }
                }

                if (dp.savingID <= 0)
                {
                    if (client != null) dp.clientID = client.clientID;
                    else
                    {
                        dp.clientID = int.Parse(cboClient.SelectedValue);
                        client = le.clients.FirstOrDefault(p => p.clientID == dp.clientID);
                    }
                    var savingClient = le.savings.FirstOrDefault(r => r.clientID == client.clientID);
                    if (savingClient != null)
                    {
                        HtmlHelper.MessageBox("This client already have a savings Account!",
                            "coreERP©: Saving Duplication", IconType.deny);
                        return;
                    }
                    dp.creation_date = DateTime.Now;
                    dp.creator = User.Identity.Name;
                    dp.period = dt.defaultPeriod;
                    dp.autoRollover = false;
                    dp.availableInterestBalance = 0;
                    dp.availablePrincipalBalance = 0;
                    dp.clearedInterestBalance = 0;
                    dp.clearedPrincipalBalance = 0;
                    dp.currencyID = 1;
                    dp.firstSavingDate = dtAppDate.SelectedDate.Value;
                    dp.fxRate = 1;
                    dp.interestAccumulated = 0;
                    dp.interestAuthorized = 0;
                    dp.interestBalance = 0;
                    dp.interestExpected = 0;
                    dp.localAmount = 0;
                    dp.maturityDate = dtAppDate.SelectedDate.Value.AddMonths(dt.defaultPeriod);
                    dp.principalAuthorized = 0;
                    dp.principalBalance = 0;
                    dp.status = "A";
                }
                else
                {
                    if(txtPlanAmount.Value != null)
                    {
                        dp.savingPlanAmount = txtPlanAmount.Value.Value;

                    }

                }
                dp.interestRate = txtInterestRate.Value.Value;
                dp.savingTypeID = depTypeID;
                dp.interestMethod = true;
                dp.principalRepaymentModeID = int.Parse(cboPrincRepaymentMode.SelectedValue);
                dp.interestRepaymentModeID = int.Parse(cboInterestRepaymentMode.SelectedValue);
                if (cboStaff.SelectedValue != "")
                {
                    dp.staffID = int.Parse(cboStaff.SelectedValue);
                }
                if (cboAgent.SelectedValue != "")
                {
                    dp.agentId = int.Parse(cboAgent.SelectedValue);
                }
                dp.currencyID = prof.currency_id.Value;
                dp.fxRate = 1;
                dp.localAmount = 0.0;

                if (dp.savingNo == null || dp.savingNo.Trim().Length == 0)
                {
                    if (prof.traditionalLoanNo == true)
                    {
                        var cl = le.clients.FirstOrDefault(p => p.clientID == client.clientID);
                        //cl.savings.Load();
                        var cnt = cl.savings.Where(p => p.savingID != dp.savingID).Count();
                        char c = (char) (((int) 'A') + cnt);
                        dp.savingNo = idGen.NewSavingsNumber(client.branchID.Value,
                            client.clientID, dp.savingID,
                            cboSavingsType.Text.Substring(0, 2));
                    }
                    else
                    {
                        dp.savingNo = idGen.NewSavingsNumber(client.branchID.Value,
                            client.clientID, dp.savingID,
                            cboSavingsType.Text.Substring(0, 2).ToUpper());
                    }
                }
                if (dp.savingID <= 0) le.savings.Add(dp);

                var ss = dp.savingSchedules.ToList();
                for (int i = ss.Count - 1; i >= 0; i--)
                {
                    var s = ss[i];
                    if (!sched.Contains(s) && s.expensed == false)
                    {
                        le.savingSchedules.Remove(s);
                    }
                }
                foreach (var i in sched)
                {
                    if (!dp.savingSchedules.Contains(i))
                    {
                        dp.savingSchedules.Add(i);
                    }
                }

                foreach (var i in signatories)
                {
                    if (!dp.savingSignatories.Contains(i))
                    {
                        dp.savingSignatories.Add(i);
                    }
                }
                for (int i = dp.savingSignatories.Count - 1; i >= 0; i--)
                {
                    var r = dp.savingSignatories.ToList()[i];
                    if (signatories.Contains(r) == false)
                    {
                        dp.savingSignatories.Remove(r);
                    }
                }

                foreach (var i in nextOfKins)
                {
                    var existingNok = dp.savingNextOfKins.FirstOrDefault(p => p.savingNextOfKinId == i.savingNextOfKinId);
                    if (existingNok == null)
                    {
                        dp.savingNextOfKins.Add(i);
                    }
                    else
                    {
                        existingNok.surName = i.surName;
                        existingNok.phoneNumber = i.phoneNumber;
                        existingNok.idTypeId = i.idTypeId;
                        existingNok.idNumber = i.idNumber;
                        existingNok.otherNames = i.otherNames;
                        existingNok.percentageAllocated = i.percentageAllocated;
                        existingNok.relationshipType = i.relationshipType;
                        existingNok.savingId = i.savingId;
                        existingNok.dob = i.dob;

                    }
                }
                for (int i = dp.savingNextOfKins.Count - 1; i >= 0; i--)
                {
                    var r = dp.savingNextOfKins.ToList()[i];
                    if (nextOfKins.Any(p=>p.savingNextOfKinId== r.savingNextOfKinId) == false)
                    {
                        dp.savingNextOfKins.Remove(r);
                    }
                }

                if (dp.savingID <= 0)
                {
                    dp.principalBalance = 0.0;
                    dp.period = le.savingTypes.First(p => p.savingTypeID == depTypeID).defaultPeriod;
                    dp.maturityDate = dtAppDate.SelectedDate.Value.AddMonths(dp.period);
                    dp.interestExpected = 0;
                }

                if (dp.savingID <= 0 || dp.savingPlanID.ToString() != cboSavingPlan.SelectedValue)
                {
                    var dep = dp.savingPlans.Where(p => p.deposited == false).ToList();
                    for (int i = dep.Count - 1; i >= 0; i--)
                    {
                        var d = dep[i];
                        le.savingPlans.Remove(d);
                    }

                    dp.savingPlanID = int.Parse(cboSavingPlan.SelectedValue);
                    dp.savingPlanAmount = txtPlanAmount.Value.Value;
                    var date = dp.firstSavingDate;
                    var endDate = date.AddMonths(dp.period);
                    while (date < endDate && dp.savingPlanID > 0)
                    {
                        dp.savingPlans.Add(new coreLogic.savingPlan
                        {
                            plannedAmount = txtPlanAmount.Value.Value,
                            plannedDate = date,
                            deposited = false,
                            creator = User.Identity.Name,
                            creationDate = DateTime.Now,
                            amountDeposited = 0,
                        });
                        if (dp.savingPlanID == 5 || dp.savingPlanID == 6)
                        {
                            date = date.AddDays(1);
                        }
                        else if (dp.savingPlanID == 7)
                        {
                            date = date.AddDays(7);
                        }
                        else if (dp.savingPlanID == 30)
                        {
                            date = date.AddMonths(1);
                        }
                        else if (dp.savingPlanID == 90)
                        {
                            date = date.AddMonths(3);
                        }
                        else if (dp.savingPlanID == 180)
                        {
                            date = date.AddMonths(6);
                        }
                        else
                        {
                            break;
                        }
                        if (dp.savingPlanID != 6 && date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            date = date.AddDays(1);
                        }
                        if (date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            date = date.AddDays(1);
                        }

                    }
                }
                //SqlCommand cmd = new SqlCommand("select * from tb", con);
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //if (client = dp.clientID)
                le.SaveChanges();
                ent.SaveChanges();

                Session["loan.cl"] = null;
                Session["saving"] = null;
                HtmlHelper.MessageBox2("Regular Deposit Data Saved Successfully!",
                    ResolveUrl("~/ln/saving/default.aspx?id=" + dp.savingID.ToString()),
                    "coreERP©: Successful", IconType.ok);
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.",
                    "coreERP: Incomplete", IconType.warning);
            }
        }

        protected void cboSavingsType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboSavingsType.SelectedValue != "")
            {
                int id = int.Parse(cboSavingsType.SelectedValue);
                var depType = le.savingTypes.FirstOrDefault(p => p.savingTypeID == id);
                if (depType != null)
                {
                    txtInterestRate.Value = depType.interestRate;
                    txtRateA.Value = depType.interestRate*12;
                    if (depType.planID != null && depType.minPlanAmount != null && depType.maxPlanAmount != null)
                    {
                        cboSavingPlan.SelectedValue = depType.planID.ToString();
                        txtPlanAmount.Value = depType.minPlanAmount;
                    }
                }
            }
        }

        protected void btnAddSchedule_Click(object sender, EventArgs e)
        {
            if (dtRepaymentDate.SelectedDate != null && txtPrincipal.Value != null &&
                txtInterest.Value != null)
            {
                coreLogic.savingSchedule g;
                if (btnAddSchedule.Text == "Add Schedule")
                {
                    g = new coreLogic.savingSchedule();
                }
                else
                {
                    g = Session["savingSchedule"] as coreLogic.savingSchedule;
                }
                g.principalPayment = txtPrincipal.Value.Value;
                g.interestPayment = txtInterest.Value.Value;
                g.repaymentDate = dtRepaymentDate.SelectedDate.Value;
                g.temp = false;
                g.expensed = false;
                g.authorized = false;

                if (btnAddSchedule.Text == "Add Schedule")
                {
                    sched.Add(g);
                }
                else
                {
                    var s = sched.FirstOrDefault(p => p.savingScheduleID == g.savingScheduleID);
                    s.principalPayment = g.principalPayment;
                    s.interestPayment = g.interestPayment;
                    s.repaymentDate = g.repaymentDate;
                }
                Session["savingSchedules"] = sched;
                gridSchedule.DataSource = sched;
                gridSchedule.DataBind();

                txtPrincipal.Value = null;
                txtInterest.Value = null;
                dtRepaymentDate.SelectedDate = null;

                btnAddSchedule.Text = "Add Schedule";
            }
        }

        protected void gridSchedule_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = sched[e.Item.ItemIndex];
                if (g != null)
                {
                    txtPrincipal.Value = g.principalPayment;
                    txtInterest.Value = g.interestPayment;
                    dtRepaymentDate.SelectedDate = g.repaymentDate;

                    Session["savingSchedule"] = g;
                    btnAddSchedule.Text = "Update Schedule";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                sched.RemoveAt(e.Item.ItemIndex);
            }
            gridSchedule.DataSource = sched;
            gridSchedule.DataBind();
        }

        protected void txtPeriod_TextChanged(object sender, EventArgs e)
        {
            if (dtAppDate.SelectedDate != null && "0" != null)
            {
                if (ent.comp_prof.FirstOrDefault().comp_name.Contains("Link Exchange"))
                {
                }
                else
                {
                }
            }
        }

        protected void txtRateA_TextChanged(object sender, EventArgs e)
        {
            if (txtRateA.Value != null)
            {
                txtInterestRate.Value = Math.Round(txtRateA.Value.Value/12.0, 6);
            }
        }

        protected void txtInterestRate_TextChanged(object sender, EventArgs e)
        {
            if (txtInterestRate.Value != null)
            {
                txtRateA.Value = Math.Round(txtInterestRate.Value.Value*12.0, 6);
            }
        }

        protected void gridGuarantor_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = signatories[e.Item.ItemIndex];
                if (g != null)
                {
                    txtDocDesc.Text = g.fullName;

                    Session["signatory"] = g;
                    btnAddDcoument.Text = "Update Signatory";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    var sig = signatories[e.Item.ItemIndex];
                    using (var lent = new coreLoansEntities())
                    {
                        var toBeDeleted =
                            lent.savingSignatories.FirstOrDefault(p => p.savingSignatoryID == sig.savingSignatoryID);
                        if (toBeDeleted != null)
                        {
                            lent.savingSignatories.Remove(toBeDeleted);
                            lent.SaveChanges();
                        }
                    }
                }
                catch (Exception)
                {
                }
                signatories.RemoveAt(e.Item.ItemIndex);
                Session["signatories"] = signatories;
            }
            gridGuarantor.DataSource = signatories;
            gridGuarantor.DataBind();
        }

        protected void btnAddDcoument_Click(object sender, EventArgs e)
        {
            if (txtDocDesc.Text != "")
            {
                coreLogic.savingSignatory g;
                if (btnAddDcoument.Text == "Add Signatory")
                {
                    g = new coreLogic.savingSignatory();
                }
                else
                {
                    g = Session["signatory"] as coreLogic.savingSignatory;
                }
                g.fullName = txtDocDesc.Text;
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
                if (btnAddDcoument.Text == "Add Signatory")
                {
                    signatories.Add(g);
                }
                Session["signatories"] = signatories;
                gridGuarantor.DataSource = signatories;
                gridGuarantor.DataBind();

                txtDocDesc.Text = "";
                btnAddDcoument.Text = "Add Signatory";
            }
        }

        protected void cboClient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (
                    var cl in
                        le.clients.Where(
                            p =>
                                (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) ||
                                 p.companyName.Contains(e.Text)
                                 || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(
                        new Telerik.Web.UI.RadComboBoxItem(
                            (cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5)
                                ? cl.companyName
                                : ((cl.clientTypeID == 6)
                                    ? cl.accountName
                                    : cl.surName +
                                      ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        private void LoadSaving(int? id)
        {
            if (id != null)
            {
                dp = le.savings.FirstOrDefault(p => p.savingID == id);

                if (dp != null)
                {
                    signatories = dp.savingSignatories.ToList();
                    client = dp.client;
                    Session["loan.cl"] = client;

                    sched = dp.savingSchedules.Where(p => p.temp == false).ToList();
                    if (Session["savingSchedules"] != null)
                    {
                        var sch = Session["savingSchedules"] as List<coreLogic.savingSchedule>;
                        if (sch != null)
                        {
                            for (int i = sch.Count - 1; i >= 0; i--)
                            {
                                if (sch[i].savingScheduleID <= 0)
                                {
                                    sched.Add(sch[i]);
                                }
                            }
                        }
                    }
                    Session["savingSchedules"] = sched;
                    signatories = dp.savingSignatories.ToList();
                    if (Session["signatories"] != null)
                    {
                        var sch = Session["signatories"] as List<coreLogic.savingSignatory>;
                        if (sch != null)
                        {
                            for (int i = sch.Count - 1; i >= 0; i--)
                            {
                                if (sch[i].savingSignatoryID <= 0)
                                {
                                    signatories.Add(sch[i]);
                                }
                            }
                        }
                    }
                    Session["signatories"] = signatories;

                }

                Session["saving"] = dp;
            }
            else
            {
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                } 
                dp = new coreLogic.saving();
                Session["saving"] = dp;
                if (Session["savingSchedules"] != null)
                {
                    sched = Session["savingSchedules"] as List<coreLogic.savingSchedule>;
                }
                else
                {
                    sched = new List<coreLogic.savingSchedule>();
                    Session["savingSchedules"] = sched;
                }
                if (Session["signatories"] != null)
                {
                    signatories = Session["signatories"] as List<coreLogic.savingSignatory>;
                }
                else
                {
                    signatories = new List<coreLogic.savingSignatory>();
                    Session["signatories"] = signatories;
                }
            }
            if (Session["nextOfKins"] != null)
            {
                nextOfKins = Session["nextOfKins"] as List<coreLogic.savingNextOfKin>;
            }
            else
            {
                nextOfKins = new List<coreLogic.savingNextOfKin>();
                Session["nextOfKins"] = nextOfKins;
            }
            gridNextOfKin.DataSource = nextOfKins;
        }

        private double GetPrincBal(savingInterest si, List<coreReports.vwSavingStatement> res)
        {
            var res2 = res
                .Where(p => p.loanID == dp.savingID && p.date < si.interestDate)
                .ToList();
            if (res2.Count > 0)
            {
                return res2.Sum(p => p.depositAmount - p.princWithdrawalAmount);
            }

            return 0;
        }

        private double GetCumDep(savingInterest si, List<coreReports.vwSavingStatement> res)
        {
            var res2 = res
                .Where(p => p.loanID == dp.savingID && p.date < si.interestDate)
                .ToList();
            if (res2.Count > 0)
            {
                return res2.Sum(p => p.depositAmount);
            }

            return 0;
        }

        protected void RadGrid1_OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            var savingNextOfKinId = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["savingNextOfKinId"];
            var b = nextOfKins.First(p => p.savingNextOfKinId == savingNextOfKinId);
            nextOfKins.Remove(b);

            Session["nextOfKins"] = nextOfKins;
            gridNextOfKin.DataSource = nextOfKins;
        }


        protected void RadGrid1_ItemInserted(object source, Telerik.Web.UI.GridInsertedEventArgs e)
        {
            //throw e.Exception;
            gridNextOfKin.DataSource = nextOfKins;
        }

        protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                e.Canceled = true;
                var newVals = new System.Collections.Specialized.ListDictionary();
                newVals["surName"] = string.Empty;
                newVals["otherNames"] = string.Empty;
                newVals["relationshipType"] = string.Empty;
                newVals["idNumber"] = string.Empty;
                newVals["phoneNumber"] = string.Empty;
                newVals["idTypeId"] = 0;
                e.Item.OwnerTableView.InsertItem(newVals);
                gridNextOfKin.DataSource = nextOfKins;
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            Hashtable newVals = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem) e.Item);
            e.Canceled = true;
            var savingNextOfKinId = (int) e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["savingNextOfKinId"];
            var b = nextOfKins.First(p => p.savingNextOfKinId == savingNextOfKinId);
            b.surName = newVals["surName"].ToString();
            b.otherNames = newVals["otherNames"].ToString();
            b.relationshipType = newVals["relationshipType"].ToString();
            b.idNumber = newVals["idNumber"].ToString();
            b.phoneNumber = (newVals["phoneNumber"]==null)?"":newVals["phoneNumber"].ToString();
            b.idTypeId = int.Parse(newVals["idTypeId"].ToString());
            b.dob = DateTime.Parse(newVals["dob"].ToString());
            b.percentageAllocated = double.Parse(newVals["percentageAllocated"].ToString());
            Session["nextOfKins"] = nextOfKins;

            gridNextOfKin.EditIndexes.Clear();
            gridNextOfKin.MasterTableView.IsItemInserted = false;
            gridNextOfKin.MasterTableView.Rebind();
            gridNextOfKin.DataSource = nextOfKins;
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            Hashtable newVals = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem) e.Item);
            e.Canceled = true;
            savingNextOfKin b = new savingNextOfKin
            {
                surName = newVals["surName"].ToString(),
                otherNames = newVals["otherNames"].ToString(),
                relationshipType = newVals["relationshipType"].ToString(),
                idNumber = newVals["idNumber"].ToString(),
                idTypeId = int.Parse(newVals["idTypeId"].ToString()),
                dob = DateTime.Parse(newVals["dob"].ToString()),
                percentageAllocated = double.Parse( newVals["percentageAllocated"].ToString()),
                savingNextOfKinId = -(dp.savingNextOfKins.Count + 1),
                phoneNumber = (newVals["phoneNumber"] == null) ? "" : newVals["phoneNumber"].ToString()
            };
            nextOfKins.Add(b);
            Session["nextOfKins"] = nextOfKins;

            gridNextOfKin.EditIndexes.Clear();
            e.Item.OwnerTableView.IsItemInserted = false;
            e.Item.OwnerTableView.Rebind();
            gridNextOfKin.DataSource = nextOfKins;

        }
    }
}