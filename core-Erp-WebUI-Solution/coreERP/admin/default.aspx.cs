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
    public partial class _default : System.Web.UI.Page
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
                RenderTree2();
            }
            divProc.Style["visibility"] = "hidden";
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
                var roles =
                    (from role in ent.roles
                     orderby role.role_name
                     select role);
                foreach (var role in roles)
                {
                    RadTreeNode node = new RadTreeNode(
                        (role.description==null)?role.role_name:role.description + " (" + role.role_name + ")" , 
                        "u:" +role.role_name);
                    node.Visible = true;
                    node.ImageUrl = "~/images/person.jpg";
                    node.ExpandedImageUrl = "~/images/person.jpg";
                    rootNode.Nodes.Add(node);
                    RadTreeNode cNode = new RadTreeNode("Assigned Users", "ur:" + role.role_name);
                    cNode.ImageUrl = "~/images/tree/folder_open.jpg";
                    cNode.ExpandedImageUrl = "~/images/tree/folder_closed.jpg";
                    node.Nodes.Add(cNode);
                    var userRoles = (from user in ent.users
                                       from userrole in ent.user_roles
                                       where user.user_name == userrole.users.user_name
                                        && userrole.roles.role_name== role.role_name
                                        orderby user.user_name
                                       select new
                                       {
                                           user.user_name,
                                           user.full_name
                                       });
                    foreach (var user in userRoles)
                    {
                        RadTreeNode rNode = new RadTreeNode(user.full_name + " (" + user.user_name + ")",
                            "urr:" + user.user_name);
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

         
        protected void RadTreeView2_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            try
            {
                RadTreeNode clickedNode = e.Node; 

                switch (e.MenuItem.Value)
                {
                    case "New":
                        RadTreeNode newCat = new RadTreeNode("New Role");
                        newCat.Selected = true;
                        newCat.ImageUrl = "~/images/person.jpg";
                        newCat.ExpandedImageUrl = "~/images/person.jpg";
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
                        RenderTree2();
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
                                var ur = ent.user_roles.FirstOrDefault(p => p.roles.role_name == editedRolename &&
                                    p.users.user_name == item.Value);
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
 
        private void Clear()
        {
            this.txtRoleDescription.Text = "";
            this.txtOriginalRolename.Text = "";
            this.txtRolename.Text = "";
            this.txtRolename.Enabled = true;
            this.clMembers.ClearSelection();
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
                    }
                }
                try
                {
                    var node = RadTreeView2.FindNodeByValue("u:" + txtOriginalRolename.Text);
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

        protected void txtRolename_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
