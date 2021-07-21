using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using coreErpPointOfSale.Models;
using coreErpPointOfSale.ViewModel;
using coreLogic;
using Newtonsoft.Json;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;

namespace CoreErpPointOfSale
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : Window
    {

        private List<comp_prof> companyProfile;
        private List<inventoryItem> inventoryItemsList;
        private List<cities> citiesList;
        private cities city;
        private salesOrder sales;
        private List<customer> cashCustomer;
        private List<salesOrder> salesList;
        private double Total = 0;

        private static System.Timers.Timer aTimer;

        private ObservableCollection<CheckBox> checkBoxList = new ObservableCollection<CheckBox>();

        public Invoice()
        {
            InitializeComponent();
            this.Loaded += Invoice_Loaded;

            // Intialize the timer with a one hour interval.
            aTimer = new System.Timers.Timer(60000);

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += aTimer_Elapsed;
            aTimer.Enabled = true;

            //disable check boxes
            chkWithHolding.IsEnabled = false;
            chkVatNNhil.IsEnabled = false;

            //Setup Invoice Grid
            InvoiceDataGrid.ItemsSource = SalesItemViewModel.SetUpSalesGrid();
            InvoiceDataGrid.ColumnWidth = 12;
            InvoiceDataGrid.AutoGenerateColumns = false;
            InvoiceDataGrid.CanUserDeleteRows = true;
            InvoiceDataGrid.AddingNewDataItem += InvoiceDataGrid_AddingNewDataItem;

            //Setup Order Number Auto Complete
            autoCompOrderNumber.SelectionMode = AutoCompleteSelectionMode.Single;
            autoCompOrderNumber.SelectionChanged += autoCompOrderNumber_SelectionChanged;

            //Set Invoice Date
            InvoiceDate.Culture = (new System.Globalization.CultureInfo("en-US"));
            InvoiceDate.Culture.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
            InvoiceDate.SelectedDate = DateTime.Now;

            chkWithHolding.Unchecked += chkWithHolding_Unchecked;
            chkWithHolding.Checked += chkWithHolding_Checked;
            chkVatNNhil.Checked += chkVatNNhil_Checked;
            chkVatNNhil.Unchecked += chkVatNNhil_Unchecked;
        }

        void Invoice_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            setupUi();
            this.Show();
        }

        void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SalesItems sales = new SalesItems("", 0.00, 0, 0.00);

            //Post UnPosted Invoices
            using (var client = new WebClient())
            {
                var invoiceUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] +
                                 "/crud/arInvoice/PostCashInvoice";

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + mainwindow.token;

                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sales));
                byte[] postResult = client.UploadData(invoiceUrl, "POST", data);

                var returnedInvoice = InvoiceListParse(postResult);

                if (returnedInvoice.Count > 0)
                {
                    MessageBox.Show("Invoices Posted Successfully");
                }
                else if (returnedInvoice.Count <= 0)
                {
                    MessageBox.Show("No Invoice Avialable for Posting");
                }
            }
        }

        // Get call if  WITHHOLDING checkbox is Checked
        private void chkWithHolding_Checked(object sender, RoutedEventArgs e)
        {
            if (chkWithHolding.IsChecked == true)
            {
                //Add a new row to Grid & call the AddingNewDataItem() method
                InvoiceDataGrid.BeginInsert();
            }
        }

        // Get call if  VAT & NHIL checkbox is Checked
        void chkVatNNhil_Checked(object sender, RoutedEventArgs e)
        {
            if (chkVatNNhil.IsChecked == true)
            {
                //Add a new row to Grid & call the AddingNewDataItem() method
                InvoiceDataGrid.BeginInsert();
            }
        }

        // Get call if  VAT & NHIL checkbox is unchecked
        void chkWithHolding_Unchecked(object sender, RoutedEventArgs e)
        {
            //Delete Withholding Tax
            deleteTaxItem("WITHHOLDING TAX");
            //Check grid to recalculate VAT & NHIL
            InvoiceDataGrid.BeginInsert();
        }

        void chkVatNNhil_Unchecked(object sender, RoutedEventArgs e)
        {
            //Delete VAT & NHIL Tax
            deleteTaxItem("VAT & NHIL");
        }

        void InvoiceDataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            int noOfCheckedCheckBoxes = CountCheckedCheckBoxes();
            double invoiceTotal = 0;


            //Get total Amount of Invoice
            switch (noOfCheckedCheckBoxes)
            {
                case 0:
                    foreach (var item in InvoiceDataGrid.Items)
                    {
                        SalesItems curItem = (SalesItems)item;
                        invoiceTotal += curItem.totalAmount;
                    }
                    break;
                case 1:
                    for (var i = 0; i <= InvoiceDataGrid.Items.Count - noOfCheckedCheckBoxes; i++)
                    {
                        SalesItems curItem = (SalesItems)InvoiceDataGrid.Items[i];
                        invoiceTotal += curItem.totalAmount;
                    }
                    break;
                case 2:
                    for (var i = 0; i <= InvoiceDataGrid.Items.Count - noOfCheckedCheckBoxes; i++)
                    {
                        SalesItems curItem = (SalesItems)InvoiceDataGrid.Items[i];
                        invoiceTotal += curItem.totalAmount;
                    }
                    break;
            }

            var withholdingAmount = invoiceTotal * companyProfile[0].withh_rate;

            if (chkWithHolding.IsChecked == true)
            {
                if (!taxExist("WITHHOLDING TAX") && chkVatNNhil.IsChecked == false)
                {
                    e.NewObject = new SalesItems("WITHHOLDING TAX", withholdingAmount, 1, withholdingAmount);
                }
                if (!taxExist("WITHHOLDING TAX") && chkVatNNhil.IsChecked == true)
                {
                    foreach (var row in InvoiceDataGrid.Items)
                    {
                        SalesItems curItem = (SalesItems)row;
                        if (curItem.description == "VAT & NHIL")
                        {
                            var amount = (invoiceTotal - withholdingAmount) * (companyProfile[0].nhil_rate + companyProfile[0].vat_rate);
                            curItem.unitPrice = amount;
                            curItem.totalAmount = amount;
                        }
                    }

                    e.NewObject = new SalesItems("WITHHOLDING TAX", withholdingAmount, 1, withholdingAmount);
                }
            }

            if (chkVatNNhil.IsChecked == true)
            {
                if (!taxExist("VAT & NHIL") && chkWithHolding.IsChecked == true)
                {
                    var VatNNhil = (invoiceTotal - withholdingAmount) *
                                   (companyProfile[0].nhil_rate + companyProfile[0].vat_rate);
                    e.NewObject = new SalesItems("VAT & NHIL", VatNNhil, 1, VatNNhil);
                }
                else if (!taxExist("VAT & NHIL") && chkWithHolding.IsChecked == false)
                {
                    var VatNNhil = invoiceTotal * (companyProfile[0].nhil_rate + companyProfile[0].vat_rate);
                    e.NewObject = new SalesItems("VAT & NHIL", VatNNhil, 1, VatNNhil);
                }
                else if (chkWithHolding.IsChecked == false && (taxExist("VAT & NHIL")))
                {
                    foreach (var row in InvoiceDataGrid.Items)
                    {
                        SalesItems curItem = (SalesItems)row;
                        if (curItem.description == "VAT & NHIL")
                        {
                            var amount = invoiceTotal * (companyProfile[0].nhil_rate + companyProfile[0].vat_rate);
                            curItem.unitPrice = amount;
                            curItem.totalAmount = amount;
                        }
                    }
                }
            }

            Total = invoiceTotal;
            lblItemCount.Content = (InvoiceDataGrid.Items.Count + 1) - noOfCheckedCheckBoxes;
        }

        void autoCompOrderNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (autoCompOrderNumber.SelectedItem != null)
            {
                var selectedSale = (salesOrder)autoCompOrderNumber.SelectedItem;
                InvoiceDataGrid.ItemsSource = InvoiceViewModel.SetInvoiceGrid(selectedSale);
                chkWithHolding.IsEnabled = true;
                chkVatNNhil.IsEnabled = true;
                chkWithHolding.IsChecked = false;
                chkVatNNhil.IsChecked = false;
            }
            InvoiceDataGrid.SelectedItem = null;
        }

        private void setupUi()
        {
            using (var client = new WebClient())
            {
                var inventoryItemUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/inventoryItem/Get";
                var companyUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/companyProfile/Get";
                var cityUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/city/Get";
                var customerUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/customer/GetCashCustomer";
                var salesOrderUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/salesOrder/GetCashCustomers";

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + mainwindow.token;

                byte[] inventoryItemResult = client.DownloadData(inventoryItemUrl);
                byte[] companyResult = client.DownloadData(companyUrl);
                byte[] citiesResult = client.DownloadData(cityUrl);
                byte[] customerResult = client.DownloadData(customerUrl);
                byte[] salesOrderResult = client.DownloadData(salesOrderUrl);

                salesList = SalesOrderParse(salesOrderResult);
                citiesList = CityParse(citiesResult);
                companyProfile = CompanyParse(companyResult);
                inventoryItemsList = InventoryItemParse(inventoryItemResult);

                //retrieve company cityName with companyCityId 
                foreach (var record in citiesList)
                {
                    if (record.city_id == companyProfile[0].city_id)
                    {
                        city = record;
                    }
                }

                //setup inventory Item AutoComplete
                autoCompOrderNumber.ItemsSource = salesList;
                autoCompOrderNumber.DisplayMemberPath = "orderNumber";
                autoCompOrderNumber.TextSearchPath = "orderNumber";
                autoCompOrderNumber.TextSearchMode = TextSearchMode.Contains;

                //setup company Details 
                lblCompanyNam.Content = companyProfile[0].comp_name;
                lblAddres.Content = companyProfile[0].addr_line_1;
                lblLocat.Content = city.city_name;
                lblPhon.Content = companyProfile[0].phon_num;
            }
        }


        public static List<inventoryItem> InventoryItemParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<inventoryItem>>(jsonStr);
        }

        public static List<comp_prof> CompanyParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<comp_prof>>(jsonStr);
        }

        public static List<cities> CityParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<cities>>(jsonStr);
        }

        public static List<customer> CustomerParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<customer>>(jsonStr);
        }

        public static List<salesOrder> SalesOrderParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<salesOrder>>(jsonStr);
        }

        public static arInvoice InvoiceParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<arInvoice>(jsonStr);
        }

        public static List<arInvoice> InvoiceListParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<arInvoice>>(jsonStr);
        }



        private int CountCheckedCheckBoxes()
        {
            int count = 0;
            checkBoxList.Clear();
            checkBoxList.Add(chkWithHolding);
            checkBoxList.Add(chkVatNNhil);
            foreach (var record in checkBoxList)
            {
                if (record.IsChecked == true)
                {

                    count++;
                }
            }
            return count;
        }

        private bool taxExist(string taxItem)
        {
            var exist = false;
            foreach (var row in InvoiceDataGrid.Items)
            {
                SalesItems curItem = (SalesItems)row;
                if (curItem.description == taxItem)
                {
                    exist = true;
                }
            }
            return exist;
        }

        private void deleteTaxItem(string taxName)
        {
            for (var i = 0; i < InvoiceDataGrid.Items.Count; i++)
            {
                var item = (SalesItems)InvoiceDataGrid.Items[i];
                if (item.description == taxName)
                {
                    InvoiceDataGrid.Items.Remove(item);
                }
            }
        }

        private void btnSubmitInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (Total == 0)
            {
                foreach (var row in InvoiceDataGrid.Items)
                {
                    SalesItems item = (SalesItems)row;
                    Total += item.totalAmount;
                }
            }
            if (autoCompOrderNumber.SelectedItem != null)
            {
                var currentInvoice = new arInvoice();
                var currentSalesOrder = (salesOrder)autoCompOrderNumber.SelectedItem;

                currentInvoice.salesOrderId = currentSalesOrder.salesOrderId;
                currentInvoice.customerId = currentSalesOrder.customerId;
                currentInvoice.customerName = currentSalesOrder.customerName;
                currentInvoice.invoiceDate = Convert.ToDateTime(InvoiceDate.DateTimeText);
                currentInvoice.totalAmount = Total;
                currentInvoice.balance = 0;
                currentInvoice.paidDate = Convert.ToDateTime(InvoiceDate.DateTimeText);
                currentInvoice.paid = true;
                currentInvoice.isVat = chkVatNNhil.IsChecked == true;
                currentInvoice.isNHIL = chkVatNNhil.IsChecked == true;
                currentInvoice.isWith = chkWithHolding.IsChecked == true;
                currentInvoice.vatRate = companyProfile[0].vat_rate;
                currentInvoice.nhilRate = companyProfile[0].nhil_rate;
                currentInvoice.withRate = companyProfile[0].withh_rate;
                currentInvoice.paymentTermId = 1;
                currentInvoice.invoiceStatusId = 1;
                currentInvoice.currencyId = 1;
                currentInvoice.balanceLocal = 0;
                currentInvoice.totalAmountLocal = Total;
                currentInvoice.buyRate = 0;
                currentInvoice.sellRate = 0;
                currentInvoice.accountId = 0;

                var num = CountCheckedCheckBoxes();
                MessageBox.Show(string.Format("No of Line Items: {0}", (InvoiceDataGrid.Items.Count - num)));

                for (var i = 0; i < (InvoiceDataGrid.Items.Count - num); i++)
                {
                    SalesItems curItem = (SalesItems)InvoiceDataGrid.Items[i];
                    arInvoiceLine line = new arInvoiceLine();

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
                    line.isVat = currentInvoice.isVat;
                    line.isNHIL = currentInvoice.isNHIL;
                    line.isWith = currentInvoice.isWith;
                    line.unitPriceLocal = curItem.unitPrice;
                    line.totalAmountLocal = curItem.totalAmount;
                    line.netAmountLocal = curItem.totalAmount;
                    line.discountAmountLocal = 0;
                    line.discountAccountId = 0;
                    line.vatAccountId = 0;
                    line.nhilAccountId = 0;
                    line.withAccountId = 0;

                    currentInvoice.arInvoiceLines.Add(line);
                }
                MessageBox.Show(string.Format("Invoice Total:" + Total));
                submitInvoice(currentInvoice);
            }
        }


        private void submitInvoice(arInvoice invoice)
        {
            using (var client = new WebClient())
            {
                var invoiceUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/arInvoice/Post";

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + mainwindow.token;

                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(invoice));
                byte[] result = client.UploadData(invoiceUrl, "POST", data);

                // now use a JSON parser to parse the resulting string back to some CLR object
                var returnInvoice = InvoiceParse(result);


                if (returnInvoice != null)
                {
                    //Reset grid
                    InvoiceDataGrid.ItemsSource = SalesItemViewModel.SetUpSalesGrid();
                    lblItemCount.Content = 0;
                    Sales_Receipt receipt = new Sales_Receipt(returnInvoice);
                    receipt.ShowDialog();
                }
                else MessageBox.Show("The selected Order has been Invoiced Already");
            }
        }

    }
}
