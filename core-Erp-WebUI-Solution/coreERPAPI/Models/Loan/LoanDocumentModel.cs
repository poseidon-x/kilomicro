using System;
using System.Collections.Generic;
using coreErpApi.Models;
using coreLogic;

namespace coreErpApi.Models.Loan
{
    public class LoanDocumentModel : loanDocument
    {
        public string description { get; set; }
        public string docum { get; set; }
        public string fileName { get; set; }
        public string mimeType { get; set; }
    }
}