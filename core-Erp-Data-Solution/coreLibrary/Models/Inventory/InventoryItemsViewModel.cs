using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Inventory
{
    public class InventoryItemsViewModel
    {
        public long inventoryItemId { get; set; }
        public string inventoryItemName { get; set; }
        public string itemNumber { get; set; }
        public string brandName { get; set; }
        public double unitPrice { get; set; }
        public long productId { get; set; }
        public string productName { get; set; }
        public long productCategoryId { get; set; }
        public string productCategoryName { get; set; }
        public long productSubCategoryId { get; set; }
        public string productSubCategoryName { get; set; }
        public long brandId { get; set; }
        public double? safetyStockLevel { get; set; }
        public double? reorderPoint { get; set; }

        public List<InventoryItemDetailsViewModel> detailItems { get; set; }

        
    }
}
