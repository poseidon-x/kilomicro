using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class clientCheck : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                var categoryID = Request.Params["catID"];
                if (categoryID == null) categoryID = "";
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(c => c.clientTypeID == 0 || c.clientTypeID == 2).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new RadComboBoxItem(cl.surName +
                    ", " + cl.otherNames + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }

                cboCheckType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.checkTypes)
                {
                    cboCheckType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.checkTypeName,
                        r.checkTypeID.ToString()));
                }

                cboBank.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in ent.bank_accts.ToList())
                { 
                    cboBank.Items.Add(new RadComboBoxItem(cl.bank_acct_desc, cl.bank_acct_id.ToString()));
                }

                cboSourceBank.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in ent.banks.ToList())
                {
                    cboSourceBank.Items.Add(new RadComboBoxItem(cl.bank_name, cl.bank_id.ToString()));
                } 
            }
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int? id = null;
            cboLoan.Items.Clear();
            if (cboClient.SelectedValue != "")
            {
                id = int.Parse(cboClient.SelectedValue);

                cboLoan.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.loans.Where(p=> p.clientID==id).OrderBy(p => p.disbursementDate))
                {
                    cboLoan.Items.Add(new RadComboBoxItem(cl.loanNo + " (" + cl.amountDisbursed.ToString("#,###.#0"), 
                        cl.loanID.ToString()));
                }
            }
            var logs = le.loanChecks.Where(p => p.clientID == id && p.cashed==false).ToList();
            foreach (var log in logs)
            {
                //log.clientReference.Load();
            }
            rpNotes.DataSource = logs;
            rpNotes.DataBind();
        }

        protected void cboLoan_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Bind();
        }

        protected string FormatDate(object dt)
        {
            string rtr = "";

            if (dt != null)
            {
                rtr = ((DateTime)dt).ToString("dd-MMM-yyyy");
            }

            return rtr;
        }

        private void Bind()
        {
            int? id = null;
            if (cboClient.SelectedValue != "")
            {
                id = int.Parse(cboClient.SelectedValue);
            }
            int? loanID = null;
            if (cboLoan.SelectedValue != "")
            {
                loanID = int.Parse(cboLoan.SelectedValue);
            }
            var logs = le.loanChecks.Where(p => p.clientID == id && (loanID == null || p.loanID == loanID)).ToList(); 
            rpNotes.DataSource = logs;
            rpNotes.DataBind();             
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int? id = null;
            if (cboClient.SelectedValue != "")
            {
                id = int.Parse(cboClient.SelectedValue);
            }
            int? loanID = null;
            coreLogic.loan ln = null;
            if (cboLoan.SelectedValue != "")
            {
                loanID = int.Parse(cboLoan.SelectedValue);
                ln = le.loans.FirstOrDefault(p => p.loanID == loanID);
            }
            int? staffID = null;
            var staff = le.staffs.FirstOrDefault(p => p.userName == User.Identity.Name);
            if (staff != null)
            {
                staffID = staff.staffID;
            }

            if (txtCheckNo.Text!=""
                && txtCheckAmount.Value!=null)
            {
                if (Session["id"] == null)
                {
                    var log = new coreLogic.loanCheck
                    {
                        checkAmount=txtCheckAmount.Value.Value,
                        checkNumber=txtCheckNo.Text,
                        checkDate=dtCheckDate.SelectedDate,
                        clientID = id.Value,
                        loanID = loanID,
                        cashed=false,
                        balance=txtCheckAmount.Value.Value
                    };
                    if (cboBank.SelectedValue != "")
                    {
                        log.bankID = int.Parse(cboBank.SelectedValue);
                    }
                    if (cboCheckType.SelectedValue != "")
                    {
                        log.checkTypeID = int.Parse(cboCheckType.SelectedValue);
                    }
                    if (cboSourceBank.SelectedValue != "")
                    {
                        log.sourceBankID = int.Parse(cboSourceBank.SelectedValue);
                    }
                    le.loanChecks.Add(log);
                }
                else
                {
                    int logID = int.Parse(Session["id"].ToString());
                    var log = le.loanChecks.FirstOrDefault(p => p.loanCheckID == logID);
                    if (log != null)
                    {
                        log.checkAmount = txtCheckAmount.Value.Value;
                        log.checkNumber=txtCheckNo.Text;
                        log.checkDate=dtCheckDate.SelectedDate;
                        log.clientID = id.Value;
                        log.loanID = loanID;
                        log.balance = txtCheckAmount.Value.Value;
                        log.cashed=false;

                        if (cboBank.SelectedValue != "")
                        {
                            log.bankID = int.Parse(cboBank.SelectedValue);
                        }
                        if (cboCheckType.SelectedValue != "")
                        {
                            log.checkTypeID = int.Parse(cboCheckType.SelectedValue);
                        }
                        if (cboSourceBank.SelectedValue != "")
                        {
                            log.sourceBankID = int.Parse(cboSourceBank.SelectedValue);
                        }
                    }
                    Session["id"] = null;
                }
                le.SaveChanges();
                txtCheckNo.Text = "";
                txtCheckAmount.Value = null;
                dtCheckDate.SelectedDate = null;
                cboBank.SelectedValue = "";
                pnlEdit.Visible = false;
                Bind();
                HtmlHelper.MessageBox("Client Post Dated Check Data Saved Successfully");
            }
        }

        protected void btnAddNotes_Click(object sender, EventArgs e)
        {
            if (pnlEdit.Visible == false)
            {
                Session["id"] = null;
                pnlEdit.Visible = true;
            }
        }

        protected void btnCompleted_Click(object sender, EventArgs e)
        {
            var btnCompleted = sender as RadButton;
            if (btnCompleted != null)
            {
                if (btnCompleted.CommandArgument != null && btnCompleted.CommandArgument != "")
                {
                    int id = int.Parse(btnCompleted.CommandArgument);
                    var log = le.clientActivityLogs.FirstOrDefault(p => p.clientActivityLogID == id);
                    if (log != null)
                    {
                        log.completed = true;
                        le.SaveChanges();
                        Bind();
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            var btnEdit = sender as RadButton;
            if (btnEdit != null)
            {
                if (btnEdit.CommandArgument != null && btnEdit.CommandArgument != "")
                {
                    int id = int.Parse(btnEdit.CommandArgument);
                    var log = le.loanChecks.FirstOrDefault(p => p.loanCheckID == id);
                    if (log != null)
                    {
                        txtCheckAmount.Value = log.checkAmount;
                        txtCheckNo.Text = log.checkNumber;
                        dtCheckDate.SelectedDate = log.checkDate;
                        cboBank.SelectedValue = log.bankID == null ? "" : log.bankID.ToString();
                        cboSourceBank.SelectedValue = log.sourceBankID == null ? "" : log.sourceBankID.ToString();

                        Session["id"] = id;
                        pnlEdit.Visible = true;
                    }
                }
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            var btnApply = sender as RadButton;
            if (btnApply != null)
            {
                if (btnApply.CommandArgument != null && btnApply.CommandArgument != "")
                {
                    int id = int.Parse(btnApply.CommandArgument);
                    var log = le.loanChecks.FirstOrDefault(p => p.loanCheckID == id);
                    if (log != null)
                    {
                        Response.Redirect("~/ln/cashier/applyChecks.aspx?checkID=" + log.loanCheckID.ToString());
                    }
                }
            }
        }

        protected void btnRefund_Click(object sender, EventArgs e)
        {

        }

        public string GetBankName(object bankID)
        {
            if (bankID != null)
            {
                var bid = int.Parse(bankID.ToString());
                var b = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bid);
                if (b != null)
                { 
                    return b.bank_branches.banks.bank_name;
                }
            }

            return "";
        }

        public string GetSourceBankName(object bankID)
        {
            if (bankID != null)
            {
                var bid = int.Parse(bankID.ToString());
                var b = ent.banks.FirstOrDefault(p => p.bank_id == bid);
                if (b != null)
                {
                    return b.bank_name;
                }
            }

            return "";
        }
        
        public string GetCheckType(object bankID)
        {
            if (bankID != null)
            {
                var bid = int.Parse(bankID.ToString());
                var b = le.checkTypes.FirstOrDefault(p=> p.checkTypeID==bid);
                if (b != null)
                { 
                    return b.checkTypeName;
                }
            }

            return "";
        }
    }
}