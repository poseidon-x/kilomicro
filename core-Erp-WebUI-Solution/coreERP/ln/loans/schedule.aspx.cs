using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.ln.loans
{
    public partial class schedule : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if(!IsPostBack)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.OrderBy(p=> p.surName))
                {
                    cboClient.Items.Add(new RadComboBoxItem((cl.clientTypeID==3||cl.clientTypeID==4||cl.clientTypeID==5)?cl.companyName:cl.surName + 
                    ", " + cl.otherNames + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }
  
        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboClient.SelectedValue != "")
            {
                int clientID = int.Parse(cboClient.SelectedValue);
                cboLoan.Items.Clear();
                cboLoan.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.loans.Where(p => p.client.clientID == clientID && p.disbursementDate==null))
                {
                    cboLoan.Items.Add(new RadComboBoxItem(cl.amountRequested.ToString("#,###.#0")
                        + " ("+cl.applicationDate.ToString("dd-MMM-yyyy") +")", cl.loanID.ToString()));
                }
            }
        }

        protected void cboLoan_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboLoan.SelectedValue != "")
            {
                int loanID = int.Parse(cboLoan.SelectedValue);
                scheduleDS.WhereParameters[0].DefaultValue = loanID.ToString(); 
                this.gridSchedule.DataBind();
            }
        }

        protected void gridSchedule_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                var rpsID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["repaymentScheduleID"];
                var rps = (from p in le.repaymentSchedules where p.repaymentScheduleID == rpsID select p).FirstOrDefault();
                //rps.loanReference.Load();

                if (newVals["principalPayment"] != null && newVals["principalPayment"].ToString() != "")
                {
                    var pp = double.Parse(newVals["principalPayment"].ToString());
                    if (pp != rps.principalPayment)
                    {
                        var rps2 = le.repaymentSchedules.FirstOrDefault(p => p.loanID == rps.loanID && p.repaymentDate > rps.repaymentDate);
                        if (rps2 != null)
                        {
                            rps2.principalPayment += rps.principalPayment - pp;
                            rps2.principalBalance += rps.principalBalance - pp;
                            rps2.balanceCD = rps.balanceCD - ( rps.principalPayment - pp);
                            rps.principalPayment = pp;
                            rps.principalBalance = pp;
                            rps.balanceCD=rps.balanceCD+(rps.principalPayment - pp);
                        }
                    }
                }

                if (newVals["interestPayment"] != null && newVals["interestPayment"].ToString() != "")
                {
                    var ip = double.Parse(newVals["interestPayment"].ToString());
                    if (ip != rps.interestPayment)
                    {
                        var rps2 = le.repaymentSchedules.FirstOrDefault(p => p.loanID == rps.loanID && p.repaymentDate > rps.repaymentDate);
                        if (rps2 != null)
                        {
                            rps2.interestPayment += rps.interestPayment - ip;
                            rps2.interestBalance += rps.interestBalance - ip;
                            rps.interestPayment = ip;
                            rps.interestBalance = ip;
                        }
                    }
                }

                if (newVals["repaymentDate"] != null && newVals["interestPayment"].ToString() != "")
                {
                    var rpDate = DateTime.Parse(newVals["repaymentDate"].ToString());
                    var rpsDateMin = le.repaymentSchedules.Where(p => p.loanID == rps.loanID).Min(p => p.repaymentDate);
                    var rpsDateMax = le.repaymentSchedules.Where(p => p.loanID == rps.loanID).Max(p => p.repaymentDate);
                    if (rpDate <= rpsDateMax && rpDate>= rps.loan.applicationDate)
                    {
                        rps.repaymentDate = rpDate;
                    }
                }

                rps.edited = true;
                rps.loan.edited = true;
                le.SaveChanges();

                gridSchedule.EditIndexes.Clear();
                gridSchedule.MasterTableView.IsItemInserted = false;
                gridSchedule.MasterTableView.Rebind();
            }
            catch (Exception ex) { }
        }

    }
}