using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class MenuData
    {
        public MenuData()
        {
            items = null;
        }
        public string text { get; set; }
        public string cssClass { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
        public bool encoded { get; set; }
        public List<MenuData> items { get; set; }
    }
}