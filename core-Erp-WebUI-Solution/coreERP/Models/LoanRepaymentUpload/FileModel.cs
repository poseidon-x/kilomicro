using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.LoanRepaymentUpload
{
    public class FileModel
    {
        public string fileName { get; set; }
    }

    public class LoanModel
    {
        public int loanId { get; set; }
        public double loanAmount { get; set; }
        public double interstBalance { get; set; }
        public double principalBalance { get; set; }
        public double disbursementDate { get; set; }
        //public double loanAmount { get; set; }
        //public double loanAmount { get; set; }
        //public double loanAmount { get; set; }

    }
}