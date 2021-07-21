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

namespace coreERP.cu.report
{
    public partial class register : corePage
    {          
        public override string URL
        {
            get { return "~/cu/report/register.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                Bind();
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            Bind(); 
        }

        private void Bind()
        {
            var rpt = new coreReports.telerik.cu.membershipRegister();
              
            var res2 = (new reportEntities()).vwMembers.ToList();
            var res = new List<coreReports.unionMember>();
            foreach (var r in res2)
            {
                res.Add(new unionMember
                {
                    accountNumber = r.accountNumber,
                    chapterName = r.clientName,
                    loanCount = r.loanCount,
                    loansbalance = r.loansbalance,
                    depositBalance = r.depositBalance,
                    sharesBalance = r.sharesBalance,
                    savingsBalance = r.savingsBalance,
                    joinedDate = r.joinedDate,
                    clientName = r.clientName,
                    pricePerShare = r.pricePerShare,
                    sex = r.sex,
                    addressLine1 = r.addressLine1,
                    addressLine2 = r.addressLine2,
                    cityTown = r.cityTown,
                    directions = r.directions,
                    workPhone = r.workPhone,
                    homePhone = r.homePhone,
                    mobilePhone = r.mobilePhone,
                    personalEmail = r.personalEmail,
                    officeEmail = r.officeEmail,
                    accountStatus = r.accountStatus,
                    idNumber = r.idNumber,
                    idType = r.idType,
                    DOB = r.DOB,
                    docRegistrationNumber = r.docRegistrationNumber,
                    emailAddress = r.emailAddress
                });
            }
            if (res.Count == 0)
            {
                HtmlHelper.MessageBox("Report cannot be displaid because no data was found for the provided criteria", "coreERP©: No Data", IconType.deny);
                return;
            }
            (rpt.Items[0].Items["table1"] as Telerik.Reporting.Table).DataSource = res;

            // Use the InstanceReportSource to pass the report to the viewer for displaying
            Telerik.Reporting.InstanceReportSource reportSource = new Telerik.Reporting.InstanceReportSource();
            reportSource.ReportDocument = rpt;

            // Assigning the report to the report viewer.
            rvw.ReportSource = reportSource;

            // Calling the RefreshReport method (only in WinForms applications).
            rvw.RefreshReport();
        }
    }
}
