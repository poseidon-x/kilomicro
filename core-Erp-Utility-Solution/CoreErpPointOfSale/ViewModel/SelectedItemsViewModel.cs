using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreErpPointOfSale.Models;

namespace coreErpPointOfSale.ViewModel
{
    public class SelectedItemsViewModel
    {
        public static ObservableCollection<SalesItems> SetUpSalesGrid()
        {
            ObservableCollection<SalesItems> sales = new ObservableCollection<SalesItems>();
            return sales;
        }

        private ObservableCollection<SalesItems> gridData = new ObservableCollection<SalesItems>();
        public ObservableCollection<SalesItems> GridData
        {
            get { return gridData; }
        }

    }
}
