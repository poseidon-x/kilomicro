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
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace coreERP.admin
{
    public partial class modules : System.Web.UI.Page
    {

        coreSecurityEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.IsInRole("itadmin") == false)
            {
                Response.Redirect("~/security/unauthorized.aspx");
                return;
            }
            ent = new coreSecurityEntities();
            divError.Style["visibility"] = "hidden";
            if (!Page.IsPostBack)
            {
                RenderTree(); 
            }
            divProc.Style["visibility"] = "hidden";
        }

        private void RenderTree()
        {
            try
            {
                this.RadTreeView1.Nodes.Clear();
                RadTreeNode rootNode = new RadTreeNode("coreERP System", "__root__");
                rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                this.RadTreeView1.Nodes.Add(rootNode);
                var modules = 
                    (from module in ent.modules
                     where module.parent_module_id.Equals(null)
                     orderby module.sort_value, module.module_name
                     select module);
                foreach (var module in modules)
                {
                    RadTreeNode node = new RadTreeNode(module.module_name, "m:" + module.module_id.ToString());
                    node.Visible = true;
                    node.ImageUrl = "~/images/module.jpg";
                    node.ExpandedImageUrl = "~/images/module.jpg";
                    rootNode.Nodes.Add(node);
                    var chldModules = (from mod in ent.modules
                                       where mod.parent_module_id==module.module_id
                                       orderby mod.sort_value, mod.module_name
                                       select mod).ToList<coreLogic.modules>();
                    foreach (var mod in chldModules)
                    {
                        RenderTree(node, mod);
                    }
                }
                rootNode.Expanded=true;
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void RenderTree(RadTreeNode pNode, coreLogic.modules module)
        {
            RadTreeNode node = new RadTreeNode(module.module_name, "m:" + module.module_id.ToString());
            node.Visible = true;
            node.ImageUrl = "~/images/module.jpg";
            node.ExpandedImageUrl = "~/images/module.jpg";
            pNode.Nodes.Add(node);
            var chldModules = (from mod in ent.modules
                               where mod.parent_module_id == module.module_id
                               orderby mod.sort_value, mod.module_name
                               select mod).ToList<coreLogic.modules>();
            foreach (var mod in chldModules)
            {
                RenderTree(node, mod);
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
                coreSecurityEntities ent = new coreSecurityEntities();

                switch (e.MenuItem.Value)
                {
                    case "New":
                        /*RadTreeNode newCat = new RadTreeNode("New Module");
                        newCat.Selected = true;
                        newCat.ImageUrl = "~/images/module.jpg";
                        newCat.ExpandedImageUrl = "~/images/module.jpg";
                        clickedNode.Nodes.Add(newCat);
                        clickedNode.Expanded = true;

                        clickedNode.Font.Bold = true;
                        //set node's value so we can find it in startNodeInEditMode
                        newCat.Value = "-1";
                        this.txtParentModuleName.Text = clickedNode.Text;
                        this.txtModuleName.Text = "New Module";
                        var splitted =  clickedNode.Value.Split(':');
                        this.txtSortValue.Text = "0";
                        chkVisible.Checked = true;
                        this.txtParentModuleID.Text =(splitted.Length>1)?splitted[1]:""; 
                        this.txtModuleID.Text = "-1";
                        txtModuleCode.Text = "";
                        this.rptRoles.DataBind();
                        this.rptRolesDenied.DataBind();
                        this.pnlEdit.Visible = true; */
                        break;
                    case "Delete":
                        /*int moduleID = int.Parse(e.Node.Value.Split(':')[1]);
                        var modules = (from module in ent.modules 
                                       where module.module_id == moduleID 
                                       select module).ToList();
                        if (modules.Count == 1)
                        {
                            ent.Remove(modules[0]);
                            ent.SaveChanges();
                        }
                        clickedNode.Remove();
                        RenderTree();*/
                        break;
                    case "Edit":
                        int editedModuleID = int.Parse(e.Node.Value.Split(':')[1]);
                        var modules2 = (from module in ent.modules 
                                        where module.module_id == editedModuleID 
                                        select module).ToList<coreLogic.modules>();
                        if (modules2.Count == 1)
                        {
                            var module = modules2[0];
                            this.txtParentModuleName.Text = clickedNode.ParentNode.Text;
                            this.txtModuleName.Text = module.module_name;
                            this.txtURL.Text = module.url;
                            this.txtSortValue.Text = module.sort_value.ToString();
                            chkVisible.Checked = module.visible;
                            this.txtParentModuleID.Text = ((module.parent_module_id==null)?"":module.parent_module_id.Value.ToString());
                            this.txtModuleID.Text = module.module_id.ToString();
                            txtModuleCode.Text = module.module_code;
                            this.rptRoles.DataBind();
                            foreach (RepeaterItem item in rptRoles.Items)
                            {
                                var cl = item.FindControl("chkAllow") as CheckBox;
                                var lblRole = item.FindControl("lblRole") as Label;
                                var rp = ent.role_perms.FirstOrDefault(p =>
                                    p.perms.perm_code == "A"
                                    && p.roles.description == lblRole.Text
                                    && p.modules.module_id == editedModuleID);
                                if (rp != null && rp.allow == true)
                                {
                                    cl.Checked = true;
                                }
                            }
                            foreach (RepeaterItem item in rptRoles.Items)
                            {
                                var cl = item.FindControl("chkDeny") as CheckBox; 
                                var lblRole = item.FindControl("lblRole") as Label; 
                                var rp = ent.role_perms.FirstOrDefault(p =>
                                    p.perms.perm_code == "A"
                                    && p.roles.description == lblRole.Text
                                    && p.modules.module_id == editedModuleID);
                                if (rp != null && rp.allow==false)
                                {
                                    cl.Checked = true;
                                } 
                            }
                            this.pnlEdit.Visible = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

         
        private void Clear()
        {
            this.txtURL.Text = "";
            this.txtParentModuleName.Text = "";
            this.txtParentModuleID.Text = "";
            this.txtModuleName.Text = "";
            this.txtModuleID.Text = "";
            this.txtSortValue.Text = "0"; 
        }
         
        protected void btnSaveModule_Click(object sender, EventArgs e)
        {
            try
            {
                int moduleID = -1;
                if (txtModuleID.Text == "-1")
                { 
                    coreLogic.modules module = new coreLogic.modules();
                    module.module_name = txtModuleName.Text;
                    module.url = txtURL.Text;
                    module.visible = chkVisible.Checked;
                    module.creation_date = DateTime.Now;
                    module.creator = User.Identity.Name;
                    if (txtParentModuleID.Text != "") module.parent_module_id = int.Parse(txtParentModuleID.Text);
                    if (txtSortValue.Text != "") module.sort_value = byte.Parse(txtSortValue.Text);
                    module.module_code = txtModuleCode.Text;
                    ent.modules.Add(module);

                    foreach (RepeaterItem it in this.rptRoles.Items)
                    {
                        var clPerms = it.FindControl("chkAllow") as CheckBox;
                        var lblRole = it.FindControl("lblRole") as Label;
                        var rp = ent.role_perms.FirstOrDefault(p => p.modules.module_id == moduleID &&
                        p.roles.description == lblRole.Text && p.perms.perm_code == "A");
                        if (rp == null && clPerms.Checked == true)
                        {
                            role_perms perm = new role_perms();
                            perm.perms = ent.perms.First(p => p.perm_code == "A");
                            perm.roles = ent.roles.First(p => p.description == lblRole.Text);
                            perm.modules = ent.modules.First(p => p.module_id == moduleID);
                            perm.allow = true;
                            perm.creation_date = DateTime.Now;
                            perm.creator = User.Identity.Name;
                            ent.role_perms.Add(perm);
                        }
                        else if (rp != null && clPerms.Checked == false && rp.allow == true)
                        {
                            ent.role_perms.Remove(rp);
                        }
                    }

                    foreach (RepeaterItem it in this.rptRoles.Items)
                    {
                        var clPerms = it.FindControl("chkDeny") as CheckBox;
                        var lblRole = it.FindControl("lblRole") as Label;
                        var rp = ent.role_perms.FirstOrDefault(p => p.modules.module_id == moduleID &&
                        p.roles.description == lblRole.Text && p.perms.perm_code == "A");
                        if (rp == null && clPerms.Checked == true)
                        {
                            role_perms perm = new role_perms();
                            perm.perms = ent.perms.First(p => p.perm_code == "A");
                            perm.roles = ent.roles.First(p => p.description == lblRole.Text);
                            perm.modules = ent.modules.First(p => p.module_id == moduleID);
                            perm.allow = false;
                            perm.creation_date = DateTime.Now;
                            perm.creator = User.Identity.Name;
                            ent.role_perms.Add(perm);
                        }
                        else if (rp != null && clPerms.Checked == false && rp.allow == false)
                        {
                            ent.role_perms.Remove(rp);
                        }
                    }

                    ent.SaveChanges();
                    moduleID = module.module_id;
                    Clear();
                    pnlEdit.Visible = false;
                    this.RenderTree();
                }
                else
                {
                    moduleID = int.Parse(txtModuleID.Text);
                    var mods = (from module in ent.modules
                                where module.module_id == moduleID
                                select module).ToList < coreLogic.modules>();
                    if (mods.Count == 1)
                    {
                        var module = mods[0];
                        module.module_name = txtModuleName.Text;
                        module.url = txtURL.Text;
                        module.visible = chkVisible.Checked;
                        if (txtSortValue.Text != "") module.sort_value = byte.Parse(txtSortValue.Text);
                        module.modification_date = DateTime.Now;
                        module.last_modifier = User.Identity.Name;
                        module.module_code = txtModuleCode.Text;

                        foreach (RepeaterItem it in this.rptRoles.Items)
                        {
                            var clPerms = it.FindControl("chkAllow") as CheckBox;
                            var lblRole = it.FindControl("lblRole") as Label;
                            var rp = ent.role_perms.FirstOrDefault(p => p.modules.module_id == moduleID &&
                            p.roles.description == lblRole.Text && p.perms.perm_code == "A");
                            if (rp == null && clPerms.Checked == true)
                            {
                                role_perms perm = new role_perms();
                                perm.perms = ent.perms.First(p => p.perm_code == "A");
                                perm.roles = ent.roles.First(p => p.description == lblRole.Text);
                                perm.modules = ent.modules.First(p => p.module_id == moduleID);
                                perm.allow = true;
                                perm.creation_date = DateTime.Now;
                                perm.creator = User.Identity.Name;
                                ent.role_perms.Add(perm);
                            }
                            else if (rp != null && clPerms.Checked == false && rp.allow == true)
                            {
                                ent.role_perms.Remove(rp);
                            }
                        }

                        foreach (RepeaterItem it in this.rptRoles.Items)
                        {
                            var clPerms = it.FindControl("chkDeny") as CheckBox;
                            var lblRole = it.FindControl("lblRole") as Label;
                            var rp = ent.role_perms.FirstOrDefault(p => p.modules.module_id == moduleID &&
                            p.roles.description == lblRole.Text && p.perms.perm_code == "A");
                            if (rp == null && clPerms.Checked == true)
                            {
                                role_perms perm = new role_perms();
                                perm.perms = ent.perms.First(p => p.perm_code == "A");
                                perm.roles = ent.roles.First(p => p.description == lblRole.Text);
                                perm.modules = ent.modules.First(p => p.module_id == moduleID);
                                perm.allow = false;
                                perm.creation_date = DateTime.Now;
                                perm.creator = User.Identity.Name;
                                ent.role_perms.Add(perm);
                            }
                            else if (rp != null && clPerms.Checked == false && rp.allow==false)
                            {
                                ent.role_perms.Remove(rp);
                            }
                        }

                        ent.SaveChanges();
                        Clear();
                        pnlEdit.Visible = false;
                        this.RenderTree();
                    }
                }
                this.RadTreeView1.FindNodeByValue("m:" + moduleID.ToString()).ExpandParentNodes();
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void btnCancelModule_Click(object sender, EventArgs e)
        {
            Clear();
            pnlEdit.Visible = false;
            RenderTree();
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
                if (ex.InnerException.Message.Contains("uk_ou_name"))
                {
                    errorMsg += "\nThe Organizational Unit you are trying to create already exist.";
                }
            }
            errorMsg += "Please correct and continue or cancel.";
            divError.Style["visibility"] = "visible";
            spanError.InnerHtml = errorMsg;
        }

        private string EncodePassword(string password)
        {
            string encodedPassword = password;
             
            HMACSHA1 hash = new HMACSHA1();
            hash.Key = HexToByte(hashKey);
            encodedPassword =
              Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password))); 

            return encodedPassword;
        } 
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        protected void RadTreeView1_NodeClick(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try {
                int editedModuleID = int.Parse(e.Node.Value.Split(':')[1]);
                var modules2 = (from module in ent.modules
                                where module.module_id == editedModuleID
                                select module).ToList<coreLogic.modules>();
                if (modules2.Count == 1)
                {
                    var module = modules2[0];
                    this.txtParentModuleName.Text = e.Node.ParentNode.Text;
                    this.txtModuleName.Text = module.module_name;
                    txtSortValue.Text = module.sort_value.ToString();
                    this.txtURL.Text = module.url;
                    this.txtParentModuleID.Text = ((module.parent_module_id == null) ? "" : module.parent_module_id.Value.ToString());
                    this.txtModuleID.Text = module.module_id.ToString();
                    chkVisible.Checked = module.visible;
                    this.pnlEdit.Visible = true;
                    rptRoles.DataBind(); 
                    foreach (RepeaterItem item in rptRoles.Items)
                    {
                        var cl = item.FindControl("chkAllow") as CheckBox;
                        var lblRole = item.FindControl("lblRole") as Label;
                        var rp = ent.role_perms.FirstOrDefault(p =>
                            p.perms.perm_code == "A"
                            && p.roles.description == lblRole.Text
                            && p.modules.module_id == editedModuleID);
                        if (rp != null && rp.allow == true)
                        {
                            cl.Checked = true;
                        }
                    }
                    foreach (RepeaterItem item in rptRoles.Items)
                    {
                        var cl = item.FindControl("chkDeny") as CheckBox;
                        var lblRole = item.FindControl("lblRole") as Label;
                        var rp = ent.role_perms.FirstOrDefault(p =>
                            p.perms.perm_code == "A"
                            && p.roles.description == lblRole.Text
                            && p.modules.module_id == editedModuleID);
                        if (rp != null && rp.allow == false)
                        {
                            cl.Checked = true;
                        }
                    }
                } 
            }
            catch (Exception) { }
        }

        private const string hashKey = "C50B3C89CB21F4F1422FF158A5B42D0E8DB8CB5CDA1742572A487D9401E3400267682B202B746511891C1BAF47F8D25C07F6C39A104696DB51F17C529AD3CABE";

    }
}
