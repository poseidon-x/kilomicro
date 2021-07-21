using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.gl.journal
{
    public partial class _default : corePage
    {
        private bool multi_cur = false;

        public override string URL
        {
            get { return "~/gl/journal/default.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cboJnlTmp.SelectedIndexChanged += new Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventHandler(cboJnlTmp_SelectedIndexChanged);
            this.cboJnl.SelectedIndexChanged += new Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventHandler(cboJnl_SelectedIndexChanged);
            if (Request.Params["mc"] != null && Request.Params["mc"].ToUpper().Trim() == "TRUE") multi_cur = true;
            if (multi_cur == true)
                lblHeader.InnerText = "Jounral Entry Transactions (Foreign)";
            else
                lblHeader.InnerText = "Jounral Entry Transactions (Local)";

            if (!IsPostBack)
            {
                //PopulateBatches(cboJnlTmp);
                //PopulateBatches2(cboJnl);
            }
        }

        protected void cboJnlTmp_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboJnlTmp.SelectedValue != null && cboJnlTmp.SelectedValue != "")
            {
                if (multi_cur == false)
                    Response.Redirect("~/gl/journal/local.aspx?op=edit&batchno=" + cboJnlTmp.SelectedValue);
                else
                    Response.Redirect("~/gl/journal/foreign.aspx?op=edit&batchno=" + cboJnlTmp.SelectedValue);
            }
        }

        protected void cboJnl_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cboJnl.SelectedValue != null && cboJnl.SelectedValue != "")
            {
                if (multi_cur == false)
                    Response.Redirect("~/gl/journal/local.aspx?op=edit&posted=true&batchno=" + cboJnl.SelectedValue);
                else
                    Response.Redirect("~/gl/journal/foreign.aspx?op=edit&posted=true&batchno=" + cboJnl.SelectedValue);
            }
        }

        protected void rdbNew_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNew.Checked == true)
            {
                if(multi_cur==false)
                    Response.Redirect("~/gl/journal/local.aspx?op=new");
                else
                    Response.Redirect("~/gl/journal/foreign.aspx?op=new");
            }
        } 

        protected void rdbEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEdit.Checked == true)
            {
               // PopulateBatches(cboJnlTmp);
            }
        }

        protected void rdbEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEdit2.Checked == true)
            {
                //PopulateBatches2(cboJnl);
            }
        }

        protected void PopulateBatches(Telerik.Web.UI.RadComboBox cboAcc, string text)
        {
            try
            {
                using (coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities())
                {
                    var batches = (from p in ent.vw_jnl_tmp
                                   from j in ent.jnl_tmp
                                   where p.jnl_batch_id == j.jnl_batch_tmp.jnl_batch_id
                                        && (multi_cur == false || p.multi_currency == true)
                                        && (p.batch_no.Contains(text) || j.ref_no.Contains(text))
                                   select p
                                    ).Distinct().Take(100).ToList<vw_jnl_tmp>();
                    if (batches.Count == 0)
                    {
                        batches = (from p in ent.vw_jnl_tmp
                                   from j in ent.jnl_tmp
                                   where p.jnl_batch_id == j.jnl_batch_tmp.jnl_batch_id
                                        && (multi_cur == false || p.multi_currency == true)
                                        && (p.batch_no.Contains(text) || j.description.Contains(text) || j.ref_no.Contains(text))
                                   select p
                                    ).Distinct().Take(100).ToList<vw_jnl_tmp>();
                    }
                    cboAcc.DataSource = batches;
                    cboAcc.DataBind();
                }
            }
            catch (Exception ex) { }
        }

        protected void PopulateBatches2(Telerik.Web.UI.RadComboBox cboAcc, string text)
        {
            try
            {
                using (coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities())
                {
                    var batches = (from p in ent.vw_jnl
                                   from j in ent.jnl
                                   where p.jnl_batch_id == j.jnl_batch.jnl_batch_id
                                        && (multi_cur == false || p.multi_currency == true)
                                        && (p.batch_no.Contains(text) || j.ref_no.Contains(text))
                                   select p
                                    ).Distinct().Take(100).ToList<vw_jnl>();
                    if (batches.Count == 0)
                    {
                        batches = (from p in ent.vw_jnl
                                   from j in ent.jnl
                                   where p.jnl_batch_id == j.jnl_batch.jnl_batch_id
                                        && (multi_cur == false || p.multi_currency == true)
                                        && (p.batch_no.Contains(text) || j.description.Contains(text) || j.ref_no.Contains(text))
                                   select p
                                    ).Distinct().Take(100).ToList<vw_jnl>();
                    }
                    cboAcc.DataSource = batches;
                    cboAcc.DataBind();
                }
            }
            catch (Exception ex) { }
        }

        protected void cboJnl_TextChanged(object sender, EventArgs e)
        {
            if (cboJnl.Text != null && cboJnl.Text != "")
            {
                if (multi_cur == false)
                    Response.Redirect("~/gl/journal/local.aspx?op=edit&posted=true&batchno=" + cboJnl.Text);
                else
                    Response.Redirect("~/gl/journal/foreign.aspx?op=edit&posted=true&batchno=" + cboJnl.Text);
            }
        }

        protected void cboJnlTmp_TextChanged(object sender, EventArgs e)
        {
            if (cboJnlTmp.Text != null && cboJnlTmp.Text != "")
            {
                if (multi_cur == false)
                    Response.Redirect("~/gl/journal/local.aspx?op=edit&batchno=" + cboJnlTmp.Text);
                else
                    Response.Redirect("~/gl/journal/foreign.aspx?op=edit&batchno=" + cboJnlTmp.Text);
            }
        }

        protected void cboJnlTmp_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                PopulateBatches(cboJnlTmp, e.Text);
            }
        }

        protected void cboJnl_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            if (e.Text.Trim().Length > 2)
            {
                PopulateBatches2(cboJnl, e.Text);
            }
        }

    }
}
