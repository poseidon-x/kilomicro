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
    
    public partial class assemblyWorkStage
    {
        public int assemblyWorkStageId { get; set; }
        public int assemblyLineId { get; set; }
        public int workStageTypeId { get; set; }
        public string workStageName { get; set; }
        public string workStageCode { get; set; }
        public string activityDescription { get; set; }
        public string creator { get; set; }
        public System.DateTime created { get; set; }
        public string modifier { get; set; }
        public System.DateTime modified { get; set; }
    
        public virtual assemblyLine assemblyLine { get; set; }
        public virtual workStageType workStageType { get; set; }
    }
}