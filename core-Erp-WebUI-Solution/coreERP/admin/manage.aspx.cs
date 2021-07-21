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
    public partial class manage : System.Web.UI.Page
    {

        coreSecurityEntities ent;
        protected void Page_Load(object sender, EventArgs e)
        {
            ent = new coreSecurityEntities();
            divError.Style["visibility"] = "hidden";
            if (!Page.IsPostBack)
            {
                RenderTree();
                RenderTree2();
            }
            divProc.Style["visibility"] = "hidden";
        }

        private void RenderTree()
        {
            try
            {
                this.RadTreeView1.Nodes.Clear();
                RadTreeNode rootNode = new RadTreeNode("The Company", "__root__");
                rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                this.RadTreeView1.Nodes.Add(rootNode);
                var roles = 
                    (from role in ent.roles
                     orderby role.role_name
                     select role);
                foreach (var role in roles)
                {
                    RadTreeNode node = new RadTreeNode(role.description, "r:" + role.role_name);
                    node.Visible = true;
                    node.ImageUrl = "~/images/role.jpg";
                    node.ExpandedImageUrl = "~/images/role.jpg";
                    rootNode.Nodes.Add(node);
                    RadTreeNode cNode = new RadTreeNode("Members", "ru:"+role.role_name);
                    cNode.ImageUrl = "~/images/tree/folder_open.jpg";
                    cNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    node.Nodes.Add(cNode);
                    var roleMembers = (from user in ent.users
                                       from userrole in ent.user_roles
                                       where user.user_name == userrole.users.user_name
                                        && userrole.roles.role_name == role.role_name
                                        orderby user.user_name
                                       select new
                                       {
                                           user.user_name,
                                           user.full_name,
                                           user.is_active,
                                           user.is_onLine
                                       });
                    foreach (var member in roleMembers)
                    {
                        RadTreeNode uNode = new RadTreeNode(
                        (member.full_name == null) ? member.user_name : member.full_name + " (" + member.user_name + ")"
                        + ((member.is_onLine == true) ? " <img src='../images/online.jpg' alt='Online'/>" : ""),  
                            "ruu:" + member.user_name);
                        uNode.Visible = true;
                        uNode.ImageUrl = "~/images/person.jpg";
                        uNode.ExpandedImageUrl = "~/images/person.jpg";
                        cNode.Nodes.Add(uNode);
                    }
                }
                rootNode.Expanded=true;
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        private void RenderTree2()
        {
            try
            {
                this.RadTreeView2.Nodes.Clear();
                RadTreeNode rootNode = new RadTreeNode("The Company", "__root__");
                rootNode.ImageUrl = "~/images/tree/folder_open.jpg";
                rootNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                this.RadTreeView2.Nodes.Add(rootNode);
                var users =
                    (from user in ent.users
                     orderby user.user_name
                     select user);
                foreach (var user in users)
                {
                    RadTreeNode node = new RadTreeNode(
                        (user.full_name==null)?user.user_name:user.full_name + " (" + user.user_name + ")"
                        + ((user.is_onLine==true)?" <img src='../images/online.jpg' alt='Online'/>":""), 
                        "u:" +user.user_name);
                    node.Visible = true;
                    node.ImageUrl = "~/images/person.jpg";
                    node.ExpandedImageUrl = "~/images/person.jpg";
                    rootNode.Nodes.Add(node);
                    RadTreeNode cNode = new RadTreeNode("Assigned Roles", "ur:" + user.user_name);
                    cNode.ImageUrl = "~/images/tree/folder_open.jpg";
                    cNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    node.Nodes.Add(cNode);
                    var userRoles = (from role in ent.roles
                                       from userrole in ent.user_roles
                                       where role.role_name == userrole.roles.role_name
                                        && userrole.users.user_name== user.user_name
                                        orderby role.role_name
                                       select new
                                       {
                                           role.role_name,
                                           role.description
                                       });
                    foreach (var role in userRoles)
                    {
                        RadTreeNode rNode = new RadTreeNode(role.description,
                            "urr:" + role.role_name);
                        rNode.Visible = true;
                        rNode.ImageUrl = "~/images/role.jpg";
                        rNode.ExpandedImageUrl = "~/images/role.jpg";
                        cNode.Nodes.Add(rNode);
                    }
                }
                rootNode.Expanded = true;
            }
            catch (Exception ex)
            {
                ManageException(ex);
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

                switch (e.MenuItem.Value)
                {
                    case "New":
                        RadTreeNode newCat = new RadTreeNode("New Role");
                        newCat.Selected = true;
                        newCat.ImageUrl = "~/images/role.jpg";
                        newCat.ExpandedImageUrl = "~/images/role.jpg";
                        clickedNode.Nodes.Add(newCat);
                        clickedNode.Expanded = true;

                        clickedNode.Font.Bold = true;
                        //set node's value so we can find it in startNodeInEditMode
                        newCat.Value = "-1";
                        
                        this.txtRolename.Text = "New_Role";  
                        this.txtOriginalRolename.Text = "-1";
                        this.pnlEditRole.Visible = true;
                        this.clMembers.DataBind();
                        break;
                    case "Delete":
                        string editedRolename = e.Node.Value.Split(':')[1];
                        var role = ent.roles.FirstOrDefault(p => p.role_name == editedRolename);
                        if (role != null)
                        {
                            ent.roles.Remove(role);
                            ent.SaveChanges();
                        }
                        clickedNode.Remove();
                        RenderTree();
                        break;
                    case "Edit":
                        editedRolename = e.Node.Value.Split(':')[1];
                        role = ent.roles.FirstOrDefault(p => p.role_name == editedRolename);
                        if (role != null)
                        {
                            clickedNode.Expanded = true;

                            clickedNode.Font.Bold = true;

                            this.txtRolename.Text = editedRolename;
                            this.txtRolename.Enabled = false;
                            this.txtOriginalRolename.Text = editedRolename;
                            this.txtRoleDescription.Text = role.description;
                            this.clMembers.DataBind();

                            foreach (ListItem item in clMembers.Items)
                            {
                                var ur = ent.user_roles.FirstOrDefault(p => p.users.user_name == item.Value &&
                                    p.roles.role_name == editedRolename);
                                if (ur != null)
                                {
                                    item.Selected = true;
                                }
                            }

                            this.pnlEditRole.Visible = true; 
                        }
                        break;
                }
            } 
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void RadTreeView2_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            try
            {
                RadTreeNode clickedNode = e.Node; 

                switch (e.MenuItem.Value)
                {
                    case "New":
                        RadTreeNode newCat = new RadTreeNode("New User");
                        newCat.Selected = true;
                        newCat.ImageUrl = "~/images/person.jpg";
                        newCat.ExpandedImageUrl = "~/images/person.jpg";
                        clickedNode.Nodes.Add(newCat);
                        clickedNode.Expanded = true;

                        clickedNode.Font.Bold = true;
                        //set node's value so we can find it in startNodeInEditMode
                        newCat.Value = "-1";
                        
                        this.txtUserName.Text = "New_User";  
                        this.txtOriginalUserName.Text = "-1";
                        this.pnlEditUser.Visible = true;
                        this.clRoles.DataBind();
                        break;
                    case "Delete":
                        string editedUsername = e.Node.Value.Split(':')[1];
                        var user = ent.users.FirstOrDefault(p => p.user_name == editedUsername);
                        if (user != null)
                        {
                            ent.users.Remove(user);
                            ent.SaveChanges();
                        }
                        clickedNode.Remove();
                        RenderTree2();
                        break;
                    case "Edit":
                        editedUsername = e.Node.Value.Split(':')[1];
                        user = ent.users.FirstOrDefault(p => p.user_name == editedUsername);
                        if (user != null)
                        {
                            clickedNode.Expanded = true;

                            clickedNode.Font.Bold = true;

                            this.txtUserName.Text = editedUsername;
                            this.txtUserName.Enabled = false;
                            this.txtOriginalUserName.Text = editedUsername;
                            this.txtFullname.Text = user.full_name;
                            this.txtPassword.Text = "";
                            this.clRoles.DataBind();

                            foreach (ListItem item in clRoles.Items)
                            {
                                var ur = ent.user_roles.FirstOrDefault(p => p.users.user_name == editedUsername &&
                                    p.roles.role_name == item.Value);
                                if (ur != null)
                                {
                                    item.Selected = true;
                                }
                            }

                            this.pnlEditUser.Visible = true; 
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
            this.txtRoleDescription.Text = "";
            this.txtOriginalRolename.Text = "";
            this.txtRolename.Text = "";
            this.txtRolename.Enabled = true;
            this.clMembers.ClearSelection();
        }

        private void Clear2()
        {
            this.txtOriginalUserName.Text = "";
            this.txtUserName.Text = "";
            this.txtFullname.Text = "";
            this.txtUserName.Enabled = true;
            this.clRoles.ClearSelection();
        }
       
        protected void btnSaveUser_Click(object sender, EventArgs e)
        { 
            try
            {
                if (txtOriginalUserName.Text == "-1")
                {
                    if (txtPassword.Text.Trim()=="")throw new Exception("Invalid password");
                    coreLogic.users user = new coreLogic.users();
                    user.user_name = this.txtUserName.Text;
                    user.full_name = this.txtFullname.Text;
                    user.is_active = true;
                    user.is_locked_out = false;
                    user.login_failure_count = 0;
                    user.password = EncodePassword(this.txtPassword.Text);
                    user.creation_date = DateTime.Now;

                    ent.users.Add(user);
                    foreach (ListItem item in clRoles.Items)
                    {
                        if (item.Selected == true)
                        {
                            user_roles ur = new user_roles();
                            ur.users = user;
                            ur.roles = ent.roles.FirstOrDefault(p => p.role_name == item.Value);
                            ur.creation_date = DateTime.Now;
                            ent.user_roles.Add(ur);
                        }
                    }

                    ent.SaveChanges();
                    Clear2();
                    pnlEditUser.Visible = false;
                    this.RenderTree2();
                    this.RenderTree();
                }
                else
                {
                    coreLogic.users user = ent.users.FirstOrDefault(p => p.user_name == txtOriginalUserName.Text);
                    if (user != null)
                    {
                        user.full_name = this.txtFullname.Text;
                        user.is_active = true;
                        user.is_locked_out = false;
                        user.login_failure_count = 0;
                        if (this.txtPassword.Text.Trim() != "") user.password = EncodePassword(this.txtPassword.Text);
                        user.modification_date = DateTime.Now;
                         
                        foreach (ListItem item in clRoles.Items)
                        {
                            var uro = ent.user_roles.FirstOrDefault(p => p.users.user_name == user.user_name &&
                                p.roles.role_name == item.Value);
                            if (item.Selected == true)
                            {
                                if (uro == null)
                                {
                                    user_roles ur = new user_roles();
                                    ur.users = user;
                                    ur.roles = ent.roles.FirstOrDefault(p => p.role_name == item.Value);
                                    ur.creation_date = DateTime.Now;
                                    ent.user_roles.Add(ur);
                                }
                            }
                            else
                            {
                                if (uro != null)
                                {                                    
                                    ent.user_roles.Remove(uro);
                                }
                            }
                        }

                        ent.SaveChanges();
                        Clear2();
                        pnlEditUser.Visible = false;
                        this.RenderTree2();
                        this.RenderTree();
                    }
                }
                try
                {
                    var node = RadTreeView1.FindNodeByValue("r:" + txtOriginalRolename.Text);
                    if (node != null) node.Expanded = true;
                }
                catch (Exception) { }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void btnCancelUser_Click(object sender, EventArgs e)
        {
            Clear2();
            pnlEditUser.Visible = false;
            RenderTree2();
        }

        protected void btnSaveRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtOriginalRolename.Text == "-1")
                { 
                    roles role = new roles();
                    role.role_name = this.txtRolename.Text;
                    role.description = this.txtRoleDescription.Text;
                    role.is_active = true; 
                    role.creation_date = DateTime.Now;

                    ent.roles.Add(role);
                    foreach (ListItem item in clMembers.Items)
                    {
                        if (item.Selected == true)
                        {
                            user_roles ur = new user_roles();
                            ur.roles = role;
                            ur.users = ent.users.FirstOrDefault(p => p.user_name == item.Value);
                            ur.creation_date = DateTime.Now;
                            ent.user_roles.Add(ur);
                        }
                    }

                    ent.SaveChanges();
                    Clear();
                    pnlEditRole.Visible = false;
                    this.RenderTree();
                    this.RenderTree2();
                }
                else
                {
                    roles role = ent.roles.FirstOrDefault(p => p.role_name == txtOriginalRolename.Text);
                    if (role != null)
                    {
                        role.description = this.txtRoleDescription.Text;
                        role.is_active = true;
                        role.modification_date = DateTime.Now;

                        foreach (ListItem item in clMembers.Items)
                        {
                            var uro = ent.user_roles.FirstOrDefault(p => p.roles.role_name == role.role_name &&
                                p.users.user_name == item.Value);
                            if (item.Selected == true)
                            {
                                if (uro == null)
                                {
                                    user_roles ur = new user_roles();
                                    ur.roles = role;
                                    ur.users = ent.users.FirstOrDefault(p => p.user_name == item.Value);
                                    ur.creation_date = DateTime.Now;
                                    ent.user_roles.Add(ur);
                                }
                            }
                            else
                            {
                                if (uro != null)
                                {
                                    ent.user_roles.Remove(uro);
                                }
                            }
                        }

                        ent.SaveChanges();
                        Clear();
                        pnlEditRole.Visible = false;
                        this.RenderTree2();
                        this.RenderTree();
                    }
                }
                try
                {
                    var node = RadTreeView2.FindNodeByValue("u:" + txtOriginalUserName.Text);
                    if (node != null) node.Expanded = true;
                }
                catch (Exception) { }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        protected void btnCancelRole_Click(object sender, EventArgs e)
        {
            Clear();
            pnlEditRole.Visible = false;
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
        private const string hashKey = "C50B3C89CB21F4F1422FF158A5B42D0E8DB8CB5CDA1742572A487D9401E3400267682B202B746511891C1BAF47F8D25C07F6C39A104696DB51F17C529AD3CABE";

    }
}
