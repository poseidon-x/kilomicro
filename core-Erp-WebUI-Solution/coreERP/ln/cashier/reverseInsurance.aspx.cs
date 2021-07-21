using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;

namespace coreERP.ln.cashier
{
    public partial class reverseInsurance1 : System.Web.UI.Page
    {
        IRepaymentsManager rpmtMgr = new RepaymentsManager();
        coreLogic.coreLoansEntities le;
        coreLogic.loanInsurance revIns;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            {
                if (Request.Params["id"] != null)
                {
                    int id = int.Parse(Request.Params["id"]);
                    Session["id"] = id;
                    revIns = le.loanInsurances.FirstOrDefault(p => p.loanInsuranceID == id);
                    if (revIns != null)
                    {
                        lblAmount.Text = revIns.amount.ToString("#,###.#0");
                        lblInsuranceDate.Text = revIns.insuranceDate.ToString("dd-MMM-yyyy");

                        Session["insuranceToDelete"] = revIns;
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
                LoadInsurance(id);
            }
        }
        private void LoadInsurance(int? id)
        {
            revIns = le.loanInsurances.FirstOrDefault(p => p.loanInsuranceID == id);
            if (revIns != null)
            {
                Session["insuranceToDelete"] = revIns;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {

            int? id = null;
            if (Session["id"] != null)
            {
                id = int.Parse(Session["id"].ToString());
            }
            LoadInsurance(id);
            if (revIns != null)
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                coreLogic.IReversalManager rpmtMgr = new coreLogic.ReversalManager();

                rpmtMgr.ReverseInsurance(le, revIns.loan, ent, revIns, User.Identity.Name);
                le.SaveChanges();
                ent.SaveChanges();
                HtmlHelper.MessageBox2("Insurance Fee Reversal Data Saved Successfully!", ResolveUrl("~/ln/cashier/reverse.aspx"), "coreERP©: Successful", IconType.ok);

            }
        }
    }
}