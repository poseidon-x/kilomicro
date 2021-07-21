using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class dueChecks : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        coreLogic.core_dbEntities ent;

        protected void Page_Load(object sender, EventArgs e)
        {
            le=new coreLogic.coreLoansEntities();
            ent = new coreLogic.core_dbEntities();
            if (!Page.IsPostBack)
            {
                var data = (from c in le.clients
                            from l in le.loans
                            from rp in le.loanChecks
                            where c.clientID == l.clientID
                              && l.loanID == rp.loanID
                                        && rp.cashed == false
                                        && rp.checkDate < DateTime.Now
                                        && (c.clientTypeID == 0 || c.clientTypeID == 2)
                            select new
                            {
                                c.clientID,
                                clientName=c.surName + ", " + c.otherNames,
                                c.accountNumber,
                                l.loanID,
                                l.loanNo,
                                rp.loanCheckID,
                                rp.checkNumber,
                                rp.checkDate,
                                rp.checkAmount,
                                rp.bankID
                            }).ToList();
                gridSchedule.DataSource = data;
                gridSchedule.DataBind();
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        { 
            foreach (var item in gridSchedule.Items)
            {
                if (item is GridDataItem)
                {
                    var item2 = item as GridDataItem;
                    if (item2.Selected)
                    {
                        var key = item2.GetDataKeyValue("loanCheckID").ToString();
                        var rpID = int.Parse(key);
                        var rp = le.loanChecks.FirstOrDefault(p => p.loanCheckID == rpID);
                        if (rp != null)
                        {
                            //coreLogic.LoansHelper.ApplyCheck(le, rpID, User.Identity.Name);
                        }
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}