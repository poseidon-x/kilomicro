using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.loans
{
    public partial class payrollLoanEnquiry3 : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cboTenure.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.loanProducts.OrderBy(p => p.loanTenure))
                {
                    cboTenure.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.loanProductName, 
                        r.loanProductID.ToString()));
                }
            }
        }

        protected void txtPrinc_TextChanged(object sender, EventArgs e)
        {
            if (txtPrinc.Value != null && cboTenure.SelectedValue != "")
            {
                int id = int.Parse(cboTenure.SelectedValue);
                var r = le.loanProducts.FirstOrDefault(p => p.loanProductID == id);
                if (r != null)
                {
                    txtProcFee.Value = Math.Ceiling(r.procFeeRate * txtPrinc.Value.Value / 100.0);
                    txtNet.Value = txtPrinc.Value.Value - txtProcFee.Value.Value;

                    txtInt.Value = Math.Ceiling(r.rate * txtPrinc.Value.Value * r.loanTenure / 100.0);
                    txtTotal.Value = txtPrinc.Value + txtInt.Value;

                    txtMD.Value = Math.Ceiling(txtTotal.Value.Value / r.loanTenure);
                    txtTD.Value = txtMD.Value * r.loanTenure;

                    txtCheck.Value = txtTD.Value - txtTotal.Value;
                }
            }
        }
    }
}