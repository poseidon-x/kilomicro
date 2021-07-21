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

namespace coreERP.ln.reports2
{
    public partial class enquiry : corePage
    {
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/reports2/enquiry.aspx"; }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rpt != null)
            {
                try
                {
                    rpt.Dispose();
                    rpt.Close();
                    rpt = null;
                }
                catch (Exception) { }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["products"] != null)
            {
                var enquiry = Session["products"] ;
                if (enquiry != null )
                {
                    rpt = new coreReports.ln2.rptLoanEnquiry();
                    rpt.SetDataSource(enquiry);
                    rpt.Subreports[0].SetDataSource((new reportEntities()).vwCompProfs.ToList());
                     
                    rpt.SetParameterValue("amd", (double)Session["amd"]);
                    this.rvw.ReportSource = rpt;
                }
            } 
        }

    }
}
