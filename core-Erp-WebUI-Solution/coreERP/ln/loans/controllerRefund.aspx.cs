using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.loans
{
    public partial class controllerRefund : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        protected void Page_Load(object sender, EventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in ent.bank_accts)
                {
                    cboBank.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.bank_acct_desc + " (" + r.bank_acct_num + ")",
                        r.bank_acct_id.ToString()));
                }

                cboPaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.modeOfPayments)
                {
                    cboPaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.modeOfPaymentName, r.modeOfPaymentID.ToString()));
                } 

                cboFile.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var file in le.controllerFiles.OrderByDescending(p=>p.fileMonth).ToList())
                {
                    cboFile.Items.Add(new Telerik.Web.UI.RadComboBoxItem(file.fileMonth.ToString() + " | " + file.fileName, 
                        file.fileID.ToString()));
                }
            }
        }

        protected void cboFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFile.SelectedValue != "")
            {
                var fileID = int.Parse(cboFile.SelectedValue);
                EntityDataSource1.WhereParameters[0].DefaultValue = fileID.ToString();
                RadGrid1.DataBind();
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (cboFile.SelectedValue !="" && dtDate.SelectedDate!= null)
            {
                var changed = false;
                coreLogic.jnl_batch batch = null;
                var pro = ent.comp_prof.FirstOrDefault();
                var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 6);
                var acctID = lt.vaultAccountID;
                int? bankID = null;
                if (cboBank != null && cboBank.SelectedValue != "")
                {
                    bankID = int.Parse(cboBank.SelectedValue);
                    var ba = ent.bank_accts.FirstOrDefault(p => p.bank_acct_id == bankID);
                    if (ba != null)
                    {
                        if (ba.accts != null)
                        {
                            acctID = ba.accts.acct_id;
                        }
                    }
                }

                foreach (Telerik.Web.UI.GridItem item in RadGrid1.SelectedItems)
                {
                    var fileID = (int)RadGrid1.MasterTableView.DataKeyValues[item.ItemIndex]["fileDetailID"];
                    var fileDetail = le.controllerFileDetails.FirstOrDefault(p => p.fileDetailID == fileID && p.refunded == false);
                    if (fileDetail.authorized == false)
                    {
                        HtmlHelper.MessageBox2("This refund has not been approved!", 
                            ResolveUrl("~/ln/loans/controllerRefund.aspx"), "coreERP©", IconType.deny);
                        return;
                    }
                    if (fileDetail != null)
                    {
                        coreLogic.jnl_batch jb = journalextensions.Post("LN", 
                            lt.refundAccountID, acctID, fileDetail.overage, "Controller Deductions Refunded",
                            pro.currency_id.Value, dtDate.SelectedDate.Value, "CONTROLLER", ent, User.Identity.Name,
                            null);
                        ent.jnl_batch.Add(jb);

                        if (batch == null)
                        {
                            batch = jb;
                        }
                        else
                        {
                            var jbs=jb.jnl.ToList();
                            batch.jnl.Add(jbs[0]);
                            batch.jnl.Add(jbs[1]);
                        } 
                        fileDetail.refunded = true;
                        changed = true;
                    }
                }

                if (changed)
                {
                    ent.jnl_batch.Add(batch);

                    le.SaveChanges();
                    ent.SaveChanges();

                    HtmlHelper.MessageBox2("Selected Deductions Refunded Successfuly!","/ln/loans/controllerRefund.aspx");
                }
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (cboFile.SelectedValue != "" )
            {
                var changed = false;
                coreLogic.jnl_batch batch = null;
                var pro = ent.comp_prof.FirstOrDefault();
                var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 6);
                var acctID = lt.holdingAccountID;

                foreach (Telerik.Web.UI.GridItem item in RadGrid1.SelectedItems)
                {
                    var fileID = (int)RadGrid1.MasterTableView.DataKeyValues[item.ItemIndex]["fileDetailID"];
                    var fileDetail = le.controllerFileDetails.FirstOrDefault(p => p.fileDetailID == fileID && p.refunded == false
                        && p.transferred==true);
                    if (fileDetail != null)
                    {
                        Response.Redirect(ResolveUrl("~/ln/loans/applyToLoan.aspx?id=" + fileDetail.fileDetailID.ToString()));
                        return;
                    }
                } 
            }
        }
    }
}