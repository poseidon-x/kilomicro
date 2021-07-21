using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace coreERP.ln.loans
{
    public partial class loanEnquiry : System.Web.UI.Page
    { 
        IScheduleManager schMgr = new ScheduleManager();
        coreLogic.coreLoansEntities le;

        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!IsPostBack)
            {
                foreach (var r in le.interestTypes)
                {
                    cboInterestType.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.interestTypeName, r.interestTypeID.ToString()));
                }
                cboInterestType.SelectedValue = "1";

                foreach (var r in le.repaymentModes)
                {
                    cboRepaymentMode.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r.repaymentModeName, r.repaymentModeID.ToString()));
                }
                cboRepaymentMode.SelectedValue = "30";
                dtLoanDate.SelectedDate = DateTime.Now;
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
           coreLogic.loan ln = new coreLogic.loan();
            if (cboInterestType.SelectedValue != "" && cboRepaymentMode.SelectedValue!="" && txtAmount.Value!=null
                && txtRate.Value != null && dtLoanDate.SelectedDate!= null && txtTenure.Value!=null)
            {
                coreLogic.client cl = null;
                List<coreLogic.repaymentSchedule> sched =
                schMgr.calculateSchedule(txtAmount.Value.Value, txtRate.Value.Value,
                dtLoanDate.SelectedDate.Value, (int?)txtGracePeriod.Value, (int)txtTenure.Value.Value,
                int.Parse(cboInterestType.SelectedValue), int.Parse(cboRepaymentMode.SelectedValue), cl);



                //using (core_dbEntities ctx = new core_dbEntities())
                //{
                //    var comp = ctx.comp_prof.FirstOrDefault();
                //    if (comp.comp_name.ToLower().Contains("eclipse"))//TODO change name to TTL
                //    {
                //        sched =
                //           schMgr.calculateScheduleTTL(txtAmount.Value.Value, txtRate.Value.Value,
                //    dtLoanDate.SelectedDate.Value, (int?)txtGracePeriod.Value, (int)txtTenure.Value.Value,
                //    int.Parse(cboInterestType.SelectedValue), int.Parse(cboRepaymentMode.SelectedValue),ln);
                //    }
                //    else
                //    {
                //        sched =
                //        schMgr.calculateSchedule(txtAmount.Value.Value, txtRate.Value.Value,
                //            dtLoanDate.SelectedDate.Value, (int?)txtGracePeriod.Value, (int)txtTenure.Value.Value,
                //            int.Parse(cboInterestType.SelectedValue), int.Parse(cboRepaymentMode.SelectedValue));
                //    }
                //}




                gridSchedule.DataSource = sched;
                gridSchedule.DataBind();
            }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            if (cboInterestType.SelectedValue != "" && cboRepaymentMode.SelectedValue != "" && txtAmount.Value != null
                && txtRate.Value != null && dtLoanDate.SelectedDate != null && txtTenure.Value != null)
            {
                Response.Redirect("~/ln/loans/loan.aspx?op=new&i=" + cboInterestType.SelectedValue
                    + "&rm=" + cboRepaymentMode.SelectedValue + "&a=" + txtAmount.Value.ToString() +
                    "&r=" + txtRate.Value.ToString() + "&d=" + dtLoanDate.SelectedDate.Value.ToString("dd-MMM-yyyy")
                    + "&t=" + txtTenure.Value.ToString());
            }
        }
    }
}