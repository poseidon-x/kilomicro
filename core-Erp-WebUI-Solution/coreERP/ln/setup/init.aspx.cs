using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.setup
{
    public partial class init : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnInit_Click(object sender, EventArgs e)
        {
            coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
            if (chkAsset.Checked == true)
            {
                le.initAsset();
            }
            if (chkStaff.Checked == true)
            {
                le.initStaff();
            }
            if (chkLoan.Checked == true)
            {
                le.initLoan();
            }
            if (chkClient.Checked == true)
            {
                le.initClient();
            }
            if (chkJournal.Checked == true)
            {
                le.initJournal();
            }
            if (chkAccount.Checked == true)
            {
                le.initAccounts();
            }
            Response.Redirect("~/");
        }
    }
}