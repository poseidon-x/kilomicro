using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.ln.investment
{
    public partial class investment : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.investment dp;
        List<coreLogic.investmentSchedule> sched = new List<coreLogic.investmentSchedule>();
        List<coreLogic.investmentSignatory> signatories = new List<coreLogic.investmentSignatory>();
        private IIDGenerator idGen;

        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new coreLogic.core_dbEntities();
            le = new coreLogic.coreLoansEntities();
            idGen = new IDGenerator();
            if (!IsPostBack)
            {
                Session["id"] = null;
                cboInvestmentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.investmentTypes)
                {
                    cboInvestmentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.investmentTypeName, r.investmentTypeID.ToString()));
                }

                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }
                 
                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
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

                cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.staffs.OrderBy(p => p.surName).ThenBy(p => p.otherNames))
                {
                    cboStaff.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " ("
                            + r.staffNo + ")", r.staffID.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    dp = le.investments.FirstOrDefault(p => p.investmentID == id);

                    if (dp != null)
                    { 
                        client = dp.client; 
                        Session["loan.cl"] = client;

                        sched = dp.investmentSchedules.Where(p => p.temp == false).ToList();
                        Session["investmentSchedules"] = sched;
                        signatories = dp.investmentSignatories.ToList();

                        Session["signatories"] = dp.investmentSignatories.ToList();
                        gridDocument.DataSource = dp.investmentSignatories;
                        gridDocument.DataBind();

                        txtPeriod.Value = dp.period;
                        chkAuto.Checked = dp.interestMethod;
                        txtInterestRate.Value = dp.interestRate;
                        txtIntBalance.Value = dp.interestBalance;
                        txtPrincBal.Value = dp.principalBalance;
                        txtAmountInvested.Value = dp.amountInvested;
                        cboInvestmentType.SelectedValue=dp.investmentType.investmentTypeID.ToString();
                        txtAmountInvested.ReadOnly = true;
                        dtAppDate.SelectedDate = dp.firstinvestmentDate;
                        if (dp.staffID != null)
                        {
                            cboStaff.SelectedValue = dp.staffID.ToString();
                        }
                        dtAppDate.Enabled = false;
                        cboClient.Items.Clear();
                        cboClient.Items.Add(new RadComboBoxItem(
                            (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName +
                                ", " + client.otherNames + " (" + client.accountNumber + ")", client.clientID.ToString()));
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
                            txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                client.companyName : client.surName;
                            txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                " " : client.otherNames;
                        }
                        
                        txtAccountNo.Text = client.accountNumber;
                        cboPrincRepaymentMode.SelectedValue = dp.principalRepaymentModeID.ToString();
                        cboInterestRepaymentMode.SelectedValue = dp.interestRepaymentModeID.ToString();
                        dtMaturyityDate.SelectedDate = dp.maturityDate;
                        gridDep.DataSource = dp.investmentAdditionals;
                        gridDep.DataBind();
                        gridInt.DataSource = dp.investmentInterests;
                        gridInt.DataBind();
                        gridWith.DataSource = dp.investmentWithdrawals;
                        gridWith.DataBind();
                        txtIntExpected.Value = dp.interestExpected;

                        gridSchedule.DataSource = dp.investmentSchedules.Where(p=>p.temp==false).OrderBy(p=>p.repaymentDate);
                        gridSchedule.DataBind();

                        gridDocument.DataSource = signatories;
                        gridDocument.DataBind();
                    }

                    Session["investment"] = dp;
                }
                else
                {
                    dp = new coreLogic.investment();
                    Session["investment"] = dp;

                    sched = new List<coreLogic.investmentSchedule>();
                    Session["investmentSchedules"] = sched;

                    signatories = new List<coreLogic.investmentSignatory>();
                    Session["signatories"] = signatories;

                    cboInterestRepaymentMode.SelectedValue = "-1";
                    cboPrincRepaymentMode.SelectedValue = "-1";
                }
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadInvestment(id);
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
                        txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            client.companyName : client.surName;
                        txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                            " " : client.otherNames;
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
            if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients
                    .Where(p=> p.clientTypeID == 3)
                    .Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p=>p.clientTypeID==1 || p.clientTypeID==2 || p.clientTypeID == 3).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 3 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(
                    p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 3 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(
                    p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 3 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length > 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.accountNumber.Contains(txtAccountNo.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 3 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length > 0 && txtOtherNames.Text.Trim().Length == 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.surName.Contains(txtSurname.Text.Trim()) || p.accountName.Contains(txtSurname.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 3 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else if (txtSurname.Text.Trim().Length == 0 && txtOtherNames.Text.Trim().Length > 0 && txtAccountNo.Text.Trim().Length == 0)
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.otherNames.Contains(txtOtherNames.Text.Trim())).Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 3 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            else
                clients = le.clients
                    .Where(p => p.clientTypeID == 3)
                    .Where(p => p.clientTypeID == 1 || p.clientTypeID == 2 || p.clientTypeID == 3 || p.clientTypeID == 5 || p.clientTypeID == 6).ToList();
            cboClient.Items.Clear();
            cboClient.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", ""));
            foreach (var item in clients)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((item.clientTypeID == 3 || item.clientTypeID == 4 || item.clientTypeID == 5) ? item.companyName : ((item.clientTypeID == 6) ? item.accountName : item.surName + ", " + item.otherNames) + " (" + item.accountNumber + ")", item.clientID.ToString()));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAmountInvested.Value != null
                && txtPeriod.Value != null
                && txtInterestRate.Value != null
                && cboInvestmentType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && dtMaturyityDate.SelectedDate!=null
                && cboPrincRepaymentMode.SelectedValue!=""
                && cboInterestRepaymentMode.SelectedValue != ""
                && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1"))
                && (txtNaration.Text.Trim()!=""||dp.investmentID>0))
            {
                var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());
                if (ct == null)
                {
                    HtmlHelper.MessageBox("There is no till defined for the currently logged in user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }
                var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == ct.cashiersTillID && p.tillDay == dtAppDate.SelectedDate.Value
                    && p.open == true);
                if (ctd == null)
                {
                    HtmlHelper.MessageBox("The till for the selected date has not been opened for this user (" + User.Identity.Name + ")", "coreERP©: Failed", IconType.deny);
                    return;
                }

                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                dp.amountInvested = txtAmountInvested.Value.Value;
                dp.firstinvestmentDate = dtAppDate.SelectedDate.Value;
                client = le.clients.FirstOrDefault(p => p.clientID == client.clientID);
                if (dp.investmentID <= 0) dp.client = client;
                dp.creation_date = DateTime.Now;
                dp.creator = User.Identity.Name;
                dp.period = (int)txtPeriod.Value.Value;
                dp.interestRate = txtInterestRate.Value.Value;
                int depTypeID = int.Parse(cboInvestmentType.SelectedValue);
                dp.investmentType = le.investmentTypes.FirstOrDefault(p => p.investmentTypeID == depTypeID);
                dp.interestMethod = chkAuto.Checked;
                dp.principalRepaymentModeID = int.Parse(cboPrincRepaymentMode.SelectedValue);
                dp.interestRepaymentModeID = int.Parse(cboInterestRepaymentMode.SelectedValue);
                if (cboStaff.SelectedValue != "")
                {
                    dp.staffID = int.Parse(cboStaff.SelectedValue);
                }
                if (dp.investmentNo == null || dp.investmentNo.Trim().Length == 0)
                {

                    var prof = ent.comp_prof.FirstOrDefault();
                    if (prof.traditionalLoanNo == true)
                    {
                        var cl = le.clients.FirstOrDefault(p => p.clientID == client.clientID);
                        //cl.investments.Load();
                        var cnt = cl.investments.Where(p => p.investmentID != dp.investmentID).Count();
                        char c = (char)(((int)'A') + cnt);
                        dp.investmentNo = idGen.NewInvestmentNumber(client.branchID.Value,
                            client.clientID, dp.investmentID, 
                            cboInvestmentType.Text.Substring(0,2));
                    }
                    else
                    {
                        dp.investmentNo = idGen.NewInvestmentNumber(client.branchID.Value,
                            client.clientID, dp.investmentID,
                            cboInvestmentType.Text.Substring(0, 2));
                    }
                }

                if (dp.investmentID <= 0) le.investments.Add(dp);

                var ss = dp.investmentSchedules.ToList();
                for (int i=ss.Count-1; i>=0; i--)
                {
                    var s = ss[i];
                    if (!sched.Contains(s) && s.expensed==false)
                    {
                        le.investmentSchedules.Remove(s);
                    }
                }
                foreach (var i in sched)
                {
                    if (!dp.investmentSchedules.Contains(i))
                    {
                        dp.investmentSchedules.Add(i);
                    }
                }
                foreach (var r in signatories)
                {
                    if (!dp.investmentSignatories.Contains(r))
                    {
                        dp.investmentSignatories.Add(r);
                    }
                }
                for (int i = dp.investmentSignatories.Count - 1; i >= 0; i--)
                {
                    var r = dp.investmentSignatories.ToList()[i];
                    if (!signatories.Contains(r))
                    {
                        dp.investmentSignatories.Remove(r);
                    }
                }
                coreLogic.investmentAdditional da=null;
                if (dp.investmentID <= 0)
                {
                    dp.principalBalance = txtAmountInvested.Value.Value;
                    dp.maturityDate = dtMaturyityDate.SelectedDate;
                    if (dp.period == 3)
                    {
                        dp.maturityDate = dp.firstinvestmentDate.AddDays(91);
                    }
                    else if (dp.period == 6)
                    {
                        dp.maturityDate = dp.firstinvestmentDate.AddDays(182);
                    }
                    else if (dp.period == 12)
                    {
                        dp.maturityDate = dp.firstinvestmentDate.AddDays(365);
                    }
                    dp.interestExpected = txtAmountInvested.Value.Value
                        * txtPeriod.Value.Value
                        * (txtInterestRate.Value.Value/100.0);
                    int mopID = 1;
                    if (cboPaymentType.SelectedValue != "")
                        mopID = int.Parse(cboPaymentType.SelectedValue);
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                    da = new coreLogic.investmentAdditional
                    {
                        checkNo = txtCheckNo.Text,
                        investmentAmount = txtAmountInvested.Value.Value,
                        bankID = bankID,
                        interestBalance = 0,
                        investmentDate = dtAppDate.SelectedDate.Value,
                        creation_date = DateTime.Now,
                        creator = User.Identity.Name,
                        principalBalance = txtAmountInvested.Value.Value,
                        modeOfPayment = mop,
                        posted=false,
                        naration=txtNaration.Text
                    };
                    dp.investmentAdditionals.Add(da);
                    CalculateSchedule();
                }
                
                le.SaveChanges();
                ent.SaveChanges();

                Session["loan.cl"] = null;
                Session["investment"] = null;
                if (da != null)
                {
                    HtmlHelper.MessageBox2("Investment Data Saved Successfully!",
                        ResolveUrl("~/ln/investmentReports/addReceipt.aspx?id=" + da.investmentAdditionalID.ToString()),
                        "coreERP©: Successful", IconType.ok);
                }
                else
                {
                    HtmlHelper.MessageBox2("Investment Data Saved Successfully!",
                           ResolveUrl("~/ln/investment/default.aspx?id=" + dp.investmentID.ToString()),
                           "coreERP©: Successful", IconType.ok);
                }
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", 
                    "coreERP: Incomplete", IconType.warning);
            }
        }

        private void CalculateSchedule()
        {
            if (sched.Count == 0 && chkAuto.Checked == true)
            {
                List<DateTime> listInt = new List<DateTime>();
                List<DateTime> listPrinc = new List<DateTime>();
                List<DateTime> listAll = new List<DateTime>();

                DateTime date = dtAppDate.SelectedDate.Value;
                int i = 1;
                var totalInt = txtAmountInvested.Value.Value * (txtPeriod.Value.Value) * (txtInterestRate.Value.Value) / 100.0;
                var intererst = 0.0;
                var princ = 0.0;
                while (date < dtMaturyityDate.SelectedDate.Value)
                {
                    date = date.AddMonths(1);
                    if (date >= dtMaturyityDate.SelectedDate.Value) break;
                    if ((cboInterestRepaymentMode.SelectedValue == "30")
                        || (cboInterestRepaymentMode.SelectedValue == "90" && i % 3 == 0)
                        || (cboInterestRepaymentMode.SelectedValue == "180" && i % 6 == 0)
                        )
                    {
                        listInt.Add(date);
                        if (listAll.Contains(date) == false) listAll.Add(date);
                    }
                    if ((cboPrincRepaymentMode.SelectedValue == "30")
                        || (cboPrincRepaymentMode.SelectedValue == "90" && i % 3 == 0)
                        || (cboPrincRepaymentMode.SelectedValue == "180" && i % 6 == 0)
                        )
                    {
                        listPrinc.Add(date);
                        if (listAll.Contains(date) == false) listAll.Add(date);
                    }
                    i += 1;
                }
                listPrinc.Add(dtMaturyityDate.SelectedDate.Value);
                listInt.Add(dtMaturyityDate.SelectedDate.Value);
                listAll.Add(dtMaturyityDate.SelectedDate.Value);

                dp.modern = true;
                foreach (DateTime date2 in listAll)
                {
                    if (listPrinc.Contains(date2))
                    {
                        princ = txtAmountInvested.Value.Value / listPrinc.Count;
                    }
                    if (listInt.Contains(date2))
                    {
                        intererst = totalInt / listInt.Count;
                    }
                    dp.investmentSchedules.Add(new coreLogic.investmentSchedule
                    {
                        interestPayment = intererst,
                        principalPayment = princ,
                        repaymentDate = date2,
                        authorized = false,
                        expensed = false,
                        temp = false
                    });
                }
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.",
                    "coreERP: Incomplete", IconType.warning);
            }
        }

        protected void cboInvestmentType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboInvestmentType.SelectedValue != "")
            {
                int id = int.Parse(cboInvestmentType.SelectedValue);
                var depType = le.investmentTypes.FirstOrDefault(p => p.investmentTypeID == id);
                if (depType != null)
                {
                    txtInterestRate.Value = depType.interestRate;
                    txtPeriod.Value = depType.defaultPeriod;
                    if (dtAppDate.SelectedDate != null)
                    {
                        if (ent.comp_prof.FirstOrDefault().comp_name.Contains("Link Exchange"))
                        {
                            dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddMonths((int)txtPeriod.Value.Value);
                        }
                        else
                        {
                            var days = 0;
                            switch ((int)txtPeriod.Value.Value)
                            {
                                case 1:
                                    days = 30;
                                    break;
                                case 2:
                                    days = 60;
                                    break;
                                case 3:
                                    days = 91;
                                    break;
                                case 6:
                                    days = 182;
                                    break;
                                case 9:
                                    days = 273;
                                    break;
                                case 12:
                                    days = 365;
                                    break;
                                default:
                                    break;
                            }
                            if (days == 0) dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddMonths((int)txtPeriod.Value.Value);
                            else
                            {
                                dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddDays(days);
                            }
                        }
                    }
                }
            }
        }

        protected void btnAddSchedule_Click(object sender, EventArgs e)
        {
            if (dtRepaymentDate.SelectedDate!=null && txtPrincipal.Value != null &&
                txtInterest.Value!= null)
            {
                coreLogic.investmentSchedule g;
                if (btnAddSchedule.Text == "Add Schedule")
                {
                    g = new coreLogic.investmentSchedule();
                }
                else
                {
                    g = Session["investmentSchedule"] as coreLogic.investmentSchedule;
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
                    var s = sched.FirstOrDefault(p => p.investmentScheduleID == g.investmentScheduleID);
                    s.principalPayment = g.principalPayment;
                    s.interestPayment = g.interestPayment;
                    s.repaymentDate = g.repaymentDate;
                }
                Session["investmentSchedules"] = sched;
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

                    Session["investmentSchedule"] = g; 
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
            if (dtAppDate.SelectedDate != null && txtPeriod.Value != null)
            {
                if (ent.comp_prof.FirstOrDefault().comp_name.Contains("Link Exchange"))
                {
                    dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddMonths((int)txtPeriod.Value.Value);
                }
                else
                {
                    var days = 0;
                    switch ((int)txtPeriod.Value.Value)
                    {
                        case 1:
                            days = 30;
                            break;
                        case 2:
                            days = 60;
                            break;
                        case 3:
                            days = 91;
                            break;
                        case 6:
                            days = 182;
                            break;
                        case 9:
                            days = 273;
                            break;
                        case 12:
                            days = 365;
                            break;
                        default:
                            break;
                    }
                    if (days == 0) dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddMonths((int)txtPeriod.Value.Value);
                    else
                    {
                        dtMaturyityDate.SelectedDate = dtAppDate.SelectedDate.Value.AddDays(days);
                    }
                }
            } 
        }

        protected void txtRateA_TextChanged(object sender, EventArgs e)
        {
            if (txtRateA.Value != null)
            {
                txtInterestRate.Value = Math.Round(txtRateA.Value.Value / 12.0,7);
            }
        }

        protected void txtInterestRate_TextChanged(object sender, EventArgs e)
        {
            if (txtInterestRate.Value != null)
            {
                txtRateA.Value = Math.Round(txtInterestRate.Value.Value * 12.0, 7);
            }
        }

        protected void gridDocument_ItemCommand(object sender, GridCommandEventArgs e)
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
                signatories.RemoveAt(e.Item.ItemIndex);
            }
            gridDocument.DataSource = signatories;
            gridDocument.DataBind();
        }

        protected void btnAddDcoument_Click(object sender, EventArgs e)
        {
            if (txtDocDesc.Text != "")
            {
                if (upload4.UploadedFiles.Count > 0)
                {
                    foreach (UploadedFile item in upload4.UploadedFiles)
                    {
                        byte[] b = new byte[item.InputStream.Length];
                        item.InputStream.Read(b, 0, b.Length);


                        if (btnAddDcoument.Text == "Update Signatory")
                        {
                            var g = Session["signatory"] as coreLogic.investmentSignatory;
                            g.fullName = txtDocDesc.Text;
                            g.image.description = txtDocDesc.Text;
                            g.image.image1 = b;
                            g.image.content_type = item.ContentType;
                        }
                        else
                        {
                            var i = new coreLogic.image
                            {
                                description = txtDocDesc.Text,
                                image1 = b,
                                content_type = item.ContentType 
                            };

                            var g = new coreLogic.investmentSignatory
                            {
                                investment = dp,
                                image = i,
                                fullName=txtDocDesc.Text
                            };
                            signatories.Add(g);
                        }
                    }
                }
                else if (btnAddDcoument.Text == "Update Signatory")
                {
                    var g = Session["signatory"] as coreLogic.investmentSignatory;
                    g.fullName = txtDocDesc.Text;
                }
                Session["signatories"] = signatories;
                gridDocument.DataSource = signatories;
                gridDocument.DataBind();

                txtDocDesc.Text = "";
                btnAddDcoument.Text = "Add Signatory";
            }
        }

        protected void cboClient_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var cl in le.clients
                    .Where(p=> p.clientTypeID==3)
                    .Where(p => (p.surName.Contains(e.Text) || p.otherNames.Contains(e.Text) || p.companyName.Contains(e.Text)
                    || p.accountName.Contains(e.Text))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        private void LoadInvestment(int? id)
        {
            if (id != null)
            {
                dp = le.investments.FirstOrDefault(p => p.investmentID == id);

                if (dp != null)
                {
                    //dp.investmentAdditionals.Load();
                    //dp.investmentInterests.Load();
                    //dp.investmentTypeReference.Load();
                    //dp.investmentWithdrawals.Load();
                    //dp.clientReference.Load();
                    //dp.investmentSchedules.Load();
                    //dp.investmentSignatories.Load();

                    foreach (var sig in dp.investmentSignatories)
                    {
                        //sig.imageReference.Load();
                    }
                    foreach (var da in dp.investmentAdditionals)
                    {
                        //da.modeOfPaymentReference.Load();
                    }

                    foreach (var dw in dp.investmentWithdrawals)
                    {
                        //dw.modeOfPaymentReference.Load();
                    }

                    client = dp.client;
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
                    Session["loan.cl"] = client;

                    sched = dp.investmentSchedules.Where(p => p.temp == false).ToList();
                    if (Session["investmentSchedules"] != null)
                    {
                        var sch = Session["investmentSchedules"] as List<coreLogic.investmentSchedule>;
                        if (sch != null)
                        {
                            for (int i = sch.Count - 1; i >= 0; i--)
                            {
                                if (sch[i].investmentScheduleID <= 0)
                                {
                                    sched.Add(sch[i]);
                                }
                            }
                        }
                    }
                    Session["investmentSchedules"] = sched;

                    signatories=dp.investmentSignatories.ToList();
                    if (Session["signatories"] != null)
                    {
                        var sch = Session["signatories"] as List<coreLogic.investmentSignatory>;
                        if (sch != null)
                        {
                            for (int i = sch.Count - 1; i >= 0; i--)
                            {
                                if (sch[i].investmentSignatoryID <= 0)
                                {
                                    signatories.Add(sch[i]);
                                }
                            }
                        }
                    }
                    Session["signatories"] = signatories;
                }
                RenderImages();
                Session["investment"] = dp;
            }
            else
            {
                if (Session["loan.cl"] != null)
                {
                    client = Session["loan.cl"] as coreLogic.client;
                }
                if (Session["investment"] != null)
                {
                    dp = Session["investment"] as coreLogic.investment;
                }
                else
                {
                    dp = new coreLogic.investment();
                    Session["investment"] = dp;
                }
                if (Session["investmentSchedules"] != null)
                {
                    sched = Session["investmentSchedules"] as List<coreLogic.investmentSchedule>;
                }
                else
                {
                    sched = new List<coreLogic.investmentSchedule>();
                    Session["investmentSchedules"] = sched;
                }
                if (Session["signatories"] != null)
                {
                    signatories = Session["signatories"] as List<coreLogic.investmentSignatory>;

                }
                else
                {
                    signatories = new List<coreLogic.investmentSignatory>();
                    Session["signatories"] = signatories;
                } 
            }
        }
    }
}