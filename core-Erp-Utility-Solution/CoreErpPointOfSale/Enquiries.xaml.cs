using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using coreErpPointOfSale.Data;
using coreErpPointOfSale.ViewModel;
using coreLogic;
using Newtonsoft.Json;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;

namespace CoreErpPointOfSale
{
    /// <summary>
    /// Interaction logic for Enquiries.xaml
    /// </summary>
    public partial class Enquiries : Window
    {
        private List<inventoryItem> inventoryItemsList;
        private List<product> productList;
        private List<productSubCategory> productSubCategoryList;
        private List<productCategory> productCategoryList;

        public Enquiries()
        {
            InitializeComponent();

            EnquiriesAutoComplete.SelectionChanged += EnquiriesAutoComplete_SelectionChanged;
            radSearchProduct.Checked += radSearchProduct_Checked;
            radSearchProductSubCat.Checked += radSearchProductSubCat_Checked;
            radSearchCategory.Checked += radSearchCategory_Checked;

            EnquiriesDataGrid.AutoGenerateColumns = false;
            EnquiriesAutoComplete.SelectionMode = AutoCompleteSelectionMode.Single;


        }

        void radSearchCategory_Checked(object sender, RoutedEventArgs e)
        {
            //Clear the content of the grid
            EnquiriesDataGrid.ItemsSource = InventoryItemsData.ClearEnquiriesGrid();

            using (var client = new WebClient())
            {
                var productCategoryUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/productCategory/Get";
                var token = mainwindow.token;

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + token;

                byte[] productCategoryResult = client.DownloadData(productCategoryUrl);

                // now use a JSON parser to parse the resulting string back to some CLR object
                productCategoryList = ProductCategoryParse(productCategoryResult);

                //setup AutoComplete
                lblSearchCreteria.Content = "Product Categories";
                EnquiriesAutoComplete.WatermarkContent = "Type a Product Category Name";
                EnquiriesAutoComplete.ItemsSource = productCategoryList;
                EnquiriesAutoComplete.DisplayMemberPath = "productCategoryName";
                EnquiriesAutoComplete.TextSearchPath = "productCategoryName";
                EnquiriesAutoComplete.TextSearchMode = TextSearchMode.Contains;
                EnquiriesAutoComplete.SelectedItem = null;

            }
        }

        void radSearchProductSubCat_Checked(object sender, RoutedEventArgs e)
        {
            //Clear the content of the grid
            EnquiriesDataGrid.ItemsSource = InventoryItemsData.ClearEnquiriesGrid();

            using (var client = new WebClient())
            {
                var productSubCateUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/productSubCategory/Get";
                var token = mainwindow.token;

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + token;

                byte[] productSubCateResult = client.DownloadData(productSubCateUrl);

                // now use a JSON parser to parse the resulting string back to some CLR object
                productSubCategoryList = ProductSubCatParse(productSubCateResult);

                //setup AutoComplete
                lblSearchCreteria.Content = "Product Sub-Categories";
                EnquiriesAutoComplete.WatermarkContent = "Type a Product Sub-Category Name";
                EnquiriesAutoComplete.ItemsSource = productSubCategoryList;
                EnquiriesAutoComplete.DisplayMemberPath = "productSubCategoryName";
                EnquiriesAutoComplete.TextSearchPath = "productSubCategoryName";
                EnquiriesAutoComplete.TextSearchMode = TextSearchMode.Contains;
                EnquiriesAutoComplete.SelectedItem = null;
            }
        }

        void radSearchProduct_Checked(object sender, RoutedEventArgs e)
        {
            //Clear the content of the grid
            EnquiriesDataGrid.ItemsSource = InventoryItemsData.ClearEnquiriesGrid();

            using (var client = new WebClient())
            {
                var productUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/product/Get";
                var token = mainwindow.token;

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + token;

                byte[] productResult = client.DownloadData(productUrl);

                // now use a JSON parser to parse the resulting string back to some CLR object
                productList = ProductParse(productResult);

                //setup AutoComplete
                lblSearchCreteria.Content = "Products";
                EnquiriesAutoComplete.WatermarkContent = "Type a Product Name";
                EnquiriesAutoComplete.ItemsSource = productList;
                EnquiriesAutoComplete.DisplayMemberPath = "productName";
                EnquiriesAutoComplete.TextSearchPath = "productName";
                EnquiriesAutoComplete.TextSearchMode = TextSearchMode.Contains;
                EnquiriesAutoComplete.SelectedItem = null;

            }
        }

