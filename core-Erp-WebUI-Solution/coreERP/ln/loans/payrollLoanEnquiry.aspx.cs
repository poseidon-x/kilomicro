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
    public partial class payrollLoanEnquiry : System.Web.UI.Page
    {
        IScheduleManager schMgr = new ScheduleManager();
        List<coreLogic.prAllowance> allowances;
        List<Product> products;

        coreLogic.coreLoansEntities le ;
        coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                le=new coreLogic.coreLoansEntities();
                Session["le"] = le;

                allowances = new List<coreLogic.prAllowance>();
                products = new List<Product>();
                Session["allowances"] = allowances;
                Session["products"] = products;

                cboAllowanceType.Items.Add(new Telerik.Web.UI.RadComboBoxItem("", ""));
                foreach (var r in le.prAllowanceTypes.OrderBy(p=>p.allowanceTypeName).ToList())
                {
                    cboAllowanceType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.allowanceTypeName, r.allowanceTypeID.ToString()));
                }
            }
            else
            {
                le=Session["le"] as coreLogic.coreLoansEntities;
                allowances = Session["allowances"] as List<coreLogic.prAllowance>;
                products = Session["products"] as List<Product>;
                gridSchedule.DataSource = products;
                gridSchedule.DataBind();
            }
        }


        protected void txt_TextChanged(object sender, EventArgs e)
        {
            ComputeGross();
            ComputeNet();
            ComputeAMD();
        }

        private void ComputeGross()
        {
            double gross = 0;
            gross = txtBasicSalary.Value.Value;
            if (allowances.Count > 0)
            {
                gross += allowances.Where(p => p.prAllowanceType.isPermanent == true).Sum(p => p.amount);
            }
            txtGrossSalary.Value = gross;
            txtGrossSalary.Text = gross.ToString();
        }

        private void ComputeAMD()
        {
           
            double amd = 0;
            amd = txtGrossSalary.Value.Value;
            if (this.txtTax.Value != null)
            {
                amd -= txtTax.Value.Value;
            }
            if (this.txtSSWelfare.Value != null)
            {
                amd -= txtSSWelfare.Value.Value;
            }
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
                    DateTime.Now, r.loanTenure);
                if (sched.Any())
                {
                    prd.schedules = sched;
                    prd.monthlyDeduction = sched[0].principalPayment + sched[0].interestPayment;
                    products.Add(prd);
                }
                else
                {
                    HtmlHelper.MessageBox2("Repayment Schedule can not be Empty!", ResolveUrl("/ln/loans/payrollLoanEnquiry.aspx"),
                    "coreERP©: Error", IconType.ok);
                }
            }
            Session["products"] = products.OrderBy(p => p.qualifiedAmountMax);
            gridSchedule.DataSource = products.OrderBy(p=>p.qualifiedAmountMax);
            gridSchedule.DataBind();
        }

        private void ComputeNet()
        {
            double net = 0;
            net = txtGrossSalary.Value.Value;
            if (txtTotalDeductions.Value != null)
            {
                net -= txtTotalDeductions.Value.Value;
            }
            if (this.txtTotalDedNotOnPR.Value != null)
            {
                net -= txtTotalDedNotOnPR.Value.Value;
            }
            if (this.txtTax.Value != null)
            {
                net -= txtTax.Value.Value;
            }
            if (this.txtSSWelfare.Value != null)
            {
                net -= txtSSWelfare.Value.Value;
            }
            if (net < 0) net = 0;
            txtNetSalary.Value = net;
            txtNetSalary.Text = net.ToString();
        }
 
        protected void gridAllowances_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                var g = allowances[e.Item.ItemIndex];
                if (g != null)
                {
                    txtAllowanceAmount.Value = g.amount;
                    cboAllowanceType.SelectedValue = g.prAllowanceType.allowanceTypeID.ToString();

                    Session["allowance"] = g;
                    btnAddAllowance.Text = "Update Allowance";
                }
            }
            else if (e.CommandName == "DeleteItem")
            {
                allowances.RemoveAt(e.Item.ItemIndex);
            }
            gridAllowances.DataSource = allowances;
            gridAllowances.DataBind();
        }

        protected void btnAddAllowance_Click(object sender, EventArgs e)
        {
            if (cboAllowanceType.SelectedValue != "" && txtAllowanceAmount.Text != "")
            {
                if (allowances == null)
                {
                    allowances = new List<coreLogic.prAllowance>();
                }
                coreLogic.prAllowance g;
                if (btnAddAllowance.Text == "Add Allowance")
                {
                    g = new coreLogic.prAllowance();
                }
                else
                {
                    g = Session["allowance"] as coreLogic.prAllowance;
                }
                int id = int.Parse(cboAllowanceType.SelectedValue);
                if (btnAddAllowance.Text == "Add Allowance" || g.allowanceTypeID != id)
                {
                    g.prAllowanceType = le.prAllowanceTypes.FirstOrDefault(p => p.allowanceTypeID == id);
                }
                g.amount = txtAllowanceAmount.Value.Value;

                if (btnAddAllowance.Text == "Add Allowance")
                {
                    allowances.Add(g);
                }
                Session["allowances"] = allowances;
                gridAllowances.DataSource = allowances;
                gridAllowances.DataBind();

                txtAllowanceAmount.Value = null;
                cboAllowanceType.SelectedValue = "";

                btnAddAllowance.Text = "Add Allowance";
                txt_TextChanged(this, EventArgs.Empty);
            }
        }

        protected void btnRecalculate_Click(object sender, EventArgs e)
        {
            txt_TextChanged(this, EventArgs.Empty);
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
            if (txtGrossSalary.Value != null)
            { 
                Session["amd"] = txtAMD.Value.Value;
                Response.Redirect("~/ln/reports2/enquiry.aspx");
            }
        }

    }
    public class Product
    {
        public int ProductID{get;set;}
        public string ProductName{get;set;}
        public double qualifiedAmount { get; set; }
        public double qualifiedAmountMax { get; set; }
        public double monthlyDeduction { get; set; }
        public double processingFee { get; set; }
        public double netLoanAmount { get; set; }
        public string tenure{get;set;}
        public List<coreLogic.repaymentSchedule> schedules { get; set; }
    }
}