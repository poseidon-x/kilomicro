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
    
    public interface Ilocation
    {
        int locationId { get; set; }
        string locationCode { get; set; }
        string locationName { get; set; }
        int locationTypeId { get; set; }
        string physicalAddress { get; set; }
        Nullable<int> cityId { get; set; }
        bool isActive { get; set; }
        Nullable<double> longitude { get; set; }
        Nullable<double> lattitude { get; set; }
    
        ICollection<IinventoryItem> inventoryItems { get; set; }
        ICollection<IinventoryTransfer> inventoryTransfers { get; set; }
        ICollection<IinventoryTransfer> inventoryTransfers1 { get; set; }
        IlocationType locationType { get; set; }
    } 
}