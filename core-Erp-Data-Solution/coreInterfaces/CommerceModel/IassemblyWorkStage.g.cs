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
    
    public interface IassemblyWorkStage
    {
        int assemblyWorkStageId { get; set; }
        int assemblyLineId { get; set; }
        int workStageTypeId { get; set; }
        string workStageName { get; set; }
        string workStageCode { get; set; }
        string activityDescription { get; set; }
        string creator { get; set; }
        System.DateTime created { get; set; }
        string modifier { get; set; }
        System.DateTime modified { get; set; }
    
        IassemblyLine assemblyLine { get; set; }
        IworkStageType workStageType { get; set; }
    } 
}