using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data;
using coreReports;
using Telerik.Web.UI;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;

namespace coreERP.gl.accounts
{
    public partial class close : corePage
    {
        public override string URL
        {
            get { return "~/gl/accounts/post.aspx"; }
        }

        core_dbEntities ent;
        jnl_batch_tmp batch;
        private List<get_bal_sht_for_close_Result> results;

        bool doubleClickFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        { 
            try
            {
                divError.Style["visibility"] = "hidden";
           
                PopulateCostCenters();
            }
            catch (Exception x){
                ManageException(x);
            }
        }

        protected void PopulateCostCenters()
        {
            try
            {
                coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                var cc = (from a in ent.vw_gl_ou
                          select a);
                cboCC.DataSource = cc.ToList();
                cboCC.DataBind();
                cboCC.Items.Insert(0, new RadComboBoxItem("Cost Center", null));
            }
            catch (Exception ex) { }
        } 

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_journal_entries";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_journal_entries";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_journal_entries";
            this.RadGrid1.ExportSettings.Pdf.Title = "Currencies Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }
   
        private string toPlural(string item)
        {
            var rtr = item;
            var lastDigit = item.Substring(item.Length - 1, 1);
            var lastDigit2 = item.Substring(item.Length - 2, 2);
            if (lastDigit == "s" || lastDigit2 == "es") { }
            else if (lastDigit == "y" || lastDigit == "i")
            {
                rtr = item.Substring(0, item.Length - 1);
            }
            else if (lastDigit2 == "ch" || lastDigit2 == "sh")
            {
                rtr = item + "es";
            }
            else
            {
                rtr = item + "s";
            }
            return rtr;
        }

        private void ManageException(Exception ex)
        {
            string errorMsg = "There was an error processing your request:";
            if (ex is System.Data.Entity.Core.UpdateException)
            {
                if (ex.InnerException.Message.Contains("uk_acct_cat_name") ||
                    ex.InnerException.Message.Contains("uk_acct_cat_code"))
                {
                    errorMsg += "<br />The Main Account Head you are trying to create already exist.";
                }
                if (ex.InnerException.Message.Contains("uk_acct_cat_max_acct_num") ||
                    ex.InnerException.Message.Contains("uk_acct_cat_min_acct_num"))
                {
                    errorMsg += "<br />The Account number range specified overlaps another account head.";
                }
            }
            errorMsg += "Please correct and continue or cancel.";
            divError.Style["visibility"] = "visible";
            divError.Style["color"] = "red";
            spanError.InnerHtml = errorMsg;
        }

