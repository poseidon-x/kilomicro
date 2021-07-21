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

public partial class _Default : System.Web.UI.Page 
{
    coreLogic.coreLoansEntities le;
    List<CachedModule> mods;
    coreSecurityEntities ent = new coreSecurityEntities();

    protected void Page_Load(object sender, EventArgs e)
    {
        //le = new coreLogic.coreLoansEntities();
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
                    return;
                }
            }
            catch (Exception) { }
        }

        Response.Redirect("/dash/home.aspx");
        return;
        //else
        //{

        //}
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
}
