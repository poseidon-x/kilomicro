using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using Telerik.Web.UI;
using System.Collections;

namespace coreERP.ln.setup
{
    public partial class cashiersTill : System.Web.UI.Page
    {
        core_dbEntities ent;
        coreLoansEntities le;
        public string URL
        {
            get { return "~/ln/setup/cashiersTill.aspx"; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new core_dbEntities();
            le=new coreLoansEntities();
        }
         
        protected void RadGrid1_ItemInserted(object source, Telerik.Web.UI.GridInsertedEventArgs e)
        {
            //throw e.Exception;
        }

        protected void cboGLAcc_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                var cbo = sender as RadComboBox;
                if (e.Text.Trim().Length > 2 && cbo != null)
                {
                    using (core_dbEntities ent = new core_dbEntities())
                    {
                        var accs = (from a in ent.vw_accounts
                                    from c in ent.currencies
                                    where (a.acc_name.Contains(e.Text) || a.cat_name.Contains(e.Text) || a.head_name1.Contains(e.Text)
                                        || a.head_name2.Contains(e.Text) || a.head_name3.Contains(e.Text))
                                        && (a.currency_id == c.currency_id)
                                    select new
                                    {
                                        a.acct_id,
                                        a.acc_num,
                                        a.acc_name,
                                        major_name = c.major_name,
                                        a.fullname
                                    }).ToList();
                        cbo.DataSource = accs;
                        cbo.DataBind();
                        cbo.DataTextField = "fullname";
                        cbo.DataValueField = "acct_id";
                    }
                }
            }
            catch (Exception ex) { }
        } 

