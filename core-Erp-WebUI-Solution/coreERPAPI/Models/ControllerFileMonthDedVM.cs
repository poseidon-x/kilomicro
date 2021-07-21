using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class ControllerFileMonthDedVM
    {
        public int fileID { get; set; }
        public string fileName { get; set; }
        public List<vw_controllerFile_outstandingLoan> controllerFileDetails { get; set; }
    }
}