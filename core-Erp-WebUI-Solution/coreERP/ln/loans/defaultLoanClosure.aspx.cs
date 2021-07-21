using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Web.Services;
using System.Data;
using coreReports;

namespace coreERP.ln.loans
{
    public partial class defaultLoanClosure : System.Web.UI.Page
    {
        coreLogic.coreLoansEntities le;
        string categoryID = null;
        private int serialNumber = 0;

        public string GetSerialNumber()
        {
            serialNumber = serialNumber + 1;

            return serialNumber.ToString("#,##0");
        }

        protected void Page_Load(object sender, EventArgs e)
        { 
            le = new coreLogic.coreLoansEntities();
            if (!IsPostBack)
            {
                if (Request.QueryString["clientId"] != null)
                {
                    int clientId = int.Parse(Request.QueryString["clientId"]);
                    LoanLoans(clientId);
                }
            }
        }

        protected void LoanLoans(int clientId)
        {
            try
            { 
                var cl = le.clients.FirstOrDefault(p => p.clientID == clientId);
                if (cl != null)
                {
                    var loans = (
                        from l in le.loans
                        where l.clientID == cl.clientID
                            && l.closed==false
                        select new LoanClosureModel
                        {
                            amountApproved = l.amountApproved,
                            amountDisbursed = l.amountDisbursed,
                            amountRequested = l.amountRequested,
                            principalBalance = 0.0,
                            interestBalance = 0.0,
                            totalBalance = 0.0,
                            loanNo = l.loanNo,
                            disbursementDate = l.disbursementDate ?? l.applicationDate,
                            loanID = l.loanID
                        }
                        ).OrderByDescending(p => p.disbursementDate)
                        .ToList();

                    using (var rent = new reportEntities())
                    {
                        foreach (var l in loans)
                        {
                            var rs = rent.vwLoanActualSchedules.Where(p => p.date <= DateTime.Now);
                            var intBal = 0.0;
                            var princBal = l.amountDisbursed;
                            if (rs.Any())
                            {
                                intBal = rs.Sum(p => p.interest - p.interestPaid);
                                princBal -= rs.Sum(p => p.principalPaid);
                            }
                            l.principalBalance = princBal;
                            l.interestBalance = intBal;
                            l.totalBalance = princBal + intBal;
                        }
                    }
                    grid.DataSource = loans;
                    grid.DataBind();
                }
            }
            catch (Exception) { }
        }
         
        public class LoanClosureModel
        {
            public double amountApproved { get; set; }
            public double amountDisbursed { get; set; }
            public double amountRequested { get; set; }
            public double principalBalance { get; set; }
            public double interestBalance { get; set; }
            public double totalBalance { get; set; }
            public string loanNo { get; set; }
            public DateTime disbursementDate { get; set; }
            public int loanID { get; set; }
        }
 
    }
}