using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models
{
    public class ImageModel
    {
        public int imageId { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string fileName { get; set; }
        public string mimeType { get; set; }
    }
}