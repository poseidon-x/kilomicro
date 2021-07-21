using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.loans
{

    public partial class transferHolding : System.Web.UI.Page
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        protected void Page_Load(object sender, EventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                cboFile.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var file in le.controllerFiles.OrderByDescending(p => p.fileMonth).ToList())
                {
                    cboFile.Items.Add(new Telerik.Web.UI.RadComboBoxItem(file.fileMonth.ToString() + " | " + file.fileName,
                        file.fileID.ToString()));
                }
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (Session["fileID"] != null && dtAppDate.SelectedDate != null)
            {
                var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 6);
                var fileID = (int)Session["fileID"];

                var file = le.controllerFiles.FirstOrDefault(p => p.fileID == fileID);
                var pro = ent.comp_prof.FirstOrDefault();
                foreach (Telerik.Web.UI.GridItem item in RadGrid1.SelectedItems)
                {
                    var fileDetailID = (int)RadGrid1.MasterTableView.DataKeyValues[item.ItemIndex]["fileDetailID"];
                    var detail = le.controllerFileDetails.FirstOrDefault(p => p.fileDetailID == fileDetailID && p.transferred == false);
                    if (detail != null)
                    {
                        if (detail.overage > 0)
                        {
                            coreLogic.jnl_batch jb = journalextensions.Post("LN", lt.holdingAccountID, lt.refundAccountID,
                                detail.overage,
                                "Controller Refund Candidates - " + detail.employeeName,
                                pro.currency_id.Value, dtAppDate.SelectedDate.Value, "CONTROLLER", ent, User.Identity.Name,
                                null);
                            ent.jnl_batch.Add(jb);
                        }
                        detail.transferred = true;
                        detail.authorized = true;
                    }
                }

                le.SaveChanges();
                ent.SaveChanges();

                HtmlHelper.MessageBox2("Refund Candidates Transferred Successfully.", "/ln/loans/transferHolding.aspx");
            }
        }

        protected void cboFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (cboFile.SelectedValue != "")
            {
                var fileID = int.Parse(cboFile.SelectedValue);
                Session["fileID"] = fileID;

                var file = le.controllerFiles.FirstOrDefault(p => p.fileID == fileID);
                //file.controllerFileDetails.Load();
                var total = file.controllerFileDetails.Sum(p => p.monthlyDeduction);
                var totalUnApplied = (
                        from d in file.controllerFileDetails
                        where d.transferred==false && d.overage>0
                        select d.overage
                    ).Sum(); 
                lblApplied.Text = (total-totalUnApplied).ToString("#,##0.#0");
                lblRefund.Text = totalUnApplied.ToString("#,##0.#0");
                lblTotal.Text = total.ToString("#,##0.#0");

                EntityDataSource1.WhereParameters[0].DefaultValue = fileID.ToString();
                RadGrid1.DataBind();
            }
        }

        protected void btnPost2_Click(object sender, EventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
            if (Session["fileID"] != null && dtAppDate.SelectedDate != null)
            {
                var lt = le.loanTypes.FirstOrDefault(p => p.loanTypeID == 6);
                var fileID = (int)Session["fileID"];

                var file = le.controllerFiles.FirstOrDefault(p => p.fileID == fileID);
                var pro = ent.comp_prof.FirstOrDefault();
                foreach (Telerik.Web.UI.GridItem item in RadGrid1.SelectedItems)
                {
                    var fileDetailID = (int)RadGrid1.MasterTableView.DataKeyValues[item.ItemIndex]["fileDetailID"];
                    var detail = le.controllerFileDetails.FirstOrDefault(p => p.fileDetailID == fileDetailID && p.transferred == false);
                    if (detail != null)
                    { 
                        detail.transferred = true;
                    }
                }

                le.SaveChanges();
                ent.SaveChanges();

                HtmlHelper.MessageBox2("Refund Candidates Transferred Successfully.", "/ln/loans/transferHolding.aspx");
            }
        }

    }
}