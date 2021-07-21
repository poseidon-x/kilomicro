using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.investment
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
                foreach (var cl in le.clients.Where(p => p.clientTypeID == 3).OrderBy(p => p.surName))
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
                    var pen = le.investmentSchedules.FirstOrDefault(p => p.investmentScheduleID == id);
                    if (pen != null && pen.authorized == false && pen.expensed == true)
                    {
                        if (txtPrincipal.Value.Value > pen.investment.principalBalance)
                        {
                            HtmlHelper.MessageBox("Principal Authorization Exceeds Principal Balance of Investment!");
                            return;
                        }
                        if (txtInterest.Value.Value > pen.investment.interestBalance)
                        {
                            HtmlHelper.MessageBox("Interest Authorization Exceeds Interest Balance of Investment!");
                            return;
                        }
                        pen.investment.interestAuthorized += txtInterest.Value.Value;
                        pen.investment.principalAuthorized += txtPrincipal.Value.Value;
                        pen.investment.modification_date = DateTime.Now;
                        pen.investment.last_modifier = User.Identity.Name;
                        pen.authorized = true;
                    }
                }
            }
            le.SaveChanges();
            HtmlHelper.MessageBox2("Interest/Principal Authorization Data Saved Successfully!", ResolveUrl("~/ln/investment/authorizePayment.aspx"), "coreERP©: Successful", IconType.ok);  
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
                    var pen = le.investmentSchedules.FirstOrDefault(p => p.investmentScheduleID == id);
                    if (pen != null)
                    {
                        //pen.investmentReference.Load();
                        pen.investment.interestAuthorized = 0;
                        pen.investment.principalAuthorized = 0;
                        pen.investment.modification_date = DateTime.Now;
                        pen.investment.last_modifier = User.Identity.Name;
                        //pen.investment.clientReference.Load();

                    }
                }
            }
            le.SaveChanges();
            HtmlHelper.MessageBox2("Interest/Principal Authorization Data Cancelled Successfully!", ResolveUrl("~/ln/investment/authorizePayment.aspx"), "coreERP©: Successful", IconType.ok);  
        }

        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var id = -1;
            if (cboClient.SelectedValue != "")
            { 
                id = int.Parse(cboClient.SelectedValue);
                CalculateInterest(id);
            }
            var pens = le.investmentSchedules.Where(p=> (p.investment.principalBalance > 0 || p.investment.interestBalance > 0)
                    && p.investment.client.clientID == id && p.authorized==false && p.expensed==true).ToList();
            foreach (var pen in pens)
            {
                //pen.investmentReference.Load();
                //pen.investment.clientReference.Load(); 
            }
            rpPenalty.DataSource = pens;
            rpPenalty.DataBind();
        }

        public void CalculateInterest(int clientID)
        {
            try
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var lns = le.investments.Where(p => (p.principalBalance > 0 || p.interestBalance>0)
                    && p.client.clientID == clientID).ToList();
                foreach (var ln in lns)
                {
                    //ln.clientReference.Load();
                    //ln.investmentSchedules.Load();
                } 
            }
            catch (Exception x)
            {
            }
        }
    }
}