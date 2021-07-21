using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.cashier
{
    public partial class reverserp : System.Web.UI.Page
    {
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        coreLogic.coreLoansEntities le ;
        coreLogic.loanRepayment rp;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            { 
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    rp = le.loanRepayments.FirstOrDefault(p => p.loanRepaymentID == id);
                    if (rp != null)
                    { 
                        lblAmountPaid.Text = rp.amountPaid.ToString("#,###.#0");
                        lblCheckNo.Text = rp.checkNo;
                        lblTypeOfRepayment.Text = rp.repaymentType.repaymentTypeName;
                        lbRepaymentDate.Text = rp.repaymentDate.ToString("dd-MMM-yyyy");

                        Session["repaymentToDelete"] = rp;
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
                LoadRepayment(id);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
           
            int? id = null;
            if (Session["id"] != null)
            {
                id = int.Parse(Session["id"].ToString());
            }
            LoadRepayment(id);
            if (rp != null)
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                coreLogic.IReversalManager rpmtMgr = new coreLogic.ReversalManager();

                rpmtMgr.ReversePayment(le, rp.loan, ent, rp, User.Identity.Name);
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Repayment Reversal Data Saved Successfully!", ResolveUrl("~/ln/cashier/reverse.aspx"), "coreERP©: Successful", IconType.ok);
            }
        }

        private void LoadRepayment(int? id)
        {
            rp = le.loanRepayments.FirstOrDefault(p => p.loanRepaymentID == id);
            if (rp != null)
            { 
                Session["repaymentToDelete"] = rp;
            }
        }
    }
}