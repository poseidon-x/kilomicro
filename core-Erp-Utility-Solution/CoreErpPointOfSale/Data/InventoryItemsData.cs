using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreErpPointOfSale.ViewModel;
using coreLogic;

namespace coreErpPointOfSale.Data
{
    public class InventoryItemsData
    {
        public static ObservableCollection<InventoryItemsViewModel> SetEnquiriesGrid(IEnumerable<inventoryItem> items)
        {
            ObservableCollection<InventoryItemsViewModel> gridItems = new ObservableCollection<InventoryItemsViewModel>();

            foreach (var record in items)
            {
                var item = new InventoryItemsViewModel(record);
                gridItems.Add(item);
            }

            return gridItems;            
        }

        public static ObservableCollection<InventoryItemsViewModel> ClearEnquiriesGrid()
        {
            ObservableCollection<InventoryItemsViewModel> gridItems = new ObservableCollection<InventoryItemsViewModel>();
            return gridItems;
        }



    }
}
