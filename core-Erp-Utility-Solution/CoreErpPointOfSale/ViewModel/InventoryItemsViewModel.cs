using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreErpPointOfSale.ViewModel
{
    public class InventoryItemsViewModel
    {
        public long InventoryItemId { get; set; }
        public string InventoryItemName { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public string Avaliability { get; set; }


        public InventoryItemsViewModel(int id, string inventoryItemName, double unitPrice)
        {
            InventoryItemId = id;
            InventoryItemName = inventoryItemName;
            UnitPrice = unitPrice;
        }

        public InventoryItemsViewModel()
        {}

        public InventoryItemsViewModel(inventoryItem item)
        {
            InventoryItemId = item.inventoryItemId;
            InventoryItemName = item.inventoryItemName;
            UnitPrice = item.unitPrice;
            Quantity = 0;
            foreach (var record in item.inventoryItemDetails)
            {
                Quantity += record.quantityOnHand;
            }
            Avaliability = Quantity > 0?"YES":"NO";
        }
    }

}
