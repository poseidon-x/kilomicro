using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.Image
{
    //public class ClientImageViewModel
    //{
    //    public int clientID { get; set; }
    //    public int clientID { get; set; }

    //    public string photo { get; set; }
    //    public string fileName { get; set; }
    //    public string mimeType { get; set; }
    //}

    public class ClientImageViewModel
    {
        public int clientID { get; set; }
        public string photo { get; set; }
        public string fileName { get; set; }
        public string mimeType { get; set; }
    }

    public class ClientResidentialAddressImageViewModel
    {
        public int ClientResidentialAddressID { get; set; }
        public int imageId { get; set; }
        public string photo { get; set; }
        public string fileName { get; set; }
        public string mimeType { get; set; }
    }
}