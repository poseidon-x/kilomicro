using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Inventory
{
    public class InventoryItemDetailsViewModel
    {
        public long inventoryItemDetailId { get; set; }       
        public long inventoryItemId { get; set; }
        public string batchNumber { get; set; }
        public DateTime? mfgDate { get; set; }
        public DateTime? expiryDate { get; set; }
        public double unitCost { get; set; }
        public double quantityOnHand { get; set; }
        public double reservedQuantity { get; set; }
        public string startSerialNumber { get; set; }
        public string endSerialNumber { get; set; }

        
    }
}
