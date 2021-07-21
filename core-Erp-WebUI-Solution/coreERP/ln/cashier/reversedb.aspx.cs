using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.cashier
{
    public partial class reversedb : System.Web.UI.Page
    {
        IDisbursementsManager disbMgr = new DisbursementsManager();
        coreLogic.coreLoansEntities le ;
        coreLogic.loanTranch rp;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            { 
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    rp = le.loanTranches.FirstOrDefault(p => p.loanTranchID == id);
                    if (rp != null)
                    {
                        lblAmountPaid.Text = rp.amountDisbursed.ToString("#,###.#0");
                        lblCheckNo.Text = rp.checkNumber; 
                        lbRepaymentDate.Text = rp.disbursementDate.ToString("dd-MMM-yyyy");

                        Session["disbursementToDelete"] = rp;
                    }
                }
            }
            else
            {
                int? id = null;
                if (Session["id"] != null)
                {
                    id = int.Parse(Session["id"].ToString());
                }
                LoadTranch(id);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
           
            int? id = null;
            if (Session["id"] != null)
            {
                id = int.Parse(Session["id"].ToString());
            }
            LoadTranch(id);
            if (rp != null)
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                coreLogic.IReversalManager rpmtMgr = new coreLogic.ReversalManager();

                rpmtMgr.ReverseDisbursement(le, rp.loan, rp, ent, User.Identity.Name);
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Disbursement Reversal Data Saved Successfully!", ResolveUrl("~/ln/cashier/reverse.aspx"), "coreERP©: Successful", IconType.ok);
            }
        }

        private void LoadTranch(int? id)
        {
            rp = le.loanTranches.FirstOrDefault(p => p.loanTranchID == id);
            if (rp != null)
            {
                Session["disbursementToDelete"] = rp;
            }
        }
    }
}