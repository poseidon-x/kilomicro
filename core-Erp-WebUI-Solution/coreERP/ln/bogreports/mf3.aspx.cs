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
using coreERP.code;

namespace coreERP.ln.bogreports
{
    public partial class mf3 : corePage
    { 
        ReportDocument rpt;
        public override string URL
        {
            get { return "~/ln/bogreports/mf3.aspx"; }
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
            if (!IsPostBack)
            {
                Bind();
            }
        }
        
        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            var re = new coreReports.reportEntities();
            rpt = new coreReports.ln.rptTenorRates();
            var res = re.vwDepositTenors.ToList();
            var res2 = re.vwLoanTenors.ToList();
            var res3 = re.vwLoanTenor2.ToList();

            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource(res2);
            rpt.Subreports[1].SetDataSource(res3);
            rpt.Subreports[2].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());

            rpt.SetParameterValue("companyName", Settings.companyName);
            rvw.ReportSource = rpt;
        }
    }
}
