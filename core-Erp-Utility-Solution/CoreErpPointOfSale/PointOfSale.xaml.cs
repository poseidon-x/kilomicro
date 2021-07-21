using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using coreErpPointOfSale.Models;
using coreErpPointOfSale.ViewModel;
using Newtonsoft.Json;
using Telerik.Windows.Controls.GridView;
using coreLogic;
using coreModels.Login;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;

namespace CoreErpPointOfSale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PointOfSale : Window
    {
        private mainwindow main;

        private List<inventoryItem> inventoryItemsList;
        private List<comp_prof> companyProfile;
        private List<cities> citiesList;
        private cities city;
        private salesOrder sales;
        private List<customer> cashCustomer;
        private salesOrder salesList;
        private LoginResponse loginResp;



        public PointOfSale()
        {
            InitializeComponent();

            SalesDataGrid.Items.CollectionChanged += Items_CollectionChanged;
            SalesDataGrid.KeyUp += SalesDataGrid_KeyUp;
            
            //Setup InventoryItems AutoCompleteBox
            ItemsAutoCompleteBox.SelectionMode = AutoCompleteSelectionMode.Single;
            ItemsAutoCompleteBox.SelectionChanged += ItemsAutoCompleteBox_SelectionChanged;

            //Set grid itemsSource
            SalesDataGrid.ItemsSource = SalesItemViewModel.SetUpSalesGrid();
            SalesDataGrid.ColumnWidth = 12;
            SalesDataGrid.AutoGenerateColumns = false;
            SalesDataGrid.CanUserDeleteRows = true;
                        
            //Set Sales Date
            SalesDate.Culture = (new System.Globalization.CultureInfo("en-US"));
            SalesDate.Culture.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
            SalesDate.SelectedDate = DateTime.Now;
            
            //Set Date Input to readonly
            SalesDate.IsReadOnly = true;
            Loaded+=MainWindow_Loaded;
            
        }

        void SalesDataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            var cellInfo = SalesDataGrid.CurrentCell;
            var rowInfo = SalesDataGrid.CurrentItem;
            if (cellInfo.Content.ToString() != "" && cellInfo.Content.ToString() != " ")
            {
                if (cellInfo.Column.UniqueName == "quantity")
                {
                    double qty =
                        ((RadMaskedNumericInput) ((GridViewEditorPresenter) cellInfo.Content).Content).Value.Value;

                    if (rowInfo != null)
                    {
                        SalesItems curItem = (SalesItems) rowInfo;
                        double unitP = curItem.unitPrice;

                        double totAmount = qty*unitP;
                        var totalContent = (TextBlock) ((GridViewCell) cellInfo.ParentRow.Cells[3]).Content;
                        totalContent.Text = totAmount.ToString("#,###.#0");
                        curItem.totalAmount = totAmount;
                    }
                    else MessageBox.Show("selected item is null");

                }
            }
        }
         
        void ItemsAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsAutoCompleteBox.SelectedItem != null)
            {
                //Convert the selected object to type of inventoryItem
                inventoryItem item = (inventoryItem)ItemsAutoCompleteBox.SelectedItem;

                if (ItemExist(item))
                {
                    SalesDataGrid.BeginInsert();
                }
                else
                {
                    MessageBox.Show("The input Item does not exist");
                }
            }
            ItemsAutoCompleteBox.SelectedItem = null;
        }

        public bool ItemExist(inventoryItem selectedItem)
        {
            var exist = false;
            foreach (var record in inventoryItemsList)
            {
                if (record.inventoryItemId == selectedItem.inventoryItemId)
                {
                    exist = true;
                    break;
                }
            }
            return exist;
        }

        protected void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SetupFormControls();
            this.Show();
        }



        private void SetupFormControls()
        {
            using (var client = new WebClient())
            {
                var inventoryItemUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/inventoryItem/Get";
                var companyUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/companyProfile/Get";
                var cityUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/city/Get";
                var customerUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/customer/GetCashCustomer";

                var token = mainwindow.token;

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + token;

                byte[] inventoryItemResult = client.DownloadData(inventoryItemUrl);
                byte[] companyResult = client.DownloadData(companyUrl);
                byte[] citiesResult = client.DownloadData(cityUrl);
                byte[] customerResult = client.DownloadData(customerUrl);

                // now use a JSON parser to parse the resulting string back to some CLR object
                inventoryItemsList = InventoryItemParse(inventoryItemResult);
                companyProfile = CompanyParse(companyResult);
                citiesList = CityParse(citiesResult);
                cashCustomer = CustomerParse(customerResult);

                //retrieve company cityName with companyCityId 
                foreach (var record in citiesList)
                {
                    if (record.city_id == companyProfile[0].city_id)
                    {
                        city = record;
                    }
                }
                
                //setup inventory Item AutoComplete
                ItemsAutoCompleteBox.ItemsSource = inventoryItemsList;
                ItemsAutoCompleteBox.DisplayMemberPath = "inventoryItemName";
                ItemsAutoCompleteBox.TextSearchPath = "inventoryItemName";
                ItemsAutoCompleteBox.TextSearchMode= TextSearchMode.Contains;

                //setup company Details 
                lblCompanyName.Content = companyProfile[0].comp_name;
                lblAddress.Content = companyProfile[0].addr_line_1;
                lblLocation.Content = city.city_name;
                lblPhone.Content = companyProfile[0].phon_num;

                //Setup Customer ComboBox
                CustomersComboBox.ItemsSource = cashCustomer;
                CustomersComboBox.DisplayMemberPath = "customerName";
                CustomersComboBox.SelectedValue = "CASH CUSTOMER";
                CustomersComboBox.SelectedValuePath = "customerName";
            }
        }

        private void radGridView_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            var selected = ItemsAutoCompleteBox.SelectedItem;

            //Convert the selected object to type of inventoryItem
            inventoryItem item = (inventoryItem)selected;
            e.NewObject = new SalesItems(item);

        }

        public static List<inventoryItem> InventoryItemParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<inventoryItem>>(jsonStr);
        }

        public static List<comp_prof> CompanyParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject < List<comp_prof>>(jsonStr);
        }

        public static List<cities> CityParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<cities>>(jsonStr);
        }

        public static List<customer> CustomerParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject <List<customer>>(jsonStr);
        }
        

        void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lblItemsCount.Content = SalesDataGrid.Items.Count.ToString();

            
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            salesOrder currentsale = new salesOrder();
            customer curCustomer = (customer)CustomersComboBox.SelectedItem;
            double salesTotal = 0;

            foreach (var item in SalesDataGrid.Items)
            {
                SalesItems curItem = (SalesItems)item;
                salesTotal += curItem.totalAmount;
            }

            currentsale.customerId = curCustomer.customerId;
            currentsale.customerName = curCustomer.customerName;
            currentsale.salesDate = Convert.ToDateTime(SalesDate.DateTimeText);
            currentsale.totalAmount = salesTotal;
            currentsale.balance = 0;
            currentsale.requiredDate = Convert.ToDateTime(SalesDate.DateTimeText);
            currentsale.shippedDate = Convert.ToDateTime(SalesDate.DateTimeText);
            currentsale.locationId = companyProfile[0].city_id;
            currentsale.salesTypeId = 1;
            currentsale.currencyId = 1;
            currentsale.buyRate = 1;
            currentsale.sellRate = 1;
            currentsale.totalAmountLocal = salesTotal;
            currentsale.balanceLocal = 0;
            currentsale.accountId = curCustomer.glAccountId;
            currentsale.paymentTermId = curCustomer.paymentTermID;

            foreach (var item in SalesDataGrid.Items)
            {
                SalesItems curItem = (SalesItems)item;
                salesOrderline line = new salesOrderline();
            
                foreach (var record in inventoryItemsList)
                {
                    if (record.inventoryItemName == curItem.description)
                    {
                        line.inventoryItemId = record.inventoryItemId;
                        line.accountId = record.accountId;
                        break;
                    }
                }
                line.description = curItem.description;
                line.unitPrice = curItem.unitPrice;
                line.quantity = curItem.quantity;
                line.unitOfMeasurementId = 1;
                line.discountAmount = 0;
                line.discountPercentage = 0;
                line.totalAmount = curItem.totalAmount;
                line.netAmount = curItem.totalAmount;
                line.unitPriceLocal = curItem.unitPrice;
                line.totalAmountLocal = curItem.totalAmount;
                line.netAmountLocal = curItem.totalAmount;
                line.discountAmountLocal = 0;

                currentsale.salesOrderlines.Add(line);
            }

            MessageBox.Show("Sales Total:" +salesTotal);
            submitSale(currentsale);

        }

        private void submitSale(salesOrder sale)
        {
            using (var client = new WebClient())
            {
                var customerUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/salesOrder/Post";

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + mainwindow.token;

                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sale));
                byte[] result = client.UploadData(customerUrl, "POST", data);
                // now use a JSON parser to parse the resulting string back to some CLR object
                salesList = SalesParse(result);
                if (salesList != null)
                {
                    MessageBox.Show("Sales Order Saved successfully, Order Number:"+ salesList.orderNumber);
                }
                //Reset grid
                SalesDataGrid.ItemsSource = SalesItemViewModel.SetUpSalesGrid();
            }
        }


        public static salesOrder SalesParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<salesOrder>(jsonStr);
        }

        public static LoginResponse LoginParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<LoginResponse>(jsonStr);
        }

        private void btnInvoice_Click(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new Invoice();
            invoice.ShowDialog();
        }

        private void btnEnquiries_Click(object sender, RoutedEventArgs e)
        {
            Enquiries enquiries = new Enquiries();
            enquiries.ShowDialog();
        }


    }
}
