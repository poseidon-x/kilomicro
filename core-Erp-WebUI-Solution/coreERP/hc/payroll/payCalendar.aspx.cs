using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using coreLogic;
using System.Data.Entity;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Data;

namespace coreERP.hc.payroll
{
    public partial class payCalendar : corePage
    {
        public override string URL
        {
            get { return "~/hc/payroll/payCalendar.aspx"; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RenderTree();
            }
        }

        private void RenderTree()
        {
            this.RadTreeView1.Nodes.Clear();
            RadTreeNode rootNode = new RadTreeNode("Pay Calendar", "__root__");
            rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
            rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            this.RadTreeView1.Nodes.Add(rootNode);
            var years = new int[] {2014,2015,2016,2017,2018,2019,2020 };
            foreach (var y in years)
            {
                if (y <= DateTime.Now.Year)
                {
                    RenderTree(rootNode, y);
                }
            }
            this.RadTreeView1.ExpandAllNodes();
        }

        private void RenderTree(RadTreeNode parentNode, int year)
        { 
            RadTreeNode node = new RadTreeNode(year.ToString(), "y:"+year.ToString());
            node.Visible = true;
            node.ImageUrl = "~/images/tree/folder_open.jpg";
            node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
            parentNode.Nodes.Add(node); 
            for (int m = 1; m <= 12;m++ )
            {
                RenderTree(node, year, m);
            }
        }

        private String[] months = new String[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private void RenderTree(RadTreeNode parentNode, int year, int month)
        {
            coreLoansEntities le=new coreLoansEntities();
            var pc = le.payCalendars.FirstOrDefault(p => p.month == month && p.year == year);
            if (pc != null)
            {
                RadTreeNode node = new RadTreeNode(months[month - 1], "m:" + pc.payCalendarID.ToString());
                node.Visible = true;
                node.ImageUrl = "~/images/tree/folder_open.jpg";
                node.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                parentNode.Nodes.Add(node);
            }
        }
         
        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            //RadGrid1.ExportSettings.ExportOnlyData = true;
            //RadGrid1.ExportSettings.IgnorePaging = true;
            //RadGrid1.ExportSettings.OpenInNewWindow = true;
            //this.RadGrid1.ExportSettings.FileName = "coreERP_ou_categories";
            //RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void btnWord_Click(object sender, System.EventArgs e)
        {
            //RadGrid1.ExportSettings.ExportOnlyData = true;
            //RadGrid1.ExportSettings.IgnorePaging = true;
            //RadGrid1.ExportSettings.OpenInNewWindow = true;
            //this.RadGrid1.ExportSettings.FileName = "coreERP_ou_categories";
            //RadGrid1.MasterTableView.ExportToWord();
        }

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            //RadGrid1.ExportSettings.ExportOnlyData = true;
            //RadGrid1.ExportSettings.IgnorePaging = true;
            //RadGrid1.ExportSettings.OpenInNewWindow = true;
            //this.RadGrid1.ExportSettings.FileName = "coreERP_ou_categories";
            //this.RadGrid1.ExportSettings.Pdf.Title = "Currencies Defined in System";
            //this.RadGrid1.ExportSettings.Pdf.AllowModify = false;
            //RadGrid1.MasterTableView.ExportToPdf();
        }


