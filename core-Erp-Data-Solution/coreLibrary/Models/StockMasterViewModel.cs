using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic.Models.Inventory;

namespace coreLogic.Models
{
    public class StockMasterViewModel
    {
        /*
         * StockTransactionsViewModel properties
         */

        //inventoryitem id
        public long inventoryItemId { get; set; }

        public string inventoryItemName { get; set; }

        public string batchNumber { get; set; }


        public string currencyName { get; set; }

        //openingBalance Quantity
        public double openingQuantityOnHand { get; set; }

        //Transfer Quantity
        public double totalQuantityTransferred { get; set; }


        //inventoryItem unit cost
        public double unitCost { get; set; }

        //inventoryItem unit Price
        public double unitPrice { get; set; }

        public double reservedQuantity { get; set; }

        public string startSerialNumber { get; set; }

        public string endSerialNumber { get; set; }
        
        //Shrinkage Quantity
        public double totalQuantityShrunk { get; set; }


        //inventory currentQuantityOnHand
        public double currentQuantityOnHand { get; set; }

        //narratioon
        public double currentInventoryValue { get; set; }

        public List<InventoryItemDetailsViewModel> detailItems { get; set; }
    }
}
