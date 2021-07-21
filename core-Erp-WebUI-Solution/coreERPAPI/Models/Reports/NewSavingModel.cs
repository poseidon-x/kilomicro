using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP.Models.Reports
{
    public class NewSavingModel
    {
        public double AvailableInterestBalance { get; set; }
        public double AvailablePrincipalBalance { get; set; }
        public string SavingNo { get; set; }
        public int SavingId { get; set; }
        public int? StaffId { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public double InterestBalance { get; set; }
        public double PrincipalBalance { get; set; }
        public string Creator { get; set; }
        public DateTime? TransDate { get; set; }
    }
}