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
    
    public partial class performanceScore
    {
        public performanceScore()
        {
            this.performanceAppraisalScores = new HashSet<performanceAppraisalScore>();
            this.performanceContractTargets = new HashSet<performanceContractTarget>();
        }
    
        public int performanceScoreID { get; set; }
        public string performanceScoreName { get; set; }
        public double scoreValue { get; set; }
    
        public virtual ICollection<performanceAppraisalScore> performanceAppraisalScores { get; set; }
        public virtual ICollection<performanceContractTarget> performanceContractTargets { get; set; }
    }
}