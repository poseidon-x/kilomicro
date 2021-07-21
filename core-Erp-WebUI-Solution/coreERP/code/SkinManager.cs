using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
 
    public class SkinManager
    {
        private RadStyleSheetManager rsm;

        public SkinManager  (RadStyleSheetManager rsm){
            this.rsm=rsm;
        }

        private void RegisterFile(string cssFile)
        { 
            rsm.StyleSheets.Add(new StyleSheetReference{
                Path=cssFile
            });
        }

        public void Register(string theme)
        {
            RegisterFile("/App_Themes/" + theme + "/Ajax." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/AsyncUpload." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/AutoComplet6eBox." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Button." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Calendar." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/CloudUpload." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/ColorPicker." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/ComboBox." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Dock." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/DropDownList." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/DropDownTree." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Editor." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/FileExplorer." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Filter." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/FormDecorator." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Grid." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/ImageEditor." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Input." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/LightBox." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/ListBox." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/ListView." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/MediaPlayer." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Menu." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Notification." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/OrgChart." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/PanelBar." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/PivotGrid." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/ProgressArea." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Rating." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/RibbonBar." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Rotator." + theme + ".css"); 
            RegisterFile("/App_Themes/" + theme + "/Scheduler." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/SchedulerRecurrenceEditor." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/SchedulerReminderDialog." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/SearchBox." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/SiteMap." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Slider." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/SocialShare." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Splitter." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/TabStrip." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/TagCloud." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Tile." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/TileList." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/ToolBar." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/ToolTip." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/TreeList." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/TreeView." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Upload." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Splitter." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Widgets." + theme + ".css");
            RegisterFile("/App_Themes/" + theme + "/Window." + theme + ".css");  
        }
    } 