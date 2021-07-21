using coreERP.Helpers;
using coreLogic;
using coreLogic.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


namespace coreERP.ln.setup
{ 
    public partial class postTill : System.Web.UI.Page
    {
        IRepaymentsManager rpmtMgr;
        IDisbursementsManager disbMgr;

        RoleHelper roleHelper = new RoleHelper();
        coreLogic.coreSecurityEntities sec = new coreLogic.coreSecurityEntities();
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            rpmtMgr = new RepaymentsManager();
            disbMgr = new DisbursementsManager();
            if (!IsPostBack)
            {
                dtEndDate.SelectedDate = DateTime.Today;
                cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in sec.users.Where(r=>r.is_active).OrderBy(p => p.full_name))
                {
                    if (le.cashiersTills.FirstOrDefault(p => p.userName.ToLower() == r.user_name.ToLower()) != null)
                    {
                        cboUserName.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.full_name + " (" + r.user_name + ")", r.user_name));
                    }
                }

                cboFieldAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.agents.OrderBy(p => p.surName).ThenBy(p => p.otherNames))
                {
                    cboFieldAgent.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.surName + ", " + r.otherNames + " (" + r.agentNo + ")",
                        r.agentID.ToString()));
                }
            
            }
        }

        protected void gridDisbursment_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
             
            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("cashierDisbursementID").ToString());
            var disb = le.cashierDisbursements.FirstOrDefault(p => p.cashierDisbursementID == id);
            if (disb != null)
            {
                le.cashierDisbursements.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }


        

        protected void gridReceipt_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("cashierReceiptID").ToString());
            var r = le.cashierReceipts.FirstOrDefault(p => p.cashierReceiptID == id);
            if (r != null)
            {
                    if (r.multiPaymentClients != null && r.multiPayments != null)
                    {
                        var rs = r.multiPayments.ToList();
                        for (int i = r.multiPayments.Count - 1; i >= 0; i--)
                        {
                            if (rs.Count > 0)
                            {
                                var e2 = rs[i];
                                if (e2 != null)
                                {
                                    le.multiPayments.Remove(e2);
                                }
                            }
                        }
                        var rs2 = r.multiPaymentClients.ToList();
                        for (int i = rs2.Count - 1; i >= 0; i--)
                        {
                            if (rs2.Count > 0)
                            {
                                var e2 = rs2[i];
                                if (e2 != null)
                                {
                                    le.multiPaymentClients.Remove(e2);
                                }
                            }
                        }
                    }
                le.cashierReceipts.Remove(r);
                if (r.loan != null && r.loan.cashierReceipts!=null
                    && r.loan.cashierReceipts.Where(p => p.cashierReceiptID != r.cashierReceiptID).Count() == 0)
                {
                        r.loan.loanStatusID = 3;
                    }
                le.SaveChanges();
                OnChange();
            }
        }
    
        protected void btnOpen_Click(object sender, EventArgs e)
        {
            if (cboUserName.SelectedValue != "" && dtStartDate.SelectedDate != null && dtEndDate.SelectedDate!=null)
            {
                ICashiersTillHelper tillHelper = new CashiersTillHelper();
                try
                {
                    var listDisb = new List<int>();
                    var listReceipt = new List<int>();
                    var listSA = new List<int>();
                    var listSW = new List<int>();
                    var listDA = new List<int>();
                    var listCSC = new List<int>();
                    var listDW = new List<int>();
                    var listDWC = new List<int>();
                    var listIA = new List<int>();
                    var listIW = new List<int>();
                    var listSC = new List<int>();
                    var listSU = new List<int>();
                    var listRSC = new List<int>();
                    var listRSU = new List<int>();
                    int? agentId = null;
                    
                    if (cboFieldAgent.SelectedValue != "")
                    {
                        agentId = int.Parse(cboFieldAgent.SelectedValue);
                    }

                    foreach (GridItem r in gridDisbursment.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listDisb.Add(int.Parse(gridDisbursment.MasterTableView.DataKeyValues[r.ItemIndex]["cashierDisbursementID"].ToString()));
                        }
                    }
                    foreach (GridItem r in gridReceipt.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listReceipt.Add(int.Parse(gridReceipt.MasterTableView.DataKeyValues[r.ItemIndex]["cashierReceiptID"].ToString()));
                        }
                    }
                    foreach (GridItem r in gridSA.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listSA.Add(int.Parse(gridSA.MasterTableView.DataKeyValues[r.ItemIndex]["savingAdditionalID"].ToString()));
                        }
                    }
                    if (roleHelper.IsOwnerOrAdmin(User.Identity.Name))
                    {
                        foreach (GridItem r in gridSW.MasterTableView.Items)
                        {
                            if (r.Selected == true)
                            {
                                listSW.Add(int.Parse(gridSW.MasterTableView.DataKeyValues[r.ItemIndex]["savingWithdrawalID"].ToString()));
                            }
                        }
                    }
                    foreach (GridItem r in gridDA.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listDA.Add(int.Parse(gridDA.MasterTableView.DataKeyValues[r.ItemIndex]["depositAdditionalID"].ToString()));
                        }
                    }
                    foreach (GridItem r in gridCSC.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listCSC.Add(int.Parse(gridCSC.MasterTableView.DataKeyValues[r.ItemIndex]["clientServiceChargeId"].ToString()));
                        }
                    }

                    

                        foreach (GridItem r in gridDW.MasterTableView.Items)
                        {
                            if (r.Selected == true)
                            {
                                listDW.Add(int.Parse(gridDW.MasterTableView.DataKeyValues[r.ItemIndex]["depositWithdrawalID"].ToString()));
                            }
                        }
                        foreach (GridItem r in gridDWC.MasterTableView.Items)
                        {
                            if (r.Selected == true)
                            {
                                listDWC.Add(int.Parse(gridDWC.MasterTableView.DataKeyValues[r.ItemIndex]["depositWithdrawalID"].ToString()));
                            }
                        }
                    

                    foreach (GridItem r in gridIA.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listIA.Add(int.Parse(gridIA.MasterTableView.DataKeyValues[r.ItemIndex]["investmentAdditionalID"].ToString()));
                        }
                    }
                    foreach (GridItem r in gridIW.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listIW.Add(int.Parse(gridIW.MasterTableView.DataKeyValues[r.ItemIndex]["investmentWithdrawalID"].ToString()));
                        }
                    }
                    foreach (GridItem r in gridSC.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listSC.Add(int.Parse(gridSC.MasterTableView.DataKeyValues[r.ItemIndex]["susuContributionID"].ToString()));
                        }
                    }
                    foreach (GridItem r in gridSU.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listSU.Add(int.Parse(gridSU.MasterTableView.DataKeyValues[r.ItemIndex]["susuAccountID"].ToString()));
                        }
                    }
                    foreach (GridItem r in gridRSC.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listRSC.Add(int.Parse(gridRSC.MasterTableView.DataKeyValues[r.ItemIndex]["regularSusuContributionID"].ToString()));
                        }
                    }
                    foreach (GridItem r in gridRSU.MasterTableView.Items)
                    {
                        if (r.Selected == true)
                        {
                            listRSU.Add(int.Parse(gridRSU.MasterTableView.DataKeyValues[r.ItemIndex]["regularSusuAccountID"].ToString()));
                        }
                    }
                    var table = new Dictionary<string, List<int>>();
                    table.Add("listDA", listDA);
                    table.Add("listCSC", listCSC);
                    table.Add("listDW", listDW);
                    table.Add("listDWC", listDWC);
                    
                    table.Add("listSA", listSA);
                    table.Add("listSW", listSW);
                    table.Add("listReceipt", listReceipt);
                    table.Add("listDisb", listDisb);
                    table.Add("listIA", listIA);
                    table.Add("listIW", listIW);
                    table.Add("listSC", listSC);
                    table.Add("listSU", listSU);
                    table.Add("listRSC", listRSC);
                    table.Add("listRSU", listRSU);

                    tillHelper.Post(cboUserName.SelectedValue, dtStartDate.SelectedDate.Value, dtEndDate.SelectedDate.Value,
                        table);

                    HtmlHelper.MessageBox2("Till posted for selected cashier successfully!", ResolveUrl("~/ln/setup/postTill.aspx"), 
                        "coreERP©: Successful", IconType.ok);
                    Session["disb"] = null;
                    Session["rcpt"] = null;
                    Session["das"] = null;
                    Session["csc"] = null;
                    Session["dws"] = null;
                    Session["sas"] = null;
                    Session["sws"] = null;
                    Session["ias"] = null;
                    Session["iws"] = null;
                    Session["sus"] = null;
                    Session["scs"] = null;
                    Session["rscs"] = null;
                    Session["rsus"] = null;
                }
                catch (ApplicationException x)
                {
                    HtmlHelper.MessageBox(x.Message, "coreERP©: Failed", IconType.deny);
                }
            }
        }

        protected void dtDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            OnChange();
        }

        protected void cboUserName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            if ((cboUserName.SelectedValue != "" || cboFieldAgent.SelectedValue!="")
                && dtStartDate.SelectedDate != null && dtEndDate.SelectedDate!= null)
            {
                int? agentId = null;
                if (cboFieldAgent.SelectedValue != "")
                {
                    agentId = int.Parse(cboFieldAgent.SelectedValue);
                }

                btnOpen.Enabled = true;
                var disb = le.cashierDisbursements.Where(p => p.txDate <= dtEndDate.SelectedDate.Value 
                    && p.txDate>= dtStartDate.SelectedDate.Value
                    && p.cashiersTill.userName.ToLower()  == cboUserName.SelectedValue.ToLower()
                    && p.posted == false
                    && (cboFieldAgent.SelectedValue == "" || p.loan.agentID == agentId))
                    .ToList();
                if (disb.Count > 0)
                {
                    gridDisbursment.DataSource = disb;
                    gridDisbursment.DataBind();
                    gridDisbursment.Visible = true;
                    trDisb.Visible = true;
                    trDisb1.Visible = true;
                    
                    foreach (var i in disb)
                    {
                        if(!string.IsNullOrEmpty(i.loan.client.companyName))
                        {
                            i.loan.client.surName = i.loan.client.companyName;
                            i.loan.client.otherNames = "";
                        }
                    }
                }
                else
                {
                    gridDisbursment.Visible = false;
                    trDisb.Visible = false;
                    trDisb1.Visible = false;
                }
                Session["disb"] = disb;

                var rcpt = le.cashierReceipts.Where(p => p.txDate <= dtEndDate.SelectedDate.Value
                    && p.txDate >= dtStartDate.SelectedDate.Value
                    && p.cashiersTill.userName.ToLower() == cboUserName.SelectedValue.ToLower()
                    && p.posted == false
                    && (cboFieldAgent.SelectedValue == "" || p.loan.agentID == agentId))
                    .ToList(); 
                if (rcpt.Count > 0)
                {
                    gridReceipt.DataSource = rcpt;
                    gridReceipt.DataBind();
                    gridReceipt.Visible = true;
                    trRcpt.Visible = true;
                    trRcpt1.Visible = true;
                    
                    foreach (var i in rcpt)
                    {
                        if(!string.IsNullOrEmpty(i.loan.client.companyName))
                        {
                            i.loan.client.surName = i.loan.client.companyName;
                            i.loan.client.otherNames = "";
                        }
                    }
                }
                else
                {
                    gridReceipt.Visible = false;
                    trRcpt.Visible = false;
                    trRcpt1.Visible = false;
                }
                Session["rcpt"] = rcpt;

                var startDate = dtStartDate.SelectedDate.Value.Date;
                var endDate = dtEndDate.SelectedDate.Value.Date.AddDays(1).AddSeconds(-1);

                var das = le.depositAdditionals.Where(p => (p.depositDate >= startDate && p.depositDate <= endDate)
                    && (p.posted == false)
                    && p.creator != null && p.creator.ToLower() == cboUserName.SelectedValue.ToLower()
                        && (cboFieldAgent.SelectedValue == "" || p.deposit.agentId == agentId)).ToList();
                if (das.Count > 0)
                {
                    gridDA.DataSource = das;
                    gridDA.DataBind();
                    gridDA.Visible = true;
                    trDA.Visible = true;
                    trDA1.Visible = true;
                    
                    foreach (var i in das)
                    {
                        if(!string.IsNullOrEmpty(i.deposit.client.companyName))
                        {
                            i.deposit.client.surName = i.deposit.client.companyName;
                            i.deposit.client.otherNames = "";
                        }
                    }
                }
                else
                {
                    gridDA.Visible = false;
                    trDA.Visible = false;
                    trDA1.Visible = false;
                }
                Session["das"] = das;

                var csc = le.clientServiceCharges.Where(p => (p.chargeDate >= startDate && p.chargeDate <= endDate)
                    && (p.posted == false)
                    && p.creator != null && p.creator.ToLower() == cboUserName.SelectedValue.ToLower()).ToList();
                if (csc.Count > 0)
                {
                    gridCSC.DataSource = csc;
                    gridCSC.DataBind();
                    gridCSC.Visible = true;
                    trCSC.Visible = true;
                    trCSC1.Visible = true;

                    foreach (var i in csc)
                    {
                        if (!string.IsNullOrEmpty(i.client.companyName))
                        {
                            i.client.surName = i.client.companyName;
                            i.client.otherNames = "";
                        }
                    }
                }
                else
                {
                    gridCSC.Visible = false;
                    trCSC.Visible = false;
                    trCSC1.Visible = false;
                }
                Session["csc"] = das;

                  var dws = le.depositWithdrawals.Where(p => (p.withdrawalDate >= startDate && p.withdrawalDate <= endDate)
                    && (p.posted == false)
                    && p.creator != null && p.creator.ToLower() == cboUserName.SelectedValue.ToLower()
                        && (cboFieldAgent.SelectedValue == "" || p.deposit.agentId == agentId)).ToList();
                    if (dws.Count > 0)
                    {
                        gridDW.DataSource = dws;
                        gridDW.DataBind();
                        gridDW.Visible = true;
                        trDW.Visible = true;
                        trDW1.Visible = true;

                        foreach (var i in dws)
                        {
                            if (!string.IsNullOrEmpty(i.deposit.client.companyName))
                            {
                                i.deposit.client.surName = i.deposit.client.companyName;
                                i.deposit.client.otherNames = "";
                            }
                        }
                    }
                    else
                    {
                        gridDW.Visible = false;
                        trDW.Visible = false;
                        trDW1.Visible = false;
                    }

                    Session["dws"] = dws;


                    var dwc = le.depositWithdrawals.Where(p => (p.withdrawalDate >= startDate && p.withdrawalDate <= endDate && p.disInvestmentCharge > 0)
                        && (p.posted == false)
                        && p.creator != null && p.creator.ToLower() == cboUserName.SelectedValue.ToLower()
                            && (cboFieldAgent.SelectedValue == "" || p.deposit.agentId == agentId)).ToList();
                    if (dwc.Count > 0)
                    {
                        gridDWC.DataSource = dwc;
                        gridDWC.DataBind();
                        gridDWC.Visible = true;
                        trDWC.Visible = true;
                        trDWC1.Visible = true;

                        foreach (var i in dwc)
                        {
                            if (!string.IsNullOrEmpty(i.deposit.client.companyName))
                            {
                                i.deposit.client.surName = i.deposit.client.companyName;
                                i.deposit.client.otherNames = "";
                            }
                        }
                    }
                    else
                    {
                        gridDWC.Visible = false;
                        trDWC.Visible = false;
                        trDWC1.Visible = false;
                    }

                    Session["dwc"] = dwc;
                
                var sas = le.savingAdditionals.Where(p => (p.savingDate >= startDate && p.savingDate <= endDate)
                    && (p.posted == false)
                    && p.creator != null && p.creator.ToLower() == cboUserName.SelectedValue.ToLower()
                        && (cboFieldAgent.SelectedValue == "" || p.saving.agentId == agentId)).ToList();
                if (sas.Count > 0)
                {
                    gridSA.DataSource = sas;
                    gridSA.DataBind();
                    gridSA.Visible = true;
                    trSA.Visible = true;
                    trSA1.Visible = true;
                    
                    foreach (var i in sas)
                    {
                        if(!string.IsNullOrEmpty(i.saving.client.companyName))
                        {
                            i.saving.client.surName = i.saving.client.companyName;
                            i.saving.client.otherNames = "";
                        }
                    }
                }
                else
                {
                    gridSA.Visible = false;
                    trSA.Visible = false;
                    trSA1.Visible = false;
                }
                Session["sas"] = sas;

                if (roleHelper.IsOwnerOrAdmin(User.Identity.Name))
                {
                    var sws = le.savingWithdrawals.Where(p => (p.withdrawalDate >= startDate && p.withdrawalDate <= endDate)
                    && (p.posted == false) && p.creator != null
                    && p.creator.ToLower() == cboUserName.SelectedValue.ToLower()
                        && (cboFieldAgent.SelectedValue == "" || p.saving.agentId == agentId)).ToList();
                    if (sws.Count > 0)
                    {
                        gridSW.DataSource = sws;
                        gridSW.DataBind();
                        gridSW.Visible = true;
                        trSW.Visible = true;
                        trSW1.Visible = true;


                        foreach (var i in sws)
                        {
                            if (!string.IsNullOrEmpty(i.saving.client.companyName))
                            {
                                i.saving.client.surName = i.saving.client.companyName;
                                i.saving.client.otherNames = "";
                            }
                        }
                    }
                    else
                    {
                        gridSW.Visible = false;
                        trSW.Visible = false;
                        trSW1.Visible = false;
                    }
                    Session["sws"] = sws;
                }
                else {
                    gridSW.Visible = false;
                    trSW.Visible = false;
                    trSW1.Visible = false;
                    Session["sws"] = null;
                }

                var iws = le.investmentWithdrawals.Where(p => (p.withdrawalDate >= startDate && p.withdrawalDate <= endDate)
                    && (p.posted == false) && p.creator != null && p.creator.ToLower() == cboUserName.SelectedValue.ToLower()).ToList(); 
                if (iws.Count > 0)
                {
                    gridIW.DataSource = iws;
                    gridIW.DataBind();
                    gridIW.Visible = true;
                    trIW.Visible = true;
                    trIW1.Visible = true;

                    foreach (var i in iws)
                    {
                        if(!string.IsNullOrEmpty(i.investment.client.companyName))
                        {
                            i.investment.client.surName = i.investment.client.companyName;
                            i.investment.client.otherNames = "";
                        }
                    }
                }
                else
                {
                    gridIW.Visible = false;
                    trIW.Visible = false;
                    trIW1.Visible = false;
                }
                Session["iws"] = iws;

                var ias = le.investmentAdditionals.Where(p => (p.investmentDate >= startDate && p.investmentDate <= endDate)
                    && (p.posted == false) && p.creator != null && p.creator.ToLower() == cboUserName.SelectedValue.ToLower()).ToList(); 
                if (ias.Count > 0)
                {
                    gridIA.DataSource = ias;
                    gridIA.DataBind();
                    gridIA.Visible = true;
                    trIA.Visible = true;
                    trIA1.Visible = true;

                    
                    foreach (var i in ias)
                    {
                        if(!string.IsNullOrEmpty(i.investment.client.companyName))
                        {
                            i.investment.client.surName = i.investment.client.companyName;
                            i.investment.client.otherNames = "";
                        }
                    }
                }
                else
                {
                    gridIA.Visible = false;
                    trIA.Visible = false;
                    trIA1.Visible = false;
                }
                Session["ias"] = ias;

                var sus = le.susuAccounts
                    .Where(p => (p.disbursementDate >= startDate && p.disbursementDate <= endDate) 
                        && (p.authorized == true)
                        && (p.posted == false) 
                        && p.disbursedBy != null 
                        && (p.disbursedBy.ToLower() == cboUserName.SelectedValue.ToLower())
                        && (cboFieldAgent.SelectedValue == "" || p.agentID == agentId)).ToList();
                if (sus.Count > 0)
                {
                    gridSU.DataSource = sus;
                    gridSU.DataBind();
                    gridSU.Visible = true;
                    trSU.Visible = true;
                    trSU1.Visible = true;
                }
                else
                {
                    gridSU.Visible = false;
                    trSU.Visible = false;
                    trSU1.Visible = false;
                }
                Session["sus"] = sus;

                var scs = le.susuContributions
                    .Where(p => (p.contributionDate >= startDate && p.contributionDate <= endDate)
                        && (p.posted == false) 
                        && p.cashierUsername != null 
                        && (p.cashierUsername.ToLower() == cboUserName.SelectedValue.ToLower())
                        && (cboFieldAgent.SelectedValue == "" || p.agentID == agentId)).ToList();
                if (scs.Count > 0)
                {
                    gridSC.DataSource = scs;
                    gridSC.DataBind();
                    gridSC.Visible = true;
                    trSC.Visible = true;
                    trSC1.Visible = true;
                }
                else
                {
                    gridSC.Visible = false;
                    trSC.Visible = false;
                    trSC1.Visible = false;
                }
                Session["scs"] = scs;

                var rsus = le.regularSusuAccounts
                    .Where(p => (p.disbursementDate >= startDate && p.disbursementDate <= endDate) 
                        && (p.authorized == true)
                        && (p.posted == false) 
                        && p.disbursedBy != null
                        && (p.disbursedBy.ToLower() == cboUserName.SelectedValue.ToLower())
                        && (cboFieldAgent.SelectedValue == "" || p.agentID == agentId)).ToList();
                if (rsus.Count > 0)
                {
                    gridRSU.DataSource = rsus;
                    gridRSU.DataBind();
                    gridRSU.Visible = true;
                    trRSU.Visible = true;
                    trRSU1.Visible = true;
                }
                else
                {
                    gridRSU.Visible = false;
                    trRSU.Visible = false;
                    trRSU1.Visible = false;
                }
                Session["rsus"] = rsus;

                var rscs = le.regularSusuContributions
                    .Where(p => (p.contributionDate >= startDate && p.contributionDate <= endDate)
                        && (p.posted == false) 
                        && p.cashierUsername != null
                        && (p.cashierUsername.ToLower() == cboUserName.SelectedValue.ToLower())
                        && (cboFieldAgent.SelectedValue == "" || p.agentID == agentId)).ToList();
                if (rscs.Count > 0)
                {
                    gridRSC.DataSource = rscs;
                    gridRSC.DataBind();
                    gridRSC.Visible = true;
                    trRSC.Visible = true;
                    trRSC1.Visible = true;
                }
                else
                {
                    gridRSC.Visible = false;
                    trRSC.Visible = false;
                    trRSC1.Visible = false;
                }
                Session["rscs"] = rscs;

            }
            else
            {
                btnOpen.Enabled = true;
            }
        }

        protected string GetBankName(object bankID)
        {
            var bankName = "";
            try
            {

                if (bankID != null)
                {
                    int bid = int.Parse(bankID.ToString());
                    var b = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bid);//ent.banks.FirstOrDefault(p => p.bank_id == bid);
                    if (b != null)
                    {
                        //b.bank_branchesReference.Load();
                        //b.bank_branches.banksReference.Load();
                        bankName = b.bank_branches.banks.bank_name;
                    }
                }
            }
            catch (Exception) { }
            return bankName;
        }

        protected void gridDisbursment_Load(object sender, EventArgs e)
        {
            if (Session["disb"] != null)
            {
                gridDisbursment.DataSource = Session["disb"];
            }
        }

        protected void gridReceipt_Load(object sender, EventArgs e)
        {
            if (Session["rcpt"] != null)
            {
                gridReceipt.DataSource = Session["rcpt"];
            }
        }

        protected void gridDA_Load(object sender, EventArgs e)
        {
            if (Session["das"] != null)
            {
                gridDA.DataSource = Session["das"];
            }
        }
        protected void gridCSC_Load(object sender, EventArgs e)
        {
            if (Session["csc"] != null)
            {
                gridCSC.DataSource = Session["csc"];
            }
        }
        protected void gridDWC_Load(object sender, EventArgs e)
        {
            if (Session["das"] != null)
            {
                gridDA.DataSource = Session["das"];
            }
        }

        protected void gridDA_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("depositAdditionalID").ToString());
            var disb = le.depositAdditionals.FirstOrDefault(p => p.depositAdditionalID == id);
            if (disb != null)
            {
                //disb.depositReference.Load();
                disb.deposit.principalBalance -= disb.depositAmount;
                disb.deposit.amountInvested -= disb.depositAmount;
                le.depositAdditionals.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridCSC_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("clientServiceChargeId").ToString());
            var clsc = le.clientServiceCharges.FirstOrDefault(p => p.clientServiceChargeId == id);
            if (clsc != null)
            {
                le.clientServiceCharges.Remove(clsc);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridDW_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("depositWithdrawalID").ToString());
            var disb = le.depositWithdrawals.FirstOrDefault(p => p.depositWithdrawalID == id);
            if (disb != null)
            {
                //disb.depositReference.Load();
                disb.deposit.principalBalance += disb.principalWithdrawal;
                disb.deposit.interestBalance += disb.interestWithdrawal;
                le.depositWithdrawals.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridDWC_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("depositWithdrawalID").ToString());
            var disb = le.depositWithdrawals.FirstOrDefault(p => p.depositWithdrawalID == id);
            if (disb != null)
            {
                //disb.depositReference.Load();
                disb.disInvestmentCharge = 0;
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridDW_Load(object sender, EventArgs e)
        {
            if (Session["dws"] != null)
            {
                gridDW.DataSource = Session["dws"];
            }
        }

        protected void gridSW_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("savingWithdrawalID").ToString());
            var disb = le.savingWithdrawals.FirstOrDefault(p => p.savingWithdrawalID == id);
            if (disb != null)
            {
                //disb.savingReference.Load();
                disb.saving.availablePrincipalBalance += disb.principalWithdrawal;
                disb.saving.availableInterestBalance += disb.interestBalance;

                disb.saving.principalBalance += disb.principalWithdrawal;
                disb.saving.interestBalance += disb.interestWithdrawal;
                le.savingWithdrawals.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridSW_Load(object sender, EventArgs e)
        {
            if (Session["sws"] != null)
            {
                gridSW.DataSource = Session["sws"];
            }
        }

        protected void gridSA_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("savingAdditionalID").ToString());
            var disb = le.savingAdditionals.FirstOrDefault(p => p.savingAdditionalID == id);
            if (disb != null)
            {
                //disb.savingReference.Load();
                disb.saving.availablePrincipalBalance -= disb.savingAmount;

                disb.saving.principalBalance -= disb.savingAmount;
                disb.saving.amountInvested -= disb.savingAmount;
                le.savingAdditionals.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridSA_Load(object sender, EventArgs e)
        {
            if (Session["sas"] != null)
            {
                gridSA.DataSource = Session["sas"];
            }
        }


        protected void gridIW_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("investmentWithdrawalID").ToString());
            var disb = le.investmentWithdrawals.FirstOrDefault(p => p.investmentWithdrawalID == id);
            if (disb != null)
            {
                //disb.investmentReference.Load();
                disb.investment.principalBalance += disb.principalWithdrawal;
                disb.investment.interestBalance += disb.interestWithdrawal;
                le.investmentWithdrawals.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridIW_Load(object sender, EventArgs e)
        {
            if (Session["iws"] != null)
            {
                gridIW.DataSource = Session["iws"];
            }
        }

        protected void gridIA_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("investmentAdditionalID").ToString());
            var disb = le.investmentAdditionals.FirstOrDefault(p => p.investmentAdditionalID == id);
            if (disb != null)
            {
                //disb.investmentReference.Load();
                disb.investment.principalBalance -= disb.investmentAmount;
                disb.investment.amountInvested -= disb.investmentAmount;
                le.investmentAdditionals.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridIA_Load(object sender, EventArgs e)
        {
            if (Session["ias"] != null)
            {
                gridIA.DataSource = Session["ias"];
            }
        }

        protected void gridSU_DeleteCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void gridRSU_DeleteCommand(object sender, GridCommandEventArgs e) 
        {

        }

        protected void gridSU_Load(object sender, EventArgs e)
        {
            if (Session["sus"] != null)
            {
                gridSU.DataSource = Session["sus"];
            }
        }

        protected void gridRSU_Load(object sender, EventArgs e)
        {
            if (Session["rsus"] != null)
            {
                gridRSU.DataSource = Session["rsus"];
            }
        }

        protected void gridSC_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("susuContributionID").ToString());
            var disb = le.susuContributions.FirstOrDefault(p => p.susuContributionID == id);
            if (disb != null)
            {
                le.susuContributions.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridRSC_DeleteCommand(object sender, GridCommandEventArgs e) 
        {
            GridDataItem item = e.Item as GridDataItem;

            // using DataKey
            var id = int.Parse(item.GetDataKeyValue("regularSusuContributionID").ToString());
            var disb = le.regularSusuContributions.FirstOrDefault(p => p.regularSusuContributionID == id);
            if (disb != null)
            {
                le.regularSusuContributions.Remove(disb);
                le.SaveChanges();
                OnChange();
            }
        }

        protected void gridSC_Load(object sender, EventArgs e)
        {
            if (Session["scs"] != null)
            {
                gridSC.DataSource = Session["scs"];
            }
        }

        protected void gridRSC_Load(object sender, EventArgs e)
        {
            if (Session["rscs"] != null)
            {
                gridRSC.DataSource = Session["rscs"];
            }
        }

        protected void gridDisbursment_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["disb"] != null)
            {
                gridDisbursment.DataSource = Session["disb"];
                gridDisbursment.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridReceipt_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["rcpt"] != null)
            {
                gridReceipt.DataSource = Session["rcpt"];
                gridReceipt.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridDA_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["das"] != null)
            {
                gridDA.DataSource = Session["das"];
                gridDA.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridCSC_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["csc"] != null)
            {
                gridCSC.DataSource = Session["csc"];
                gridCSC.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridDW_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["dws"] != null)
            {
                gridDW.DataSource = Session["dws"];
                gridDW.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridDWC_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["dws"] != null)
            {
                gridDWC.DataSource = Session["dws"];
                gridDWC.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridSA_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["sas"] != null)
            {
                gridSA.DataSource = Session["sas"];
                gridSA.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridSW_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["sws"] != null)
            {
                gridSW.DataSource = Session["sws"];
                gridSW.CurrentPageIndex = e.NewPageIndex;
            } 
        }

        protected void gridIA_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["ias"] != null)
            {
                gridIA.DataSource = Session["ias"];
                gridIA.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridIW_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["iws"] != null)
            {
                gridIW.DataSource = Session["iws"];
                gridIW.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridSC_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["scs"] != null)
            {
                gridSC.DataSource = Session["scs"];
                gridSC.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridSU_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["sus"] != null)
            {
                gridSU.DataSource = Session["sus"];
                gridSU.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridRSC_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["rscs"] != null)
            {
                gridRSC.DataSource = Session["rscs"];
                gridRSC.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void gridRSU_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            if (Session["rsus"] != null)
            {
                gridRSU.DataSource = Session["rsus"];
                gridRSU.CurrentPageIndex = e.NewPageIndex;
            }
        }

        protected void cboFieldAgent_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

    }
}