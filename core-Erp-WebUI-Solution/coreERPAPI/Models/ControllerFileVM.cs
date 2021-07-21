using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class ControllerFileVM
    {
        public int fileID { get; set; }
        public string fileName { get; set; }
        public List<controllerFileDetail> controllerFileDetails { get; set; }
    }
}