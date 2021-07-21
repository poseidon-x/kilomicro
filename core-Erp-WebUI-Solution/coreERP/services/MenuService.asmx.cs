using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Telerik.Web.UI;
using coreLogic;

namespace coreERP.services
{
    /// <summary>
    /// Summary description for MenuService
    /// </summary>
    [WebService()]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MenuService : System.Web.Services.WebService
    {
        [WebMethod]
        public object[] GetMenu(object item, object context)
        {
            try
            {
                coreSecurityEntities ent = new coreSecurityEntities();
                List<CachedModule> mods;
                if (Application["mods"] == null)
                {
                    mods = new List<CachedModule>();
                    var ms = ent.modules.ToList();
                    foreach (var m in ms)
                    {
                        mods.Add(new CachedModule
                        {
                            module_id = m.module_id,
                            modification_date = m.modification_date,
                            module_name = m.module_name,
                            last_modifier = m.last_modifier,
                            creation_date = m.creation_date,
                            creator = m.creator,
                            isAuthenticated = true,//Authorizer.IsUserAuthorized(Context.User.Identity.Name, "V", m.module_id),
                            parent_module_id = m.parent_module_id,
                            sort_value = m.sort_value,
                            url = m.url,
                            visible = m.visible
                        });
                    }
                    Application["mods"] = mods;
                }
                else
                {
                    mods = Application["mods"] as List<CachedModule>;
                }
                List<RadMenuItemData> data = new List<RadMenuItemData>();
                IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
                IDictionary<string, object> it = (IDictionary<string, object>)item;
                if (it["Value"] != null)
                {
                    int id = int.Parse(it["Value"].ToString());
                    var list = mods.Where(p => p.parent_module_id == id && p.visible == true).OrderBy(p => p.sort_value).ToList();
                    foreach (var r in list)
                    {
                        var chldModules = (from mod in mods
                                           where mod.parent_module_id == r.module_id
                                                && mod.visible == true
                                           select mod).Count();
                        data.Add(new RadMenuItemData
                        {
                            Text = r.module_name,
                            Value = r.module_id.ToString(),
                            NavigateUrl = ((chldModules > 0 || r.url.Contains("#")) ? "" : System.Web.VirtualPathUtility.ToAbsolute(r.url)),
                            ExpandMode = ((chldModules > 0) ? MenuItemExpandMode.WebService : MenuItemExpandMode.ClientSide)
                        });
                    }
                }

                return data.ToArray();
            }
            catch (Exception x)
            {
                return new RadMenuItemData[] { };
            }
        }
    }
}
