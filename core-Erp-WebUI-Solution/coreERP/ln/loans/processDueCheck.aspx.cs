using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class processDueCheck : System.Web.UI.Page
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
                
                var logs = le.loanChecks.Where(p => p.checkDate<= DateTime.Now
                    && p.balance>0).ToList(); 
                rpNotes.DataSource = logs;
                rpNotes.DataBind();
            }
        }

        protected void cboClient_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int? id = null;
             
            var logs = le.loanChecks.Where(p => p.clientID == id).ToList(); 
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
            var btnApply = sender as RadButton;
            if (btnApply != null)
            {
                if (btnApply.CommandArgument != null && btnApply.CommandArgument != "")
                {
                    int id = int.Parse(btnApply.CommandArgument);
                    var log = le.loanChecks.FirstOrDefault(p => p.loanCheckID == id);
                    if (log != null)
                    {
                        log.balance = 0;
                        le.SaveChanges();

                        var logs = le.loanChecks.Where(p => p.checkDate <= DateTime.Now
                            && p.balance > 0).ToList();
                        rpNotes.DataSource = logs;
                        rpNotes.DataBind();
                    }
                }
            }
        }

        public string GetBankName(object bankID)
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

        public string GetClientName(object cID)
        {
            if (cID != null)
            {
                var bid = int.Parse(cID.ToString());
                var b = le.clients.FirstOrDefault(p => p.clientID == bid);
                if (b != null)
                {
                    return b.surName + ", " + b.otherNames;
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