        protected void RadGrid1_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.InitInsertCommandName)
            {
                e.Canceled = true;
                var newVals = new System.Collections.Specialized.ListDictionary(); 
                newVals["accountID"] = 0;
                newVals["userName"] = ""; 
                e.Item.OwnerTableView.InsertItem(newVals);
            }
        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanTypes";
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanTypes";
            RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            this.RadGrid1.ExportSettings.FileName = "coreERP_loanTypes";
            this.RadGrid1.ExportSettings.Pdf.Title = "loanTypes Defined in System";
            this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            RadGrid1.MasterTableView.ExportToPdf();
        }

        public string AccountName(int accID)
        {
           var item = (from m in ent.accts
                        where m.acct_id == accID
                        select new
                        {
                            m.acc_num,
                            m.acc_name
                        }).FirstOrDefault();
            return item == null ? "" : item.acc_num + " - " + item.acc_name;
        }

        public string UserName(string  userName)
        {
            coreSecurityEntities sec = new coreSecurityEntities();
            var item = (from m in sec.users
                        where m.user_name==userName
                        select new
                        {
                            m.user_name,
                            m.full_name
                        }).FirstOrDefault();
            return item == null ? "" : item.full_name + " (" + item.user_name + ")";
        }

        public string RepyamentModeName(object accID)
        {
            var rpID = 0;
            if (accID != null) rpID = int.Parse(accID.ToString());
            var item = (from m in le.repaymentModes
                        where m.repaymentModeID == rpID
                        select new
                        {
                            m.repaymentModeName
                        }).FirstOrDefault();
            return item == null ? "" : item.repaymentModeName;
        }

        protected void PopulateAccounts(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                int? id = null;
                if (ViewState[cboAcc.ValidationGroup] != null)
                {
                    id = int.Parse(ViewState[cboAcc.ValidationGroup].ToString());
                }
                var accs = (from a in ent.vw_accounts
                            from c in ent.currencies
                            where (a.currency_id == c.currency_id)
                                && (a.acct_id == id)
                            select new
                            {
                                a.acct_id,
                                a.acc_num,
                                a.acc_name,
                                major_name = c.major_name,
                                a.fullname
                            }).ToList(); ;
                cboAcc.Items.Clear();
                cboAcc.DataSource = accs;
                cboAcc.DataBind();
                if (accs.Count() > 0)
                {
                    cboAcc.SelectedValue = id.ToString();
                }
            }
            catch (Exception ex) { }
        }

        protected void PopulateUsers(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                coreSecurityEntities sec = new coreSecurityEntities();
                var accs = (from a in sec.users 
                            orderby a.full_name
                            select new
                            {
                                a.full_name,
                                a.user_name
                            });
                cboAcc.Items.Clear(); 
                foreach (var cur in accs)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = cur.full_name + " (" + cur.user_name + ")" ;
                    item.Value = cur.user_name;
                     
                    cboAcc.Items.Add(item);

                }
                cboAcc.Items.Insert(0, new RadComboBoxItem("", "0"));
            }
            catch (Exception ex) { }
        }

        protected void PopulateRepaymentModes(Telerik.Web.UI.RadComboBox cboAcc)
        {
            try
            {
                var accs = (from a in le.repaymentModes 
                            select new
                            {
                                a.repaymentModeName,
                                a.repaymentModeID 
                            });
                cboAcc.Items.Clear(); 
                foreach (var cur in accs)
                {
                    RadComboBoxItem item = new RadComboBoxItem();

                    item.Text = cur.repaymentModeName ;
                    item.Value = cur.repaymentModeID.ToString();
                     
                    cboAcc.Items.Add(item);

                }
                cboAcc.Items.Insert(0, new RadComboBoxItem("", "0"));
            }
            catch (Exception ex) { }
        }

        void cboAcc_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var rcb = o as RadComboBox;
            if (rcb != null)
            {
                Session[rcb.ValidationGroup] = e.Value;
            }
        }

        public void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode == true)
            {
                var item = e.Item as GridEditableItem; 
                int? id = null;
                try
                {
                    var val = item.OwnerTableView.DataKeyValues[item.ItemIndex]["cashiersTillID"];
                    if (val != null) { id = int.Parse(val.ToString()); }
                }
                catch (Exception) { }
                le = new coreLoansEntities();
                var lt = le.cashiersTills.FirstOrDefault(p=> p.cashiersTillID == id); 
                var rcb = item["accountID"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    rcb.ValidationGroup = "accountID"; 
                    if (lt != null && Session[rcb.ValidationGroup] == null)
                    {
                        Session[rcb.ValidationGroup] = lt.accountID;
                    }
                    PopulateAccounts(rcb);
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAcc_SelectedIndexChanged);
                    //rcb.DataBind();
                }
                rcb = item["userName"].Controls[1] as RadComboBox;
                if (rcb != null)
                {
                    PopulateUsers(rcb);
                    rcb.ValidationGroup = "userName";
                    rcb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cboAcc_SelectedIndexChanged);
                    //rcb.DataBind();
                } 
            }
        }

        protected void RadGrid1_UpdateCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true; 
                var cashiersTillID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["cashiersTillID"];
                var b = le.cashiersTills.FirstOrDefault(p=> p.cashiersTillID==cashiersTillID);
                if(Session["accountID"] != null)
                {
                    b.accountID=int.Parse(Session["accountID"].ToString());
                } 
                if (Session["userName"] != null)
                {
                    b.userName =  Session["userName"].ToString();
                } 
                le.SaveChanges();
                RadGrid1.EditIndexes.Clear();
                RadGrid1.MasterTableView.IsItemInserted = false;
                RadGrid1.MasterTableView.Rebind();
                Session["accountID"] = null;
                Session["userName"] = null;           
            }
            catch (Exception ex) { }
        }

        protected void RadGrid1_InsertCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Hashtable newVals = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newVals, (GridEditableItem)e.Item);
                e.Canceled = true;
                coreLogic.cashiersTill b = new coreLogic.cashiersTill();
                if (Session["accountID"] != null)
                {
                    b.accountID = int.Parse(Session["accountID"].ToString());
                }
                if (Session["userName"] != null)
                {
                    b.userName = Session["userName"].ToString();
                }

                le.cashiersTills.Add(b);
                le.SaveChanges();

                RadGrid1.EditIndexes.Clear();
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Item.OwnerTableView.Rebind();

                Session["accountID"] = null;
                Session["userName"] = null; 
                    
            }
            catch (Exception ex) {  }
        }

    }
}
