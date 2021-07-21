using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreErpPointOfSale.Models
{
    public class SelectedItems
    {
        public long InventoryItemId { get; set; }
        public string InventoryItemName { get; set; }

        public SelectedItems(inventoryItem item)
        {
            InventoryItemId = item.inventoryItemId;
            InventoryItemName = item.inventoryItemName;
        }

        //public SelectedItems GetSelectedItems()
        //{
            
        //    return InventoryItemName;
        //}
    }

}
