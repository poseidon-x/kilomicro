//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic.Designer
{
    using System;
    using System.Collections.Generic;
    
    public partial class performanceAppraisal
    {
        public performanceAppraisal()
        {
            this.performanceAppraisalScores = new HashSet<performanceAppraisalScore>();
        }
    
        public int performanceAppraisalID { get; set; }
        public int performanceContractID { get; set; }
        public int performanceAppraisalTypeID { get; set; }
        public string staffComments { get; set; }
        public string managerComments { get; set; }
        public string hrComments { get; set; }
        public Nullable<int> managerStaffID { get; set; }
        public System.DateTime appraisalDate { get; set; }
    
        public virtual staff staff { get; set; }
        public virtual performanceAppraisalType performanceAppraisalType { get; set; }
        public virtual performanceContract performanceContract { get; set; }
        public virtual ICollection<performanceAppraisalScore> performanceAppraisalScores { get; set; }
    }
}
