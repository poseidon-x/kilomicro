using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.ln.setup
{
    public partial class staffCategory : corePage
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public override string URL
        {
            get { return "~/ln/setup/staffCategory.aspx"; }
        }
    }
}

        