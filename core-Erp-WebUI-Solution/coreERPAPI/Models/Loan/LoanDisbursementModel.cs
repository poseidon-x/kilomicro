using System;
using System.Collections.Generic;
using coreErpApi.Models;
using coreLogic;
using coreErp.Models.Loan;

namespace coreErpApi.Models.Loan
{
    public class LoanDisbursementModel : LoanModel
    {
        public cashierDisbursement loanDisbursement { get; set; }
    }
}