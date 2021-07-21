using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Inventory
{
    public class InventoryLocationsViewModel
    {
        public long locationId { get; set; }
        public string locationName { get; set; }
        public string locationCode { get; set; }
        public int locationTypeId { get; set; }
        public string locationTypeName { get; set; }
        public string physicalAddress { get; set; }
        public int? cityId { get; set; }        
        public string cityName { get; set; }
        public List<InventoryItemsViewModel> items { get; set; }

        
    }
}
