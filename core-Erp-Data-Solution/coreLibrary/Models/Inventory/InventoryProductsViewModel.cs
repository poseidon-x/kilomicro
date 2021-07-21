using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Inventory
{
    public class InventoryProductsViewModel
    {
        public long productId { get; set; }
        public string productName { get; set; }
        public string productCode { get; set; }
        public long subCategoryId { get; set; }
        public string subCategoryName { get; set; }
        public long categoryId { get; set; }
        public string categoryName { get; set; }
        public List<InventoryItemsViewModel> items { get; set; }

        
    }
}
