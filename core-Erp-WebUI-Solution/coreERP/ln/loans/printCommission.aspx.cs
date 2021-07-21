using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using coreReports;
using coreLogic;
using Telerik.Web.UI;

namespace coreERP.ln.reports
{
    public partial class printCommission : corePage
    {
        public override string URL
        {
            get { return "~/ln/reports/printCommission.aspx"; }
        }

        coreLogic.coreLoansEntities le;
        protected void Page_Load(object sender, EventArgs e)
        {
            le = new coreLogic.coreLoansEntities();
            if (!IsPostBack)
            {
                cboAgent.Items.Add(new RadComboBoxItem("", ""));
                foreach (var cl in le.agents.OrderBy(p => p.surName))
                {
                    cboAgent.Items.Add(new RadComboBoxItem(cl.surName +
                    ", " + cl.otherNames + " (" + cl.agentNo + ")", cl.agentID.ToString()));
                }

                dtEnd.SelectedDate = DateTime.Now;
                dtStart.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            var list = Session["list"] as List<coreReports.ln2.Incentive>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    Session["list"] = list;
                    ReportDocument rpt = new coreReports.ln.rptCommission();
                    rpt.SetDataSource(list);
                    rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                    this.rvw.ReportSource = rpt; 
                }
            }
        }

        protected void cboAgent_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            OnChange();
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            List<coreReports.ln2.Incentive> list = new List<coreReports.ln2.Incentive>();
            foreach (ListItem item in cblLoans.Items)
            {
                if (item.Selected == true)
                {
                    var id = int.Parse(item.Value);
                    var ln = le.loanIncentives.FirstOrDefault(p => p.loanIncentiveID == id);
                    if (ln != null)
                    {
                        //ln.agentReference.Load();
                        //ln.loanReference.Load();
                        //ln.loan.clientReference.Load();
                        list.Add(new coreReports.ln2.Incentive
                        {
                            amount = ln.netCommission,
                            agentName = ln.agent.surName + ", " + ln.agent.otherNames
                            + " (" + ln.agent.agentNo + ")",
                            clientName = ln.loan.client.surName + ", " + ln.loan.client.otherNames
                            + " (" + ln.loan.client.accountNumber + ")",
                            invoiceDate = ln.incetiveDate.Value,
                            invoiceDescription = "Deal Commission for: " + ln.loan.client.surName + ", " + ln.loan.client.otherNames
                            + " (" + ln.loan.client.accountNumber + ")",
                            receiptID = "COM-" + ln.loan.loanNo + "-" + ln.loanIncentiveID.ToString(),
                            netLoanAmount=ln.loanAmount,
                            witholding=ln.withHoldingAmount,
                            commission=ln.commissionAmount
                        });
                    }
                }
            }
            if (list.Count>0)
            {
                Session["list"] = list;
                ReportDocument rpt = new coreReports.ln.rptCommission();
                rpt.SetDataSource(list);
                rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                    this.rvw.ReportSource = rpt;
                    this.rvw.DataBind(); 
            }
        }

        protected void dtStart_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            cblLoans.Items.Clear();
            if (cboAgent.SelectedValue != "" && dtStart.SelectedDate != null && dtEnd.SelectedDate!=null)
            {
                var sd = dtStart.SelectedDate.Value;
                var ed = dtEnd.SelectedDate.Value;
                var id = int.Parse(cboAgent.SelectedValue);
                var lns = le.loanIncentives.Where(p => p.agentID == id && p.incetiveDate>= sd && p.incetiveDate<= ed).ToList();
               
                foreach (var cl in lns)
                {
                    if (cl.netCommission > 0)
                    {
                        //cl.loanReference.Load();
                        //cl.loan.clientReference.Load();
                        cblLoans.Items.Add(new ListItem(cl.loan.client.surName +
                        ", " + cl.loan.client.otherNames + " (" + cl.netCommission.ToString("#,###.#0") + ")", cl.loanIncentiveID.ToString()));
                    }
                }
            }
        }
    }
}
