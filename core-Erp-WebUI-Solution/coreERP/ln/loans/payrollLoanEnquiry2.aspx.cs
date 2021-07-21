using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace coreERP.ln.loans
{
    public partial class payrollLoanEnquiry2 : System.Web.UI.Page
    {
        IScheduleManager schMgr = new ScheduleManager();
        List<Product> products;

        coreLogic.coreLoansEntities le = new coreLogic.coreLoansEntities();
        coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["le"] = le;
                 
                products = new List<Product>(); 
                Session["products"] = products;                
            }
            else
            { 
                products = Session["products"] as List<Product>;
                gridSchedule.DataSource = products;
                gridSchedule.DataBind();
            }
        }


        protected void btnRecalculate_Click(object sender, EventArgs e)
        {
            txt_TextChanged(this, EventArgs.Empty);
        }

        protected void txt_TextChanged(object sender, EventArgs e)
        {  
            ComputeAMD();
        }
 
        private void ComputeAMD()
        {
            
            double amd = 0;
            amd = txtNetSalary.Value.Value; 
            amd /= 2.0;
            if (txtTotalDeductions.Value != null)
            {
                amd -= txtTotalDeductions.Value.Value;
            }
            if (this.txtTotalDedNotOnPR.Value != null)
            {
                amd -= txtTotalDedNotOnPR.Value.Value;
            }
            if (amd < 0) amd = 0;
            txtAMD.Value = amd;
            txtAMD.Text = amd.ToString();

            products=new List<Product>();
            foreach (var r in le.loanProducts)
            {
                var amt = ComputeApproved(r.loanProductID,90);
                var ded = amt / r.loanTenure;
                var amt2 = ComputeApproved(r.loanProductID, 95);
                var pf = Math.Ceiling(r.procFeeRate * amt/100.0);
                var net = amt - pf;

                var prd = new Product { 
                    qualifiedAmount=amt,
                    qualifiedAmountMax=amt2,
                    monthlyDeduction=ded,
                    ProductName=r.loanProductName,
                    tenure=r.loanTenure.ToString() + " Months",
                    ProductID=r.loanProductID,
                    processingFee=pf,
                    netLoanAmount=net
                };

                List<coreLogic.repaymentSchedule> sched =
                    schMgr.calculateScheduleM(amt, r.rate,
                    DateTime.Now,r.loanTenure);
                prd.schedules=sched;
                prd.monthlyDeduction = sched[0].principalPayment + sched[0].interestPayment;

                products.Add(prd);
            }
            Session["products"] = products.OrderBy(p => p.qualifiedAmountMax);
            gridSchedule.DataSource = products.OrderBy(p => p.qualifiedAmountMax);
            gridSchedule.DataBind();
        }
  
        private double ComputeApproved(int loanProductID, double basePerc)
        {
            double approved = 0.0;

            var lp = le.loanProducts.FirstOrDefault(p => p.loanProductID == loanProductID);
            if (lp != null)
            {
                approved = txtAMD.Value.Value * basePerc / 100.0;
                approved = Math.Ceiling((approved * lp.loanTenure) / (1 + ((lp.rate / 100.0) * lp.loanTenure)));
            }

            return approved;
        }

        protected void gridSchedule_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            int productID = (int)e.DetailTableView.ParentItem.GetDataKeyValue("ProductID");
            var prd = products.FirstOrDefault(p=>p.ProductID==productID);
            e.DetailTableView.DataSource = prd.schedules;
        }

        protected void gridSchedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (products != null)
            {
                gridSchedule.DataSource = products;
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            if (txtAMD.Value != null)
            {
                Session["amd"] = txtAMD.Value.Value;
                Response.Redirect("~/ln/reports2/enquiry.aspx");
            }
        }

    }
}