using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.investment
{
    public partial class add : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        coreLogic.client client;
        coreLogic.investment dp;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!IsPostBack)
            { 
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
                
                this.cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                }

                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    dp = le.investments.FirstOrDefault(p => p.investmentID == id);

                    if (dp != null)
                    {
                        ////dp.investmentAdditionals.Load();
                        ////dp.investmentInterests.Load();
                        ////dp.investmentTypeReference.Load();
                        ////dp.investmentWithdrawals.Load();
                        ////dp.clientReference.Load();
                        ////dp.investmentSignatories.Load();

                        //foreach (var sig in dp.investmentSignatories)
                        //{
                        //    //sig.imageReference.Load();
                        //}
                        //foreach (var da in dp.investmentAdditionals)
                        //{
                        //    //da.modeOfPaymentReference.Load();
                        //}

                        //foreach (var dw in dp.investmentWithdrawals)
                        //{
                        //    //dw.modeOfPaymentReference.Load();
                        //}

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
                          
                        txtPeriod.Value = dp.period;
                        txtInterestRate.Value = dp.interestRate;
                        txtIntBalance.Value = dp.interestBalance;
                        txtPrincBal.Value = dp.principalBalance; 
                        cboInvestmentType.SelectedValue=dp.investmentType.investmentTypeID.ToString();
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
                            txtSurname.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                client.companyName : client.surName;
                            txtOtherNames.Text = (client.clientTypeID == 3 || client.clientTypeID == 4 || client.clientTypeID == 5) ?
                                " " : client.otherNames;
                        }
                        txtAccountNo.Text = client.accountNumber;
                       
                        gridDep.DataSource = dp.investmentAdditionals;
                        gridDep.DataBind();
                        gridInt.DataSource = dp.investmentInterests;
                        gridInt.DataBind();
                        gridWith.DataSource = dp.investmentWithdrawals;
                        gridWith.DataBind();

                        gridDocument.DataSource = dp.investmentSignatories;
                        gridDocument.DataBind();

                    }

                    Session["investment"] = dp;
                }
                else
                {
                    dp = new coreLogic.investment();
                    Session["investment"] = dp;
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
                    rotator1.Items.Clear();

                    RenderImages();
                }
            }
        }

        private void RenderImages()
        {
            var phyAddr = client.clientAddresses.FirstOrDefault(p => p.addressTypeID == 1);

            if (phyAddr != null)
            {
                //phyAddr.addressReference.Load();
                //phyAddr.address.addressImages.Load();
                foreach (var item in phyAddr.address.addressImages)
                {
                    //item.imageReference.Load();
                    RadBinaryImage img = new RadBinaryImage();
                    img.Width = 177;
                    img.Height = 123;
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
            if (txtAmountInvested.Value != null
                && txtPeriod.Value != null
                && txtInterestRate.Value != null
                && cboInvestmentType.SelectedValue != ""
                && dtAppDate.SelectedDate != null
                && cboPaymentType.SelectedValue != ""
                && ((cboPaymentType.SelectedValue != "1" && cboBank.SelectedValue != "") || (cboPaymentType.SelectedValue == "1"))
                && txtNaration.Text.Trim() != "")
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
                if (dp.investmentID > 0)
                {
                    dp.principalBalance += txtAmountInvested.Value.Value;
                    dp.amountInvested += txtAmountInvested.Value.Value;
                    var days = dp.maturityDate >= dtAppDate.SelectedDate.Value ?
                        (dp.maturityDate.Value - dtAppDate.SelectedDate.Value).TotalDays : 0; 
                    int mopID = int.Parse(cboPaymentType.SelectedValue);
                    var mop = le.modeOfPayments.FirstOrDefault(p => p.modeOfPaymentID == mopID);
                    int? bankID = null;
                    if (cboBank.SelectedValue != "") bankID = int.Parse(cboBank.SelectedValue);
                    var da = new coreLogic.investmentAdditional
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
                        naration = txtNaration.Text,
                        posted = false
                    };
                    dp.principalBalance += txtAmountInvested.Value.Value; 
                    dp.investmentAdditionals.Add(da);

                    dp.modification_date = DateTime.Now;
                    dp.last_modifier = User.Identity.Name;

                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    List<DateTime> listInt = new List<DateTime>();
                    List<DateTime> listPrinc = new List<DateTime>();
                    List<DateTime> listAll = new List<DateTime>();

                    DateTime date = dtAppDate.SelectedDate.Value;
                    int i = 1;
                    var totalInt = txtAmountInvested.Value.Value * (txtPeriod.Value.Value) * (txtInterestRate.Value.Value) / 100.0;
                    var intererst = 0.0;
                    var princ = 0.0;
                    while (date < dp.maturityDate)
                    {
                        date = date.AddMonths(1);
                        if (date >= dp.maturityDate) break;
                        if ((dp.interestRepaymentModeID == 30)
                            || (dp.interestRepaymentModeID == 90 && i % 3 == 0)
                            || (dp.interestRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listInt.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        if ((dp.principalRepaymentModeID == 30)
                            || (dp.principalRepaymentModeID == 90 && i % 3 == 0)
                            || (dp.principalRepaymentModeID == 180 && i % 6 == 0)
                            )
                        {
                            listPrinc.Add(date);
                            if (listAll.Contains(date) == false) listAll.Add(date);
                        }
                        i += 1;
                    }
                    listPrinc.Add(dp.maturityDate.Value);
                    listInt.Add(dp.maturityDate.Value);
                    listAll.Add(dp.maturityDate.Value);

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
                        var ds = le.investmentSchedules.FirstOrDefault(p => p.investmentID == dp.investmentID && p.repaymentDate == date2);
                        if (ds == null)
                        {
                            dp.investmentSchedules.Add(new coreLogic.investmentSchedule
                            {
                                interestPayment = intererst,
                                principalPayment = princ,
                                repaymentDate = date2,
                                authorized = false
                            });
                        }
                        else
                        {
                            ds.interestPayment += intererst;
                            ds.principalPayment += princ;
                            ds.authorized = false;
                        }
                    }

                    le.SaveChanges();
                    ent.SaveChanges();

                    Session["loan.cl"] = null;
                    Session["investment"] = null;
                    HtmlHelper.MessageBox2("Additional Investment Data Saved Successfully!",
                        ResolveUrl("~/ln/investmentReports/addReceipt.aspx?id=" + da.investmentAdditionalID.ToString()),
                        "coreERP©: Successful", IconType.ok);
                }
            }
            else
            {
                HtmlHelper.MessageBox("Kindly complete all the required fields before saving the transaction.", "coreERP: Incomplete", IconType.warning);
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
            }
        }
    }
}