        void EnquiriesAutoComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EnquiriesAutoComplete.SelectedItem != null)
            {
                if (radSearchProduct.IsChecked == true)
                {
                    using (var client = new WebClient())
                    {
                        var prod = (product) EnquiriesAutoComplete.SelectedItem;
                        var id = prod.productId;
                        var inventoryItemUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/inventoryItem/GetItemByProduct/" + id;
                        var token = mainwindow.token;

                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        client.Headers[HttpRequestHeader.Accept] = "application/json";
                        client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + token;

                        byte[] inventoryItemResult = client.DownloadData(inventoryItemUrl);

                        // now use a JSON parser to parse the resulting string back to some CLR object
                        inventoryItemsList = InventoryItemParse(inventoryItemResult);

                        //Set grid itemsSource
                        EnquiriesDataGrid.ItemsSource = InventoryItemsData.SetEnquiriesGrid(inventoryItemsList);
                        EnquiriesDataGrid.ColumnWidth = 12;
                        EnquiriesDataGrid.AutoGenerateColumns = false;
                        EnquiriesDataGrid.CanUserDeleteRows = false;
                    }
                }
                else if (radSearchProductSubCat.IsChecked == true)
                {
                    using (var client = new WebClient())
                    {
                        var subCat = (productSubCategory)EnquiriesAutoComplete.SelectedItem;
                        var id = subCat.productSubCategoryId;
                        var inventoryItemUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/inventoryItem/GetItemByProductSubCat/" + id;
                        var token = mainwindow.token;

                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        client.Headers[HttpRequestHeader.Accept] = "application/json";
                        client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + token;

                        byte[] inventoryItemResult = client.DownloadData(inventoryItemUrl);

                        // now use a JSON parser to parse the resulting string back to some CLR object
                        inventoryItemsList = InventoryItemParse(inventoryItemResult);

                        //Set grid itemsSource
                        EnquiriesDataGrid.ItemsSource = InventoryItemsData.SetEnquiriesGrid(inventoryItemsList);
                        EnquiriesDataGrid.ColumnWidth = 12;
                        EnquiriesDataGrid.AutoGenerateColumns = false;
                        EnquiriesDataGrid.CanUserDeleteRows = false;
                    }
                }
                else if (radSearchCategory.IsChecked == true)
                {
                    using (var client = new WebClient())
                    {
                        var category = (productCategory)EnquiriesAutoComplete.SelectedItem;
                        var id = category.productCategoryId;
                        var inventoryItemUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/inventoryItem/GetItemByCategory/" + id;
                        var token = mainwindow.token;

                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        client.Headers[HttpRequestHeader.Accept] = "application/json";
                        client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + token;

                        byte[] inventoryItemResult = client.DownloadData(inventoryItemUrl);

                        // now use a JSON parser to parse the resulting string back to some CLR object
                        inventoryItemsList = InventoryItemParse(inventoryItemResult);

                        //Set grid itemsSource
                        EnquiriesDataGrid.ItemsSource = InventoryItemsData.SetEnquiriesGrid(inventoryItemsList);
                        EnquiriesDataGrid.ColumnWidth = 12;
                        EnquiriesDataGrid.AutoGenerateColumns = false;
                        EnquiriesDataGrid.CanUserDeleteRows = false;
                    }
                }

                
            }
            //EnquiriesAutoComplete.SelectedItem = null;
        }


        public static List<inventoryItem> InventoryItemParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<inventoryItem>>(jsonStr);
        }

        public static List<product> ProductParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<product>>(jsonStr);
        }

        public static List<productSubCategory> ProductSubCatParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<productSubCategory>>(jsonStr);
        }

        public static List<productCategory> ProductCategoryParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<productCategory>>(jsonStr);
        }
        
    }
}
