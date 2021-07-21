using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.momo.setup
{
    public partial class momoProvider : corePage
    {
        public override string URL
        {
            get { return "~/momo/setup/momoProvider.aspx"; }
        }
    }
}
