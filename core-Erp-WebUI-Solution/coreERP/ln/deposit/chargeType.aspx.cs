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
    public partial class chargeType : corePage
    { 
        public override string URL
        {
            get { return "~/ln/deposit/chargeType.aspx"; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        { 
        }
          
    }
}