        protected void RadTreeView1_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            try
            {
                RadTreeNode clickedNode = e.Node;
                coreLoansEntities le = new coreLoansEntities();

                switch (e.MenuItem.Value)
                {
                    case "New":
                        if (clickedNode.Value.StartsWith("y:"))
                        {
                            RadTreeNode newCat = new RadTreeNode("New Calendar Month");
                            newCat.Selected = true;
                            newCat.ImageUrl = "~/images/ou/dept.jpg";
                            newCat.ExpandedImageUrl = "~/images/ou/dept.jpg";
                            clickedNode.Nodes.Add(newCat);
                            clickedNode.Expanded = true;

                            clickedNode.Font.Bold = true;
                            //set node's value so we can find it in startNodeInEditMode
                            newCat.Value = "-1";
                            this.cboYear.SelectedValue = clickedNode.Value.Replace("y:", "");
                            txtDaysInMonth.Value = 22;
                            Session["pcID"] = -1;
                            this.pnlEdit.Visible = true;
                        }
                        break;
                    case "Delete":
                        if (clickedNode.Value.StartsWith("m:"))
                        {
                            int pcID = int.Parse(e.Node.Value.Split(':')[1]);
                            var pcs = le.payCalendars.Where(p => p.payCalendarID == pcID).ToList();
                            if (pcs.Count == 1)
                            {
                                le.payCalendars.Remove(pcs[0]);
                                le.SaveChanges();
                            }
                            clickedNode.Remove();
                            RenderTree();
                        }
                        break;
                    case "Edit":
                         if (clickedNode.Value.StartsWith("m:"))
                        {
                            int pcID = int.Parse(e.Node.Value.Split(':')[1]);
                            var pcs = le.payCalendars.Where(p => p.payCalendarID == pcID).ToList();
                            if (pcs.Count == 1)
                            {
                                var pc = pcs[0];
                                this.cboYear.SelectedValue = pc.year.ToString();
                                this.cboMonth.SelectedValue = pc.month.ToString();
                                this.txtDaysInMonth.Value = pc.daysInMonth;
                                this.pnlEdit.Visible = true;
                                Session["pcID"] = pc.payCalendarID;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void RadTreeView1_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
        {
            core_dbEntities ent = new core_dbEntities();
            if (e.Node.Value == "-1")
            {
                ou_cat cat = new ou_cat();
                cat.cat_name = e.Text;
                cat.creation_date = DateTime.Now;
                cat.creator = User.Identity.Name;
                if (e.Node.ParentNode.Value != "__root__")
                {
                    cat.parent_ou_cat_id = int.Parse(e.Node.ParentNode.Value);
                }
                ent.ou_cat.Add(cat);
                ent.SaveChanges();
                e.Node.Value = cat.ou_cat_id.ToString();
            }
            else
            {
                int catID = int.Parse(e.Node.Value);
                var cats = (from cat in ent.ou_cat where cat.ou_cat_id == catID select cat).ToList<ou_cat>();
                if (cats.Count == 1)
                {
                    var cat = cats[0];
                    cat.cat_name = e.Text;
                    cat.modification_date = DateTime.Now;
                    cat.last_modifier = User.Identity.Name;
                    ent.SaveChanges();
                }
            }
            e.Node.Text = e.Text;
        }

        private void Clear()
        {
            txtDaysInMonth.Value = 22;
            cboMonth.SelectedValue = "";
            cboYear.SelectedValue = "";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                    coreLoansEntities le = new coreLoansEntities();
                if (Session["pcID"] != null && Session["pcID"].ToString() == "-1")
                {
                    coreLogic.payCalendar pc = new coreLogic.payCalendar();
                    pc.daysInMonth = (int)txtDaysInMonth.Value;
                    pc.year = int.Parse(cboYear.SelectedValue);
                    pc.month = int.Parse(cboMonth.SelectedValue);
                    le.payCalendars.Add(pc);
                    le.SaveChanges();
                    Clear();
                    pnlEdit.Visible = false;
                    this.RenderTree();
                }
                else
                {
                    int pcID = int.Parse(Session["pcID"].ToString());
                    var pc = le.payCalendars.FirstOrDefault(p => p.payCalendarID == pcID);
                    if (pc != null)
                    {
                        pc.daysInMonth = (int)txtDaysInMonth.Value;
                        pc.year = int.Parse(cboYear.SelectedValue);
                        pc.month = int.Parse(cboMonth.SelectedValue);
                        le.SaveChanges();
                        Clear();
                        pnlEdit.Visible = false;
                        this.RenderTree();
                    }
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void ManageException(Exception ex)
        {
            string errorMsg = "There was an error processing your request:";
            if (ex is System.Data.Entity.Core.UpdateException)
            {
                if (ex.InnerException.Message.Contains("uk_ou_name"))
                {
                    errorMsg += "\nThe Organizational Unit you are trying to create already exist.";
                }
            }
            errorMsg += "Please correct and continue or cancel.";
            divError.Style["visibility"] = "visible";
            spanError.InnerHtml = errorMsg;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            pnlEdit.Visible = false;
            RenderTree();
        }

    }
}
