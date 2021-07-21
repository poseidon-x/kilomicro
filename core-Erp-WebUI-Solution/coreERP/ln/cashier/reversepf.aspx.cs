using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.cashier
{
    public partial class reversepf : System.Web.UI.Page
    {
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        coreLogic.coreLoansEntities le ;
        coreLogic.loanFee rp;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            { 
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    rp = le.loanFees.FirstOrDefault(p => p.loanFeeID == id);
                    if (rp != null)
                    {
                        lblAmount.Text = rp.feeAmount.ToString("#,###.#0");  
                        lbFeeDate.Text = rp.feeDate.ToString("dd-MMM-yyyy");

                        Session["feeToDelete"] = rp;
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
                LoadFee(id);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            
            int? id = null;
            if (Session["id"] != null)
            {
                id = int.Parse(Session["id"].ToString());
            }
            LoadFee(id);
            if (rp != null)
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                coreLogic.IReversalManager rpmtMgr = new coreLogic.ReversalManager();

                rpmtMgr.ReverseFee(le, rp.loan, ent, rp, User.Identity.Name);
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Processing Fee Reversal Data Saved Successfully!", ResolveUrl("~/ln/cashier/reverse.aspx"), "coreERP©: Successful", IconType.ok);

            }
        }

        private void LoadFee(int? id)
        {
            rp = le.loanFees.FirstOrDefault(p => p.loanFeeID == id);
            if (rp != null)
            {
                Session["feeToDelete"] = rp;
            }
        }
    }
}