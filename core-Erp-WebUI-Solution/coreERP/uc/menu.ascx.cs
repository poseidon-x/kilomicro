using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using coreLogic;
//using Telerik.Web.Device.Detection;

namespace coreERP.uc
{
    public partial class menu : System.Web.UI.UserControl
    { 
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

       
       /* private void RenderTree(RadMenuItem pNode, coreLogic.CachedModule module)
        {
            RadMenuItem node = new RadMenuItem(module.module_name, module.url);
            node.Visible = module.visible;
            node.Font.Size = FontUnit.Medium;
            pNode.Items.Add(node);
            node.Font.Size = FontUnit.Medium;
            node.Font.Name = "Calibri";
            var chldModules = (from mod in mods
                               where mod.parent_module_id == module.module_id
                                    && mod.visible == true
                               orderby mod.sort_value, mod.module_name
                               select mod).ToList<coreLogic.CachedModule>();
            if (chldModules.Count > 0)
            {
                node.NavigateUrl = "";
            }
            else
            {

            }
            foreach (var mod in chldModules)
            {
                RenderTree(node, mod);
            }
        }
        */
    }
}