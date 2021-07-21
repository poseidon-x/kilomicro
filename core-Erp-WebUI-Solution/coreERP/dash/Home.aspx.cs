using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Data.Entity;
using coreLogic;
using System.Linq;
using System.Globalization;

public partial class Home : System.Web.UI.Page 
{
    coreLogic.coreLoansEntities le;
    List<CachedModule> mods;
    coreSecurityEntities ent = new coreSecurityEntities();

    protected void Page_Load(object sender, EventArgs e)
    {
        le = new coreLogic.coreLoansEntities();
        if (!Page.IsPostBack)
        {
            try
            {
                if (Request.Params["skin"] != null)
                {
                    string skin = Request.Params["skin"];
                    using (var ent = new core_dbEntities())
                    {
                        var ip = ent.interfacePreferences.FirstOrDefault(p => p.userName.Trim().ToLower() == User.Identity.Name.Trim().ToLower());
                        if (ip == null)
                        {
                            ip = new interfacePreference
                            {
                                userName = User.Identity.Name.Trim(),
                                skinName = skin
                            };
                            ent.interfacePreferences.Add(ip);
                        }
                        else
                        {
                            ip.skinName = skin;
                        }
                        ent.SaveChanges();
                    }
                    Response.Redirect(Request.UrlReferrer.AbsolutePath);
                }
            }
            catch (Exception x) { }
            using (var ent = new core_dbEntities())
            {
                var group = new Telerik.Web.UI.TileGroup();
                tile1.Groups.Add(group);
                if (Request.Params["menuID"] != null)
                {
                    int menuID = int.Parse(Request.Params["menuID"]);
                    var pm = ent.roleMenu.FirstOrDefault(p => p.roleMenuID == menuID);
                    foreach (var menu in pm.roleMenuItem.OrderBy(p=> p.itemName))
                    {
                        RenderTile(group, menu);
                    }
                    instruction.Visible = false;
                    roleName.InnerText = pm.roleName + " Home Page";
                }
                else
                {
                    foreach (var menu in ent.roleMenu.OrderBy(p=> p.roleName))
                    {
                        RenderTile(group, menu);
                    }
                    roleName.Visible = false;
                }
            }
        }
        else
        {

        }
    }

    private void RenderTile(TileGroup group, roleMenu menu)
    {
        var tileItem = new Telerik.Web.UI.RadImageAndTextTile();
        tileItem.Text = menu.roleName; //tileItem.Title = new TileTitle();
        //tileItem.Title.Text = menu.roleName;
        tileItem.ImageUrl = "/Content/icons/" + menu.iconFile;
        tileItem.Shape = TileShape.Wide;
        tileItem.ImageWidth = 126;
        tileItem.ImageHeight = 116;
        tileItem.Font.Bold = false;
        tileItem.Font.Size = 12;
        SetPeek(tileItem, menu.description, tileItem.ImageUrl);
        if (menu.navigateUrl != null)
        {
            tileItem.NavigateUrl = menu.navigateUrl;
        }
        else
        {
            tileItem.NavigateUrl = "/dash/Home.aspx?menuID=" + menu.roleMenuID.ToString();
        }
        
        group.Tiles.Add(tileItem);
    }

    private void RenderTile(TileGroup group, roleMenuItem menu)
    {
        var tileItem = new Telerik.Web.UI.RadImageAndTextTile();
        tileItem.Text = menu.itemName; //tileItem.Title = new TileTitle();
        //tileItem.Title.Text = menu.roleName;
        tileItem.ImageUrl = "/Content/icons/" + menu.iconFile;
        tileItem.Shape = TileShape.Wide;
        tileItem.ImageWidth = 96;
        tileItem.ImageHeight = 86;
        tileItem.Font.Bold = false;
        tileItem.Font.Size = 12;
        SetPeek(tileItem, menu.description,tileItem.ImageUrl);
        var url = "#";
        if (menu.navigateUrl != null)
        {
            url = menu.navigateUrl;
        }
        else
        {
            using (var ent = new coreSecurityEntities())
            {
                var mod = ent.modules.FirstOrDefault(p => p.module_id == menu.moduleID);
                if (mod != null)
                {
                    url = mod.url;
                }
            }
        }
        tileItem.NavigateUrl = ResolveUrl(url);
        group.Tiles.Add(tileItem);
    }

