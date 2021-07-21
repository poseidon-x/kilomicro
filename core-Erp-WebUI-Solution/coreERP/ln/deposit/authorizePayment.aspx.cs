using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.deposit
{
    public partial class authorizePayment : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 1 || p.clientTypeID == 2|| p.clientTypeID == 5 || p.clientTypeID == 6).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new Telerik.Web.UI.RadComboBoxItem((cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5) ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtPrincipal = item.FindControl("txtPrincipal") as Telerik.Web.UI.RadNumericTextBox;
                var txtInterest = item.FindControl("txtInterest") as Telerik.Web.UI.RadNumericTextBox;
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                var dtDate = item.FindControl("dtDate") as RadDatePicker;
                if (lblID != null && txtInterest != null && 
                    txtPrincipal != null && chkSelected != null && chkSelected.Checked == true
                    && dtDate != null)
                {
                    int id = int.Parse(lblID.Text);
                    var pen = le.depositSchedules.FirstOrDefault(p => p.depositScheduleID == id);
                    if (pen != null && pen.authorized == false && pen.expensed == true)
                    {
                        if (txtPrincipal.Value.Value > pen.deposit.principalBalance)
                        {
                            HtmlHelper.MessageBox("Principal Authorization Exceeds Principal Balance of Deposit!");
                            return;
                        }
                        if (txtInterest.Value.Value > pen.deposit.interestBalance)
                        {
                            HtmlHelper.MessageBox("Interest Authorization Exceeds Interest Balance of Deposit!");
                            return;
                        }
                        pen.deposit.interestAuthorized += txtInterest.Value.Value;
                        pen.deposit.principalAuthorized += txtPrincipal.Value.Value;
                        pen.deposit.modification_date = DateTime.Now;
                        pen.deposit.last_modifier = User.Identity.Name;
                        pen.authorized = true;
                    }
                }
            }
            le.SaveChanges();
            HtmlHelper.MessageBox2("Interest/Principal Authorization Data Saved Successfully!", ResolveUrl("~/ln/deposit/authorizePayment.aspx"), "coreERP©: Successful", IconType.ok);  
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var pro = ent.comp_prof.FirstOrDefault();
            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtPrincipal = item.FindControl("txtPrincipal") as Telerik.Web.UI.RadNumericTextBox;
                var txtInterest = item.FindControl("txtInterest") as Telerik.Web.UI.RadNumericTextBox;
                var chkSelected = item.FindControl("chkSelected") as CheckBox;
                if (lblID != null && txtInterest != null &&
                    txtPrincipal != null && chkSelected != null && chkSelected.Checked == true)
                {
                    int id = int.Parse(lblID.Text);
                    var pen = le.depositSchedules.FirstOrDefault(p => p.depositScheduleID == id);
                    if (pen != null)
                    {
                        //pen.depositReference.Load();
                        pen.deposit.interestAuthorized = 0;
                        pen.deposit.principalAuthorized = 0;
                        pen.deposit.modification_date = DateTime.Now;
                        pen.deposit.last_modifier = User.Identity.Name;
                        //pen.deposit.clientReference.Load();

                    }
                }
            }
            le.SaveChanges();
            HtmlHelper.MessageBox2("Interest/Principal Authorization Data Cancelled Successfully!", ResolveUrl("~/ln/deposit/authorizePayment.aspx"), "coreERP©: Successful", IconType.ok);  
        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var id = -1;
            if (cboClient.SelectedValue != "")
            { 
                id = int.Parse(cboClient.SelectedValue);
                CalculateInterest(id);
            }
            var pens = le.depositSchedules.Where(p=> (p.deposit.principalBalance > 0 || p.deposit.interestBalance > 0)
                    && p.deposit.client.clientID == id && p.authorized==false && p.expensed==true).ToList();
            foreach (var pen in pens)
            {
                //pen.depositReference.Load();
                //pen.deposit.clientReference.Load(); 
            }
            rpPenalty.DataSource = pens;
            rpPenalty.DataBind();
        }

        public void CalculateInterest(int clientID)
        {
            try
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var lns = le.deposits.Where(p => (p.principalBalance > 0 || p.interestBalance>0)
                    && p.client.clientID == clientID).ToList();
                foreach (var ln in lns)
                {
                    //ln.clientReference.Load();
                    //ln.depositSchedules.Load();
                } 
            }
            catch (Exception x)
            {
            }
        }
    }
}