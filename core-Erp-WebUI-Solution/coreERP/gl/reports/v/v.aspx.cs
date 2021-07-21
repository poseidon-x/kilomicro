using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreReports;
using coreLogic;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Telerik.Web.UI;
using coreERP.code;

namespace coreERP.gl.reports.v
{
    public partial class v : corePage
    {

        ReportDocument rpt;

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

        public override string URL
        {
            get { return "~/gl/reports/v/v.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var batchno=Request.Params["batchno"] ?? "";
                Session["batchno"] = batchno;
                PopulateBatches();
                if (!String.IsNullOrEmpty(batchno))
                {
                    Bind(batchno);
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            //var date = DateTime.Parse(cboPeriod.SelectedValue);
            //var period = int.Parse(cboPeriod.Text);
            var batchno=cboBatch.SelectedValue;
            Session["batchno"] = batchno;
            Bind(batchno);
            //this.rvw.DataBind();
        }

        protected void rvw_DataBinding(object sender, EventArgs e)
        {
            if (Session["batchno"] != null && Session["batchno"]!= "")
            {
                var batchno =  Session["batchno"].ToString(); 
                Bind(batchno);
            }
        }

        private void Bind(string batchno)
        {
            rpt = new coreReports.gl.v.rptV();
            var res = (new reportEntities()).get_v(batchno).ToList<vw_v>();
            rpt.SetDataSource(res);
            rpt.Subreports[0].SetDataSource((new coreReports.reportEntities()).vwCompProfs.ToList());
            rpt.SetParameterValue("companyName", Settings.companyName);
            rpt.SetParameterValue("reportTitle", ((res[0].v_type == "C") ? "Receipt Voucher" : "Payment Voucher"));
            rpt.SetParameterValue("compAddr", Settings.compAddr);
            rpt.SetParameterValue("compCity", Settings.compCity);
            rpt.SetParameterValue("compPhone", Settings.compPhone);
            this.rvw.ReportSource = rpt;
        }

        protected void PopulateBatches()
        {
            try
            {
                using (var gent = new coreGLEntities())
                {
                    using (var ent = new core_dbEntities())
                    {
                        var res = (from h in gent.v_head
                                   from d in gent.v_dtl
                                   where h.v_head_id == d.v_head_id
                                   select new coreERP.code.Res
                                   {
                                       batch_no = h.batch_no,
                                       tx_date = d.tx_date,
                                       recipient = "",
                                       sup_id = h.sup_id,
                                       cust_id = h.cust_id,
                                       emp_id = h.emp_id,
                                       v_type = h.v_type
                                   }
                                 ).Distinct().OrderBy(p => p.batch_no).ToList<coreERP.code.Res>();
                        foreach (var r in res)
                        {
                            if (r.v_type == "C")
                            {
                                var c = ent.custs.FirstOrDefault(p => p.cust_id == r.cust_id);
                                if (c != null)
                                {
                                    r.recipient = c.cust_name;
                                }
                            }
                            else if (r.v_type == "S")
                            {
                                var c = ent.sups.FirstOrDefault(p => p.sup_id == r.sup_id);
                                if (c != null)
                                {
                                    r.recipient = c.sup_name;
                                }
                            }
                        }
                        cboBatch.DataSource = res;
                        cboBatch.DataBind();
                        if (Session["batchno"] == null || Session["batchno"] == "")
                        {
                            cboBatch.Items.Insert(0, new RadComboBoxItem("Select A Batch", null));
                        }
                        else
                        {
                            cboBatch.SelectedValue = Session["batchno"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        { 
            if (Session["batchno"] != null)
            {
                var batchno = Session["batchno"].ToString();
                using (var gent = new coreGLEntities())
                {
                    var batch = gent.v_head.FirstOrDefault(p => p.batch_no == batchno);
                    if (batch != null)
                    {
                        if (batch.posted == true)
                        {
                            Response.Redirect("~/gl/v/v.aspx?op=edit&batchno=" + Session["batchno"].ToString() + "&posted=true");
                        }
                        else {
                            Response.Redirect("~/gl/v/v.aspx?op=edit&batchno=" + Session["batchno"].ToString());
                        }
                    }
                    else
                    {
                        Response.Redirect("~/gl/v/v.aspx");
                    }
                }
            }
            else
            {
                Response.Redirect("~/gl/v/v.aspx");
            }
        }

    }
}