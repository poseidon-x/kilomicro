using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class SideMenuItem
    {
        public int itemId { get; set; }
        public string text { get; set; }
        public bool encoded { get; set; }
        public bool expanded { get; set; }
        public string content { get; set; }
        public string imageUrl { get; set; }
        public int menuType { get; set; }
        public string url { get; set; }
        public string cssClass { get; set; }
        public List<SideMenuItem> items { get; set; }
    }
}