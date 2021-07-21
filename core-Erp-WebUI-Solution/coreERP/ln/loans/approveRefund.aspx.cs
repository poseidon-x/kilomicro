using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class approveRefund : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;

        protected void Page_Load(object sender, EventArgs e)
        {
            le=new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                var data = le.multiPaymentClients.Where(p => p.approved == false && p.balance>0.9).ToList();
                foreach (var dat in data)
                {
                    foreach (var mp in dat.multiPayments)
                    {
                        var ln = le.loans.FirstOrDefault(p => p.loanID == mp.loanID);
                        if (ln != null)
                        {
                            var cl = ln.client;
                            if (cl != null)
                            {
                                dat.clientName = (cl.clientTypeID == 3 || cl.clientTypeID == 4 || cl.clientTypeID == 5)
                                        ? cl.companyName : ((cl.clientTypeID == 6) ? cl.accountName : cl.surName +
                                    ", " + cl.otherNames) + " (" + cl.accountNumber + ")";
                                break;
                            }
                        }
                    }
                }
                gridSchedule.DataSource = data;
                gridSchedule.DataBind();
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            bool chnged = false;
            foreach (var item in gridSchedule.Items)
            {
                if (item is GridDataItem)
                {
                    var item2 = item as GridDataItem;
                    if (item2.Selected)
                    {
                        var key = item2.GetDataKeyValue("multiPaymentClientID").ToString();
                        var rpID = int.Parse(key);
                        var rp = le.multiPaymentClients.FirstOrDefault(p => p.multiPaymentClientID == rpID);
                        if (rp != null)
                        {
                            rp.approved = true;
                            chnged = true;
                        }
                    }
                }
            }

            if (chnged == true)
            {
                le.SaveChanges();
                HtmlHelper.MessageBox2("Loan refunds successfully apporved",
                    ResolveUrl("/ln/loans/approveRefund.aspx"), "coreERP©: Failed", IconType.deny);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}