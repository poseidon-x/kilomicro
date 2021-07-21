using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreErpApi.Controllers.Models
{
    public class ControllerFileDetailVM
    {
        public bool authorized { get; set; }
        public double balBF { get; set; }
        public string controllerRemarksId { get; set; }
        public virtual controllerFile controllerFile { get; set; }
        public virtual controllerRemark controllerRemark { get; set; }
        public string controllerRepaymentTypeId { get; set; }
        public bool duplicate { get; set; }
        public string employeeName { get; set; }
        public int fileDetailID { get; set; }
        public int fileID { get; set; }
        public string loanNo { get; set; }
        public string managementUnit { get; set; }
        public double monthlyDeduction { get; set; }
        public bool notFound { get; set; }
        public string oldID { get; set; }
        public double? origAmt { get; set; }
        public double overage { get; set; }
        public bool refunded { get; set; }
        public string remarks { get; set; }
        public virtual repaymentSchedule repaymentSchedule { get; set; }
        public int? repaymentScheduleID { get; set; }
        public string staffID { get; set; }
        public bool transferred { get; set; }
    }
}