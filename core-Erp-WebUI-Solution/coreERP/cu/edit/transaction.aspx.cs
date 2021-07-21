using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.cu.edit
{
    public partial class transaction : corePage
    {
        public override string URL
        {
            get { return "~/cu/edit/transaction.aspx"; }
        }
    }
}
