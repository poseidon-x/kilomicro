using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class writeOff : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;

        string categoryID = null;
        protected void cboClient_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int clientID = -1;
            if (cboClient.SelectedValue != "")
            {
                clientID = int.Parse(cboClient.SelectedValue);
            }
            var data = (from c in le.clients
                        from l in le.loans
                        from rp in le.repaymentSchedules
                        where c.clientID == l.clientID
                          && l.loanID == rp.loanID
                                    && (rp.proposedInterestWriteOff > 0 || (l.balance<10 && rp.interestBalance>0))
                                    && l.clientID == clientID 
                        select new
                        {
                            c.clientID,
                            clientName = c.surName + ", " + c.otherNames,
                            c.accountNumber,
                            l.loanID,
                            l.loanNo,
                            proposedInterestWriteOff=(rp.proposedInterestWriteOff==0)?rp.interestBalance:
                                rp.proposedInterestWriteOff,
                            rp.repaymentDate,
                            rp.repaymentScheduleID,
                            rp.principalBalance,
                            rp.interestWritenOff,
                            rp.interestPayment,
                            rp.interestBalance,
                            rp.balanceCD,
                            rp.balanceBF,
                            rp.principalPayment
                        }).ToList();
            gridSchedule.DataSource = data;
            gridSchedule.DataBind();
            
            foreach (var item in gridSchedule.Items)
            {
                if (item is GridDataItem)
                {
                    var item2 = item as GridDataItem;
                    var txt = item2.FindControl("txtAmount") as RadNumericTextBox;
                    var lblDate = item2.FindControl("txtDate") as Label;
                    if (lblDate != null )
                    {
                        var key = item2.GetDataKeyValue("repaymentScheduleID").ToString();
                        var rpID = int.Parse(key);

                        DateTime? date = null;
                        try
                        {
                            date = (from l in le.loans
                                    from rp in le.repaymentSchedules
                                    from rs in le.loanRepayments
                                    where l.loanID == rp.loanID && l.loanID == rs.loanID && rp.repaymentScheduleID == rpID
                                    select rs.repaymentDate).Max();
                        }
                        catch (Exception) { }
                        if (date != null)
                        {
                            lblDate.Text = date.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            lblDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            le=new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            categoryID = Request.Params["catID"];
            if (categoryID == null) categoryID = "1";
            if (!Page.IsPostBack)
            {
                cboClient.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.clients.Where(
                    p => ((categoryID != "5" && p.categoryID != 5) || (categoryID == "5" && p.categoryID == 5))).OrderBy(p => p.surName))
                {
                    cboClient.Items.Add(new RadComboBoxItem(cl.surName +
                    ", " + cl.otherNames + " (" + cl.accountNumber + ")", cl.clientID.ToString()));
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            bool changed=false;
            coreLogic.jnl_batch batch = null;
            var pro = ent.comp_prof.FirstOrDefault();

            foreach (var item in gridSchedule.Items)
            {
                if (item is GridDataItem)
                {
                    var item2 = item as GridDataItem;
                    var txt = item2.FindControl("txtAmount") as RadNumericTextBox;
                    var lblDate = item2.FindControl("txtDate") as Label ;
                    var date = DateTime.ParseExact(lblDate.Text, "dd-MMM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
                    if (txt != null && txt.Value != null && item2.Selected==true)
                    {                         
                        var key = item2.GetDataKeyValue("repaymentScheduleID").ToString();
                        var rpID = int.Parse(key);
                        var rp = le.repaymentSchedules.FirstOrDefault(p => p.repaymentScheduleID == rpID);
                        if (rp != null)
                        {
                            var amount =  txt.Value.Value;

                            var user = (new coreLogic.coreSecurityEntities()).users.First(p => p.user_name.ToLower().Trim() == User.Identity.Name.ToLower().Trim());
                            if (user.accessLevel.approvalLimit < amount)
                            {
                                HtmlHelper.MessageBox("The amount to be approved is beyond your access level",
                                                            "coreERP©: Failed", IconType.deny);
                                return;
                            }
                   
                            rp.interestWritenOff = txt.Value.Value;
                            rp.interestBalance -= amount;
                            if (rp.interestBalance < 0) rp.interestBalance = 0;
                            changed=true; 

                            var lw = new coreLogic.loanIterestWriteOff
                            {
                                writeOffAmount=txt.Value.Value,
                                creation_date=DateTime.Now,
                                creator=User.Identity.Name,
                                writeOffDate=date
                            };
                            rp.loan.loanIterestWriteOffs.Add(lw);

                            if (batch == null)
                            {
                                batch = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                                    rp.loan.loanType.accountsReceivableAccountID, amount,
                                    "Loan Interest Write off - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                                    pro.currency_id.Value, (rp.repaymentDate < date) ? rp.repaymentDate : date, rp.loan.loanNo, ent, User.Identity.Name,
                                rp.loan.client.branchID);
                            }
                            else
                            {
                                var batch2 = journalextensions.Post("LN", rp.loan.loanType.unearnedInterestAccountID,
                                    rp.loan.loanType.accountsReceivableAccountID, amount,
                                    "Loan Interest Write off - " + rp.loan.client.surName + "," + rp.loan.client.otherNames,
                                    pro.currency_id.Value, (rp.repaymentDate < date) ? rp.repaymentDate : date, rp.loan.loanNo, ent, User.Identity.Name,
                                rp.loan.client.branchID);
                                var j = batch2.jnl.ToList();
                                if (j.Count > 1)
                                {
                                    batch.jnl.Add(j[0]);
                                    batch.jnl.Add(j[1]);
                                }
                            }
                            rp.proposedInterestWriteOff = 0;
                        }
                    }
                }
            }

            if(changed)
            {
                ent.jnl_batch.Add(batch);
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Loan Interest Write-off Data Saved Successfully!", ResolveUrl("~/ln/loans/writeOff.aspx"), "coreERP©: Successful", IconType.ok);   
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnAll_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}