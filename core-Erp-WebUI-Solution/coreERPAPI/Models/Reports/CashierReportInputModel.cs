using System;

namespace coreERP.Models.Reports
{
    public class CashierReportInputModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CashierUsername { get; set; }
        public string FieldAgentName { get; set; }
    }
}