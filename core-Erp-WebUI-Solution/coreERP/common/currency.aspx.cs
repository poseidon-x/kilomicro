using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;

namespace coreERP.common
{
    public partial class currency : corePage
    {
        public override string URL
        {
            get { return "~/common/currency.aspx"; }
        }
    }
}