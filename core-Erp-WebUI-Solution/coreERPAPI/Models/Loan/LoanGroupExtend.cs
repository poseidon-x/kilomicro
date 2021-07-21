using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.Loan
{
    public class LoanGroupExtend  
    {
        public string BranchName { get; set; }

      //  public virtual client client { get; set; }
        public DateTime created { get; set; }
        public string creator { get; set; }
        public int leaderClientId { get; set; }
        public  List<loanGroupClient> loanGroupClients { get; set; }
        public  loanGroupDay loanGroupDay { get; set; }
        public int loanGroupDayId { get; set; }
        public int loanGroupId { get; set; }
        public string loanGroupName { get; set; }
        public string loanGroupNumber { get; set; }
        public DateTime? modified { get; set; }
        public string modifier { get; set; }
        public int relationsOfficerStaffId { get; set; }
        public  staff staff { get; set; }
    }
}