    protected string UserFullname()
    {
        string userName = HttpContext.Current.User.Identity.Name.ToLower();

        try
        {
            using (var ent = new coreSecurityEntities())
            {
                TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
                var user = ent.users.FirstOrDefault(p => p.user_name.ToLower() == userName);
                userName = txtInfo.ToTitleCase(user?.full_name ?? "");
            }
        }
        catch (Exception) { }
        return userName;
    }

    private void SetPeek(RadImageAndTextTile tile, string text, string iconFile)
    {
        LiteralControl div = new LiteralControl("<div class=\"peekTemplate\"><div style='width:60%;'>" + text + "</div><img src='"+ iconFile + "' style=\"width:40%;max-width:96px;max-height:87px\" /></div>");
        tile.PeekContentContainer.Controls.Add(div); //Add the literal control to the Peek Template Container
        tile.PeekTemplateSettings.Animation =  PeekTemplateAnimation.Slide;
        tile.PeekTemplateSettings.ShowPeekTemplateOnMouseOver = true;
        tile.PeekTemplateSettings.HidePeekTemplateOnMouseOut = true;
        tile.PeekTemplateSettings.ShowInterval = 600000; //in ms
        tile.PeekTemplateSettings.CloseDelay = 5000; //in ms
    }
    private void RenderTree(RadTreeNode pNode, coreLogic.CachedModule module)
    {
        RadTreeNode node = new RadTreeNode(module.module_name, module.url);
        node.Visible = module.visible;
        node.ImageUrl = "~/images/module.jpg";
        pNode.Nodes.Add(node);
        node.Font.Size = FontUnit.Medium;
        node.Font.Name = "Calibri";
        var chldModules = (from mod in mods
                           where mod.parent_module_id == module.module_id
                                && mod.visible == true
                           orderby mod.sort_value, mod.module_name
                           select mod).ToList<coreLogic.CachedModule>();
        if (chldModules.Count > 0)
        {
            node.ImageUrl = "~/images/chart_of_accounts/accountHead.jpg";
            node.ExpandedImageUrl = "~/images/chart_of_accounts/accountSubHead.jpg";
        }
        else
        {
            node.ImageUrl = "~/images/chart_of_accounts/account.jpg";
            node.ExpandedImageUrl = "~/images/chart_of_accounts/account.jpg";
        }
        node.NavigateUrl = module.url;
        foreach (var mod in chldModules)
        {
            RenderTree(node, mod);
        }
    }

    protected void ManageException(Exception x)
    {
    }

    private bool IsOwner(string userName)
    {
        try
        {
            var secEnt = new coreSecurityEntities();
            var userRoles = secEnt.user_roles
                .Include(r=>r.users)
                .Include(w => w.roles)
                .Where(p => p.roles.role_name.Trim().ToLower() == "owner" && p.users.user_name.Trim().ToLower()==userName).ToList();
            if(userRoles !=null && userRoles.Count > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    protected string DisplayUserBranchName()
    {
        string userName = HttpContext.Current?.User?.Identity?.Name?.ToLower();
        var branchName = "";
        try
        {
            var isOwner = IsOwner(userName);
            if (isOwner)
            {
                branchName= "System Owner";
            }
            else
            {
                var le = new coreLoansEntities();
                var staffBranchId = le?.staffs?.FirstOrDefault(e => e.userName.ToLower() == userName)?.branchID;
                if (staffBranchId != null)
                {
                    TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
                    branchName = le.branches.FirstOrDefault(r => r.branchID == staffBranchId)?.branchName;
                    if (!string.IsNullOrWhiteSpace(branchName))
                        branchName = txtInfo.ToTitleCase(branchName);
                }
            }            
            return branchName;
        }
        catch (Exception) { }
        return branchName;
    }
}
