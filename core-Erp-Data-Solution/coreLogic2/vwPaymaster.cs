//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwPaymaster
    {
        public int payMasterID { get; set; }
        public int payCalendarID { get; set; }
        public double basicSalary { get; set; }
        public double netSalary { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public bool isProcessed { get; set; }
        public bool isPosted { get; set; }
        public string staffNo { get; set; }
        public int staffID { get; set; }
        public string staffName { get; set; }
        public string bankAccountNo { get; set; }
        public string bankName { get; set; }
        public string bankBranchName { get; set; }
        public string ssn { get; set; }
        public string jobTitleName { get; set; }
        public int jobTitleID { get; set; }
        public System.DateTime DOB { get; set; }
        public int staffCategoryID { get; set; }
        public string staffCategoryName { get; set; }
        public string levelName { get; set; }
        public int levelID { get; set; }
        public double totalAllowances { get; set; }
        public double totalDeductions { get; set; }
        public double totalBenefitsInKind { get; set; }
        public double totalTax { get; set; }
        public double totalOneTimeDeductions { get; set; }
        public double totalEmployeePension { get; set; }
        public double totalEmployerPension { get; set; }
        public double totalTaxRelief { get; set; }
        public int staffManagerID { get; set; }
        public double totalLoanDeductions { get; set; }
        public double totalOvertime { get; set; }
        public double overtimeTaxAmount { get; set; }
    }
}