using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreERP.Models;
using coreLogic;

namespace coreErpApi.Controllers.Models.Deposit
{
    public class DepositModel : deposit
    {
        public int clientInvestmentReceiptDetailId { get; set; }
        public List<DepositSignitoryModel> depositSignitoriesModel { get; set; }
    }

    public class DepositSignitoryModel 
    {
        public int depositSignatoryID { get; set; }
        public int depositID { get; set; }
        public string signatoryName { get; set; }
        public List<DepositImage> signatures { get; set; }
    }

    public class DepositImage
    {
        public int imageId { get; set; }
        public string image { get; set; }
        public string fileName { get; set; }
        public string mimeType { get; set; }
    }
}