        private double totalDr;
        private double totalCr;
        public void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridCommandItem)
                {
                    var addButton = e.Item.FindControl("AddNewRecordButton") as Button;
                    addButton.Visible = false;
                    var lnkButton = (LinkButton) e.Item.FindControl("InitInsertButton");
                    lnkButton.Visible = false;
                }
                if (e.Item is GridFooterItem)
                {
                    var footerItem = e.Item as GridFooterItem;
                    footerItem["loc_end_bal"].Text = totalDr.ToString("#,###.##;(#,###.##);0");
                    footerItem["frgn_end_bal"].Text = totalCr.ToString("#,###.##;(#,###.##);0");
                }
            }
            catch (Exception x) { }
        }

        protected void btnFetch_Click(object sender, EventArgs e)
        {
            int? ccID = null;
            if (cboCC.SelectedValue != "")
            {
                ccID = int.Parse(cboCC.SelectedValue);
            }
            if (CanClose())
            {
                var rent = new reportEntities();
                rent.Database.CommandTimeout = 60000;
                results = rent.get_bal_sht_for_close(dtTransactionDate.SelectedDate.Value.AddYears(-50),
                                            dtTransactionDate.SelectedDate.Value.MonthEnd(), true).ToList();

                RadGrid1.DataSource = results;
                totalDr = (double)(from r in results
                                   where (r.cat_code == 1 || r.cat_code == 5 || r.cat_code == 6 || r.cat_code == 8)
                                         && r.loc_end_bal > 0
                                   select r.loc_end_bal
                          ).Sum()
                          +
                          (double)(from r in results
                                   where !(r.cat_code == 1 || r.cat_code == 5 || r.cat_code == 6 || r.cat_code == 8)
                                         && r.loc_end_bal < 0
                                   select -r.loc_end_bal
                          ).Sum();
                totalCr = (double)(from r in results
                                   where !(r.cat_code == 1 || r.cat_code == 5 || r.cat_code == 6 || r.cat_code == 8)
                                         && r.loc_end_bal > 0
                                   select r.loc_end_bal
                          ).Sum()
                          +
                          (double)(from r in results
                                   where (r.cat_code == 1 || r.cat_code == 5 || r.cat_code == 6 || r.cat_code == 8)
                                         && r.loc_end_bal < 0
                                   select -r.loc_end_bal
                          ).Sum();
                RadGrid1.DataBind();
                Session["closingResults"] = results;
                Session["closingDr"] = totalDr;
                Session["closingCr"] = totalCr;

                if (Math.Round(totalDr, 2) == Math.Round(totalCr, 2))
                {
                    btnClose.Enabled = true;
                }
                else
                {
                    btnClose.Enabled = false;
                }
            }
            else
            {
                divError.Style["visibility"] = "visible";
                spanError.InnerHtml = "A later closed period exist";
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ent = new core_dbEntities();

            results = Session["closingResults"] as List<get_bal_sht_for_close_Result>;
            totalDr = (double)Session["closingDr"];
            totalCr = (double)Session["closingCr"];

            if (results != null && Math.Round(totalDr, 2) == Math.Round(totalCr, 2))
            {
                var period = new acct_period();
                var date = dtTransactionDate.SelectedDate.Value.MonthEnd();
                var fmo = (from p in ent.comp_prof select p.fmoy).Single();
                var accPeriod = date.AccountingPeriod(fmo);
                period.close_date = date;
                period.creator = Context.User.Identity.Name;
                period.creation_date = DateTime.Now;
                period.acct_period1 = accPeriod;
                ent.acct_period.Add(period);

                foreach (var bal in results)
                {
                    var accBal = new acct_bals()
                    {
                        acct_id = bal.acct_id.Value,
                        acct_period = accPeriod,
                        buy_rate = (double)bal.cur_rate,
                        currency_id = bal.currency_id.Value,
                        frgn_bal = (double)bal.frgn_end_bal,
                        loc_bal = (double)bal.loc_end_bal,
                        sell_rate = (double)bal.cur_rate,
                        creator = Context.User.Identity.Name,
                        creation_date = DateTime.Now
                    };
                    ent.acct_bals.Add(accBal);
                }
                ent.close_period(accPeriod, date);

                ent.SaveChanges();

                results.Clear();
                totalCr = 0;
                totalDr = 0;
                RadGrid1.DataSource = results;
                divError.Style["visibility"] = "visible";
                divError.Style["color"] = "green";
                divError.InnerHtml = "Period " + accPeriod.ToString() + " closed successfully";
                //spanError = System.Drawing.Color.Green;
                RadGrid1.DataBind();

            }
        }

        protected void Date_Changed(object sender, EventArgs e)
        {
            if (!CanClose())
            {
                divError.Style["visibility"] = "visible";
                divError.Style["color"] = "red";
                spanError.InnerHtml = "A later closed period exist";
            }
            else
            {
                divError.Style["visibility"] = "hidden";
                spanError.InnerHtml = "";
            }
        }

        protected bool CanClose()
        {
            if (dtTransactionDate.SelectedDate.HasValue)
            {
                return dtTransactionDate.SelectedDate.Value.MonthEnd().CanClose();
            }

            return false;
        }

        protected decimal Debit(object cc, object bb)
        {
            decimal balance = 0;
            if (cc == null || bb == null || decimal.TryParse(bb.ToString(), out balance)==false) return 0;
            var catCode = (int)cc; 
            if(catCode==1 || catCode==5 || catCode==6 || catCode==8)
            {
                if(balance >0)
                {
                    return balance;
                }
            }
            else if (!(catCode == 1 || catCode == 5 || catCode == 6 || catCode == 8))
            {
                if (balance < 0)
                {
                    return -balance;
                }
            }

            return 0;
        }

        protected decimal Credit(object cc, object bb)
        {
            decimal balance = 0;
            if (cc == null || bb == null || decimal.TryParse(bb.ToString(), out balance) == false) return 0;
            var catCode = (int)cc; 
            if (catCode == 1 || catCode == 5 || catCode == 6 || catCode == 8)
            {
                if (balance < 0)
                {
                    return -balance;
                }
            }
            else if (!(catCode == 1 || catCode == 5 || catCode == 6 || catCode == 8))
            {
                if (balance > 0)
                {
                    return balance;
                }
            }

            return 0;
        }
    }
}
