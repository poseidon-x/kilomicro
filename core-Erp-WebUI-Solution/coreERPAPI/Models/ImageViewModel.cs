using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Models
{
    public class GuarantorPhotoViewModel
    {
        public int guarantorPhotoId { get; set; }
        public string photo { get; set; }
        public string fileName { get; set; }
        public string mimeType { get; set; }
    }

    public class CollateralImageViewModel
    {
        public int collateralImageID { get; set; }
        public string photo { get; set; }
        public string fileName { get; set; }
        public string mimeType { get; set; }
    }
}