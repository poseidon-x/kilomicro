using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class postCollection : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!Page.IsPostBack)
            {
                txtMonth.Value = int.Parse(DateTime.Now.Date.AddMonths(-1).ToString("yyyyMM")); 
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities(); 

            foreach (RepeaterItem item in rpPenalty.Items)
            {
                var lblID = item.FindControl("lblID") as Label;
                var txtColl = item.FindControl("txtColl") as Telerik.Web.UI.RadNumericTextBox; 
                var chkSelected = item.FindControl("chkSelected") as CheckBox; 
                if (lblID != null && txtColl != null && chkSelected != null && chkSelected.Checked == true)
                {
                    int id = int.Parse(lblID.Text);
                    var inc = le.collections.FirstOrDefault(p => p.collectionID == id);
                    if (inc != null)
                    {
                        inc.collection1 = txtColl.Value.Value;                         
                    }
                }
            } 
            le.SaveChanges(); 
            HtmlHelper.MessageBox2("Payroll Loan Collections Approved successfully!", ResolveUrl("/ln/loans/postCollection.aspx"), "coreERP©: Successful", IconType.ok);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
         

        public void CalculateIncentive(int month)
        {
            try
            {
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var lns = le.getCollection(month);
                foreach (var ln in lns)
                {
                    var lp = le.collections.FirstOrDefault(p => p.loanProductID == ln.loanProductID
                        && p.month == month);
                    if (lp == null)
                    {
                        var inc = new coreLogic.collection
                        {
                            collection1 = ln.collection,
                            month = month,
                            loanProductID = ln.loanProductID
                        };
                        le.collections.Add(inc);
                    }
                }
                le.SaveChanges();
            }
            catch (Exception x)
            {
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var id = -1;
            if (txtMonth.Value != null)
            {
                id = (int)txtMonth.Value.Value;
                CalculateIncentive(id);
            } 
            var incs = le.collections.Where(p=> p.month == id).ToList();
            foreach (var pen in incs)
            {
                //pen.loanProductReference.Load();
            }
            rpPenalty.DataSource = incs;
            rpPenalty.DataBind();
        }
    }
}