using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreErpApi.Controllers.Models
{
    public class LoanRestructureViewModel
    {
        //public int loanId { get; set; }
        public loan loan { get; set; }
        public  double loanTotalPrincipal { get; set; }
        public double loanTotalInterest { get; set; }
        public double loanOverallBalanace { get; set; }
        public double totalPrinPaid { get; set; }
        public double totalIntrPaid { get; set; }
        public double totalyPenaltyPayms { get; set; }
        public double penaltyBalance { get; set; }
        public double loanBalance { get; set; }
        public double totalyPenalties { get; set; }
        public double totalPenaltyPayms { get; set; }
        public int interestRate { get; set; }
        public int paymentMode { get; set; }
        public int bank { get; set; }
        public string checkNo { get; set; }

        
        public int additionalTenure { get; set; }
        public double additionalPrincipal { get; set; }
        public bool saveChanges { get; set; }  

   

    }
}